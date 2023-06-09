﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LibraryForVM
{
	public static class ExceptionHandling
	{
		private static object mLock = new object();

		public static void HandleException(Exception Ex)
		{
			HandleException("Exception", Ex);
		}
		public static void HandleThreadException(object Sender, ThreadExceptionEventArgs E)
		{
			HandleException("ThreadException", E.Exception.ToString());
		}
		public static void HandleUnhandledException(object Sender, UnhandledExceptionEventArgs E)
		{
			HandleException("UnhandledException", E.ExceptionObject.ToString());
		}
        /// <summary> 擴充版 Exception(個人專門格式) </summary>  
        private static void HandleException(string ExceptionPrefix, Exception Ex)
        {
            var trace = new StackTrace(Ex, true);
            int cnt = 0;
            string directoryPath = ".\\..\\VehicleManagerData\\Exception";
            string filePath = $"{directoryPath}\\{ExceptionPrefix}{DateTime.Now.ToString("yyyyMMdd")}.txt";
            if (!System.IO.Directory.Exists(directoryPath)) System.IO.Directory.CreateDirectory(directoryPath);
            if (!System.IO.File.Exists(filePath)) System.IO.File.Create(filePath).Close();
            for (var i = 0; i < trace.GetFrames().Length; i++)
            {
                int linenumber = trace.GetFrame(i).GetFileLineNumber();
                string location = trace.GetFrame(i).GetFileName();
                string classname = trace.GetFrame(i).GetMethod().DeclaringType.Name;
                string method = trace.GetFrame(i).GetMethod().Name;
                string message;
                if (linenumber > 0)
                {
                    if (cnt == 0)
                    {
                        message = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} - [{ExceptionPrefix}] -錯誤原因:{Ex.Message}\r\n";
                        System.IO.File.AppendAllText(filePath, message);
                        Console.Write(message);
                    }

                    string level = cnt == 0 ? "影響當 前" : $"影響前{cnt}層";
                    message = $"  {level}位置:{location} 類別:{classname} 函式:{method},行號:{linenumber}\r\n";
                    System.IO.File.AppendAllText(filePath, message);
                    Console.Write(message);
                    cnt++;
                }

            }
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
				Console.WriteLine(message);
			}
		}
	}
}
