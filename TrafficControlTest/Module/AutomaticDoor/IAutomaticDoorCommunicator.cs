using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.NewCommunication;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public interface IAutomaticDoorCommunicator : ISystemWithLoopTask
	{
		event EventHandler<ClientAddedEventArgs> ClientAdded;
		event EventHandler<ClientRemovedEventArgs> ClientRemoved;
		event EventHandler<ConnectStateChangedEventArgs> RemoteConnectStateChanged;
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
}
