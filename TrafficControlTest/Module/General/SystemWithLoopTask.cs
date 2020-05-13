using System;
using System.Threading;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General
{
	public abstract class SystemWithLoopTask : SystemWithConfig, ISystemWithLoopTask
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
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					mTimePeriod = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}

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
