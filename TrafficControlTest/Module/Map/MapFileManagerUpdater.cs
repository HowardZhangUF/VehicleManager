using LibraryForVM;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.NewCommunication;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Map
{
	public class MapFileManagerUpdater : IMapFileManagerUpdater
	{
		public bool mIsDownloadingMap { get { return mMapFileNamesOfDownloading.Any(); } }

		private List<string> mMapFileNamesOfDownloading { get; } = new List<string>();

		private IMapFileManager rMapFileManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;

		public MapFileManagerUpdater(IMapFileManager MapFileManager, IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(MapFileManager, VehicleCommunicator, VehicleInfoManager);
		}
		public void Set(IMapFileManager MapFileManager)
		{
			UnsubscribeEvent_IMapFileManager(rMapFileManager);
			rMapFileManager = MapFileManager;
			SubscribeEvent_IMapFileManager(rMapFileManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IMapFileManager MapFileManager, IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(MapFileManager);
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
		}

		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SentDataFailed += HandleEvent_VehicleCommunicatorSentDataFailed;
				VehicleCommunicator.ReceivedData += HandleEvent_VehicleCommunicatorReceivedData;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SentDataFailed -= HandleEvent_VehicleCommunicatorSentDataFailed;
				VehicleCommunicator.ReceivedData -= HandleEvent_VehicleCommunicatorReceivedData;
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void HandleEvent_VehicleCommunicatorSentDataFailed(object Sender, SentDataEventArgs Args)
		{
			if (Args.Data is Serializable)
			{
				// 當傳送「下載地圖」失敗時，代表「下載地圖」失敗。重新整理「下載中地圖清單」
				if (Args.Data is GetMap)
				{
					string mapFileName = (Args.Data as GetMap).Require + ".map";
					if (mMapFileNamesOfDownloading.Contains(mapFileName))
					{
						mMapFileNamesOfDownloading.Remove(mapFileName);
					}
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorReceivedData(object Sender, ReceivedDataEventArgs Args)
		{
			if (Args.Data is Serializable)
			{
				// 當收到「下載地圖」的回覆，使用 IMapFileManager 將其儲存，並更新「下載中地圖清單」
				if (Args.Data is GetMap)
				{
					GetMap tmpData = Args.Data as GetMap;
					IVehicleInfo vehicleInfo = rVehicleInfoManager.GetItemByIpPort(Args.IpPort);
					string vehicleName = vehicleInfo == null ? string.Empty : vehicleInfo.mName;
					rMapFileManager.AddMapFile(vehicleName, tmpData.Response.Name, tmpData.Response.Data);
					if (mMapFileNamesOfDownloading.Contains(tmpData.Response.Name))
					{
						mMapFileNamesOfDownloading.Remove(tmpData.Response.Name);
					}
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			// 當有車離線時，重新整理「下載中地圖清單」
			if (!string.IsNullOrEmpty(Args.Item.mCurrentMapName) && mMapFileNamesOfDownloading.Count > 0)
			{
				if (mMapFileNamesOfDownloading.Contains(Args.Item.mCurrentMapName))
				{
					mMapFileNamesOfDownloading.Remove(Args.Item.mCurrentMapName);
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			// 當車的「當前使用地圖」屬性改變時 (僅使用 Contains 判斷有可能抓到 "CurrentMapName" 與 "CurrentMapNameList" 兩種結果，所以需要做更細節的字串判斷)
			if (Args.StatusName.Contains("CurrentMapName") && Args.StatusName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Any(o => o == "CurrentMapName"))
			{
				// 當 CurrentMapName 不為空或 Null 時，發送「下載地圖」的請求，並更新「下載中地圖清單」
				if (!string.IsNullOrEmpty(Args.Item.mCurrentMapName))
				{
					mMapFileNamesOfDownloading.Add(Args.Item.mCurrentMapName);
					rVehicleCommunicator.SendDataOfGetMap(Args.Item.mIpPort, Args.Item.mCurrentMapName);
				}
			}
		}
	}
}
