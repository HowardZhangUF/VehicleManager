using System;
using System.Threading;

namespace TrafficControlTest.Module.General
{
	public abstract class SystemWithLoopTask : SystemWithConfig, ISystemWithLoopTask
	{
		public event EventHandler<SystemStatusChangedEventArgs> SystemStatusChanged;

		public bool mIsExecuting
		{
			get
			{
				return _IsExecuting;
			}
			private set
			{
				_IsExecuting = value;
				RaiseEvent_SystemStatusChanged(_IsExecuting);
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
		public override string[] GetConfigNameList()
		{
			return new string[] { "TimePeriod" };
		}
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

		protected virtual void RaiseEvent_SystemStatusChanged(bool SystemNewStatus, bool Sync = true)
		{
			if (Sync)
			{
				SystemStatusChanged?.Invoke(this, new SystemStatusChangedEventArgs(DateTime.Now, SystemNewStatus));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { SystemStatusChanged?.Invoke(this, new SystemStatusChangedEventArgs(DateTime.Now, SystemNewStatus)); });
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
					try
					{
						Task();
						Thread.Sleep(mTimePeriod);
					}
					catch (Exception Ex)
					{
						Library.ExceptionHandling.HandleException(Ex);
					}
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
			finally
			{
				mIsExecuting = false;
			}
		}
	}
}
