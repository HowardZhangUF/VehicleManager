using AsyncSocket;
using System;
using System.Net.Sockets;
using static Serialization.StandardOperate;

namespace Serialization
{
    #region 接收序列化資料

    /// <summary>
    /// 接收序列化資料委派
    /// </summary>
    public delegate void ReceivedSerialDataEvent(object sender, ReceivedSerialDataEventArgs e);

    /// <summary>
    /// 接收序列化資料事件參數
    /// </summary>
    public class ReceivedSerialDataEventArgs : EventArgs
    {
        /// <summary>
        /// 接收到的序列化資料
        /// </summary>
        public ISerializable Data { get; set; }

        /// <summary>
        /// 接收時間
        /// </summary>
        public DateTime ReceivedTime { get; set; }

        /// <summary>
        /// 遠端資訊，有可能是 Server 端也有可能是 Client 端。
        /// </summary>
        public EndPointInfo RemoteInfo { get; set; }
    }

    #endregion 接收序列化資料

    /// <summary>
    /// 非同步通訊伺服端
    /// </summary>
    internal class SerialServer : IServer
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 非同步通訊伺服端基底類別
        /// </summary>
        private Server @base = new Server();

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

        public SerialServer()
        {
            @base.ReceivedDataEvent += base_ReceivedDataEvent;
        }

        /// <summary>
        /// 連線狀態改變事件，使用 Task 發布
        /// </summary>
        public event ConnectStatusChangedEvent ConnectStatusChangedEvent { add { @base.ConnectStatusChangedEvent += value; } remove { @base.ConnectStatusChangedEvent -= value; } }

        /// <summary>
        /// 監聽狀態改變事件，使用 Task 發布
        /// </summary>
        public event ListenStatusChangedEvent ListenStatusChangedEvent { add { @base.ListenStatusChangedEvent += value; } remove { @base.ListenStatusChangedEvent -= value; } }

        /// <summary>
        /// 從遠端接收到序列化資料的事件，使用 Task 發布
        /// </summary>
        public event ReceivedSerialDataEvent ReceivedSerialDataEvent;

        /// <summary>
        /// 監聽狀態
        /// </summary>
        public EListenStatus ListenStatus => @base.ListenStatus;

        /// <summary>
        /// 切斷所有連線
        /// </summary>
        public void Disconnect() => @base.Disconnect();

        /// <summary>
        /// 切斷所有連線
        /// </summary>
        public void Disconnect(string ipport) => @base.Disconnect(ipport);

        /// <summary>
        /// 切斷所有連線
        /// </summary>
        public void Disconnect(Socket handler) => @base.Disconnect(handler);

        /// <summary>
        /// 發送資料至所有遠端(使用序列化傳輸)
        /// </summary>
        public void Send(string data) => @base.Send(GenStringMessage(data).Serialize());

        /// <summary>
        /// 發送資料至所有遠端(使用序列化傳輸)
        /// </summary>
        public void Send(byte[] data) => @base.Send(GenByteArray(data).Serialize());

        /// <summary>
        /// 發送資料至所有遠端(使用序列化傳輸)
        /// </summary>
        public void Send(ISerializable data) => @base.Send(data.Serialize());

        /// <summary>
        /// 發送資料至指定遠端(使用序列化傳輸)
        /// </summary>
        public void Send(string ipport, string data) => @base.Send(ipport, GenStringMessage(data).Serialize());

        /// <summary>
        /// 發送資料至指定遠端(使用序列化傳輸)
        /// </summary>
        public void Send(string ipport, byte[] data) => @base.Send(ipport, GenByteArray(data).Serialize());

        /// <summary>
        /// 發送資料至指定遠端(使用序列化傳輸)
        /// </summary>
        public void Send(string ipport, ISerializable data) => @base.Send(data.Serialize());

        /// <summary>
        /// 開始監聽連線
        /// </summary>
        public void StartListening(string ip, int port) => @base.StartListening(ip, port);

        /// <summary>
        /// 停止監聽
        /// </summary>
        public void StopListen() => @base.StopListen();
    }
}
