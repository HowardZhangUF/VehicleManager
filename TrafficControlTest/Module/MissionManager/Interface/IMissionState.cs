﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

	public interface IMissionState
	{
		event EventHandlerIMissionStateStateUpdated StateUpdated;

		IMission mMission { get; }
		string mSourceIpPort { get; }
		string mMissionId { get; }
		string mExecutorId { get; }
		SendState mSendState { get; }
		ExecuteState mExecuteState { get; }
		DateTime mReceivedTimestamp { get; }
		DateTime mExecutionStartTimestamp { get; }
		DateTime mExecutionStopTimestamp { get; }
		DateTime mLastUpdate { get; }

		void Set(IMission Mission);
		void UpdatePriority(int Priority);
		void UpdateSourceIpPort(string SourceIpPort);
		void UpdateExecutorId(string ExecutorId);
		void UpdateSendState(SendState SendState);
		void UpdateExecuteState(ExecuteState ExecuteState);
		string[] ToStringArray();
	}
}