using GLCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Map
{
	public class MapManagerUpdater : SystemWithConfig, IMapManagerUpdater
	{
		public event EventHandler<LoadMapSuccessedEventArgs> LoadMapSuccessed;
		public event EventHandler<LoadMapFailedEventArgs> LoadMapFailed;
		public event EventHandler<SynchronizeMapStartedEventArgs> SynchronizeMapStarted;

		private IMapManager rMapManager = null;
		private IMapFileManager rMapFileManager = null;
		private IMapFileManagerUpdater rMapFileManagerUpdater = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private bool mAutoLoadMap = false;

		public MapManagerUpdater(IMapManager MapManager, IMapFileManager MapFileManager, IMapFileManagerUpdater MapFileManagerUpdater, IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(MapManager, MapFileManager, MapFileManagerUpdater, VehicleCommunicator, VehicleInfoManager);
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public void Set(IMapFileManager MapFileManager)
		{
			UnsubscribeEvent_IMapFileManager(rMapFileManager);
			rMapFileManager = MapFileManager;
			SubscribeEvent_IMapFileManager(rMapFileManager);
		}
		public void Set(IMapFileManagerUpdater MapFileManagerUpdater)
		{
			UnsubscribeEvent_IMapFileManagerUpdater(rMapFileManagerUpdater);
			rMapFileManagerUpdater = MapFileManagerUpdater;
			SubscribeEvent_IMapFileManagerUpdater(rMapFileManagerUpdater);
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
		public void Set(IMapManager MapManager, IMapFileManager MapFileManager, IMapFileManagerUpdater MapFileManagerUpdater, IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(MapManager);
			Set(MapFileManager);
			Set(MapFileManagerUpdater);
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
		}
		public void LoadMap(string MapFileName)
		{
			GLCMD.CMD.LoadMap(rMapFileManager.GetMapFileFullPath(MapFileName), 3);
			rMapManager.SetMapData(MapFileName, MD5HashCalculator.CalculateFileHash(rMapFileManager.GetMapFileFullPath(MapFileName)), GLCMD.CMD.SingleTowerPairInfo.Select(o => ConvertToIMapObjectOfTowardPoint(o)).ToList(), GLCMD.CMD.SingleAreaInfo.Select(o => ConvertToIMapObjectOfRectangle(o)).ToList());
			RaiseEvent_LoadMapSuccessed(MapFileName);
		}
		public void LoadMap2(string MapFileNameWithoutExtension)
		{
			LoadMap(MapFileNameWithoutExtension + ".map");
		}
		public void SynchronizeMapToOnlineVehicles(string MapFileName)
		{
			IEnumerable<string> vehicleNames = rVehicleInfoManager.GetItemNames();
			if (vehicleNames != null && vehicleNames.Count() > 0)
			{
				foreach (string vehicleName in vehicleNames)
				{
					rVehicleCommunicator.SendDataOfUploadMapToAGV(rVehicleInfoManager.GetItem(vehicleName).mIpPort, MapFileName);
				}
				foreach (string vehicleName in vehicleNames)
				{
					rVehicleCommunicator.SendDataOfChangeMap(rVehicleInfoManager.GetItem(vehicleName).mIpPort, MapFileName);
				}
				RaiseEvent_SynchronizeMapStarted(MapFileName, vehicleNames);
			}
		}
		public void SynchronizeMapToOnlineVehicles2(string MapFileNameWithoutExtension)
		{
			SynchronizeMapToOnlineVehicles(MapFileNameWithoutExtension + ".map");
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "AutoLoadMap" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "AutoLoadMap":
					return mAutoLoadMap.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "AutoLoadMap":
					mAutoLoadMap = bool.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}

		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				// do nothing ...
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				// do nothing ...
			}
		}
		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				// do nothing ...
			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				// do nothing ...
			}
		}
		private void SubscribeEvent_IMapFileManagerUpdater(IMapFileManagerUpdater MapFileManagerUpdater)
		{
			if (MapFileManagerUpdater != null)
			{
				// do nothing ...
			}
		}
		private void UnsubscribeEvent_IMapFileManagerUpdater(IMapFileManagerUpdater MapFileManagerUpdater)
		{
			if (MapFileManagerUpdater != null)
			{
				// do nothing ...
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				// do nothing ...
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				// do nothing ...
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		protected virtual void RaiseEvent_LoadMapSuccessed(string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				LoadMapSuccessed?.Invoke(this, new LoadMapSuccessedEventArgs(DateTime.Now, MapFileName));
			}
			else
			{
				Task.Run(() => { LoadMapSuccessed?.Invoke(this, new LoadMapSuccessedEventArgs(DateTime.Now, MapFileName)); });
			}
		}
		protected virtual void RaiseEvent_LoadMapFailed(string MapFileName, ReasonOfLoadMapFail Reason, bool Sync = true)
		{
			if (Sync)
			{
				LoadMapFailed?.Invoke(this, new LoadMapFailedEventArgs(DateTime.Now, MapFileName, Reason));
			}
			else
			{
				Task.Run(() => { LoadMapFailed?.Invoke(this, new LoadMapFailedEventArgs(DateTime.Now, MapFileName, Reason)); });
			}
		}
		protected virtual void RaiseEvent_SynchronizeMapStarted(string MapFileName, IEnumerable<string> VehicleNames, bool Sync = true)
		{
			if (Sync)
			{
				SynchronizeMapStarted?.Invoke(this, new SynchronizeMapStartedEventArgs(DateTime.Now, MapFileName, VehicleNames));
			}
			else
			{
				Task.Run(() => { SynchronizeMapStarted?.Invoke(this, new SynchronizeMapStartedEventArgs(DateTime.Now, MapFileName, VehicleNames)); });
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			// 當車的「當前使用地圖」屬性改變時 (僅使用 Contains 判斷有可能抓到 "CurrentMapName" 與 "CurrentMapNameList" 兩種結果，所以需要做更細節的字串判斷)
			if (Args.StatusName.Contains("CurrentMapName") && Args.StatusName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Any(o => o == "CurrentMapName"))
			{
				// 當啟用「自動讀取地圖」功能時，且當前使用地圖不為空或 null ，將嘗試讀取地圖
				if (mAutoLoadMap && !string.IsNullOrEmpty(Args.Item.mCurrentMapName))
				{
					DateTime tmpTimestamp = DateTime.Now;
					Task.Run(() => TryLoadMap(Args.Item.mCurrentMapName));
				}
			}
		}
		private void TryLoadMap(string MapFileName)
		{
			bool isLoadMapSuccess = false;
			ReasonOfLoadMapFail reason = ReasonOfLoadMapFail.None;

			DateTime tmpTimestamp = DateTime.Now;
			while (DateTime.Now.Subtract(tmpTimestamp).TotalMilliseconds < 5000)
			{
				// 當下載地圖完成時
				if (!rMapFileManagerUpdater.mIsDownloadingMap)
				{
					// 若有該地圖存在
					if (rMapFileManager.GetLocalMapFileNameList().Any(o => o == MapFileName))
					{
						// 該地圖 Hash 與當前地圖 Hash 不同時
						if (MD5HashCalculator.CalculateFileHash(rMapFileManager.GetMapFileFullPath(MapFileName)) != rMapManager.mCurrentMapFileHash)
						{
							LoadMap(MapFileName);
							isLoadMapSuccess = true;
							break;
						}
						else
						{
							reason = ReasonOfLoadMapFail.MapFileHashIsEqualToOldMap;
							break;
						}
					}
					else
					{
						reason = ReasonOfLoadMapFail.MapFileIsNotExist;
						break;
					}
				}
				Thread.Sleep(600);
			}

			if (!isLoadMapSuccess)
			{
				if (reason == ReasonOfLoadMapFail.None) reason = ReasonOfLoadMapFail.IsDownloadingMapFile;
				RaiseEvent_LoadMapFailed(MapFileName, reason);
			}
		}

		private static IMapObjectOfTowardPoint ConvertToIMapObjectOfTowardPoint(ISingleTowardPairInfo Input)
		{
			IMapObjectOfTowardPoint result = null;
			if (Input != null)
			{
				TypeOfMapObjectOfTowardPoint type = TypeOfMapObjectOfTowardPoint.Normal;
				switch (Input.StyleName)
				{
					case "ChargingDocking":
						type = TypeOfMapObjectOfTowardPoint.Charge;
						break;
					default:
						type = TypeOfMapObjectOfTowardPoint.Normal;
						break;
				}
				result = new MapObjectOfTowardPoint(Input.Name, Input.X, Input.Y, Input.Toward, type, Input.Parameters.ToArray());
			}
			return result;
		}
		private static IMapObjectOfRectangle ConvertToIMapObjectOfRectangle(ISingleAreaInfo Input)
		{
			IMapObjectOfRectangle result = null;
			if (Input != null)
			{
				TypeOfMapObjectOfRectangle type = TypeOfMapObjectOfRectangle.None;
				switch (Input.StyleName)
				{
					case "ForbiddenArea":
						type = TypeOfMapObjectOfRectangle.Foribdden;
						break;
					case "OneWayArea":
						type = TypeOfMapObjectOfRectangle.Oneway;
						break;
					case "PathPlanningArea":
						type = TypeOfMapObjectOfRectangle.PathPlanning;
						break;
					case "AutoDoorArea":
						type = TypeOfMapObjectOfRectangle.AutomaticDoor;
						break;
					default:
						type = TypeOfMapObjectOfRectangle.None;
						break;
				}
				result = new MapObjectOfRectangle(Input.Name, Input.MaxX, Input.MaxY, Input.MinX, Input.MinY, type, Input.Parameters.ToArray());
			}
			return result;
		}
	}
}
