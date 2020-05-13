using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Account
{
	/// <summary>
	/// - Reference: IAccountManager
	/// - 提供登入、登出方法
	/// - 提供確認當前登入使用者的方法
	/// </summary>
	public interface IAccessControl
	{
		event EventHandlerLogInOutEvent UserLogIn;
		event EventHandlerLogInOutEvent UserLogOut;

		string mCurrentUser { get; }
		AccountRank mCurrentRank { get; }

		void Set(IAccountManager AccountManager);
		bool LogIn(string Password);
		bool LogOut();
	}
}
