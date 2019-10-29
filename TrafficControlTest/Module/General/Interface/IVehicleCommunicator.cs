using System.Collections.Generic;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleCommunicator;

namespace TrafficControlTest.Interface
{
	/// <summary>提供與 Vehicle 連線、溝通的方法</summary>
	public interface IVehicleCommunicator
	{
		event EventHandlerDateTime SystemStarted;
		event EventHandlerDateTime SystemStopped;
		event EventHandlerRemoteConnectState RemoteConnectStateChanged;
		event EventHandlerLocalListenState LocalListenStateChanged;
		event EventHandlerSentSerializableData SentSerializableData;
		event EventHandlerReceivedSerializableData ReceivedSerializableData;
		event EventHandlerSentSerializableData SentSerializableDataSuccessed;
		event EventHandlerSentSerializableData SentSerializableDataFailed;

		/// <summary>監聽狀態</summary>
		ListenState mListenState { get; }
		/// <summary>連線中的 Client 數量</summary>
		int mClientCount { get; }
		/// <summary>連線中的 Client 地址資訊 (IP:Port)</summary>
		List<string> mClientAddressInfo { get; }
		/// <summary>開始監聽</summary>
		void StartListen(int Port);
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
		/// <summary>向指定 IP:Port 傳送序列化資料 InsertMovingBuffer</summary>
		void SendSerializableData_InsertMovingBuffer(string IpPort, string Buffer);
		/// <summary>向指定 IP:Port 傳送序列化資料 RemoveMovingBuffer</summary>
		void SendSerializableData_RemoveMovingBuffer(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 PauseMoving</summary>
		void SendSerializableData_PauseMoving(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 ResumeMoving</summary>
		void SendSerializableData_ResumeMoving(string IpPort);
	}
}
