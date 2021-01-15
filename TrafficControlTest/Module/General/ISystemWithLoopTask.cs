using System;

namespace TrafficControlTest.Module.General
{
	public interface ISystemWithLoopTask : ISystemWithConfig
	{
		event EventHandler<SystemStatusChangedEventArgs> SystemStatusChanged;
		event EventHandler<SystemInfoReportedEventArgs> SystemInfoReported;

		bool mIsExecuting { get; }
		int mTimePeriod { get; set; }

		void Start();
		void Stop();
		void ReportSystemInfo();
		string GetSystemInfo();
		/// <summary>此方法將被迴圈執行</summary>
		void Task();
	}

	public class SystemStatusChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public bool SystemNewStatus { get; private set; }

		public SystemStatusChangedEventArgs(DateTime OccurTime, bool SystemNewStatus) : base()
		{
			this.OccurTime = OccurTime;
			this.SystemNewStatus = SystemNewStatus;
		}
	}

	public class SystemInfoReportedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string SystemInfo { get; private set; }

		public SystemInfoReportedEventArgs(DateTime OccurTime, string SystemInfo) : base()
		{
			this.OccurTime = OccurTime;
			this.SystemInfo = SystemInfo;
		}
	}
}
