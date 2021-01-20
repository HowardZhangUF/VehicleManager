using GLCore;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.NewCommunication;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Map
{
	public class MapManager : SystemWithConfig, IMapManager
	{
		public event EventHandler<LoadMapSuccessedEventArgs> LoadMapSuccessed;
		public event EventHandler<LoadMapFailedEventArgs> LoadMapFailed;
		public event EventHandler<SynchronizeMapStartedEventArgs> SynchronizeMapStarted;

		public string mCurrentMapFileName { get; private set; } = string.Empty;
		public string mCurrentMapFileNameWithoutExtension { get { return mCurrentMapFileName.Substring(0, mCurrentMapFileName.LastIndexOf('.')); } }
		public string mCurrentMapFileHash { get; private set; } = string.Empty;
		public List<IMapObjectOfTowardPoint> mTowardPointMapObjects { get; private set; } = new List<IMapObjectOfTowardPoint>();
		public List<IMapObjectOfRectangle> mRectangleMapObjects { get; private set; } = new List<IMapObjectOfRectangle>();

		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMapFileManager rMapFileManager = null;
		private bool mAutoLoadMap = false;
		private bool mIsDownloadingMapFile { get { return (mMapFileNamesOfDownloading.Count > 0); } }
		private IList<string> mMapFileNamesOfDownloading { get; } = new List<string>();

		public MapManager(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMapFileManager MapFileManager)
		{
			Set(VehicleCommunicator, VehicleInfoManager, MapFileManager);
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
		public void Set(IMapFileManager MapFileManager)
		{
			UnsubscribeEvent_IMapFileManager(rMapFileManager);
			rMapFileManager = MapFileManager;
			SubscribeEvent_IMapFileManager(rMapFileManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMapFileManager MapFileManager)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
			Set(MapFileManager);
		}
		public List<IMapObjectOfTowardPoint> GetTowardPointMapObjects(TypeOfMapObjectOfTowardPoint Type)
		{
			return mTowardPointMapObjects.Where(o => o.mType == Type).ToList();
		}
		public List<IMapObjectOfRectangle> GetRectangleMapObjects(TypeOfMapObjectOfRectangle Type)
		{
			return mRectangleMapObjects.Where(o => o.mType == Type).ToList();
		}
		public IMapObjectOfTowardPoint GetTowardPointMapObject(string Name)
		{
			return mTowardPointMapObjects.FirstOrDefault(o => o.mName == Name);
		}
		public IMapObjectOfRectangle GetRectangleMapObject(string Name)
		{
			return mRectangleMapObjects.FirstOrDefault(o => o.mName == Name);
		}
		public void LoadMap(string MapFileName)
		{
			GLCMD.CMD.LoadMap(rMapFileManager.GetMapFileFullPath(MapFileName), 3);
			mCurrentMapFileName = MapFileName;
			mCurrentMapFileHash = MD5HashCalculator.CalculateFileHash(rMapFileManager.GetMapFileFullPath(MapFileName));
			mTowardPointMapObjects.Clear();
			mTowardPointMapObjects.AddRange(GLCMD.CMD.SingleTowerPairInfo.Select(o => ConvertToIMapObjectOfTowardPoint(o)).ToList());
			mRectangleMapObjects.Clear();
			mRectangleMapObjects.AddRange(GLCMD.CMD.SingleAreaInfo.Select(o => ConvertToIMapObjectOfRectangle(o)).ToList());
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
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{

			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{

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
		private void HandleEvent_VehicleCommunicatorSentDataFailed(object Sender, SentDataEventArgs Args)
		{
			if (Args.Data is Serializable)
			{
				// 當傳送 GetMap 後卻沒有收到回應時，代表 GetMap 失敗。則將 mMapFileNamesOfDownloading 裡的相應資料移除
				if (Args.Data is GetMap)
				{
					GetMap tmpData = Args.Data as GetMap;
					string mapFileName = tmpData.Require + ".map";
					if (mMapFileNamesOfDownloading.Contains(mapFileName)) mMapFileNamesOfDownloading.Remove(mapFileName);
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
					rMapFileManager.AddMapFile(tmpData.Response.Name, tmpData.Response.Data);
					if (mMapFileNamesOfDownloading.Contains(tmpData.Response.Name)) mMapFileNamesOfDownloading.Remove(tmpData.Response.Name);
				}
				// 當收到「上傳地圖檔」的回覆，向其發送「取得當前地圖清單」的請求，以取得最新的該車地圖資訊
				else if (Args.Data is UploadMapToAGV)
				{
					rVehicleCommunicator.SendDataOfRequestMapList(Args.IpPort);
				}
				// 當收到「改變當前地圖」的回覆，向其發送「取得當前地圖清單」的請求，以取得最新的該車地圖資訊
				else if (Args.Data is ChangeMap)
				{
					rVehicleCommunicator.SendDataOfRequestMapList(Args.IpPort);
				}
				// 當收到「讀取地圖」的事件，向其發送「取得當前地圖清單」的請求，以取得最新的該車地圖資訊
				else if (Args.Data is LoadMap)
				{
					rVehicleCommunicator.SendDataOfRequestMapList(Args.IpPort);
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			// 當有車連線時，向其發送「取得當前地圖清單」的請求
			rVehicleCommunicator.SendDataOfRequestMapList(Args.Item.mIpPort);
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
					rVehicleCommunicator.SendDataOfGetMap(Args.Item.mIpPort, Args.Item.mCurrentMapName);
					mMapFileNamesOfDownloading.Add(Args.Item.mCurrentMapName);
				}

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
				if (!mIsDownloadingMapFile)
				{
					// 若有該地圖存在
					if (rMapFileManager.GetLocalMapFileNameList().Any(o => o == MapFileName))
					{
						// 該地圖 Hash 與當前地圖 Hash 不同時
						if (MD5HashCalculator.CalculateFileHash(rMapFileManager.GetMapFileFullPath(MapFileName)) != mCurrentMapFileHash)
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
		private void TryLoadMap2(string MapFileNameWithoutExtension)
		{
			TryLoadMap(MapFileNameWithoutExtension + ".map");
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
