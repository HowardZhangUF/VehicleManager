using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorInfo : IAutomaticDoorInfo
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public IRectangle2D mRange { get; private set; } = null;
		public string mIpPort { get; private set; } = string.Empty;
		public bool mIsConnected { get; private set; } = false;
		public AutomaticDoorState mState { get; private set; } = AutomaticDoorState.Closed;
		public DateTime mLastUpdated { get; private set; } = DateTime.Now;

		public AutomaticDoorInfo(string Name, IRectangle2D Range, string IpPort)
		{
			Set(Name, Range, IpPort);
		}
		public void Set(string Name, IRectangle2D Range, string IpPort)
		{
			mName = Name;
			mRange = Range;
			mIpPort = IpPort;
			mLastUpdated = DateTime.Now;
		}
		public void UpdateIsConnected(bool IsConnected)
		{
			if (mIsConnected != IsConnected)
			{
				mIsConnected = IsConnected;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("IsConnected");
			}
		}
		public void UpdateState(AutomaticDoorState State)
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
			return $"{mName}/IPPort:{mIpPort}/IsConnected:{mIsConnected.ToString()}/State:{mState.ToString()}";
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
