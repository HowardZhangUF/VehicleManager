using System;

namespace Geometry
{
    /// <summary>
    /// 標準操作
    /// </summary>
    public static class StandardOperate
    {
        /// <summary>
        /// 圖形中心點
        /// </summary>
        public static IPair Center(this IArea area)
        {
            return new Pair((area.Max.X + area.Min.X) / 2, (area.Max.Y + area.Min.Y) / 2);
        }

        /// <summary>
        /// 圖形中心點
        /// </summary>
        public static IPair Center(this ILine area)
        {
            return new Pair((area.Begin.X + area.End.X) / 2, (area.Begin.Y + area.End.Y) / 2);
        }

        /// <summary>
        /// 是否包含點
        /// </summary>
        public static bool Contain(this IArea area, IPair pair)
        {
            return area.Min.X <= pair.X && area.Max.X >= pair.X && area.Min.Y <= pair.Y && area.Max.Y >= pair.Y;
        }

        /// <summary>
        /// 兩點距離
        /// </summary>
        public static double Distance(this IPair lhs, IPair rhs)
        {
            return Math.Sqrt((lhs.X - rhs.X) * (lhs.X - rhs.X) + (lhs.Y - rhs.Y) * (lhs.Y - rhs.Y));
        }

        /// <summary>
        /// 兩點距離平方
        /// </summary>
        public static double Distance2(this IPair lhs, IPair rhs)
        {
            return (lhs.X - rhs.X) * (lhs.X - rhs.X) + (lhs.Y - rhs.Y) * (lhs.Y - rhs.Y);
        }

        /// <summary>
        /// 與原點距離
        /// </summary>
        public static double DistanceToZero(this IPair lhs)
        {
            return Math.Sqrt(lhs.X * lhs.X + lhs.Y * lhs.Y);
        }

        /// <summary>
        /// 與原點距離平方
        /// </summary>
        public static double DistanceToZero2(this IPair lhs)
        {
            return lhs.X * lhs.X + lhs.Y * lhs.Y;
        }

        /// <summary>
        /// 內積
        /// </summary>
        public static int Dot(this IPair lhs, IPair rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        /// <summary>
        /// 線段長度
        /// </summary>
        public static double Length(this ILine line)
        {
            return Math.Sqrt((line.End.X - line.Begin.X) * (line.End.X - line.Begin.X) + (line.End.Y - line.Begin.Y) * (line.End.Y - line.Begin.Y));
        }

        /// <summary>
        /// 點跟線是否靠近
        /// </summary>
        public static bool Near(this ILine line, IPair pair, double delta)
        {
            double lineLength = line.Length();
            double pairBeginLength = pair.Distance(line.Begin);
            double pairEndLength = pair.Distance(line.End);

            return delta + lineLength >= pairBeginLength + pairEndLength;
        }

        /// <summary>
        /// 將角度正規劃在 [0,360) 區間
        /// </summary>
        public static double Normalization(double angle)
        {
            double thetaTmp = angle % 360;
            if (thetaTmp < 0)
                angle = thetaTmp + 360;
            else
                angle = thetaTmp;
            return angle;
        }

        /// <summary>
        /// 重設座標，並自動依照座標大小分配給 Min/Max
        /// </summary>
        public static void Set(this IArea area, IPair p0, IPair p1)
        {
            area.Set(p0.X, p0.Y, p1.X, p1.Y);
        }

        /// <summary>
        /// 重設座標，並自動依照座標大小分配給 Min/Max
        /// </summary>
        public static void Set(this IArea area, int x0, int y0, int x1, int y1)
        {
            area.Min.X = Math.Min(x0, x1);
            area.Min.Y = Math.Min(y0, y1);
            area.Max.X = Math.Max(x0, x1);
            area.Max.Y = Math.Max(y0, y1);
        }

        /// <summary>
        /// 減法
        /// </summary>
        public static IPair Subtraction(this IPair lhs, IPair rhs)
        {
            return new Pair(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }
    }
}