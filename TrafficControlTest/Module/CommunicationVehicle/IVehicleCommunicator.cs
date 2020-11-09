using System;
using System.Collections.Generic;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CommunicationVehicle
{
	/// <summary>
	/// - 提供監聽方法以供車子建立連線
	/// - 提供傳送資料給車子的方法
	/// - 當建立/中斷連線、送出/收到資料時會拋出事件
	/// </summary>
	public interface IVehicleCommunicator : ISystemWithLoopTask
	{
		event EventHandler<RemoteConnectStateChangedEventArgs> RemoteConnectStateChanged;
		event EventHandler<LocalListenStateChangedEventArgs> LocalListenStateChanged;
		event EventHandler<SentSerializableDataEventArgs> SentSerializableData;
		event EventHandler<ReceivedSerializableDataEventArgs> ReceivedSerializableData;
		event EventHandler<SentSerializableDataEventArgs> SentSerializableDataSuccessed;
		event EventHandler<SentSerializableDataEventArgs> SentSerializableDataFailed;
		
		/// <summary>監聽狀態</summary>
		ListenState mListenState { get; }
		/// <summary>連線中的 Client 數量</summary>
		int mClientCount { get; }
		/// <summary>連線中的 Client 地址資訊 (IP:Port)</summary>
		List<string> mClientAddressInfo { get; }

		/// <summary>確認指定 IP:Port 是否連線中</summary>
		bool IsIpPortConnected(string IpPort);
		/// <summary>開始監聽</summary>
		void StartListen();
		/// <summary>停止監聽</summary>
		void StopListen();
		/// <summary>向指定 IP:Port 傳送序列化資料</summary>
		void SendSerializableData(string IpPort, object Data);
		/// <summary>向指定 IP:Port 傳送序列化資料 Goto</summary>
		void SendSerializableData_Goto(string IpPort, string Target);
		/// <summary>向指定 IP:Port 傳送序列化資料 GotoPoint</summary>
		void SendSerializableData_GotoPoint(string IpPort, int X, int Y);
		/// <summary>向指定 IP:Port 傳送序列化資料 GotoTowardPoint</summary>
		void SendSerializableData_GotoTowardPoint(string IpPort, int X, int Y, int Toward);
		/// <summary>向指定 IP:Port 傳送序列化資料 Dock</summary>
		void SendSerializableData_Dock(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 Stop</summary>
		void SendSerializableData_Stop(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 InsertMovingBuffer</summary>
		void SendSerializableData_InsertMovingBuffer(string IpPort, int X, int Y);
		/// <summary>向指定 IP:Port 傳送序列化資料 RemoveMovingBuffer</summary>
		void SendSerializableData_RemoveMovingBuffer(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 PauseMoving</summary>
		void SendSerializableData_PauseMoving(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 ResumeMoving</summary>
		void SendSerializableData_ResumeMoving(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 RequestMapList</summary>
		void SendSerializableData_RequestMapList(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 GetMap</summary>
		void SendSerializableData_GetMap(string IpPort, string MapName);
		/// <summary>向指定 IP:Port 傳送序列化資料 UploadMapToAGV</summary>
		void SendSerializableData_UploadMapToAGV(string IpPort, string MapPath);
		/// <summary>向指定 IP:Port 傳送序列化資料 ChangeMap</summary>
		void SendSerializableData_ChangeMap(string IpPort, string MapName);
	}

	public class RemoteConnectStateChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }
		public ConnectState NewState { get; private set; }

		public RemoteConnectStateChangedEventArgs(DateTime OccurTime, string IpPort, ConnectState NewState) : base()
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
			this.NewState = NewState;
		}
	}

	public class LocalListenStateChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public int Port { get; private set; }
		public ListenState NewState { get; private set; }

		public LocalListenStateChangedEventArgs(DateTime OccurTime, ListenState NewState, int Port) : base()
		{
			this.OccurTime = OccurTime;
			this.NewState = NewState;
			this.Port = Port;
		}
	}

	public class SentSerializableDataEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }
		public object Data { get; private set; }

		public SentSerializableDataEventArgs(DateTime OccurTime, string IpPort, object Data) : base()
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
			this.Data = Data;
		}
	}

	public class ReceivedSerializableDataEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }
		public object Data { get; private set; }

		public ReceivedSerializableDataEventArgs(DateTime OccurTime, string IpPort, object Data) : base()
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
			this.Data = Data;
		}
	}
}
