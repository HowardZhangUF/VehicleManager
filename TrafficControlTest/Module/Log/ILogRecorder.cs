using LibraryForVM;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	/// <summary>
	/// - Reference: ICurrentLogAdapter, IVehicleInfoManager, IMissionStateManager, IVehicleControlManager, IAutomaticDoorControlManager, IHostCommunicator
	/// - 每當 IVehicleInfoManager 發生 ItemAdded, ItemRemoved, ItemUpdated 事件時，使用 IHistoryLogAdapter 將資料記錄至資料庫(儲存當前車子狀態，以供網頁即時顯示車子狀態)
	/// - 每當 IMissionStateManager 發生 ItemAdded, ItemUpdated 事件時，使用 IHistoryLogAdapter 將資料記錄至資料庫 (供快速查詢歷史任務)
	/// - 每當 IVehicleControlManager 發生 ItemAdded, ItemUpdated 事件時，使用 IHistoryLogAdapter 將資料記錄至資料庫 (供快速查詢歷史任務)
	/// - 每當 IAutomaticDoorControlManager 發生 ItemAdded, ItemUpdated 事件時，使用 IHistoryLogAdapter 將資料記錄至資料庫 (供快速查詢歷史任務)
	/// - 每當 IHostCommunicator 發生 SentString, ReceivedString 事件時，使用 IHistoryLogAdapter 將資料記錄至資料庫 (供快速查詢與 Host 的通訊歷史)
	/// - 定時從 IVehicleInfoManager 拿取所有車的狀態資訊並使用 ICurrentLogAdapter 記錄至資料庫(儲存歷史車子狀態，以供未來重現、查詢問題時使用)
	/// </summary>
	public interface ILogRecorder : ISystemWithLoopTask
	{
		void Set(ICurrentLogAdapter CurrentLogAdapter);
		void Set(IHistoryLogAdapter HistoryLogAdapter);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IVehicleControlManager VehicleControlManager);
		void Set(IAutomaticDoorControlManager AutomaticDoorControlManager);
		void Set(IHostCommunicator HostCommunicator);
		void Set(ICurrentLogAdapter CurrentLogAdapter, IHistoryLogAdapter HistoryLogAdapter, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager, IAutomaticDoorControlManager AutomaticDoorControlManager, IHostCommunicator HostCommunicator);
	}
}
