using System;
using System.Collections.Generic;

namespace LibraryForVM
{
	/// <summary>
	/// - 儲存所有的 IItem
	/// - 提供 Add(), Remove(), IsExist(), GetItem(), GetItems(), GetItemNames(), mCount 等方法、屬性供外部使用
	/// - 當物件的資訊新增、移除、資訊更新時發生時會拋出事件
	/// </summary>
	public interface IItemManager<T> where T : IItem
	{
		event EventHandler<ItemCountChangedEventArgs<T>> ItemAdded;
		event EventHandler<ItemCountChangedEventArgs<T>> ItemRemoved;
		event EventHandler<ItemUpdatedEventArgs<T>> ItemUpdated;
		event EventHandler<ItemAddFailedEventArgs<T>> ItemAddFailed;
		event EventHandler<ItemRemoveFailedEventArgs<T>> ItemRemoveFailed;

		T this[string Name] { get; }
		int mCount { get; }

		bool IsExist(string Name);
		T GetItem(string Name);
		IEnumerable<T> GetItems();
		IEnumerable<string> GetItemNames();
		bool Add(string Name, T Item);
		bool Remove(string Name);
		void RemoveAll();
	}

	public class ItemCountChangedEventArgs<T> : EventArgs where T : IItem
	{
		public DateTime OccurTime { get; private set; }
		public string ItemName { get; private set; }
		public T Item { get; private set; }

		public ItemCountChangedEventArgs(DateTime OccurTime, string ItemName, T Item) : base()
		{
			this.OccurTime = OccurTime;
			this.ItemName = ItemName;
			this.Item = Item;
		}
	}
	public class ItemUpdatedEventArgs<T> : EventArgs where T : IItem
	{
		public DateTime OccurTime { get; private set; }
		public string ItemName { get; private set; }
		public string StatusName { get; private set; }
		public T Item { get; private set; }

		public ItemUpdatedEventArgs(DateTime OccurTime, string ItemName, string StatusName, T Item) : base()
		{
			this.OccurTime = OccurTime;
			this.ItemName = ItemName;
			this.StatusName = StatusName;
			this.Item = Item;
		}
	}
	public class ItemAddFailedEventArgs<T> : EventArgs where T : IItem
	{
		public DateTime OccurTime { get; private set; }
		public string ItemName { get; private set; }
		public T Item { get; private set; }

		public ItemAddFailedEventArgs(DateTime OccurTime, string ItemName, T Item) : base()
		{
			this.OccurTime = OccurTime;
			this.ItemName = ItemName;
			this.Item = Item;
		}
	}
	public class ItemRemoveFailedEventArgs<T> : EventArgs where T : IItem
	{
		public DateTime OccurTime { get; private set; }
		public string ItemName { get; private set; }

		public ItemRemoveFailedEventArgs(DateTime OccurTime, string ItemName) : base()
		{
			this.OccurTime = OccurTime;
			this.ItemName = ItemName;
		}
	}
}