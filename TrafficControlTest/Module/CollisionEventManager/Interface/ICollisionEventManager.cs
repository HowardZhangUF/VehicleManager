using System.Collections.Generic;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Interface
{
	public interface ICollisionEventManager
	{
		event EventHandlerICollisionPair CollisionEventAdded;
		event EventHandlerICollisionPair CollisionEventRemoved;
		event EventHandlerICollisionPair CollisionEventStateUpdated;

		/// <summary>檢查指定 Collision 是否存在</summary>
		bool IsExist(string Name);
		/// <summary>取得指定 Collision 的資訊</summary>
		ICollisionPair this[string Name] { get; }
		/// <summary>取得指定 Collision 的資訊</summary>
		ICollisionPair Get(string Name);
		/// <summary>取得 Collision 的 Name 的清單</summary>
		List<string> GetNames();
		/// <summary>取得 Collision 清單</summary>
		List<ICollisionPair> GetList();
		/// <summary>新增 Collision 資訊</summary>
		void Add(string Name, ICollisionPair Data);
		/// <summary>移除指定 Collision 資訊</summary>
		void Remove(string Name);
		/// <summary>更新指定 Collision 的資訊</summary>
		void Update(string Name, IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy);
	}
}
