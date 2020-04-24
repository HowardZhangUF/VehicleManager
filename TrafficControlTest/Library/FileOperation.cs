using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Library
{
	public static class FileOperation
	{
		public static void CreateDirectory(string DirectoryPath)
		{
			if (!Directory.Exists(DirectoryPath))
			{
				Directory.CreateDirectory(DirectoryPath);
			}
		}
		public static void DeleteDirectory(string DirectoryPath)
		{
			if (Directory.Exists(DirectoryPath))
			{
				Directory.Delete(DirectoryPath, true);
			}
		}
		public static void CopyFile(string SrcFilePath, string DstDirectoryPath)
		{
			if (File.Exists(SrcFilePath))
			{
				FileInfo srcFileInfo = new FileInfo(SrcFilePath);
				DirectoryInfo dstDirectoryInfo = new DirectoryInfo(DstDirectoryPath);

				CopyFile(srcFileInfo, dstDirectoryInfo);
			}
		}
		public static void CopyFile(FileInfo SrcFileInfo, DirectoryInfo DstDirectoryInfo)
		{
			Directory.CreateDirectory(DstDirectoryInfo.FullName);

			SrcFileInfo.CopyTo(Path.Combine(DstDirectoryInfo.FullName, SrcFileInfo.Name), true);
		}
		public static void CopyFileViaCommandPrompt(string SrcFilePath, string DstDirectoryPath)
		{
			if (File.Exists(SrcFilePath) && Directory.Exists(DstDirectoryPath))
			{
				string fullCmd = $"COPY \"{SrcFilePath}\" \"{DstDirectoryPath}\"";
				ShellCommandExecutor.ExecuteInCommadPrompt(fullCmd);
			}
		}
		public static void CopyAll(string SrcDirectoryPath, string DstDirectoryPath)
		{
			if (Directory.Exists(SrcDirectoryPath))
			{
				DirectoryInfo srcDirectoryInfo = new DirectoryInfo(SrcDirectoryPath);
				DirectoryInfo DstDirectoryInfo = new DirectoryInfo(DstDirectoryPath);

				CopyAll(srcDirectoryInfo, DstDirectoryInfo);
			}
		}
		public static void CopyAll(DirectoryInfo SrcDirectoryInfo, DirectoryInfo DstDirectoryInfo)
		{
			// Reference: https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo?redirectedfrom=MSDN&view=netframework-4.8

			Directory.CreateDirectory(DstDirectoryInfo.FullName);

			// Copy each file into the new directory
			foreach (FileInfo fileInfo in SrcDirectoryInfo.GetFiles())
			{
				//Console.WriteLine(@"Copying {0}\{1}", DstDirectoryInfo.FullName, fileInfo.Name);
				fileInfo.CopyTo(Path.Combine(DstDirectoryInfo.FullName, fileInfo.Name), true);
			}

			// Copy each subdirectory using recursion
			foreach (DirectoryInfo srcSubDirectoryInfo in SrcDirectoryInfo.GetDirectories())
			{
				DirectoryInfo dstSubDirectoryInfo = DstDirectoryInfo.CreateSubdirectory(srcSubDirectoryInfo.Name);
				CopyAll(srcSubDirectoryInfo, dstSubDirectoryInfo);
			}
		}
		public static void CopyAllViaCommandPrompt(string SrcDirectoryPath, string DstDirectoryPath)
		{
			if (Directory.Exists(SrcDirectoryPath))
			{
				DeleteFile(DstDirectoryPath);
				string fullCmd = $"XCOPY \"{SrcDirectoryPath}\" \"{DstDirectoryPath}\" /I /S /E";
				ShellCommandExecutor.ExecuteInCommadPrompt(fullCmd);
			}
		}
		public static void CopyAllUnder(string SrcDirectoryPath, string DstDirectoryPath)
		{
			if (Directory.Exists(SrcDirectoryPath))
			{
				DirectoryInfo srcDirectoryInfo = new DirectoryInfo(SrcDirectoryPath);
				DirectoryInfo DstDirectoryInfo = new DirectoryInfo(Path.Combine(DstDirectoryPath, srcDirectoryInfo.Name));

				CopyAll(srcDirectoryInfo, DstDirectoryInfo);
			}
		}
		public static void CopyAllUnderViaCommandPrompt(string SrcDirectoryPath, string DstDirectoryPath)
		{
			if (Directory.Exists(SrcDirectoryPath))
			{
				DirectoryInfo srcDirectoryInfo = new DirectoryInfo(SrcDirectoryPath);
				DirectoryInfo dstDirectoryInfo = new DirectoryInfo(Path.Combine(DstDirectoryPath, srcDirectoryInfo.Name));

				CopyAllViaCommandPrompt(srcDirectoryInfo.FullName, dstDirectoryInfo.FullName);
			}
		}
		public static void DeleteFile(string FilePath)
		{
			if (File.Exists(FilePath))
			{
				File.Delete(FilePath);
			}
		}
		public static void CompressDirectory(string DirectoryPath)
		{
			if (Directory.Exists(DirectoryPath))
			{
				string pathOf7z = "C:\\Program Files\\7-Zip\\7z.exe";
				string compressedFilePath = $"{DirectoryPath}.7z";
				string fullCmd = $"\"{pathOf7z}\" a {compressedFilePath} {DirectoryPath} -p27635744 -mhe";
				if (File.Exists(pathOf7z))
				{
					DeleteFile(compressedFilePath);
					ShellCommandExecutor.ExecuteInCommadPrompt(fullCmd);
				}
			}
		}
	}
}
