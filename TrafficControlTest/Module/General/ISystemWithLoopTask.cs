using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General
{
	public interface ISystemWithLoopTask : ISystemWithConfig
	{
        event EventHandlerDateTime SystemStarted;
        event EventHandlerDateTime SystemStopped;

        bool mIsExecuting { get; }
        int mTimePeriod { get; set; }

        void Start();
        void Stop();
		/// <summary>此方法將被迴圈執行</summary>
        void Task();
    }
}
