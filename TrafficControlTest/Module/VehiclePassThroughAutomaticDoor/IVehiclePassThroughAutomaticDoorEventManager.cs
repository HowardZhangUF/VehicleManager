using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.VehiclePassThroughAutomaticDoor
{
	public interface IVehiclePassThroughAutomaticDoorEventManager : IItemManager<IVehiclePassThroughAutomaticDoorEvent>
	{
		void UpdateDistance(string Name, int Distance);
		void UpdateIsHandled(string Name, bool IsHandled);
		void UpdateState(string Name, PassThroughState State);
	}
}
