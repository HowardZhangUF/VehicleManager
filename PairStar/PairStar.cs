using Algorithm;
using Geometry;
using MapReader;
using System;
using System.Collections.Generic;

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
            return lhs.Distance(rhs);
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
            double theta = Math.Atan2(target.Y - current.Y, target.X - current.X);
            int step = (int)current.Distance(target);
            if (step > Width / 2) step = Width / 2;
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
                current.Add((int)(step*Math.Cos(theta)),(int)(step*Math.Sin(theta))),
        };
        }

        /// <summary>
        /// 載入地圖
        /// </summary>
        public void LoadMap(string path)
        {
            lock (key)
            {
                @base = new AStar<IPair>(GetBound, Move, Distance, Direction, ComparerWithX, ComparerWithY);

                // 使用讀取器讀取
                var reader = new Reader(path);

                @base.Insert(reader.ObstaclePointsList);
            }
        }

        /// <summary>
        /// 計算兩點夾角，回傳 [0~360)
        /// </summary>
        private double Direction(IPair lhs, IPair rhs)
        {
            return Math.Atan2(rhs.Y - lhs.Y, rhs.X - lhs.X) * 180 / Math.PI;
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

        /// <summary>
        /// 插入，若資料重則不新增。回傳成功插入資料個數
        /// </summary>
        public int Insert(IEnumerable<IPair> data)
        {
            lock (key)
            {
                if (@base == null) @base = new AStar<IPair>(GetBound, Move, Distance, Direction, ComparerWithX, ComparerWithY);
                return @base?.Insert(data) ?? 0;
            }
        }

        /// <summary>
        /// 移除指定範圍內的所有資料
        /// </summary>
        public void Remove(IPair min, IPair max)
        {
            lock (key)
            {
                @base?.Remove(min, max);
            }
        }
    }
}
