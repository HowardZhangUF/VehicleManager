using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.VehiclePassThroughAutomaticDoor
{
	public class VehiclePassThroughAutomaticDoorEvent : IVehiclePassThroughAutomaticDoorEvent
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public string mVehicleName { get; private set; } = string.Empty;
		public string mAutomaticDoorName { get; private set; } = string.Empty;
		public int mDistance { get; private set; } = int.MaxValue;
		public PassThroughState mState { get; private set; } = PassThroughState.WillPass;
		public bool mIsHandled { get; private set; } = false;
		public DateTime mLastUpdated { get; private set; } = DateTime.Now;

		public VehiclePassThroughAutomaticDoorEvent(string VehicleName, string AutomaticDoorName, int Distance)
		{
			Set(VehicleName, AutomaticDoorName, Distance);
		}
		public void Set(string VehicleName, string AutomaticDoorName, int Distance)
		{
			mName = $"PassThroughEventOf{VehicleName}&{AutomaticDoorName}";
			mVehicleName = VehicleName;
			mAutomaticDoorName = AutomaticDoorName;
			mDistance = Distance;
			mLastUpdated = DateTime.Now;
		}
		public void UpdateDistance(int Distance)
		{
			if (mDistance != Distance)
			{
				mDistance = Distance;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("Distance");
			}
		}
		public void UpdateIsHandled(bool IsHandled)
		{
			if (mIsHandled != IsHandled)
			{
				mIsHandled = IsHandled;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("IsHandled");
			}
		}
		public void UpdateState(PassThroughState State)
		{
			if (mState != State)
			{
				mState = State;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("State");
			}
		}

		protected virtual void RaiseEvent_StatusUpdated(string StatusName, bool Sync = true)
		{
			if (Sync)
			{
				StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs(DateTime.Now, mName, StatusName));
			}
			else
			{
				Task.Run(() => { StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs(DateTime.Now, mName, StatusName)); });
			}
		}
	}
}
