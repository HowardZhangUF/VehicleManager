using AsyncSocket;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>AGV 監控器</summary>
	class AGVMonitor
	{
		public delegate void AGVMonitorStartedStoppedEventHandler(DateTime occurTime);
		public event AGVMonitorStartedStoppedEventHandler AGVMonitorStarted;
		public event AGVMonitorStartedStoppedEventHandler AGVMonitorStopped;

		public delegate void AGVConnectStatusChangedEventHandler(DateTime occurTime, EndPointInfo remoteInfo, EConnectStatus newStatus);
		public event AGVConnectStatusChangedEventHandler AGVConnectStatusChanged;

		public delegate void AGVMonitorListenStatusChangedEventHandler(DateTime occurTime, EListenStatus newStatus);
		public event AGVMonitorListenStatusChangedEventHandler AGVMonitorListenStatusChanged;

		public delegate void ReceivedAGVStatusEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, AGVStatus data);
		public event ReceivedAGVStatusEventHandler ReceivedAGVStatusData;

		public delegate void ReceivedAGVPathEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, AGVPath data);
		public event ReceivedAGVPathEventHandler ReceivedAGVPathData;

		public delegate void ReceivedRequestMapListEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, RequestMapList data);
		public event ReceivedRequestMapListEventHandler ReceivedRequestMapListData;

		public delegate void ReceivedGetMapEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, GetMap data);
		public event ReceivedGetMapEventHandler ReceivedGetMapData;

		public delegate void ReceivedUploadMapToAGVEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, UploadMapToAGV data);
		public event ReceivedUploadMapToAGVEventHandler ReceivedUploadMapToAGVData;

		public delegate void ReceivedChangeMapEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, ChangeMap data);
		public event ReceivedChangeMapEventHandler ReceivedChangeMapData;

		/// <summary>供 AGV 通訊用的 Socket</summary>
		private SerialServer SocketServer = null;

		/// <summary>處理 SocketServer 事件的執行緒</summary>
		private Thread ThdSocketEventHandler = null;

		/// <summary>處理 SocketServer 事件的執行緒的 Manual Reset Event</summary>
		private readonly ManualResetEvent MRESocketEventHandler = new ManualResetEvent(false);

		/// <summary>SocketServer 事件的儲存器</summary>
		private readonly Queue<EventArgs> SocketEventContainer = new Queue<EventArgs>();

		/// <summary>是否執行中</summary>
		public bool isAlive { get { return SocketServer == null ? false : true; } }

		/// <summary>開啟與 AGV 通訊用的 Socket</summary>
		public void Start(int port)
		{
			Initialize(port);
			AGVMonitorStarted?.Invoke(DateTime.Now);
		}

		/// <summary>關閉與 AGV 通訊用的 Socket</summary>
		public void Stop()
		{
			Destroy();
			AGVMonitorStopped?.Invoke(DateTime.Now);
		}

		/// <summary>向指定 IP 傳送要求地圖清單的命令</summary>
		public void SendRequestMapListCommand(string ipPort)
		{
			SocketServer.Send(ipPort, new RequestMapList(null));
		}

		/// <summary>向指定 IP 傳送要求指定地圖的命令</summary>
		public void SendGetMapCommand(string ipPort, string fileName)
		{
			SocketServer.Send(ipPort, new GetMap(fileName));
		}

		/// <summary>向指定 IP 傳送上傳指定地圖的命令</summary>
		public void SendUploadMapCommand(string ipPort, string fileName)
		{
			SocketServer.Send(ipPort, new UploadMapToAGV(new FileInfo(fileName)));
		}

		/// <summary>向指定 IP 傳送使用指定地圖的命令</summary>
		public void SendChangeMapCommand(string ipPort, string fileName)
		{
			SocketServer.Send(ipPort, new ChangeMap(fileName));
		}

		/// <summary>向指定 IP 傳送序列化資料</summary>
		public void SendSerializableData(string ipPort, Serializable data)
		{
			if (data != null)
			{
				SocketServer.Send(ipPort, data);
			}
		}

		/// <summary>初始化</summary>
		private void Initialize(int port)
		{
			if (SocketServer == null)
			{
				SocketServer = new SerialServer();
				SocketServer.ConnectStatusChangedEvent += SocketServer_Event;
				SocketServer.ListenStatusChangedEvent += SocketServer_Event;
				SocketServer.ReceivedSerialDataEvent += SocketServer_Event;
				SocketServer.StartListening(port);
			}

			if (ThdSocketEventHandler == null)
			{
				ThdSocketEventHandler = new Thread(TaskSocketEventHandler);
				ThdSocketEventHandler.IsBackground = true;
				ThdSocketEventHandler.Start();
			}
		}

		/// <summary>摧毀</summary>
		private void Destroy()
		{
			if (SocketServer != null)
			{
				if (SocketServer.ListenStatus == EListenStatus.Listening)
				{
					SocketServer.StopListen();
				}
				SocketServer.ConnectStatusChangedEvent -= SocketServer_Event;
				SocketServer.ListenStatusChangedEvent -= SocketServer_Event;
				SocketServer.ReceivedSerialDataEvent -= SocketServer_Event;
				SocketServer = null;
			}

			if (ThdSocketEventHandler != null)
			{
				if (ThdSocketEventHandler.IsAlive)
				{
					ThdSocketEventHandler.Abort();
				}
				ThdSocketEventHandler = null;
			}
		}

		/// <summary>處理 Socket 事件</summary>
		private void SocketServer_Event(object sender, EventArgs e)
		{
			lock (SocketEventContainer)
			{
				SocketEventContainer.Enqueue(e);
			}
			MRESocketEventHandler.Set();
			MRESocketEventHandler.Reset();
		}

		/// <summary>處理 SocketServer 事件的執行緒</summary>
		private void TaskSocketEventHandler()
		{
			try
			{
				while (true)
				{
					List<EventArgs> events = null;
					lock (SocketEventContainer)
					{
						events = SocketEventContainer.ToList();
						SocketEventContainer.Clear();
					}

					foreach (EventArgs e in events)
					{
						HandleSocketEvent(e);
					}

					MRESocketEventHandler.WaitOne();
					Thread.Sleep(1);
				}
			}
			catch (ThreadAbortException)
			{
				// do nothing
			}
		}

		/// <summary>處理 Socket 事件</summary>
		private void HandleSocketEvent(EventArgs e)
		{
			if (e is ConnectStatusChangedEventArgs)
			{
				HandleSocketEvent(e as ConnectStatusChangedEventArgs);
			}
			else if (e is ListenStatusChangedEventArgs)
			{
				HandleSocketEvent(e as ListenStatusChangedEventArgs);
			}
			else if (e is ReceivedSerialDataEventArgs)
			{
				HandleSocketEvent(e as ReceivedSerialDataEventArgs);
			}
		}

		/// <summary>處理 Socket 事件(連線狀態改變事件)</summary>
		private void HandleSocketEvent(ConnectStatusChangedEventArgs e)
		{
			HandleConnectStatusChanged(e.StatusChangedTime, e.RemoteInfo, e.ConnectStatus);
		}

		/// <summary>處理 Socket 事件(監聽狀態改變事件)</summary>
		private void HandleSocketEvent(ListenStatusChangedEventArgs e)
		{
			HandleListenStatusChanged(e.StatusChangedTime, e.ListenStatus);
		}

		/// <summary>處理 Socket 事件(接收序列化資料事件)</summary>
		private void HandleSocketEvent(ReceivedSerialDataEventArgs e)
		{
			HandleReceivedSerialData(e.ReceivedTime, e.RemoteInfo, e.Data);
		}

		/// <summary>處理連線狀態改變</summary>
		private void HandleConnectStatusChanged(DateTime occurTime, EndPointInfo remoteInfo, EConnectStatus newStatus)
		{
			AGVConnectStatusChanged?.Invoke(occurTime, remoteInfo, newStatus);
		}

		/// <summary>處理監聽狀態改變</summary>
		private void HandleListenStatusChanged(DateTime occurTime, EListenStatus newStatus)
		{
			AGVMonitorListenStatusChanged?.Invoke(occurTime, newStatus);
		}

		/// <summary>處理收到的序列化資料</summary>
		private void HandleReceivedSerialData(DateTime receivedTime, EndPointInfo remoteInfo, Serializable data)
		{
			if (data is AGVStatus)
			{
				ReceivedAGVStatusData?.Invoke(receivedTime, remoteInfo, data as AGVStatus);
			}
			else if (data is AGVPath)
			{
				ReceivedAGVPathData?.Invoke(receivedTime, remoteInfo, data as AGVPath);
			}
			else if (data is RequestMapList)
			{
				ReceivedRequestMapListData?.Invoke(receivedTime, remoteInfo, data as RequestMapList);
			}
			else if (data is GetMap)
			{
				ReceivedGetMapData?.Invoke(receivedTime, remoteInfo, data as GetMap);
			}
			else if (data is UploadMapToAGV)
			{
				ReceivedUploadMapToAGVData?.Invoke(receivedTime, remoteInfo, data as UploadMapToAGV);
			}
			else if (data is ChangeMap)
			{
				ReceivedChangeMapData?.Invoke(receivedTime, remoteInfo, data as ChangeMap);
			}
		}
	}
}
