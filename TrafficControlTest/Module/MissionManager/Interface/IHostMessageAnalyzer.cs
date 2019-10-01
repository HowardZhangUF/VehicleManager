using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	public interface IHostMessageAnalyzer
	{
		void Set(IHostCommunicator HostCommunicator);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IMissionAnalyzer[] MissionAnalyzers);
		void Set(IHostCommunicator HostCommunicator, IMissionStateManager MissionStateManager, IMissionAnalyzer[] MissionAnalyzers);
	}
}
