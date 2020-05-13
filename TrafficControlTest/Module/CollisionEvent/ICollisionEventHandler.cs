using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CollisionEvent
{
	/// <summary>
	/// - Reference: ICollisionEventManager, IVehicleControlManager, IVehicleInfoManager
	/// - 監控 ICollisionEventManager 的 ItemAdded, ItemUpdated 事件來計算對於 ICollisionPair 的 IVehicleControl(對策)並加入 IVehicleControlManager 中
	/// - 監控 ICollisionEventManager 的 ItemRemoved 事件來移除相關的 IVehicleControl 與恢復受到干預的車
	/// </summary>
	public interface ICollisionEventHandler
	{
		void Set(ICollisionEventManager CollisionEventManager);
		void Set(IVehicleControlManager VehicleControlManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(ICollisionEventManager CollisionEventManager, IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager);
	}
}
