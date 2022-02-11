using System;
using TrafficControlTest.Library;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	/// <summary>提供記錄 Event (CurrentVehicleInfo, HistoryVehicleInfo, AllMissionState, HistoryHostCommunication ... etc.) 至資料庫的功能</summary>
	public interface IEventRecorder
	{
		bool mIsExecuting { get; }

		void Set(DatabaseAdapter DatabaseAdapter);
		void Start();
		void Stop();
		void CreateTableOfHistoryVehicleInfo(string VehicleName);
		void CreateIndexOfHistoryVehicleInfo(string VehicleName);
		void RecordVehicleInfo(DatabaseDataOperation Action, IVehicleInfo VehicleInfo);
		void RecordHistoryVehicleInfo(DatabaseDataOperation Action, DateTime Timestamp, IVehicleInfo VehicleInfo);
		void RecordMissionState(DatabaseDataOperation Action, IMissionState MissionState);
		void RecordVehicleControl(DatabaseDataOperation Action, IVehicleControl VehicleControl);
		void RecordAutomaticDoorControl(DatabaseDataOperation Action, IAutomaticDoorControl AutomaticDoorControl);
		void RecordHistoryHostCommunication(DatabaseDataOperation Action, DateTime Timestamp, string Event, string IpPort, string Data);
	}
}
