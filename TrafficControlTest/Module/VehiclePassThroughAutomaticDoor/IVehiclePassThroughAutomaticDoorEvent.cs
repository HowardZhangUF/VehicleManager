using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.VehiclePassThroughAutomaticDoor
{
	public enum PassThroughState
	{
		WillPass,
		Passing,
		Passed
	}

	public interface IVehiclePassThroughAutomaticDoorEvent : IItem
	{
		string mVehicleName { get; }
		string mAutomaticDoorName { get; }
		int mDistance { get; }
		PassThroughState mState { get; }
		bool mIsHandled { get; }
		DateTime mLastUpdated { get; }

		void Set(string VehicleName, string AutomaticDoorName, int Distance);
		void UpdateDistance(int Distance);
		void UpdateState(PassThroughState State);
		void UpdateIsHandled(bool IsHandled);
	}
}
