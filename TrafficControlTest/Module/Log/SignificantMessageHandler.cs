using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Log
{
	public class SignificantMessageHandler : SystemWithLoopTask, ISignificantMessageHandler
	{
		public event EventHandler<SignificantMessageEventArgs> SignificantMessage;

		private readonly Queue<SignificantMessageEventArgs> mSignificantMessageCollection = new Queue<SignificantMessageEventArgs>();
		private readonly object mLockOfSignificantMessageCollection = new object();

		public void AddSignificantMessage(string OccurTime, string Category, string Message)
		{
			lock (mLockOfSignificantMessageCollection)
			{
				mSignificantMessageCollection.Enqueue(new SignificantMessageEventArgs(OccurTime, Category, Message));
			}
		}
		public override string GetSystemInfo()
		{
			return $"CountOfSignificantMessages: {mSignificantMessageCollection.Count}";
		}
		public override void Task()
		{
			Subtask_HandleSignificantMessageCollection();
		}

		protected virtual void RaiseEvent_SignificantMessage(string OccurTime, string Category, string Message, bool Sync = true)
		{
			RaiseEvent_SignificantMessage(new SignificantMessageEventArgs(OccurTime, Category, Message), Sync);
		}
		protected virtual void RaiseEvent_SignificantMessage(SignificantMessageEventArgs SignificantMessageEventArgs, bool Sync = true)
		{
			if (Sync)
			{
				SignificantMessage?.Invoke(this, SignificantMessageEventArgs);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { SignificantMessage?.Invoke(this, SignificantMessageEventArgs); });
			}
		}

		private void Subtask_HandleSignificantMessageCollection()
		{
			List<SignificantMessageEventArgs> SignificantMessages = null;
			lock (mLockOfSignificantMessageCollection)
			{
				if (mSignificantMessageCollection.Count > 0)
				{
					SignificantMessages = mSignificantMessageCollection.ToList();
					mSignificantMessageCollection.Clear();
				}
			}

			if (SignificantMessages != null && SignificantMessages.Count > 0)
			{
				for (int i = 0; i < SignificantMessages.Count; ++i)
				{
					RaiseEvent_SignificantMessage(SignificantMessages[i]);
				}
			}
		}
	}
}
