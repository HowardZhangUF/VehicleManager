using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.InterveneCommand
{
	/// <summary>
	/// - 儲存所有的預計對車子進行的干預 (IVechielControl)
	/// - 提供 Add(), Remove(), IsExist(), GetItem(), GetItems(), GetItemNames(), mCount 等方法、屬性供外部使用
	/// - 當物件的資訊(IVehicleInfo) 新增、移除、資訊更新時發生時會拋出事件
	/// </summary>
	public interface IVehicleControlManager : IItemManager<IVehicleControl>
	{
		/// <summary>檢查指定資料是否存在</summary>
		bool IsExistByCauseId(string CauseId);
		/// <summary>取得指定資料</summary>
		IVehicleControl GetItemByCauseId(string CauseId);
		/// <summary>更新指定資料</summary>
		void UpdateSendState(string Name, SendState SendState);
		/// <summary>更新指定資料</summary>
		void UpdateParameters(string Name, string[] Parameters);
	}
}
