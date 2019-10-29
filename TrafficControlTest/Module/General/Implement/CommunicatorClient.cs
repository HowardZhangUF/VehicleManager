using AsyncSocket;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Implement
{
	public class CommunicatorClient : ICommunicatorClient
	{
		public event EventHandlerDateTime SystemStarted;
		public event EventHandlerDateTime SystemStopped;
		public event EventHandlerRemoteConnectState ConnectStateChanged;
		public event EventHandlerSentSerializableData SentSerializableData;
		public event EventHandlerReceivedSerializableData ReceivedSerializableData;

		public string mRemoteIpPort { get { return $"{mSocketClient.RemoteIP}:{mSocketClient.RemotePort}"; } }
		public ConnectState mConnectState { get { return (mSocketClient.ConnectStatus == EConnectStatus.Connect) ? ConnectState.Connected : ConnectState.Disconnected; } }

		private SerialClient mSocketClient = null;
		private readonly Queue<EventArgs> mSerialClientEvents = new Queue<EventArgs>();
		private readonly object mLockOfSerialClientEvents = new object();
		private Thread mThdHandleSerialClientEvents = null;
		private bool[] mThdHandleSerialClientEventsExitFlag = null;

		public CommunicatorClient()
		{
			Constructor();
		}
		public void StartConnect(string Ip, int Port)
		{
			if (mConnectState != ConnectState.Connected)
			{
				InitializeThread();
				mSocketClient.Connect(Ip, Port);
			}
		}
		public void StopConnect()
		{
			if (mConnectState == ConnectState.Connected)
			{
				mSocketClient.Disconnect();
				DestroyThread();
			}
		}
		public void SendSerializableData(object Data)
		{
			if (mConnectState == ConnectState.Connected)
			{
				if (Data is Serializable)
				{
					mSocketClient.Send(Data as Serializable);
					RaiseEvent_SentSerializableData(mRemoteIpPort, Data);
				}
			}
		}

		~CommunicatorClient()
		{
			Destructor();
		}
		private void Constructor()
		{
			if (mSocketClient == null)
			{
				mSocketClient = new SerialClient();
				SubscribeEvent_SerialClient(mSocketClient);
			}
		}
		private void Destructor()
		{
			if (mSocketClient != null)
			{
				UnsubscribeEvent_SerialClient(mSocketClient);
				mSocketClient = null;
			}
		}
		private void SubscribeEvent_SerialClient(SerialClient SerialClient)
		{
			if (SerialClient != null)
			{
				SerialClient.ConnectStatusChangedEvent += HandleEvent_SerialClientEvent;
				SerialClient.ReceivedSerialDataEvent += HandleEvent_SerialClientEvent;
			}
		}
		private void UnsubscribeEvent_SerialClient(SerialClient SerialClient)
		{
			if (SerialClient != null)
			{
				SerialClient.ConnectStatusChangedEvent -= HandleEvent_SerialClientEvent;
				SerialClient.ReceivedSerialDataEvent -= HandleEvent_SerialClientEvent;
			}
		}
		private void InitializeThread()
		{
			mThdHandleSerialClientEventsExitFlag = new bool[] { false };
			mThdHandleSerialClientEvents = new Thread(() => Task_HandleSerialClientEvents(mThdHandleSerialClientEventsExitFlag));
			mThdHandleSerialClientEvents.IsBackground = true;
			mThdHandleSerialClientEvents.Start();
		}
		private void DestroyThread()
		{
			if (mThdHandleSerialClientEvents != null)
			{
				if (mThdHandleSerialClientEvents.IsAlive)
				{
					mThdHandleSerialClientEventsExitFlag[0] = true;
				}
				mThdHandleSerialClientEvents = null;
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
		protected virtual void RaiseEvent_RemoteConnectStateChanged(string IpPort, ConnectState NewState, bool Sync = true)
		{
			if (Sync)
			{
				ConnectStateChanged?.Invoke(DateTime.Now, IpPort, NewState);
			}
			else
			{
				Task.Run(() => { ConnectStateChanged?.Invoke(DateTime.Now, IpPort, NewState); });
			}
		}
		protected virtual void RaiseEvent_SentSerializableData(string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				SentSerializableData?.Invoke(DateTime.Now, IpPort, Data);
			}
			else
			{
				Task.Run(() => { SentSerializableData?.Invoke(DateTime.Now, IpPort, Data); });
			}
		}
		protected virtual void RaiseEvent_ReceivedSerializableData(string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				ReceivedSerializableData?.Invoke(DateTime.Now, IpPort, Data);
			}
			else
			{
				Task.Run(() => { ReceivedSerializableData?.Invoke(DateTime.Now, IpPort, Data); });
			}
		}
		private void HandleEvent_SerialClientEvent(object Sender, EventArgs E)
		{
			lock (mLockOfSerialClientEvents)
			{
				mSerialClientEvents.Enqueue(E);
			}
		}
		private void HandleSerialClientEvent(EventArgs E)
		{
			if (E is ConnectStatusChangedEventArgs)
			{
				HandleSerialClientEvent(E as ConnectStatusChangedEventArgs);
			}
			else if (E is ReceivedSerialDataEventArgs)
			{
				HandleSerialClientEvent(E as ReceivedSerialDataEventArgs);
			}
			else
			{
				Console.WriteLine("Received Unknown Serial Data.");
			}
		}
		private void HandleSerialClientEvent(ConnectStatusChangedEventArgs E)
		{
			RaiseEvent_RemoteConnectStateChanged(E.RemoteInfo.ToString(), E.ConnectStatus == EConnectStatus.Connect ? ConnectState.Connected : ConnectState.Disconnected);
		}
		private void HandleSerialClientEvent(ReceivedSerialDataEventArgs E)
		{
			RaiseEvent_ReceivedSerializableData(E.RemoteInfo.ToString(), E.Data);
		}
		private void Task_HandleSerialClientEvents(bool[] ExitFlag)
		{
			try
			{
				RaiseEvent_SystemStarted();
				while (!ExitFlag[0])
				{
					Subtask_HandleSerialClientEvents();
					Thread.Sleep(100);
				}
			}
			finally
			{
				Subtask_HandleSerialClientEvents();
				RaiseEvent_SystemStopped();
			}
		}
		private void Subtask_HandleSerialClientEvents()
		{
			List<EventArgs> events = null;

			lock (mLockOfSerialClientEvents)
			{
				if (mSerialClientEvents.Count > 0)
				{
					events = mSerialClientEvents.ToList();
					mSerialClientEvents.Clear();
				}
			}

			if (events != null && events.Count > 0)
			{
				foreach (EventArgs e in events)
				{
					HandleSerialClientEvent(e);
				}
				events.Clear();
			}
		}
	}
}
