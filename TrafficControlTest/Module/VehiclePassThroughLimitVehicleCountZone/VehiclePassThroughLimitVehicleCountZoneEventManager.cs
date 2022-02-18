using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.VehiclePassThroughAutomaticDoor;

namespace TrafficControlTest.Module.VehiclePassThroughLimitVehicleCountZone
{
	public class VehiclePassThroughLimitVehicleCountZoneEventManager : ItemManager<IVehiclePassThroughLimitVehicleCountZoneEvent>, IVehiclePassThroughLimitVehicleCountZoneEventManager
	{
		public VehiclePassThroughLimitVehicleCountZoneEventManager()
		{

		}
		public bool IsExist(string VehicleName, string LimitVehicleCountZoneName)
		{
			return mItems.Any(o => o.Value.rVehicleInfo.mName == VehicleName && o.Value.rLimitVehicleCountZoneInfo.mName == LimitVehicleCountZoneName);
		}
		public void UpdateDistance(string VehicleName, string LimitVehicleCountZoneName, int Distance)
		{
			if (IsExist(VehicleName, LimitVehicleCountZoneName))
			{
				mItems.First(o => o.Value.rVehicleInfo.mName == VehicleName && o.Value.rLimitVehicleCountZoneInfo.mName == LimitVehicleCountZoneName).Value.UpdateDistance(Distance);
			}
		}
		public void UpdateState(string VehicleName, string LimitVehicleCountZoneName, PassThroughState State)
		{
			if (IsExist(VehicleName, LimitVehicleCountZoneName))
			{
				mItems.First(o => o.Value.rVehicleInfo.mName == VehicleName && o.Value.rLimitVehicleCountZoneInfo.mName == LimitVehicleCountZoneName).Value.UpdateState(State);
			}
		}
	}
}
