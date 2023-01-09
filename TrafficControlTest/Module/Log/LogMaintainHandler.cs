using LibraryForVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using System.Globalization;

namespace TrafficControlTest.Module.Log
{
	public class LogMaintainHandler : SystemWithConfig, ILogMaintainHandler
	{
		public event EventHandler<BackupCurrentLogExecutedEventArgs> BackupCurrentLogExecuted;
		public event EventHandler<DeleteOldLogExecutedEventArgs> DeleteOldLogExecuted;

		private IHistoryLogAdapter rHistoryLogAdapter = null;
		private ITimeElapseDetector rTimeElapseDetector = null;
		private int mDayOfWeekOfBackupCurrentLog { get; set; } = 1;//*原本為5,0
		private int mDayOfWeekOfDeleteOldLog { get; set; } = 1;//*原本為15,0

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
			mDayOfWeekOfBackupCurrentLog = 1;//1表星期一 0為星期天 目前先寫死
            Console.WriteLine($"有進入到備份函式 {(int)DateTime.Now.DayOfWeek}==?{mDayOfWeekOfBackupCurrentLog}");
			if ((int)DateTime.Now.DayOfWeek == mDayOfWeekOfBackupCurrentLog)//* 原本為DateTime.Now.Day
			{
				int week = GetIso8601WeekOfYear(DateTime.Now.AddDays(-1));
				int year = DateTime.Now.Year;

				//ex 2023/1/2 52W 但應歸為 2022第52W  
				if (DateTime.Now.Month == 1 && week >= 52)
					year -= 1;
				else if (DateTime.Now.Month == 12 && week == 1)
					year += 1;


				string fileName = $"HistoryLog{year}-{week}W.db";
				bool result = rHistoryLogAdapter.BackupHistoryLogToFile(fileName);
				if (result) RaiseEvent_BackupCurrentLogExecuted();
			}
		}
		public void DeleteOldLog()
		{
			mDayOfWeekOfDeleteOldLog = 2;//1表星期一 0為星期天 目前先寫死
            Console.WriteLine($"有進入刪除函式 {(int)DateTime.Now.DayOfWeek}==?{mDayOfWeekOfDeleteOldLog}");
			if ((int)DateTime.Now.DayOfWeek == mDayOfWeekOfDeleteOldLog)//* 原本為DateTime.Now.Day
			{				
				DateTime date = DateTime.Now.AddDays(-1).Date;
				bool result = rHistoryLogAdapter.DeleteHistoryLogBefore(date);//刪除星期一以前的
				if (result) RaiseEvent_DeleteOldLogExecuted();
			}
		}
		/// <summary>Iso8601 週數計算格式 以禮拜一當作第一天 </summary>
		public static int GetIso8601WeekOfYear(DateTime time)
		{
			// Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
			// be the same week# as whatever Thursday, Friday or Saturday are,
			// and we always get those right
			DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
			if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
			{
				time = time.AddDays(3);
			}

			// Return the week of our adjusted day
			return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "DayOfWeekOfBackupCurrentLog", "DayOfWeekOfDeleteOldLog" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "DayOfWeekOfBackupCurrentLog":
					return mDayOfWeekOfBackupCurrentLog.ToString();
				case "DayOfWeekOfDeleteOldLog":
					return mDayOfWeekOfDeleteOldLog.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "DayOfWeekOfBackupCurrentLog":
					mDayOfWeekOfBackupCurrentLog = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "DayOWeekOfDeleteOldLog":
					mDayOfWeekOfDeleteOldLog = int.Parse(NewValue);
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
            Console.WriteLine($"有進入觸發一天長度事件");
			Console.WriteLine($"舊值:{Args.OldValue},{mDayOfWeekOfBackupCurrentLog-1}");
            Console.WriteLine($"新值:{Args.NewValue},{mDayOfWeekOfBackupCurrentLog}");
            BackupCurrentLog();
            DeleteOldLog();
            //每個月的 a 號複製當前 Log
            //if (Args.OldValue == mDayOfWeekOfBackupCurrentLog - 1 && Args.NewValue == mDayOfWeekOfBackupCurrentLog)
            //{
            //    BackupCurrentLog();
            //}
            ////每個月的 b 號刪除舊的 Log
            //if (Args.OldValue == mDayOfWeekOfDeleteOldLog - 1 && Args.NewValue == mDayOfWeekOfDeleteOldLog)
            //{
            //    DeleteOldLog();
            //}
        }
    }
}
