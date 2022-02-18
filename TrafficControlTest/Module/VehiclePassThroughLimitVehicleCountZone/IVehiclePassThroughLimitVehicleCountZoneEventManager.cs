using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.VehiclePassThroughAutomaticDoor;

namespace TrafficControlTest.Module.VehiclePassThroughLimitVehicleCountZone
{
	public interface IVehiclePassThroughLimitVehicleCountZoneEventManager : IItemManager<IVehiclePassThroughLimitVehicleCountZoneEvent>
	{
		bool IsExist(string VehicleName, string LimitVehicleCountZoneName);
		void UpdateDistance(string VehicleName, string LimitVehicleCountZoneName, int Distance);
		void UpdateState(string VehicleName, string LimitVehicleCountZoneName, PassThroughState State);
	}
}
