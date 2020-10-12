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
	public interface ICommunicatorClientUsingString : ISystemWithLoopTask
	{
		event EventHandler<ConnectStateChangedEventArgs> ConnectStateChanged;
		event EventHandler<SentStringEventArgs> SentString;
		event EventHandler<ReceivedStringEventArgs> ReceivedString;

		string mRemoteIpPort { get; }
		bool mIsConnected { get; }

		void Connect(string Ip, int Port);
		void Connect(string IpPort);
		void Disconnect();
		void Send(string Data);
	}

	public class ConnectStateChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }
		public bool Connected { get; private set; }

		public ConnectStateChangedEventArgs(DateTime OccurTime, string IpPort, bool Connected) : base()
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
			this.Connected = Connected;
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
