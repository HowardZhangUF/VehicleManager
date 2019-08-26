using System;
using TrafficControlTest.Interface;
using VehicleSimulator.Implement;
using VehicleSimulator.Interface;

namespace VehicleSimulator.Library
{
	public static class Library
	{
		public static IVehicleSimulatorInfo GenerateIVehicleSimulatorInfo(string Name)
		{
			return new VehicleSimulatorInfo(Name);
		}
		public static IVehicleStateReporter GenerateIVehicleStateReporter(IVehicleSimulatorInfo VehicleSimulatorInfo, ICommunicatorClient CommunicatorClient)
		{
			return new VehicleStateReporter(VehicleSimulatorInfo, CommunicatorClient);
		}
		public static IConsoleMessageHandler GenerateIConsoleMessageHandler(IVehicleSimulatorInfo VehicleSimulatorInfo, ICommunicatorClient CommunicatorClient)
		{
			return new ConsoleMessageHandler(VehicleSimulatorInfo, CommunicatorClient);
		}
	}

	public static class EventHandlerLibraryOfIVehicleSimulator
	{
		public delegate void EventHandlerIVehicleSimulator(DateTime OccurTime, string Name, IVehicleSimulatorInfo VehicleSimulatorInfo);
	}
}
