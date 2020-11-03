using System;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Mission
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

    public enum MissionFailedReason
    {
        None,
        VehicleDisconnected,
        VehicleIdleButNotArrived,
        ExectutedTimeout,
        SentTimeout,
        CancelByGUI,
        CancelByHostCommand
    }

	/// <summary>
	/// - 儲存任務的資訊及狀態
	/// </summary>
	/// <remarks>
	/// mName 為根據時間產生的流水號，其為唯一，
	/// mMission.mMissionId 為客戶指令給的 MissionID ，
	/// 若客戶無提供 MissionID 時，在回報任務狀態時的 MissionID 會填入 mName 的資訊。
	/// </remarks>
	public interface IMissionState : IItem
	{
		IMission mMission { get; }
		string mSourceIpPort { get; }
		string mExecutorId { get; }
		SendState mSendState { get; }
		ExecuteState mExecuteState { get; }
        MissionFailedReason mFailedReason { get; }
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
        void UpdateFailedReason(MissionFailedReason FailedReason);
		string[] ToStringArray();
	}
}
