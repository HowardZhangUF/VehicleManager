using System;

namespace TrafficControlTest.Module.General
{
	public interface ISystemWithLoopTask : ISystemWithConfig
	{
		event EventHandler<SystemStatusChangedEventArgs> SystemStatusChanged;

        bool mIsExecuting { get; }
        int mTimePeriod { get; set; }

        void Start();
        void Stop();
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
}
