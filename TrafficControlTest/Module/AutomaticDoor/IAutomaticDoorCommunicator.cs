using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public interface IAutomaticDoorCommunicator : ISystemWithLoopTask
	{
		event EventHandler<ClientAddedEventArgs> ClientAdded;
		event EventHandler<ClientRemovedEventArgs> ClientRemoved;
		event EventHandler<RemoteConnectStateChangedEventArgs> RemoteConnectStateChanged;
		event EventHandler<SentDataEventArgs> SentData;
		event EventHandler<ReceivedDataEventArgs> ReceivedData;

		List<string> GetClientList();
		bool IsConnected(string IpPort);
		void Add(string IpPort);
		void Remove(string IpPort);
		void Connect(string IpPort);
		void Disconnect(string IpPort);
		void RemoveAll();
		void SendData(string IpPort, string Data);
	}

	public class ClientAddedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }

		public ClientAddedEventArgs(DateTime OccurTime, string IpPort)
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
		}
	}

	public class ClientRemovedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }

		public ClientRemovedEventArgs(DateTime OccurTime, string IpPort)
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
		}
	}

	public class RemoteConnectStateChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }
		public bool Connected { get; private set; }

		public RemoteConnectStateChangedEventArgs(DateTime OccurTime, string IpPort, bool Connected)
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
			this.Connected = Connected;
		}
	}

	public class SentDataEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }
		public string Data { get; private set; }

		public SentDataEventArgs(DateTime OccurTime, string IpPort, string Data)
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
			this.Data = Data;
		}
	}

	public class ReceivedDataEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string IpPort { get; private set; }
		public string Data { get; private set; }

		public ReceivedDataEventArgs(DateTime OccurTime, string IpPort, string Data)
		{
			this.OccurTime = OccurTime;
			this.IpPort = IpPort;
			this.Data = Data;
		}
	}
}
