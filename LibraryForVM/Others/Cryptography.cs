using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForVM
{
	public static class Cryptography
	{
		private static byte[] mKey = Encoding.ASCII.GetBytes("castec27");
		private static byte[] mIv = Encoding.ASCII.GetBytes("27635744");

		public static string EncryptString(string Src)
		{
			string result = string.Empty;
			using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
			{
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(mKey, mIv), CryptoStreamMode.Write))
					{
						byte[] srcByte = Encoding.ASCII.GetBytes(Src);
						cs.Write(srcByte, 0, srcByte.Length);
						cs.FlushFinalBlock();
						result = Convert.ToBase64String(ms.ToArray());
					}
				}
			}
			return result;
		}
		public static string DecryptString(string Src)
		{
			string result = string.Empty;
			using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
			{
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(mKey, mIv), CryptoStreamMode.Write))
					{
						byte[] srcByte = Convert.FromBase64String(Src);
						cs.Write(srcByte, 0, srcByte.Length);
						cs.FlushFinalBlock();
						result = Encoding.ASCII.GetString(ms.ToArray());
					}
				}
			}
			return result;
		}
	}
}
