using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Log
{
	public class LogExporter : SystemWithConfig, ILogExporter
	{
		public event EventHandler<LogExportedEventArgs> ExportStarted;
		public event EventHandler<LogExportedEventArgs> ExportCompleted;

		public bool mIsExporting { get; private set; }

		private ProjectType rProjectType { get; set; } = ProjectType.Common;
		private List<string> mDirectoryPaths { get; set; } = new List<string>();
		private List<string> mFilePaths { get; set; } = new List<string>();
		private string mBaseDirectory { get; set; } = ".\\..\\VehicleManagerData\\LogExport";
		private string mExportDirectoryNamePrefix { get; set; } = "CASTEC_Log_VM_";
		private string mExportDirectoryNameTimeFormat { get; set; } = "yyyyMMdd";
		private bool mExportProjectInfo { get; set; } = true;
		private string mExportDirectoryFullPath
		{
			get
			{
				if (mExportProjectInfo)
				{
					return $"{mBaseDirectory}\\{mExportDirectoryNamePrefix}{rProjectType.ToString()}_{DateTime.Now.ToString(mExportDirectoryNameTimeFormat)}";
				}
				else
				{
					return $"{mBaseDirectory}\\{mExportDirectoryNamePrefix}{DateTime.Now.ToString(mExportDirectoryNameTimeFormat)}";
				}
			}
		}

		public LogExporter(ProjectType ProjectType)
		{
			Set(ProjectType);
		}
		public void Set(ProjectType ProjectType)
		{
			rProjectType = ProjectType;
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

				bool Frank = false;//選擇是否要啟用 原本Frank的ExportLog 
				if (Frank)// Frank原本的資料為將所有原本在VehicleMangerData裡的資料複製到Logexport並壓縮
				{
					// Create New Directory for this Export
					FileOperation.DeleteDirectory(dstDirectoryFileName);
					FileOperation.CreateDirectory(dstDirectoryFileName);
					// Copy Directories
					FileOperation.CopyDirectoriesUnderViaCommandPrompt(mDirectoryPaths, dstDirectoryFileName);

					// Copy Files
					FileOperation.CopyFilesViaCommandPrompt(mFilePaths, dstDirectoryFileName);
				}
				else
				{
					string srcdir = $@"..\.\VehicleManagerData";
					string dstdir = dstDirectoryFileName;
					FileOperation.JeffHandleFile(srcdir, dstdir);//只複製Map跟HistoryLog.db
				}

				// Compress Directory
				FileOperation.CompressDirectory(dstDirectoryFileName);

				RaiseEvent_ExportCompleted(dstDirectoryFileName, items);
				mIsExporting = false;

				// Open Directory
				System.Diagnostics.Process.Start(mBaseDirectory);
			});
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "BaseDirectory", "ExportDirectoryNamePrefix", "ExportDirectoryNameTimeFormat", "ExportProjectInfo" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "BaseDirectory":
					return mBaseDirectory;
				case "ExportDirectoryNamePrefix":
					return mExportDirectoryNamePrefix;
				case "ExportDirectoryNameTimeFormat":
					return mExportDirectoryNameTimeFormat;
				case "ExportProjectInfo":
					return mExportProjectInfo.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "BaseDirectory":
					mBaseDirectory = NewValue;
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ExportDirectoryNamePrefix":
					mExportDirectoryNamePrefix = NewValue;
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ExportDirectoryNameTimeFormat":
					mExportDirectoryNameTimeFormat = NewValue;
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ExportProjectInfo":
					mExportProjectInfo = bool.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
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
