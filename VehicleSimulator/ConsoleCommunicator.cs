using AsyncSocket;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	/// <summary>用來跟主控台通訊</summary>
	class ConsoleCommunicator
	{
		public delegate void ConnectStatusChangedEventHandler(DateTime occurTime, EndPointInfo remoteInfo, EConnectStatus newStatus);
		public event ConnectStatusChangedEventHandler ConnectStatusChanged;

		public delegate void ReceivedRequestMapListEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, RequestMapList data);
		public event ReceivedRequestMapListEventHandler ReceivedRequestMapListData;

		public delegate void ReceivedGetMapEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, GetMap data);
		public event ReceivedGetMapEventHandler ReceivedGetMapData;

		public delegate void ReceivedUploadMapToAGVEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, UploadMapToAGV data);
		public event ReceivedUploadMapToAGVEventHandler ReceivedUploadMapToAGVData;

		public delegate void ReceivedChangeMapEventHandler(DateTime receivedTime, EndPointInfo remoteInfo, ChangeMap data);
		public event ReceivedChangeMapEventHandler ReceivedChangeMapData;

		/// <summary>與主控台通訊用的 Socket</summary>
		private SerialClient SocketClient = null;

		/// <summary>處理 SocketClient 事件的執行緒</summary>
		private Thread ThdSocketEventHandler = null;

		/// <summary>處理 SocketClient 事件的執行緒的 Manual Reset Event</summary>
		private readonly ManualResetEvent MRESocketEventHandler = new ManualResetEvent(false);

		/// <summary>SocketClient 事件的儲存器</summary>
		private readonly Queue<EventArgs> SocketEventContainer = new Queue<EventArgs>();

		/// <summary>是否執行中</summary>
		public bool IsAlive { get { return SocketClient == null ? false : SocketClient.ConnectStatus == EConnectStatus.Connect ? true : false; } }

		/// <summary>開啟與主控台通訊用的 Socket</summary>
		public void Start(string ip, int port)
		{
			Initialize(ip, port);
		}

		/// <summary>關閉與主控台通訊用的 Socket</summary>
		public void Stop()
		{
			Destroy();
		}

		/// <summary>向指定 IP 傳送序列化資料</summary>
		public void SendSerializableData(Serializable data)
		{
			if (data != null)
			{
				SocketClient.Send(data);
			}
		}

		/// <summary>初始化</summary>
		private void Initialize(string ip, int port)
		{
			if (SocketClient == null)
			{
				SocketClient = new SerialClient();
				SocketClient.ConnectStatusChangedEvent += SocketClient_Event;
				SocketClient.ReceivedSerialDataEvent += SocketClient_Event;
				SocketClient.Connect(ip, port);
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
			if (SocketClient != null)
			{
				if (SocketClient.ConnectStatus == EConnectStatus.Connect)
				{
					SocketClient.Disconnect();
				}
				SocketClient.ConnectStatusChangedEvent -= SocketClient_Event;
				SocketClient.ReceivedSerialDataEvent -= SocketClient_Event;
				SocketClient = null;
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
		private void SocketClient_Event(object sender, EventArgs e)
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

		/// <summary>處理 Socket 事件(接收序列化資料事件)</summary>
		private void HandleSocketEvent(ReceivedSerialDataEventArgs e)
		{
			HandleReceivedSerialData(e.ReceivedTime, e.RemoteInfo, e.Data);
		}

		/// <summary>處理連線狀態改變</summary>
		private void HandleConnectStatusChanged(DateTime occurTime, EndPointInfo remoteInfo, EConnectStatus newStatus)
		{
			ConnectStatusChanged?.Invoke(occurTime, remoteInfo, newStatus);
		}

		/// <summary>處理收到的序列化資料</summary>
		private void HandleReceivedSerialData(DateTime receivedTime, EndPointInfo remoteInfo, Serializable data)
		{
			if (data is RequestMapList)
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
