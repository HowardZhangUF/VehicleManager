using System;
using System.Collections.Generic;
using System.Data;
using TrafficControlTest.Library;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	public class EventRecorder : IEventRecorder
	{
		public bool mIsExecuting { get { return rDatabaseAdapter != null ? rDatabaseAdapter.mIsExecuting : false; } }

		private DatabaseAdapter rDatabaseAdapter = null;
		private string mTableNameOfCurrentVehicleInfo = "CurrentVehicleInfo";
		private string mTableNameOfHistoryVehicleInfo = "HistoryVehicleInfo";
		private string mTableNameOfHistoryMissionInfo = "HistoryMissionInfo";
		private string mTableNameOfHistoryVehicleControlInfo = "HistoryVehicleControlInfo";
		private string mTableNameOfHistoryAutomaticDoorControlInfo = "HistoryAutomaticDoorControlInfo";
		private string mTableNameOfHistoryHostCommunicationInfo = "HistoryHostCommunicationInfo";

		public EventRecorder(DatabaseAdapter DatabaseAdapter)
		{
			Set(DatabaseAdapter);
		}
		public void Set(DatabaseAdapter DatabaseAdapter)
		{
			if (DatabaseAdapter != null)
			{
				rDatabaseAdapter = DatabaseAdapter;
				InitializeDatabaseTable();
			}
		}
		public void Start()
		{
			rDatabaseAdapter.Start();
		}
		public void Stop()
		{
			rDatabaseAdapter.Stop();
		}
		public void RecordCurrentVehicleInfo(DatabaseDataOperation Action, IVehicleInfo VehicleInfo)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					CurrentVehicleInfoDataAdd(VehicleInfo);
					break;
				case DatabaseDataOperation.Remove:
					CurrentVehicleInfoDataRemove(VehicleInfo);
					break;
				case DatabaseDataOperation.Update:
					CurrentVehicleInfoDataUpdate(VehicleInfo);
					break;
			}
		}
		public void RecordHistoryVehicleInfo(DatabaseDataOperation Action, DateTime Timestamp, IVehicleInfo VehicleInfo)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					HistoryVehicleInfoDataAdd(Timestamp, VehicleInfo);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					// do nothing
					break;
			}
		}
		public void RecordHistoryMissionInfo(DatabaseDataOperation Action, IMissionState MissionState)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					HistoryMissionInfoDataAdd(MissionState);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					HistoryMissionInfoDataUpdate(MissionState);
					break;
			}
		}
		public void RecordHistoryVehicleControlInfo(DatabaseDataOperation Action, IVehicleControl VehicleControl)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					HistoryVehicleControlInfoDataAdd(VehicleControl);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					HistoryVehicleControlInfoDataUpdate(VehicleControl);
					break;
			}
		}
		public void RecordHistoryAutomaticDoorControlInfo(DatabaseDataOperation Action, IAutomaticDoorControl AutomaticDoorControl)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					HistoryAutomaticDoorControlInfoDataAdd(AutomaticDoorControl);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					HistoryAutomaticDoorControlInfoDataUpdate(AutomaticDoorControl);
					break;
			}
		}
		public void RecordHistoryHostCommunicationInfo(DatabaseDataOperation Action, DateTime ReceiveTimestamp, string Event, string IpPort, string Data)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					HistoryHostCommunicationInfoDataAdd(ReceiveTimestamp, Event, IpPort, Data);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					// do nothing
					break;
			}
		}

		private void InitializeDatabaseTable()
		{
			CreateTableOfCurrentVehicleInfo();
			CreateTableOfHistoryVehicleInfo();
			CreateTableOfHistoryMissionInfo();
			CreateTableOfHistoryVehicleControlInfo();
			CreateTableOfHistoryAutomaticDoorControlInfo();
			CreateTableOfHistoryHostCommunicationInfo();

			CreateIndexOfHistoryVehicleInfo();
			CreateIndexOfHistoryMissionInfo();
			CreateIndexOfHistoryVehicleControlInfo();
			CreateIndexOfHistoryAutomaticDoorControlInfo();
			CreateIndexOfHistoryHostCommunicationInfo();
		}
		private void CreateTableOfCurrentVehicleInfo()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS '{mTableNameOfCurrentVehicleInfo}' (";
			tmp += "ID TEXT UNIQUE, ";
			tmp += "State TEXT, ";
			tmp += "OriState TEXT, ";
			tmp += "X INTEGER, ";
			tmp += "Y INTEGER, ";
			tmp += "Toward INTEGER, ";
			tmp += "Target TEXT, ";
			tmp += "Velocity REAL, ";
			tmp += "LocationScore REAL, ";
			tmp += "BatteryValue REAL, ";
			tmp += "AlarmMessage TEXT, ";
			tmp += "Path TEXT, ";
			tmp += "IPPort TEXT, ";
			tmp += "MissionID TEXT, ";
			tmp += "InterveneCommand TEXT, ";
			tmp += "MapName TEXT, ";
			tmp += "LastUpdateTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void CreateTableOfHistoryVehicleInfo()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS '{mTableNameOfHistoryVehicleInfo}' (";
			tmp += "No INTEGER PRIMARY KEY AUTOINCREMENT, ";
			tmp += "RecordTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "ID TEXT, ";
			tmp += "State TEXT, ";
			tmp += "OriState TEXT, ";
			tmp += "X INTEGER, ";
			tmp += "Y INTEGER, ";
			tmp += "Toward INTEGER, ";
			tmp += "Target TEXT, ";
			tmp += "Velocity REAL, ";
			tmp += "LocationScore REAL, ";
			tmp += "BatteryValue REAL, ";
			tmp += "AlarmMessage TEXT, ";
			tmp += "Path TEXT, ";
			tmp += "IPPort TEXT, ";
			tmp += "MissionID TEXT, ";
			tmp += "InterveneCommand TEXT, ";
			tmp += "MapName TEXT, ";
			tmp += "LastUpdateTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void CreateTableOfHistoryMissionInfo()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS '{mTableNameOfHistoryMissionInfo}' (";
			tmp += "ID TEXT PRIMARY KEY, ";
			tmp += "HostMissionID Text,";
			tmp += "Type TEXT, ";
			tmp += "Priority INTEGER, ";
			tmp += "VehicleID TEXT, ";
			tmp += "Parameters TEXT, ";
			tmp += "SourceIPPort TEXT, ";
			tmp += "ExecutorID TEXT, ";
			tmp += "ExecuteState TEXT, ";
			tmp += "FailedReason TEXT, ";
			tmp += "ReceiveTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "ExecutionStartTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "ExecutionStopTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "LastUpdateTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void CreateTableOfHistoryVehicleControlInfo()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS '{mTableNameOfHistoryVehicleControlInfo}' (";
			tmp += "ID TEXT PRIMARY KEY, ";
			tmp += "VehicleID Text,";
			tmp += "Command TEXT, ";
			tmp += "Parameters TEXT, ";
			tmp += "CauseID TEXT, ";
			tmp += "CauseDetail TEXT, ";
			tmp += "SendState TEXT, ";
			tmp += "ExecuteState TEXT, ";
			tmp += "FailedReason TEXT, ";
			tmp += "ReceiveTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "ExecutionStartTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "ExecutionStopTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "LastUpdateTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void CreateTableOfHistoryAutomaticDoorControlInfo()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS '{mTableNameOfHistoryAutomaticDoorControlInfo}' (";
			tmp += "ID TEXT PRIMARY KEY, ";
			tmp += "AutomaticDoorName Text, ";
			tmp += "Command TEXT, ";
			tmp += "Cause TEXT, ";
			tmp += "SendState TEXT, ";
			tmp += "ReceiveTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "LastUpdateTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void CreateTableOfHistoryHostCommunicationInfo()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS '{mTableNameOfHistoryHostCommunicationInfo}' (";
			tmp += "No INTEGER PRIMARY KEY AUTOINCREMENT, ";
			tmp += "ReceiveTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "Event TEXT, ";
			tmp += "IPPort TEXT, ";
			tmp += "Data TEXT)";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void CreateIndexOfHistoryVehicleInfo()
		{
			rDatabaseAdapter.EnqueueNonQueryCommand($"CREATE INDEX IF NOT EXISTS index{mTableNameOfHistoryVehicleInfo}1 ON {mTableNameOfHistoryVehicleInfo} (RecordTimestamp)");
			rDatabaseAdapter.EnqueueNonQueryCommand($"CREATE INDEX IF NOT EXISTS index{mTableNameOfHistoryVehicleInfo}2 ON {mTableNameOfHistoryVehicleInfo} (ID)");
		}
		private void CreateIndexOfHistoryMissionInfo()
		{
			rDatabaseAdapter.EnqueueNonQueryCommand($"CREATE INDEX IF NOT EXISTS index{mTableNameOfHistoryMissionInfo} ON {mTableNameOfHistoryMissionInfo} (ReceiveTimestamp)");
		}
		private void CreateIndexOfHistoryVehicleControlInfo()
		{
			rDatabaseAdapter.EnqueueNonQueryCommand($"CREATE INDEX IF NOT EXISTS index{mTableNameOfHistoryVehicleControlInfo} ON {mTableNameOfHistoryVehicleControlInfo} (ReceiveTimestamp)");
		}
		private void CreateIndexOfHistoryAutomaticDoorControlInfo()
		{
			rDatabaseAdapter.EnqueueNonQueryCommand($"CREATE INDEX IF NOT EXISTS index{mTableNameOfHistoryAutomaticDoorControlInfo} ON {mTableNameOfHistoryAutomaticDoorControlInfo} (ReceiveTimestamp)");
		}
		private void CreateIndexOfHistoryHostCommunicationInfo()
		{
			rDatabaseAdapter.EnqueueNonQueryCommand($"CREATE INDEX IF NOT EXISTS index{mTableNameOfHistoryHostCommunicationInfo} ON {mTableNameOfHistoryHostCommunicationInfo} (ReceiveTimestamp)");
		}
		private void CurrentVehicleInfoDataAdd(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO '{mTableNameOfCurrentVehicleInfo}' VALUES (";
			tmp += $"'{VehicleInfo.mName}', ";
			tmp += $"'{VehicleInfo.mCurrentState}', ";
			tmp += $"'{VehicleInfo.mCurrentOriState}', ";
			tmp += $"{VehicleInfo.mLocationCoordinate.mX.ToString()}, ";
			tmp += $"{VehicleInfo.mLocationCoordinate.mY.ToString()}, ";
			tmp += $"{((int)VehicleInfo.mLocationToward).ToString()}, ";
			tmp += $"'{VehicleInfo.mCurrentTarget}', ";
			tmp += $"{VehicleInfo.mTranslationVelocity.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mLocationScore.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mBatteryValue.ToString("F2")}, ";
			tmp += $"'{VehicleInfo.mErrorMessage}', ";
			tmp += $"'{VehicleInfo.mPathString}', ";
			tmp += $"'{VehicleInfo.mIpPort}', ";
			tmp += $"'{VehicleInfo.mCurrentMissionId}', ";
			tmp += $"'{VehicleInfo.mCurrentInterveneCommand}', ";
			tmp += $"'{VehicleInfo.mCurrentMapName}', ";
			tmp += $"'{VehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void CurrentVehicleInfoDataRemove(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"DELETE FROM '{mTableNameOfCurrentVehicleInfo}' WHERE ID = '{VehicleInfo.mName}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void CurrentVehicleInfoDataUpdate(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE {mTableNameOfCurrentVehicleInfo} SET ";
			tmp += $"State = '{VehicleInfo.mCurrentState}', ";
			tmp += $"OriState = '{VehicleInfo.mCurrentOriState}', ";
			tmp += $"X = {VehicleInfo.mLocationCoordinate.mX.ToString()}, ";
			tmp += $"Y = {VehicleInfo.mLocationCoordinate.mY.ToString()}, ";
			tmp += $"Toward = {((int)VehicleInfo.mLocationToward).ToString()}, ";
			tmp += $"Target = '{VehicleInfo.mCurrentTarget}', ";
			tmp += $"Velocity = {VehicleInfo.mTranslationVelocity.ToString("F2")}, ";
			tmp += $"LocationScore = {VehicleInfo.mLocationScore.ToString("F2")}, ";
			tmp += $"BatteryValue = {VehicleInfo.mBatteryValue.ToString("F2")}, ";
			tmp += $"AlarmMessage = '{VehicleInfo.mErrorMessage}', ";
			tmp += $"Path = '{VehicleInfo.mPathString}', ";
			tmp += $"IPPort = '{VehicleInfo.mIpPort}', ";
			tmp += $"MissionID = '{VehicleInfo.mCurrentMissionId}', ";
			tmp += $"InterveneCommand = '{VehicleInfo.mCurrentInterveneCommand}', ";
			tmp += $"MapName = '{VehicleInfo.mCurrentMapName}', ";
			tmp += $"LastUpdateTimestamp = '{VehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{VehicleInfo.mName}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryVehicleInfoDataAdd(DateTime Timestamp, IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO '{mTableNameOfHistoryVehicleInfo}' (RecordTimestamp, ID, State, OriState, X, Y, Toward, Target, Velocity, LocationScore, BatteryValue, AlarmMessage, Path, IPPort, MissionID, InterveneCommand, MapName, LastUpdateTimestamp) VALUES (";
			tmp += $"'{Timestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{VehicleInfo.mName}', ";
			tmp += $"'{VehicleInfo.mCurrentState}', ";
			tmp += $"'{VehicleInfo.mCurrentOriState}', ";
			tmp += $"{VehicleInfo.mLocationCoordinate.mX.ToString()}, ";
			tmp += $"{VehicleInfo.mLocationCoordinate.mY.ToString()}, ";
			tmp += $"{((int)VehicleInfo.mLocationToward).ToString()}, ";
			tmp += $"'{VehicleInfo.mCurrentTarget}', ";
			tmp += $"{VehicleInfo.mTranslationVelocity.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mLocationScore.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mBatteryValue.ToString("F2")}, ";
			tmp += $"'{VehicleInfo.mErrorMessage}', ";
			tmp += $"'{VehicleInfo.mPathString}', ";
			tmp += $"'{VehicleInfo.mIpPort}', ";
			tmp += $"'{VehicleInfo.mCurrentMissionId}', ";
			tmp += $"'{VehicleInfo.mCurrentInterveneCommand}', ";
			tmp += $"'{VehicleInfo.mCurrentMapName}', ";
			tmp += $"'{VehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryMissionInfoDataAdd(IMissionState MissionState)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO '{mTableNameOfHistoryMissionInfo}' VALUES (";
			tmp += $"'{MissionState.mName}', ";
			tmp += $"'{MissionState.mMission.mMissionId}', ";
			tmp += $"'{MissionState.mMission.mMissionType}', ";
			tmp += $"{MissionState.mMission.mPriority.ToString()}, ";
			tmp += $"'{MissionState.mMission.mVehicleId}', ";
			tmp += $"'{MissionState.mMission.mParametersString}', ";
			tmp += $"'{MissionState.mSourceIpPort}', ";
			tmp += $"'{MissionState.mExecutorId}', ";
			tmp += $"'{MissionState.mExecuteState.ToString()}', ";
			tmp += $"'{MissionState.mFailedReason.ToString()}', ";
			tmp += $"'{MissionState.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{MissionState.mExecutionStartTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{MissionState.mExecutionStopTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{MissionState.mLastUpdate.ToString(Library.Library.TIME_FORMAT)}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryMissionInfoDataUpdate(IMissionState MissionState)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE '{mTableNameOfHistoryMissionInfo}' SET ";
			tmp += $"Priority = {MissionState.mMission.mPriority.ToString()}, ";
			tmp += $"ExecutorID = '{MissionState.mExecutorId}', ";
			tmp += $"ExecuteState = '{MissionState.mExecuteState.ToString()}', ";
			tmp += $"FailedReason = '{MissionState.mFailedReason.ToString()}', ";
			tmp += $"ReceiveTimestamp = '{MissionState.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"ExecutionStartTimestamp = '{MissionState.mExecutionStartTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"ExecutionStopTimestamp = '{MissionState.mExecutionStopTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"LastUpdateTimestamp = '{MissionState.mLastUpdate.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{MissionState.mName}' AND ReceiveTimestamp = '{MissionState.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryVehicleControlInfoDataAdd(IVehicleControl VehicleControl)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO '{mTableNameOfHistoryVehicleControlInfo}' VALUES (";
			tmp += $"'{VehicleControl.mName}', ";
			tmp += $"'{VehicleControl.mVehicleId}', ";
			tmp += $"'{VehicleControl.mCommand.ToString()}', ";
			tmp += $"'{VehicleControl.mParametersString}', ";
			tmp += $"'{VehicleControl.mCauseId}', ";
			tmp += $"'{VehicleControl.mCauseDetail}', ";
			tmp += $"'{VehicleControl.mSendState.ToString()}', ";
			tmp += $"'{VehicleControl.mExecuteState.ToString()}', ";
			tmp += $"'{VehicleControl.mFailedReason.ToString()}', ";
			tmp += $"'{VehicleControl.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{VehicleControl.mExecutionStartTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{VehicleControl.mExecutionStopTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{VehicleControl.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryVehicleControlInfoDataUpdate(IVehicleControl VehicleControl)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE '{mTableNameOfHistoryVehicleControlInfo}' SET ";
			tmp += $"SendState = '{VehicleControl.mSendState.ToString()}', ";
			tmp += $"ExecuteState = '{VehicleControl.mExecuteState.ToString()}', ";
			tmp += $"FailedReason = '{VehicleControl.mFailedReason.ToString()}', ";
			tmp += $"ExecutionStartTimestamp = '{VehicleControl.mExecutionStartTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"ExecutionStopTimestamp = '{VehicleControl.mExecutionStopTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"LastUpdateTimestamp = '{VehicleControl.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{VehicleControl.mName}' AND ReceiveTimestamp = '{VehicleControl.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryAutomaticDoorControlInfoDataAdd(IAutomaticDoorControl AutomaticDoorControl)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO '{mTableNameOfHistoryAutomaticDoorControlInfo}' VALUES (";
			tmp += $"'{AutomaticDoorControl.mName}', ";
			tmp += $"'{AutomaticDoorControl.mAutomaticDoorName}', ";
			tmp += $"'{AutomaticDoorControl.mCommand.ToString()}', ";
			tmp += $"'{AutomaticDoorControl.mCause}', ";
			tmp += $"'{AutomaticDoorControl.mSendState.ToString()}', ";
			tmp += $"'{AutomaticDoorControl.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{AutomaticDoorControl.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryAutomaticDoorControlInfoDataUpdate(IAutomaticDoorControl AutomaticDoorControl)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE '{mTableNameOfHistoryAutomaticDoorControlInfo}' SET ";
			tmp += $"SendState = '{AutomaticDoorControl.mSendState.ToString()}', ";
			tmp += $"LastUpdateTimestamp = '{AutomaticDoorControl.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{AutomaticDoorControl.mName}' AND ReceiveTimestamp = '{AutomaticDoorControl.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryHostCommunicationInfoDataAdd(DateTime ReceiveTimestamp, string Event, string IpPort, string Data)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO '{mTableNameOfHistoryHostCommunicationInfo}' (ReceiveTimestamp, Event, IPPort, Data) VALUES (";
			tmp += $"'{ReceiveTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{Event}', ";
			tmp += $"'{IpPort}', ";
			tmp += $"'{Data}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
	}
}
