using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Mission
{
	/// <summary>
	/// - Reference: IVehicleCommunicator, IVehicleInfoManager, IMissionStateManager
	/// - 監控 IVehicleCommunicator 的 SendSuccessed 事件，來更新 IMissionStateManager 裡面的任務的 Send 與 Execute 狀態
	/// - 監控 IVehicleCommunicator 的 SendFailed 事件，來更新 IMissionStateManager 裡面的任務的 Send 狀態
	/// - 監控 IVehicleInfoManager 的 ItemUpdated 事件，來更新 IMIssionStateManager 裡面的任務的 Execute 狀態
	/// - 監控 IMissionStateManager 的 ItemUpdated 事件，當有任務完成時更新 IMissionStateManager 的清單
	/// </summary>
	public interface IMissionUpdater
	{
		int mToleranceOfX { get; }
		int mToleranceOfY { get; }
		int mToleranceOfToward { get; }

		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IMapManager MapManager);
		void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMapManager MapManager);
	}
}
