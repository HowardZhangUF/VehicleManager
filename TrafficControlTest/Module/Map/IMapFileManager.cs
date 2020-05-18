using System.Collections.Generic;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Map
{
	/// <summary>
	/// - 提供設定地圖檔資料夾路徑的功能。範例： .\\Map\\
	/// - 提供取得本地地圖檔清單的功能
	/// - 提供取得地圖檔完整路徑的功能
	/// - 提供新增、移除地圖檔的功能
	/// - 當新增、移除地圖檔時會拋出事件
	/// </summary>
	/// <remarks>
	/// =========================================================
	/// File Example (D:\\Map\\aaa.map)
	///		- FileName						: aaa.map
	///		- FileNamewithoutExtension		: aaa
	///		- FileExtension					: .map
	///		- FileFullPath / FileFullName	: D:\\Map\\aaa.map
	/// =========================================================
	/// Directory Example (D:\\Map)
	///		- DirectoryName								: Map
	///		- DirectoryFullPath / DirectoryFullName		: D:\\Map
	/// =========================================================
	/// </remarks>
	public interface IMapFileManager : ISystemWithConfig
	{
		event EventHandlerMapFileName MapFileAdded;
		event EventHandlerMapFileName MapFileRemoved;

		string[] GetLocalMapFileNameList();
		string[] GetLocalMapFileNameWithoutExtensionList();
		string GetMapFileFullPath(string MapFileName);
		string GetMapFileFullPath2(string MapFileNameWithoutExtension);
		void AddMapFile(string MapFileName, byte[] MapData);
		void AddMapFile2(string MapFileNameWithoutExtension, byte[] MapData);
		void RemoveMapFile(string MapFileName);
		void RemoveMapFile2(string MapFileNameWithoutExtension);
	}
}
