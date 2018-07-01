using System;

namespace Serialization
{
    /// <summary>
    /// 可支援序列化通訊的字串訊息
    /// </summary>
    [Serializable]
    public class StringMessage : Serializable
    {
        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Message { get; set; }
    }
}
