using LibraryForVM;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Module.Configure;

namespace TrafficControlTest.Module.Map
{
	public class MapFileManager : SystemWithConfig, IMapFileManager
	{
		public IConfigurator rConfigurator;
		public event EventHandler<MapFileCountChangedEventArgs> MapFileAdded;

		private MapManagementSetting mMapManagementSetting { get; set; } = null;

		public MapFileManager(IConfigurator Configurator)
		{
			rConfigurator = Configurator;
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
			// MapManagementSetting 更新後，在 process 會將所有用到 MapManagementSetting 的類別進行資料同步
			IRectangle2D mapRange = GetMapRange(MapData);
			int regionId = mMapManagementSetting.GetCorrespondingMapRegionId(SrcVehicleName);
			//若000啟動設定為false時將不會更新地圖(預設為false)
			if (regionId == 0 && !bool.Parse(rConfigurator.GetValue("MapManagerUpdater/Region000Activate")))
				return;
				
			mMapManagementSetting.mRegionSettings[regionId].SetCurrentMap(MapFileName, mapRange.ToString());
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

		private static IRectangle2D GetMapRange(byte[] MapData)
		{
			IRectangle2D result = null;

			if (MapData != null)
			{
				string mapDataString = System.Text.Encoding.UTF8.GetString(MapData);
				if (!string.IsNullOrEmpty(mapDataString))
				{
					string[] mapDataLines = mapDataString.Split(new string[] { "\r\n" }, StringSplitOptions.None);
					if (mapDataLines != null && mapDataLines.Length > 0)
					{
						MapReader.Reader reader = new MapReader.Reader();
						reader.Read(mapDataLines);
						int maxX = reader.MaximumPosition.X;
						int maxY = reader.MaximumPosition.Y;
						int minX = reader.MinimumPosition.X;
						int minY = reader.MinimumPosition.Y;
						result = new Rectangle2D(new Point2D(maxX, maxY), new Point2D(minX, minY));
					}
				}
			}

			return result;
		}
	}
}
