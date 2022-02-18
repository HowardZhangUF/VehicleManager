using System;

namespace TrafficControlTest.Module.CollisionEvent
{
	public interface ITimePeriod
	{
		DateTime mStart { get; }
		DateTime mEnd { get; }
		/// <summary>指定時間區間的大小</summary>
		TimeSpan mDuration { get; }

		void Set(DateTime Start, DateTime End);
		string ToString();
		string ToString(string TimeFormat);
	}
}
