using AsyncSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Implement
{
	public class HostCommunicator : IHostCommunicator
	{
		public event EventHandlerDateTime SystemStarted;
		public event EventHandlerDateTime SystemStopped;
		public event EventHandlerLocalListenState LocalListenStateChanged;
		public event EventHandlerRemoteConnectState RemoteConnectStateChanged;
		public event EventHandlerSentString SentString;
		public event EventHandlerReceivedString ReceivedString;

		public ListenState mListenState { get { return mServer.ListenStatus == EListenStatus.Idle ? ListenState.Closed : ListenState.Listening; } }
		public int mClientCout { get { return (mServer == null || mServer.ListenStatus == EListenStatus.Idle) ? 0 : mServer.ClientCount; } }

		private Server mServer = null;
		private int mListenPort = 9000;
		private readonly Queue<EventArgs> mServerEvents = new Queue<EventArgs>();
		private readonly object mLockOfServerEvents = new object();
		private Thread mThdHandleServerEvents = null;
		private bool[] mThdHandleServerEventsExitFlag = null;

		public HostCommunicator()
		{
			Constructor();
		}
		public void SetConfigOfListenPort(int Port)
		{
			mListenPort = Port;
		}
		public int GetConfigOfListenPort()
		{
			return mListenPort;
		}
		public void StartListen()
		{
			if (mServer.ListenStatus == EListenStatus.Idle)
			{
				InitializeThread();
				mServer.StartListening(mListenPort);
			}
		}
		public void StopListen()
		{
			if (mServer.ListenStatus == EListenStatus.Listening)
			{
				mServer.StopListen();
				DestroyThread();
			}
		}
		public void SendString(string IpPort, string Data)
		{
			if (mServer.ClientDictionary.Keys.Contains(IpPort))
			{
				mServer.Send(IpPort, Data);
				RaiseEvent_SentString(IpPort, Data);
			}
		}

		~HostCommunicator()
		{
			Destructor();
		}
		private void Constructor()
		{
			if (mServer == null)
			{
				mServer = new Server();
				SubscribeEvent_Server(mServer);
			}
		}
		private void Destructor()
		{
			if (mServer != null)
			{
				UnsubscribeEvent_Server(mServer);
				mServer = null;
			}
		}
		private void SubscribeEvent_Server(Server Server)
		{
			if (Server != null)
			{
				Server.ListenStatusChangedEvent += HandleEvent_ServerEvent;
				Server.ConnectStatusChangedEvent += HandleEvent_ServerEvent;
				Server.ReceivedDataEvent += HandleEvent_ServerEvent;
			}
		}
		private void UnsubscribeEvent_Server(Server Server)
		{
			if (Server != null)
			{
				Server.ListenStatusChangedEvent -= HandleEvent_ServerEvent;
				Server.ConnectStatusChangedEvent -= HandleEvent_ServerEvent;
				Server.ReceivedDataEvent -= HandleEvent_ServerEvent;
			}
		}
		private void InitializeThread()
		{
			mThdHandleServerEventsExitFlag = new bool[] { false };
			mThdHandleServerEvents = new Thread(() => Task_HandleServerEvents(mThdHandleServerEventsExitFlag));
			mThdHandleServerEvents.IsBackground = true;
			mThdHandleServerEvents.Start();
		}
		private void DestroyThread()
		{
			if (mThdHandleServerEvents != null)
			{
				if (mThdHandleServerEvents.IsAlive)
				{
					mThdHandleServerEventsExitFlag[0] = true;
				}
				mThdHandleServerEvents = null;
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
				Task.Run(() => { SystemStarted?.Invoke(DateTime.Now); });
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
				Task.Run(() => { SystemStopped?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_LocalListenStateChanged(ListenState NewState, bool Sync = true)
		{
			if (Sync)
			{
				LocalListenStateChanged?.Invoke(DateTime.Now, NewState);
			}
			else
			{
				Task.Run(() => { LocalListenStateChanged?.Invoke(DateTime.Now, NewState); });
			}
		}
		protected virtual void RaiseEvent_RemoteConnectStateChanged(string IpPort, ConnectState NewState, bool Sync = true)
		{
			if (Sync)
			{
				RemoteConnectStateChanged?.Invoke(DateTime.Now, IpPort, NewState);
			}
			else
			{
				Task.Run(() => { RemoteConnectStateChanged?.Invoke(DateTime.Now, IpPort, NewState); });
			}
		}
		protected virtual void RaiseEvent_SentString(string IpPort, string Data, bool Sync = true)
		{
			if (Sync)
			{
				SentString?.Invoke(DateTime.Now, IpPort, Data);
			}
			else
			{
				Task.Run(() => { SentString?.Invoke(DateTime.Now, IpPort, Data); });
			}
		}
		protected virtual void RaiseEvent_ReceivedString(string IpPort, string Data, bool Sync = true)
		{
			if (Sync)
			{
				ReceivedString?.Invoke(DateTime.Now, IpPort, Data);
			}
			else
			{
				Task.Run(() => { ReceivedString?.Invoke(DateTime.Now, IpPort, Data); });
			}
		}
		private void HandleEvent_ServerEvent(object Sender, EventArgs E)
		{
			lock (mLockOfServerEvents)
			{
				mServerEvents.Enqueue(E);
			}
		}
		private void HandleServerEvent(EventArgs E)
		{
			if (E is ListenStatusChangedEventArgs)
			{
				HandleServerEvent(E as ListenStatusChangedEventArgs);
			}
			else if (E is ConnectStatusChangedEventArgs)
			{
				HandleServerEvent(E as ConnectStatusChangedEventArgs);
			}
			else if (E is ReceivedDataEventArgs)
			{
				HandleServerEvent(E as ReceivedDataEventArgs);
			}
		}
		private void HandleServerEvent(ListenStatusChangedEventArgs E)
		{
			RaiseEvent_LocalListenStateChanged(E.ListenStatus == EListenStatus.Listening ? ListenState.Listening : ListenState.Closed);
		}
		private void HandleServerEvent(ConnectStatusChangedEventArgs E)
		{
			RaiseEvent_RemoteConnectStateChanged(E.RemoteInfo.ToString(), E.ConnectStatus == EConnectStatus.Connect ? ConnectState.Connected : ConnectState.Disconnected);
		}
		private void HandleServerEvent(ReceivedDataEventArgs E)
		{
			RaiseEvent_ReceivedString(E.RemoteInfo.ToString(), Encoding.Default.GetString(E.Data));
		}
		private void Task_HandleServerEvents(bool[] ExitFlag)
		{
			try
			{
				RaiseEvent_SystemStarted();
				while (!ExitFlag[0])
				{
					Subtask_HandleServerEvents();
					Thread.Sleep(100);
				}
			}
			finally
			{
				Subtask_HandleServerEvents();
				RaiseEvent_SystemStopped();
			}
		}
		private void Subtask_HandleServerEvents()
		{
			List<EventArgs> events = null;

			lock (mLockOfServerEvents)
			{
				if (mServerEvents.Count > 0)
				{
					events = mServerEvents.ToList();
					mServerEvents.Clear();
				}
			}

			if (events != null && events.Count > 0)
			{
				foreach (EventArgs e in events)
				{
					HandleServerEvent(e);
				}
				events.Clear();
			}
		}
	}
}
