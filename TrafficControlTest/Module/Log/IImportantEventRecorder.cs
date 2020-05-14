using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	/// <summary>
	/// - Reference: IEventRecorder, IVehicleInfoManager, IMissionStateManager
	/// - 每當 IVehicleInfoManager 發生 ItemAdded, ItemRemoved, ItemUpdated 事件時，使用 IEventRecorder 將資料記錄至資料庫(儲存當前車子狀態，以供網頁即時顯示車子狀態)
	/// - 每當 IMissionStateManager 發生 ItemAdded, ItemUpdated 事件時，使用 IEventRecorder 將資料記錄至資料庫 (供快速查詢歷史任務)
	/// - 每當 IHostCommunicator 發生 SentString, ReceivedString 事件時，使用 IEventRecorder 將資料記錄至資料庫 (供快速查詢與 Host 的通訊歷史)
	/// - 定時從 IVehicleInfoManager 拿取所有車的狀態資訊並使用 IEventRecorder 記錄至資料庫(儲存歷史車子狀態，以供未來重現、查詢問題時使用)
	/// </summary>
	public interface IImportantEventRecorder : ISystemWithLoopTask
	{
		void Set(IEventRecorder EventRecorder);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IHostCommunicator HostCommunicator);
		void Set(IEventRecorder EventRecorder, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IHostCommunicator HostCommunicator);
	}
}
