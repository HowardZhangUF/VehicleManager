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
		bool IsExist(string Name);
		/// <summary>檢查指定 Vehicle 是否在線上(透過 Vehicle IP:Port)</summary>
		bool IsExistByIpPort(string IpPort);
		/// <summary>取得指定 Vehicle 的資訊(透過 Vehicle 名稱)</summary>
		IVehicleInfo this[string Name] { get; }
		/// <summary>取得指定 Vehicle 的資訊(透過 Vehicle 名稱)</summary>
		IVehicleInfo Get(string Name);
		/// <summary>取得指定 Vehicle 的資訊(透過 Vehicle IP:Port)</summary>
		IVehicleInfo GetByIpPort(string IpPort);
		/// <summary>取得線上 Vehicle 的清單</summary>
		List<string> GetNames();
		/// <summary>取得線上 Vehicle 的資訊</summary>
		List<IVehicleInfo> GetList();
		/// <summary>新增 Vehicle 資訊</summary>
		void Add(string Name, IVehicleInfo Data);
		/// <summary>移除 Vehicle 資訊</summary>
		void Remove(string Name);
		/// <summary>更新指定 Vehicle 的資訊</summary>
		void Update(string Name, string State, IPoint2D Position, double Toward, double Battery, double Velocity, string Target, string AlarmMessage, bool IsInterveneAvailable, bool IsIntervene, string InterveneCommand);
		/// <summary>更新指定 Vehicle 的資訊</summary>
		void Update(string Name, IEnumerable<IPoint2D> Path);
		/// <summary>更新指定 Vehicle 的資訊</summary>
		void Update(string Name, string IpPort);
	}
}
