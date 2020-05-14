using AsyncSocket;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CommunicationVehicle
{
	class VehicleCommunicator : SystemWithLoopTask, IVehicleCommunicator
	{
		public event EventHandlerLocalListenState LocalListenStateChanged;
		public event EventHandlerRemoteConnectState RemoteConnectStateChanged;
		public event EventHandlerSentSerializableData SentSerializableData;
		public event EventHandlerReceivedSerializableData ReceivedSerializableData;
		public event EventHandlerSentSerializableData SentSerializableDataSuccessed;
		public event EventHandlerSentSerializableData SentSerializableDataFailed;
		
		public ListenState mListenState { get { return mSocketServer.ListenStatus == EListenStatus.Idle ? ListenState.Closed : ListenState.Listening; } }
		public int mClientCount { get { return (mSocketServer == null || mSocketServer.ListenStatus == EListenStatus.Idle) ? 0 : mSocketServer.ClientCount; } }
		public List<string> mClientAddressInfo { get { return (mSocketServer == null || mSocketServer.ListenStatus == EListenStatus.Idle) ? null : mSocketServer.ClientDictionary.Keys.ToList(); } }

		private SerialServer mSocketServer = null;
		private int mListenPort { get; set; }
		private readonly Queue<EventArgs> mSerialServerCommunicationSerialDataEvents = new Queue<EventArgs>();
		private readonly object mLockOfSerialServerCommunicationSerialDataEvents = new object();

		public VehicleCommunicator()
		{
			Constructor();
		}
		public bool IsIpPortConnected(string IpPort)
		{
			return mClientAddressInfo == null ? false : mClientAddressInfo.Contains(IpPort);
		}
		public void StartListen()
		{
			if (mSocketServer.ListenStatus == EListenStatus.Idle)
			{
				Start();
				mSocketServer.StartListening(mListenPort);
			}
		}
		public void StopListen()
		{
			if (mSocketServer.ListenStatus == EListenStatus.Listening)
			{
				mSocketServer.StopListen();
				Stop();
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
		public void SendSerializableData_Stop(string IpPort)
		{
			SendSerializableData(IpPort, new Stop(null));
		}
		public void SendSerializableData_InsertMovingBuffer(string IpPort, int X, int Y)
		{
			SendSerializableData(IpPort, new InsertMovingBuffer(new List<int>() { X, Y }));
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
		public void SendSerializableData_RequestMapList(string IpPort)
		{
			SendSerializableData(IpPort, new RequestMapList(null));
		}
		public void SendSerializableData_GetMap(string IpPort, string MapName)
		{
			SendSerializableData(IpPort, new GetMap(MapName));
		}
		public void SendSerializableData_UploadMapToAGV(string IpPort, string MapPath)
		{
			if (System.IO.File.Exists(MapPath)) SendSerializableData(IpPort, new UploadMapToAGV(new FileInfo(MapPath)));
		}
		public void SendSerializableData_ChangeMap(string IpPort, string MapName)
		{
			SendSerializableData(IpPort, new ChangeMap(MapName));
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
			Subtask_HandleSerialServerEvents();
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
				SerialServer.ListenStatusChangedEvent += HandleEvent_SerialServerListenStatusChangedEvent;
				SerialServer.ConnectStatusChangedEvent += HandleEvent_SerialServerConnectStatusChangedEvent;
				SerialServer.ReceivedSerialDataEvent += HandleEvent_SerialServerReceivedSerialDataEvent;
				SerialServer.SentSerializableDataSuccessed += HandleEvent_SerialServerSentSerializableDataSuccessed;
				SerialServer.SentSerializableDataFailed += HandleEvent_SerialServerSentSerializableDataFailed;
			}
		}
		private void UnsubscribeEvent_SerialServer(SerialServer SerialServer)
		{
			if (SerialServer != null)
			{
				SerialServer.ListenStatusChangedEvent -= HandleEvent_SerialServerListenStatusChangedEvent;
				SerialServer.ConnectStatusChangedEvent -= HandleEvent_SerialServerConnectStatusChangedEvent;
				SerialServer.ReceivedSerialDataEvent -= HandleEvent_SerialServerReceivedSerialDataEvent;
				SerialServer.SentSerializableDataSuccessed -= HandleEvent_SerialServerSentSerializableDataSuccessed;
				SerialServer.SentSerializableDataFailed -= HandleEvent_SerialServerSentSerializableDataFailed;
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
		protected virtual void RaiseEvent_SentSerializableData(string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				SentSerializableData?.Invoke(DateTime.Now, IpPort, Data);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { SentSerializableData?.Invoke(DateTime.Now, IpPort, Data); });
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
				System.Threading.Tasks.Task.Run(() => { ReceivedSerializableData?.Invoke(DateTime.Now, IpPort, Data); });
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
				System.Threading.Tasks.Task.Run(() => { SentSerializableDataSuccessed?.Invoke(DateTime.Now, IpPort, Data); });
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
				System.Threading.Tasks.Task.Run(() => { SentSerializableDataFailed?.Invoke(DateTime.Now, IpPort, Data); });
			}
		}
		private void HandleEvent_SerialServerListenStatusChangedEvent(object sender, ListenStatusChangedEventArgs e)
		{
			RaiseEvent_LocalListenStateChanged(e.ListenStatus == EListenStatus.Listening ? ListenState.Listening : ListenState.Closed, mListenPort);
		}
		private void HandleEvent_SerialServerConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
		{
			RaiseEvent_RemoteConnectStateChanged(e.RemoteInfo.ToString(), e.ConnectStatus == EConnectStatus.Connect ? ConnectState.Connected : ConnectState.Disconnected);
		}
		private void HandleEvent_SerialServerReceivedSerialDataEvent(object sender, ReceivedSerialDataEventArgs e)
		{
			lock(mLockOfSerialServerCommunicationSerialDataEvents)
			{
				mSerialServerCommunicationSerialDataEvents.Enqueue(e);
			}
		}
		private void HandleEvent_SerialServerSentSerializableDataSuccessed(object sender, SentSerializableDataSuccessedEventArgs e)
		{
			lock (mLockOfSerialServerCommunicationSerialDataEvents)
			{
				mSerialServerCommunicationSerialDataEvents.Enqueue(e);
			}
		}
		private void HandleEvent_SerialServerSentSerializableDataFailed(object sender, SentSerializableDataFailedEventArgs e)
		{
			lock (mLockOfSerialServerCommunicationSerialDataEvents)
			{
				mSerialServerCommunicationSerialDataEvents.Enqueue(e);
			}
		}
		private void Subtask_HandleSerialServerEvents()
		{
			List<EventArgs> events = null;
			lock (mLockOfSerialServerCommunicationSerialDataEvents)
			{
				if (mSerialServerCommunicationSerialDataEvents.Count > 0)
				{
					events = mSerialServerCommunicationSerialDataEvents.ToList();
					mSerialServerCommunicationSerialDataEvents.Clear();
				}
			}

			if (events != null && events.Count > 0)
			{
				for (int i = 0; i < events.Count; ++i)
				{
					if (events[i] is ReceivedSerialDataEventArgs)
					{
						RaiseEvent_ReceivedSerializableData((events[i] as ReceivedSerialDataEventArgs).RemoteInfo.ToString(), (events[i] as ReceivedSerialDataEventArgs).Data);
					}
					else if (events[i] is SentSerializableDataSuccessedEventArgs)
					{
						RaiseEvent_SentSerializableDataSuccessed((events[i] as SentSerializableDataSuccessedEventArgs).RemoteInfo.ToString(), (events[i] as SentSerializableDataSuccessedEventArgs).Data);
					}
					else if (events[i] is SentSerializableDataFailedEventArgs)
					{
						RaiseEvent_SentSerializableDataFailed((events[i] as SentSerializableDataFailedEventArgs).RemoteInfo.ToString(), (events[i] as SentSerializableDataFailedEventArgs).Data);
					}
				}
				Thread.Sleep(5);
			}
		}
	}
}
