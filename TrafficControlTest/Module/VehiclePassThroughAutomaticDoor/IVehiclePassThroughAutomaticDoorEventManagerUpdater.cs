using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.VehiclePassThroughAutomaticDoor
{
	/// <summary>
	/// - 巡覽 IVehiclePassThroughAutomaticDoorEventManager 裡面的項目/事件是否已經解除，若解除時於該 Mananger 中移除該項目/事件
	/// - 巡覽 IVehicleInfoManager 與 IAutomaticDoorInfoManager 並計算是否將有事件發生，若有時新增/更新該項目/事件至該 Manager 中
	/// </summary>
	public interface IVehiclePassThroughAutomaticDoorEventManagerUpdater : ISystemWithLoopTask
	{
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager);
		void Set(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager);
		void Set(IVehicleInfoManager VehicleInfoManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager);
	}
}
