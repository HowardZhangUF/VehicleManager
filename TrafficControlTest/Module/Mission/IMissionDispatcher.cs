using TrafficControlTest.Module.Communication;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Mission
{
	/// <summary>
	/// - Reference: IMissionStateManager, IVehicleInfoManager, IVehicleCommunicator
	/// - 定時檢查 IMissionStateManager 裡面有任務尚未派出的話，透過 IVehicleInfoManager 尋找合適的車子並使用 IVehicleCommunicator 將任務派給車子
	/// </summary>
	public interface IMissionDispatcher : ISystemWithLoopTask
	{
		event EventHandlerMissionDispatched MissionDispatched;

		void Set(IMissionStateManager MissionStateManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator);
	}
}
