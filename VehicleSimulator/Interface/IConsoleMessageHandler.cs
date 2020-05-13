using TrafficControlTest.Module.Communication;

namespace VehicleSimulator.Interface
{
	public interface IConsoleMessageHandler
	{
		void Set(IVehicleSimulatorInfo VehicleSimulator);
		void Set(ICommunicatorClient CommunicatorClient);
		void Set(IVehicleSimulatorInfo VehicleSimulatorInfo, ICommunicatorClient CommunicatorClient);
	}
}
