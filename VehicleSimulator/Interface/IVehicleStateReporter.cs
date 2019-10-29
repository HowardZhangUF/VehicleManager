using TrafficControlTest.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace VehicleSimulator.Interface
{
	public interface IVehicleStateReporter
	{
		event EventHandlerDateTime SystemStarted;
		event EventHandlerDateTime SystemStopped;

		bool mIsExcuting { get; }
		bool mAutoStart { get; set; }

		void Set(IVehicleSimulatorInfo VehicleSimulatorInfo);
		void Set(ICommunicatorClient CommunicatorClient);
		void Set(IVehicleSimulatorInfo VehicleSimulatorInfo, ICommunicatorClient CommunicatorClient);
		void Start();
		void Stop();
	}
}
