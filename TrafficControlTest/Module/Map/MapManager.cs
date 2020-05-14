using GLCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Map
{
	public class MapManager : SystemWithConfig, IMapManager
	{
		public event EventHandlerMapFileName MapLoaded;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMapFileManager rMapFileManager = null;
		private string mCurrentMapName = string.Empty;
		private string mCurrentMapHash = string.Empty;
		private bool mAutoLoadMap = false;

		public MapManager(IVehicleInfoManager VehicleInfoManager, IMapFileManager MapFileManager)
		{
			Set(VehicleInfoManager, MapFileManager);
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
		public void Set(IVehicleInfoManager VehicleInfoManager, IMapFileManager MapFileManager)
		{
			Set(VehicleInfoManager);
			Set(MapFileManager);
		}
		public void LoadMap(string MapFileName)
		{
			GLCMD.CMD.LoadMap(rMapFileManager.GetMapFileFullPath(MapFileName), 3);
			mCurrentMapName = MapFileName;
			mCurrentMapHash = MD5HashCalculator.CalculateFileHash(rMapFileManager.GetMapFileFullPath(MapFileName));
			RaiseEvent_MapLoaded(MapFileName);
		}
		public string GetCurrentMapName()
		{
			return mCurrentMapName;
		}
		public string GetCurrentMapHash()
		{
			return mCurrentMapHash;
		}
		public string[] GetGoalNameList()
		{
			if (string.IsNullOrEmpty(mCurrentMapName))
			{
				return new string[0];
			}
			else
			{
				return GLCMD.CMD.SingleTowerPairInfo.Select(o => o.Name).ToArray();
			}
		}
		public int[] GetGoalCoordinate(string GoalName)
		{
			if (GLCMD.CMD.SingleTowerPairInfo.Any(o => o.Name == GoalName))
			{
				var goalData = GLCMD.CMD.SingleTowerPairInfo.First(o => o.Name == GoalName);
				return new int[] { goalData.X, goalData.Y, (int)goalData.Toward };
			}
			else
			{
				return new int[] { 0, 0, 0 };
			}
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
		protected virtual void RaiseEvent_MapLoaded(string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapLoaded?.Invoke(DateTime.Now, MapFileName);
			}
			else
			{
				Task.Run(() => { MapLoaded?.Invoke(DateTime.Now, MapFileName); });
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			if (mAutoLoadMap)
			{
				// 要確實抓到 "CurrentMapName" 而非 "CurrentMapNameList"
				if (Args.StatusName.Contains("CurrentMapName") && Args.StatusName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Any(o => o == "CurrentMapName"))
				{
					DateTime tmpTimestamp = DateTime.Now;
					Task.Run(() => TryLoadMap(Args.Item.mCurrentMapName));
				}
			}
		}
		private void TryLoadMap(string MapFileName)
		{
			DateTime tmpTimestamp = DateTime.Now;
			while (DateTime.Now.Subtract(tmpTimestamp).TotalMilliseconds < 5000)
			{
				// 當下載地圖完成時
				if (!rMapFileManager.mIsGettingMap)
				{
					// 若有該地圖存在，且該地圖 Hash 與當前地圖 Hash 不同時
					if (rMapFileManager.GetLocalMapNameList().Any(o => o == MapFileName) && MD5HashCalculator.CalculateFileHash(rMapFileManager.GetMapFileFullPath(MapFileName)) != GetCurrentMapHash())
					{
						LoadMap(MapFileName);
						break;
					}
					else
					{
						break;
					}
				}
				Thread.Sleep(600);
			}
		}
	}
}
