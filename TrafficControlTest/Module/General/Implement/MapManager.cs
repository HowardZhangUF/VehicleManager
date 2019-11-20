using GLCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Implement
{
	public class MapManager : IMapManager
	{
		public event EventHandlerMapFileName MapLoaded;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMapFileManager rMapFileManager = null;
		private string mCurrentMapName = string.Empty;
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
		public void SetConfigOfAutoLoadMap(bool Enable)
		{
			mAutoLoadMap = Enable;
		}
		public bool GetConfigOfAutoLoadMap()
		{
			return mAutoLoadMap;
		}
		public void LoadMap(string MapFileName)
		{
			GLCMD.CMD.LoadMap(rMapFileManager.GetMapFileFullPath(MapFileName), 3);
			mCurrentMapName = MapFileName;
			RaiseEvent_MapLoaded(MapFileName);
		}
		public string GetCurrentMapName()
		{
			return mCurrentMapName;
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
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo Item)
		{
			if (mAutoLoadMap)
			{
				// 要確實抓到 "CurrentMapName" 而非 "CurrentMapNameList"
				if (StateName.Contains("CurrentMapName") && StateName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Any(o => o == "CurrentMapName"))
				{
					DateTime tmpTimestamp = DateTime.Now;
					Task.Run(() =>
					{
						while (DateTime.Now.Subtract(tmpTimestamp).TotalMilliseconds < 5000)
						{
							if (rMapFileManager.GetLocalMapNameList().Any(o => o == Item.mCurrentMapName))
							{
								LoadMap(Item.mCurrentMapName);
								break;
							}
							Thread.Sleep(600);
						}
					});
				}
			}
		}
	}
}
