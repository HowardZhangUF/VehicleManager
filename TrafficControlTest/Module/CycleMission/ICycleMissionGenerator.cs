using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CycleMission
{
	public interface ICycleMissionGenerator : ISystemWithLoopTask
	{
		event EventHandlerCycleMissionAssigned CycleMissionAssigned;
		event EventHandlerCycleMissionUnassigned CycleMissionUnassigned;
		event EventHandlerCycleMissionExecutedIndexChanged CycleMissionExecutedIndexChanged;

		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager);
		void AssignCycleMission(string VehicleId, string[] Targets, int StartIndex = 0);
		void UnassignCycleMission(string VehicleId);
		bool IsAssigned(string VehicleId);
		string[] GetMissionList(string VehicleId);
		int GetCurrentMissionIndex(string VehicleId);
	}
}
