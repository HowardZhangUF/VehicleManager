using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	/// <summary>
	/// - 根據 IMapManager 的 MapChanged 事件來新增/移除 ILimitVehicleCountZoneInfoManager 的成員
	/// - 巡覽 ILimitVehicleCountZoneInfoManager 與 IVehicleInfoManager 並計算每個 Zone 裡面的車數量，並更新至 LimitVehicleCountZoneInfoManager
	/// </summary>
	public interface ILimitVehicleCountZoneInfoManagerUpdater : ISystemWithLoopTask
	{
		void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager);
		void Set(IMapManager MapManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager);
	}
}
