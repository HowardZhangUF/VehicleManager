using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	public interface IMissionUpdater
	{
		int mToleranceOfX { get; }
		int mToleranceOfY { get; }
		int mToleranceOfToward { get; }

		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager);
	}
}
