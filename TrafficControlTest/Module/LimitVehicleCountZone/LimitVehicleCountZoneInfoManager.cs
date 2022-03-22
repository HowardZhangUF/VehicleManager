using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public class LimitVehicleCountZoneInfoManager : ItemManager<ILimitVehicleCountZoneInfo>, ILimitVehicleCountZoneInfoManager
	{
		public void UpdateCurrentVehicleNameList(string Name, List<Tuple<string, DateTime>> CurrentVehicleNameList)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateCurrentVehicleNameList(CurrentVehicleNameList);
			}
		}
	}
}
