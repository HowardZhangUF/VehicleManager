using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MD5Hash
{
    /// <summary>
    /// MD5 雜湊值計算
    /// </summary>
    public static class MD5
    {
        /// <summary>
        /// 計算陣列的雜湊值，並回傳十六進位字串
        /// </summary>
        public static string GetByteArrayHash(byte[] bytes)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return ToBase16String(md5.ComputeHash(bytes));
            }
        }

        /// <summary>
        /// 計算檔案的雜湊值，並回傳十六進位字串
        /// </summary>
        public static string GetFileHash(string file)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    return ToBase16String(md5.ComputeHash(stream));
                }
            }
        }

        /// <summary>
        /// 計算字串的雜湊值，並回傳十六進位字串
        /// </summary>
        public static string GetStringHash(string str)
        {
            return GetByteArrayHash(Encoding.Default.GetBytes(str));
        }

        /// <summary>
        /// 回傳十六進位字串
        /// </summary>
        private static string ToBase16String(this IEnumerable<byte> bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte bt in bytes)
            {
                sb.Append(bt.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}