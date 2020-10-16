using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.General
{
	public abstract class ItemManager<T> : IItemManager<T> where T : IItem
	{
		public event EventHandler<ItemCountChangedEventArgs<T>> ItemAdded;
		public event EventHandler<ItemCountChangedEventArgs<T>> ItemRemoved;
		public event EventHandler<ItemUpdatedEventArgs<T>> ItemUpdated;

		public T this[string Name] { get { return GetItem(Name); } }
		public int mCount { get { return mItems.Count; } }

		protected Dictionary<string, T> mItems = new Dictionary<string, T>();
		protected readonly object mLock = new object();

		public ItemManager() { }
		public bool IsExist(string ItemName)
		{
			return mItems.Keys.Contains(ItemName);
		}
		public T GetItem(string ItemName)
		{
			return mItems.Keys.Contains(ItemName) ? mItems[ItemName] : default(T);
		}
		public IEnumerable<T> GetItems()
		{
			return mItems.Values.ToArray();
		}
		public IEnumerable<string> GetItemNames()
		{
			return mItems.Keys.ToArray();
		}
		public bool Add(string ItemName, T Item)
		{
			bool result = false;
			lock (mLock)
			{
				if (!mItems.Keys.Contains(ItemName))
				{
					mItems.Add(ItemName, Item);
					SubscribeEvent_Item(Item);
					RaiseEvent_ItemAdded(ItemName, mItems[ItemName]);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
		public bool Remove(string ItemName)
		{
			bool result = false;
			lock (mLock)
			{
				if (mItems.Keys.Contains(ItemName))
				{
					T tmpData = mItems[ItemName];
					UnsubscribeEvent_Item(mItems[ItemName]);
					mItems.Remove(ItemName);
					RaiseEvent_ItemRemoved(ItemName, tmpData);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
		public void RemoveAll()
		{
			while (mItems.Any())
			{
				Remove(mItems.Keys.First());
			}
		}

		private void SubscribeEvent_Item(T Item)
		{
			if (Item != null)
			{
				Item.StatusUpdated += HandleEvent_ItemStatusUpdated;
			}
		}
		private void UnsubscribeEvent_Item(T Item)
		{
			if (Item != null)
			{
				Item.StatusUpdated -= HandleEvent_ItemStatusUpdated;
			}
		}
		protected virtual void RaiseEvent_ItemAdded(string ItemName, T Item, bool Sync = true)
		{
			if (Sync)
			{
				ItemAdded?.Invoke(this, new ItemCountChangedEventArgs<T>(DateTime.Now, ItemName, Item));
			}
			else
			{
				Task.Run(() => { ItemAdded?.Invoke(this, new ItemCountChangedEventArgs<T>(DateTime.Now, ItemName, Item)); });
			}
		}
		protected virtual void RaiseEvent_ItemRemoved(string ItemName, T Item, bool Sync = true)
		{
			if (Sync)
			{
				ItemRemoved?.Invoke(this, new ItemCountChangedEventArgs<T>(DateTime.Now, ItemName, Item));
			}
			else
			{
				Task.Run(() => { ItemRemoved?.Invoke(this, new ItemCountChangedEventArgs<T>(DateTime.Now, ItemName, Item)); });
			}
		}
		protected virtual void RaiseEvent_ItemUpdated(string ItemName, string StatusName, T Item, bool Sync = true)
		{
			if (Sync)
			{
				ItemUpdated?.Invoke(this, new ItemUpdatedEventArgs<T>(DateTime.Now, ItemName, StatusName, Item));
			}
			else
			{
				Task.Run(() => { ItemUpdated?.Invoke(this, new ItemUpdatedEventArgs<T>(DateTime.Now, ItemName, StatusName, Item)); });
			}
		}
		private void HandleEvent_ItemStatusUpdated(object Sender, StatusUpdatedEventArgs Args)
		{
			RaiseEvent_ItemUpdated(Args.ItemName, Args.StatusName, mItems[Args.ItemName]);
		}
	}
}
