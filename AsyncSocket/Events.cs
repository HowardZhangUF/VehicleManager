using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocket
{
    /// <summary>
    /// 終端資訊
    /// </summary>
    public class EndPointInfo
    {
        /// <summary>
        /// 位址
        /// </summary>
        public string IP { get; }

        /// <summary>
        /// 埠號
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// 建立終端資訊
        /// </summary>
        public EndPointInfo(string ip, int port)
        {
            IP = ip;
            Port = port;
        }

        /// <summary>
        /// 建立終端資訊
        /// </summary>
        public EndPointInfo(IPEndPoint endPoint)
        {
            IP = endPoint.Address.ToString();
            Port = endPoint.Port;
        }

        /// <summary>
        /// IP:Port
        /// </summary>
        public override string ToString()
        {
            return $"{IP}:{Port}";
        }
    }

    #region 接收資料
    /// <summary>
    /// 接收資料委派
    /// </summary>
    public delegate void ReceivedDataEvent(object sender, ReceivedDataEventArgs e);

    /// <summary>
    /// 接收資料事件參數
    /// </summary>
    public class ReceivedDataEventArgs : EventArgs
    {
        /// <summary>
        /// 遠端資訊，有可能是 Server 端也有可能是 Client 端。
        /// </summary>
        public EndPointInfo RemoteInfo { get; set; }

        /// <summary>
        /// 接收到的資料
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 接收時間
        /// </summary>
        public DateTime ReceivedTime { get; set; }
    }
    #endregion

    #region 連線狀態
    /// <summary>
    /// 連線狀態
    /// </summary>
    public enum EConnectStatus
    {
        /// <summary>
        /// 斷線
        /// </summary>
        Disconnect,

        /// <summary>
        /// 連線
        /// </summary>
        Connect,
    }

    /// <summary>
    /// 連線狀態改變委派
    /// </summary>
    public delegate void ConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e);

    /// <summary>
    /// 連線狀態改變事件參數
    /// </summary>
    public class ConnectStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 遠端資訊，有可能是 Server 端也有可能是 Client 端。
        /// </summary>
        public EndPointInfo RemoteInfo { get; set; }

        /// <summary>
        /// 連線狀態
        /// </summary>
        public EConnectStatus ConnectStatus { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime StatusChangedTime { get; set; }
    }
    #endregion

    #region 監聽狀態
    /// <summary>
    /// 監聽狀態
    /// </summary>
    public enum EListenStatus
    {
        /// <summary>
        /// 未監聽
        /// </summary>
        Idle,

        /// <summary>
        /// 監聽中
        /// </summary>
        Listening,
    }

    /// <summary>
    /// 監聽狀態改變委派
    /// </summary>
    public delegate void ListenStatusChangedEvent(object sender, ListenStatusChangedEventArgs e);

    /// <summary>
    /// 監聽狀態改變事件參數
    /// </summary>
    public class ListenStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 監聽狀態
        /// </summary>
        public EListenStatus ListenStatus { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime StatusChangedTime { get; set; }
    }
    #endregion
}
