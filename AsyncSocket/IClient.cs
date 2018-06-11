namespace AsyncSocket
{
    /// <summary>
    /// 非同步通訊用戶端
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// 連線狀態改變事件，使用 Task 發布
        /// </summary>
        event ConnectStatusChangedEvent ConnectStatusChangedEvent;

        /// <summary>
        /// 從遠端接收到資料的事件，使用 Task 發布
        /// </summary>
        event ReceivedDataEvent ReceivedDataEvent;

        /// <summary>
        /// 連線狀態
        /// </summary>
        EConnectStatus ConnectStatus { get; }

        /// <summary>
        /// 遠端位址
        /// </summary>
        string RemoteIP { get; }

        /// <summary>
        /// 遠端埠號
        /// </summary>
        int RemotePort { get; }

        /// <summary>
        /// 連線
        /// </summary>
        void Connect(string ip, int port);

        /// <summary>
        /// 斷線
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 發送資料至遠端
        /// </summary>
        void Send(string data);

        /// <summary>
        /// 發送資料至遠端
        /// </summary>
        void Send(byte[] data);
    }
}