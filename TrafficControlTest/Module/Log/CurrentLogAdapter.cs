using Library;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	public class CurrentLogAdapter : ICurrentLogAdapter
	{
		public bool mIsExecuting { get { return rDatabaseAdapter != null ? rDatabaseAdapter.mIsExecuting : false; } }

		private DatabaseAdapter rDatabaseAdapter = null;
		private string mTableNameOfCurrentVehicleInfo = "CurrentVehicleInfo";

		public CurrentLogAdapter(DatabaseAdapter DatabaseAdapter)
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

		private void InitializeDatabaseTable()
		{
			CreateTableOfCurrentVehicleInfo();
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
	}
}
