using System;

namespace Geometry
{
    /// <summary>
    /// 具有方向的點
    /// </summary>
    [Serializable]
    public class TowardPair : ITowardPair
    {
        private Pair mPosition = new Pair();

        private Angle mToward = new Angle();

        /// <summary>
        /// 建構 (0,0,0) 座標點
        /// </summary>
        public TowardPair()
        {
        }

        /// <summary>
        /// 建構具有方向的座標點
        /// </summary>
        public TowardPair(int x, int y, double toward)
        {
            Position.X = x;
            Position.Y = y;
            Toward.Theta = toward;
        }

        /// <summary>
        /// 建構具有方向的座標點
        /// </summary>
        public TowardPair(double x, double y, double toward) : this((int)x, (int)y, toward)
        {
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public TowardPair(ITowardPair towardPair) : this(towardPair.Position.X, towardPair.Position.Y, towardPair.Toward.Theta)
        {
        }

        /// <summary>
        /// 建構具有方向的座標點
        /// </summary>
        public TowardPair(IPair position, double toward) : this(position.X, position.Y, toward)
        {
        }

        /// <summary>
        /// 建構具有方向的座標點
        /// </summary>
        public TowardPair(IPair position, IAngle toward) : this(position.X, position.Y, toward.Theta)
        {
        }

        /// <summary>
        /// 建構具有方向的座標點
        /// </summary>
        public TowardPair(int x, int y, IAngle toward) : this(x, y, toward.Theta)
        {
        }

        /// <summary>
        /// 建構具有方向的座標點
        /// </summary>
        public TowardPair(double x, double y, IAngle toward) : this((int)x, (int)y, toward.Theta)
        {
        }

        /// <summary>
        /// 座標點
        /// </summary>
        public IPair Position { get { return mPosition; } set { mPosition.X = value.X; mPosition.Y = value.Y; } }

        /// <summary>
        /// 方向
        /// </summary>
        public IAngle Toward { get { return mToward; } set { mToward.Theta = value.Theta; } }

        /// <summary>
        /// 比較是否相等
        /// </summary>
        public bool Equals(ITowardPair other)
        {
            return Toward.Equals(other.Toward) && Position.Equals(other.Position);
        }

        /// <summary>
        /// 回傳湊雜碼 Toward^Position
        /// </summary>
        public override int GetHashCode()
        {
            return Toward.GetHashCode() ^ Position.GetHashCode();
        }

        /// <summary>
        /// 回傳座標，例如：-100,100,90.00
        /// </summary>
        public override string ToString()
        {
            return Position.ToString() + "," + Toward.ToString();
        }
    }
}