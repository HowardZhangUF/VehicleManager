using LibraryForVM;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.CollisionEvent
{
	/// <summary>
	/// - Reference: IVehicleInfoManager, ICollisionEventManager
	/// - 定期從 IVehicleInfoManager 拿車子資訊 (IVehicleInfo) 來計算是否有車子將會發生 ICollisionEvent
	/// - 根據計算結果來使用 ICollisionEventManager 的 Add(), Update() 方法
	/// </summary>
	public interface ICollisionEventDetector : ISystemWithLoopTask
	{
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(ICollisionEventManager CollisionEventManager);
		void Set(IVehicleInfoManager VehicleInfoManager, ICollisionEventManager CollisionEventManager);
	}
}
