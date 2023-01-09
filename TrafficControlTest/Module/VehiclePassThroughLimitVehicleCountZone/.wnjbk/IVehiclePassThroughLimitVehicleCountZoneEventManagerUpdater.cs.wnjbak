using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.LimitVehicleCountZone;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.VehiclePassThroughLimitVehicleCountZone
{
	/// <summary>
	/// - 巡覽 IVehicleInfoManager 與 ILimitVehicleCountZoneInfoManager 並計算穿越事件的發生與解除
	/// </summary>
	public interface IVehiclePassThroughLimitVehicleCountZoneEventManagerUpdater : ISystemWithLoopTask
	{
		void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager);
		void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleInfoManager VehicleInfoManager, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager);
	}
}
