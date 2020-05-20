using System;
using System.Collections.Generic;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Log
{
	public interface ILogExporter : ISystemWithConfig
	{
		/*
		 * 使用 Add() 方法設定要輸出的資料夾、檔案的路徑，
		 * 之後使用 StartExport() 方法即可開始輸出。
		 */

		event EventHandler<LogExportedEventArgs> ExportStarted;
		event EventHandler<LogExportedEventArgs> ExportCompleted;

		bool mIsExporting { get; }

		IEnumerable<string> GetDirectoryPaths();
		void AddDirectoryPaths(IEnumerable<string> DirectoryPaths);
		void ClearDirectoryPaths();
		IEnumerable<string> GetFilePaths();
		void AddFilePaths(IEnumerable<string> FilePaths);
		void ClearFilePaths();
		IEnumerable<string> GetTotalItemPaths();
		void StartExport();
	}

	public class LogExportedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string DirectoryPath { get; private set; }
		public IEnumerable<string> Items { get; private set; }

		public LogExportedEventArgs(DateTime OccurTime, string DirectoryPath, IEnumerable<string> Items) : base()
		{
			this.OccurTime = OccurTime;
			this.DirectoryPath = DirectoryPath;
			this.Items = Items;
		}
	}
}
