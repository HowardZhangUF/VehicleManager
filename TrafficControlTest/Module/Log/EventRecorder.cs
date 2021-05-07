using System;
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
		private string mTableNameOfVehicleState = "CurrentVehicleState";
		private string mTableNamePrefixOfHistoryVehicleInfo = "HistoryVehicleInfoOf";
		private string mTableNameOfMissionState = "MissionState";
		private string mTableNameOfVehicleControl = "VehicleControl";
		private string mTableNameOfAutomaticDoorControl = "AutomaticDoorControl";
		private string mTableNameOfHistoryHostCommunication = "HistoryHostCommunication";

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
		public void CreateTableOfHistoryVehicleInfo(string VehicleName)
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNamePrefixOfHistoryVehicleInfo}{VehicleName.Replace("-", "Dash")} (";
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
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		public void RecordVehicleInfo(DatabaseDataOperation Action, IVehicleInfo VehicleInfo)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					VehicleInfoDataAdd(VehicleInfo);
					break;
				case DatabaseDataOperation.Remove:
					VehicleInfoDataRemove(VehicleInfo);
					break;
				case DatabaseDataOperation.Update:
					VehicleInfoDataUpdate(VehicleInfo);
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
		public void RecordMissionState(DatabaseDataOperation Action, IMissionState MissionState)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					MissionStateDataAdd(MissionState);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					MissionStateDataUpdate(MissionState);
					break;
			}
		}
		public void RecordVehicleControl(DatabaseDataOperation Action, IVehicleControl VehicleControl)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					VehicleControlDataAdd(VehicleControl);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					VehicleControlDataUpdate(VehicleControl);
					break;
			}
		}
		public void RecordAutomaticDoorControl(DatabaseDataOperation Action, IAutomaticDoorControl AutomaticDoorControl)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					AutomaticDoorControlDataAdd(AutomaticDoorControl);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					AutomaticDoorControlDataUpdate(AutomaticDoorControl);
					break;
			}
		}
		public void RecordHistoryHostCommunication(DatabaseDataOperation Action, DateTime Timestamp, string Event, string IpPort, string Data)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					HistoryHostCommunicationAdd(Timestamp, Event, IpPort, Data);
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
			CreateTableOfCurrentVehicleState();
			CreateTableOfAllMissionState();
			CreateTableOfVehicleControl();
			CreateTableOfAutomaticDoorControl();
			CreateTableOfHistoryHostCommunication();
		}
		private void CreateTableOfCurrentVehicleState()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfVehicleState} (";
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
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		private void CreateTableOfAllMissionState()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfMissionState} (";
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
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		private void CreateTableOfVehicleControl()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfVehicleControl} (";
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
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		private void CreateTableOfAutomaticDoorControl()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfAutomaticDoorControl} (";
			tmp += "ID TEXT PRIMARY KEY, ";
			tmp += "AutomaticDoorName Text, ";
			tmp += "Command TEXT, ";
			tmp += "Cause TEXT, ";
			tmp += "SendState TEXT, ";
			tmp += "ReceiveTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "LastUpdateTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		private void CreateTableOfHistoryHostCommunication()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfHistoryHostCommunication} (";
			tmp += "No INTEGER PRIMARY KEY AUTOINCREMENT, ";
			tmp += "Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "Event TEXT, ";
			tmp += "IPPort TEXT, ";
			tmp += "Data TEXT)";
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		private void VehicleInfoDataAdd(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNameOfVehicleState} VALUES (";
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
		private void VehicleInfoDataRemove(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"DELETE FROM {mTableNameOfVehicleState} WHERE ID = '{VehicleInfo.mName}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void VehicleInfoDataUpdate(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE {mTableNameOfVehicleState} SET ";
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
			tmp += $"INSERT INTO {mTableNamePrefixOfHistoryVehicleInfo}{VehicleInfo.mName.Replace("-", "Dash")} VALUES (";
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
		private void MissionStateDataAdd(IMissionState MissionState)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNameOfMissionState} VALUES (";
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
		private void MissionStateDataUpdate(IMissionState MissionState)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE {mTableNameOfMissionState} SET ";
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
		private void VehicleControlDataAdd(IVehicleControl VehicleControl)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNameOfVehicleControl} VALUES (";
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
		private void VehicleControlDataUpdate(IVehicleControl VehicleControl)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE {mTableNameOfVehicleControl} SET ";
			tmp += $"SendState = '{VehicleControl.mSendState.ToString()}', ";
			tmp += $"ExecuteState = '{VehicleControl.mExecuteState.ToString()}', ";
			tmp += $"FailedReason = '{VehicleControl.mFailedReason.ToString()}', ";
			tmp += $"ExecutionStartTimestamp = '{VehicleControl.mExecutionStartTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"ExecutionStopTimestamp = '{VehicleControl.mExecutionStopTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"LastUpdateTimestamp = '{VehicleControl.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{VehicleControl.mName}' AND ReceiveTimestamp = '{VehicleControl.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void AutomaticDoorControlDataAdd(IAutomaticDoorControl AutomaticDoorControl)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNameOfAutomaticDoorControl} VALUES (";
			tmp += $"'{AutomaticDoorControl.mName}', ";
			tmp += $"'{AutomaticDoorControl.mAutomaticDoorName}', ";
			tmp += $"'{AutomaticDoorControl.mCommand.ToString()}', ";
			tmp += $"'{AutomaticDoorControl.mCause}', ";
			tmp += $"'{AutomaticDoorControl.mSendState.ToString()}', ";
			tmp += $"'{AutomaticDoorControl.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{AutomaticDoorControl.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void AutomaticDoorControlDataUpdate(IAutomaticDoorControl AutomaticDoorControl)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE {mTableNameOfAutomaticDoorControl} SET ";
			tmp += $"SendState = '{AutomaticDoorControl.mSendState.ToString()}', ";
			tmp += $"LastUpdateTimestamp = '{AutomaticDoorControl.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{AutomaticDoorControl.mName}' AND ReceiveTimestamp = '{AutomaticDoorControl.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryHostCommunicationAdd(DateTime Timestamp, string Event, string IpPort, string Data)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNameOfHistoryHostCommunication} (Timestamp, Event, IPPort, Data) VALUES (";
			tmp += $"'{Timestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{Event}', ";
			tmp += $"'{IpPort}', ";
			tmp += $"'{Data}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
	}
}
