﻿using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;

namespace TrafficControlTest.Module.Vehicle
{
	/// <summary>
	/// - Reference: IVehicleCommunicator, IMissionStateManager, IVehicleInfoManager
	/// - 根據 IVehicleCommunicator 的 ReceivedData 事件來使用 IVehicleInfoManager 的 Add(), Update() 方法
	/// - 根據 IVehicleCommunicator 的 ConnectStateChanged 事件來使用 IVehicleInfoManager 的 Remove() 方法
	/// - 根據 IVehicleCommunicator 的 SentSerializableDataSuccessed 來使用 IVehicleInfoManager 的 UpdateCurrentInterveneCommand() 方法
	/// - 根據 IMissionStateManager 的 ItemStateUpdated 事件來使用 IVehicleInfoManager 的 UpdateCurrentMissionId() 方法
	/// - 根據 IVehicleInfoManager 的 ItemUpdated(Path, CurrentState) 事件來使用 IVehicleInfoManager 的 UpdateCurrentInterveneCommand() 方法
	/// </summary>
	public interface IVehicleInfoUpdater
	{
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IVehicleControlManager VehicleControlManager);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager);
	}
}
