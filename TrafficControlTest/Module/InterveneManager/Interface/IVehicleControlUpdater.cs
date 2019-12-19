using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;

namespace TrafficControlTest.Module.InterveneManager.Interface
{
	/// <summary>
	/// Reference: IVehicleControlManager, IVehicleInfoManager, IVehicleCommunicator
	/// - 根據 VehicleControl 的 ItemUpdated 事件當 SendState 變成 SendSuccessed/SendFailed 時使用 VehicleControlManager 的 Remove() 方法將 VehicleControl 移除
	/// - 根據 VehicleCommunicator 的 SendDataSuccessed 事件來更新 VehicleControl 的 SendState 成 SendSuccessed
	/// - 根據 VehicleCommunicator 的 SendDataFailed 事件來更新 VehicleControl 的 SendState 成 SendFailed
	/// </summary>
	public interface IVehicleControlUpdater
	{
		void Set(IVehicleControlManager VehicleControlManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator);
	}
}
