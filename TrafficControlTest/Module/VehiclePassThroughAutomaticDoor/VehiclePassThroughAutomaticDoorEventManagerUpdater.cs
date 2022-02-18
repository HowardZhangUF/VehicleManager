using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.VehiclePassThroughAutomaticDoor
{
	public class VehiclePassThroughAutomaticDoorEventManagerUpdater : SystemWithLoopTask, IVehiclePassThroughAutomaticDoorEventManagerUpdater
	{
		public int mOpenDoorDistance { get; private set; } = 2500;
		public int mCloseDoorDistance { get; private set; } = 2500;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private IAutomaticDoorInfoManager rAutomaticDoorInfoManager = null;
		private IVehiclePassThroughAutomaticDoorEventManager rVehiclePassThroughAutomaticDoorEventManager = null;

		public VehiclePassThroughAutomaticDoorEventManagerUpdater(IVehicleInfoManager VehicleInfoManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			Set(VehicleInfoManager, AutomaticDoorInfoManager, VehiclePassThroughAutomaticDoorEventManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			UnsubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
			rAutomaticDoorInfoManager = AutomaticDoorInfoManager;
			SubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
		}
		public void Set(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(rVehiclePassThroughAutomaticDoorEventManager);
			rVehiclePassThroughAutomaticDoorEventManager = VehiclePassThroughAutomaticDoorEventManager;
			SubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(rVehiclePassThroughAutomaticDoorEventManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			Set(VehicleInfoManager);
			Set(AutomaticDoorInfoManager);
			Set(VehiclePassThroughAutomaticDoorEventManager);
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "TimePeriod", "OpenDoorDistance", "CloseDoorDistance" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "OpenDoorDistance":
					return mOpenDoorDistance.ToString();
				case "CloseDoorDistance":
					return mCloseDoorDistance.ToString();
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
				case "OpenDoorDistance":
					mOpenDoorDistance = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "CloseDoorDistance":
					mCloseDoorDistance = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			result += $"OpenDoorDistance: {mOpenDoorDistance}, CloseDoorDistance: {mCloseDoorDistance}, CountOfVehiclePassThroughAutomaticDoorEvent: {rVehiclePassThroughAutomaticDoorEventManager.mCount}";
			if (rVehiclePassThroughAutomaticDoorEventManager.mCount > 0)
			{
				result += ", ";
				result += string.Join(", ", rVehiclePassThroughAutomaticDoorEventManager.GetItems().Select(o => $"{o.mVehicleName}-{o.mAutomaticDoorName}"));
			}
			return result;
		}
		public override void Task()
		{
			Subtask_IsCurrentEventSolved();
			Subtask_DetectEvent();
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
		private void SubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			if (VehiclePassThroughAutomaticDoorEventManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			if (VehiclePassThroughAutomaticDoorEventManager != null)
			{
				// do nothing
			}
		}
		private void Subtask_IsCurrentEventSolved()
		{
			// 根據現有的 Event 找到對應的 IVehicleInfo 與 IAutomaticDoorInfo 後，計算此 Event 是否已經結束，若已經結束，則將其從 Manager 中移除。
			// 備註：為避免「特殊案例」，車子斷線並不會讓 Event 結束
			// 特殊情境：車子在通過自動門途中斷線，誤判 Event 結束(車子不在自動門附近)，而讓自動門關門，結果自動門夾到車子
			IEnumerable<IVehiclePassThroughAutomaticDoorEvent> events = rVehiclePassThroughAutomaticDoorEventManager.GetItems();
			if (events != null && events.Count() > 0)
			{
				for (int i = 0; i < events.Count(); ++i)
				{
					IVehiclePassThroughAutomaticDoorEvent e = events.ElementAt(i);
					string vehicleName = e.mVehicleName;
					string automaticDoorName = e.mAutomaticDoorName;
					if (rVehicleInfoManager.IsExist(vehicleName) && rAutomaticDoorInfoManager.IsExist(automaticDoorName))
					{
						IVehicleInfo vehicleInfo = rVehicleInfoManager[vehicleName];
						IAutomaticDoorInfo automaticDoorInfo = rAutomaticDoorInfoManager[automaticDoorName];
						if (!IsVehiclePassThroughAutomaticDoor(vehicleInfo, automaticDoorInfo))
						{
							// 更新 PassThrough 狀態
							if (e.mState == PassThroughState.Passing)
							{
								e.UpdateState(PassThroughState.Passed);
							}

							// 檢查車子與自動門的距離是否大於 mCloseDoorDistance
							int distance = GetDistanceBetweenVehicleAndAutomaticDoor(vehicleInfo, automaticDoorInfo);
							if (distance > mCloseDoorDistance)
							{
								rVehiclePassThroughAutomaticDoorEventManager.Remove(e.mName);
								break;
							}
						}
					}
				}
			}
		}
		private void Subtask_DetectEvent()
		{
			// 使用移動中的 IVehicleInfo 與所有的 IAutomaticDoorInfo 計算是否有 Event 發生，若有，則將其新增/更新至 IVehiclePassThroughAutomaticDoorEventManager 中
			List<IVehicleInfo> vehicleInfos = rVehicleInfoManager.GetItems().Where(o => o.mCurrentState == "Running" || o.mCurrentState == "Operating" || o.mCurrentState == "Pause").ToList();
			List<IAutomaticDoorInfo> automaticDoorInfos = rAutomaticDoorInfoManager.GetItems().ToList();
			if (vehicleInfos != null && vehicleInfos.Count > 0)
			{
				for (int i = 0; i < vehicleInfos.Count; ++i)
				{
					for (int j = 0; j < automaticDoorInfos.Count; ++j)
					{
						// 如果車子路徑線有通過自動門
						if (IsVehiclePassThroughAutomaticDoor(vehicleInfos[i], automaticDoorInfos[j]))
						{
							PassThroughState state = IsVehicleInAutomaticDoor(vehicleInfos[i], automaticDoorInfos[j]) ? PassThroughState.Passing : PassThroughState.WillPass;
							int distance = GetDistanceBetweenVehicleAndAutomaticDoor(vehicleInfos[i], automaticDoorInfos[j]);
							IVehiclePassThroughAutomaticDoorEvent e = Library.Library.GenerateIVehiclePassThroughAutomaticDoorEvent(vehicleInfos[i].mName, automaticDoorInfos[j].mName, distance);
							e.UpdateState(state);
							// 檢查是否正在通過自動門或是車子與自動門的距離是否小於 mOpenDoorDistance
							if (state == PassThroughState.Passing || distance < mOpenDoorDistance)
							{
								if (rVehiclePassThroughAutomaticDoorEventManager.IsExist(e.mName))
								{
									rVehiclePassThroughAutomaticDoorEventManager[e.mName].UpdateDistance(e.mDistance);
									rVehiclePassThroughAutomaticDoorEventManager[e.mName].UpdateState(e.mState);
								}
								else
								{
									rVehiclePassThroughAutomaticDoorEventManager.Add(e.mName, e);
								}
							}
						}
					}
				}
			}
		}
		private bool IsVehiclePassThroughAutomaticDoor(IVehicleInfo VehicleInfo, IAutomaticDoorInfo AutomaticDoorInfo)
		{
			List<IPoint2D> fullPath = new List<IPoint2D>();
			fullPath.Add(VehicleInfo.mLocationCoordinate);
			fullPath.AddRange(VehicleInfo.mPath);
			return GeometryAlgorithm.IsLinePassThroughRectangle(fullPath, AutomaticDoorInfo.mRange);
		}
		private bool IsVehicleInAutomaticDoor(IVehicleInfo VehicleInfo, IAutomaticDoorInfo AutomaticDoorInfo)
		{
			return AutomaticDoorInfo.mRange.IsIncludePoint(VehicleInfo.mLocationCoordinate);
		}
		private int GetDistanceBetweenVehicleAndAutomaticDoor(IVehicleInfo VehicleInfo, IAutomaticDoorInfo AutomaticDoorInfo)
		{
			return GeometryAlgorithm.GetDistanceBetweenPointAndRectangleEdge(VehicleInfo.mLocationCoordinate, AutomaticDoorInfo.mRange);
		}
	}
}
