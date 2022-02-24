using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class Point
	{
		public int mX { get; private set; }
		public int mY { get; private set; }

		public Point()
		{
			Set(0, 0);
		}
		public Point(int X, int Y)
		{
			Set(X, Y);
		}
		public void Set(int X, int Y)
		{
			mX = X;
			mY = Y;
		}
		public override string ToString()
		{
			return ToString();
		}
		public virtual string ToString(string Separator = ",")
		{
			return $"({mX}{Separator}{mY})";
		}
		public static Point FromString(string String, string Separator = ",")
		{
			string[] tmp = String.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
			return new Point(int.Parse(tmp[0]), int.Parse(tmp[1]));
		}
		/// <summary>判斷兩指定線段是否有交點</summary>
		public static bool IsHaveIntersectionPoint(Point Line1Point1, Point Line1Point2, Point Line2Point1, Point Line2Point2)
		{
			/*
			 * 參考資料：https://www.cnblogs.com/sanmubai/p/7306599.html
			 * 
			 * 線段 AB 其端點為 (xa, ya) 與 (xb, yb) ，其直線方程為：
			 * x = xa + lambda * (xb - xa)
			 * y = ya + lambda * (yb - ya)
			 * 0 <= lambda <= 1
			 * 
			 * 線段 CD 其端點為 (xc, yc) 與 (xd, yd) ，其直線方程為：
			 * x = xc + micro * (xd - xc)
			 * y = yc + micro * (yd - yc)
			 * 0 <= micro <= 1
			 * 
			 * 則交點應滿足：
			 * x = xa + lambda * (xb - xa) = xc + micro * (xd - xc)
			 * y = ya + lambda * (yb - ya) = yc + micro * (yd - yc)
			 * 
			 * 可整理成：
			 * (xb - xa) * lambda - (xd - xc) * micro = xc - xa
			 * (yb - ya) * lambda - (yd - yc) * micro = yc - ya
			 * 
			 * 行列式 delta 的算法為：
			 * A = |(xb - xa) -(xd - xc)|
			 * 	   |(yb - ya) -(yd - yc)|
			 * delta = (xb - xa) * (-(yd - yc)) - (-(xd - xc)) * (yb - ya)
			 * 	     = (xb - xa) * (yc - yd)    - (xc - xd)    * (yb - ya)
			 * 
			 * 若其行列式等於零，表示線段 AB 與線段 CD 重合或平行。
			 * 
			 * 若其行列式不等於零，則可求出：
			 * lambda = 1 / delta * det|(xc - xa) -(xd - xc)|
			 * 						   |(yc - ya) -(yd - yc)|
			 * 		  = 1 / delta * ((xc - xa) * (yc - yd) - (xc - xd) * (yc - ya))
			 * mircro = 1 / delta * det|(xb - xa) (xc - xa)|
			 * 						   |(yb - ya) (yc - ya)|
			 * 	      = 1 / delta * ((xb - xa) * (yc - ya) - (xc - xa) * (yb - ya))
			 * 
			 * 需特別注意，僅有當 0 <= lambda <= 1 且 0 <= micro <= 1 時，兩線段才有相交，
			 * 否則，交點在線段的延長線上，仍認為兩線段不相交。
			 * 
			 * 算出 lambda 與 micro 後，可得交點為：
			 * x = xa + lambda * (xb - xa)
			 * y = ya + lambda * (yb - ya)
			 */
			bool result = false;
			double delta = (Line1Point2.mX - Line1Point1.mX) * (Line2Point1.mY - Line2Point2.mY) - (Line2Point1.mX - Line2Point2.mX) * (Line1Point2.mY - Line1Point1.mY);
			if (delta <= double.Epsilon && delta >= -double.Epsilon) //若 delta 為 0
			{
				result = false;
			}
			else
			{
				double lambda = ((Line2Point1.mX - Line1Point1.mX) * (Line2Point1.mY - Line2Point2.mY) - (Line2Point1.mX - Line2Point2.mX) * (Line2Point1.mY - Line1Point1.mY)) / delta;
				if (0 <= lambda && lambda <= 1)
				{
					double micro = ((Line1Point2.mX - Line1Point1.mX) * (Line2Point1.mY - Line1Point1.mY) - (Line2Point1.mX - Line1Point1.mX) * (Line1Point2.mY - Line1Point1.mY)) / delta;
					if (0 <= micro && micro <= 1)
					{
						result = true;
					}
					else
					{
						result = false;
					}
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
	}
}
