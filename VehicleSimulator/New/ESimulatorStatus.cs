using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public enum ESimulatorStatus
	{
		None,
		Idle,
		Working,
		ChargingButFree,
		Charging,
		Maintaining,
		Failing,
		Paused
	}
}
