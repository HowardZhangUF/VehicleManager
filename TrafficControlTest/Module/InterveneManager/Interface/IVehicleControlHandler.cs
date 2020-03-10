﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Interface
{
	/// <summary>
	/// - Reference: IVehicleControlManager, IVehicleInfoManager, IVehicleCommunicator
	/// - 當 IVehicleControlManager 裡面有東西時，將其發送給指定的車
	/// </summary>
	public interface IVehicleControlHandler : ISystemWithLoopTask
	{
		void Set(IVehicleControlManager VehicleControlManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator);
	}
}
