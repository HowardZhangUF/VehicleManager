using System;

namespace LibraryForVM
{
	public class TimeElapseDetector : SystemWithLoopTask, ITimeElapseDetector
	{
		public event EventHandler<DateTimeChangedEventArgs> YearChanged;
		public event EventHandler<DateTimeChangedEventArgs> MonthChanged;
		public event EventHandler<DateTimeChangedEventArgs> DayChanged;
		public event EventHandler<DateTimeChangedEventArgs> HourChanged;
		public event EventHandler<DateTimeChangedEventArgs> MinuteChanged;

		private DateTime mLastTimestamp { get; set; } = DateTime.Now;
		private DateTime mCurrentTimestamp { get; set; } = DateTime.Now;

		public override string GetSystemInfo()
		{
			return $"LastTimestamp: {mLastTimestamp.ToString("yyyy/MM/dd HH:mm:ss.fff")}, CurrentTimestamp: {mCurrentTimestamp.ToString("yyyy/MM/dd HH:mm:ss.fff")}";
		}
		public override void Task()
		{
			mCurrentTimestamp = DateTime.Now;
			Subtask_CheckYear(mLastTimestamp, mCurrentTimestamp);
			Subtask_CheckMonth(mLastTimestamp, mCurrentTimestamp);
			Subtask_CheckDay(mLastTimestamp, mCurrentTimestamp);
			Subtask_CheckHour(mLastTimestamp, mCurrentTimestamp);
			Subtask_CheckMinute(mLastTimestamp, mCurrentTimestamp);
			mLastTimestamp = mCurrentTimestamp;
		}

		protected virtual void RaiseEvent_YearChanged(DateTimeChangedEventArgs EventArgs, bool Sync = true)
		{
			if (Sync)
			{
				YearChanged?.Invoke(this, EventArgs);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { YearChanged?.Invoke(this, EventArgs); });
			}
		}
		protected virtual void RaiseEvent_MonthChanged(DateTimeChangedEventArgs EventArgs, bool Sync = true)
		{
			if (Sync)
			{
				MonthChanged?.Invoke(this, EventArgs);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { MonthChanged?.Invoke(this, EventArgs); });
			}
		}
		protected virtual void RaiseEvent_DayChanged(DateTimeChangedEventArgs EventArgs, bool Sync = true)
		{
			if (Sync)
			{
				DayChanged?.Invoke(this, EventArgs);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { DayChanged?.Invoke(this, EventArgs); });
			}
		}
		protected virtual void RaiseEvent_HourChanged(DateTimeChangedEventArgs EventArgs, bool Sync = true)
		{
			if (Sync)
			{
				HourChanged?.Invoke(this, EventArgs);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { HourChanged?.Invoke(this, EventArgs); });
			}
		}
		protected virtual void RaiseEvent_MinuteChanged(DateTimeChangedEventArgs EventArgs, bool Sync = true)
		{
			if (Sync)
			{
				MinuteChanged?.Invoke(this, EventArgs);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { MinuteChanged?.Invoke(this, EventArgs); });
			}
		}

		private void Subtask_CheckYear(DateTime LastTimestamp, DateTime CurrentTimestamp)
		{
			if (LastTimestamp.Year != CurrentTimestamp.Year)
			{
				RaiseEvent_YearChanged(new DateTimeChangedEventArgs(DateTime.Now, LastTimestamp.Year, CurrentTimestamp.Year));
			}
		}
		private void Subtask_CheckMonth(DateTime LastTimestamp, DateTime CurrentTimestamp)
		{
			if (LastTimestamp.Month != CurrentTimestamp.Month)
			{
				RaiseEvent_MonthChanged(new DateTimeChangedEventArgs(DateTime.Now, LastTimestamp.Month, CurrentTimestamp.Month));
			}
		}
		private void Subtask_CheckDay(DateTime LastTimestamp, DateTime CurrentTimestamp)
		{
			if (LastTimestamp.Day != CurrentTimestamp.Day)
			{
				RaiseEvent_DayChanged(new DateTimeChangedEventArgs(DateTime.Now, LastTimestamp.Day, CurrentTimestamp.Day));
			}
		}
		private void Subtask_CheckHour(DateTime LastTimestamp, DateTime CurrentTimestamp)
		{
			if (LastTimestamp.Hour != CurrentTimestamp.Hour)
			{
				RaiseEvent_HourChanged(new DateTimeChangedEventArgs(DateTime.Now, LastTimestamp.Hour, CurrentTimestamp.Hour));
			}
		}
		private void Subtask_CheckMinute(DateTime LastTimestamp, DateTime CurrentTimestamp)
		{
			if (LastTimestamp.Minute != CurrentTimestamp.Minute)
			{
				RaiseEvent_MinuteChanged(new DateTimeChangedEventArgs(DateTime.Now, LastTimestamp.Minute, CurrentTimestamp.Minute));
			}
		}
	}
}
