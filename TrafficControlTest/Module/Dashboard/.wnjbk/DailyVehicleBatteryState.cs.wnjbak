using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficControlTest.Module.Dashboard
{
	public class DailyVehicleBatteryState
	{
		public string mVehicleId { get; private set; } = string.Empty;
		public DateTime mDate { get; private set; } = default(DateTime);
		public List<VehicleBatteryState> mCollection { get; } = new List<VehicleBatteryState>();

		public DailyVehicleBatteryState(string VehicleId, DateTime Date)
		{
			mVehicleId = VehicleId;
			mDate = Date.Date;
		}
		public DailyVehicleBatteryState(string VehicleId, DateTime Date, List<DateTime> Timestamps, List<double> BatteryValues)
		{
			mVehicleId = VehicleId;
			mDate = Date.Date;
			for (int i = 0; i < Timestamps.Count; ++i)
			{
				mCollection.Add(new VehicleBatteryState(Timestamps[i], BatteryValues[i]));
			}
			mCollection = mCollection.OrderBy(o => o.mTimestamp).ToList();
		}
		public void AddBatteryState(DateTime Timestamp, double BatteryValue)
		{
			mCollection.Add(new VehicleBatteryState(Timestamp, BatteryValue));
		}
	}
}
