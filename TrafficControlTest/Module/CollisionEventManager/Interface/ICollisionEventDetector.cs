using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Interface
{
	public interface ICollisionEventDetector
	{
		event EventHandlerDateTime SystemStarted;
		event EventHandlerDateTime SystemStopped;

		bool mIsExcuting { get; }

		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(ICollisionEventManager CollisionEventManager);
		void Set(IVehicleInfoManager VehicleInfoManager, ICollisionEventManager CollisionEventManager);
		void Start();
		void Stop();
	}
}
