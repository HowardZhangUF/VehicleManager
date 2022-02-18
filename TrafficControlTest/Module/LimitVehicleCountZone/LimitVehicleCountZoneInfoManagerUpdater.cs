using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public class LimitVehicleCountZoneInfoManagerUpdater : SystemWithLoopTask, ILimitVehicleCountZoneInfoManagerUpdater
	{
		private ILimitVehicleCountZoneInfoManager rLimitVehicleCountZoneInfoManager = null;
		private IMapManager rMapManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;

		public LimitVehicleCountZoneInfoManagerUpdater(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(LimitVehicleCountZoneInfoManager, MapManager, VehicleInfoManager);
		}
		public void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			UnsubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
			rLimitVehicleCountZoneInfoManager = LimitVehicleCountZoneInfoManager;
			SubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
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
		public void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(LimitVehicleCountZoneInfoManager);
			Set(MapManager);
			Set(VehicleInfoManager);
		}
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			result += $"CountOfLimitVehicleCountZoneInfo: {rLimitVehicleCountZoneInfoManager.mCount}, CountOfVehicleInfo: {rVehicleInfoManager.mCount}";
			return result;
		}
		public override void Task()
		{
			Subtask_CalculateVehicleNameListInLimitVehicleCountZoneInfo();
		}

		private void SubscribeEvent_ILimitVehicleCountZoneInfoManager(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			if (LimitVehicleCountZoneInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_ILimitVehicleCountZoneInfoManager(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			if (LimitVehicleCountZoneInfoManager != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged += HandleEvent_IMapManagerMapChanged;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged -= HandleEvent_IMapManagerMapChanged;
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				// do nothing
			}
		}
		private void HandleEvent_IMapManagerMapChanged(object sender, MapChangedEventArgs e)
		{
			rLimitVehicleCountZoneInfoManager.RemoveAll();
			List<IMapObjectOfRectangle> singleVehicleInfos = rMapManager.GetRectangleMapObjects(TypeOfMapObjectOfRectangle.SingleVehicle);
			if (singleVehicleInfos != null && singleVehicleInfos.Count > 0)
			{
				for (int i = 0; i < singleVehicleInfos.Count; ++i)
				{
					// SingleVehicle 區塊沒有額外命名，所以名字採用流水號，允需車數量固定為 1
					ILimitVehicleCountZoneInfo tmp = new LimitVehicleCountZoneInfo(i.ToString().PadLeft(3, '0'), singleVehicleInfos[i].mRange, 1);
					rLimitVehicleCountZoneInfoManager.Add(tmp.mName, tmp);
				}
			}
		}
		private void Subtask_CalculateVehicleNameListInLimitVehicleCountZoneInfo()
		{
			List<ILimitVehicleCountZoneInfo> tmpLimitVehicleCountZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().ToList();
			List<IVehicleInfo> tmpVehicleInfos = rVehicleInfoManager.GetItems().ToList();
			if (tmpLimitVehicleCountZoneInfos.Count > 0 && tmpVehicleInfos.Count > 0)
			{
				// 計算每一個 ILimitVehicleCountZoneInfo 內存在的車的清單
				for (int i = 0; i < tmpLimitVehicleCountZoneInfos.Count; ++i)
				{
					List<string> tmpVehicleNames = tmpVehicleInfos.Where(o => tmpLimitVehicleCountZoneInfos[i].mRange.IsIncludePoint(o.mLocationCoordinate)).Select(o => o.mName).ToList();
					rLimitVehicleCountZoneInfoManager.UpdateCurrentVehicleNameList(tmpLimitVehicleCountZoneInfos[i].mName, tmpVehicleNames);
				}
			}
		}
	}
}
