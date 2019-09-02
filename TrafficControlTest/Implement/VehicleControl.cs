using System;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleControl;

namespace TrafficControlTest.Implement
{
	public class VehicleControl : IVehicleControl
	{
		public event EventHandlerIVehicleControlStateUpdated StateUpdated;

		public string mName { get; private set; }
		public string mVehicleId { get; private set; }
		public SendState mSendState { get; private set; }
		public ExecuteState mExecuteState { get; private set; }
		public Command mCommand { get; private set; }
		public string[] mParameters { get; private set; }
		public string mCauseId { get; private set; }
		public string mCauseDetail { get; private set; }
		public DateTime mExecutionStartTimestamp { get; private set; }
		public DateTime mExecutionStopTimestamp { get; private set; }
		public DateTime mLastUpdated { get; private set; }

		public VehicleControl(string VehicleId, Command Command, string[] Parameters, string CauseId, string CauseDetail)
		{
			Set(VehicleId, Command, Parameters, CauseId, CauseDetail);
		}
		public void Set(string VehicleId, Command Command, string[] Parameters, string CauseId, string CauseDetail)
		{
			mName = $"VehicleControl_{CauseId}";
			mVehicleId = VehicleId;
			mSendState = SendState.Unsend;
			mExecuteState = ExecuteState.Unexecute;
			mCommand = Command;
			mParameters = Parameters;
			mCauseId = CauseId;
			mCauseDetail = CauseDetail;
			mExecutionStartTimestamp = DateTime.Now;
			mExecutionStopTimestamp = DateTime.Now;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StateUpdated("Name,Command,Parameters,CauseId,CauseDetail");
		}
		public void UpdateSendState(SendState SendState)
		{
			if (mSendState != SendState)
			{
				mSendState = SendState;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StateUpdated("SendState");
			}
		}
		public void UpdateExecuteState(ExecuteState ExecuteState)
		{
			if (mExecuteState != ExecuteState)
			{
				mExecuteState = ExecuteState;
				switch (ExecuteState)
				{
					case ExecuteState.Executing:
						mExecutionStartTimestamp = DateTime.Now;
						break;
					case ExecuteState.Completed:
					case ExecuteState.Failed:
					case ExecuteState.Aborted:
					case ExecuteState.Canceled:
						mExecutionStopTimestamp = DateTime.Now;
						break;
				}
				mLastUpdated = DateTime.Now;
				RaiseEvent_StateUpdated("ExecuteState");
			}
		}
		public void UpdateParameters(string[] Parameters)
		{
			mParameters = Parameters;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StateUpdated("Parameters");
		}

		protected virtual void RaiseEvent_StateUpdated(string StateName, bool Sync = true)
		{
			if (Sync)
			{
				StateUpdated?.Invoke(DateTime.Now, mName, StateName, this);
			}
			else
			{
				Task.Run(() => StateUpdated?.Invoke(DateTime.Now, mName, StateName, this));
			}
		}
	}
}
