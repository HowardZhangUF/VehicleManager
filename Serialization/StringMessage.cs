using System;

namespace Serialization
{
    /// <summary>
    /// 可支援序列化通訊的字串訊息
    /// </summary>
    [Serializable]
    public class StringMessage : ISerializable
    {
        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 訊息時間戳
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 流水號，用來識別遠端訊息回應的對象
        /// </summary>
        public uint TxID { get; set; }
    }
}
