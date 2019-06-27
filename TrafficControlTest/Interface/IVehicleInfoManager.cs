using System.Collections.Generic;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleInfoManager;

namespace TrafficControlTest.Interface
{
	public interface IVehicleInfoManager
	{
		event EventHandlerIVehicleInfo VehicleAdded;
		event EventHandlerIVehicleInfo VehicleRemoved;
		event EventHandlerIVehicleInfo VehicleStateUpdated;

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
		/// <summary>新增 Vehicle 資訊</summary>
		void AddVehicleInfo(string IpPort, string Name);
		/// <summary>移除 Vehicle 資訊</summary>
		void RemoveVehicleInfo(string IpPort);
	}
}
