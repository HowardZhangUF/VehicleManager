using System;
using System.Threading.Tasks;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Account
{
	public class AccessControl : IAccessControl
	{
		public event EventHandler<UserLogChangedEventArgs> UserLogChanged;

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
					RaiseEvent_UserLogChagned(mCurrentUser, mCurrentRank, true);
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
				RaiseEvent_UserLogChagned(mLastUser, mLastRank, false);
				return true;
			}
			else
			{
				return false;
			}
		}

		protected virtual void RaiseEvent_UserLogChagned(string UserName, AccountRank UserRank, bool IsLogin, bool Sync = true)
		{
			if (Sync)
			{
				UserLogChanged?.Invoke(this, new UserLogChangedEventArgs(DateTime.Now, UserName, UserRank, IsLogin));
			}
			else
			{
				Task.Run(() => { UserLogChanged?.Invoke(this, new UserLogChangedEventArgs(DateTime.Now, UserName, UserRank, IsLogin)); });
			}
		}
	}
}
