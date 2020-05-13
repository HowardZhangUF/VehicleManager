using TrafficControlTest.Module.Communication;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.InterveneCommand
{
	/// <summary>
	/// - Reference: IVehicleControlManager, IVehicleInfoManager, IVehicleCommunicator
	/// - 根據 IVehicleControl 的 ItemUpdated 事件當 SendState 變成 SendSuccessed/SendFailed 時使用 IVehicleControlManager 的 Remove() 方法將 IVehicleControl 移除
	/// - 根據 IVehicleCommunicator 的 SendDataSuccessed 事件來更新 IVehicleControl 的 SendState 成 SendSuccessed
	/// - 根據 IVehicleCommunicator 的 SendDataFailed 事件來更新 IVehicleControl 的 SendState 成 SendFailed
	/// </summary>
	public interface IVehicleControlUpdater
	{
		void Set(IVehicleControlManager VehicleControlManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator);
	}
}
