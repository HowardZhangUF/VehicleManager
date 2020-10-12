using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public enum AutomaticDoorControlCommand
	{
		/// <summary>無</summary>
		None,
		/// <summary>開門</summary>
		Open,
		/// <summary>關門</summary>
		Close
	}

	public enum AutomaticDoorControlCommandSendState
	{
		/// <summary>尚未送出</summary>
		Unsend,
		/// <summary>已送出等待回應中</summary>
		Sending,
		/// <summary>已送出且有收到回應</summary>
		SentSuccessed,
		/// <summary>已送出但未收到回應</summary>
		SentFailed
	}

	/// <summary>預計對自動門執行的控制及原因</summary>
	public interface IAutomaticDoorControl : IItem
	{
		string mAutomaticDoorName { get; }
		AutomaticDoorControlCommand mCommand { get; }
		string mCause { get; }
		AutomaticDoorControlCommandSendState mSendState { get; }
		DateTime mLastUpdated { get; }

		void Set(string AutomaticDoorName, AutomaticDoorControlCommand Command, string Cause);
		void UpdateSendState(AutomaticDoorControlCommandSendState SendState);
	}
}
