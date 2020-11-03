using System;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Mission
{
	public class MissionState : IMissionState
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public IMission mMission { get; private set; }
		public string mSourceIpPort { get; private set; }
		public string mName { get; private set; }
		public string mExecutorId { get; private set; }
		public SendState mSendState { get; private set; }
		public ExecuteState mExecuteState { get; private set; }
        public MissionFailedReason mFailedReason { get; private set; }
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
			DateTime tmp = DateTime.Now;
			mMission = Mission;
			mName = $"Mission{tmp.ToString("yyyyMMddHHmmssfff")}";
			mExecutorId = string.Empty;
			mSendState = SendState.Unsend;
			mExecuteState = ExecuteState.Unexecute;
            mFailedReason = MissionFailedReason.None;
			mReceivedTimestamp = tmp;
			mExecutionStartTimestamp = DateTime.MinValue;
			mExecutionStopTimestamp = DateTime.MinValue;
			mLastUpdate = tmp;
		}
		public string GetMissionId()
		{
			return !string.IsNullOrEmpty(mMission.mMissionId) ? mMission.mMissionId : mName;
		}
		public void UpdatePriority(int Priority)
		{
			if (mMission.mPriority != Priority)
			{
				mMission.UpdatePriority(Priority);
				mLastUpdate = DateTime.Now;
				RaiseEvent_StatusUpdated("Priority");
			}
		}
		public void UpdateSourceIpPort(string SourceIpPort)
		{
			if (!string.IsNullOrEmpty(SourceIpPort) && mSourceIpPort != SourceIpPort)
			{
				mSourceIpPort = SourceIpPort;
				mLastUpdate = DateTime.Now;
				RaiseEvent_StatusUpdated("SourceIpPort");
			}
		}
		public void UpdateExecutorId(string ExecutorId)
		{
			if (!string.IsNullOrEmpty(ExecutorId) && mExecutorId != ExecutorId)
			{
				mExecutorId = ExecutorId;
				mLastUpdate = DateTime.Now;
				RaiseEvent_StatusUpdated("ExecutorId");
			}
		}
		public void UpdateSendState(SendState SendState)
		{
			if (mSendState != SendState)
			{
				mSendState = SendState;
				mLastUpdate = DateTime.Now;
				RaiseEvent_StatusUpdated("SendState");
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
					RaiseEvent_StatusUpdated("ExecuteState,ExecutionStartTimestamp");
				}
				else if (mExecuteState == ExecuteState.ExecuteSuccessed || mExecuteState == ExecuteState.ExecuteFailed)
				{
					mExecutionStopTimestamp = DateTime.Now;
					mLastUpdate = DateTime.Now;
					RaiseEvent_StatusUpdated("ExecuteState,ExecutionStopTimestamp");
				}
			}
		}
        public void UpdateFailedReason(MissionFailedReason FailedReason)
        {
            if (mFailedReason != FailedReason)
            {
                mFailedReason = FailedReason;
                mLastUpdate = DateTime.Now;
                RaiseEvent_StatusUpdated("FailedReason");
            }
        }
		public string[] ToStringArray()
		{
			string[] result = null;
			result = new string[] { mName, mMission.mMissionId, mMission.mPriority.ToString(), mMission.mMissionType.ToString(), mMission.mVehicleId, mMission.mParametersString, mSourceIpPort, $"{mSendState.ToString()} / {mExecuteState.ToString()}", mExecutorId, mReceivedTimestamp.ToString("yyyy/MM/dd HH:mm:ss.fff") };
			return result;
		}
		public override string ToString()
		{
			return $"{mName}/{mMission.ToString()}/{mExecutorId}/{mSendState.ToString()}/{mExecuteState.ToString()}";
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
