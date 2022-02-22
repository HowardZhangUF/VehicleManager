using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForVM
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
			mConnectionString = $"data source={mDatabaseServerAddressIp};journal mode=Truncate";
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
					lock (mLockOfSqlConnection)
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
			}
			catch (SQLiteException ex)
			{
				HandleDbException(ex, NonQueryCmd);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			return result;
		}
		public override int[] ExecuteNonQueryCommands(IEnumerable<string> NonQueryCmds)
		{
			List<int> result = new List<int>();
			try
			{
				lock (mLockOfSqlConnection)
				{
					using (SQLiteConnection sql = new SQLiteConnection(mConnectionString))
					{
						if (sql.State == ConnectionState.Closed) sql.Open();
						using (SQLiteTransaction sqlTrans = sql.BeginTransaction())
						{
							for (int i = 0; i < NonQueryCmds.Count(); ++i)
							{
								using (SQLiteCommand sqlc = new SQLiteCommand(NonQueryCmds.ElementAt(i), sql))
								{
									result.Add(sqlc.ExecuteNonQuery());
								}
							}
							sqlTrans.Commit();
						}
					}
				}
			}
			catch (SQLiteException ex)
			{
				HandleDbException(ex, NonQueryCmds);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			return result.ToArray();
		}
		public override DataSet ExecuteQueryCommand(string QueryCmd)
		{
			DataSet result = mDefaultValueOfQueryCmdResult;
			try
			{
				if (!string.IsNullOrEmpty(QueryCmd))
				{
					lock (mLockOfSqlConnection)
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
			}
			catch (SQLiteException ex)
			{
				HandleDbException(ex, QueryCmd);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			return result;
		}
		public override DataSet[] ExecuteQueryCommands(IEnumerable<string> QueryCmds)
		{
			List<DataSet> result = new List<DataSet>();
			try
			{
				lock (mLockOfSqlConnection)
				{
					using (SQLiteConnection sql = new SQLiteConnection(mConnectionString))
					{
						if (sql.State == ConnectionState.Closed) sql.Open();
						using (SQLiteTransaction sqlTrans = sql.BeginTransaction())
						{
							for (int i = 0; i < QueryCmds.Count(); ++i)
							{
								using (SQLiteCommand sqlc = new SQLiteCommand(QueryCmds.ElementAt(i), sql))
								{
									using (SQLiteDataAdapter sqlda = new SQLiteDataAdapter(sqlc))
									{
										DataSet tmpResult = new DataSet();
										tmpResult.Clear();
										sqlda.Fill(tmpResult);
										result.Add(tmpResult);
									}
								}
							}
							sqlTrans.Commit();
						}
					}
				}
			}
			catch (SQLiteException ex)
			{
				HandleDbException(ex, QueryCmds);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			return result.ToArray();
		}
		public override void BackupToFile(string FilePath)
		{
			CopyFile(mDatabaseServerAddressIp, FilePath);
		}

		private void CopyFile(string SrcFileName, string Dst)
		{
			FileOperation.CopyFileViaCommandPrompt(SrcFileName, Dst);
		}
	}
}
