using System.Collections.Generic;
using TrafficControlTest.Module.General;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CollisionEvent
{
	public interface ICollisionEventManager : IItemManager<ICollisionPair>
	{
		/// <summary>是否存在以 VehicleName1 與 VehicleName2 為原因的 Collision</summary>
		bool IsExist(string VehicleName1, string VehicleName2);
		/// <summary>更新以 VehicleName1 與 VehicleName2 為原因的 Collision 的資訊</summary>
		void Update(string VehicleName1, string VehicleName2, IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy);
	}
}
