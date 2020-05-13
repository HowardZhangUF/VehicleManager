using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Log
{
	public class LogExporter
	{
		public delegate void EventHandlerExportLog(DateTime OccurTime, string DirectoryPath, List<string> Items);

		public event EventHandlerExportLog ExportStarted;
		public event EventHandlerExportLog ExportCompleted;

		public bool mIsExporting { get; private set; }

		public static string mBaseDirectory { get; private set; } = ".\\LogExport";

		private List<string> mDirectoryPaths { get; set; } = new List<string>();
		private List<string> mFilePaths { get; set; } = new List<string>();

		public LogExporter()
		{

		}
		public void AddDirectoryPaths(List<string> DirectoryPaths)
		{
			mDirectoryPaths.AddRange(DirectoryPaths);
		}
		public void ClearDirectoryPaths()
		{
			mDirectoryPaths.Clear();
		}
		public void AddFilePaths(List<string> FilePaths)
		{
			mFilePaths.AddRange(FilePaths);
		}
		public void ClearFilePaths()
		{
			mFilePaths.Clear();
		}
		public void StartExport()
		{
			Task.Run(() =>
			{
				string dstDirectoryFileName = $"{mBaseDirectory}\\CASTEC_VM_Log_{DateTime.Now.ToString("yyyyMMdd")}";
				List<string> items = new List<string>();
				items.AddRange(mDirectoryPaths);
				items.AddRange(mFilePaths);
				RaiseEvent_ExportStarted(dstDirectoryFileName, items);

				// Create Base Directory
				FileOperation.CreateDirectory(mBaseDirectory);

				// Create New Directory for this Export
				FileOperation.DeleteDirectory(dstDirectoryFileName);
				FileOperation.CreateDirectory(dstDirectoryFileName);

				// Copy Directories
				foreach (string dirPath in mDirectoryPaths)
				{
					FileOperation.CopyDirectoryUnderViaCommandPrompt(dirPath, dstDirectoryFileName);
				}
				// Copy Files
				foreach (string filePath in mFilePaths)
				{
					FileOperation.CopyFileViaCommandPrompt(filePath, dstDirectoryFileName);
				}

				// Compress Directory
				FileOperation.CompressDirectory(dstDirectoryFileName);

				RaiseEvent_ExportCompleted(dstDirectoryFileName, items);

				// Open Directory
				System.Diagnostics.Process.Start(mBaseDirectory);
			});
		}

		protected virtual void RaiseEvent_ExportStarted(string DirectoryPath, List<string> Items, bool Sync = true)
		{
			if (Sync)
			{
				ExportStarted?.Invoke(DateTime.Now, DirectoryPath, Items);
			}
			else
			{
				Task.Run(() => ExportStarted?.Invoke(DateTime.Now, DirectoryPath, Items));
			}
		}
		protected virtual void RaiseEvent_ExportCompleted(string DirectoryPath, List<string> Items, bool Sync = true)
		{
			if (Sync)
			{
				ExportCompleted?.Invoke(DateTime.Now, DirectoryPath, Items);
			}
			else
			{
				Task.Run(() => ExportCompleted?.Invoke(DateTime.Now, DirectoryPath, Items));
			}
		}
	}
}
