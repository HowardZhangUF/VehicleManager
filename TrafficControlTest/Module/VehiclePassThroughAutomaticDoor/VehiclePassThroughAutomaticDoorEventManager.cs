using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.VehiclePassThroughAutomaticDoor
{
	public class VehiclePassThroughAutomaticDoorEventManager : ItemManager<IVehiclePassThroughAutomaticDoorEvent>, IVehiclePassThroughAutomaticDoorEventManager
	{
		public void UpdateDistance(string Name, int Distance)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateDistance(Distance);
			}
		}
		public void UpdateIsHandled(string Name, bool IsHandled)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateIsHandled(IsHandled);
			}
		}
		public void UpdateState(string Name, PassThroughState State)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateState(State);
			}
		}
	}
}
