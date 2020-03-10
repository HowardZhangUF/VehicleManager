using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Interface
{
    public interface ISystemWithLoopTask
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
