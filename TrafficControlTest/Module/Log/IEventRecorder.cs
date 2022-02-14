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
		void RecordCurrentVehicleInfo(DatabaseDataOperation Action, IVehicleInfo VehicleInfo);
		void RecordHistoryVehicleInfo(DatabaseDataOperation Action, DateTime Timestamp, IVehicleInfo VehicleInfo);
		void RecordHistoryMissionInfo(DatabaseDataOperation Action, IMissionState MissionState);
		void RecordHistoryVehicleControlInfo(DatabaseDataOperation Action, IVehicleControl VehicleControl);
		void RecordHistoryAutomaticDoorControlInfo(DatabaseDataOperation Action, IAutomaticDoorControl AutomaticDoorControl);
		void RecordHistoryHostCommunicationInfo(DatabaseDataOperation Action, DateTime Timestamp, string Event, string IpPort, string Data);
	}
}
