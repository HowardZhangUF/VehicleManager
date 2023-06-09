﻿using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public interface ILimitVehicleCountZoneInfoManager : IItemManager<ILimitVehicleCountZoneInfo>
	{
		void UpdateCurrentVehicleNameList(string Name, List<Tuple<string, DateTime>> CurrentVehicleNameList);
	}
}
