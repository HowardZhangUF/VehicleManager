using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.General
{
	public interface ITimeElapseDetector : ISystemWithLoopTask
	{
		event EventHandler<DateTimeChangedEventArgs> YearChanged;
		event EventHandler<DateTimeChangedEventArgs> MonthChanged;
		event EventHandler<DateTimeChangedEventArgs> DayChanged;
		event EventHandler<DateTimeChangedEventArgs> HourChanged;
		event EventHandler<DateTimeChangedEventArgs> MinuteChanged;
	}

	public class DateTimeChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public int OldValue { get; private set; }
		public int NewValue { get; private set; }

		public DateTimeChangedEventArgs(DateTime OccurTime, int OldValue, int NewValue)
		{
			this.OccurTime = OccurTime;
			this.OldValue = OldValue;
			this.NewValue = NewValue;
		}
		public override string ToString()
		{
			return $"OldValue: {OldValue}, NewValue: {NewValue}";
		}
	}
}
