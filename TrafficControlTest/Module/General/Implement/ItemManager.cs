using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Implement
{
	// 相同的東西有 add, remove, updated 事件， get(), getNames(), getList(), add(), remove() 方法
	// 不同的有 update() 方法
	// 不同的部分就特別使用 public interface IVehicleInfoManager : IItemManager<IVehicleInfo> 方法去額外宣告
	public class ItemManager<T> : IItemManager<T> where T : IItem
	{
		public event EventHandlerItem<T> ItemAdded;
		public event EventHandlerItem<T> ItemRemoved;
		public event EventHandlerItemUpdated<T> ItemUpdated;

		public int mCount { get { return mItems.Count; } }

		protected Dictionary<string, T> mItems = new Dictionary<string, T>();
		protected readonly object mLock = new object();

		public ItemManager() { }
		public bool IsExist(string Name)
		{
			return mItems.Keys.Contains(Name);
		}
		public T GetItem(string Name)
		{
			return mItems.Keys.Contains(Name) ? mItems[Name] : default(T);
		}
		public IEnumerable<T> GetItems()
		{
			return mItems.Values;
		}
		public IEnumerable<string> GetItemNames()
		{
			return mItems.Keys;
		}
		public bool Add(string Name, T Item)
		{
			bool result = false;
			lock (mLock)
			{
				if (!mItems.Keys.Contains(Name))
				{
					mItems.Add(Name, Item);
					SubscribeEvent_Item(Item);
					RaiseEvent_ItemAdded(Name, mItems[Name]);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
		public bool Remove(string Name)
		{
			bool result = false;
			lock (mLock)
			{
				if (mItems.Keys.Contains(Name))
				{
					T tmpData = mItems[Name];
					UnsubscribeEvent_Item(mItems[Name]);
					mItems.Remove(Name);
					RaiseEvent_ItemRemoved(Name, tmpData);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		private void SubscribeEvent_Item(T Item)
		{
			if (Item != null)
			{
				Item.Updated += HandleEvent_ItemUpdated;
			}
		}
		private void UnsubscribeEvent_Item(T Item)
		{
			if (Item != null)
			{
				Item.Updated -= HandleEvent_ItemUpdated;
			}
		}
		protected virtual void RaiseEvent_ItemAdded(string Name, T Item, bool Sync = true)
		{
			if (Sync)
			{
				ItemAdded?.Invoke(DateTime.Now, Name, Item);
			}
			else
			{
				Task.Run(() => { ItemAdded?.Invoke(DateTime.Now, Name, Item); });
			}
		}
		protected virtual void RaiseEvent_ItemRemoved(string Name, T Item, bool Sync = true)
		{
			if (Sync)
			{
				ItemRemoved?.Invoke(DateTime.Now, Name, Item);
			}
			else
			{
				Task.Run(() => { ItemRemoved?.Invoke(DateTime.Now, Name, Item); });
			}
		}
		protected virtual void RaiseEvent_ItemUpdated(string Name, string StateName, T Item, bool Sync = true)
		{
			if (Sync)
			{
				ItemUpdated?.Invoke(DateTime.Now, Name, StateName, Item);
			}
			else
			{
				Task.Run(() => { ItemUpdated?.Invoke(DateTime.Now, Name, StateName, Item); });
			}
		}
		private void HandleEvent_ItemUpdated(DateTime OccurTime, string Name, string StateName)
		{
			RaiseEvent_ItemUpdated(Name, StateName, mItems[Name]);
		}
	}
}
