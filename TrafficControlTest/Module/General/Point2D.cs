using System;

namespace TrafficControlTest.Module.General
{
	[Serializable]
	public class Point2D : IPoint2D
	{
		public int mX { get; set; } = 0;
		public int mY { get; set; } = 0;

		public Point2D(int X, int Y)
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
			return $"({mX},{mY})";
		}
	}
}
