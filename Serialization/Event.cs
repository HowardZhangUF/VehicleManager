using AsyncSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
