using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public class LimitVehicleCountZoneInfoManager : ItemManager<ILimitVehicleCountZoneInfo>, ILimitVehicleCountZoneInfoManager
	{
		public void UpdateCurrentVehicleNameList(string Name, List<string> CurrentVehicleNameList)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateCurrentVehicleNameList(CurrentVehicleNameList);
			}
		}
	}
}
