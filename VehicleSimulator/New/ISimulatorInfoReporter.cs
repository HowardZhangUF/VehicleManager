using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace VehicleSimulator.New
{
	public interface ISimulatorInfoReporter : ISystemWithLoopTask
	{
		void Set(ISimulatorInfo ISimulatorInfo);
		void Set(IHostCommunicator IHostCommunicator);
		void Set(ISimulatorInfo ISimulatorInfo, IHostCommunicator IHostCommunicator);
	}
}
