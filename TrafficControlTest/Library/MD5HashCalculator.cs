using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Library
{
	public class MD5HashCalculator
	{
		public static string CalculateFileHash(string FilePath)
		{
			using (MD5 md5 = MD5.Create())
			{
				using (FileStream fs = new FileStream(FilePath, FileMode.Open))
				{
					var hash = md5.ComputeHash(fs);

					StringBuilder sb = new StringBuilder();
					for (int i = 0; i < hash.Length; ++i)
					{
						sb.Append(hash[i].ToString("X2"));
					}
					return sb.ToString();
				}
			}
		}
	}
}
