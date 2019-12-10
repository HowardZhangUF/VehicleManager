using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	public enum SendState
	{
		Unsend,
		Sending,
		SendSuccessed,
		SendFailed
	}

	public enum ExecuteState
	{
		Unexecute,
		Executing,
		ExecuteSuccessed,
		ExecuteFailed
	}

	/// <summary>
	/// - 儲存任務的資訊及狀態
	/// </summary>
	/// <remarks>
	/// mName 為根據時間產生的流水號，其為唯一，
	/// mMission.mMissionId 為客戶指令給的 MissionID 。
	/// </remarks>
	public interface IMissionState : IItem
	{
		IMission mMission { get; }
		string mSourceIpPort { get; }
		string mExecutorId { get; }
		SendState mSendState { get; }
		ExecuteState mExecuteState { get; }
		DateTime mReceivedTimestamp { get; }
		DateTime mExecutionStartTimestamp { get; }
		DateTime mExecutionStopTimestamp { get; }
		DateTime mLastUpdate { get; }

		void Set(IMission Mission);
		string GetMissionId();
		void UpdatePriority(int Priority);
		void UpdateSourceIpPort(string SourceIpPort);
		void UpdateExecutorId(string ExecutorId);
		void UpdateSendState(SendState SendState);
		void UpdateExecuteState(ExecuteState ExecuteState);
		string[] ToStringArray();
	}
}
