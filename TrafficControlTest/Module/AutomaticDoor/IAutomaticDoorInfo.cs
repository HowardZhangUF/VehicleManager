using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public enum AutomaticDoorState
	{
		None,
		Closed,
		Closing,
		Opened,
		Opening
	}

	public interface IAutomaticDoorInfo : IItem
	{
		IRectangle2D mRange { get; }
		string mIpPort { get; }
		bool mIsConnected { get; }
		AutomaticDoorState mState { get; }
		DateTime mLastUpdated { get; }

		void Set(string Name, IRectangle2D Range, string IpPort);
		void UpdateIsConnected(bool IsConnected);
		void UpdateState(AutomaticDoorState State);
	}
}
