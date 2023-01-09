using System;

namespace LibraryForVM
{
	[Serializable]
	public class Rectangle2D : IRectangle2D
	{
		public IPoint2D mMaxPoint { get; private set; }
		public IPoint2D mMinPoint { get; private set; }

		public int mMaxX { get { return mMaxPoint.mX; } }
		public int mMaxY { get { return mMaxPoint.mY; } }
		public int mMinX { get { return mMinPoint.mX; } }
		public int mMinY { get { return mMinPoint.mY; } }
		public int mCenterX { get { return (mMaxPoint.mX + mMinPoint.mX) / 2; } }
		public int mCenterY { get { return (mMaxPoint.mY + mMinPoint.mY) / 2; } }
		public int mWidth { get { if (mMaxPoint == null || mMinPoint == null) return 0; else return (mMaxPoint.mX - mMinPoint.mX); } }
		public int mHeight { get { if (mMaxPoint == null || mMinPoint == null) return 0; else return (mMaxPoint.mY - mMinPoint.mY); } }

		public Rectangle2D(IPoint2D MaxPoint, IPoint2D MinPoint)
		{
			Set(MaxPoint, MinPoint);
		}
		public void Set(IPoint2D MaxPoint, IPoint2D MinPoint)
		{
			mMaxPoint = MaxPoint;
			mMinPoint = MinPoint;
		}
		public void Amplify(int OffsetX, int OffsetY)
		{
			mMaxPoint.mX += OffsetX;
			mMaxPoint.mY += OffsetY;
			mMinPoint.mX -= OffsetX;
			mMinPoint.mY -= OffsetY;
		}
		public bool IsIncludePoint(IPoint2D Point)
		{
			return (Point.mX >= mMinPoint.mX && Point.mX <= mMaxPoint.mX && Point.mY >= mMinPoint.mY && Point.mY <= mMaxPoint.mY);
		}
		public override string ToString()
		{
			return $"{mMaxPoint.ToString()},{mMinPoint.ToString()}";
		}
	}
}
