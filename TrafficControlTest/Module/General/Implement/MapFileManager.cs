using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Implement
{
	public class MapFileManager : IMapFileManager
	{
		public event EventHandlerMapFileName MapFileAdded;
		public event EventHandlerMapFileName MapFileRemoved;
		public event EventHandlerVehicleNamesMapFileName VehicleCurrentMapSynchronized;

		public bool mIsGettingMap { get { return (mMapsOfGetting.Count > 0); } }
		public IList<string> mMapsOfGetting { get; } = new List<string>();

		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private string mMapFileDirectory { get; set; } = string.Empty;

		public MapFileManager(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(VehicleCommunicator, VehicleInfoManager);
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
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
		}
		public void SetConfigOfMapFileDirectory(string MapFileDirectory)
		{
			mMapFileDirectory = MapFileDirectory;
		}
		public string GetConfigOfMapFileDirectory()
		{
			return mMapFileDirectory;
		}
		public string[] GetLocalMapNameList()
		{
			string[] result = null;
			if (Directory.Exists(mMapFileDirectory))
			{
				result = Directory.GetFiles(mMapFileDirectory, "*.map", System.IO.SearchOption.TopDirectoryOnly);
				result = result.Select(o => o.Replace(mMapFileDirectory, string.Empty).Replace(".map", string.Empty)).ToArray();
			}
			return result;
		}
		public string GetMapFileFullPath(string MapFileName)
		{
			string result = string.Empty;
			if (File.Exists(mMapFileDirectory + MapFileName + ".map"))
			{
				result = mMapFileDirectory + MapFileName + ".map";
			}
			return result;
		}
		public void AddMapFile(string MapFileName, byte[] MapData)
		{
			if (!string.IsNullOrEmpty(MapFileName) && MapData != null)
			{
				if (!Directory.Exists(mMapFileDirectory)) Directory.CreateDirectory(mMapFileDirectory);
				File.WriteAllBytes(mMapFileDirectory + MapFileName, MapData);
				RaiseEvent_MapFileAdded(MapFileName);
			}
		}
		public void RemoveMapFile(string MapFileName)
		{
			if (File.Exists(mMapFileDirectory + MapFileName))
			{
				File.Delete(mMapFileDirectory + MapFileName);
				RaiseEvent_MapFileRemoved(MapFileName);
			}
		}
		public void SynchronizeVehicleCurrentMap(string MapFileName)
		{
			IEnumerable<string> vehicleNames = rVehicleInfoManager.GetItemNames();
			foreach (string vehicleName in vehicleNames)
			{
				rVehicleCommunicator.SendSerializableData_ChangeMap(rVehicleInfoManager.GetItem(vehicleName).mIpPort, MapFileName);
			}
			RaiseEvent_VehicleCurrentMapSynchronized(vehicleNames, MapFileName);
		}

		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.ReceivedSerializableData += HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.ReceivedSerializableData -= HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		protected virtual void RaiseEvent_MapFileAdded(string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapFileAdded?.Invoke(DateTime.Now, MapFileName);
			}
			else
			{
				Task.Run(() => { MapFileAdded?.Invoke(DateTime.Now, MapFileName); });
			}
		}
		protected virtual void RaiseEvent_MapFileRemoved(string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapFileRemoved?.Invoke(DateTime.Now, MapFileName);
			}
			else
			{
				Task.Run(() => { MapFileRemoved?.Invoke(DateTime.Now, MapFileName); });
			}
		}
		protected virtual void RaiseEvent_VehicleCurrentMapSynchronized(IEnumerable<string> VehicleNames, string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCurrentMapSynchronized?.Invoke(DateTime.Now, VehicleNames, MapFileName);
			}
			else
			{
				Task.Run(() => { VehicleCurrentMapSynchronized?.Invoke(DateTime.Now, VehicleNames, MapFileName); });
			}
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			if (Data is Serializable)
			{
				if (Data is GetMap)
				{
					GetMap tmpData = Data as GetMap;
					AddMapFile(tmpData.Response.Name, tmpData.Response.Data);
					if (mMapsOfGetting.Contains(Path.GetFileNameWithoutExtension(tmpData.Response.Name))) mMapsOfGetting.Remove(Path.GetFileNameWithoutExtension(tmpData.Response.Name));
				}
				else if (Data is UploadMapToAGV)
				{
					rVehicleCommunicator.SendSerializableData_RequestMapList(IpPort);
				}
				else if (Data is ChangeMap)
				{
					rVehicleCommunicator.SendSerializableData_RequestMapList(IpPort);
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			rVehicleCommunicator.SendSerializableData_RequestMapList(Item.mIpPort);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo Item)
		{
			// 做兩次判斷式，因為僅一次判斷有可能抓到 "CurrentMapName" 與 "CurrentMapNameList" 兩種結果，所以再做第二次判斷確認
			// 當 CurrentMapName 改變時去將該地圖抓下來
			if (StateName.Contains("CurrentMapName"))
			{
				string[] tmpStateNames = StateName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
				if (tmpStateNames.Any(o => o == "CurrentMapName"))
				{
					rVehicleCommunicator.SendSerializableData_GetMap(Item.mIpPort, Item.mCurrentMapName);
					mMapsOfGetting.Add(Item.mCurrentMapName);
				}
			}
		}
	}
}
