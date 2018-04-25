using System;

namespace Geometry
{
    /// <summary>
    /// 座標
    /// </summary>
    [Serializable]
    public class Pair : IPair
    {
        /// <summary>
        /// 由座標建構點
        /// </summary>
        public Pair(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public Pair(IPair pair) : this(pair.X, pair.Y)
        {
        }

        /// <summary>
        /// 建構 (0,0) 座標
        /// </summary>
        public Pair()
        {
        }

        /// <summary>
        /// 由座標建構點
        /// </summary>
        public Pair(double x, double y) : this((int)x, (int)y)
        {
        }

        /// <summary>
        /// X 座標
        /// </summary>
        public int X { get; set; } = 0;

        /// <summary>
        /// Y 座標
        /// </summary>
        public int Y { get; set; } = 0;

        /// <summary>
        /// 比較是否相等
        /// </summary>
        public bool Equals(IPair other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// 回傳湊雜碼 X^Y
        /// </summary>
        public override int GetHashCode()
        {
            return X ^ Y;
        }

        /// <summary>
        /// 回傳座標，例如：-100,100
        /// </summary>
        public override string ToString()
        {
            return X + "," + Y;
        }
    }
}