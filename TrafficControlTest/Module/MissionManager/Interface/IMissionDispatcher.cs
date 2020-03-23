using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.MissionManager.Interface
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
