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
	public class HostCommunicator : SystemWithLoopTask, IHostCommunicator
	{
		public event EventHandlerLocalListenState LocalListenStateChanged;
		public event EventHandlerRemoteConnectState RemoteConnectStateChanged;
		public event EventHandlerSentString SentString;
		public event EventHandlerReceivedString ReceivedString;

		public ListenState mListenState { get { return mServer.ListenStatus == EListenStatus.Idle ? ListenState.Closed : ListenState.Listening; } }
		public int mClientCout { get { return (mServer == null || mServer.ListenStatus == EListenStatus.Idle) ? 0 : mServer.ClientCount; } }

		private Server mServer = null;
		private int mListenPort = 9000;
		private readonly Queue<ReceivedDataEventArgs> mServerReceivedDataEvents = new Queue<ReceivedDataEventArgs>();
		private readonly object mLockOfServerReceivedDataEvents = new object();

		public HostCommunicator()
		{
			Constructor();
		}
		public void StartListen()
		{
			if (mServer.ListenStatus == EListenStatus.Idle)
			{
				Start();
				mServer.StartListening(mListenPort);
			}
		}
		public void StopListen()
		{
			if (mServer.ListenStatus == EListenStatus.Listening)
			{
				mServer.StopListen();
				Stop();
			}
		}
		public void SendString(string Data)
		{
			foreach (string ipPort in mServer.ClientDictionary.Keys)
			{
				SendString(ipPort, Data);
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
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "ListenPort":
					return mListenPort.ToString();
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
				case "ListenPort":
					mListenPort = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override void Task()
		{
			Subtask_HandleServerReceivedDataEvents();
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
				Server.ListenStatusChangedEvent += HandleEvent_ServerListenStatusChangedEvent;
				Server.ConnectStatusChangedEvent += HandleEvent_ServerConnectStatusChangedEvent;
				Server.ReceivedDataEvent += HandleEvent_ServerReceivedDataEvent;
			}
		}
		private void UnsubscribeEvent_Server(Server Server)
		{
			if (Server != null)
			{
				Server.ListenStatusChangedEvent -= HandleEvent_ServerListenStatusChangedEvent;
				Server.ConnectStatusChangedEvent -= HandleEvent_ServerConnectStatusChangedEvent;
				Server.ReceivedDataEvent -= HandleEvent_ServerReceivedDataEvent;
			}
		}
		protected virtual void RaiseEvent_LocalListenStateChanged(ListenState NewState, int Port, bool Sync = true)
		{
			if (Sync)
			{
				LocalListenStateChanged?.Invoke(DateTime.Now, NewState, Port);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { LocalListenStateChanged?.Invoke(DateTime.Now, NewState, Port); });
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
				System.Threading.Tasks.Task.Run(() => { RemoteConnectStateChanged?.Invoke(DateTime.Now, IpPort, NewState); });
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
				System.Threading.Tasks.Task.Run(() => { SentString?.Invoke(DateTime.Now, IpPort, Data); });
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
				System.Threading.Tasks.Task.Run(() => { ReceivedString?.Invoke(DateTime.Now, IpPort, Data); });
			}
		}
		private void HandleEvent_ServerListenStatusChangedEvent(object sender, ListenStatusChangedEventArgs e)
		{
			RaiseEvent_LocalListenStateChanged(e.ListenStatus == EListenStatus.Listening ? ListenState.Listening : ListenState.Closed, mListenPort);
		}
		private void HandleEvent_ServerConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
		{
			RaiseEvent_RemoteConnectStateChanged(e.RemoteInfo.ToString(), e.ConnectStatus == EConnectStatus.Connect ? ConnectState.Connected : ConnectState.Disconnected);
		}
		private void HandleEvent_ServerReceivedDataEvent(object sender, ReceivedDataEventArgs e)
		{
			lock (mLockOfServerReceivedDataEvents)
			{
				mServerReceivedDataEvents.Enqueue(e);
			}
		}
		private void Subtask_HandleServerReceivedDataEvents()
		{
			List<ReceivedDataEventArgs> events = null;
			lock (mLockOfServerReceivedDataEvents)
			{
				if (mServerReceivedDataEvents.Count > 0)
				{
					events = mServerReceivedDataEvents.ToList();
					mServerReceivedDataEvents.Clear();
				}
			}

			if (events != null && events.Count > 0)
			{
				for (int i = 0; i < events.Count; ++i)
				{
					RaiseEvent_ReceivedString(events[i].RemoteInfo.ToString(), Encoding.Default.GetString(events[i].Data));
					Thread.Sleep(5);
				}
			}
		}
	}
}
