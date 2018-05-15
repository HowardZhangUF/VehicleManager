using Algorithm;
using Geometry;
using System;
using System.Collections.Generic;
using System.IO;

namespace PairAStar
{
    /// <summary>
    /// A 星路徑搜尋。<see cref="Pair"/> 與 <see cref="AStar{T}"/> 的結合
    /// </summary>
    public class PairStar
    {
        /// <summary>
        /// 車寬
        /// </summary>
        public const int Width = 1000;

        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 基底類別
        /// </summary>
        private AStar<IPair> @base;

        /// <summary>
        /// 比較 X 座標
        /// </summary>
        private int ComparerWithX(IPair lhs, IPair rhs)
        {
            if (lhs.X > rhs.X) return 1;
            if (lhs.X < rhs.X) return -1;
            return 0;
        }

        /// <summary>
        /// 比較 Y 座標
        /// </summary>
        private int ComparerWithY(IPair lhs, IPair rhs)
        {
            if (lhs.Y > rhs.Y) return 1;
            if (lhs.Y < rhs.Y) return -1;
            return 0;
        }

        /// <summary>
        /// 計算兩點距離
        /// </summary>
        private double Distance(IPair lhs, IPair rhs)
        {
            return lhs.Distance2(rhs);
        }

        /// <summary>
        /// 獲得以 <paramref name="center"/> 為中心的包圍矩形
        /// </summary>
        private void GetBound(IPair center, out IPair min, out IPair max)
        {
            min = center.Subtraction(Width / 2, Width / 2);
            max = center.Add(Width / 2, Width / 2);
        }

        /// <summary>
        /// 求移動後的座標集合
        /// </summary>
        private IEnumerable<IPair> Move(IPair current, IPair target)
        {
            int step = (int)current.Distance(target);
            if (step > Width) step = Width;
            if (step == 0) step = 1;
            return new List<IPair>()
            {
                current.Add(step,0),
                current.Add(-step,0),
                current.Add(0,step),
                current.Add(0,-step),
                current.Add(step,step),
                current.Add(step,-step),
                current.Add(-step,step),
                current.Add(-step,-step),
            };
        }

        /// <summary>
        /// 載入地圖
        /// </summary>
        private IEnumerable<IPair> ReadObstaclePoints(string path)
        {
            var lines = File.ReadAllLines(path);
            int index = 0;

            // 找到標頭
            for (; index < lines.Length; index++)
            {
                if (lines[index] == "Obstacle Points") break;
            }

            index++;
            List<IPair> res = new List<IPair>();
            for (; index < lines.Length; index++)
            {
                var para = lines[index].Split(',');
                int x = 0, y = 0;
                if (para.Length == 2 && int.TryParse(para[0], out x) && int.TryParse(para[1], out y))
                {
                    res.Add(new Pair(x, y));
                }
                else
                {
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// 載入地圖
        /// </summary>
        public void LoadMap(string path)
        {
            lock (key)
            {
                @base = new AStar<IPair>(ComparerWithX, ComparerWithY, GetBound, Move, Distance);
                var points = ReadObstaclePoints(path);
                @base.Insert(points);
            }
        }

        /// <summary>
        /// 搜尋路徑，若路徑不存在，則回傳 null
        /// </summary>
        public IEnumerable<IPair> FindPath(IPair start, IPair end)
        {
            lock (key)
            {
                return @base?.FindPath(start, end);
            }
        }
    }
}
