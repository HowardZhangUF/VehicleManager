using System;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Account
{
	public class AccessControl : IAccessControl
	{
		public event EventHandlerLogInOutEvent UserLogIn;
		public event EventHandlerLogInOutEvent UserLogOut;

		public string mCurrentUser { get; private set; } = string.Empty;
		public AccountRank mCurrentRank { get; private set; } = AccountRank.None;

		private IAccountManager rAccountManager = null;

		public AccessControl(IAccountManager AccountManager)
		{
			Set(AccountManager);
		}
		public void Set(IAccountManager AccountManager)
		{
			if (AccountManager != null)
			{
				rAccountManager = AccountManager;
			}
		}
		public bool LogIn(string Password)
		{
			if (string.IsNullOrEmpty(mCurrentUser))
			{
				IAccount tmp = rAccountManager.CheckPassword(Password);
				if (tmp != null)
				{
					mCurrentUser = tmp.mName;
					mCurrentRank = tmp.mRank;
					RaiseEvent_UserLogIn(mCurrentUser, mCurrentRank);
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		public bool LogOut()
		{
			if (!string.IsNullOrEmpty(mCurrentUser))
			{
				string mLastUser = mCurrentUser;
				AccountRank mLastRank = mCurrentRank;
				mCurrentUser = string.Empty;
				mCurrentRank = AccountRank.None;
				RaiseEvent_UserLogOut(mLastUser, mLastRank);
				return true;
			}
			else
			{
				return false;
			}
		}

		protected virtual void RaiseEvent_UserLogIn(string Name, AccountRank Rank, bool Sync = true)
		{
			if (Sync)
			{
				UserLogIn?.Invoke(DateTime.Now, Name, Rank);
			}
			else
			{
				Task.Run(() => { UserLogIn?.Invoke(DateTime.Now, Name, Rank); });
			}
		}
		protected virtual void RaiseEvent_UserLogOut(string Name, AccountRank Rank, bool Sync = true)
		{
			if (Sync)
			{
				UserLogOut?.Invoke(DateTime.Now, Name, Rank);
			}
			else
			{
				Task.Run(() => { UserLogOut?.Invoke(DateTime.Now, Name, Rank); });
			}
		}
	}
}
