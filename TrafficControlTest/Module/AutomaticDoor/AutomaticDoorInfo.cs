using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorInfo : IAutomaticDoorInfo
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public IRectangle2D mRange { get; private set; } = null;
		public string mIpPort { get; private set; } = string.Empty;
		public bool mIsConnected { get; private set; } = false;
		public bool mIsOpened { get; private set; } = false;
		public DateTime mLastUpdated { get; private set; } = DateTime.Now;

		public void UpdateRange(IRectangle2D Range)
		{
			if (Range != null)
			{
				mRange = Range;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("Range");
			}
		}
		public void UpdateIpPort(string IpPort)
		{
			if (!string.IsNullOrEmpty(IpPort) && mIpPort != IpPort)
			{
				mIpPort = IpPort;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("IpPort");
			}
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
		public void UpdateIsOpened(bool IsOpened)
		{
			if (mIsOpened != IsOpened)
			{
				mIsOpened = IsOpened;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("IsOpened");
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
