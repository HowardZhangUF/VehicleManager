using System.Collections.Generic;
using TrafficControlTest.Module.General;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CollisionEvent
{
	public interface ICollisionEventManager : IItemManager<ICollisionPair>
	{
		/// <summary>取得指定 Collision 的資訊</summary>
		ICollisionPair this[string Name] { get; }

		/// <summary>更新指定 Collision 的資訊</summary>
		void Update(string Name, IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy);
	}
}
