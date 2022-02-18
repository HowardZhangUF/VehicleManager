using LibraryForVM;
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
	public class VehiclePassThroughLimitVehicleCountZoneEvent : IVehiclePassThroughLimitVehicleCountZoneEvent
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public IVehicleInfo rVehicleInfo { get; private set; } = null;
		public ILimitVehicleCountZoneInfo rLimitVehicleCountZoneInfo { get; private set; } = null;
		public int mDistance { get; private set; } = int.MaxValue;
		public PassThroughState mState { get; private set; } = PassThroughState.WillPass;
		public DateTime mStartTimestamp { get; private set; } = DateTime.Now;
		public TimeSpan mDuration { get { return DateTime.Now.Subtract(mStartTimestamp); } }
		public DateTime mLastUpdated { get; private set; } = DateTime.Now;

		public VehiclePassThroughLimitVehicleCountZoneEvent(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, int Distance)
		{
			Set(VehicleInfo, LimitVehicleCountZoneInfo, Distance);
		}
		public void Set(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, int Distance)
		{
			DateTime tmp = DateTime.Now;
			mName = $"VehiclePassThroughLimitVehicleCountZoneEvent{tmp.ToString("yyyyMMddHHmmssfff")}";
			rVehicleInfo = VehicleInfo;
			rLimitVehicleCountZoneInfo = LimitVehicleCountZoneInfo;
			mDistance = Distance;
			mStartTimestamp = tmp;
			mLastUpdated = tmp;
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
		public void UpdateState(PassThroughState State)
		{
			if (mState != State)
			{
				mState = State;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("State");
			}
		}
		public override string ToString()
		{
			return $"{mName}/{rVehicleInfo.mName}/{rLimitVehicleCountZoneInfo.mName}/{mDistance}/{mState.ToString()}";
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
