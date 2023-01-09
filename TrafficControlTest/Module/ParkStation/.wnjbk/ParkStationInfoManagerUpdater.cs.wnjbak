using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.ParkStation
{
	public class ParkStationInfoManagerUpdater : SystemWithLoopTask, IParkStationInfoManagerUpdater // 基本上與 ChargeStationInfoManagerUpdater 內容相似
	{
		private IParkStationInfoManager rParkStationInfoManager = null;
		private IMapManager rMapManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private int mParkStationLocationRangeWidth = 1600;
		private object mLockOfUpdateParkStationInfoIsBeingUsed = new object();

		public ParkStationInfoManagerUpdater(IParkStationInfoManager ParkStationInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(ParkStationInfoManager, MapManager, VehicleInfoManager);
		}
		public void Set(IParkStationInfoManager ParkStationInfoManager)
		{
			UnsubscribeEvent_IParkStationInfoManager(rParkStationInfoManager);
			rParkStationInfoManager = ParkStationInfoManager;
			SubscribeEvent_IParkStationInfoManager(rParkStationInfoManager);
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IParkStationInfoManager ParkStationInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(ParkStationInfoManager);
			Set(MapManager);
			Set(VehicleInfoManager);
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "TimePeriod", "ParkStationLocationRangeWidth" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "ParkStationLocationRangeWidth":
					return mParkStationLocationRangeWidth.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					mTimePeriod = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ParkStationLocationRangeWidth":
					mParkStationLocationRangeWidth = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			IParkStationInfo[] parkStations = rParkStationInfoManager.GetItems().ToArray();
			IVehicleInfo[] vehicles = rVehicleInfoManager.GetItems().ToArray();
			result += $"ParkStationInfoCount: {parkStations.Length}";
			result += $", VehicleInfoCount: {vehicles.Length}";
			return result;
		}
		public override void Task()
		{
			Subtask_UpdateParkStationInfoIsBeingUsed();
		}

		private void SubscribeEvent_IParkStationInfoManager(IParkStationInfoManager ParkStationInfoManager)
		{
			if (ParkStationInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IParkStationInfoManager(IParkStationInfoManager ParkStationInfoManager)
		{
			if (ParkStationInfoManager != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged += HandleEvent_MapManagerMapChanged;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged -= HandleEvent_MapManagerMapChanged;
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
			}
		}
		private void HandleEvent_MapManagerMapChanged(object sender, MapChangedEventArgs e)
		{
			rParkStationInfoManager.RemoveAll();
			List<IMapObjectOfTowardPoint> parkStationInfos = rMapManager.GetTowardPointMapObjects(TypeOfMapObjectOfTowardPoint.Park);
			if (parkStationInfos != null && parkStationInfos.Count > 0)
			{
				for (int i = 0; i < parkStationInfos.Count; ++i)
				{
					IRectangle2D parkStationLocationRange = new Rectangle2D(new Point2D(parkStationInfos[i].mLocation.mX + mParkStationLocationRangeWidth / 2, parkStationInfos[i].mLocation.mY + mParkStationLocationRangeWidth / 2), new Point2D(parkStationInfos[i].mLocation.mX - mParkStationLocationRangeWidth / 2, parkStationInfos[i].mLocation.mY - mParkStationLocationRangeWidth / 2));
					IParkStationInfo parkStationInfo = Library.Library.GenerateIParkStationInfo(parkStationInfos[i].mName, parkStationInfos[i].mLocation, parkStationLocationRange);
					rParkStationInfoManager.Add(parkStationInfo.mName, parkStationInfo);
				}
			}

			Subtask_UpdateParkStationInfoIsBeingUsed();
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object sender, ItemCountChangedEventArgs<IVehicleInfo> e)
		{
			Subtask_UpdateParkStationInfoIsBeingUsed();
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object sender, ItemCountChangedEventArgs<IVehicleInfo> e)
		{
			Subtask_UpdateParkStationInfoIsBeingUsed();
		}
		private void Subtask_UpdateParkStationInfoIsBeingUsed()
		{
			lock (mLockOfUpdateParkStationInfoIsBeingUsed)
			{
				IParkStationInfo[] parkStations = rParkStationInfoManager.GetItems().ToArray();
				if (parkStations != null && parkStations.Length > 0)
				{
					IVehicleInfo[] vehicles = rVehicleInfoManager.GetItems().ToArray();
					for (int i = 0; i < parkStations.Length; ++i)
					{
						if (vehicles.Any(o => GeometryAlgorithm.IsPointInside(o.mLocationCoordinate, parkStations[i].mLocationRange)))
						{
							if (parkStations[i].mIsBeingUsed != true) parkStations[i].UpdateIsBeingUsed(true);
						}
						else
						{
							if (parkStations[i].mIsBeingUsed != false) parkStations[i].UpdateIsBeingUsed(false);
						}
					}
				}
			}
		}
	}
}
