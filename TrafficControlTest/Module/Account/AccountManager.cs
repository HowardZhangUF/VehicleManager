using Library;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.Account
{
	public class AccountManager : IAccountManager
	{
		private Dictionary<string, IAccount> mAccountCollection = new Dictionary<string, IAccount>();
		private DatabaseAdapter rDatabaseAdapter = null;
		private string mTableNameOfAccount = "Account";

		public AccountManager(DatabaseAdapter DatabaseAdapter)
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
		public void Read()
		{
			List<IAccount> accounts = SelectIAccountFromDatabaseTable();
			if (accounts != null && accounts.Count > 0)
			{
				mAccountCollection.Clear();
				for (int i = 0; i < accounts.Count; ++i)
				{
					mAccountCollection.Add(accounts[i].mName, accounts[i]);
				}
			}
		}
		public void Save()
		{
			// 先將資料庫中的所有資料刪除後再將當前 Account 資料寫入資料庫
			ClearDatabaseTableData(mTableNameOfAccount);
			InsertIAccountToDatabaseTable(mAccountCollection.Values);
		}
		public bool AddAccount(string Name, string Password, AccountRank Rank)
		{
			if (!mAccountCollection.Keys.Contains(Name))
			{
				mAccountCollection.Add(Name, GenerateIAccount(Name, Password, Rank));
				Save();
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool RemoveAccount(string Name)
		{
			if (mAccountCollection.Keys.Contains(Name))
			{
				mAccountCollection.Remove(Name);
				Save();
				return true;
			}
			else
			{
				return false;
			}
		}
		public IAccount CheckPassword(string Password)
		{
			return mAccountCollection.Values.FirstOrDefault(o => o.mPassword == Password);
		}

		private void InitializeDatabaseTable()
		{
			// 建立表格並插入初始資料
			CreateTableOfAccount();
			InsertDefaultAccountsToDatabaseTable();
		}
		private void CreateTableOfAccount()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfAccount} (";
			tmp += "Name TEXT UNIQUE, ";
			tmp += "Password TEXT UNIQUE, ";
			tmp += "Rank INTEGER)";
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		private void InsertDefaultAccountsToDatabaseTable()
		{
			List<string> tmp = new List<string>();
			tmp.Add($"INSERT OR IGNORE INTO {mTableNameOfAccount} VALUES ('CastecSoftware', '{EncryptString("castecsw")}', {(int)AccountRank.Software})");
			tmp.Add($"INSERT OR IGNORE INTO {mTableNameOfAccount} VALUES ('CastecService', '{EncryptString("castec0")}', {(int)AccountRank.Service})");
			tmp.Add($"INSERT OR IGNORE INTO {mTableNameOfAccount} VALUES ('Customer', '{EncryptString("customer")}', {(int)AccountRank.Customer})");
			rDatabaseAdapter.ExecuteNonQueryCommands(tmp);
		}
		private void ClearDatabaseTableData(string TableName)
		{
			// 刪除指定表格中的所有資料
			string tmp = string.Empty;
			tmp += $"DELETE FROM {TableName}";
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		private void InsertIAccountToDatabaseTable(IEnumerable<IAccount> Accounts)
		{
			// 將 Account 資料寫入資料庫
			List<string> cmds = new List<string>();
			foreach (IAccount account in Accounts.OrderBy(o => o.mRank))
			{
				cmds.Add(GenerateInsertAccountDataSqlCommand(account));
			}
			rDatabaseAdapter.ExecuteNonQueryCommands(cmds);
		}
		private string GenerateInsertAccountDataSqlCommand(IAccount Account)
		{
			// 產生將 Account 資料寫入資料庫的 Sql 語句
			return $"INSERT INTO {mTableNameOfAccount} VALUES ('{Account.mName}', '{EncryptString(Account.mPassword)}', {(int)Account.mRank})";
		}
		private List<IAccount> SelectIAccountFromDatabaseTable()
		{
			// 從資料庫取出所有的 Account 資料
			string cmd = $"SELECT * FROM {mTableNameOfAccount}";
			DataTable selectResult = rDatabaseAdapter.ExecuteQueryCommand(cmd)?.Tables[0];
			if (selectResult != null && selectResult.Rows != null && selectResult.Rows.Count > 0)
			{
				List<IAccount> result = new List<IAccount>();
				for (int i = 0; i < selectResult.Rows.Count; ++i)
				{
					result.Add(GenerateIAccount(selectResult.Rows[i].ItemArray[0].ToString(), DecryptString(selectResult.Rows[i].ItemArray[1].ToString()), (AccountRank)int.Parse(selectResult.Rows[i].ItemArray[2].ToString())));
				}
				return result;
			}
			else
			{
				return null;
			}
		}
	}
}
