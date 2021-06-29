using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.Map
{
	public class MapManagementSetting
	{
		public string mMapFileDirectory { get; set; } = ".//Map//";
		public Dictionary<int, MapRegionSetting> mRegionSettings = new Dictionary<int, MapRegionSetting>();

		public MapManagementSetting()
		{

		}
		public string GetMapDirectoryPath(string VehicleName)
		{
			// 如果只有單地圖時，(不論單車或多車)，都將地圖放置到 RegionId 0 的資料夾裡
			if (mRegionSettings.Count == 0 || mRegionSettings.Count == 1)
			{
				// example: .//Map//000//
				return GetMapDirectoryPath(0);
			}
			// 如果有多地圖時，地圖要根據 RegionId 放置到不同的資料夾內
			// 如果指定自走車尚未分類，則將地圖放置到 RegionId 0 的資料夾裡
			else
			{
				// example: .//Map/001//
				return GetMapDirectoryPath(GetCorrespondingMapRegionId(VehicleName));
			}
		}
		public string GetMapDirectoryPath(int RegionId)
		{
			if (RegionId < 0)
			{
				return $"{mMapFileDirectory}000//";
			}
			else
			{
				return $"{mMapFileDirectory}{RegionId.ToString().PadLeft(3, '0')}//";
			}
		}
		public int GetCorrespondingMapRegionId(string VehicleName)
		{
			MapRegionSetting correspondingMapRegionSetting = GetCorrespondingMapRegionSetting(VehicleName);
			return correspondingMapRegionSetting == null ? 0 : correspondingMapRegionSetting.mRegionId;
		}
		public MapRegionSetting GetCorrespondingMapRegionSetting(string VehicleName)
		{
			MapRegionSetting result = null;
			if (mRegionSettings.Count > 0)
			{
				foreach (var setting in mRegionSettings.Values)
				{
					if (setting.GetRegionMember().Contains(VehicleName))
					{
						result = setting;
						break;
					}
				}
			}
			return result;
		}
		public string[] GetCurrentMapNames()
		{
			List<string> result = new List<string>();
			foreach (var setting in mRegionSettings.Values)
			{
				if (!string.IsNullOrEmpty(setting.mCurrentMapName))
				{
					// example: .//Map//000//abc.map
					result.Add($"{GetMapDirectoryPath(setting.mRegionId)}{setting.mCurrentMapName}");
				}
			}
			return result.ToArray();
		}
		public string ToJsonString()
		{
			return JsonConvert.SerializeObject(this);
		}
		public static MapManagementSetting FromJsonString(string JsonString)
		{
			return JsonConvert.DeserializeObject<MapManagementSetting>(JsonString);
		}
	}
}
