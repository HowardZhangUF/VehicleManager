using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.LimitVehicleCountZone;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.VehiclePassThroughAutomaticDoor;

namespace TrafficControlTest.Module.VehiclePassThroughLimitVehicleCountZone
{
	public class VehiclePassThroughLimitVehicleCountZoneEventManagerUpdater : SystemWithLoopTask, IVehiclePassThroughLimitVehicleCountZoneEventManagerUpdater
	{
		public int mDistanceThreshold { get; private set; } = 2500;

		private IVehiclePassThroughLimitVehicleCountZoneEventManager rVehiclePassThroughLimitVehicleCountZoneEventManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private ILimitVehicleCountZoneInfoManager rLimitVehicleCountZoneInfoManager = null;

		public VehiclePassThroughLimitVehicleCountZoneEventManagerUpdater(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleInfoManager VehicleInfoManager, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			Set(VehiclePassThroughLimitVehicleCountZoneEventManager, VehicleInfoManager, LimitVehicleCountZoneInfoManager);
		}
		public void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager)
		{
			UnsubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(rVehiclePassThroughLimitVehicleCountZoneEventManager);
			rVehiclePassThroughLimitVehicleCountZoneEventManager = VehiclePassThroughLimitVehicleCountZoneEventManager;
			SubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(rVehiclePassThroughLimitVehicleCountZoneEventManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);

		}
		public void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			UnsubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
			rLimitVehicleCountZoneInfoManager = LimitVehicleCountZoneInfoManager;
			SubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
		}
		public void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleInfoManager VehicleInfoManager, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			Set(VehiclePassThroughLimitVehicleCountZoneEventManager);
			Set(VehicleInfoManager);
			Set(LimitVehicleCountZoneInfoManager);
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "TimePeriod", "DistanceThreshold" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "DistanceThreshold":
					return mDistanceThreshold.ToString();
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
				case "DistanceThreshold":
					mDistanceThreshold = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			List<IVehicleInfo> vehicleInfos = rVehicleInfoManager.GetItems().Where(o => o.mCurrentState == "Running" || o.mCurrentState == "Operating" || o.mCurrentState == "Pause").ToList();
			List<ILimitVehicleCountZoneInfo> zoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().ToList();
			List<IVehiclePassThroughLimitVehicleCountZoneEvent> eventInfos = rVehiclePassThroughLimitVehicleCountZoneEventManager.GetItems().ToList();
			result += $"VehicleRunningOperatingPauseCount: {vehicleInfos.Count}";
			result += $", LimitVehicleCountZoneCount: {zoneInfos.Count}";
			result += $", VehiclePassThroughLimitVehicleCountZoneEventCount: {eventInfos.Count}";
			return result;
		}
		public override void Task()
		{
			Subtask_DetectVehiclePassThroughLimitVehicleCountZoneEvent();
		}

		private void SubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager)
		{
			if (VehiclePassThroughLimitVehicleCountZoneEventManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager)
		{
			if (VehiclePassThroughLimitVehicleCountZoneEventManager != null)
			{
				// do nothing
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
		private void Subtask_DetectVehiclePassThroughLimitVehicleCountZoneEvent()
		{
			List<IVehicleInfo> vehicleInfos = rVehicleInfoManager.GetItems().ToList();
			List<ILimitVehicleCountZoneInfo> limitVehicleCountZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().ToList();
			List<IVehiclePassThroughLimitVehicleCountZoneEvent> currentEvents = new List<IVehiclePassThroughLimitVehicleCountZoneEvent>();
			List<IVehiclePassThroughLimitVehicleCountZoneEvent> lastEvents = rVehiclePassThroughLimitVehicleCountZoneEventManager.GetItems().ToList();
			if (vehicleInfos != null && vehicleInfos.Count > 0 && limitVehicleCountZoneInfos != null && limitVehicleCountZoneInfos.Count > 0)
			{
				// 計算當前的事件集合
				for (int i = 0; i < vehicleInfos.Count; ++i)
				{
					for (int j = 0; j < limitVehicleCountZoneInfos.Count; ++j)
					{
						// 如果自走車已經走到限車區內
						if (IsVehicleInLimitVehicleCountZone(vehicleInfos[i], limitVehicleCountZoneInfos[j]))
						{
							if (IsILimitVehicleCountZoneFull(limitVehicleCountZoneInfos[j]))
							{
								// 如果自走車不在該區域的允許移動名單內
								if (!IsVehicleAllowedMoveInLimitVehicleCountZone(vehicleInfos[i], limitVehicleCountZoneInfos[j]))
								{
									IVehiclePassThroughLimitVehicleCountZoneEvent tmp = Library.Library.GenerateIVehiclePassThroughLimitVehicleCountZoneEvent(vehicleInfos[i], limitVehicleCountZoneInfos[j], 0);
									currentEvents.Add(tmp);
								}
							}
						}
						// 如果自走車還沒走到限車區內
						else
						{
							// 如果車子路徑線有穿越限車區
							if (IsVehiclePassThroughLimitVehicleCountZone(vehicleInfos[i], limitVehicleCountZoneInfos[j]))
							{
								// 該限車區為滿的
								if (IsILimitVehicleCountZoneFull(limitVehicleCountZoneInfos[j]))
								{
									int distance = GetDistanceBetweenVehicleAndLimitVehicleCountZoneAlongPathLine(vehicleInfos[i], limitVehicleCountZoneInfos[j]);
									// 車子不是限車區裡面的車，且即將通過限車區
									// 加上距離大於 0 的條件是為了避免運算溢位的錯誤
									// 距離等於 0 代表車子在限車區內部
									if (!IsVehicleAllowedMoveInLimitVehicleCountZone(vehicleInfos[i], limitVehicleCountZoneInfos[j]) && distance >= 0 && distance < mDistanceThreshold)
									{
										IVehiclePassThroughLimitVehicleCountZoneEvent tmp = Library.Library.GenerateIVehiclePassThroughLimitVehicleCountZoneEvent(vehicleInfos[i], limitVehicleCountZoneInfos[j], distance);
										currentEvents.Add(tmp);
									}
								}
							}
						}
					}
				}

				// 新的事件集合與舊的事件集合比較 (Add, Update Item 判斷)
				for (int i = 0; i < currentEvents.Count; ++i)
				{
					// 如果 Event 已存在，更新之，反之，新增之
					if (rVehiclePassThroughLimitVehicleCountZoneEventManager.IsExist(currentEvents[i].rVehicleInfo.mName, currentEvents[i].rLimitVehicleCountZoneInfo.mName))
					{
						rVehiclePassThroughLimitVehicleCountZoneEventManager.UpdateDistance(currentEvents[i].rVehicleInfo.mName, currentEvents[i].rLimitVehicleCountZoneInfo.mName, currentEvents[i].mDistance);
						rVehiclePassThroughLimitVehicleCountZoneEventManager.UpdateState(currentEvents[i].rVehicleInfo.mName, currentEvents[i].rLimitVehicleCountZoneInfo.mName, currentEvents[i].mState);
					}
					else
					{
						rVehiclePassThroughLimitVehicleCountZoneEventManager.Add(currentEvents[i].mName, currentEvents[i]);
					}
				}

				// 舊的事件集合與新的事件集合比較 (Remove Item 判斷)
				for (int i = 0; i < lastEvents.Count; ++i)
				{
					// 如果舊的 Event 在當前事件集合中找不到對應的項目，則代表該 Event 已結束
					if (!currentEvents.Any(o => o.rVehicleInfo.mName == lastEvents[i].rVehicleInfo.mName && o.rLimitVehicleCountZoneInfo.mName == lastEvents[i].rLimitVehicleCountZoneInfo.mName))
					{
						rVehiclePassThroughLimitVehicleCountZoneEventManager.Remove(lastEvents[i].mName);
					}
				}
			}
		}
		/// <summary>計算指定 IVehicleInfo 是否在指定 ILimitVehicleCountZoneInfo 區域內</summary>
		private bool IsVehicleInLimitVehicleCountZone(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			return LimitVehicleCountZoneInfo.mRange.IsIncludePoint(VehicleInfo.mLocationCoordinate);
		}
		/// <summary>計算指定 IVehicleInfo 的路徑是否有穿越指定 ILimitVehicleCountZoneInfo 區域</summary>
		private bool IsVehiclePassThroughLimitVehicleCountZone(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			List<IPoint2D> fullPath = new List<IPoint2D>();
			fullPath.Add(VehicleInfo.mLocationCoordinate);
			fullPath.AddRange(VehicleInfo.mPath);
			return GeometryAlgorithm.IsLinePassThroughRectangle(fullPath, LimitVehicleCountZoneInfo.mRange);
		}
		/// <summary>查詢指定的 ILimitVehicleCountZoneInfo 是否已滿車</summary>
		private bool IsILimitVehicleCountZoneFull(ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			bool result = false;
			if (GetCurrentVehicleCount(LimitVehicleCountZoneInfo, rLimitVehicleCountZoneInfoManager) >= LimitVehicleCountZoneInfo.mMaxVehicleCount)
			{
				result = true;
			}
			return result;
		}
		/// <summary>計算指定 IVehicleInfo 在指定 ILimitVehicleCountZoneInfo 內是否是可允許移動的</summary>
		private bool IsVehicleAllowedMoveInLimitVehicleCountZone(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			// LimitVehicleCountZoneInfo 的上限是 n 台車，當區域內的車數量大於 n 時，只有先進來的前 n 台車可以動，反之，會被干預
			return GetCurrentVehicleNameList(LimitVehicleCountZoneInfo, rLimitVehicleCountZoneInfoManager).Take(LimitVehicleCountZoneInfo.mMaxVehicleCount).Contains(VehicleInfo.mName);
		}
		/// <summary>計算指定 IVehicleInfo 與指定 ILimitVehicleCountZoneInfo 的距離(沿著路徑線計算)</summary>
		private int GetDistanceBetweenVehicleAndLimitVehicleCountZoneAlongPathLine(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			int result = 0;
			if (!LimitVehicleCountZoneInfo.mRange.IsIncludePoint(VehicleInfo.mLocationCoordinate))
			{
				if (VehicleInfo.mPathDetail != null && VehicleInfo.mPathDetail.Count > 0)
				{
					List<IPoint2D> fullPath = new List<IPoint2D>();
					fullPath.Add(VehicleInfo.mLocationCoordinate);
					fullPath.AddRange(VehicleInfo.mPathDetail);

					for (int i = 0; i < fullPath.Count - 1; ++i)
					{
						List<IPoint2D> intersectionPoint = GeometryAlgorithm.GetIntersectionPoint(LimitVehicleCountZoneInfo.mRange, fullPath[i], fullPath[i + 1]).ToList();
						if (intersectionPoint != null && intersectionPoint.Count > 0)
						{
							// 找到交點
							List<IPoint2D> points = fullPath.Take(i + 1).ToList();
							points.Add(intersectionPoint.ElementAt(0));
							result = (int)GeometryAlgorithm.GetDistance(points);
							break;
						}
					}
				}
			}
			return result;
		}

		/// <summary>查詢指定的 ILimitVehicleCountZoneInfo 的當前資訊。若該區有其他區聯集，會一同計算</summary>
		private static List<Tuple<string, DateTime>> GetCurrentVehicleList(ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			if (LimitVehicleCountZoneInfo.mIsUnioned) // 如果欲查詢的 zone 有與其他 zone 聯集
			{
				int unionId = LimitVehicleCountZoneInfo.mUnionId;
				List<ILimitVehicleCountZoneInfo> unionedZoneInfos = LimitVehicleCountZoneInfoManager.GetItems().Where(o => o.mUnionId == unionId).ToList();

				Dictionary<string, Tuple<string, DateTime>> result = new Dictionary<string, Tuple<string, DateTime>>();
				for (int i = 0; i < unionedZoneInfos.Count; ++i)
				{
					for (int j = 0; j < unionedZoneInfos[i].mCurrentVehicleNameList.Count; ++j)
					{
						string vehicleName = unionedZoneInfos[i].mCurrentVehicleNameList[j].Item1;
						DateTime enterTimestamp = unionedZoneInfos[i].mCurrentVehicleNameList[j].Item2;
						if (result.ContainsKey(vehicleName)) // 如果項目重複
						{
							if (enterTimestamp < result[vehicleName].Item2) // 進入時間點使用最小值
							{
								result[vehicleName] = unionedZoneInfos[i].mCurrentVehicleNameList[j];
							}
						}
						else
						{
							result.Add(vehicleName, unionedZoneInfos[i].mCurrentVehicleNameList[j]);
						}
					}
				}
				return result.Values.OrderBy(o => o.Item2).ToList();
			}
			else
			{
				return LimitVehicleCountZoneInfo.mCurrentVehicleNameList.OrderBy(o => o.Item2).ToList();
			}
		}
		/// <summary>查詢指定的 ILimitVehicleCountZoneInfo 的當前自走車清單。若該區有其他區聯集，會一同計算</summary>
		private static List<string> GetCurrentVehicleNameList(ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			return GetCurrentVehicleList(LimitVehicleCountZoneInfo, LimitVehicleCountZoneInfoManager).Select(o => o.Item1).ToList();
		}
		/// <summary>查詢指定的 ILimitVehicleCountZoneInfo 的當前自走車數量。若該區有其他區聯集，會一同計算</summary>
		private static int GetCurrentVehicleCount(ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			return GetCurrentVehicleList(LimitVehicleCountZoneInfo, LimitVehicleCountZoneInfoManager).Count();
		}
	}
}
