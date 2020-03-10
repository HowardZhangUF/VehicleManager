using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Interface
{
	/// <summary>
	/// - Reference: IVehicleInfoManager
	/// - 載入地圖後會發出事件
	/// - 提供設定「自動載入地圖」的開關
	/// - 提供取得當前地圖檔名稱(不含副檔名)的功能
	/// - 提供載入地圖的功能，輸入參數為地圖檔名稱(不含副檔名)
	/// - 提供取得地圖物件清單的功能
	/// - 當 VehicleInfo 的 CurrentMapName 改變且與當前地圖不一樣時，自動載入新地圖
	/// - (?)當收到 「Vehicle 當前地圖已更換」的訊息時，自動載入新地圖，並將新地圖同步至所有車
	/// </summary>
	public interface IMapManager
	{
		event EventHandlerMapFileName MapLoaded;

		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMapFileManager MapFileManager);
		void Set(IVehicleInfoManager VehicleInfoManager, IMapFileManager MapFileManager);
		void SetConfigOfAutoLoadMap(bool Enable);
		bool GetConfigOfAutoLoadMap();
		void LoadMap(string MapFileName);
		string GetCurrentMapName();
		string[] GetGoalNameList();
		int[] GetGoalCoordinate(string GoalName);
	}
}
