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
	///	- Reference: IVehicleCommunicator, IVehicleInfoManager
	/// - 提供設定地圖檔資料夾路徑的功能。範例： .\\Map\\
	/// - 提供取得本地地圖檔清單的功能。取得的清單僅有檔案名稱(不含副檔名)，並非完整路徑
	/// - 提供取得地圖檔完整路徑的功能。輸入範例： Test ，輸出範例： .\\Map\\Test.map
	/// - 提供新增、移除地圖檔的功能。參數僅需輸入檔案名稱(含副檔名)，範例： Test.map
	/// - 提供同步所有車的當前使用地圖的功能。輸入範例： Test.map
	/// - 當新增、移除地圖檔時會拋出事件
	/// - 當收到地圖時，會將其儲存至本地
	/// - 當上傳地圖成功後，會發送詢問當前地圖資訊的要求以更新資訊
	/// - 當變更地圖成功後，會發送詢問當前地圖資訊的要求以更新資訊
	/// - 車子連線時，會發送詢問當前地圖資訊的要求
	/// - 當車子當前使用地圖資訊改變時，會向車子要求當前地圖檔
	/// </summary>
	public interface IMapFileManager
	{
		event EventHandlerMapFileName MapFileAdded;
		event EventHandlerMapFileName MapFileRemoved;
		event EventHandlerVehicleNamesMapFileName VehicleCurrentMapSynchronized;

		bool mIsGettingMap { get; }
		IList<string> mMapsOfGetting { get; }

		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager);
		void SetConfigOfMapFileDirectory(string MapFileDirectory);
		string GetConfigOfMapFileDirectory();
		string[] GetLocalMapNameList();
		string GetMapFileFullPath(string MapFileName);
		void AddMapFile(string MapFileName, byte[] MapData);
		void RemoveMapFile(string MapFileName);
		void SynchronizeVehicleCurrentMap(string MapFileName);
	}
}
