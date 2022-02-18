using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryForVM
{
	public static class ExceptionHandling
	{
		private static object mLock = new object();

		public static void HandleException(Exception Ex)
		{
			HandleException("Exception", Ex.ToString());
		}
		public static void HandleThreadException(object Sender, ThreadExceptionEventArgs E)
		{
			HandleException("ThreadException", E.Exception.ToString());
		}
		public static void HandleUnhandledException(object Sender, UnhandledExceptionEventArgs E)
		{
			HandleException("UnhandledException", E.ExceptionObject.ToString());
		}

		private static void HandleException(string ExceptionPrefix, string ExceptionString)
		{
			lock (mLock)
			{
				string directoryPath = ".\\..\\VehicleManagerData\\Exception";
				string filePath = $"{directoryPath}\\{ExceptionPrefix}{DateTime.Now.ToString("yyyyMMdd")}.txt";
				string message = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} - [{ExceptionPrefix}] - {ExceptionString}\r\n";

				if (!System.IO.Directory.Exists(directoryPath)) System.IO.Directory.CreateDirectory(directoryPath);
				if (!System.IO.File.Exists(filePath)) System.IO.File.Create(filePath).Close();
				System.IO.File.AppendAllText(filePath, message);
			}
		}
	}
}
