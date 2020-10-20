using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.VehiclePassThroughAutomaticDoor
{
	/// <summary>
	/// - 每當有 IVehiclePassThroughAutomaticDoorEvent 發生時，下達 OpenDoor 控制
	/// - 每當有 IVehiclePassThroughAutomaticDoorEvent 結束且沒有其他車要通過該自動門時，下達 CloseDoor 控制
	/// </summary>
	public interface IVehiclePassThroughAutomaticDoorEventHandler
	{
		void Set(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager);
		void Set(IAutomaticDoorControlManager AutomaticDoorControlManager);
		void Set(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager, IAutomaticDoorControlManager AutomaticDoorControlManager);
	}
}
