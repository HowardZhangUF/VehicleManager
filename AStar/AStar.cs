using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    /// <summary>
    /// 計算兩點夾角，回傳 [0~360)
    /// </summary>
    public delegate double Direction<T>(T lhs, T rhs);

    /// <summary>
    /// 計算兩點距離
    /// </summary>
    public delegate double Distance<T>(T lhs, T rhs);

    /// <summary>
    /// 給定 <paramref name="center"/> ，回傳邊界範圍
    /// </summary>
    /// <param name="center">中心座標</param>
    /// <param name="min">邊界最小座標</param>
    /// <param name="max">邊界最大座標</param>
    public delegate void GetBound<T>(T center, out T min, out T max);

    /// <summary>
    /// 由使用者決定要如何移動
    /// </summary>
    /// <param name="current">現在座標</param>
    /// <param name="target">目標座標</param>
    /// <param name="direction">移動方向</param>
    public delegate IEnumerable<T> Move<T>(T current, T target);

    /// <summary>
    /// A 星路徑搜尋
    /// </summary>
    public class AStar<T>
    {
        #region Node

        /// <summary>
        /// A 星路徑搜尋開啟/關閉列表資料結構
        /// </summary>
        private class Node
        {
            /// <summary>
            /// 方向角
            /// </summary>
            public double Direction { get; set; } = 0;

            /// <summary>
            /// 總成本
            /// </summary>
            public double F { get { return G + H; } }

            /// <summary>
            /// A 星路徑搜尋走至該點的成本
            /// </summary>
            public double G { get; set; } = 0;

            /// <summary>
            /// 該點到終點的成本
            /// </summary>
            public double H { get; set; } = 0;

            /// <summary>
            /// 父節點
            /// </summary>
            public Node Parent { get; set; } = null;

            /// <summary>
            /// 座標
            /// </summary>
            public T Value { get; set; } = default(T);
        }

        #endregion Node

        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 2D Tree
        /// </summary>
        private readonly KDTree<T> tree;

        /// <summary>
        /// 計算 A星演算法中的 G 值
        /// </summary>
        private double CalculateG(Node open, T next)
        {
            double angle = Math.Abs((open.Direction - Direction(open.Value, next))) % 180.0;
            return 0.7 * open.G + 0.4 * Distance(open.Value, next) + 0.1 * angle;
        }

        /// <summary>
        /// 根據 <see cref="Node.F"/> 回傳列表中成本最小的點。假設至少包含一筆資料
        /// </summary>
        private Node FindMinF(List<Node> list)
        {
            var min = list[0];
            for (int ii = 1; ii < list.Count; ii++)
            {
                if (min.F > list[ii].F) min = list[ii];
            }
            return min;
        }

        /// <summary>
        /// 搜尋路徑，若路徑不存在，則回傳 null
        /// </summary>
        private IEnumerable<T> FindPath(List<Node> openList, T end)
        {
            List<Node> closeList = new List<Node>();

            while (true)
            {
                // 檢查開啟列表是否有資料，若開啟列表沒有資料，則表示找不到路徑
                if (!openList.Any()) return null;

                // 尋找開啟節點
                var open = FindMinF(openList);

                // 將結點移至關閉列表
                openList.Remove(open);
                closeList.Add(open);

                // 取得移動列表，若移動失敗則繼續尋找下一個開啟列表中的節點
                var nextList = Move(open.Value, end);
                if (nextList == null || !nextList.Any()) continue;

                // 檢查移動列表是否可以走
                nextList = nextList.Where(o => IsClear(o));
                nextList = nextList.Where(o => !closeList.Any(close => tree.IsEqual(close.Value, o)));
                if (nextList == null || !nextList.Any()) continue;

                // 檢查移動列表中是否有包含終點
                if (nextList.Any(o => tree.IsEqual(o, end)))
                {
                    // 成功搜尋到路徑!!
                    var path = new List<T>();
                    path.Add(end);
                    while (open != null)
                    {
                        path.Add(open.Value);
                        open = open.Parent;
                    }

                    path.Reverse();
                    return path;
                }
                else
                {
                    // 將移動列表加入開啟列表
                    foreach (var next in nextList)
                    {
                        // 在開啟列表中找找看有沒有一樣的點
                        var same = openList.FirstOrDefault(o => tree.IsEqual(o.Value, next));

                        if (same != null)
                        {
                            // 在開啟列表中找到相同的點，計算 G 值，決定是否要更新 G 值
                            double newG = CalculateG(open, next);
                            same.G = Math.Min(same.G, newG);
                        }
                        else
                        {
                            // 沒有在開啟中找到相同的點，直接把該點加入開啟列表
                            openList.Add(new Node()
                            {
                                Parent = open,
                                H = Distance(next, end),
                                G = CalculateG(open, next),
                                Value = next,
                                Direction = Direction(open.Value, next),
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 以 <paramref name="center"/> 為中心的特定範圍內是否可行走 
        /// </summary>
        private bool IsClear(T center)
        {
            T min, max;
            GetBound(center, out min, out max);
            return !tree.IsExist(min, max);
        }

        /// <summary>
        /// <para>A 星路徑搜尋</para>
        /// </summary>
        /// <param name="getBound">給定 <paramref name="center"/> ，回傳邊界範圍</param>
        /// <param name="move">由使用者決定要如何移動</param>
        /// <param name="distance">計算兩點距離</param>
        /// <param name="direction">計算兩點夾角，回傳 [0~360)</param>
        /// <param name="comparerWith">各個座標元素比較，若 lhs 大於 rhs 則回傳 +1；若 lhs 等於 rhs 則回傳 0；若 lhs 小於 rhs 則回傳 -1</param>
        public AStar(GetBound<T> getBound, Move<T> move, Distance<T> distance, Direction<T> direction, params Comparison<T>[] comparerWith)
        {
            tree = new KDTree<T>(comparerWith);
            GetBound = getBound;
            Move = move;
            Distance = distance;
            Direction = direction;
        }

        /// <summary>
        /// 計算兩點夾角，回傳 [0~360)
        /// </summary>
        public Direction<T> Direction { get; }

        /// <summary>
        /// 計算兩點距離
        /// </summary>
        public Distance<T> Distance { get; }

        /// <summary>
        /// 給定 <paramref name="center"/> ，回傳邊界範圍
        /// </summary>
        public GetBound<T> GetBound { get; }

        /// <summary>
        /// 由使用者決定要如何移動
        /// </summary>
        public Move<T> Move { get; }

        /// <summary>
        /// 搜尋路徑，若路徑不存在，則回傳 null
        /// </summary>
        public IEnumerable<T> FindPath(T start, T end)
        {
            lock (key)
            {
                if (!IsClear(start) || !IsClear(end)) return null;
                if (tree.IsEqual(start, end)) return null;

                List<Node> openList = new List<Node>();
                openList.Add(new Node()
                {
                    Value = start,
                    H = Distance(start, end),
                });

                return FindPath(openList, end);
            }
        }

        /// <summary>
        /// 插入，若資料重則不新增。成功插入則回傳 True
        /// </summary>
        public bool Insert(T data)
        {
            lock (key)
            {
                return tree.Insert(data);
            }
        }

        /// <summary>
        /// 插入，若資料重則不新增。回傳成功插入資料個數
        /// </summary>
        public int Insert(IEnumerable<T> collection)
        {
            lock (key)
            {
                return tree.Insert(collection);
            }
        }
    }
}
