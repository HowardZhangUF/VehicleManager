using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.Dashboard
{
	public class MultiDayVehicleMissionCount
	{
		public string mVehicleId { get; private set; } = string.Empty;
		public DateTime mStartDate { get; private set; } = default(DateTime);
		public DateTime mEndDate { get; private set; } = default(DateTime);
		public List<DailyVehicleMissionCount> mCollection { get; private set; } = new List<DailyVehicleMissionCount>();

		public MultiDayVehicleMissionCount(string VehicleId, params DailyVehicleMissionCount[] DailyVehicleMissionCount)
		{
			mVehicleId = VehicleId;
			mCollection.AddRange(DailyVehicleMissionCount.OrderBy(o => o.mDate));
			mStartDate = mCollection.First().mDate;
			mEndDate = mCollection.Last().mDate;
		}
	}
}
