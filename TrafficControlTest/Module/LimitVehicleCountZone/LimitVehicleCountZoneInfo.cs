using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public class LimitVehicleCountZoneInfo : ILimitVehicleCountZoneInfo
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public IRectangle2D mRange { get; private set; } = null;
		public int mMaxVehicleCount { get; private set; } = 0;
		public bool mIsUnioned { get; private set; } = false;
		public int mUnionId { get; private set; } = 0;
		public List<Tuple<string, DateTime>> mCurrentVehicleNameList { get; private set; } = new List<Tuple<string, DateTime>>();
		public List<Tuple<string, DateTime>> mLastVehicleNameList { get; private set; } = new List<Tuple<string, DateTime>>();
		public TimeSpan mCurrentStatusDuration { get { return DateTime.Now.Subtract(mTimestampOfStatusChanged); } }
		public DateTime mLastUpdated { get; private set; } = DateTime.Now;

		private DateTime mTimestampOfStatusChanged = DateTime.Now;

		public LimitVehicleCountZoneInfo(string Name, IRectangle2D Range, int MaxVehicleCount, bool IsUnioned, int UnionId)
		{
			Set(Name, Range, MaxVehicleCount, IsUnioned, UnionId);
		}
		public void Set(string Name, IRectangle2D Range, int MaxVehicleCount, bool IsUnioned, int UnionId)
		{
			mName = Name;
			mRange = Range;
			mMaxVehicleCount = MaxVehicleCount;
			mIsUnioned = IsUnioned;
			mUnionId = UnionId;
			mLastUpdated = DateTime.Now;
		}
		public void UpdateCurrentVehicleNameList(List<string> CurrentVehicleNameList)
		{
			// 輸入參數 CurrentVehicleNameList 為新名單，類別屬性 mCurrentVehicleNameList 為目前名單

			// 新名單不為 null ，且新名單的內容與目前名單的內容不一樣時
			if (CurrentVehicleNameList != null && string.Join(",", CurrentVehicleNameList.OrderBy(o => o).ToArray()) != string.Join(",", mCurrentVehicleNameList.Select(o => o.Item1).OrderBy(o => o).ToArray()))
			{
				mLastVehicleNameList.Clear();
				mLastVehicleNameList.AddRange(mCurrentVehicleNameList);
				
				// 若新名單的車不在目前名單裡面，代表有車進入，則加入該車至目前名單
				for (int i = 0; i < CurrentVehicleNameList.Count; ++i)
				{
					if (!mCurrentVehicleNameList.Any(o => o.Item1 == CurrentVehicleNameList[i]))
					{
						mCurrentVehicleNameList.Add(new Tuple<string, DateTime>(CurrentVehicleNameList[i], DateTime.Now));
					}
				}
				// 若目前名單的車不在新名單裡面，代表有車離開，則從目前名單移除該車
				int tmpIndex = 0;
				while (true)
				{
					if (tmpIndex >= mCurrentVehicleNameList.Count) break;

					if (!CurrentVehicleNameList.Contains(mCurrentVehicleNameList[tmpIndex].Item1))
					{
						mCurrentVehicleNameList.RemoveAt(tmpIndex);
					}
					else
					{
						tmpIndex++;
					}
				}

				// 將目前名單按照進入時間排序
				mCurrentVehicleNameList = mCurrentVehicleNameList.OrderBy(o => o.Item2).ToList();

				mTimestampOfStatusChanged = DateTime.Now;
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated("CurrentVehicleNameList,LastVehicleNameList,CurrentStatusDuration");
			}
		}
		public override string ToString()
		{
			return $"{mName}/{mRange.ToString()}/Max:{mMaxVehicleCount}/IsUnioned:{mIsUnioned}/UnionId:{mUnionId}/Current:{string.Join(",", mCurrentVehicleNameList.Select(o => o.Item1).ToArray())}/Last:{string.Join(",", mLastVehicleNameList.Select(o => o.Item1).ToArray())}/Duration:{mCurrentStatusDuration.TotalMilliseconds}(ms)/{mLastUpdated.ToString("yyyy/MM/dd HH:mm:ss.fff")}";
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
