using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Implement;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	public interface IMissionStateManager : IItemManager<IMissionState>
	{
		IMissionState this[string MissionId] { get; }
		void UpdateExecutorId(string MissionId, string ExecutorId);
		void UpdateSendState(string MissionId, SendState SendState);
		void UpdateExecuteState(string MissionId, ExecuteState ExecuteState);
	}
}
