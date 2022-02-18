namespace LibraryForVM
{
	public class TowardPoint2D : ITowardPoint2D
	{
		public int mX { get; set; }
		public int mY { get; set; }
		public double mToward { get; set; }

		public TowardPoint2D(int X, int Y, double Toward)
		{
			Set(X, Y, Toward);
		}
		public TowardPoint2D(IPoint2D Point, double Toward)
		{
			Set(Point, Toward);
		}
		public void Set(int X, int Y)
		{
			Set(X, Y, 0);
		}
		public void Set(IPoint2D Point, double Toward)
		{
			Set(Point.mX, Point.mY, Toward);
		}
		public void Set(int X, int Y, double Toward)
		{
			mX = X;
			mY = Y;
			mToward = Toward;
		}
		public override string ToString()
		{
			return $"{mX},{mY},{mToward.ToString("F2")}";
		}
	}
}
