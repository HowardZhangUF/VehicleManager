﻿using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.InterveneCommand;

namespace TrafficControlTest.Module.VehiclePassThroughLimitVehicleCountZone
{
	/// <summary>
	/// - 每當有 IVehiclePassThroughLimitVehicleCountZoneEvent 發生/結束時，進行/解除干預
	/// </summary>
	public interface IVehiclePassThroughLimitVehicleCountZoneEventHandler : ISystemWithLoopTask
	{
		void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager);
		void Set(IVehicleControlManager VehicleControlManager);
		void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleControlManager VehicleControlManager);
	}
}
