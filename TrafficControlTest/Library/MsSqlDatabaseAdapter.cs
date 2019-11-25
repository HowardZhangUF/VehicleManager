using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace TrafficControlTest.Library
{
	public class MsSqlDatabaseAdapter : DatabaseAdapter
	{
		/// <summary>輸入參數範例 ("127.0.0.1", "1433", "Castec", "27635744", "AgvState", false)</summary>
		public MsSqlDatabaseAdapter(string DatabaseServerAddressIp, string DatabaseServerAddressPort, string UserAccount, string UserPassword, string InitialDatabase, bool PingBeforeBuildConnection) : base(DatabaseServerAddressIp, DatabaseServerAddressPort, UserAccount, UserPassword, InitialDatabase, PingBeforeBuildConnection)
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
			try
			{
				base.SetDatabaseParameters(DatabaseServerAddressIp, DatabaseServerAddressPort, UserAccount, UserPassword, InitialDatabase);
				mConnectionString = $"Data Source={mDatabaseServerAddressIp},{mDatabaseServerAddressPort};Initial Catalog={mInitialDatabase};User Id={mUserAccount};Password={mUserPassword};";
				mIsConnected = false;
			}
			catch (SqlException ex)
			{
				HandleDbException(ex);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		public override bool Connect()
		{
			bool result = false;
			try
			{
				if (mConnectionString != string.Empty)
				{
					if (mPingBeforeBuildConnection)
					{
						if (GetPingStatus(mDatabaseServerAddressIp) != IPStatus.Success)
						{
							return result;
						}
					}

					using (SqlConnection sql = new SqlConnection(mConnectionString))
					{
						sql.Open();
						sql.Close();
						mIsConnected = true;
						result = true;
					}
				}
			}
			catch (SqlException ex)
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
					using (SqlConnection sql = new SqlConnection(mConnectionString))
					{
						if (sql.State == ConnectionState.Closed) sql.Open();
						using (SqlCommand sqlc = new SqlCommand(NonQueryCmd, sql))
						{
							result = sqlc.ExecuteNonQuery();
						}
					}
				}
			}
			catch (SqlException ex)
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
					using (SqlConnection sql = new SqlConnection(mConnectionString))
					{
						if (sql.State == ConnectionState.Closed) sql.Open();
						using (SqlCommand sqlc = new SqlCommand(QueryCmd, sql))
						{
							using (SqlDataAdapter sqlda = new SqlDataAdapter(sqlc))
							{
								result = new DataSet();
								result.Clear();
								sqlda.Fill(result);
							}
						}
					}
				}
			}
			catch (SqlException ex)
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
