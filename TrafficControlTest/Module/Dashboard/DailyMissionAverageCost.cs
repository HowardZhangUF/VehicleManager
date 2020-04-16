using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.Dashboard
{
	public class DailyMissionAverageCost
	{
		public DateTime mDate { get; private set; } = default(DateTime);
		public int mSuccessedMissionCount { get; private set; } = default(int);
		public double mAverageCostInSec { get; private set; } = default(double);

		public DailyMissionAverageCost(DateTime Date, int SuccessedMissionCount, double AverageCostInSec)
		{
			mDate = Date.Date;
			SetSuccessedMissionCount(SuccessedMissionCount);
			SetAverageCostInSec(AverageCostInSec);
		}
		public void SetSuccessedMissionCount(int SuccessedMissionCount)
		{
			mSuccessedMissionCount = SuccessedMissionCount;
		}
		public void SetAverageCostInSec(double AverageCostInSec)
		{
			mAverageCostInSec = AverageCostInSec;
		}
	}
}
