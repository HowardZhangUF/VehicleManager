using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Interface
{
	public interface IHostCommunicator
	{
		event EventHandlerDateTime SystemStarted;
		event EventHandlerDateTime SystemStopped;
		event EventHandlerRemoteConnectState RemoteConnectStateChanged;
		event EventHandlerLocalListenState LocalListenStateChanged;
		event EventHandlerSentString SentString;
		event EventHandlerReceivedString ReceivedString;

		ListenState mListenState { get; }
		int mClientCout { get; }
		void StartListen(int Port);
		void StopListen();
		void SendString(string IpPort, string Data);
	}
}
