using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Map
{
	/// <summary>區域的設定/資訊</summary>
	public class MapRegionSetting
	{
		/// <summary>區域識別碼。自動產生，不可重複</summary>
		public int mRegionId { get; set; } = -1;
		/// <summary>區域名稱。主要用於顯示用，可自訂</summary>
		public string mRegionName { get; set; } = string.Empty;
		/// <summary>區域成員。格式為： VehicleName1,VehicleName2,VehicleName3</summary>
		public string mRegionMember { get; set; } = string.Empty;
		/// <summary>當前地圖檔案名稱。包含副檔名</summary>
		public string mCurrentMapName { get; set; } = string.Empty;
		/// <summary>當前地圖範圍。格式為： (MaxX,MaxY),(MinX,MinY)</summary>
		public string mCurrentMapRange { get; set; } = string.Empty;

		public MapRegionSetting()
		{

		}
		public void Set(int RegionId, string RegionName, string RegionMember, string CurrentMapName, string CurrentMapRange)
		{
			mRegionId = RegionId;
			mRegionName = RegionName;
			mRegionMember = RegionMember;
			mCurrentMapName = CurrentMapName;
			mCurrentMapRange = CurrentMapRange;
		}
		public void SetRegionName(string RegionName)
		{
			mRegionName = RegionName;
		}
		public void SetRegionMember(string RegionMember)
		{
			mRegionMember = RegionMember;
		}
		public void SetCurrentMap(string CurrentMapName, string CurrentMapRange)
		{
			mCurrentMapName = CurrentMapName;
			mCurrentMapRange = CurrentMapRange;
		}
		public string[] GetRegionMember()
		{
			if (!string.IsNullOrEmpty(mRegionMember))
			{
				return mRegionMember.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			}
			else
			{
				return new string[] { };
			}
		}
		public IRectangle2D GetCurrentMapRange()
		{
			if (!string.IsNullOrEmpty(mCurrentMapRange) && mCurrentMapRange.Contains(",") && mCurrentMapRange.Contains("(") && mCurrentMapRange.Contains(")"))
			{
				string[] tmpData = mCurrentMapRange.Split(new string[] { ",", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
				return new Rectangle2D(new Point2D(int.Parse(tmpData[0]), int.Parse(tmpData[1])), new Point2D(int.Parse(tmpData[2]), int.Parse(tmpData[3])));
			}
			else
			{
				return null;
			}
		}
	}
}
