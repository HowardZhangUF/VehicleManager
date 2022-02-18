using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.Log
{
	/// <summary>
	/// 提供 BackupCurrentLog() 與 DeleteOldLog() 方法以在程式開啟時可供呼叫
	/// 將會於指定日進行 BackupCurrentLog() 與 DeleteOldLog() 的動作
	/// </summary>
	public interface ILogMaintainHandler : ISystemWithConfig
	{
		event EventHandler<BackupCurrentLogExecutedEventArgs> BackupCurrentLogExecuted;
		event EventHandler<DeleteOldLogExecutedEventArgs> DeleteOldLogExecuted;

		void Set(IHistoryLogAdapter HistoryLogAdapter);
		void Set(ITimeElapseDetector TimeElapseDetector);
		void Set(IHistoryLogAdapter HistoryLogAdapter, ITimeElapseDetector TimeElapseDetector);
		void BackupCurrentLog();
		void DeleteOldLog();
	}

	public class BackupCurrentLogExecutedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }

		public BackupCurrentLogExecutedEventArgs(DateTime OccurTime) : base()
		{
			this.OccurTime = OccurTime;
		}
	}

	public class DeleteOldLogExecutedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }

		public DeleteOldLogExecutedEventArgs(DateTime OccurTime) : base()
		{
			this.OccurTime = OccurTime;
		}
	}
}
