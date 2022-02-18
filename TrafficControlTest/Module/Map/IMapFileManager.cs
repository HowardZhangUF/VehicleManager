using LibraryForVM;
using System;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Map
{
	/// <summary>
	/// - 提供設定/取得「地圖管理設定檔」的資訊的功能
	/// - 提供取得本地地圖檔清單的功能
	/// - 提供新增地圖檔的功能
	/// - 當新增地圖檔時會拋出事件
	/// </summary>
	/// <remarks>
	/// =========================================================
	/// File Example (D:\\Map\\000\\aaa.map)
	///		- FileName		: aaa.map
	///		- FileFullPath	: D:\\Map\000\\aaa.map
	/// =========================================================
	/// </remarks>
	public interface IMapFileManager : ISystemWithConfig
	{
		event EventHandler<MapFileCountChangedEventArgs> MapFileAdded;

		string[] GetLocalMapFileFullPathList();
		void AddMapFile(string SrcVehicleName, string MapFileName, byte[] MapData);
	}

	public class MapFileCountChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string MapFileFullPath { get; private set; }

		public MapFileCountChangedEventArgs(DateTime OccurTime, string MapFileFullPath) : base()
		{
			this.OccurTime = OccurTime;
			this.MapFileFullPath = MapFileFullPath;
		}
	}
}
