using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Communication
{
	public interface ICommunicatorClient
	{
		event EventHandlerDateTime SystemStarted;
		event EventHandlerDateTime SystemStopped;
		event EventHandlerRemoteConnectState ConnectStateChanged;
		event EventHandlerSentSerializableData SentSerializableData;
		event EventHandlerReceivedSerializableData ReceivedSerializableData;

		/// <summary>遠端點資訊 (IP:Port)</summary>
		string mRemoteIpPort { get; }
		/// <summary>連線狀態</summary>
		ConnectState mConnectState { get; }

		/// <summary>開始連線</summary>
		void StartConnect(string ip, int port);
		/// <summary>停止連線</summary>
		void StopConnect();
		/// <summary>傳送序列化資料</summary>
		void SendSerializableData(object Data);
	}
}
