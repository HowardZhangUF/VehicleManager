using AsyncSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Communication
{
	public class CommunicatorClientUsingString : SystemWithLoopTask, ICommunicatorClientUsingString
	{
		public event EventHandler<ConnectStateChangedEventArgs> ConnectStateChanged;
		public event EventHandler<SentStringEventArgs> SentString;
		public event EventHandler<ReceivedStringEventArgs> ReceivedString;

		public string mRemoteIpPort { get { return mSocketClient != null ? $"{mSocketClient.RemoteIP}:{mSocketClient.RemotePort}" : string.Empty; } }
		public bool mIsConnected { get { return mSocketClient != null ? (mSocketClient.ConnectStatus == EConnectStatus.Connect ? true : false) : false; } }

		private Client mSocketClient = null;
		private readonly Queue<ReceivedDataEventArgs> mClientReceivedDataEvents = new Queue<ReceivedDataEventArgs>();
		private readonly object mLockOfClientReceivedDataEvents = new object();

		public CommunicatorClientUsingString()
		{
			Constructor();
		}
		~CommunicatorClientUsingString()
		{
			Destructor();
		}
		public void Connect(string Ip, int Port)
		{
			if (!mIsConnected)
			{
				mSocketClient.Connect(Ip, Port);
			}
		}
		public void Disconnect()
		{
			if (mIsConnected)
			{
				mSocketClient.Disconnect();
			}
		}
		public void Send(string Data)
		{
			if (mIsConnected)
			{
				mSocketClient.Send(Data);
				RaiseEvent_SentString(mRemoteIpPort, Data);
			}
		}
		public override void Task()
		{
			Subtask_HandleClientReceivedDataEvents();
		}

		private void Constructor()
		{
			if (mSocketClient == null)
			{
				mSocketClient = new Client();
				SubscribeEvent_Client(mSocketClient);
			}
		}
		private void Destructor()
		{
			if (mSocketClient != null)
			{
				UnsubscribeEvent_Client(mSocketClient);
				mSocketClient = null;
			}
		}
		private void SubscribeEvent_Client(Client Client)
		{
			if (Client != null)
			{
				Client.ConnectStatusChangedEvent += HandleEvent_ClientConnectStatusChangedEvent;
				Client.ReceivedDataEvent += HandleEvent_ClientReceivedDataEvent;
			}
		}
		private void UnsubscribeEvent_Client(Client Client)
		{
			if (Client != null)
			{
				Client.ConnectStatusChangedEvent -= HandleEvent_ClientConnectStatusChangedEvent;
				Client.ReceivedDataEvent -= HandleEvent_ClientReceivedDataEvent;
			}
		}
		protected virtual void RaiseEvent_ConnectStateChanged(string IpPort, bool Connected, bool Sync = true)
		{
			if (Sync)
			{
				ConnectStateChanged?.Invoke(this, new ConnectStateChangedEventArgs(DateTime.Now, IpPort, Connected));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ConnectStateChanged?.Invoke(this, new ConnectStateChangedEventArgs(DateTime.Now, IpPort, Connected)); });
			}
		}
		protected virtual void RaiseEvent_SentString(string IpPort, string Data, bool Sync = true)
		{
			if (Sync)
			{
				SentString?.Invoke(this, new SentStringEventArgs(DateTime.Now, IpPort, Data));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { SentString?.Invoke(this, new SentStringEventArgs(DateTime.Now, IpPort, Data)); });
			}
		}
		protected virtual void RaiseEvent_ReceivedString(string IpPort, string Data, bool Sync = true)
		{
			if (Sync)
			{
				ReceivedString?.Invoke(this, new ReceivedStringEventArgs(DateTime.Now, IpPort, Data));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ReceivedString?.Invoke(this, new ReceivedStringEventArgs(DateTime.Now, IpPort, Data)); });
			}
		}
		private void HandleEvent_ClientConnectStatusChangedEvent(object Sender, ConnectStatusChangedEventArgs E)
		{
			RaiseEvent_ConnectStateChanged(E.RemoteInfo.ToString(), E.ConnectStatus == EConnectStatus.Connect ? true : false);
		}
		private void HandleEvent_ClientReceivedDataEvent(object Sender, ReceivedDataEventArgs E)
		{
			lock (mLockOfClientReceivedDataEvents)
			{
				mClientReceivedDataEvents.Enqueue(E);
			}
		}
		private void Subtask_HandleClientReceivedDataEvents()
		{
			List<ReceivedDataEventArgs> events = null;
			lock (mLockOfClientReceivedDataEvents)
			{
				if (mClientReceivedDataEvents.Count > 0)
				{
					events = mClientReceivedDataEvents.ToList();
					mClientReceivedDataEvents.Clear();
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
