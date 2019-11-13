using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Interface
{
	/// <summary>
	/// - 提供監聽方法以供客戶端系統建立連線
	/// - 提供傳送資料給客戶端系統的方法
	/// - 當建立/中斷連線、送出/收到資料時會拋出事件
	/// </summary>
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

		void SetConfigOfListenPort(int Port);
		int GetConfigOfListenPort();
		void StartListen();
		void StopListen();
		void SendString(string IpPort, string Data);
	}
}
