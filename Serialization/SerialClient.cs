using AsyncSocket;
using System;
using static Serialization.StandardOperate;

namespace Serialization
{
    /// <summary>
    /// 非同步通訊用戶端
    /// </summary>
    internal class SerialClient : IClient
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 非同步通訊用戶端基底類別
        /// </summary>
        private Client @base = new Client();

        /// <summary>
        /// 接收資料暫存區
        /// </summary>
        private byte[] rxBuffer = new byte[] { };

        /// <summary>
        /// 獲取從遠端來的資料
        /// </summary>
        private void base_ReceivedDataEvent(object sender, ReceivedDataEventArgs e)
        {
            lock (key)
            {
                Array.Resize(ref rxBuffer, rxBuffer.Length + e.Data.Length);
                ISerializable obj = Deserialize(ref rxBuffer);
                if (obj != null)
                {
                    var arg = new ReceivedSerialDataEventArgs
                    {
                        Data = obj,
                        RemoteInfo = e.RemoteInfo,
                        ReceivedTime = DateTime.Now,
                    };
                    new WaitTask(() => ReceivedSerialDataEvent?.Invoke(this, arg)).Start();
                }
            }
        }

        public SerialClient()
        {
            @base.ReceivedDataEvent += base_ReceivedDataEvent;
        }

        /// <summary>
        /// 連線狀態改變事件，使用 Task 發布
        /// </summary>
        public event ConnectStatusChangedEvent ConnectStatusChangedEvent { add { @base.ConnectStatusChangedEvent += value; } remove { @base.ConnectStatusChangedEvent -= value; } }

        /// <summary>
        /// 從遠端接收到序列化資料的事件，使用 Task 發布
        /// </summary>
        public event ReceivedSerialDataEvent ReceivedSerialDataEvent;

        /// <summary>
        /// 連線狀態
        /// </summary>
        public EConnectStatus ConnectStatus => @base.ConnectStatus;

        /// <summary>
        /// 遠端位址
        /// </summary>
        public string RemoteIP => @base.RemoteIP;

        /// <summary>
        /// 遠端埠號
        /// </summary>
        public int RemotePort => @base.RemotePort;

        /// <summary>
        /// 連線
        /// </summary>
        public void Connect(string ip, int port) => @base.Connect(ip, port);

        /// <summary>
        /// 斷線
        /// </summary>
        public void Disconnect() => @base.Disconnect();

        /// <summary>
        /// 發送資料至遠端(使用序列化傳輸)
        /// </summary>
        public void Send(string data) => @base.Send(GenStringMessage(data).Serialize());

        /// <summary>
        /// 發送資料至遠端(使用序列化傳輸)
        /// </summary>
        public void Send(byte[] data) => @base.Send(GenByteArray(data).Serialize());

        /// <summary>
        /// 發送資料至遠端(使用序列化傳輸)
        /// </summary>
        public void Send(ISerializable data) => @base.Send(data.Serialize());
    }
}
