using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Interface
{
	/// <summary>
	/// - Reference: IVehicleCommunicator, IVehicleInfoManager
	/// - 根據 IVehicleCommunicator 的 ReceivedData 事件來使用 IVehicleInfoManager 的 Add(), Update() 方法
	/// - 根據 IVehicleCommunicator 的 ConnectStateChanged 事件來使用 IVehicleInfoManager 的 Remove() 方法
	/// </summary>
	public interface IVehicleInfoUpdater
	{
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager);
	}
}
