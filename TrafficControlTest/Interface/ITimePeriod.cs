using System;

namespace TrafficControlTest.Interface
{
	public interface ITimePeriod
	{
		DateTime mStart { get; }
		DateTime mEnd { get; }
		TimeSpan mDuration { get; }

		void Set(DateTime Start, DateTime End);
		string ToString(string TimeFormat);
	}
}
