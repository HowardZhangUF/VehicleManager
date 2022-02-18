using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.ChargeStation
{
    /// <summary>
	/// - 根據 IMapManager 的 LoadMapSuccessed 事件來新增/移除 IChargeStationInfoManager 的成員
    /// - 使用 IVehicleInfoManager 的 State/Location 資訊來更新 IChargeStationInfoManager 的 IsBeUsing 屬性
    /// </summary>
    public interface IChargeStationInfoManagerUpdater : ISystemWithConfig
    {
        void Set(IChargeStationInfoManager ChargeStationInfoManager);
        void Set(IMapManager MapManager);
        void Set(IVehicleInfoManager VehicleInfoManager);
        void Set(IChargeStationInfoManager ChargeStationInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager);
    }
}
