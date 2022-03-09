using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.ChargeStation
{
	public class ChargeStationInfoManagerUpdater : SystemWithConfig, IChargeStationInfoManagerUpdater
	{
		private IChargeStationInfoManager rChargeStationInfoManager = null;
		private IMapManager rMapManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private int mChargeStationLocationRangeDistance = -900; // 充電站實際位置與充電站站點方向相反，所以此數為負值
		private int mChargeStationLocationRangeWidth = 1200; // 充電站站點與實際充電位置距離通常是小於 1200 mm ，目前此二參數大概範圍為 (900 - 600 = 300) mm ~ (900 + 600 = 1500) mm

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
			return new string[] { "ChargeStationLocationRangeDistance", "ChargeStationLocationRangeWidth" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "ChargeStationLocationRangeDistance":
					return mChargeStationLocationRangeDistance.ToString();
				case "ChargeStationLocationRangeWidth":
					return mChargeStationLocationRangeWidth.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "ChargeStationLocationRangeDistance":
					mChargeStationLocationRangeDistance = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ChargeStationLocationRangeWidth":
					mChargeStationLocationRangeWidth = int.Parse(NewValue);
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
					int tmpRangeCenterX = (int)(mChargeStationLocationRangeDistance * Math.Cos(chargeStationInfos[i].mLocation.mToward / 180.0f * Math.PI) + chargeStationInfos[i].mLocation.mX);
					int tmpRangeCenterY = (int)(mChargeStationLocationRangeDistance * Math.Sin(chargeStationInfos[i].mLocation.mToward / 180.0f * Math.PI) + chargeStationInfos[i].mLocation.mY);
					IRectangle2D chargeStationLocationRange = new Rectangle2D(new Point2D(tmpRangeCenterX + mChargeStationLocationRangeWidth / 2, tmpRangeCenterY + mChargeStationLocationRangeWidth / 2), new Point2D(tmpRangeCenterX - mChargeStationLocationRangeWidth / 2, tmpRangeCenterY - mChargeStationLocationRangeWidth / 2));
					IChargeStationInfo chargeStationInfo = Library.Library.GenerateIChargeStationInfo(chargeStationInfos[i].mName, chargeStationInfos[i].mLocation, chargeStationLocationRange);
					rChargeStationInfoManager.Add(chargeStationInfo.mName, chargeStationInfo);
				}
			}

			Subtask_UpdateChargeStationInfoIsBeingUsed();
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object sender, ItemCountChangedEventArgs<IVehicleInfo> e)
		{
			Subtask_UpdateChargeStationInfoIsBeingUsed();
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object sender, ItemCountChangedEventArgs<IVehicleInfo> e)
		{
			Subtask_UpdateChargeStationInfoIsBeingUsed();
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object sender, ItemUpdatedEventArgs<IVehicleInfo> e)
		{
			if (e.StatusName.Contains("CurrentState") && (e.Item.mCurrentState == "Charge" || e.Item.mCurrentState == "ChargeIdle" || e.Item.mPreviousState == "Charge" || e.Item.mPreviousState == "ChargeIdle"))
			{
				// 當有車進入充電狀態/離開充電狀態時
				Subtask_UpdateChargeStationInfoIsBeingUsed();
			}
		}
		private void Subtask_UpdateChargeStationInfoIsBeingUsed()
		{
			IChargeStationInfo[] chargeStations = rChargeStationInfoManager.GetItems().ToArray();
			if (chargeStations != null && chargeStations.Length > 0)
			{
				IVehicleInfo[] vehicles = rVehicleInfoManager.GetItems().ToArray();
				for (int i = 0; i < chargeStations.Length; ++i)
				{
					if (vehicles.Any(o => (o.mCurrentState == "Charge" || o.mCurrentState == "ChargeIdle") && GeometryAlgorithm.IsPointInside(o.mLocationCoordinate, chargeStations[i].mLocationRange)))
					{
						if (chargeStations[i].mIsBeingUsed != true) chargeStations[i].UpdateIsBeingUsed(true);
					}
					else
					{
						if (chargeStations[i].mIsBeingUsed != false) chargeStations[i].UpdateIsBeingUsed(false);
					}
				}
			}
		}
	}
}
