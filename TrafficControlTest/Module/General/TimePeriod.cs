using System;

namespace TrafficControlTest.Module.General
{
	public class TimePeriod : ITimePeriod
	{
		public DateTime mStart { get; private set; }
		public DateTime mEnd { get; private set; }
		public TimeSpan mDuration { get { return mEnd.Subtract(mStart); } }

		public TimePeriod(DateTime Start, DateTime End)
		{
			Set(Start, End);
		}
		public void Set(DateTime Start, DateTime End)
		{
			mStart = Start;
			mEnd = End;
		}
		public override string ToString()
		{
			return ToString(Library.Library.TIME_FORMAT);
		}
		public string ToString(string TimeFormat)
		{
			return $"{mStart.ToString(TimeFormat)} ({mStart.Subtract(DateTime.Now).TotalSeconds.ToString("F2")} sec) ~ {mEnd.ToString(TimeFormat)} ({mEnd.Subtract(DateTime.Now).TotalSeconds.ToString("F2")} sec)";
		}
	}
}
