using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.Map
{
	/// <summary>地圖區域的設定/資訊，一張地圖會切分成多塊區域</summary>
	/// <remarks>
	/// 將地圖切分成多個區域，每個區域有編號、放置地圖用的資料夾。
	/// 第 0 個區域固定存在，沒有被分區的自走車，皆會歸屬於第 0 個區域。
	/// </remarks>
	public class MapManagementSetting
	{
		public string mMapFileDirectory { get; set; } = ".\\..\\VehicleManagerData\\Map";
		public Dictionary<int, MapRegionSetting> mRegionSettings = new Dictionary<int, MapRegionSetting>();

		public MapManagementSetting()
		{

		}
		/// <summary>取得指定自走車所在的區域的地圖資料夾路徑</summary>
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
		/// <summary>取得指定區域的地圖資料夾路徑</summary>
		public string GetMapDirectoryPath(int RegionId)
		{
			// RegionId 轉換成路徑時，固定為長度為三的字串，預設 Id 範圍為 000 ~ 999
			if (RegionId < 0)
			{
				return $"{mMapFileDirectory}\\000\\";
			}
			else
			{
				return $"{mMapFileDirectory}\\{RegionId.ToString().PadLeft(3, '0')}\\";
			}
		}
		/// <summary>取得指定自走車所在的區域的編號</summary>
		public int GetCorrespondingMapRegionId(string VehicleName)
		{
			return GetCorrespondingMapRegionSetting(VehicleName).mRegionId;
		}
		/// <summary>取得與指定自走車在同一個區域的自走車的名稱集合</summary>
		/// <remarks>
		/// 如果同事數量為 0 ，代表該自走車屬於第 0 個區域，代表所有未分區的車皆是該車的同事
		/// 備註：有分區的時候，至少會有一個同事(自己)
		/// </remarks>
		public string[] GetCoworkerNames(string VehicleName)
		{
			return GetCorrespondingMapRegionSetting(VehicleName).GetRegionMember();
		}
		/// <summary>取得指定自走車所的的區域的設定/資訊</summary>
		/// <remarks>第 0 個區域固定存在，沒有被分區的自走車，皆會歸屬於第 0 個區域</remarks>
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
			if (result == null)
			{
				result = mRegionSettings[0];
			}
			return result;
		}
		/// <summary>取得每個區域的當前地圖名稱集合</summary>
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
		/// <summary>取得有被分區的自走車的名稱集合</summary>
		public string[] GetBeAssignedVehicleNames()
		{
			List<string> result = new List<string>();
			foreach (var setting in mRegionSettings.Values)
			{
				result.AddRange(setting.GetRegionMember());
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
