using TrafficControlTest.Interface;

namespace VehicleSimulator.Interface
{
	public interface IConsoleMessageHandler
	{
		void Set(IVehicleSimulatorInfo VehicleSimulator);
		void Set(ICommunicatorClient CommunicatorClient);
		void Set(IVehicleSimulatorInfo VehicleSimulatorInfo, ICommunicatorClient CommunicatorClient);
	}
}
