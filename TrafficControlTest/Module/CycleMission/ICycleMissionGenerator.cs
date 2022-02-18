using System;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.CycleMission
{
	public interface ICycleMissionGenerator : ISystemWithLoopTask
	{
		event EventHandler<CycleMissionAssignedEventArgs> CycleMissionAssigned;
		event EventHandler<CycleMissionUnassignedEventArgs> CycleMissionUnassigned;
		event EventHandler<CycleMissionExecutedIndexChangedEventArgs> CycleMissionExecutedIndexChanged;

		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager);
		void AssignCycleMission(string VehicleId, string[] Targets, int StartIndex = 0);
		void UnassignCycleMission(string VehicleId);
		bool IsAssigned(string VehicleId);
		string[] GetMissionList(string VehicleId);
		int GetCurrentMissionIndex(string VehicleId);
	}

	public class CycleMissionAssignedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string VehicleId { get; private set; }
		public string[] Missions { get; private set; }

		public CycleMissionAssignedEventArgs(DateTime OccurTime, string VehicleId, string[] Missions) : base()
		{
			this.OccurTime = OccurTime;
			this.VehicleId = VehicleId;
			this.Missions = Missions;
		}
	}
	public class CycleMissionUnassignedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string VehicleId { get; private set; }

		public CycleMissionUnassignedEventArgs(DateTime OccurTime, string VehicleId) : base()
		{
			this.OccurTime = OccurTime;
			this.VehicleId = VehicleId;
		}
	}
	public class CycleMissionExecutedIndexChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string VehicleId { get; private set; }
		public int Index { get; private set; }

		public CycleMissionExecutedIndexChangedEventArgs(DateTime OccurTime, string VehicleId, int Index) : base()
		{
			this.OccurTime = OccurTime;
			this.VehicleId = VehicleId;
			this.Index = Index;
		}
	}
}
