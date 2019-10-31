using System;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

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

	/// <summary>干預指令</summary>
	public enum Command
	{
		InsertMovingBuffer,
		RemoveMovingBuffer,
		PauseMoving,
		ResumeMoving
	}

	public interface IVehicleControl : IItem
	{
		/// <summary>欲控制的車的識別碼</summary>
		string mVehicleId { get; }
		/// <summary>傳送狀態</summary>
		SendState mSendState { get; }
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

		void Set(string VehicleId, Command Command, string[] Parameters, string CauseId, string CauseDetail);
		void UpdateSendState(SendState SendState);
		void UpdateParameters(string[] Parameters);
	}
}
