using Library;
using System;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.CollisionEvent
{
	/// <summary>會發生交會的組合</summary>
	public interface ICollisionPair : IItem
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
        /// <summary>第一台車預估當前速度通過交會區域的時間區間</summary>
        ITimePeriod mPassPeriodOfVehicle1WithCurrentVelocity { get; }
        /// <summary>第二台車預估當前速度通過交會區域的時間區間</summary>
        ITimePeriod mPassPeriodOfVehicle2WithCurrentVelocity { get; }
        /// <summary>第一台車預估最高速度通過交會區域的時間區間</summary>
        ITimePeriod mPassPeriodOfVehicle1WithMaximumVeloctiy { get; }
        /// <summary>第二台車預估最高速度通過交會區域的時間區間</summary>
        ITimePeriod mPassPeriodOfVehicle2WithMaximumVeloctiy { get; }
        /// <summary>發生持續時間</summary>
        TimeSpan mDuration { get; }
		DateTime mLastUpdated { get; }

        void Set(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy);
		void Update(IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy);
		string ToString();
	}
}
