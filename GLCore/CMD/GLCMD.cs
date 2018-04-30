using Geometry;
using GLStyle;
using SharpGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ThreadSafety;

namespace GLCore
{
    /// <summary>
    /// GL 物件命令管理器
    /// </summary>
    public static class GLCMD
    {
        /// <summary>
        /// 障礙點樣式名稱，Style.ini 中必須要有這個設定
        /// </summary>
        public const string ObstaclePointsStyleName = "ObstaclePoints";

        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private static object key = new object();

        /// <summary>
        /// 障礙點識別碼
        /// </summary>
        public static int ObstaclePointsID { get; set; } = -1;

        /// <summary>
        /// 目前所選擇的 ID
        /// </summary>
        public static int SelectTargetID { get; private set; } = -1;

        /// <summary>
        /// 序號管理器
        /// </summary>
        public static SerialNumberManager SerialNumber { get; } = new SerialNumberManager();

        /// <summary>
        /// 命令紀錄
        /// </summary>
        private static History CommandHistory { get; } = new History(10);

        /// <summary>
        /// 當下複合物件列表
        /// </summary>
        private static Dictionary<int, IMulti> CurrentMultiObject { get; set; } = new Dictionary<int, IMulti>();

        /// <summary>
        /// 當下單一物件列表
        /// </summary>
        private static Dictionary<int, ISingle> CurrentSingleObject { get; set; } = new Dictionary<int, ISingle>();

        /// <summary>
        /// 背景暫存單一物件列表
        /// </summary>
        private static Dictionary<int, ISingle> DuplicateSingleObject { get; } = new Dictionary<int, ISingle>();

        /// <summary>
        /// 上一筆移動指令
        /// </summary>
        private static string PreMoveCommand { get; set; } = string.Empty;

        #region 擦子、畫筆等工具

        /// <summary>
        /// 擦子
        /// </summary>
        public static ISafty<Eraser> Eraser { get; } = new Safty<Eraser>(new Eraser(nameof(Eraser)));

        /// <summary>
        /// 畫筆
        /// </summary>
        public static ISafty<Pen> Pen { get; } = new Safty<Pen>(new Pen(nameof(Pen)));

        /// <summary>
        /// 插入工具
        /// </summary>
        public static ISafty<Join> Join { get; } = new Safty<Join>(new Join());
        #endregion 擦子、畫筆

        #region 複合物件操作

        /// <summary>
        /// 加入複合面。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int AddMultiArea(string style, IEnumerable<IArea> area)
        {
            lock (key)
            {
                var multi = new MultiArea(style, area);
                int id = SerialNumber.Next();
                CurrentMultiObject.Add(id, multi);
                return id;
            }
        }

        /// <summary>
        /// 加入複合點。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int AddMultiPair(string style, IEnumerable<IPair> pair)
        {
            lock (key)
            {
                var multi = new MultiPair(style, pair);
                int id = SerialNumber.Next();
                CurrentMultiObject.Add(id, multi);
                return id;
            }
        }

        /// <summary>
        /// 加入複合線。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int AddMultiStripLine(string style, IEnumerable<IPair> pair)
        {
            lock (key)
            {
                var multi = new MultiStripLine(style, pair);
                int id = SerialNumber.Next();
                CurrentMultiObject.Add(id, multi);
                return id;
            }
        }

        /// <summary>
        /// 刪除複合物件。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DeleteMulti(int id)
        {
            lock (key)
            {
                if (!CurrentMultiObject.Keys.Contains(id)) return -1;
                CurrentMultiObject.Remove(id);
                return id;
            }
        }

        /// <summary>
        /// 執行緒安全操作複合物件的幾何座標集合，若操作過程中會改變資料，請將 <paramref name="isDataChange"/> 設為 True
        /// </summary>
        public static void SaftyEditMultiGeometry<TGeometry>(int id, bool isDataChange, Action<List<TGeometry>> action) where TGeometry : IGeometry
        {
            lock (key)
            {
                if (!CurrentMultiObject.Keys.Contains(id)) return;
                if (!(CurrentMultiObject[id] is IMulti<TGeometry>)) return;

                (CurrentMultiObject[id] as IMulti<TGeometry>).Geometry.SaftyEdit(isDataChange, list => action(list));
            }
        }

        #endregion 複合物件操作

        #region 載入地圖

        /// <summary>
        /// 載入地圖
        /// </summary>
        public static void LoadMap(string file)
        {
            lock (key)
            {
                ClearAll();
                var lines = File.ReadAllLines(file);

                for (int ii = 0; ii < lines.Length; ii++)
                {
                    switch (lines[ii])
                    {
                        case "Goal List":
                            ii = ReadGoalList(lines, ii);
                            CurrentSingleObject = DuplicateSingleObject.DeepClone();
                            break;

                        case "Obstacle Points":
                            ii = ReadObstaclePoints(lines, ii);
                            break;

                        case "Minimum Position":
                            break;

                        case "Maximum Position":
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 清除所有資料
        /// </summary>
        private static void ClearAll()
        {
            lock (key)
            {
                ObstaclePointsID = -1;
                SelectTargetID = -1;
                CommandHistory.Clear();
                CurrentMultiObject.Clear();
                CurrentSingleObject.Clear();
                DuplicateSingleObject.Clear();
                Eraser.SaftyEdit(true, eraser => eraser.Cancel());
                Pen.SaftyEdit(true, pen => pen.Cancel());
            }
        }

        /// <summary>
        /// <para>讀取目標點列表至  <see cref="DuplicateSingleObject"/>，並回傳結束的資料行數</para>
        /// <para>資料格式如：Goal 2,8421,2264,0,MagneticTracking</para>
        /// </summary>
        private static int ReadGoalList(string[] lines, int begin)
        {
            for (int ii = begin + 1; ii < lines.Length; ii++)
            {
                var para = lines[ii].Split(',');
                if (para.Length == 5 && int.TryParse(para[1], out int x) && int.TryParse(para[2], out int y) && double.TryParse(para[3], out double toward))
                {
                    var goal = new SingleTowardPair(para[4], x, y, toward) { Name = para[0] };
                    DuplicateSingleObject.Add(SerialNumber.Next(), goal);
                    begin = ii;
                }
                else
                {
                    break;
                }
            }
            return begin;
        }

        /// <summary>
        /// <para>讀取障礙點至  <see cref="CurrentMultiObject"/> 及寫入 <see cref="ObstaclePointsID"/>，並回傳結束的資料行數</para>
        /// <para>資料格式如：-12794,3803</para>
        /// </summary>
        private static int ReadObstaclePoints(string[] lines, int begin)
        {
            var data = new List<IPair>();
            for (int ii = begin + 1; ii < lines.Length; ii++)
            {
                var para = lines[ii].Split(',');
                if (para.Length == 2 && int.TryParse(para[0], out int x) && int.TryParse(para[1], out int y))
                {
                    data.Add(new Pair(x, y));
                    begin = ii;
                }
                else
                {
                    break;
                }
            }

            if (data.Count != 0)
            {
                ObstaclePointsID = SerialNumber.Next();
                CurrentMultiObject.Add(ObstaclePointsID, new MultiPair(ObstaclePointsStyleName, data));
            }
            return begin;
        }

        #endregion 載入地圖

        #region 儲存地圖
        /// <summary>
        /// 儲存地圖
        /// </summary>
        public static void SaveMap(string file)
        {
            lock (key)
            {
                List<string> data = new List<string>();
                data.Add("Goal List");
                data.AddRange(GetGoalList());
                data.Add("Obstacle Points");
                data.AddRange(GetObstaclePointsList());

                File.WriteAllLines(file, data);
            }
        }

        /// <summary>
        /// <para>以 *.map 目標點字串格式回傳 <see cref="CurrentMultiObject"/> of <see cref="ObstaclePointsID"/> 中所有點</para>
        /// <para>資料格式如：-12794,3803</para>
        /// </summary>
        private static IEnumerable<string> GetObstaclePointsList()
        {
            return (CurrentMultiObject[ObstaclePointsID] as IMulti<IPair>).Geometry.SaftyEdit(list => ToString(list));
        }

        /// <summary>
        /// 將集合中的元素一一轉為字串
        /// </summary>
        private static IEnumerable<string> ToString<T>(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                yield return item.ToString();
            }
        }

        /// <summary>
        /// <para>以 *.map 目標點字串格式回傳 <see cref="CurrentSingleObject"/> 中所有 <see cref="ITowardPairStyle"/> 的圖示 </para>
        /// <para>資料格式如：Goal 2,8421,2264,0,MagneticTracking</para>
        /// </summary>
        private static IEnumerable<string> GetGoalList()
        {
            foreach (var goal in CurrentSingleObject.Where(o => StyleManager.GetStyleType(o.Value.StyleName) == nameof(ITowardPairStyle)).Select(o => o.Value as ISingleTowardPair))
            {
                yield return $"{goal.Name},{goal.Geometry.Position.X},{goal.Geometry.Position.Y},{goal.Geometry.Toward.Theta},{goal.StyleName}";
            }
        }
        #endregion

        /// <summary>
        /// 執行命令。執行失敗回傳 -1，執行成功則回傳控制對象的 id
        /// <para>加入物件：Add,id,style,x,y,toward...</para>
        /// <para>刪除物件：Delete,id</para>
        /// <para>移動物件：Move,id,n,x,y</para>
        /// <para>修改樣式：ChangeStyle,id,newStyle</para>
        /// <para>重新命名：Rename,id,newName</para>
        /// </summary>
        public static int Do(string cmd)
        {
            lock (key)
            {
                return Do(CurrentSingleObject, cmd, true);
            }
        }

        /// <summary>
        /// 加入面。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleArea(string style, int minX, int minY, int maxX, int maxY)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{minX},{minY},{maxX},{maxY}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入面。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleArea(string style, double minX, double minY, double maxX, double maxY)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{minX},{minY},{maxX},{maxY}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入面。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleArea(string style, IPair min, IPair max)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{min.X},{min.Y},{max.X},{max.Y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入面。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleArea(string style, IArea area)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{area.Min.X},{area.Min.Y},{area.Max.X},{area.Max.Y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入線段。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleLine(string style, int x0, int y0, int x1, int y1)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{x0},{y0},{x1},{y1}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入線段。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleLine(string style, double x0, double y0, double x1, double y1)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{x0},{y0},{x1},{y1}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入線段。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleLine(string style, IPair begin, IPair end)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{begin.X},{begin.Y},{end.X},{end.Y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入線段。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleLine(string style, ILine line)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{line.Begin.X},{line.Begin.Y},{line.End.X},{line.End.Y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入座標點。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSinglePair(string style, int x, int y)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{x},{y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入座標點。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSinglePair(string style, double x, double y)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{x},{y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入座標點。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSinglePair(string style, IPair pair)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{pair.X},{pair.Y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入標示物。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleTowardPair(string style, int x, int y, double toward)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{x},{y},{toward}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入標示物。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleTowardPair(string style, double x, double y, double toward)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{x},{y},{toward}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 加入標示物。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="style"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoAddSingleTowardPair(string style, ITowardPair pos)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Add)},{SerialNumber.Next()},{style},{pos.Position.X},{pos.Position.Y},{pos.Toward.Theta}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 修改樣式。執行失敗回傳 -1，執行成功則回傳控制對象的 id。<paramref name="newStyle"/> 為 *.ini 定義的樣式名稱
        /// </summary>
        public static int DoChangeStyle(int id, string newStyle)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.ChangeStyle)},{id},{newStyle}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 刪除物件。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoDelete(int id)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Delete)},{id}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 移動起點座標。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoMoveBegin(int id, int x, int y)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Move)},{id},{nameof(EMoveType.Begin)},{x},{y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 移動物件。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoMoveCenter(int id, int x, int y)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Move)},{id},{nameof(EMoveType.Center)},{x},{y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 移動終點座標。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoMoveEnd(int id, int x, int y)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Move)},{id},{nameof(EMoveType.End)},{x},{y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 移動最大值座標。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoMoveMax(int id, int x, int y)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Move)},{id},{nameof(EMoveType.Max)},{x},{y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 移動最小值座標。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoMoveMin(int id, int x, int y)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Move)},{id},{nameof(EMoveType.Min)},{x},{y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 移動方向角。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoMoveToward(int id, int x, int y)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Move)},{id},{nameof(EMoveType.Toward)},{x},{y}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 移動方向角。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoMoveToward(int id, double theta)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Move)},{id},{nameof(EMoveType.Toward)},{Math.Cos(theta * Math.PI / 180.0)},{Math.Sin(theta * Math.PI / 180.0)}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 重新命名。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoRename(int id, string newName)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Rename)},{id},{newName}";
                return Do(cmd);
            }
        }

        /// <summary>
        /// 繪圖
        /// </summary>
        public static void Draw(OpenGL gl)
        {
            lock (key)
            {
                // 特殊物件
                Eraser?.SaftyEdit(false, eraser => eraser?.Draw(gl));
                Pen?.SaftyEdit(false, pen => { pen.Draw(gl); });
                Join?.SaftyEdit(false, join => { join.Draw(gl); });

                // 先畫不透明再畫透明
                // 不透明
                foreach (var obj in CurrentMultiObject.Values.Where(o => !o.Transparent))
                {
                    obj.Draw(gl);
                }
                foreach (var obj in CurrentSingleObject.Values.Where(o => !o.Transparent))
                {
                    obj.Draw(gl);
                }

                // 透明
                foreach (var obj in CurrentMultiObject.Values.Where(o => o.Transparent))
                {
                    obj.Draw(gl);
                }
                foreach (var obj in CurrentSingleObject.Values.Where(o => o.Transparent))
                {
                    obj.Draw(gl);
                }

                // 選擇邊界
                if (CurrentSingleObject.Keys.Contains(SelectTargetID))
                {
                    CurrentSingleObject[SelectTargetID].DrawBound(gl);
                }
            }
        }

        /// <summary>
        /// 顯示文字
        /// </summary>
        public static void DrawText(OpenGL gl, Func<IPair, IPair> convert)
        {
            lock (key)
            {
                foreach (var obj in CurrentSingleObject.Values)
                {
                    obj.DrawText(gl, convert);
                }
            }
        }

        /// <summary>
        /// 根據座標獲得物件所有對應的 id
        /// </summary>
        public static IEnumerable<int> GetAllTargetID(IPair pos)
        {
            const float MinAllowableError = 50.0f;

            lock (key)
            {
                foreach (var item in CurrentSingleObject)
                {
                    if (item.Value is ISinglePair)
                    {
                        var obj = item.Value as ISinglePair;
                        if (obj.CanDrag && obj.Geometry.Distance(pos) <= Math.Max(obj.Style.Size, MinAllowableError)) yield return item.Key;
                    }
                    else if (item.Value is ISingleLine)
                    {
                        var obj = item.Value as ISingleLine;
                        if (obj.CanDrag && obj.Geometry.Near(pos, Math.Max(obj.Style.Width, MinAllowableError))) yield return item.Key;
                    }
                    else if (item.Value is ISingleArea)
                    {
                        var obj = item.Value as ISingleArea;
                        if (obj.CanDrag && obj.Geometry.Contain(pos)) yield return item.Key;
                    }
                    else if (item.Value is ISingleTowardPair)
                    {
                        var obj = item.Value as ISingleTowardPair;
                        if (obj.CanDrag && obj.Geometry.Position.Distance(pos) <= Math.Max((obj.Style.Width + obj.Style.Height) / 2, MinAllowableError)) yield return item.Key;
                    }
                }
            }
        }

        /// <summary>
        /// 獲得已做的歷史紀錄
        /// </summary>
        public static IEnumerable<string> GetDoHistory()
        {
            lock (key)
            {
                return CommandHistory.GetDoHistory();
            }
        }

        /// <summary>
        /// 根據 id 取得樣式種類。若 id 不存在則回傳 <see cref="string.Empty"/>
        /// </summary>
        public static string GetStyleType(int id)
        {
            lock (key)
            {
                if (!CurrentSingleObject.Keys.Contains(id)) return string.Empty;

                return StyleManager.GetStyleType(CurrentSingleObject[id].StyleName);
            }
        }

        /// <summary>
        /// 獲得已復原的歷史紀錄
        /// </summary>
        public static IEnumerable<string> GetUndoHistory()
        {
            lock (key)
            {
                return CommandHistory.GetUndoHistory();
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public static void Redo(int step)
        {
            lock (key)
            {
                if (CommandHistory.Backward(step))
                {
                    CurrentSingleObject = DuplicateSingleObject.DeepClone();
                    foreach (var cmd in CommandHistory.GetDoHistory())
                    {
                        Do(CurrentSingleObject, cmd, false);
                    }
                }
            }
        }

        /// <summary>
        /// 選擇物件。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int Select(int id)
        {
            lock (key)
            {
                // 儲存移動指令
                if (!string.IsNullOrEmpty(PreMoveCommand))
                {
                    PushHistory(PreMoveCommand);
                    PreMoveCommand = string.Empty;
                }

                if (CurrentSingleObject.Keys.Contains(id))
                {
                    SelectTargetID = id;
                    return id;
                }

                SelectTargetID = -1;
                return -1;
            }
        }

        /// <summary>
        /// 復原
        /// </summary>
        public static void Undo(int step)
        {
            lock (key)
            {
                if (CommandHistory.Forward(step))
                {
                    CurrentSingleObject = DuplicateSingleObject.DeepClone();
                    foreach (var cmd in CommandHistory.GetDoHistory())
                    {
                        Do(CurrentSingleObject, cmd, false);
                    }
                }
            }
        }

        /// <summary>
        /// 加入物件。執行失敗回傳 -1
        /// </summary>
        /// <param name="para">id,style,x,y,toward...</param>
        private static int Add(Dictionary<int, ISingle> dic, IEnumerable<string> para)
        {
            string style = para.ElementAt(1);

            lock (key)
            {
                ISingle single = null;
                switch (StyleManager.GetStyleType(style))
                {
                    case nameof(IPairStyle):
                        single = new SinglePair(style, double.Parse(para.ElementAt(2)), double.Parse(para.ElementAt(3)));
                        break;

                    case nameof(ILineStyle):
                        single = new SingleLine(style, double.Parse(para.ElementAt(2)), double.Parse(para.ElementAt(3)), double.Parse(para.ElementAt(4)), double.Parse(para.ElementAt(5)));
                        break;

                    case nameof(IAreaStyle):
                        single = new SingleArea(style, double.Parse(para.ElementAt(2)), double.Parse(para.ElementAt(3)), double.Parse(para.ElementAt(4)), double.Parse(para.ElementAt(5)));
                        break;

                    case nameof(ITowardPairStyle):
                        single = new SingleTowardPair(style, double.Parse(para.ElementAt(2)), double.Parse(para.ElementAt(3)), double.Parse(para.ElementAt(4)));
                        break;
                }

                if (single != null)
                {
                    int num = int.Parse(para.ElementAt(0));
                    dic.Add(num, single);
                    return num;
                }

                return -1;
            }
        }

        /// <summary>
        /// 利用 <see cref="Serializable"/> 方式複製
        /// </summary>
        private static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 根據 id 移除對象
        /// </summary>
        private static int Delete(Dictionary<int, ISingle> dic, int id)
        {
            lock (key)
            {
                if (dic.Keys.Contains(id))
                {
                    dic.Remove(id);
                    return id;
                }

                return -1;
            }
        }

        /// <summary>
        /// 執行命令。執行失敗回傳 -1，執行成功則回傳控制對象的 id
        /// <para>加入物件：Add,id,style,x,y,toward...</para>
        /// <para>刪除物件：Delete,id</para>
        /// <para>移動物件：Move,id,n,x,y</para>
        /// <para>修改樣式：ChangeStyle,id,newStyle</para>
        /// <para>重新命名：Rename,id,newName</para>
        /// </summary>
        private static int Do(Dictionary<int, ISingle> dic, string cmd, bool pushHistory)
        {
            lock (key)
            {
                var para = cmd.Split(',');
                int res = -1;
                switch (para[0])
                {
                    case nameof(ECMDType.Add):
                        res = Add(dic, para.Skip(1));
                        break;

                    case nameof(ECMDType.Delete):
                        res = Delete(dic, int.Parse(para[1]));
                        break;

                    case nameof(ECMDType.Move):
                        res = Move(dic, para.Skip(1));
                        if (res != -1 && pushHistory) PreMoveCommand = cmd;
                        break;

                    case nameof(ECMDType.ChangeStyle):
                        res = SetStyle(dic, para.Skip(1));
                        break;

                    case nameof(ECMDType.Rename):
                        res = Rename(dic, int.Parse(para[1]), para[2]);
                        break;
                }

                // 儲存移動指令
                if (para[0] != nameof(ECMDType.Move) && !string.IsNullOrEmpty(PreMoveCommand))
                {
                    PushHistory(PreMoveCommand);
                    PreMoveCommand = string.Empty;
                }

                if (res == -1 || !pushHistory) return res;
                if (para[0] == nameof(ECMDType.Move)) return res;

                PushHistory(cmd);

                return res;
            }
        }

        /// <summary>
        /// 移動物件。執行失敗回傳 -1
        /// </summary>
        /// <param name="para">id,n,dx,dy</param>
        private static int Move(Dictionary<int, ISingle> dic, IEnumerable<string> para)
        {
            lock (key)
            {
                int id = int.Parse(para.ElementAt(0));

                if (dic.Keys.Contains(id))
                {
                    bool canMove = false;

                    var target = dic[id];
                    canMove = target.Move((EMoveType)Enum.Parse(typeof(EMoveType), para.ElementAt(1)), int.Parse(para.ElementAt(2)), int.Parse(para.ElementAt(3)));

                    if (!canMove) return -1;

                    return id;
                }

                return -1;
            }
        }

        /// <summary>
        /// 加入歷史紀錄
        /// </summary>
        private static void PushHistory(string cmd)
        {
            lock (key)
            {
                var overflow = CommandHistory.Push(cmd);
                if (overflow != string.Empty) Do(DuplicateSingleObject, overflow, false);
            }
        }

        /// <summary>
        /// 根據 id 重新命名物件
        /// </summary>
        private static int Rename(Dictionary<int, ISingle> dic, int id, string newName)
        {
            lock (key)
            {
                if (dic.Keys.Contains(id))
                {
                    var target = dic[id];
                    target.Name = newName;

                    return id;
                }

                return -1;
            }
        }

        /// <summary>
        /// 修改樣式。執行失敗回傳 -1
        /// </summary>
        /// <param name="para">id,newStyle</param>
        private static int SetStyle(Dictionary<int, ISingle> dic, IEnumerable<string> para)
        {
            lock (key)
            {
                int id = int.Parse(para.ElementAt(0));

                if (dic.Keys.Contains(id))
                {
                    var target = dic[id];
                    target.StyleName = para.ElementAt(1);

                    return id;
                }

                return -1;
            }
        }
    }
}