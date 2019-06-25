using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleCommunicator;

namespace TrafficControlTest.Interface
{
	/// <summary>提供與 Vehicle 連線、溝通的方法</summary>
	public interface IVehicleCommunicator
	{
		event EventHandlerDateTime VehicleCommunicatorStarted;
		event EventHandlerDateTime VehicleCommunicatorStopped;

		event EventHandlerRemoteConnectState VehicleConnectStateChanged;
		event EventHandlerLocalListenState VehicleCommunicatorListenStateChanged;

		event EventHandlerSentSerializableData SentSerializableData;
		event EventHandlerReceivedSerializableData ReceivedSerializableData;

		/// <summary>監聽狀態。小於 0 時代表未監聽；大於等於 0 時代表監聽中，數字代表連線中的 Client 數量</summary>
		ListenState mListenState { get; }
		/// <summary>連線中的 Client 數量</summary>
		int mClientCount { get; }
		/// <summary>開始監聽</summary>
		void StartListen(int Port);
		/// <summary>停止監聽</summary>
		void StopListen();
		/// <summary>向指定 IP:Port 傳送序列化資料</summary>
		void SendSerializableData(string IpPort, object Data);
	}
}
