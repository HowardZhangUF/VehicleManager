using System;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CommunicationHost
{
	/// <summary>
	/// - 提供監聽方法以供客戶端系統建立連線
	/// - 提供傳送資料給客戶端系統的方法
	/// - 當建立/中斷連線、送出/收到資料時會拋出事件
	/// </summary>
	public interface IHostCommunicator : ISystemWithLoopTask
	{
		event EventHandler<RemoteConnectStateChangedEventArgs> RemoteConnectStateChanged;
		event EventHandler<LocalListenStateChangedEventArgs> LocalListenStateChanged;
		event EventHandler<SentStringEventArgs> SentString;
		event EventHandler<ReceivedStringEventArgs> ReceivedString;

		ListenState mListenState { get; }
		int mClientCout { get; }

		void StartListen();
		void StopListen();
		void SendString(string Data);
		void SendString(string IpPort, string Data);
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

	public class SentStringEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }
		public string Data { get; private set; }

		public SentStringEventArgs(DateTime OccurTime, string IpPort, string Data) : base()
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
			this.Data = Data;
		}
	}

	public class ReceivedStringEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }
		public string Data { get; private set; }

		public ReceivedStringEventArgs(DateTime OccurTime, string IpPort, string Data) : base()
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
			this.Data = Data;
		}
	}
}
