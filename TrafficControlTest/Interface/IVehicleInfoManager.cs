using SerialData;
using System;
using System.Collections.Generic;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleInfoManager;

namespace TrafficControlTest.Interface
{
	public interface IVehicleInfoManager
	{
		event EventHandlerVehicleInfo VehicleAdded;
		event EventHandlerVehicleInfo VehicleRemoved;
		event EventHandlerVehicleInfo VehicleStatusUpdated;
		event EventHandlerVehicleInfo VehiclePathUpdated;

		/// <summary>設定 IVehicleCommunicator</summary>
		void SetVehicleCommunicator(IVehicleCommunicator VehicleCommunicator);
		/// <summary>檢查指定 Vehicle 是否在線上(透過 Vehicle 名稱)</summary>
		bool IsVehicleExist(string Name);
		/// <summary>檢查指定 Vehicle 是否在線上(透過 Vehicle IP:Port)</summary>
		bool IsVehicleExistByIpPort(string IpPort);
		/// <summary>取得指定 Vehicle 的資訊(透過 Vehicle 名稱)</summary>
		IVehicleInfo this[string Name] { get; }
		/// <summary>取得指定 Vehicle 的資訊(透過 Vehicle 名稱)</summary>
		IVehicleInfo GetVehicleInfo(string Name);
		/// <summary>取得指定 Vehicle 的資訊(透過 Vehicle IP:Port)</summary>
		IVehicleInfo GetVehicleInfoByIpPort(string IpPort);
		/// <summary>取得線上 Vehicle 的清單</summary>
		List<string> GetVehicleNameList();
		/// <summary>取得線上 Vehicle 的資訊</summary>
		List<IVehicleInfo> GetVehicleInfoList();
	}
}
