using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Implement
{
    public abstract class SystemWithLoopTask : ISystemWithLoopTask
    {
        public event EventHandlerDateTime SystemStarted;
        public event EventHandlerDateTime SystemStopped;

        public bool mIsExecuting 
        {
            get
            {
                return _IsExecuting;
            }
            private set
            {
                _IsExecuting = value;
                if (_IsExecuting) RaiseEvent_SystemStarted();
                else RaiseEvent_SystemStopped();
            }
        }
        public int mTimePeriod { get; set; } = 500;

        private Thread mThdLoop = null;
        private bool[] mThdLoopExitFlag = null;
        private bool _IsExecuting = false;

        public void Start()
        {
            InitializeThread();
        }
        public void Stop()
        {
            DestroyThread();
        }
        public abstract void Task();

        protected virtual void RaiseEvent_SystemStarted(bool Sync = true)
        {
            if (Sync)
            {
                SystemStarted?.Invoke(DateTime.Now);
            }
            else
            {
				System.Threading.Tasks.Task.Run(() => { SystemStarted?.Invoke(DateTime.Now); });
            }
        }
        protected virtual void RaiseEvent_SystemStopped(bool Sync = true)
        {
            if (Sync)
            {
                SystemStopped?.Invoke(DateTime.Now);
            }
            else
            {
				System.Threading.Tasks.Task.Run(() => { SystemStopped?.Invoke(DateTime.Now); });
            }
        }

        private void InitializeThread()
        {
            mThdLoopExitFlag = new bool[] { false };
            mThdLoop = new Thread(() => LoopTask(mThdLoopExitFlag));
            mThdLoop.IsBackground = true;
            mThdLoop.Start();
        }
        private void DestroyThread()
        {
            if (mThdLoop != null)
            {
                if (mThdLoop.IsAlive)
                {
                    mThdLoopExitFlag[0] = true;
                }
                mThdLoop = null;
            }
        }
        private void LoopTask(bool[] ExitFlag)
        {
            try
            {
                mIsExecuting = true;
                while (!ExitFlag[0])
                {
                    Task();
                    Thread.Sleep(mTimePeriod);
                }
            }
            finally
            {
                mIsExecuting = false;
            }
        }
    }
}
