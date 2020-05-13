using System;
using System.Threading.Tasks;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.InterveneCommand
{
	public class VehicleControl : IVehicleControl
	{
		public event EventHandlerIItemUpdated Updated;

		public string mName { get; private set; }
		public string mVehicleId { get; private set; }
		public SendState mSendState { get; private set; }
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
			mName = $"ControlFor{CauseId}";
			mVehicleId = VehicleId;
			mSendState = SendState.Unsend;
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
		public void UpdateParameters(string[] Parameters)
		{
			mParameters = Parameters;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StateUpdated("Parameters");
		}
		public override string ToString()
		{
			return $"{mName}/{mCommand.ToString()}/{(mParameters != null ? string.Join(",", mParameters) : string.Empty)}/{mVehicleId}/{mCauseId}/{mSendState.ToString()}";
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
