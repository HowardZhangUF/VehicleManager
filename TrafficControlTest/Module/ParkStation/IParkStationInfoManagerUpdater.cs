using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.ParkStation
{
	public interface IParkStationInfoManagerUpdater : ISystemWithLoopTask // 基本上與 IChargeStationInfoManagerUpdater 內容相似
	{
		void Set(IParkStationInfoManager ParkStationInfoManager);
		void Set(IMapManager MapManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IParkStationInfoManager ParkStationInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager);
	}
}
