using System;

namespace Geometry
{
    /// <summary>
    /// 線段
    /// </summary>
    [Serializable]
    public class Line : ILine
    {
        private Pair mBegin = new Pair();
        private Pair mEnd = new Pair();

        /// <summary>
        /// 由兩點座標建構線段
        /// </summary>
        public Line(int x0, int y0, int x1, int y1)
        {
            mBegin.X = x0;
            mBegin.Y = y0;
            mEnd.X = x1;
            mEnd.Y = y1;
        }

        /// <summary>
        /// 由兩點座標建構線段
        /// </summary>
        public Line(double x0, double y0, double x1, double y1) : this((int)x0, (int)y0, (int)x1, (int)y1)
        {
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public Line(ILine line) : this(line.Begin.X, line.Begin.Y, line.End.X, line.End.Y)
        {
        }

        /// <summary>
        /// 由兩點座標建構線段
        /// </summary>
        public Line(IPair beg, IPair end) : this(beg.X, beg.Y, end.X, end.Y)
        {
        }

        /// <summary>
        /// 建構 (0,0,0,0) 線段
        /// </summary>
        public Line()
        {
        }

        /// <summary>
        /// 起點座標
        /// </summary>
        public IPair Begin { get { return mBegin; } set { mBegin.X = value.X; mBegin.Y = value.Y; } }

        /// <summary>
        /// 終點座標
        /// </summary>
        public IPair End { get { return mEnd; } set { mEnd.X = value.X; mEnd.Y = value.Y; } }

        /// <summary>
        /// 比較是否相等
        /// </summary>
        public bool Equals(ILine other)
        {
            return Begin.Equals(other.Begin) && End.Equals(other.End);
        }

        /// <summary>
        /// 回傳湊雜碼 Begin^End
        /// </summary>
        public override int GetHashCode()
        {
            return Begin.GetHashCode() ^ End.GetHashCode();
        }

        /// <summary>
        /// 回傳座標，例如：-100,100,0,200(前面兩個數值代表起始座標，後面代表終點座標)
        /// </summary>
        public override string ToString()
        {
            return Begin.ToString() + "," + End.ToString();
        }
    }
}