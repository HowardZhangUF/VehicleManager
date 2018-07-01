using System;
using System.ComponentModel;

namespace Serialization
{
    /// <summary>
    /// 可序列化，所有可被序列化的類別都繼承此類別，並且須將類別加入 "[<see cref="SerializableAttribute"/>]" 標籤
    /// </summary>
    [Serializable]
    public abstract class Serializable
    {
        /// <summary>
        /// 訊息時間戳
        /// </summary>
        [DisplayName("Time Stamp")]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 流水號，用來識別遠端訊息回應的對象
        /// </summary>
        [DisplayName("TxID")]
        public ulong TxID { get; private set; }

        /// <summary>
        /// 流水號
        /// </summary>
        private static ulong SerialNumber = 0;

        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private static readonly object key = new object();

        public Serializable()
        {
            lock (key)
            {
                SerialNumber++;
                TxID = SerialNumber;
            }
        }

        /// <summary>
        /// 從 <see cref="serializable"/> 覆寫 TxID
        /// </summary>
        /// <param name="serializable"></param>
        public void CopyTxIDFrom<T>(T serializable) where T : Serializable
        {
            TxID = serializable.TxID;
        }
    }
}