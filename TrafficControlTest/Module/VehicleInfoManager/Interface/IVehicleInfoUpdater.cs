using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Interface
{
	/// <summary>
	/// - Reference: IVehicleCommunicator, IMissionStateManager, IVehicleInfoManager
	/// - 根據 IVehicleCommunicator 的 ReceivedData 事件來使用 IVehicleInfoManager 的 Add(), Update() 方法
	/// - 根據 IVehicleCommunicator 的 ConnectStateChanged 事件來使用 IVehicleInfoManager 的 Remove() 方法
	/// - 根據 IMissionStateManager 的 ItemStateUpdated 事件來使用 IVehicleInfoManager 的 UpdateCurrentMissionId() 方法
	/// </summary>
	public interface IVehicleInfoUpdater
	{
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator, IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager);
	}
}
