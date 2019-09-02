using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleCommunicator;

namespace TrafficControlTest.Interface
{
	public interface IVehicleControlHandler
	{
		event EventHandlerDateTime SystemStarted;
		event EventHandlerDateTime SystemStopped;

		bool mIsExcuting { get; }

		void Set(IVehicleControlManager VehicleControlManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator);
		void Start();
		void Stop();
	}
}
