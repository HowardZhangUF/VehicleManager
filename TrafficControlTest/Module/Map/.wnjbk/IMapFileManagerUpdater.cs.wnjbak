using System.Collections.Generic;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Map
{
	/// <summary>
	/// - Reference: IMapFileManager, IVehicleCommunicator, IVehicleInfoManager
	/// - 當 IVehicleCommunicator 傳送「下載地圖」失敗時，重新整理「下載中地圖清單」
	/// - 當 IVehicleCommunicator 收到「下載地圖」的回覆，使用 IMapFileManager 將其儲存，並更新「下載中地圖清單」
	/// - 當 IVehicleInfoManager 有車離線時，重新整理「下載中地圖清單」
	/// - 當 IVehicleInfoManager 車的「當前使用地圖」屬性改變且其不為空或 Null 時，發送「下載地圖」的請求，並更新「下載中地圖清單」
	/// </summary>
	public interface IMapFileManagerUpdater
	{
		bool mIsDownloadingMap { get; }
		List<string> mMapFileNamesOfDownloading { get; }

		void Set(IMapFileManager MapFileManager);
		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMapFileManager MapFileManager, IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager);
	}
}
