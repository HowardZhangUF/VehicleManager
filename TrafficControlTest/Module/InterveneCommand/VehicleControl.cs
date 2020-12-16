using System;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.InterveneCommand
{
	public class VehicleControl : IVehicleControl
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; }
		public string mVehicleId { get; private set; }
		public Command mCommand { get; private set; }
		public string[] mParameters { get; private set; }
        public string mParametersString
        {
            get
            {
                if (mParameters == null || mParameters.Length == 0)
                {
                    return string.Empty;
                }
                else if (mParameters.Length == 1)
                {
                    // 參數只有一個，代表是 Goal 點
                    return mParameters[0];
                }
                else
                {
                    // 參數有多個，可能為 X + Y 或是 X + Y + Toward
                    return $"({string.Join(",", mParameters)})";
                }
            }
        }
        public string mCauseId { get; private set; }
		public string mCauseDetail { get; private set; }
		public SendState mSendState { get; private set; }
		public ExecuteState mExecuteState { get; private set; }
		public FailedReason mFailedReason { get; private set; }
        public DateTime mReceivedTimestamp { get; private set; }
		public DateTime mExecutionStartTimestamp { get; private set; }
		public DateTime mExecutionStopTimestamp { get; private set; }
		public DateTime mLastUpdated { get; private set; }

		public VehicleControl(string VehicleId, Command Command, string[] Parameters, string CauseId, string CauseDetail)
		{
			Set(VehicleId, Command, Parameters, CauseId, CauseDetail);
		}
		public void Set(string VehicleId, Command Command, string[] Parameters, string CauseId, string CauseDetail)
		{
            DateTime tmp = DateTime.Now;
			mName = $"Control{tmp.ToString("yyyyMMddHHmmssfff")}";
			mVehicleId = VehicleId;
			mCommand = Command;
			mParameters = Parameters;
			mCauseId = CauseId;
			mCauseDetail = CauseDetail;
			mSendState = SendState.Unsend;
			mExecuteState = ExecuteState.Unexecute;
            mReceivedTimestamp = tmp;
			mExecutionStartTimestamp = DateTime.MinValue;
			mExecutionStopTimestamp = DateTime.MinValue;
			mLastUpdated = tmp;
			RaiseEvent_StatusUpdated("Name,VehicleId,Command,Parameters,CauseId,CauseDetail");
		}
		public void UpdateSendState(SendState SendState)
		{
			if (mSendState != SendState)
			{
				mSendState = SendState;
				mLastUpdated = DateTime.Now;
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
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("ExecuteState,ExecutionStartTimestamp");
				}
				else if (mExecuteState == ExecuteState.ExecuteSuccessed || mExecuteState == ExecuteState.ExecuteFailed)
				{
					mExecutionStopTimestamp = DateTime.Now;
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("ExecuteState,ExecutionStopTimestamp");
				}
				else
				{
					RaiseEvent_StatusUpdated("ExecuteState");
				}
			}
		}
		public void UpdateExecuteFailedReason(FailedReason ExecuteFailedReason)
		{
			if (mFailedReason != ExecuteFailedReason)
			{
				mFailedReason = ExecuteFailedReason;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("FailedReason");
			}
		}
		public override string ToString()
		{
			return $"{mName}/{mVehicleId}/{mCommand.ToString()}/{(mParameters != null ? string.Join(",", mParameters) : string.Empty)}/{mCauseId}/{mSendState.ToString()}/{mExecuteState.ToString()}";
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
