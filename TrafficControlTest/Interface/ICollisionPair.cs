using System;

namespace TrafficControlTest.Interface
{
	/// <summary>會發生交會的組合</summary>
	public interface ICollisionPair
	{
		IVehicleInfo mVehicle1 { get; }
		IVehicleInfo mVehicle2 { get; }
		IRectangle2D mCollisionRegion { get; }
		ITimePeriod mPeriod { get; }
		/// <summary>發生持續時間</summary>
		TimeSpan mDuration { get; }
		DateTime mLastUpdated { get; }

		void Set(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D CollisionRegion, ITimePeriod Period);
		void Update(IRectangle2D CollisionRegion, ITimePeriod Period);
		string ToString();
	}
}
