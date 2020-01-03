using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.DatabaseAdapter;

namespace TrafficControlTest.Module.General.Interface
{
	/// <summary>
	/// - Reference: IEventRecorder, IVehicleInfoManager, IMissionStateManager
	/// - 每當 IVehicleInfoManager 發生 ItemAdded, ItemRemoved, ItemUpdated 事件時，使用 IEventRecorder 將資料記錄至資料庫(儲存當前車子狀態，以供網頁即時顯示車子狀態)
	/// - 每當 IMissionStateManager 發生 ItemAdded, ItemUpdated 事件時，使用 IEventRecorder 將資料記錄至資料庫
	/// - 定時從 IVehicleInfoManager 拿取所有車的狀態資訊並使用 IEventRecorder 記錄至資料庫(儲存歷史車子狀態，以供未來重現、查詢問題時使用)
	/// </summary>
	public interface IImportantEventRecorder
	{
		event EventHandlerDateTime SystemStarted;
		event EventHandlerDateTime SystemStopped;

		bool mIsExecuting { get; }
		int mPeriodOfTask { get; set; }
		int mPeriodOfRecordingHistoryVehicleInfo { get; set; }

		void Set(IEventRecorder EventRecorder);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IEventRecorder EventRecorder, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager);
		void Start();
		void Stop();
	}
}
