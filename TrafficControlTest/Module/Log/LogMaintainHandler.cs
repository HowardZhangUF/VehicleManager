using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Log
{
	public class LogMaintainHandler : SystemWithConfig, ILogMaintainHandler
	{
		public event EventHandler<BackupCurrentLogExecutedEventArgs> BackupCurrentLogExecuted;
		public event EventHandler<DeleteOldLogExecutedEventArgs> DeleteOldLogExecuted;

		private IHistoryLogAdapter rHistoryLogAdapter = null;
		private ITimeElapseDetector rTimeElapseDetector = null;
		private int mDayOfMonthOfBackupCurrentLog { get; set; } = 5;
		private int mDayOfMonthOfDeleteOldLog { get; set; } = 15;

		public LogMaintainHandler(IHistoryLogAdapter HistoryLogAdapter, ITimeElapseDetector TimeElapseDetector)
		{
			Set(HistoryLogAdapter, TimeElapseDetector);
		}
		public void Set(IHistoryLogAdapter HistoryLogAdapter)
		{
			rHistoryLogAdapter = HistoryLogAdapter;
		}
		public void Set(ITimeElapseDetector TimeElapseDetector)
		{
			UnsubscribeEvent_ITimeElapseDetector(TimeElapseDetector);
			rTimeElapseDetector = TimeElapseDetector;
			SubscribeEvent_ITimeElapseDetector(TimeElapseDetector);
		}
		public void Set(IHistoryLogAdapter HistoryLogAdapter, ITimeElapseDetector TimeElapseDetector)
		{
			Set(HistoryLogAdapter);
			Set(TimeElapseDetector);
		}
		public void BackupCurrentLog()
		{
			if (DateTime.Now.Day >= mDayOfMonthOfBackupCurrentLog)
			{
				DateTime date = DateTime.Now.AddMonths(-1);
				string fileName = $"HistoryLog{date.ToString("yyyyMM")}.db";

				bool result = rHistoryLogAdapter.BackupHistoryLogToFile(fileName);
				if (result) RaiseEvent_BackupCurrentLogExecuted();
			}
		}
		public void DeleteOldLog()
		{
			if (DateTime.Now.Day >= mDayOfMonthOfDeleteOldLog)
			{
				DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

				bool result = rHistoryLogAdapter.DeleteHistoryLogBefore(date);
				if (result) RaiseEvent_DeleteOldLogExecuted();
			}
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "DayOfMonthOfBackupCurrentLog", "DayOfMonthOfDeleteOldLog" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "DayOfMonthOfBackupCurrentLog":
					return mDayOfMonthOfBackupCurrentLog.ToString();
				case "DayOfMonthOfDeleteOldLog":
					return mDayOfMonthOfDeleteOldLog.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "DayOfMonthOfBackupCurrentLog":
					mDayOfMonthOfBackupCurrentLog = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "DayOfMonthOfDeleteOldLog":
					mDayOfMonthOfDeleteOldLog = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}

		protected virtual void RaiseEvent_BackupCurrentLogExecuted(bool Sync = true)
		{
			if (Sync)
			{
				BackupCurrentLogExecuted?.Invoke(this, new BackupCurrentLogExecutedEventArgs(DateTime.Now));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { BackupCurrentLogExecuted?.Invoke(this, new BackupCurrentLogExecutedEventArgs(DateTime.Now)); });
			}
		}
		protected virtual void RaiseEvent_DeleteOldLogExecuted(bool Sync = true)
		{
			if (Sync)
			{
				DeleteOldLogExecuted?.Invoke(this, new DeleteOldLogExecutedEventArgs(DateTime.Now));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { DeleteOldLogExecuted?.Invoke(this, new DeleteOldLogExecutedEventArgs(DateTime.Now)); });
			}
		}

		private void SubscribeEvent_ITimeElapseDetector(ITimeElapseDetector TimeElapseDetector)
		{
			if (TimeElapseDetector != null)
			{
				TimeElapseDetector.DayChanged += HandleEvent_TimeElapseDetectorDayChanged;
			}
		}
		private void UnsubscribeEvent_ITimeElapseDetector(ITimeElapseDetector TimeElapseDetector)
		{
			if (TimeElapseDetector != null)
			{
				TimeElapseDetector.DayChanged -= HandleEvent_TimeElapseDetectorDayChanged;
			}
		}
		private void HandleEvent_TimeElapseDetectorDayChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			// 每個月的 a 號複製當前 Log
			if (Args.OldValue == mDayOfMonthOfBackupCurrentLog - 1 && Args.NewValue == mDayOfMonthOfBackupCurrentLog)
			{
				BackupCurrentLog();
			}
			// 每個月的 b 號刪除舊的 Log
			if (Args.OldValue == mDayOfMonthOfDeleteOldLog - 1 && Args.NewValue == mDayOfMonthOfDeleteOldLog)
			{
				DeleteOldLog();
			}
		}
	}
}
