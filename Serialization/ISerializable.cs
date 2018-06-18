using System;

namespace Serialization
{
    /// <summary>
    /// 可序列化，所有可被序列化的類別都繼承此介面，並且須將類別加入 "[<see cref="Serializable"/>]" 標籤
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// 訊息時間戳
        /// </summary>
        DateTime TimeStamp { get; set; }

        /// <summary>
        /// 流水號，用來識別遠端訊息回應的對象
        /// </summary>
        uint TxID { get; set; }
    }
}
