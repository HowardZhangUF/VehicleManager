using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.Dashboard
{
	public class DailyMissionCount
	{
		public DateTime mDate { get; private set; } = default(DateTime);
		public int mSuccessedCount { get; private set; } = default(int);
		public int mFailedCount { get; private set; } = default(int);
		public int mTotalCount { get { return mSuccessedCount + mFailedCount; } }

		public DailyMissionCount(DateTime Date, int SuccessedCount, int FailedCount)
		{
			mDate = Date.Date;
			SetSuccessedCount(SuccessedCount);
			SetFailedCount(FailedCount);
		}
		public void SetSuccessedCount(int SuccessedCount)
		{
			mSuccessedCount = SuccessedCount;
		}
		public void SetFailedCount(int FailedCount)
		{
			mFailedCount = FailedCount;
		}
	}
}
