using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Account
{
	public class Account : IAccount
	{
		public string mName { get; private set; } = string.Empty;
		public string mPassword { get; private set; } = string.Empty;
		public AccountRank mRank { get; private set; } = AccountRank.None;

		public Account(string Name, string Password, AccountRank Rank)
		{
			Set(Name, Password, Rank);
		}
		public void Set(string Name, string Password, AccountRank Rank)
		{
			mName = Name;
			mPassword = Password;
			mRank = Rank;
		}
	}
}
