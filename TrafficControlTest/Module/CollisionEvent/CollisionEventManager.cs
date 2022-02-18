using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.CollisionEvent
{
	public class CollisionEventManager : ItemManager<ICollisionPair>, ICollisionEventManager
	{
		public CollisionEventManager()
		{
		}
		public bool IsExist(string VehicleName1, string VehicleName2)
		{
			return mItems.Any(o => o.Value.mVehicle1.mName == VehicleName1 && o.Value.mVehicle2.mName == VehicleName2);
		}
		public void Update(string VehicleName1, string VehicleName2, IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy)
		{
			if (IsExist(VehicleName1, VehicleName2))
			{
				mItems.First(o => o.Value.mVehicle1.mName == VehicleName1 && o.Value.mVehicle2.mName == VehicleName2).Value.Update(CollisionRegion, Period, PassPeriodOfVehicle1WithCurrentVelocity, PassPeriodOfVehicle2WithCurrentVelocity, PassPeriodOfVehicle1WithMaximumVeloctiy, PassPeriodOfVehicle2WithMaximumVeloctiy);
			}
		}
	}
}
