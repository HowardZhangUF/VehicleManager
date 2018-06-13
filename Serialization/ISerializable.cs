using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialization
{
    /// <summary>
    /// 可序列化，所有可被序列化的類別都繼承此介面，並且須將類別加入 "[<see cref="Serializable"/>]" 標籤
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// 流水號，用來識別遠端訊息回應的對象
        /// </summary>
        uint TxID { get; set; }
    }

    /// <summary>
    /// 標準操作
    /// </summary>
    public static class StandardOperate
    {
        /// <summary>
        /// <para>標頭</para>
        /// </summary>
        private static readonly byte[] Head = new byte[] { 0xFF, 0xFF };

        /// <summary>
        /// <para>檢查碼</para>
        /// </summary>
        public static readonly byte CheckSum = 0x00;

        /// <summary>
        /// 檢查碼長度
        /// </summary>
        public static readonly int CheckSumLength = 1;

        /// <summary>
        /// 整數資料長度
        /// </summary>
        public static readonly int Int32Length = 4;

        /// <summary>
        /// <para>反序列化第一筆資料，並刪除原始資料中已經反序列化的部分</para>
        /// </summary>
        public static ISerializable Deserialize(ref byte[] array)
        {
            for (int ii = 0; ii + Head.Length + Int32Length + CheckSumLength - 1 < array.Length; ii++)
            {
                // 比對開頭
                if (array[ii] == Head[0] && array[ii + 1] == Head[1])
                {
                    int dataLength = BitConverter.ToInt32(array, ii + Head.Length);
                    // 判斷長度
                    if ((dataLength >= 0) && (ii + Head.Length + Int32Length + dataLength + CheckSumLength - 1 < array.Length))
                    {
                        // 判斷檢查碼
                        if (array[ii + Head.Length + Int32Length + dataLength + CheckSumLength - 1] == CheckSum)
                        {
                            // 將資料反序列化
                            byte[] data = array.Skip(ii + Head.Length + Int32Length).Take(dataLength).ToArray();
                            ISerializable obj = null;
                            using (MemoryStream stream = new MemoryStream(data))
                            {
                                IFormatter formatter = new BinaryFormatter();
                                stream.Seek(0, SeekOrigin.Begin);
                                obj = formatter.Deserialize(stream) as ISerializable;
                            }
                            array = array.Skip(ii + Head.Length + Int32Length + dataLength + CheckSumLength).ToArray();
                            return obj;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 產生可支援序列化通訊的陣列
        /// </summary>
        public static ByteArray GenByteArray(byte[] message) => new ByteArray() { Message = message };

        /// <summary>
        /// 產生可支援序列化通訊的字串訊息
        /// </summary>
        public static StringMessage GenStringMessage(string message) => new StringMessage() { Message = message };

        /// <summary>
        /// <para>將資料序列化，並回傳序列化之後的結果 </para>
        /// <para>格式如下：</para>
        /// <para>標頭+資料量+實際資料+檢查碼 </para>
        /// <para>標頭佔 2 bytes 固定為 0xFF,0xFF </para>
        /// <para>資料量佔 4 bytes，等於 實際資料.Count </para>
        /// <para>檢查碼佔 1 byte，固定為 0x00 </para>
        /// </summary>
        public static byte[] Serialize(this ISerializable data)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            byte[] org = stream.ToArray();

            byte[] res = new byte[Head.Length + Int32Length + org.Length + CheckSumLength];
            Head.CopyTo(res, 0);
            BitConverter.GetBytes(org.Length).CopyTo(res, Head.Length);
            org.CopyTo(res, Head.Length + Int32Length);
            res[Head.Length + Int32Length + org.Length] = CheckSum;
            return res;
        }
    }

    /// <summary>
    /// 可支援序列化通訊的陣列
    /// </summary>
    [Serializable]
    public class ByteArray : ISerializable
    {
        /// <summary>
        /// 訊息內容
        /// </summary>
        public byte[] Message { get; set; }

        /// <summary>
        /// 流水號，用來識別遠端訊息回應的對象
        /// </summary>
        public uint TxID { get; set; }
    }

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
        /// 流水號，用來識別遠端訊息回應的對象
        /// </summary>
        public uint TxID { get; set; }
    }
}
