﻿using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Log
{
	public class LogRecorder : ILogRecorder
	{
		public bool mIsExecuting { get { return rDatabaseAdapter != null ? rDatabaseAdapter.mIsExecuting : false; } }

		private DatabaseAdapter rDatabaseAdapter = null;
		private string mTableNameOfGeneralLog = "GeneralLog";
		
		public LogRecorder(DatabaseAdapter DatabaseAdapter)
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
		public void RecordGeneralLog(string Timestamp, string Category, string SubCategory, string Message)
		{
			rDatabaseAdapter.EnqueueNonQueryCommand($"INSERT INTO {mTableNameOfGeneralLog} (Timestamp, Category, SubCategory, Message) VALUES ('{Timestamp}', '{Category}', '{SubCategory}', '{Message}')");
		}

		private void InitializeDatabaseTable()
		{
			CreateTableOfGeneralLog();
		}
		private void CreateTableOfGeneralLog()
		{
			rDatabaseAdapter.EnqueueNonQueryCommand($"CREATE TABLE IF NOT EXISTS {mTableNameOfGeneralLog} (No INTEGER PRIMARY KEY AUTOINCREMENT, Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, Category TEXT, SubCategory TEXT, Message TEXT);");
		}
	}
}
