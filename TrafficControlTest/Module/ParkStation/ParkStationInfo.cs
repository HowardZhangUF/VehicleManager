using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.ParkStation
{
	public class ParkStationInfo : IParkStationInfo // 基本上與 ChargeStationInfo 內容一樣
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public ITowardPoint2D mLocation { get; private set; } = null;
		public IRectangle2D mLocationRange { get; private set; } = null;
		public bool mEnable { get; private set; } = true;
		public bool mIsBeingUsed { get; private set; } = false;
		public TimeSpan mIsBeingUsedDuration { get { return DateTime.Now.Subtract(mTimestampOfIsBeingUsedChanged); } }
		public DateTime mLastUpdated { get; private set; } = DateTime.Now;

		private DateTime mTimestampOfIsBeingUsedChanged = DateTime.Now;

		public ParkStationInfo(string Name, ITowardPoint2D Location, IRectangle2D LocationRange)
		{
			Set(Name, Location, LocationRange);
		}
		public void Set(string Name, ITowardPoint2D Location, IRectangle2D LocationRange)
		{
			mName = Name;
			mLocation = Location;
			mLocationRange = LocationRange;
			mLastUpdated = DateTime.Now;
		}
		public void UpdateEnable(bool Enable)
		{
			if (mEnable != Enable)
			{
				mEnable = Enable;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("Enable");
			}
		}
		public void UpdateIsBeingUsed(bool IsBeingUsed)
		{
			if (mIsBeingUsed != IsBeingUsed)
			{
				mIsBeingUsed = IsBeingUsed;
				mTimestampOfIsBeingUsedChanged = DateTime.Now;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("IsBeingUsed");
			}
		}
		public override string ToString()
		{
			return $"{mName}/Enable:{mEnable.ToString()}/IsBeingUsed:{mIsBeingUsed.ToString()}";
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
