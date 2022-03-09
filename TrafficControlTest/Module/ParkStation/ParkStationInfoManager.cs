using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.ParkStation
{
	public class ParkStationInfoManager : ItemManager<IParkStationInfo>, IParkStationInfoManager // 基本上與 ChargeStationInfoManager 內容一樣
	{
		public void UpdateEnable(string Name, bool Enable)
		{
			lock (mLock)
			{
				if (mItems.Keys.Contains(Name))
				{
					mItems[Name].UpdateEnable(Enable);
				}
			}
		}
		public void UpdateIsBeingUsed(string Name, bool IsBeingUsed)
		{
			lock (mLock)
			{
				if (mItems.Keys.Contains(Name))
				{
					mItems[Name].UpdateIsBeingUsed(IsBeingUsed);
				}
			}
		}
	}
}
