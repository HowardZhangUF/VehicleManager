using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.LimitVehicleCountZone;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.VehiclePassThroughAutomaticDoor;

namespace TrafficControlTest.Module.VehiclePassThroughLimitVehicleCountZone
{
	public interface IVehiclePassThroughLimitVehicleCountZoneEvent : IItem
	{
		IVehicleInfo rVehicleInfo { get; }
		ILimitVehicleCountZoneInfo rLimitVehicleCountZoneInfo { get; }
		int mDistance { get; }
		PassThroughState mState { get; }
		DateTime mStartTimestamp { get; }
		TimeSpan mDuration { get; }
		DateTime mLastUpdated { get; }

		void Set(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, int Distance);
		void UpdateDistance(int Distance);
		void UpdateState(PassThroughState State);
	}
}
