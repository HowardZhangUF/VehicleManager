using System.Collections.Generic;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Implement
{
	/// <summary>
	/// - 儲存所有的 IItem
	/// - 提供 Add(), Remove(), IsExist(), GetItem(), GetItems(), GetItemNames(), mCount 等方法、屬性供外部使用
	/// - 當物件的資訊新增、移除、資訊更新時發生時會拋出事件
	/// </summary>
	public interface IItemManager<T> where T : IItem
	{
		event EventHandlerItem<T> ItemAdded;
		event EventHandlerItem<T> ItemRemoved;
		event EventHandlerItemUpdated<T> ItemUpdated;

		int mCount { get; }

		bool IsExist(string Name);
		T GetItem(string Name);
		IEnumerable<T> GetItems();
		IEnumerable<string> GetItemNames();
		bool Add(string Name, T Item);
		bool Remove(string Name);
	}
}