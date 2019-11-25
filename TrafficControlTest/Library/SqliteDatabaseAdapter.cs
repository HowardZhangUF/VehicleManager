using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Library
{
	public class SqliteDatabaseAdapter : DatabaseAdapter
	{
		/// <summary>輸入參數範例： ($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Test.db", string.Empty, string.Empty, string.Empty, string.Empty, false)</summary>
		public SqliteDatabaseAdapter(string DatabaseServerAddressIp, string DatabaseServerAddressPort, string UserAccount, string UserPassword, string InitialDatabase, bool PingBeforeBuildConnection) : base(DatabaseServerAddressIp, DatabaseServerAddressPort, UserAccount, UserPassword, InitialDatabase, PingBeforeBuildConnection)
		{
			try
			{
				// do nothing ...
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		public override void SetDatabaseParameters(string DatabaseServerAddressIp, string DatabaseServerAddressPort, string UserAccount, string UserPassword, string InitialDatabase)
		{
			base.SetDatabaseParameters(DatabaseServerAddressIp, DatabaseServerAddressPort, UserAccount, UserPassword, InitialDatabase);
			mConnectionString = $"Data Source={mDatabaseServerAddressIp}";
			mIsConnected = false;
		}
		public override bool Connect()
		{
			bool result = false;
			try
			{
				if (!string.IsNullOrEmpty(mConnectionString) && !string.IsNullOrEmpty(mDatabaseServerAddressIp))
				{
					if (!System.IO.File.Exists(mDatabaseServerAddressIp)) SQLiteConnection.CreateFile(mDatabaseServerAddressIp);
					mIsConnected = true;
					result = true;
				}
			}
			catch (SQLiteException ex)
			{
				HandleDbException(ex);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			return result;
		}
		public override int ExecuteNonQueryCommand(string NonQueryCmd)
		{
			int result = mDefaultValueOfNonQueryCmdResult;
			try
			{
				if (!string.IsNullOrEmpty(NonQueryCmd))
				{
					using (SQLiteConnection sql = new SQLiteConnection(mConnectionString))
					{
						if (sql.State == ConnectionState.Closed) sql.Open();
						using (SQLiteCommand sqlc = new SQLiteCommand(NonQueryCmd, sql))
						{
							result = sqlc.ExecuteNonQuery();
						}
					}
				}
			}
			catch (SQLiteException ex)
			{
				HandleDbException(ex);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			return result;
		}
		public override DataSet ExecuteQueryCommand(string QueryCmd)
		{
			DataSet result = mDefaultValueOfQueryCmdResult;
			try
			{
				if (!string.IsNullOrEmpty(QueryCmd))
				{
					using (SQLiteConnection sql = new SQLiteConnection(mConnectionString))
					{
						if (sql.State == ConnectionState.Closed) sql.Open();
						using (SQLiteCommand sqlc = new SQLiteCommand(QueryCmd, sql))
						{
							using (SQLiteDataAdapter sqlda = new SQLiteDataAdapter(sqlc))
							{
								result = new DataSet();
								result.Clear();
								sqlda.Fill(result);
							}
						}
					}
				}
			}
			catch (SQLiteException ex)
			{
				HandleDbException(ex);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			return result;
		}
	}
}
