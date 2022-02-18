using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public interface ILimitVehicleCountZoneInfoManager : IItemManager<ILimitVehicleCountZoneInfo>
	{
		void UpdateCurrentVehicleNameList(string Name, List<string> CurrentVehicleNameList);
	}
}
