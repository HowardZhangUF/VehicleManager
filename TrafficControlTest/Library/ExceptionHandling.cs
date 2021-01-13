using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Library
{
	public static class ExceptionHandling
	{
		private static object mLock = new object();

		public static void HandleException(Exception Ex)
		{
			lock (mLock)
			{
				string directoryPath = ".\\Exception";
				string filePath = $".\\Exception\\Exception{DateTime.Now.ToString("yyyyMMdd")}.txt";
				string message = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} - [Exception] - {Ex.ToString()}\r\n";

				if (!System.IO.Directory.Exists(directoryPath)) System.IO.Directory.CreateDirectory(directoryPath);
				if (!System.IO.File.Exists(filePath)) System.IO.File.Create(filePath).Close();
				System.IO.File.AppendAllText(filePath, message);
			}
		}
	}
}
