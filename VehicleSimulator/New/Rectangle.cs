using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class Rectangle
	{
		public Point mMin { get; private set; } = new Point();
		public Point mMax { get; private set; } = new Point();
		public Point mCenter { get; private set; } = new Point();
		public Point mLeftTop { get; private set; } = new Point();
		public Point mRightBottom { get; private set; } = new Point();
		public int mWidth { get; private set; } = 0;
		public int mHeight { get; private set; } = 0;

		public Rectangle()
		{

		}
		public Rectangle(int X1, int Y1, int X2, int Y2)
		{
			Set(X1, Y1, X2, Y2);
		}
		public void Set(int X1, int Y1, int X2, int Y2)
		{
			mMin.Set(Math.Min(X1, X2), Math.Min(Y1, Y2));
			mMax.Set(Math.Max(X1, X2), Math.Max(Y1, Y2));
			mCenter.Set((mMax.mX + mMin.mX) / 2, (mMax.mY + mMin.mY) / 2);
			mLeftTop.Set(mMin.mX, mMax.mY);
			mRightBottom.Set(mMax.mX, mMin.mY);
			mWidth = mMax.mX - mMin.mX;
			mHeight = mMax.mY - mMin.mY;
		}
		public void Amplify(int OffsetX, int OffsetY)
		{
			mMin.Set(mMin.mX - OffsetX, mMin.mY - OffsetY);
			mMax.Set(mMax.mX + OffsetX, mMax.mY + OffsetY);
			mCenter.Set((mMax.mX + mMin.mX) / 2, (mMax.mY + mMin.mY) / 2);
			mLeftTop.Set(mMin.mX, mMax.mY);
			mRightBottom.Set(mMax.mX, mMin.mY);
			mWidth = mMax.mX - mMin.mX;
			mHeight = mMax.mY - mMin.mY;
		}
		public bool IsIncludePoint(Point Point, bool IncludeOnEdge = false)
		{
			return IsIncludePoint(this, Point, IncludeOnEdge);
		}
		public bool IsOverlap(Rectangle Rectangle, bool IncludeOnEdge = false)
		{
			return IsOverlap(this, Rectangle, IncludeOnEdge);
		}
		public bool IsBeenLineThrough(Point Point1, Point Point2)
		{
			return IsRectangleBeenLineThrough(this, Point1, Point2);
		}
		public override string ToString()
		{
			return ToString();
		}
		public virtual string ToString(string Separator = ",")
		{
			return $"{mMin.ToString(Separator)}{Separator}{mMax.ToString(Separator)}";
		}
		public static Rectangle FromString(string String, string Separator = ",")
		{
			string[] tmp = String.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
			return new Rectangle(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2]), int.Parse(tmp[3]));
		}
		/// <summary>判斷指定矩形是否有包含指定點。可透過參數 IncludeOnEdge 設定落在邊線上是否算包含</summary>
		public static bool IsIncludePoint(Rectangle Rectangle, Point Point, bool IncludeOnEdge = false)
		{
			if (IncludeOnEdge)
			{
				return Point.mX >= Rectangle.mMin.mX && Point.mX <= Rectangle.mMax.mX && Point.mY >= Rectangle.mMin.mY && Point.mY <= Rectangle.mMax.mY;
			}
			else
			{
				return Point.mX > Rectangle.mMin.mX && Point.mX < Rectangle.mMax.mX && Point.mY > Rectangle.mMin.mY && Point.mY < Rectangle.mMax.mY;
			}
		}
		/// <summary>判斷兩指定矩形是否有重疊。可透過參數 IncludeOnEdge 設定邊線重疊是否算重疊</summary>
		public static bool IsOverlap(Rectangle Rectangle1, Rectangle Rectangle2, bool IncludeOnEdge = false)
		{
			if (IncludeOnEdge)
			{
				return Rectangle1.mMax.mX >= Rectangle2.mMin.mX && Rectangle2.mMax.mX >= Rectangle1.mMin.mX && Rectangle1.mMax.mY >= Rectangle2.mMin.mY && Rectangle2.mMax.mY >= Rectangle1.mMin.mY;
			}
			else
			{
				return Rectangle1.mMax.mX > Rectangle2.mMin.mX && Rectangle2.mMax.mX > Rectangle1.mMin.mX && Rectangle1.mMax.mY > Rectangle2.mMin.mY && Rectangle2.mMax.mY > Rectangle1.mMin.mY;
			}
		}
		public static bool IsRectangleBeenLineThrough(Rectangle Rectangle, Point Point1, Point Point2)
		{
			return Point.IsHaveIntersectionPoint(Rectangle.mMin, Rectangle.mLeftTop, Point1, Point2)
				|| Point.IsHaveIntersectionPoint(Rectangle.mLeftTop, Rectangle.mMax, Point1, Point2)
				|| Point.IsHaveIntersectionPoint(Rectangle.mMax, Rectangle.mRightBottom, Point1, Point2)
				|| Point.IsHaveIntersectionPoint(Rectangle.mRightBottom, Rectangle.mMin, Point1, Point2);
		}
	}
}
