using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Log
{
	public class LogExporter : ILogExporter
	{
		public event EventHandler<LogExportedEventArgs> ExportStarted;
		public event EventHandler<LogExportedEventArgs> ExportCompleted;

		public bool mIsExporting { get; private set; }

		private List<string> mDirectoryPaths { get; set; } = new List<string>();
		private List<string> mFilePaths { get; set; } = new List<string>();
		private string mBaseDirectory { get; set; } = ".\\LogExport";
		private string mDirectoryNamePrefix { get; set; } = "CASTEC_Log_VM_";
		private string mDirectoryNameTimeFormat { get; set; } = "yyyyMMdd";
		private string mExportDirectoryFullPath { get { return $"{mBaseDirectory}\\{mDirectoryNamePrefix}{DateTime.Now.ToString(mDirectoryNameTimeFormat)}"; } }

		public LogExporter()
		{

		}
		public IEnumerable<string> GetDirectoryPaths()
		{
			return mDirectoryPaths;
		}
		public void AddDirectoryPaths(IEnumerable<string> DirectoryPaths)
		{
			foreach (string dirPath in DirectoryPaths)
			{
				if (!mDirectoryPaths.Contains(dirPath))
				{
					mDirectoryPaths.Add(dirPath);
				}
			}
		}
		public void ClearDirectoryPaths()
		{
			mDirectoryPaths.Clear();
		}
		public IEnumerable<string> GetFilePaths()
		{
			return mFilePaths;
		}
		public void AddFilePaths(IEnumerable<string> FilePaths)
		{
			foreach (string filePath in FilePaths)
			{
				if (!mFilePaths.Contains(filePath))
				{
					mFilePaths.Add(filePath);
				}
			}
		}
		public void ClearFilePaths()
		{
			mFilePaths.Clear();
		}
		public IEnumerable<string> GetTotalItemPaths()
		{
			List<string> result = new List<string>();
			result.AddRange(mDirectoryPaths);
			result.AddRange(mFilePaths);
			return result;
		}
		public void StartExport()
		{
			Task.Run(() =>
			{
				string dstDirectoryFileName = mExportDirectoryFullPath;
				IEnumerable<string> items = GetTotalItemPaths();

				mIsExporting = true;
				RaiseEvent_ExportStarted(dstDirectoryFileName, items);

				// Create Base Directory
				FileOperation.CreateDirectory(mBaseDirectory);

				// Create New Directory for this Export
				FileOperation.DeleteDirectory(dstDirectoryFileName);
				FileOperation.CreateDirectory(dstDirectoryFileName);

				// Copy Directories
				FileOperation.CopyDirectoriesUnderViaCommandPrompt(mDirectoryPaths, dstDirectoryFileName);

				// Copy Files
				FileOperation.CopyFilesViaCommandPrompt(mFilePaths, dstDirectoryFileName);

				// Compress Directory
				FileOperation.CompressDirectory(dstDirectoryFileName);

				RaiseEvent_ExportCompleted(dstDirectoryFileName, items);
				mIsExporting = false;

				// Open Directory
				System.Diagnostics.Process.Start(mBaseDirectory);
			});
		}

		protected virtual void RaiseEvent_ExportStarted(string DirectoryPath, IEnumerable<string> Items, bool Sync = true)
		{
			if (Sync)
			{
				ExportStarted?.Invoke(this, new LogExportedEventArgs(DateTime.Now, DirectoryPath, Items));
			}
			else
			{
				Task.Run(() => { ExportStarted?.Invoke(this, new LogExportedEventArgs(DateTime.Now, DirectoryPath, Items)); });
			}
		}
		protected virtual void RaiseEvent_ExportCompleted(string DirectoryPath, IEnumerable<string> Items, bool Sync = true)
		{
			if (Sync)
			{
				ExportCompleted?.Invoke(this, new LogExportedEventArgs(DateTime.Now, DirectoryPath, Items));
			}
			else
			{
				Task.Run(() => { ExportCompleted?.Invoke(this, new LogExportedEventArgs(DateTime.Now, DirectoryPath, Items)); });
			}
		}
	}
}
