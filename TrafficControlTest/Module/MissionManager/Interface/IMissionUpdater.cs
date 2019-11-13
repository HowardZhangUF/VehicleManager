using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	/// <summary>
	/// - Reference: IVehicleCommunicator, IVehicleInfoManager, IMissionStateManager
	/// - 監控 IVehicleCommunicator 的 SendSuccessed 事件，來更新 IMissionStateManager 裡面的任務的 Send 與 Execute 狀態
	/// - 監控 IVehicleCommunicator 的 SendFailed 事件，來更新 IMissionStateManager 裡面的任務的 Send 狀態
	/// - 監控 IVehicleInfoManager 的 ItemUpdated 事件，來確認 IMIssionStateManager 裡面的任務的 Execute 狀態
	/// </summary>
	public interface IMissionUpdater
	{
		int mToleranceOfX { get; }
		int mToleranceOfY { get; }
		int mToleranceOfToward { get; }

		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager);
	}
}
