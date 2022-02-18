using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.ChargeStation
{
	public class ChargeStationInfoManagerUpdater : SystemWithConfig, IChargeStationInfoManagerUpdater
	{
		private IChargeStationInfoManager rChargeStationInfoManager = null;
		private IMapManager rMapManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private int mMaximumDistanceBetweenChargeStationAndVehicle = 1500;

		public ChargeStationInfoManagerUpdater(IChargeStationInfoManager ChargeStationInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(ChargeStationInfoManager, MapManager, VehicleInfoManager);
		}
		public void Set(IChargeStationInfoManager ChargeStationInfoManager)
		{
			UnsubscribeEvent_IChargeStationInfoManager(rChargeStationInfoManager);
			rChargeStationInfoManager = ChargeStationInfoManager;
			SubscribeEvent_IChargeStationInfoManager(rChargeStationInfoManager);
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
		public void Set(IChargeStationInfoManager ChargeStationInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(ChargeStationInfoManager);
			Set(MapManager);
			Set(VehicleInfoManager);
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "MaximumDistanceBetweenChargeStationAndVehicle" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "MaximumDistanceBetweenChargeStationAndVehicle":
					return mMaximumDistanceBetweenChargeStationAndVehicle.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "MaximumDistanceBetweenChargeStationAndVehicle":
					mMaximumDistanceBetweenChargeStationAndVehicle = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}

		private void SubscribeEvent_IChargeStationInfoManager(IChargeStationInfoManager ChargeStationInfoManager)
		{
			if (ChargeStationInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IChargeStationInfoManager(IChargeStationInfoManager ChargeStationInfoManager)
		{
			if (ChargeStationInfoManager != null)
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
		private void HandleEvent_MapManagerMapChanged(object sender, MapChangedEventArgs e)
		{
			rChargeStationInfoManager.RemoveAll();
			List<IMapObjectOfTowardPoint> chargeStationInfos = rMapManager.GetTowardPointMapObjects(TypeOfMapObjectOfTowardPoint.Charge);
			if (chargeStationInfos != null && chargeStationInfos.Count > 0)
			{
				for (int i = 0; i < chargeStationInfos.Count; ++i)
				{
					IChargeStationInfo chargeStationInfo = Library.Library.GenerateIChargeStationInfo(chargeStationInfos[i].mName, chargeStationInfos[i].mLocation);
					rChargeStationInfoManager.Add(chargeStationInfo.mName, chargeStationInfo);
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object sender, ItemCountChangedEventArgs<IVehicleInfo> e)
		{
			Subtask_UpdateChargeStationInfoIsBeUsing();
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object sender, ItemCountChangedEventArgs<IVehicleInfo> e)
		{
			Subtask_UpdateChargeStationInfoIsBeUsing();
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object sender, ItemUpdatedEventArgs<IVehicleInfo> e)
		{
			if (e.StatusName.Contains("CurrentState") && (e.Item.mCurrentState == "Charge" || e.Item.mCurrentState == "ChargeIdle" || e.Item.mPreviousState == "Charge" || e.Item.mPreviousState == "ChargeIdle"))
			{
				// 當有車進入充電狀態/離開充電狀態時
				Subtask_UpdateChargeStationInfoIsBeUsing();
			}
		}
		private void Subtask_UpdateChargeStationInfoIsBeUsing()
		{
			IChargeStationInfo[] chargeStations = rChargeStationInfoManager.GetItems().ToArray();
			if (chargeStations != null && chargeStations.Length > 0)
			{
				IVehicleInfo[] vehicles = rVehicleInfoManager.GetItems().ToArray();
				for (int i = 0; i < chargeStations.Length; ++i)
				{
					if (vehicles.Any(o => (o.mCurrentState == "Charge" || o.mCurrentState == "ChargeIdle") && CalculateDistance(chargeStations[i].mLocation, o.mLocationCoordinate) < mMaximumDistanceBetweenChargeStationAndVehicle))
					{
						if (chargeStations[i].mIsBeUsing != true) chargeStations[i].UpdateIsBeUsing(true);
					}
					else
					{
						if (chargeStations[i].mIsBeUsing != false) chargeStations[i].UpdateIsBeUsing(false);
					}
				}
			}
		}
		private int CalculateDistance(IPoint2D Point1, IPoint2D Point2)
		{
			return (int)Math.Sqrt((Point1.mX - Point2.mX) * (Point1.mX - Point2.mX) + (Point1.mY - Point2.mY) * (Point1.mY - Point2.mY));
		}
	}
}
