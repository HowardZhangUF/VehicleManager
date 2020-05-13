
namespace TrafficControlTest.Module.General
{
	public interface IVector2D
	{
		double mXComponent { get; set; }
		double mYComponent { get; set; }
		double mMagnitude { get; }
		/// <summary>方向 (degree)</summary>
		double mDirection { get; }

		void Set(double XComponent, double YComponent);
		/// <summary>將自身轉換成單位向量</summary>
		void Normalize();
		string ToString();
	}
}
