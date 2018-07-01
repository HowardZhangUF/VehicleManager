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
        public uint TxID { get; set; }
    }
}
