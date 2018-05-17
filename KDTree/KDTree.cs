using System;
using System.Collections.Generic;

namespace Algorithm
{
    /// <summary>
    /// <para>KD Tree</para>
    /// <para>提供插入、搜尋、範圍搜尋等功能</para>
    /// </summary>
    public class KDTree<T>
    {
        #region Node

        /// <summary>
        /// 2D Tree 資料結構
        /// </summary>
        private class Node
        {
            /// <summary>
            /// 左樹，小於等於 <see cref="Value"/>
            /// </summary>
            public Node Left { get; set; } = null;

            /// <summary>
            /// 右樹，大於 <see cref="Value"/>
            /// </summary>
            public Node Right { get; set; } = null;

            /// <summary>
            /// 值，預設 null
            /// </summary>
            public T Value { get; set; } = default(T);

            /// <summary>
            /// 資料是否存在(當此變數變為 false，表示被刪除)
            /// </summary>
            public bool IsExist { get; set; } = true;
        }

        #endregion Node

        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 根
        /// </summary>
        private readonly Node root = new Node();

        /// <summary>
        /// 依據層數獲得比較函式
        /// </summary>
        private Comparison<T> GetComparer(int layer)
        {
            return ComparerWith[layer % ComparerWith.Length];
        }

        /// <summary>
        /// 插入，若資料重複則不新增。成功插入則回傳 True
        /// </summary>
        private bool Insert(Node root, T data, int layer = 0)
        {
            // 插入 Value
            if (root.Value == null)
            {
                root.Value = data;
                return true;
            }

            // 找到相同資料
            if (IsEqual(data, root.Value))
            {
                if (!root.IsExist)
                {
                    // 將上一個被刪除的資料重新啟用
                    root.IsExist = true;
                    return true;
                }
                return false;
            }

            // 插入左子樹
            if (IsElementLessOrEqual(data, root.Value, layer))
            {
                if (root.Left == null) root.Left = new Node();
                return Insert(root.Left, data, layer + 1);
            }

            // 插入右子樹
            if (root.Right == null) root.Right = new Node();
            return Insert(root.Right, data, layer + 1);
        }

        /// <summary>
        /// 移除指定範圍內的所有資料
        /// </summary>
        public void Remove(T min, T max)
        {
            lock (key)
            {
                Remove(root, min, max);
            }
        }

        /// <summary>
        /// 移除指定範圍內的所有資料
        /// </summary>
        private void Remove(Node root, T min, T max, int layer = 0)
        {
            // 判斷自己是否在邊界內
            if (IsInTheBound(root.Value, min, max))
            {
                root.IsExist = false;
            }

            // 判斷左子樹是否在邊界內
            if (root.Left != null && IsElementMoreOrEqual(root.Value, min, layer))
            {
                Remove(root.Left, min, max, layer + 1);
            }

            // 判斷右子樹是否在邊界內
            if (root.Right != null && IsElementLessOrEqual(root.Value, max, layer))
            {
                Remove(root.Right, min, max, layer + 1);
            }
        }

        /// <summary>
        /// <para>lhs 的 X 座標或 Y 座標是否小於等於 rhs</para>
        /// <para>(根據 <paramref name="layer"/> 決定要比較 X 座標或 Y 座標)</para>
        /// </summary>
        private bool IsElementLessOrEqual(T lhs, T rhs, int layer)
        {
            var comparer = GetComparer(layer);

            return comparer(lhs, rhs) <= 0;
        }

        /// <summary>
        /// <para>lhs 的 X 座標或 Y 座標是否大於等於 rhs</para>
        /// <para>(根據 <paramref name="layer"/> 決定要比較 X 座標或 Y 座標)</para>
        /// </summary>
        private bool IsElementMoreOrEqual(T lhs, T rhs, int layer)
        {
            var comparer = GetComparer(layer);

            return comparer(lhs, rhs) >= 0;
        }

        /// <summary>
        /// 是否存在與 <paramref name="data"/> 座標值相同的資料
        /// </summary>
        private bool IsExist(Node root, T data, int layer = 0)
        {
            // 是否剛好等於自己
            if (IsEqual(data, root.Value)) return root.IsExist;

            // 資料有可能在左子樹
            if (IsElementLessOrEqual(data, root.Value, layer))
            {
                if (root.Left == null) return false;
                return IsExist(root.Left, data, layer + 1);
            }

            // 資料有可能在右子樹
            if (root.Right == null) return false;
            return IsExist(root.Right, data, layer + 1);
        }

        /// <summary>
        /// 是否存在與位於 <paramref name="min"/>, <paramref name="max"/> 邊界內的資料
        /// </summary>
        private bool IsExist(Node root, T min, T max, int layer = 0)
        {
            // 判斷自己是否在邊界內
            if (IsInTheBound(root.Value, min, max) && root.IsExist) return true;

            // 判斷左子樹是否在邊界內
            if (IsElementMoreOrEqual(root.Value, min, layer))
            {
                if (root.Left != null)
                {
                    bool exist = IsExist(root.Left, min, max, layer + 1);
                    if (exist) return true;
                }
            }

            // 判斷右子樹是否在邊界內
            if (IsElementLessOrEqual(root.Value, max, layer))
            {
                if (root.Right != null)
                {
                    bool exist = IsExist(root.Right, min, max, layer + 1);
                    if (exist) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// <paramref name="data"/> 是否在 <paramref name="min"/>, <paramref name="max"/> 邊界內
        /// </summary>
        private bool IsInTheBound(T data, T min, T max)
        {
            foreach (var comparer in ComparerWith)
            {
                if (comparer(data, min) < 0) return false;
                if (comparer(data, max) > 0) return false;
            }

            return true;
        }

        /// <summary>
        /// 將所有 <see cref="Node"/> 組合成特定格是的字串
        /// </summary>
        private string ToString(Node root, Func<T, string> convertToString, int layer = 0)
        {
            string res = string.Empty;

            // 加入右樹
            if (root.Right != null)
            {
                res += ToString(root.Right, convertToString, layer + 1);
            }

            // 加入自己
            if (root.Value != null)
            {
                res += new string('+', layer + 1) + convertToString(root.Value) + "\r\n";
            }

            // 加入左樹
            if (root.Left != null)
            {
                res += ToString(root.Left, convertToString, layer + 1);
            }

            return res;
        }

        /// <summary>
        /// <para>2D Tree</para>
        /// <para>提供插入、刪除、搜尋、範圍搜尋等功能</para>
        /// </summary>
        /// <param name="comparerWith">各個座標元素比較，若 lhs 大於 rhs 則回傳 +1；若 lhs 等於 rhs 則回傳 0；若 lhs 小於 rhs 則回傳 -1</param>
        public KDTree(params Comparison<T>[] comparerWith)
        {
            ComparerWith = comparerWith;
        }

        /// <summary>
        /// 各個座標元素比較，若 lhs 大於 rhs 則回傳 +1；若 lhs 等於 rhs 則回傳 0；若 lhs 小於 rhs 則回傳 -1
        /// </summary>
        private Comparison<T>[] ComparerWith { get; }

        /// <summary>
        /// 插入，若資料重則不新增。成功插入則回傳 True
        /// </summary>
        public bool Insert(T data)
        {
            return Insert(root, data);
        }

        /// <summary>
        /// 插入，若資料重則不新增。回傳成功插入資料個數
        /// </summary>
        public int Insert(IEnumerable<T> collection)
        {
            lock (key)
            {
                int success = 0;
                foreach (var data in collection)
                {
                    if (Insert(data)) ++success;
                }
                return success;
            }
        }

        /// <summary>
        /// 檢查兩個物件值是否相等
        /// </summary>
        public bool IsEqual(T lhs, T rhs)
        {
            lock (key)
            {
                foreach (var comparer in ComparerWith)
                {
                    if (comparer(lhs, rhs) != 0) return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 是否存在與 <paramref name="data"/> 座標值相同的資料
        /// </summary>
        public bool IsExist(T data)
        {
            lock (key)
            {
                return IsExist(root, data);
            }
        }

        /// <summary>
        /// 是否存在與位於 <paramref name="min"/>, <paramref name="max"/> 邊界內的資料
        /// </summary>
        public bool IsExist(T min, T max)
        {
            lock (key)
            {
                return IsExist(root, min, max);
            }
        }

        /// <summary>
        /// 將所有 <see cref="Node"/> 組合成特定格是的字串
        /// </summary>
        public string ToString(Func<T, string> convertToString)
        {
            lock (key)
            {
                return ToString(root, convertToString);
            }
        }
    }
}
