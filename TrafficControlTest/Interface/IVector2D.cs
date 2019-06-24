namespace TrafficControlTest.Interface
{
	public interface IVector2D
	{
		double mXComponent { get; set; }
		double mYComponent { get; set; }
		double mMagnitude { get; }
		/// <summary>方向 (degree)</summary>
		double mDirection { get; }

		void Set(double XComponent, double YComponent);
		void Normalize();
		string ToString();
	}
}
