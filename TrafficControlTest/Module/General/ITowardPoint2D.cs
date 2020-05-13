
namespace TrafficControlTest.Module.General
{
	public interface ITowardPoint2D : IPoint2D
	{
		double mToward { get; set; }

		void Set(int X, int Y, double Toward);
		new string ToString();
	}
}
