using System;
using System.Collections.Generic;

namespace Geometry
{
    /// <summary>
    /// 標準操作
    /// </summary>
    public static class StandardOperate
    {
        /// <summary>
        /// 加法
        /// </summary>
        public static IPair Add(this IPair lhs, IPair rhs)
        {
            return new Pair(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        /// <summary>
        /// 加法
        /// </summary>
        public static IPair Add(this IPair lhs, int x, int y)
        {
            return new Pair(lhs.X + x, lhs.Y + y);
        }

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
        /// 沿著(0,0)旋轉
        /// </summary>
        public static IPair Rotate(this IPair lhs, IAngle angle)
        {
            double theta = angle.Theta * Math.PI / 180.0;
            double x = lhs.X * Math.Cos(theta) + lhs.Y * Math.Sin(theta);
            double y = -lhs.X * Math.Sin(theta) + lhs.Y * Math.Cos(theta);
            return new Pair(x, y);
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

        /// <summary>
        /// 將線段轉為點集合
        /// </summary>
        public static IEnumerable<IPair> ToPairs(this ILine line, int delta = 10)
        {
            List<IPair> res = new List<IPair>();
            int dx = line.End.X - line.Begin.X;
            int dy = line.End.Y - line.Begin.Y;
            int sx = Math.Sign(dx) * delta;
            int sy = Math.Sign(dy) * delta;
            if (dx == 0 && dy == 0) return res;
            if (Math.Abs(dx) >= Math.Abs(dy))
            {
                for (int tx = 0; Math.Abs(tx) < Math.Abs(dx + sx); tx += sx)
                {
                    int ty = (int)(((double)tx) / dx * dy);
                    res.Add(line.Begin.Add(tx, ty));
                }
                return res;
            }
            else
            {
                for (int ty = 0; Math.Abs(ty) < Math.Abs(dy + sy); ty += sy)
                {
                    int tx = (int)(((double)ty) / dy * dx);
                    res.Add(line.Begin.Add(tx, ty));
                }
                return res;
            }
        }
    }
}