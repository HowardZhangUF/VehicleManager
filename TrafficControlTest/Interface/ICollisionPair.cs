using System;

namespace TrafficControlTest.Interface
{
	/// <summary>會發生交會的組合</summary>
	public interface ICollisionPair
	{
		IVehicleInfo mVehicle1 { get; }
		IVehicleInfo mVehicle2 { get; }
		IRectangle2D mCollisionRegion { get; }
		/// <summary>預計交會開始到交會結束的時間區間</summary
		/// <remarks>
		/// 通常交會控制會是一台車暫停一台車繼續移動，
		/// 此 Period 即為繼續移動的那台車的進入/離開交會區域的時間區間。
		/// </remarks>
		ITimePeriod mPeriod { get; }
		/// <summary>發生持續時間</summary>
		TimeSpan mDuration { get; }
		DateTime mLastUpdated { get; }

		void Set(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D CollisionRegion, ITimePeriod Period);
		void Update(IRectangle2D CollisionRegion, ITimePeriod Period);
		string ToString();
	}
}
