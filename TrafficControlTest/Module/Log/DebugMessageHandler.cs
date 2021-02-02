using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Log
{
	public class DebugMessageHandler : SystemWithLoopTask, IDebugMessageHandler
	{
		public event EventHandler<DebugMessageEventArgs> DebugMessage;

		private readonly Queue<DebugMessageEventArgs> mDebugMessageCollection = new Queue<DebugMessageEventArgs>();
		private readonly object mLockOfDebugMessageCollection = new object();

		public void AddDebugMessage(string OccurTime, string Category, string SubCategory, string Message)
		{
			lock (mLockOfDebugMessageCollection)
			{
				mDebugMessageCollection.Enqueue(new DebugMessageEventArgs(OccurTime, Category, SubCategory, Message));
			}
		}
		public override string GetSystemInfo()
		{
			return $"CountOfDebugMessages: {mDebugMessageCollection.Count}";
		}
		public override void Task()
		{
			Subtask_HandleDebugMessageCollection();
		}

		protected virtual void RaiseEvent_DebugMessage(string OccurTime, string Category, string SubCategory, string Message, bool Sync = true)
		{
			RaiseEvent_DebugMessage(new DebugMessageEventArgs(OccurTime, Category, SubCategory, Message), Sync);
		}
		protected virtual void RaiseEvent_DebugMessage(DebugMessageEventArgs DebugMessageEventArgs, bool Sync = true)
		{
			if (Sync)
			{
				DebugMessage?.Invoke(this, DebugMessageEventArgs);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { DebugMessage?.Invoke(this, DebugMessageEventArgs); });
			}
		}

		private void Subtask_HandleDebugMessageCollection()
		{
			List<DebugMessageEventArgs> debugMessages = null;
			lock (mLockOfDebugMessageCollection)
			{
				if (mDebugMessageCollection.Count > 0)
				{
					debugMessages = mDebugMessageCollection.ToList();
					mDebugMessageCollection.Clear();
				}
			}

			if (debugMessages != null && debugMessages.Count > 0)
			{
				for (int i = 0; i < debugMessages.Count; ++i)
				{
					RaiseEvent_DebugMessage(debugMessages[i]);
				}
			}
		}
	}
}
