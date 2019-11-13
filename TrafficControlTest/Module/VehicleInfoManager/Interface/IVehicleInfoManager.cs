using System.Collections.Generic;
using TrafficControlTest.Module.General.Implement;

namespace TrafficControlTest.Interface
{
	/// <summary>
	/// - 儲存所有的車子的資訊 (IVehicleInfo)
	/// - 提供 Add(), Remove(), IsExist(), GetItem(), GetItems(), GetItemNames(), mCount 等方法、屬性供外部使用
	/// - 當物件的資訊 (IVehicleInfo) 新增、移除、資訊更新時發生時會拋出事件
	/// </summary>
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
