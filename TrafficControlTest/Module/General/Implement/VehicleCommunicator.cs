using AsyncSocket;
using SerialData;
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
	class VehicleCommunicator : IVehicleCommunicator
	{
		public event EventHandlerDateTime SystemStarted;
		public event EventHandlerDateTime SystemStopped;
		public event EventHandlerRemoteConnectState RemoteConnectStateChanged;
		public event EventHandlerLocalListenState LocalListenStateChanged;
		public event EventHandlerSentSerializableData SentSerializableData;
		public event EventHandlerReceivedSerializableData ReceivedSerializableData;
		public event EventHandlerSentSerializableData SentSerializableDataSuccessed;
		public event EventHandlerSentSerializableData SentSerializableDataFailed;

		public ListenState mListenState { get { return mSocketServer.ListenStatus == EListenStatus.Idle ? ListenState.Closed : ListenState.Listening; } }
		public int mClientCount { get { return (mSocketServer == null || mSocketServer.ListenStatus == EListenStatus.Idle) ? 0 : mSocketServer.ClientCount; } }
		public List<string> mClientAddressInfo { get { return (mSocketServer == null || mSocketServer.ListenStatus == EListenStatus.Idle) ? null : mSocketServer.ClientDictionary.Keys.ToList(); } }

		private SerialServer mSocketServer = null;
		private int mListenPort { get; set; }
		private readonly Queue<EventArgs> mSerialServerEvents = new Queue<EventArgs>();
		private readonly object mLockOfSerialServerEvents = new object();
		private Thread mThdHandleSerialServerEvents = null;
		private bool[] mThdHandleSerialServerEventsExitFlag = null;

		public VehicleCommunicator()
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
			if (mSocketServer.ListenStatus == EListenStatus.Idle)
			{
				InitializeThread();
				mSocketServer.StartListening(mListenPort);
			}
		}
		public void StopListen()
		{
			if (mSocketServer.ListenStatus == EListenStatus.Listening)
			{
				mSocketServer.StopListen();
				DestroyThread();
			}
		}
		public void SendSerializableData(string IpPort, object Data)
		{
			if (mSocketServer.ClientDictionary.Keys.Contains(IpPort))
			{
				if (Data is Serializable)
				{
					mSocketServer.SendAndWaitAck(IpPort, Data as Serializable);
					RaiseEvent_SentSerializableData(IpPort, Data);
				}
			}
		}
		public void SendSerializableData_Goto(string IpPort, string Target)
		{
			SendSerializableData(IpPort, new GoTo(Target));
		}
		public void SendSerializableData_GotoPoint(string IpPort, int X, int Y)
		{
			SendSerializableData(IpPort, new GoToPoint(new List<int> { X, Y }));
		}
		public void SendSerializableData_GotoTowardPoint(string IpPort, int X, int Y, int Toward)
		{
			SendSerializableData(IpPort, new GoToTowardPoint(new List<int> { X, Y, Toward }));
		}
		public void SendSerializableData_Dock(string IpPort)
		{

		}
		public void SendSerializableData_InsertMovingBuffer(string IpPort, string Buffer)
		{
			SendSerializableData(IpPort, new InsertMovingBuffer(Buffer.Split(',').Select(o => int.Parse(o)).ToList()));
		}
		public void SendSerializableData_RemoveMovingBuffer(string IpPort)
		{
			SendSerializableData(IpPort, new RemoveMovingBuffer(null));
		}
		public void SendSerializableData_PauseMoving(string IpPort)
		{
			SendSerializableData(IpPort, new PauseMoving(null));
		}
		public void SendSerializableData_ResumeMoving(string IpPort)
		{
			SendSerializableData(IpPort, new ResumeMoving(null));
		}

		~VehicleCommunicator()
		{
			Destructor();
		}
		private void Constructor()
		{
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
		}
		private void SubscribeEvent_SerialServer(SerialServer SerialServer)
		{
			if (SerialServer != null)
			{
				SerialServer.ConnectStatusChangedEvent += HandleEvent_SerialServerEvent;
				SerialServer.ListenStatusChangedEvent += HandleEvent_SerialServerEvent;
				SerialServer.ReceivedSerialDataEvent += HandleEvent_SerialServerEvent;
				SerialServer.SentSerializableDataSuccessed += HandleEvent_SerialServerEvent;
				SerialServer.SentSerializableDataFailed += HandleEvent_SerialServerEvent;
			}
		}
		private void UnsubscribeEvent_SerialServer(SerialServer SerialServer)
		{
			if (SerialServer != null)
			{
				SerialServer.ConnectStatusChangedEvent -= HandleEvent_SerialServerEvent;
				SerialServer.ListenStatusChangedEvent -= HandleEvent_SerialServerEvent;
				SerialServer.ReceivedSerialDataEvent -= HandleEvent_SerialServerEvent;
				SerialServer.SentSerializableDataSuccessed -= HandleEvent_SerialServerEvent;
				SerialServer.SentSerializableDataFailed -= HandleEvent_SerialServerEvent;
			}
		}
		private void InitializeThread()
		{
			mThdHandleSerialServerEventsExitFlag = new bool[] { false };
			mThdHandleSerialServerEvents = new Thread(() => Task_HandleSerialServerEvents(mThdHandleSerialServerEventsExitFlag));
			mThdHandleSerialServerEvents.IsBackground = true;
			mThdHandleSerialServerEvents.Start();
		}
		private void DestroyThread()
		{
			if (mThdHandleSerialServerEvents != null)
			{
				if (mThdHandleSerialServerEvents.IsAlive)
				{
					mThdHandleSerialServerEventsExitFlag[0] = true;
				}
				mThdHandleSerialServerEvents = null;
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
				RemoteConnectStateChanged?.Invoke(DateTime.Now, IpPort, NewState);
			}
			else
			{
				Task.Run(() => { RemoteConnectStateChanged?.Invoke(DateTime.Now, IpPort, NewState); });
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
		protected virtual void RaiseEvent_SentSerializableDataSuccessed(string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				SentSerializableDataSuccessed?.Invoke(DateTime.Now, IpPort, Data);
			}
			else
			{
				Task.Run(() => { SentSerializableDataSuccessed?.Invoke(DateTime.Now, IpPort, Data); });
			}
		}
		protected virtual void RaiseEvent_SentSerializableDataFailed(string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				SentSerializableDataFailed?.Invoke(DateTime.Now, IpPort, Data);
			}
			else
			{
				Task.Run(() => { SentSerializableDataFailed?.Invoke(DateTime.Now, IpPort, Data); });
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
			else if (E is SentSerializableDataSuccessedEventArgs)
			{
				HandleSerialServerEvent(E as SentSerializableDataSuccessedEventArgs);
			}
			else if (E is SentSerializableDataFailedEventArgs)
			{
				HandleSerialServerEvent(E as SentSerializableDataFailedEventArgs);
			}
			else
			{
				Console.WriteLine("Received Unknown Serial Data.");
			}
		}
		private void HandleSerialServerEvent(ConnectStatusChangedEventArgs E)
		{
			RaiseEvent_RemoteConnectStateChanged(E.RemoteInfo.ToString(), E.ConnectStatus == EConnectStatus.Connect ? ConnectState.Connected : ConnectState.Disconnected);
		}
		private void HandleSerialServerEvent(ListenStatusChangedEventArgs E)
		{
			RaiseEvent_LocalListenStateChanged(E.ListenStatus == EListenStatus.Listening ? ListenState.Listening : ListenState.Closed);
		}
		private void HandleSerialServerEvent(ReceivedSerialDataEventArgs E)
		{
			RaiseEvent_ReceivedSerializableData(E.RemoteInfo.ToString(), E.Data);
		}
		private void HandleSerialServerEvent(SentSerializableDataSuccessedEventArgs E)
		{
			RaiseEvent_SentSerializableDataSuccessed(E.RemoteInfo.ToString(), E.Data);
		}
		private void HandleSerialServerEvent(SentSerializableDataFailedEventArgs E)
		{
			RaiseEvent_SentSerializableDataFailed(E.RemoteInfo.ToString(), E.Data);
		}
		private void Task_HandleSerialServerEvents(bool[] ExitFlag)
		{
			try
			{
				RaiseEvent_SystemStarted();
				while (!ExitFlag[0])
				{
					Subtask_HandleSerialServerEvents();
					Thread.Sleep(100);
				}
			}
			finally
			{
				Subtask_HandleSerialServerEvents();
				RaiseEvent_SystemStopped();
			}
		}
		private void Subtask_HandleSerialServerEvents()
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
		}
	}
}
