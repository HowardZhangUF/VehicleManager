using LibraryForVM;
using System;
using TrafficControlTest.Library;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	public enum DatabaseDataOperation
	{
		Add,
		Remove,
		Update
	}

	/// <summary>提供記錄 GeneralLog, HistoryVehicleInfo, HistoryMissionInfo, HistoryVehicleControlInfo, HistoryAutomaticDoorContolInfo, HistoryHostCommunicationInfo 至資料庫的功能</summary>
	public interface IHistoryLogAdapter
	{
		bool mIsExecuting { get; }

		void Set(DatabaseAdapter DatabaseAdapter);
		void Start();
		void Stop();
		void RecordGeneralLog(string Timestamp, string Category, string SubCategory, string Message);
		void RecordHistoryVehicleInfo(DatabaseDataOperation Action, DateTime Timestamp, IVehicleInfo VehicleInfo);
		void RecordHistoryMissionInfo(DatabaseDataOperation Action, IMissionState MissionState);
		void RecordHistoryVehicleControlInfo(DatabaseDataOperation Action, IVehicleControl VehicleControl);
		void RecordHistoryAutomaticDoorControlInfo(DatabaseDataOperation Action, IAutomaticDoorControl AutomaticDoorControl);
		void RecordHistoryHostCommunicationInfo(DatabaseDataOperation Action, DateTime Timestamp, string Event, string IpPort, string Data);
		bool BackupHistoryLogToFile(string FileName);
		bool DeleteHistoryLogBefore(DateTime DateTime);
	}
}
