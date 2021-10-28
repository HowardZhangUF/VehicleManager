using GLCore;
using MapReader;
using MD5Hash;
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
		private string mIntegratedMapFileName { get; set; } = "Integrated.map";
		private MapManagementSetting mMapManagementSetting { get; set; } = null;
		private object mLockOfLoadingMap = new object();
		private object mLockOfMergingMaps = new object();

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
		public void LoadMap(string MapFileFullPath)
		{
			lock (mLockOfLoadingMap)
			{
				GLCMD.CMD.LoadMap(MapFileFullPath, 3);
				rMapManager.SetMapData(MapFileFullPath, MD5.GetFileHash(MapFileFullPath), GLCMD.CMD.SingleTowerPairInfo.Select(o => ConvertToIMapObjectOfTowardPoint(o)).ToList(), GLCMD.CMD.SingleAreaInfo.Select(o => ConvertToIMapObjectOfRectangle(o)).ToList());
				RaiseEvent_LoadMapSuccessed(MapFileFullPath);
			}
		}
		public void SynchronizeMapToOnlineVehicles(string MapFileName)
		{
			throw new NotImplementedException();
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "AutoLoadMap", "IntegratedMapFileName", "MapManagementSetting" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "AutoLoadMap":
					return mAutoLoadMap.ToString();
				case "IntegratedMapFileName":
					return mIntegratedMapFileName;
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
				case "AutoLoadMap":
					mAutoLoadMap = bool.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "IntegratedMapFileName":
					mIntegratedMapFileName = NewValue;
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "MapManagementSetting":
					mMapManagementSetting = MapManagementSetting.FromJsonString(NewValue);
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
		protected virtual void RaiseEvent_LoadMapSuccessed(string MapFileFullPath, bool Sync = true)
		{
			if (Sync)
			{
				LoadMapSuccessed?.Invoke(this, new LoadMapSuccessedEventArgs(DateTime.Now, MapFileFullPath));
			}
			else
			{
				Task.Run(() => { LoadMapSuccessed?.Invoke(this, new LoadMapSuccessedEventArgs(DateTime.Now, MapFileFullPath)); });
			}
		}
		protected virtual void RaiseEvent_LoadMapFailed(string MapFileFullPath, ReasonOfLoadMapFail Reason, bool Sync = true)
		{
			if (Sync)
			{
				LoadMapFailed?.Invoke(this, new LoadMapFailedEventArgs(DateTime.Now, MapFileFullPath, Reason));
			}
			else
			{
				Task.Run(() => { LoadMapFailed?.Invoke(this, new LoadMapFailedEventArgs(DateTime.Now, MapFileFullPath, Reason)); });
			}
		}
		protected virtual void RaiseEvent_SynchronizeMapStarted(string MapFileFullPath, IEnumerable<string> VehicleNames, bool Sync = true)
		{
			if (Sync)
			{
				SynchronizeMapStarted?.Invoke(this, new SynchronizeMapStartedEventArgs(DateTime.Now, MapFileFullPath, VehicleNames));
			}
			else
			{
				Task.Run(() => { SynchronizeMapStarted?.Invoke(this, new SynchronizeMapStartedEventArgs(DateTime.Now, MapFileFullPath, VehicleNames)); });
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
					Task.Run(() => TryMergeAndLoadMap());
				}
			}
		}
		private void TryMergeAndLoadMap()
		{
			try
			{
				bool isLoadMapSuccess = false;
				ReasonOfLoadMapFail reason = ReasonOfLoadMapFail.None;
				DateTime tmpTimestamp = DateTime.Now;
				string integratedMapFileFullPath = System.IO.Path.Combine(mMapManagementSetting.mMapFileDirectory, mIntegratedMapFileName);

				while (DateTime.Now.Subtract(tmpTimestamp).TotalMilliseconds < 5000)
				{
					// 當下載地圖完成時
					if (!rMapFileManagerUpdater.mIsDownloadingMap)
					{
						// 組合地圖
						MergeMaps(mMapManagementSetting.GetCurrentMapNames(), integratedMapFileFullPath);

						// 若有該地圖存在
						if (rMapFileManager.GetLocalMapFileFullPathList().Any(o => o == integratedMapFileFullPath))
						{
							// 該地圖 Hash 與當前地圖 Hash 不同時
							if (MD5.GetFileHash(integratedMapFileFullPath) != rMapManager.mCurrentMapFileHash)
							{
								LoadMap(integratedMapFileFullPath);
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
					RaiseEvent_LoadMapFailed(integratedMapFileFullPath, reason);
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
		private void MergeMaps(string[] MapFileFullPaths, string ItegratedMapFileFullPath)
		{
			if (MapFileFullPaths == null || MapFileFullPaths.Length == 0) return;

			BaseMapReader result = null;
			lock (mLockOfMergingMaps)
			{
				result = new Reader(MapFileFullPaths[0]);
				for (int i = 1; i < MapFileFullPaths.Length; ++i)
				{
					BaseMapReader tmpMap = new Reader(MapFileFullPaths[i]);
					result = result + tmpMap;
				}
				result.Save(ItegratedMapFileFullPath);
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
