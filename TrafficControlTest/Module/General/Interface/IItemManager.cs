using System.Collections.Generic;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Implement
{
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