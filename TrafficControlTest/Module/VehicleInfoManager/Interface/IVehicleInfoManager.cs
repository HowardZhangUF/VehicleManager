using System.Collections.Generic;
using TrafficControlTest.Module.General.Implement;

namespace TrafficControlTest.Interface
{
	public interface IVehicleInfoManager : IItemManager<IVehicleInfo>
	{
		/// <summary>取得指定 Vehicle 的資訊(透過 Vehicle 名稱)</summary>
		IVehicleInfo this[string Name] { get; }

		/// <summary>檢查指定 Vehicle 是否在線上(透過 Vehicle IP:Port)</summary>
		bool IsExistByIpPort(string IpPort);
		/// <summary>取得指定 Vehicle 的資訊(透過 Vehicle IP:Port)</summary>
		IVehicleInfo GetItemByIpPort(string IpPort);
		/// <summary>更新指定 Vehicle 的資訊</summary>
		void UpdateItem(string Name, string State, IPoint2D Position, double Toward, double Battery, double Velocity, string Target, string AlarmMessage, bool IsInterveneAvailable, bool IsBeingIntervened, string InterveneCommand);
		/// <summary>更新指定 Vehicle 的資訊</summary>
		void UpdateItem(string Name, IEnumerable<IPoint2D> Path);
		/// <summary>更新指定 Vehicle 的資訊</summary>
		void UpdateItem(string Name, string IpPort);
	}
}
