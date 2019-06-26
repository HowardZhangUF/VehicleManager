using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Interface
{
	public interface IVehicleMessageAnalyzer
	{
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager);
	}
}
