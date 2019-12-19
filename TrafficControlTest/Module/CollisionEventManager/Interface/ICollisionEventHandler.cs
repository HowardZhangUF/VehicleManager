using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Interface
{
	public interface ICollisionEventHandler
	{
		void Set(ICollisionEventManager CollisionEventManager);
		void Set(IVehicleControlManager VehicleControlManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(ICollisionEventManager CollisionEventManager, IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager);
	}
}
