using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Account
{
	/// <summary>
	/// - Reference: IAccountManager
	/// - 提供登入、登出方法
	/// - 提供確認當前登入使用者的方法
	/// </summary>
	public interface IAccessControl
	{
		event EventHandler<UserLogChangedEventArgs> UserLogChanged;

		string mCurrentUser { get; }
		AccountRank mCurrentRank { get; }

		void Set(IAccountManager AccountManager);
		bool LogIn(string Password);
		bool LogOut();
	}

	public class UserLogChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string UserName { get; private set; }
		public AccountRank UserRank { get; private set; }
		public bool IsLogin { get; private set; }

		public UserLogChangedEventArgs(DateTime OccurTime, string UserName, AccountRank UserRank, bool IsLogin) : base()
		{
			this.OccurTime = OccurTime;
			this.UserName = UserName;
			this.UserRank = UserRank;
			this.IsLogin = IsLogin;
		}
	}
}
