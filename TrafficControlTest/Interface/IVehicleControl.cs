using System;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleControl;

namespace TrafficControlTest.Interface
{
	/// <summary>傳送狀態。有收到回應代表傳送成功，反之為傳送失敗</summary>
	public enum SendState
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

	/// <summary>執行狀態</summary>
	public enum ExecuteState
	{
		/// <summary>尚未執行</summary>
		Unexecute,
		/// <summary>尚未執行就被取消</summary>
		Canceled,
		/// <summary>執行中</summary>
		Executing,
		/// <summary>執行成功</summary>
		Completed,
		/// <summary>執行失敗</summary>
		Failed,
		/// <summary>執行後被中斷</summary>
		Aborted
	}

	/// <summary>干預指令</summary>
	public enum Command
	{
		InsertMovingBuffer,
		RemoveMovingBuffer,
		PauseMoving,
		ResumeMoving
	}

	public interface IVehicleControl
	{
		event EventHandlerIVehicleControl StateUpdated;

		/// <summary>識別碼</summary>
		string mName { get; }
		/// <summary>傳送狀態</summary>
		SendState mSendState { get; }
		/// <summary>執行狀態</summary>
		ExecuteState mExecuteState { get; }
		/// <summary>指令</summary>
		Command mCommand { get; }
		/// <summary>指令參數</summary>
		string[] mParameters { get; }
		/// <summary>起因的識別碼</summary>
		string mCauseId { get; }
		/// <summary>起因</summary>
		string mCauseDetail { get; }
		/// <summary>開始執行時間</summary>
		DateTime mExecutionStartTimestamp { get; }
		/// <summary>執行結束時間</summary>
		DateTime mExecutionStopTimestamp { get; }
		/// <summary>上次更新時間</summary>
		DateTime mLastUpdated { get; }

		void Set(Command Command, string[] Parameters, string CauseId, string CauseDetail);
		void UpdateSendState(SendState SendState);
		void UpdateExecuteState(ExecuteState ExecuteState);
		void UpdateParameters(string[] Parameters);
	}
}
