﻿using System;
using System.Collections.Generic;

namespace TrafficControlTest.Module.General
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

		int mCount { get; }

		bool IsExist(string Name);
		T GetItem(string Name);
		IEnumerable<T> GetItems();
		IEnumerable<string> GetItemNames();
		bool Add(string Name, T Item);
		bool Remove(string Name);
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
}