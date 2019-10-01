using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleCommunicator;

namespace TrafficControlTest.Module.General.Interface
{
	public interface ICommunicatorServer
	{
		event EventHandlerDateTime SystemStarted;
		event EventHandlerDateTime SystemStopped;
		event EventHandlerRemoteConnectState RemoteConnectStateChanged;
		event EventHandlerLocalListenState LocalListenStateChanged;
		event EventHandlerSentString SentString;
		event EventHandlerReceivedString ReceivedString;
		event EventHandlerSentSerializableData SentSerializableData;
		event EventHandlerReceivedSerializableData ReceivedSerializableData;
		event EventHandlerSentSerializableData SentSerializableDataSuccessed;
		event EventHandlerSentSerializableData SentSerializableDataFailed;

		ListenState mListenState { get; }
		int mClientCout { get; }
		void StartListen(int Port);
		void StopListen();
		void SendString(string IpPort, string Data);
		void SendSerializableData(string IpPort, object Data);
	}
}
