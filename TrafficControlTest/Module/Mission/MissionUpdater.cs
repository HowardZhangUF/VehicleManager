using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Mission
{
	public class MissionUpdater : SystemWithLoopTask, IMissionUpdater
	{
		public int mTimeoutOfSendingMission { get; private set; } = 5;
		public int mTimeoutOfExecutingMission { get; private set; } = 600;
		public int mToleranceOfX { get; private set; } = 500;
		public int mToleranceOfY { get; private set; } = 500;
		public int mToleranceOfToward { get; private set; } = 5;
		public bool mAutoDetectNonSystemMission { get; private set; } = true;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMissionStateManager rMissionStateManager = null;
		private IMapManager rMapManager = null;

		public MissionUpdater(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMapManager MapManager)
		{
			Set(VehicleInfoManager, MissionStateManager, MapManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			UnsubscribeEvent_IMissionStateManager(rMissionStateManager);
			rMissionStateManager = MissionStateManager;
			SubscribeEvent_IMissionStateManager(rMissionStateManager);
		}
		public void Set(IMapManager MapManager)
		{
			rMapManager = MapManager;
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMapManager MapManager)
		{
			Set(VehicleInfoManager);
			Set(MissionStateManager);
			Set(MapManager);
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "TimeoutOfSendingMission":
					return mTimeoutOfSendingMission.ToString();
				case "TimeoutOfExecutingMission":
					return mTimeoutOfExecutingMission.ToString();
				case "ToleranceOfX":
					return mToleranceOfX.ToString();
				case "ToleranceOfY":
					return mToleranceOfY.ToString();
				case "ToleranceOfToward":
					return mToleranceOfToward.ToString();
				case "AutoDetectNonSystemMission":
					return mAutoDetectNonSystemMission.ToString();
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
				case "TimeoutOfSendingMission":
					mTimeoutOfSendingMission = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "TimeoutOfExecutingMission":
					mTimeoutOfExecutingMission = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ToleranceOfX":
					mToleranceOfX = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ToleranceOfY":
					mToleranceOfY = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ToleranceOfToward":
					mToleranceOfToward = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "AutoDetectNonSystemMission":
					mAutoDetectNonSystemMission = bool.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override void Task()
		{
			Subtask_CheckMissionSendTimeout();
			Subtask_CheckMissionExecuteTimeout();
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
		private void SubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemUpdated -= HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			// 當車子進入執行任務狀態且上一狀態不是干預狀態
			if ((Args.StatusName.Contains("CurrentState") || Args.StatusName.Contains("CurrentTarget")) && Args.Item.mCurrentState == "Running" && Args.Item.mPreviousState != "Pause" && !string.IsNullOrEmpty(Args.Item.mCurrentTarget))
			{
				IMissionState executingMission = rMissionStateManager.GetItems().FirstOrDefault(o => o.mSendState == SendState.Sending && o.mExecutorId == Args.Item.mName && o.mMission.mParametersString == Args.Item.mCurrentTarget);
				// 如果車子執行的任務來自於任務佇列
				if (executingMission != null)
				{
					executingMission.UpdateSendState(SendState.SendSuccessed);
					executingMission.UpdateExecuteState(ExecuteState.Executing);
				}
				// 反之
				else
				{
					if (mAutoDetectNonSystemMission)
					{
						// 根據該車的「當前目標」資訊去生成任務
						IMissionState tmpMissionState = GenerateIMissionState(Args.Item.mName, Args.Item.mCurrentTarget);
						if (tmpMissionState != null)
						{
							// 更新任務資訊並加入佇列
							tmpMissionState.UpdateSendState(SendState.SendSuccessed);
							tmpMissionState.UpdateExecutorId(Args.Item.mName);
							tmpMissionState.UpdateSourceIpPort($"{Args.Item.mName}Self");
							rMissionStateManager.Add(tmpMissionState.mName, tmpMissionState);
							tmpMissionState.UpdateExecuteState(ExecuteState.Executing); // 系統偵測到「任務執行狀態」改變時會更新「對應車輛的當前任務識別碼」資訊，故將此行放在 Add 之後
						}
					}
				}
			}

			// 當車子離開執行任務狀態或是干預狀態
			if (Args.StatusName.Contains("CurrentState") && Args.Item.mCurrentState != "Running" && Args.Item.mCurrentState != "Pause")
			{
				IMissionState executingMission = rMissionStateManager.GetItems().FirstOrDefault(o => o.mExecuteState == ExecuteState.Executing && o.mExecutorId == Args.Item.mName);
				if (executingMission != null)
				{
					if (IsVehicleCompletedMission(Args.Item, executingMission))
					{
						executingMission.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
					}
					else
					{
						executingMission.UpdateExecuteState(ExecuteState.ExecuteFailed);
					}
				}
			}
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			if (Args.StatusName.Contains("ExecuteState"))
			{
				switch (Args.Item.mExecuteState)
				{
					case ExecuteState.ExecuteSuccessed:
					case ExecuteState.ExecuteFailed:
						rMissionStateManager.Remove(Args.ItemName);
						break;
				}
			}
		}
		private void Subtask_CheckMissionSendTimeout()
		{
			List<IMissionState> sendingMissions = rMissionStateManager.GetItems().Where(o => o.mSendState == SendState.Sending).ToList();
			for (int i = 0; i < sendingMissions.Count; ++i)
			{
				if (rVehicleInfoManager.IsExist(sendingMissions[i].mExecutorId))
				{
					// 若指定車存在，但持續 n 秒任務傳送狀態皆未改變，標示該任務為傳送失敗、執行失敗
					if (DateTime.Now.Subtract(sendingMissions[i].mLastUpdate).TotalSeconds > mTimeoutOfSendingMission)
					{
						sendingMissions[i].UpdateSendState(SendState.SendFailed);
					}
				}
				else
				{
					// 若指定車不存在，標示該任務為傳送失敗、執行失敗
					sendingMissions[i].UpdateSendState(SendState.SendFailed);
				}
			}
		}
		private void Subtask_CheckMissionExecuteTimeout()
		{
			List<IMissionState> executingMissions = rMissionStateManager.GetItems().Where(o => o.mExecuteState == ExecuteState.Executing).ToList();
			for (int i = 0; i < executingMissions.Count; ++i)
			{
				if (rVehicleInfoManager.IsExist(executingMissions[i].mExecutorId))
				{
					// 若指定車存在，但持續 n 秒任務執行狀態皆未改變，標示該任務為執行失敗
					if (DateTime.Now.Subtract(executingMissions[i].mLastUpdate).TotalSeconds > mTimeoutOfExecutingMission)
					{
						executingMissions[i].UpdateExecuteState(ExecuteState.ExecuteFailed);
					}
				}
				else
				{
					// 若指定車不存在，標示該任務為執行失敗
					executingMissions[i].UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
			}
		}
		private IMissionState GenerateIMissionState(string VehicleId, string Target)
		{
			IMission tmpMission = null;
			if (Target.Contains("(") && Target.Contains(")") && Target.Contains(","))
			{
				// Target 為座標
				string[] tmpSplit = Target.Split(new string[] { "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
				switch (tmpSplit.Length)
				{
					case 2:
						tmpMission = Library.Library.GenerateIMission(MissionType.GotoPoint, string.Empty, 50, VehicleId, tmpSplit);
						break;
					case 3:
						tmpMission = Library.Library.GenerateIMission(MissionType.GotoPoint, string.Empty, 50, VehicleId, tmpSplit);
						break;
				}
			}
			else
			{
				// Target 為站點
				tmpMission = Library.Library.GenerateIMission(MissionType.Goto, string.Empty, 50, VehicleId, new string[] { Target });
			}

			if (tmpMission != null)
			{
				return Library.Library.GenerateIMissionState(tmpMission);
			}
			else
			{
				return null;
			}
		}
		private bool IsVehicleCompletedMission(IVehicleInfo VehicleInfo, IMissionState MissionState)
		{
			bool result = false;
			if (VehicleInfo.mCurrentState == "RouteNotFind" || VehicleInfo.mCurrentState == "ObstacleExists" || VehicleInfo.mCurrentState == "BumperTrigger")
			{
				result = false;
			}
			else if (VehicleInfo.mCurrentState == "Charge")
			{
				if (MissionState.mMission.mMissionType == MissionType.Dock)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			else if (VehicleInfo.mCurrentState == "Idle")
			{
				switch (MissionState.mMission.mMissionType)
				{
					case MissionType.Goto:
						result = IsVehicleArrived(VehicleInfo, MissionState.mMission.mParameters[0]);
						break;
					case MissionType.GotoPoint:
						if (MissionState.mMission.mParameters.Length == 2)
						{
							result = IsVehicleArrived(VehicleInfo, int.Parse(MissionState.mMission.mParameters[0]), int.Parse(MissionState.mMission.mParameters[1]));
						}
						else if (MissionState.mMission.mParameters.Length == 3)
						{
							result = IsVehicleArrived(VehicleInfo, int.Parse(MissionState.mMission.mParameters[0]), int.Parse(MissionState.mMission.mParameters[1]), int.Parse(MissionState.mMission.mParameters[2]));
						}
						break;
					case MissionType.Dock:
						result = false;
						break;
				}
			}
			return result;
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, string Target)
		{
			int[] targetCoordinate = rMapManager.GetGoalCoordinate(Target);
			return IsVehicleArrived(VehicleInfo, targetCoordinate[0], targetCoordinate[1], targetCoordinate[2]);
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, int X, int Y)
		{
			return VehicleInfo.mCurrentState == "Idle" && Math.Abs(VehicleInfo.mLocationCoordinate.mX - X) < mToleranceOfX && Math.Abs(VehicleInfo.mLocationCoordinate.mY - Y) < mToleranceOfY;
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, int X, int Y, int Toward)
		{
			int diffX = Math.Abs(VehicleInfo.mLocationCoordinate.mX - X);
			int diffY = Math.Abs(VehicleInfo.mLocationCoordinate.mY - Y);
			int diffToward = Math.Abs((int)(VehicleInfo.mLocationToward) - Toward);
			return VehicleInfo.mCurrentState == "Idle" && diffX < mToleranceOfX && diffY < mToleranceOfY && ((diffToward < mToleranceOfToward) || (diffToward <= 360 && diffToward > (360 - mToleranceOfToward)));
		}
	}
}
