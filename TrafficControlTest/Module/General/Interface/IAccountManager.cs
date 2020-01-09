using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.General.Interface
{
	/// <summary>
	/// - 提供管理帳號的方法
	/// - 提供新增、移除帳號的方法
	/// - 提供確認帳號資訊的方法
	/// </summary>
	public interface IAccountManager
	{
		void Set(DatabaseAdapter DatabaseAdapter);
		void Read();
		void Save();
		bool AddAccount(string Name, string Password, AccountRank Rank);
		bool RemoveAccount(string Name);
		IAccount CheckPassword(string Password);
	}
}
