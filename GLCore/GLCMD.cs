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
    /// 命令種類
    /// </summary>
    public enum ECMDType
    {
        /// <summary>
        /// 移動
        /// </summary>
        Move,

        /// <summary>
        /// 加入
        /// </summary>
        Add,

        /// <summary>
        /// 刪除
        /// </summary>
        Delete,

        /// <summary>
        /// 變更樣式
        /// </summary>
        ChangeStyle,

        /// <summary>
        /// 復原
        /// </summary>
        Undo,

        /// <summary>
        /// 選擇
        /// </summary>
        Select,

        /// <summary>
        /// 重新命名
        /// </summary>
        Rename,
    }

    /// <summary>
    /// 移動方式
    /// </summary>
    public enum EMoveType
    {
        /// <summary>
        /// 不移動
        /// </summary>
        Stop,

        /// <summary>
        /// 移動中心點
        /// </summary>
        Center,

        /// <summary>
        /// 移動最大值座標
        /// </summary>
        Max,

        /// <summary>
        /// 移動最小值座標
        /// </summary>
        Min,

        /// <summary>
        /// 移動起點座標
        /// </summary>
        Begin,

        /// <summary>
        /// 移動終點座標
        /// </summary>
        End,

        /// <summary>
        /// 移動方向角
        /// </summary>
        Toward,
    }

    /// <summary>
    /// GL 物件命令管理器
    /// </summary>
    public static class GLCMD
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private static object key = new object();

        /// <summary>
        /// 目前所選擇的 ID
        /// </summary>
        public static int SelectTargetID { get; private set; }

        /// <summary>
        /// 序號管理器
        /// </summary>
        public static SerialNumberManager SerialNumber { get; } = new SerialNumberManager();

        /// <summary>
        /// 背景暫存
        /// </summary>
        private static Dictionary<int, ISingle> BackupObject { get; } = new Dictionary<int, ISingle>();

        /// <summary>
        /// 當下物件列表
        /// </summary>
        private static Dictionary<int, ISingle> CurrentObject { get; set; } = new Dictionary<int, ISingle>();

        /// <summary>
        /// 歷史紀錄
        /// </summary>
        private static History History { get; } = new History(10);

        /// <summary>
        /// 複合物件列表
        /// </summary>
        private static Dictionary<int, IMulti> MultiObject { get; set; } = new Dictionary<int, IMulti>();

        /// <summary>
        /// 上一筆移動指令
        /// </summary>
        private static string PreMoveCommand { get; set; } = string.Empty;

        #region 擦子、畫筆

        /// <summary>
        /// 擦子大小
        /// </summary>
        public static int EraserSize { get { return Eraser.SaftyEdit(eraser => eraser.Geometry.Max.X - eraser.Geometry.Min.X); } }

        /// <summary>
        /// 畫線中
        /// </summary>
        public static bool PenDrawing { get; private set; }

        /// <summary>
        /// 擦子
        /// </summary>
        private static ISafty<SingleArea> Eraser { get; } = new Safty<SingleArea>(new SingleArea(nameof(Eraser)));

        /// <summary>
        /// 畫筆
        /// </summary>
        private static ISafty<SingleLine> Pen { get; } = new Safty<SingleLine>(new SingleLine(nameof(Pen)));

        /// <summary>
        /// 根據 ID 擦掉障礙點
        /// </summary>
        /// <param name="id">類型為 <see cref="MultiPair"/> 的識別碼</param>
        public static void EraserObstaclePoints(int id)
        {
            Eraser.SaftyEdit(false, eraser =>
            SaftyEditMultiGeometry<IPair>(id, true, list => list.RemoveAll(pair => eraser.Geometry.Contain(pair)))
            );
        }

        /// <summary>
        /// 取消畫筆
        /// </summary>
        public static void PenCancel()
        {
            Pen.SaftyEdit(false, pen => { PenDrawing = false; });
        }

        /// <summary>
        /// 完成畫筆
        /// </summary>
        /// <param name="id">類型為 <see cref="MultiPair"/> 的識別碼</param>
        public static void PenFinish(int id)
        {
            Pen.SaftyEdit(false, pen =>
            {
                if (PenDrawing)
                {
                    PenDrawing = false;
                    SaftyEditMultiGeometry<IPair>(id, true, list =>
                    {
                        list.AddRange(pen.Geometry.ToPairs());
                    });
                }
            });
        }

        /// <summary>
        /// 設定擦子大小及位置
        /// </summary>
        public static void SetEraser(IPair pos, int size)
        {
            Eraser.SaftyEdit(true, eraser => eraser.Geometry.Set(pos.X - size / 2, pos.Y - size / 2, pos.X + size / 2, pos.Y + size / 2));
        }

        /// <summary>
        /// 設定擦子位置
        /// </summary>
        public static void SetEraser(IPair pos)
        {
            Eraser.SaftyEdit(true, eraser => eraser.Move(EMoveType.Center, pos.X, pos.Y));
        }

        /// <summary>
        /// 設定畫筆起點和終點
        /// </summary>
        /// <param name="pos"></param>
        public static void SetPenBeginAndEnd(IPair pos)
        {
            Pen.SaftyEdit(true, pen => { pen.Geometry.Begin = new Pair(pos); pen.Geometry.End = new Pair(pos); PenDrawing = true; });
        }

        /// <summary>
        /// 設定畫筆終點
        /// </summary>
        /// <param name="pos"></param>
        public static void SetPenEnd(IPair pos)
        {
            Pen.SaftyEdit(true, pen => { pen.Geometry.End = new Pair(pos); });
        }

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
                MultiObject.Add(id, multi);
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
                MultiObject.Add(id, multi);
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
                MultiObject.Add(id, multi);
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
                if (!MultiObject.Keys.Contains(id)) return -1;
                MultiObject.Remove(id);
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
                if (!MultiObject.Keys.Contains(id)) return;
                if (!(MultiObject[id] is IMulti<TGeometry>)) return;

                (MultiObject[id] as IMulti<TGeometry>).Geometry.SaftyEdit(isDataChange, list => action(list));
            }
        }

        #endregion 複合物件操作

        /// <summary>
        /// 執行命令。執行失敗回傳 -1，執行成功則回傳控制對象的 id
        /// <para>加入物件：Add,id,style,x,y,toward...</para>
        /// <para>刪除物件：Delete,id</para>
        /// <para>移動物件：Move,id,n,x,y</para>
        /// <para>修改樣式：ChangeStyle,id,newStyle</para>
        /// <para>選擇物件：Select,id(不記錄步驟)</para>
        /// <para>重新命名：Rename,id,newName</para>
        /// </summary>
        public static int Do(string cmd)
        {
            lock (key)
            {
                return Do(CurrentObject, cmd, true);
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
        /// 選擇物件。執行失敗回傳 -1，執行成功則回傳控制對象的 id。
        /// </summary>
        public static int DoSelect(int id)
        {
            lock (key)
            {
                string cmd = $"{nameof(ECMDType.Select)},{id}";
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
                // 先畫不透明再畫透明
                // 不透明
                foreach (var obj in CurrentObject.Values.Where(o => !o.Transparent))
                {
                    obj.Draw(gl);
                }
                foreach (var obj in MultiObject.Values.Where(o => !o.Transparent))
                {
                    obj.Draw(gl);
                }

                // 透明
                foreach (var obj in CurrentObject.Values.Where(o => o.Transparent))
                {
                    obj.Draw(gl);
                }
                foreach (var obj in MultiObject.Values.Where(o => o.Transparent))
                {
                    obj.Draw(gl);
                }

                // 特殊物件
                Eraser?.SaftyEdit(false, eraser => eraser?.Draw(gl));
                Pen?.SaftyEdit(false, pen => { if (PenDrawing) pen.Draw(gl); });

                // 選擇邊界
                if (CurrentObject.Keys.Contains(SelectTargetID))
                {
                    CurrentObject[SelectTargetID].DrawBound(gl);
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
                foreach (var obj in CurrentObject.Values)
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
                foreach (var item in CurrentObject)
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
                return History.GetDoHistory();
            }
        }

        /// <summary>
        /// 根據 id 取得樣式種類。若 id 不存在則回傳 <see cref="string.Empty"/>
        /// </summary>
        public static string GetStyleType(int id)
        {
            lock (key)
            {
                if (!CurrentObject.Keys.Contains(id)) return string.Empty;

                return StyleManager.GetStyleType(CurrentObject[id].StyleName);
            }
        }

        /// <summary>
        /// 獲得已復原的歷史紀錄
        /// </summary>
        public static IEnumerable<string> GetUndoHistory()
        {
            lock (key)
            {
                return History.GetUndoHistory();
            }
        }

        /// <summary>
        /// 復原
        /// </summary>
        public static void Undo(int step)
        {
            lock (key)
            {
                if (History.Forward(step))
                {
                    CurrentObject = BackupObject.DeepClone();
                    foreach (var cmd in History.GetDoHistory())
                    {
                        Do(CurrentObject, cmd, false);
                    }
                }
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public static void Redo(int step)
        {
            lock (key)
            {
                if (History.Backward(step))
                {
                    CurrentObject = BackupObject.DeepClone();
                    foreach (var cmd in History.GetDoHistory())
                    {
                        Do(CurrentObject, cmd, false);
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
        /// <para>選擇物件：Select,id(不記錄步驟)</para>
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

                    case nameof(ECMDType.Select):
                        res = Select(dic, int.Parse(para[1]));
                        break;

                    case nameof(ECMDType.Rename):
                        res = Rename(dic, int.Parse(para[1]), para[2]);
                        break;
                }

                if (para[0] != nameof(ECMDType.Move) && !string.IsNullOrEmpty(PreMoveCommand))
                {
                    PushHistory(PreMoveCommand);
                    PreMoveCommand = string.Empty;
                }

                if (res == -1 || !pushHistory) return res;
                if (para[0] == nameof(ECMDType.Select) || para[0] == nameof(ECMDType.Move)) return res;

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
                var overflow = History.Push(cmd);
                if (overflow != string.Empty) Do(BackupObject, overflow, false);
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
        /// 根據 id 選擇對象
        /// </summary>
        private static int Select(Dictionary<int, ISingle> dic, int id)
        {
            lock (key)
            {
                if (dic.Keys.Contains(id))
                {
                    SelectTargetID = id;
                    return id;
                }

                SelectTargetID = -1;
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

    /// <summary>
    /// FILO 歷史紀錄
    /// </summary>
    public class History
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private object key = new object();

        /// <summary>
        /// 建立歷史紀錄。 <paramref name="maxLine"/> 為最大記錄筆數
        /// </summary>
        public History(int maxLine)
        {
            MaxLine = maxLine;
        }

        /// <summary>
        /// 旗標，從 0 ~ Flag-1 表示已經做的步驟，從 Flag ~ Data.Count-1 表示復原的總數
        /// </summary>
        public int Flag { get; private set; }

        /// <summary>
        /// 紀錄最大筆數
        /// </summary>
        public int MaxLine { get; }

        /// <summary>
        /// 歷史資料
        /// </summary>
        private List<string> Data { get; } = new List<string>();

        /// <summary>
        /// 清除紀錄
        /// </summary>
        public void Clear()
        {
            lock (key)
            {
                Flag = 0;
                Data.Clear();
            }
        }

        /// <summary>
        /// 獲得已做的歷史紀錄
        /// </summary>
        public IEnumerable<string> GetDoHistory()
        {
            lock (key)
            {
                for (int ii = 0; ii < Flag; ii++)
                {
                    yield return Data[ii];
                }
            }
        }

        /// <summary>
        /// 獲得已復原的歷史紀錄
        /// </summary>
        public IEnumerable<string> GetUndoHistory()
        {
            lock (key)
            {
                for (int ii = Flag; ii < Data.Count; ii++)
                {
                    yield return Data[ii];
                }
            }
        }

        /// <summary>
        /// 將 <see cref="Flag"/> 向前移
        /// </summary>
        public bool Forward(int step)
        {
            lock (key)
            {
                if (Flag > 0)
                {
                    Flag = Math.Max(Flag - step, 0);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 將 <see cref="Flag"/> 向後移
        /// </summary>
        public bool Backward(int step)
        {
            lock (key)
            {
                if (Flag < MaxLine)
                {
                    Flag = Math.Min(Flag + step, Data.Count);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 加入一筆紀錄。若紀錄已滿，則回傳第一筆紀錄，否則回傳 <see cref="string.Empty"/>
        /// </summary>
        public string Push(string newData)
        {
            lock (key)
            {
                Data.RemoveRange(Flag, Data.Count - Flag);
                Data.Add(newData);
                if (Data.Count > MaxLine)
                {
                    var overflow = Data[0];
                    Data.RemoveAt(0);
                    Flag = Data.Count;
                    return overflow;
                }
                Flag = Data.Count;
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// 遞增序號管理器
    /// </summary>
    public class SerialNumberManager
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private object key = new object();

        /// <summary>
        /// 編號
        /// </summary>
        private int number = 0;

        /// <summary>
        /// 取得下一個不重複的號碼
        /// </summary>
        public int Next()
        {
            lock (key)
            {
                number++;
                return number;
            }
        }
    }
}