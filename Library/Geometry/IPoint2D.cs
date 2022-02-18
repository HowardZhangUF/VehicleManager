
namespace Library
{
	public interface IPoint2D
	{
		int mX { get; set; }
		int mY { get; set; }

		void Set(int X, int Y);
		string ToString();
	}
}
