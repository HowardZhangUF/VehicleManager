using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	public interface IMissionStateManager
	{
		event EventHandlerIMissionState MissionStateAdded;
		event EventHandlerIMissionState MissionStateRemoved;
		event EventHandlerIMissionStateStateUpdated MissionStateStateUpdated;

		bool IsExist(string MissionId);
		IMissionState this[string MissionId] { get; }
		IMissionState Get(string MissionId);
		List<string> GetMissionIds();
		List<IMissionState> GetList();
		void Add(string MissionId, IMissionState MissionState);
		void Remove(string MissionId);
		void UpdateExecutorId(string MissionId, string ExecutorId);
		void UpdateSendState(string MissionId, SendState SendState);
		void UpdateExecuteState(string MissionId, ExecuteState ExecuteState);
	}
}
