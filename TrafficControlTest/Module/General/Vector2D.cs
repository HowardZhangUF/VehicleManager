using System;

namespace TrafficControlTest.Module.General
{
	class Vector2D : IVector2D
	{
		public double mXComponent { get; set; }
		public double mYComponent { get; set; }
		public double mMagnitude { get { return Math.Sqrt(mXComponent * mXComponent + mYComponent * mYComponent); } }
		public double mDirection { get { return Math.Atan2(mYComponent, mXComponent) / Math.PI * 180.0f; } }

		public Vector2D(double XComponent, double YComponent)
		{
			Set(XComponent, YComponent);
		}
		public void Set(double XComponent, double YComponent)
		{
			mXComponent = XComponent;
			mYComponent = YComponent;
		}
		public void Normalize()
		{
			double x = mXComponent / mMagnitude;
			double y = mYComponent / mMagnitude;
			Set(x, y);
		}
		public override string ToString()
		{
			return $"{mXComponent.ToString("F2")}i + {mYComponent.ToString("F2")}j";
		}
	}
}
