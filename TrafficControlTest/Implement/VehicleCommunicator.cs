using AsyncSocket;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleCommunicator;

namespace TrafficControlTest.Implement
{
	class VehicleCommunicator : IVehicleCommunicator
	{
		public event EventHandlerDateTime VehicleCommunicatorStarted;
		public event EventHandlerDateTime VehicleCommunicatorStopped;
		public event EventHandlerRemoteConnectState VehicleConnectStateChanged;
		public event EventHandlerLocalListenState VehicleCommunicatorListenStateChanged;
		public event EventHandlerSentSerializableData SentSerializableData;
		public event EventHandlerReceivedSerializableData ReceivedSerializableData;

		public ListenState mListenState { get { return mSocketServer.ListenStatus == EListenStatus.Idle ? ListenState.Closed : ListenState.Listening; } }
		public int mClientCount { get { return (mSocketServer == null || mSocketServer.ListenStatus == EListenStatus.Idle) ? 0 : mSocketServer.ClientCount; } }

		private SerialServer mSocketServer = null;
		private readonly Queue<EventArgs> mSerialServerEvents = new Queue<EventArgs>();
		private readonly object mLockOfSerialServerEvents = new object();
		private Thread mThdHandleSerialServerEvents = null;

		public VehicleCommunicator()
		{
			Constructor();
		}
		public void StartListen(int Port)
		{
			if (mSocketServer.ListenStatus == EListenStatus.Idle)
			{
				mSocketServer.StartListening(Port);
			}
		}
		public void StopListen()
		{
			if (mSocketServer.ListenStatus == EListenStatus.Listening)
			{
				mSocketServer.StopListen();
			}
		}
		public void SendSerializableData(string IpPort, object Data)
		{
			if (mSocketServer.ClientDictionary.Keys.Contains(IpPort))
			{
				if (Data is Serializable)
				{
					mSocketServer.Send(IpPort, Data as Serializable);
					RaiseEvent_SentSerializableData(IpPort, Data);
				}
			}
		}

		~VehicleCommunicator()
		{
			Destructor();
		}
		private void Constructor()
		{
			InitializeThread();

			if (mSocketServer == null)
			{
				mSocketServer = new SerialServer();
				SubscribeEvent_SerialServer(mSocketServer);
			}
		}
		private void Destructor()
		{
			if (mSocketServer != null)
			{
				UnsubscribeEvent_SerialServer(mSocketServer);
				mSocketServer = null;
			}

			DestroyThread();
		}
		private void SubscribeEvent_SerialServer(SerialServer SerialServer)
		{
			if (SerialServer != null)
			{
				SerialServer.ConnectStatusChangedEvent += HandleEvent_SerialServerEvent;
				SerialServer.ListenStatusChangedEvent += HandleEvent_SerialServerEvent;
				SerialServer.ReceivedSerialDataEvent += HandleEvent_SerialServerEvent;
			}
		}
		private void UnsubscribeEvent_SerialServer(SerialServer SerialServer)
		{
			if (SerialServer != null)
			{
				SerialServer.ConnectStatusChangedEvent -= HandleEvent_SerialServerEvent;
				SerialServer.ListenStatusChangedEvent -= HandleEvent_SerialServerEvent;
				SerialServer.ReceivedSerialDataEvent -= HandleEvent_SerialServerEvent;
			}
		}
		private void InitializeThread()
		{
			mThdHandleSerialServerEvents = new Thread(Task_HandleSerialServerEvents);
			mThdHandleSerialServerEvents.IsBackground = true;
			mThdHandleSerialServerEvents.Start();
		}
		private void DestroyThread()
		{
			if (mThdHandleSerialServerEvents != null)
			{
				if (mThdHandleSerialServerEvents.IsAlive)
				{
					mThdHandleSerialServerEvents.Abort();
				}
				mThdHandleSerialServerEvents = null;
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorStarted(bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorStarted?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorStarted?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorStopped(bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorStopped?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorStopped?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_VehicleConnectStateChanged(string IpPort, ConnectState NewState, bool Sync = true)
		{
			if (Sync)
			{
				VehicleConnectStateChanged?.Invoke(DateTime.Now, IpPort, NewState);
			}
			else
			{
				Task.Run(() => { VehicleConnectStateChanged?.Invoke(DateTime.Now, IpPort, NewState); });
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorListenStateChanged(ListenState NewState, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorListenStateChanged?.Invoke(DateTime.Now, NewState);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorListenStateChanged?.Invoke(DateTime.Now, NewState); });
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
		private void HandleEvent_SerialServerEvent(object Sender, EventArgs E)
		{
			lock(mLockOfSerialServerEvents)
			{
				mSerialServerEvents.Enqueue(E);
			}
		}
		private void HandleSerialServerEvent(EventArgs E)
		{
			if (E is ConnectStatusChangedEventArgs)
			{
				HandleSerialServerEvent(E as ConnectStatusChangedEventArgs);
			}
			else if (E is ListenStatusChangedEventArgs)
			{
				HandleSerialServerEvent(E as ListenStatusChangedEventArgs);
			}
			else if (E is ReceivedSerialDataEventArgs)
			{
				HandleSerialServerEvent(E as ReceivedSerialDataEventArgs);
			}
			else
			{
				Console.WriteLine("Received Unknown Serial Data.");
			}
		}
		private void HandleSerialServerEvent(ConnectStatusChangedEventArgs E)
		{
			RaiseEvent_VehicleConnectStateChanged(E.RemoteInfo.ToString(), E.ConnectStatus == EConnectStatus.Connect ? ConnectState.Connected : ConnectState.Disconnected);
		}
		private void HandleSerialServerEvent(ListenStatusChangedEventArgs E)
		{
			RaiseEvent_VehicleCommunicatorListenStateChanged(E.ListenStatus == EListenStatus.Listening ? ListenState.Listening : ListenState.Closed);
		}
		private void HandleSerialServerEvent(ReceivedSerialDataEventArgs E)
		{
			RaiseEvent_ReceivedSerializableData(E.RemoteInfo.ToString(), E.Data);
		}
		private void Task_HandleSerialServerEvents()
		{
			try
			{
				RaiseEvent_VehicleCommunicatorStarted();
				while (true)
				{
					List<EventArgs> events = null;

					lock (mLockOfSerialServerEvents)
					{
						if (mSerialServerEvents.Count > 0)
						{
							events = mSerialServerEvents.ToList();
							mSerialServerEvents.Clear();
						}
					}

					if (events != null && events.Count > 0)
					{
						foreach (EventArgs e in events)
						{
							HandleSerialServerEvent(e);
						}
						events.Clear();
					}
					Thread.Sleep(100);
				}
			}
			catch (ThreadAbortException e)
			{
				Console.WriteLine(e.ToString());
			}
			finally
			{
				RaiseEvent_VehicleCommunicatorStopped();
			}
		}
	}
}
