using LibraryForVM;
using System;

namespace TrafficControlTest.Module.InterveneCommand
{
	/// <summary>傳送狀態。有收到回應代表傳送成功，反之為傳送失敗</summary>
	public enum SendState
	{
		/// <summary>尚未送出</summary>
		Unsend,
		/// <summary>已送出等待回應中</summary>
		Sending,
		/// <summary>已送出且有收到回應</summary>
		SendSuccessed,
		/// <summary>已送出但未收到回應</summary>
		SendFailed
	}

	public enum ExecuteState
	{
		/// <summary>尚未執行</summary>
		Unexecute,
		/// <summary>執行中</summary>
		Executing,
		/// <summary>執行完成且成功</summary>
		ExecuteSuccessed,
		/// <summary>執行完成且失敗</summary>
		ExecuteFailed,
		/// <summary>執行暫停</summary>
		ExecutePaused
	}

	/// <summary>控制指令</summary>
	public enum Command
	{
		InsertMovingBuffer,
		RemoveMovingBuffer,
		PauseMoving,        // Immediately 切換自走車狀態，自走車 Running 時可以下，當自走車已經在執行其他的 Control 時可以下
		ResumeMoving,       // Immediately 切換自走車狀態，自走車 Pause 時可以下，當自走車已經在執行其他的 Control 時可以下
		Goto,               // Normal 讓自走車做一件事，自走車 Idle 時可以下
		GotoPoint,          // Normal 讓自走車做一件事，自走車 Idle 時可以下
		GotoTowardPoint,    // Normal 讓自走車做一件事，自走車 Idle 時可以下
		Stop,               // Immediately 切換自走車狀態，自走車 Running/PathNotFound/ObstacleExist/BumperTrigger 時可以下，當自走車已經在執行其他的 Control 時可以下
		Charge,             // Normal 讓自走車做一件事，自走車 Idle 時可以下
		Uncharge,           // Normal 讓自走車做一件事，自走車 Charge/ChargeIdle 時可以下
		Stay,               // System 讓自走車保持當前狀態，並不要執行其他 Control (不會對自走車下指令，而是讓系統知道不要下 Control 給該車) (執行中會保持 Executing 的狀態，直到收到 Unstay 的 Control 則會變成 ExecuteSuccessed)
		Unstay,				// System 讓自走車恢復成可工作狀態 (不會對自走車下指令，而是讓系統知道不要下 Control 給該車) (執行完就會變成 ExecuteSuccessed)
		PauseControl,		// System 讓該自走車當前 Control 暫停 (本身不是對自走車下指令，而是讓系統對該車當前的 Control 調整成 ExecutePaused)
		ResumeControl,      // System 讓該自走車當前 Control 恢復 (本身不是對自走車下指令，而是讓系統 ExecutePasued 的 Control 調整成 Executing ，並且重新傳送動作指令給自走車)
		Abort               // System + Immediately 中止指定自走車的所有 Command 並下達 Stop 給自走車
	}

	public enum FailedReason
	{
		None,
		VehicleDisconnected,
		VehicleOccurError,
		VehicleGotoCharge,
		VehicleNotGoingtoCharge,
		VehicleIdleButNotArrived,
		VehicleStopped,
		ExectutedTimeout,
		SentTimeout,
		CancelByGUI,
		CancelByHostCommand,
		ObjectNotExist,
		CollisionRemoved,
		PassThroughLimitVehicleCountZoneEventRemoved
	}

	/// <summary>
	/// - 儲存預計對車子進行的控制及其原因
	/// - 儲存物件的識別碼
	/// - 物件的資訊更新時會拋出事件
	/// </summary>
	public interface IVehicleControl : IItem
	{
		/// <summary>欲控制的車的識別碼</summary>
		string mVehicleId { get; }
		/// <summary>控制指令</summary>
		Command mCommand { get; }
		/// <summary>控制指令參數</summary>
		string[] mParameters { get; }
		/// <summary>參數結合成單一字串</summary>
		string mParametersString { get; }
		/// <summary>起因的識別碼</summary>
		string mCauseId { get; }
		/// <summary>起因</summary>
		string mCauseDetail { get; }
		/// <summary>傳送狀態</summary>
		SendState mSendState { get; }
		/// <summary>執行狀態</summary>
		ExecuteState mExecuteState { get; }
		/// <summary>失敗原因</summary>
		FailedReason mFailedReason { get; }
		/// <summary>接收時間</summary>
		DateTime mReceivedTimestamp { get; }
		/// <summary>開始執行時間</summary>
		DateTime mExecutionStartTimestamp { get; }
		/// <summary>執行結束時間</summary>
		DateTime mExecutionStopTimestamp { get; }
		/// <summary>上次更新時間</summary>
		DateTime mLastUpdated { get; }

		void Set(string VehicleId, Command Command, string[] Parameters, string CauseId, string CauseDetail);
		void UpdateSendState(SendState SendState);
		void UpdateExecuteState(ExecuteState ExecuteState);
		void UpdateExecuteFailedReason(FailedReason ExecuteFailedReason);
	}
}
