using System;

namespace Serialization
{
    /// <summary>
    /// 可支援序列化通訊的陣列
    /// </summary>
    [Serializable]
    public class ByteArray : Serializable
    {
        /// <summary>
        /// 訊息內容
        /// </summary>
        public byte[] Message { get; set; }
    }
}
