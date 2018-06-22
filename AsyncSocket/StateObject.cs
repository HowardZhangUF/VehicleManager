using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocket
{
    /// <summary>
    /// 資料接收傳遞類別
    /// </summary>
    internal class StateObject
    {
        /// <summary>
        /// 資料長度
        /// </summary>
        public const int BUFFER_SIZE = 8192;

        /// <summary>
        /// 存放資料陣列
        /// </summary>
        public byte[] buffer { get; set; } = new byte[BUFFER_SIZE];

        /// <summary>
        /// 通訊管理器
        /// </summary>
        public Socket workSocket { get; set; } = null;
    }
}
