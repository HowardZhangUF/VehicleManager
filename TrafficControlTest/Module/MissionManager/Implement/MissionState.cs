using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class MissionState : IMissionState
	{
		public event EventHandlerIMissionStateStateUpdated StateUpdated;

		public IMission mMission { get; private set; }
		public string mSourceIpPort { get; private set; }
		public string mMissionId { get; private set; }
		public string mExecutorId { get; private set; }
		public SendState mSendState { get; private set; }
		public ExecuteState mExecuteState { get; private set; }
		public DateTime mReceivedTimestamp { get; private set; }
		public DateTime mExecutionStartTimestamp { get; private set; }
		public DateTime mExecutionStopTimestamp { get; private set; }
		public DateTime mLastUpdate { get; private set; }

		public MissionState(IMission Mission)
		{
			Set(Mission);
		}
		public void Set(IMission Mission)
		{
			mMission = Mission;
			mMissionId = string.IsNullOrEmpty(Mission.mMissionId) ? $"Cmd{DateTime.Now.ToString("yyyyMMddHHmmssfff")}" : Mission.mMissionId;
			mExecutorId = string.Empty;
			mSendState = SendState.Unsend;
			mExecuteState = ExecuteState.Unexecute;
			mReceivedTimestamp = DateTime.Now;
			mExecutionStartTimestamp = DateTime.Now;
			mExecutionStopTimestamp = DateTime.Now;
			mLastUpdate = DateTime.Now;
		}
		public void UpdatePriority(int Priority)
		{
			if (mMission.mPriority != Priority)
			{
				mMission.UpdatePriority(Priority);
				mLastUpdate = DateTime.Now;
				RaiseEvent_StateUpdated("Priority");
			}
		}
		public void UpdateSourceIpPort(string SourceIpPort)
		{
			if (!string.IsNullOrEmpty(SourceIpPort) && mSourceIpPort != SourceIpPort)
			{
				mSourceIpPort = SourceIpPort;
				mLastUpdate = DateTime.Now;
				RaiseEvent_StateUpdated("SourceIpPort");
			}
		}
		public void UpdateExecutorId(string ExecutorId)
		{
			if (!string.IsNullOrEmpty(ExecutorId) && mExecutorId != ExecutorId)
			{
				mExecutorId = ExecutorId;
				mLastUpdate = DateTime.Now;
				RaiseEvent_StateUpdated("ExecutorId");
			}
		}
		public void UpdateSendState(SendState SendState)
		{
			if (mSendState != SendState)
			{
				mSendState = SendState;
				mLastUpdate = DateTime.Now;
				RaiseEvent_StateUpdated("SendState");
			}
		}
		public void UpdateExecuteState(ExecuteState ExecuteState)
		{
			if (mExecuteState != ExecuteState)
			{
				mExecuteState = ExecuteState;
				if (mExecuteState == ExecuteState.Executing)
				{
					mExecutionStartTimestamp = DateTime.Now;
					mLastUpdate = DateTime.Now;
					RaiseEvent_StateUpdated("ExecuteState,ExecutionStartTimestamp");
				}
				else if (mExecuteState == ExecuteState.ExecuteSuccessed || mExecuteState == ExecuteState.ExecuteFailed)
				{
					mExecutionStopTimestamp = DateTime.Now;
					mLastUpdate = DateTime.Now;
					RaiseEvent_StateUpdated("ExecuteState,ExecutionStopTimestamp");
				}
			}
		}
		public string[] ToStringArray()
		{
			string[] result = null;
			result = new string[] { mMissionId, mExecutorId, mSendState.ToString(), mExecuteState.ToString(), mExecutionStartTimestamp.ToString("yyyy/MM/dd HH:mm:ss.fff"), mExecutionStopTimestamp.ToString("yyyy/MM/dd HH:mm:ss.fff"), mLastUpdate.ToString("yyyy/MM/dd HH:mm:ss.fff") };
			return result;
		}

		protected virtual void RaiseEvent_StateUpdated(string StateName, bool Sync = true)
		{
			if (Sync)
			{
				StateUpdated?.Invoke(DateTime.Now, mMissionId, StateName, this);
			}
			else
			{
				Task.Run(() => StateUpdated?.Invoke(DateTime.Now, mMissionId, StateName, this));
			}
		}
	}
}
