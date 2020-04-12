using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.Dashboard
{
	public class VehicleBatteryState
	{
		public DateTime mTimestamp = default(DateTime);
		public double mBatteryValue = default(double);

		public VehicleBatteryState(DateTime Timestamp, double BatteryValue)
		{
			mTimestamp = Timestamp;
			mBatteryValue = BatteryValue;
		}
	}
}
