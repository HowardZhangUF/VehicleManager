
namespace Library
{
	public interface ITowardPoint2D : IPoint2D
	{
		double mToward { get; set; }

		void Set(IPoint2D Point, double Toward);
		void Set(int X, int Y, double Toward);
		new string ToString();
	}
}
