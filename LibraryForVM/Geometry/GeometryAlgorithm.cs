using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForVM
{
	public static class GeometryAlgorithm
	{
		#region IPoint2D
		/// <summary>計算向量 (End - Start) 與 X+ 的夾角。範圍為 -180 ~ 180 ，單位為 degree</summary>
		public static double GetAngle(IPoint2D Start, IPoint2D End)
		{
			return Math.Atan2(End.mY - Start.mY, End.mX - Start.mX) / Math.PI * 180.0f;
		}
		public static double GetDistance(IPoint2D Point1, IPoint2D Point2)
		{
			return Math.Sqrt(((Point2.mX - Point1.mX) / 1000.0f) * ((Point2.mX - Point1.mX) / 1000.0f) + ((Point2.mY - Point1.mY) / 1000.0f) * ((Point2.mY - Point1.mY) / 1000.0f)) * 1000.0f;
		}
		/// <summary>計算點集合依序連起來的線段的長度總和</summary>
		public static double GetDistance(IEnumerable<IPoint2D> Points)
		{
			double result = 0;
			if (Points.Count() > 1)
			{
				for (int i = 1; i < Points.Count(); ++i)
				{
					result += GetDistance(Points.ElementAt(i - 1), Points.ElementAt(i));
				}
			}
			return result;
		}
		/// <summary>計算指定矩形與指定線段(由兩點組成)的交點。最多有可能會有兩個交點</summary>
		public static IEnumerable<IPoint2D> GetIntersectionPoint(IRectangle2D Rectangle, IPoint2D Point1, IPoint2D Point2)
		{
			List<IPoint2D> result = null;
			if (Rectangle != null && Point1 != null && Point2 != null)
			{
				result = new List<IPoint2D>();
				// 計算矩形的四個邊與兩點組成的線段的交點
				IPoint2D tmp = null;
				tmp = GetIntersectionPoint(Point1, Point2, new Point2D(Rectangle.mMaxX, Rectangle.mMaxY), new Point2D(Rectangle.mMaxX, Rectangle.mMinY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
				tmp = GetIntersectionPoint(Point1, Point2, new Point2D(Rectangle.mMaxX, Rectangle.mMaxY), new Point2D(Rectangle.mMinX, Rectangle.mMaxY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
				tmp = GetIntersectionPoint(Point1, Point2, new Point2D(Rectangle.mMinX, Rectangle.mMinY), new Point2D(Rectangle.mMaxX, Rectangle.mMinY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
				tmp = GetIntersectionPoint(Point1, Point2, new Point2D(Rectangle.mMinX, Rectangle.mMinY), new Point2D(Rectangle.mMinX, Rectangle.mMaxY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
			}
			return result;
		}
		/// <summary>計算兩線段(由兩點組成)的交點</summary>
		public static IPoint2D GetIntersectionPoint(IPoint2D Line1Point1, IPoint2D Line1Point2, IPoint2D Line2Point1, IPoint2D Line2Point2)
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
			 // 運算過程中加上轉換成 double 的語法以避免計算溢位
			IPoint2D result = null;
			if (Line1Point1 != null && Line1Point2 != null && Line2Point1 != null && Line2Point2 != null)
			{
				double delta = (double)(Line1Point2.mX - Line1Point1.mX) * (double)(Line2Point1.mY - Line2Point2.mY) - (double)(Line2Point1.mX - Line2Point2.mX) * (double)(Line1Point2.mY - Line1Point1.mY);
				if (delta <= double.Epsilon && delta >= -double.Epsilon) //若 delta 為 0
				{
					result = null;
				}
				else
				{
					double lambda = ((double)(Line2Point1.mX - Line1Point1.mX) * (double)(Line2Point1.mY - Line2Point2.mY) - (double)(Line2Point1.mX - Line2Point2.mX) * (double)(Line2Point1.mY - Line1Point1.mY)) / delta;
					if (0 <= lambda && lambda <= 1)
					{
						double micro = ((double)(Line1Point2.mX - Line1Point1.mX) * (double)(Line2Point1.mY - Line1Point1.mY) - (double)(Line2Point1.mX - Line1Point1.mX) * (double)(Line1Point2.mY - Line1Point1.mY)) / delta;
						if (0 <= micro && micro <= 1)
						{
							int x = Line1Point1.mX + (int)(lambda * (Line1Point2.mX - Line1Point1.mX));
							int y = Line1Point1.mY + (int)(lambda * (Line1Point2.mY - Line1Point1.mY));
							result = new Point2D(x, y);
						}
					}
				}
			}
			return result;
		}
		/// <summary>將一條線轉換成點集合(不包含該線的兩端點)</summary>
		public static IEnumerable<IPoint2D> ConvertLineToPoints(IPoint2D Start, IPoint2D End, int Interval)
		{
			List<IPoint2D> result = new List<IPoint2D>();

			int diffX = Math.Abs(Start.mX - End.mX);
			int diffY = Math.Abs(Start.mY - End.mY);
			// 斜率為無限大
			if (diffX == 0)
			{
				if (Start.mY < End.mY)
				{
					for (int i = Start.mY + Interval; i < End.mY; i += Interval)
					{
						result.Add(new Point2D(Start.mX, i));
					}
				}
				else
				{
					for (int i = Start.mY - Interval; i > End.mY; i -= Interval)
					{
						result.Add(new Point2D(Start.mX, i));
					}
				}
			}
			// 斜率為零
			else if (diffY == 0)
			{
				if (Start.mX < End.mX)
				{
					for (int i = Start.mX + Interval; i < End.mX; i += Interval)
					{
						result.Add(new Point2D(i, Start.mY));
					}
				}
				else
				{
					for (int i = Start.mX - Interval; i > End.mX; i -= Interval)
					{
						result.Add(new Point2D(i, Start.mY));
					}
				}
			}
			else
			{
				// y = mx + c
				// x = (y - c) / m
				double m = (double)(Start.mY - End.mY) / (Start.mX - End.mX);
				double c = Start.mY - m * Start.mX;
				double radian = Math.Atan((Start.mY - End.mY) / (Start.mX - End.mX));
				double distance = Math.Abs((Interval * Math.Cos(radian)));
				if (Start.mX < End.mX)
				{
					for (double i = Start.mX + distance; i < End.mX; i += distance)
					{
						double y = Math.Round(m * i + c, 0, MidpointRounding.AwayFromZero);
						result.Add(new Point2D((int)Math.Round(i, 0, MidpointRounding.AwayFromZero), (int)y));
					}
				}
				else
				{
					for (double i = Start.mX - distance; i > End.mX; i -= distance)
					{
						double y = Math.Round(m * i + c, 0, MidpointRounding.AwayFromZero);
						result.Add(new Point2D((int)Math.Round(i, 0, MidpointRounding.AwayFromZero), (int)y));
					}
				}
			}
			return result;
		}
		public static string ConvertToString(IEnumerable<IPoint2D> Points)
		{
			string result = string.Empty;
			result = string.Join(" ", Points.Select((o) => o.ToString()));
			return result;
		}
		#endregion

		#region IVector2D
		public static IVector2D GetVector(int X1, int Y1, int X2, int Y2)
		{
			IVector2D result = null;
			result = new Vector2D(X2 - X1, Y2 - Y1);
			return result;
		}
		public static IVector2D GetNormalizeVector(IVector2D Vector)
		{
			IVector2D result = null;
			result = new Vector2D(Vector.mXComponent / Vector.mMagnitude, Vector.mYComponent / Vector.mMagnitude);
			return result;
		}
		public static double GetDotProduct(IVector2D Vector1, IVector2D Vector2)
		{
			double result = 0;
			if (Vector1 != null && Vector2 != null)
			{
				result = Vector1.mXComponent * Vector2.mXComponent + Vector1.mYComponent * Vector2.mYComponent;
			}
			return result;
		}
		public static double GetAngleOfTwoVector(IVector2D Vector1, IVector2D Vector2)
		{
			double result = -1;
			if (Vector1 != null && Vector2 != null)
			{
				result = (Math.Acos(GetDotProduct(Vector1, Vector2) / (Vector1.mMagnitude * Vector2.mMagnitude)) / Math.PI * 180);
			}
			return result;
		}
		#endregion

		#region IRectangle2D
		/// <summary>計算指定點與指定矩形的邊的距離</summary>
		public static int GetDistanceBetweenPointAndRectangleEdge(IPoint2D Point, IRectangle2D Rectangle)
		{
			int result = 0;
			if (!Rectangle.IsIncludePoint(Point))
			{
				IPoint2D rectangleCenter = new Point2D((Rectangle.mMaxX + Rectangle.mMinX) / 2, (Rectangle.mMaxY + Rectangle.mMinY) / 2);
				IEnumerable<IPoint2D> intersectionPoints = GetIntersectionPoint(Rectangle, Point, rectangleCenter);
				if (intersectionPoints.Count() == 1)
				{
					result = (int)GetDistance(Point, intersectionPoints.ElementAt(0));
				}
			}
			return result;
		}
		/// <summary>判斷兩個矩形是否有重疊。共用同一個邊不算重疊</summary>
		public static bool IsRectangleOverlap(IRectangle2D Rectangle1, IRectangle2D Rectangle2)
		{
			if (Rectangle1 != null && Rectangle2 != null && Rectangle1.mMaxX > Rectangle2.mMinX && Rectangle2.mMaxX > Rectangle1.mMinX && Rectangle1.mMaxY > Rectangle2.mMinY && Rectangle2.mMaxY > Rectangle1.mMinY)
				return true;
			else
				return false;
		}
		/// <summary>判斷指定點集合中是否有任意點在指定矩形內。在矩形邊上也算是在矩形內</summary>
		public static bool IsAnyPointInside(IEnumerable<IPoint2D> Points, IRectangle2D Rectangle)
		{
			bool result = false;
			result = Points.Any(o => Rectangle.IsIncludePoint(o));
			return result;
		}
		/// <summary>判斷指定點是否在指定矩形內。在矩形邊上也算是在矩形內</summary>
		public static bool IsPointInside(IPoint2D Point, IRectangle2D Rectangle)
		{
			return (Point.mX >= Rectangle.mMinX && Point.mX <= Rectangle.mMaxX && Point.mY >= Rectangle.mMinY && Point.mY <= Rectangle.mMaxY);
		}
		/// <summary>判斷指定線段(點集合)是否有穿越指定矩形</summary>
		public static bool IsLinePassThroughRectangle(IEnumerable<IPoint2D> Points, IRectangle2D Rectangle)
		{
			bool result = false;
			for (int i = 0; i < Points.Count() - 1; ++i)
			{
				if (GetIntersectionPoint(Rectangle, Points.ElementAt(i), Points.ElementAt(i + 1)).Count() > 0)
				{
					result = true;
					break;
				}
			}
			return result;
		}
		/// <summary>計算能涵蓋兩個指定矩形的矩形</summary>
		public static IRectangle2D GetCoverRectangle(IRectangle2D Rectangle1, IRectangle2D Rectangle2)
		{
			IRectangle2D result = null;
			int x_max = Math.Max(Rectangle1.mMaxX, Rectangle2.mMaxX);
			int y_max = Math.Max(Rectangle1.mMaxY, Rectangle2.mMaxY);
			int x_min = Math.Min(Rectangle1.mMinX, Rectangle2.mMinX);
			int y_min = Math.Min(Rectangle1.mMinY, Rectangle2.mMinY);
			if (x_min < x_max && y_min < y_max)
			{
				result = new Rectangle2D(new Point2D(x_max, y_max), new Point2D(x_min, y_min));
			}
			return result;
		}
		/// <summary>計算兩個矩形的交集矩形</summary>
		public static IRectangle2D GetIntersectionRectangle(IRectangle2D Rectangle1, IRectangle2D Rectangle2)
		{
			IRectangle2D result = null;
			int x_max = Math.Min(Rectangle1.mMaxX, Rectangle2.mMaxX);
			int y_max = Math.Min(Rectangle1.mMaxY, Rectangle2.mMaxY);
			int x_min = Math.Max(Rectangle1.mMinX, Rectangle2.mMinX);
			int y_min = Math.Max(Rectangle1.mMinY, Rectangle2.mMinY);
			if (x_min < x_max && y_min < y_max)
			{
				result = new Rectangle2D(new Point2D(x_max, y_max), new Point2D(x_min, y_min));
			}
			return result;
		}
		/// <summary>以指定點為中心，產生一個邊長為 Radius * 2 大小的正方形</summary>
		public static IRectangle2D GetRectangle(int X, int Y, int Radius)
		{
			IRectangle2D result = null;
			if (Radius > 0)
			{
				result = new Rectangle2D(new Point2D(X + Radius, Y + Radius), new Point2D(X - Radius, Y - Radius));
			}
			return result;
		}
		/// <summary>將指定 Rectangle 放大。以其中心為基準， X 軸方向增加 OffsetX ， Y 軸方向增加 OffsetY</summary>
		public static IRectangle2D GetAmplifyRectangle(IRectangle2D Rectangle, int OffsetX, int OffsetY)
		{
			IRectangle2D result = null;
			int x_max = Rectangle.mMaxX + OffsetX;
			int y_max = Rectangle.mMaxY + OffsetY;
			int x_min = Rectangle.mMinX - OffsetX;
			int y_min = Rectangle.mMinY - OffsetY;
			result = new Rectangle2D(new Point2D(x_max, y_max), new Point2D(x_min, y_min));
			return result;
		}
		/// <summary>計算能涵蓋指定 Point 集合的矩形</summary>
		public static IRectangle2D GetCoverRectangle(IEnumerable<IPoint2D> Points)
		{
			IRectangle2D result = null;
			if (Points != null && Points.Count() > 0)
			{
				result = new Rectangle2D(GetDeepClone(Points.ElementAt(0)), GetDeepClone(Points.ElementAt(0)));
				for (int i = 1; i < Points.Count(); ++i)
				{
					if (Points.ElementAt(i).mX < result.mMinX) result.mMinPoint.mX = Points.ElementAt(i).mX;
					else if (Points.ElementAt(i).mX > result.mMaxX) result.mMaxPoint.mX = Points.ElementAt(i).mX;
					if (Points.ElementAt(i).mY < result.mMinY) result.mMinPoint.mY = Points.ElementAt(i).mY;
					else if (Points.ElementAt(i).mY > result.mMaxY) result.mMaxPoint.mY = Points.ElementAt(i).mY;
				}
			}
			return result;
		}
		/// <summary>合併矩形。若其中任兩個矩形有重疊，則將其合併成一個矩形</summary>
		public static IEnumerable<IRectangle2D> MergeRectangle(IEnumerable<IRectangle2D> Rectangles)
		{
			List<IRectangle2D> result = null;
			result = GetDeepClone(Rectangles).ToList();
			if (result != null && result.Count() > 1)
			{
				for (int i = 0; i < result.Count(); ++i)
				{
					for (int j = i + 1; j < result.Count(); ++j)
					{
						if (IsRectangleOverlap(result[i], result[j]))
						{
							result.Add(GetCoverRectangle(result[i], result[j]));
							result.RemoveAt(j);
							result.RemoveAt(i);

							// 回頭再檢查一次
							i = 0 - 1;
							j = i + 1;
							break;
						}
					}
				}
			}
			return result;
		}
		#endregion

		public static T GetDeepClone<T>(T o)
		{
			using (var ms = new System.IO.MemoryStream())
			{
				var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				formatter.Serialize(ms, o);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}
	}
}
