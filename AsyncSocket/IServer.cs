using System.Net.Sockets;

namespace AsyncSocket
{
    /// <summary>
    /// 非同步通訊伺服端
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// 連線狀態改變事件，使用 Task 發布
        /// </summary>
        event ConnectStatusChangedEvent ConnectStatusChangedEvent;

        /// <summary>
        /// 監聽狀態改變事件，使用 Task 發布
        /// </summary>
        event ListenStatusChangedEvent ListenStatusChangedEvent;

        /// <summary>
        /// 監聽狀態
        /// </summary>
        EListenStatus ListenStatus { get; }

        /// <summary>
        /// 切斷所有連線
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 切斷所有連線
        /// </summary>
        void Disconnect(string ipport);

        /// <summary>
        /// 切斷所有連線
        /// </summary>
        void Disconnect(Socket handler);

        /// <summary>
        /// 發送資料至所有遠端
        /// </summary>
        void Send(string data);

        /// <summary>
        /// 發送資料至所有遠端
        /// </summary>
        void Send(byte[] data);

        /// <summary>
        /// 發送資料至指定遠端
        /// </summary>
        void Send(string ipport, string data);

        /// <summary>
        /// 發送資料至指定遠端
        /// </summary>
        void Send(string ipport, byte[] data);

        /// <summary>
        /// 開始監聽連線
        /// </summary>
        void StartListening(string ip, int port);

        /// <summary>
        /// 停止監聽
        /// </summary>
        void StopListen();
    }
}