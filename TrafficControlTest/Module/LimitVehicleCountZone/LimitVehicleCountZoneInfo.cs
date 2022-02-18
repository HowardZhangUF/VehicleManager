using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public class LimitVehicleCountZoneInfo : ILimitVehicleCountZoneInfo
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public IRectangle2D mRange { get; private set; } = null;
		public int mMaxVehicleCount { get; private set; } = 0;
		public List<string> mCurrentVehicleNameList { get; private set; } = new List<string>();
		public List<string> mLastVehicleNameList { get; private set; } = new List<string>();
		public TimeSpan mCurrentStatusDuration { get { return DateTime.Now.Subtract(mTimestampOfStatusChanged); } }
		public DateTime mLastUpdated { get; private set; } = DateTime.Now;

		private DateTime mTimestampOfStatusChanged = DateTime.Now;

		public LimitVehicleCountZoneInfo(string Name, IRectangle2D Range, int MaxVehicleCount)
		{
			Set(Name, Range, MaxVehicleCount);
		}
		public void Set(string Name, IRectangle2D Range, int MaxVehicleCount)
		{
			mName = Name;
			mRange = Range;
			mMaxVehicleCount = MaxVehicleCount;
			mLastUpdated = DateTime.Now;
		}
		public void UpdateCurrentVehicleNameList(List<string> CurrentVehicleNameList)
		{
			if (CurrentVehicleNameList != null && string.Join(",", CurrentVehicleNameList.OrderBy(o => o).ToArray()) != string.Join(",", mCurrentVehicleNameList.OrderBy(o => o).ToArray()))
			{
				mLastVehicleNameList.Clear();
				mLastVehicleNameList.AddRange(mCurrentVehicleNameList);
				// 調整 mCurrentVehicleNameList 的內容但要保留順序
				for (int i = 0; i < CurrentVehicleNameList.Count; ++i)
				{
					if (!mCurrentVehicleNameList.Contains(CurrentVehicleNameList[i]))
					{
						mCurrentVehicleNameList.Add(CurrentVehicleNameList[i]);
					}
				}
				int tmpIndex = 0;
				while (true)
				{
					if (tmpIndex >= mCurrentVehicleNameList.Count) break;

					if (!CurrentVehicleNameList.Contains(mCurrentVehicleNameList[tmpIndex]))
					{
						mCurrentVehicleNameList.RemoveAt(tmpIndex);
					}
					else
					{
						tmpIndex++;
					}
				}
				mTimestampOfStatusChanged = DateTime.Now;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("CurrentVehicleNameList,LastVehicleNameList,CurrentStatusDuration");
			}
		}
		public override string ToString()
		{
			return $"{mName}/{mRange.ToString()}/Max:{mMaxVehicleCount}/Current:{string.Join(",", mCurrentVehicleNameList.ToArray())}/Last:{string.Join(",", mLastVehicleNameList.ToArray())}/Duration:{mCurrentStatusDuration.TotalMilliseconds}(ms)/{mLastUpdated.ToString("yyyy/MM/dd HH:mm:ss.fff")}";
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
