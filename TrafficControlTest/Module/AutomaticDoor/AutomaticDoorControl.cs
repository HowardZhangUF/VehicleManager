using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorControl : IAutomaticDoorControl
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public string mAutomaticDoorName { get; private set; } = string.Empty;
		public AutomaticDoorControlCommand mCommand { get; private set; } = AutomaticDoorControlCommand.None;
		public string mCause { get; private set; } = string.Empty;
		public AutomaticDoorControlCommandSendState mSendState { get; private set; } = AutomaticDoorControlCommandSendState.Unsend;
		public DateTime mLastUpdated { get; private set; } = DateTime.Now;

		public void Set(string AutomaticDoorName, AutomaticDoorControlCommand Command, string Cause)
		{
			mName = $"ControlFor{AutomaticDoorName}";
			mAutomaticDoorName = AutomaticDoorName;
			mCommand = Command;
			mCause = Cause;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StatusUpdated("Name,AutomaticDoorName,Command,Cause");
		}
		public void UpdateSendState(AutomaticDoorControlCommandSendState SendState)
		{
			mSendState = SendState;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StatusUpdated("SendState");
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
