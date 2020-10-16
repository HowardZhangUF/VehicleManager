﻿using System;
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
		public void UpdateIsOpened(bool IsOpened)
		{
			if (mIsOpened != IsOpened)
			{
				mIsOpened = IsOpened;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("IsOpened");
			}
		}
		public override string ToString()
		{
			return $"{mName}/IPPort:{mIpPort}/IsConnected:{mIsConnected.ToString()}/IsOpened:{mIsOpened.ToString()}";
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
