using System;

namespace LibraryForVM
{
	/// <summary>
	/// - 儲存物件的識別碼
	/// - 物件的資訊更新時會拋出事件
	/// </summary>
	public interface IItem
	{
		event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		string mName { get; }
	}

	public class StatusUpdatedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string ItemName { get; private set; }
		public string StatusName { get; private set; }

		public StatusUpdatedEventArgs(DateTime OccurTime, string ItemName, string StatusName) : base()
		{
			this.OccurTime = OccurTime;
			this.ItemName = ItemName;
			this.StatusName = StatusName;
		}
	}
}
