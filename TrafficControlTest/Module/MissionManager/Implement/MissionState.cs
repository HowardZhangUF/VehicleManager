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
		public event EventHandlerIItemUpdated Updated;

		public IMission mMission { get; private set; }
		public string mSourceIpPort { get; private set; }
		public string mName { get; private set; }
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
			mName = string.IsNullOrEmpty(Mission.mMissionId) ? $"Mission{DateTime.Now.ToString("yyyyMMddHHmmssfff")}" : Mission.mMissionId;
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
			result = new string[] { mName, mMission.mPriority.ToString(), mMission.mMissionType.ToString(), mMission.mVehicleId, mMission.mParameters == null ? string.Empty : string.Join(", ", mMission.mParameters), $"{mSendState.ToString()} / {mExecuteState.ToString()}", mExecutorId, mSourceIpPort, mReceivedTimestamp.ToString("yyyy/MM/dd HH:mm:ss.fff") };
			return result;
		}

		protected virtual void RaiseEvent_StateUpdated(string StateName, bool Sync = true)
		{
			if (Sync)
			{
				Updated?.Invoke(DateTime.Now, mName, StateName);
			}
			else
			{
				Task.Run(() => Updated?.Invoke(DateTime.Now, mName, StateName));
			}
		}
	}
}
