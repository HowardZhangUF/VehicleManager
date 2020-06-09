using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Mission
{
	/// <summary>
	/// - Reference: IVehicleInfoManager, IMissionStateManager
	/// - 監控 IMissionStateManager 的 ItemUpdated 事件，當有任務完成時更新 IMissionStateManager 的清單
	/// - 定期使用 IVehicleInfoManager 的資訊來更新任務的傳送/執行狀態
	/// </summary>
	public interface IMissionUpdater : ISystemWithLoopTask
	{
		int mToleranceOfX { get; }
		int mToleranceOfY { get; }
		int mToleranceOfToward { get; }

		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IMapManager MapManager);
		void Set(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMapManager MapManager);
	}
}
