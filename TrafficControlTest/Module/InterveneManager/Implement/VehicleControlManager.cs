using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.General.Implement;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Implement
{
	class VehicleControlManager : ItemManager<IVehicleControl>, IVehicleControlManager
	{
		public IVehicleControl this[string Name] { get { return GetItem(Name); } }

		public VehicleControlManager()
		{

		}
		public bool IsExistByCauseId(string CauseId)
		{
			return mItems.Values.Any(o => o.mCauseId == CauseId);
		}
		public IVehicleControl GetItemByCauseId(string CauseId)
		{
			return (IsExistByCauseId(CauseId) ? mItems.Values.First((o) => o.mCauseId == CauseId) : null);
		}
		public void UpdateSendState(string Name, SendState SendState)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateSendState(SendState);
			}
		}
		public void UpdateParameters(string Name, string[] Parameters)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateParameters(Parameters);
			}
		}
	}
}
