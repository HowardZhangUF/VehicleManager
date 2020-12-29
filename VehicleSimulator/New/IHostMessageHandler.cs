using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator.New
{
	public interface IHostMessageHandler
	{
		void Set(IHostCommunicator IHostCommunicator);
		void Set(ISimulatorControl ISimulatorControl);
		void Set(IHostCommunicator IHostCommunicator, ISimulatorControl ISimulatorControl);
	}
}
