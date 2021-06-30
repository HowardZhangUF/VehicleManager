using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Map
{
	public class MapFileManager : SystemWithConfig, IMapFileManager
	{
		public event EventHandler<MapFileCountChangedEventArgs> MapFileAdded;

		private MapManagementSetting mMapManagementSetting { get; set; } = null;

		public MapFileManager()
		{
		}
		public string[] GetLocalMapFileFullPathList()
		{
			string[] result = null;
			if (Directory.Exists(mMapManagementSetting.mMapFileDirectory))
			{
				result = Directory.GetFiles(mMapManagementSetting.mMapFileDirectory, "*.map", SearchOption.AllDirectories);
			}
			return result;
		}
		public void AddMapFile(string SrcVehicleName, string MapFileName, byte[] MapData)
		{
			// 每當下載地圖時，更新地圖管理設定(會下載地圖，代表有自走車連線，或是有自走車更新地圖)
			// 更新後通知其他類別一起將 MapManageSetting 更新???
			int regionId = mMapManagementSetting.GetCorrespondingMapRegionId(SrcVehicleName);
			mMapManagementSetting.mRegionSettings[regionId].SetCurrentMap(MapFileName, string.Empty);
			RaiseEvent_ConfigUpdated("MapManagementSetting", mMapManagementSetting.ToJsonString());

			// 儲存地圖
			string mapDirectory = mMapManagementSetting.GetMapDirectoryPath(regionId);
			if (!Directory.Exists(mapDirectory)) Directory.CreateDirectory(mapDirectory);
			string mapFullPath = Path.Combine(mapDirectory, MapFileName);
			File.WriteAllBytes(mapFullPath, MapData);
			RaiseEvent_MapFileAdded(mapFullPath);
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "MapManagementSetting" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "MapManagementSetting":
					return mMapManagementSetting == null ? string.Empty : mMapManagementSetting.ToJsonString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "MapManagementSetting":
					mMapManagementSetting = MapManagementSetting.FromJsonString(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}

		protected virtual void RaiseEvent_MapFileAdded(string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapFileAdded?.Invoke(this, new MapFileCountChangedEventArgs(DateTime.Now, MapFileName));
			}
			else
			{
				Task.Run(() => { MapFileAdded?.Invoke(this, new MapFileCountChangedEventArgs(DateTime.Now, MapFileName)); });
			}
		}
	}
}
