using System;

namespace TrafficControlTest.Module.Dashboard
{
	public class DailyVehicleMissionCount
	{
		public string mVehicleId { get; private set; } = string.Empty;
		public DateTime mDate { get; private set; } = default(DateTime);
		public int mSuccessedMissionCount { get; private set; } = default(int);
		public int mFailedMissionCount { get; private set; } = default(int);
		public int mTotalMissionCount { get { return mSuccessedMissionCount + mFailedMissionCount; } }

		public DailyVehicleMissionCount(string VehicleId, DateTime Date, int SuccessedMissionCount, int FailedMissionCount)
		{
			mVehicleId = VehicleId;
			mDate = Date.Date;
			SetSuccessedMissionCount(SuccessedMissionCount);
			SetFailedMissionCount(FailedMissionCount);
		}
		public void SetSuccessedMissionCount(int SuccessedMissionCount)
		{
			mSuccessedMissionCount = SuccessedMissionCount;
		}
		public void SetFailedMissionCount(int FailedMissionCount)
		{
			mFailedMissionCount = FailedMissionCount;
		}
	}
}
