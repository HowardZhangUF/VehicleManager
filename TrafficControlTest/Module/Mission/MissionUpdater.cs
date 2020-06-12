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
			Subtask_CheckMissionSendState();
			Subtask_CheckMissionExecuteState();
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
			if (mAutoDetectNonSystemMission)
			{
				if ((Args.StatusName.Contains("CurrentState") || Args.StatusName.Contains("CurrentTarget")) && Args.Item.mCurrentState == "Running" && !string.IsNullOrEmpty(Args.Item.mCurrentTarget))
				{
					// 該車沒有在執行任務，且任務佇列也沒有指派任務給該車
					if (string.IsNullOrEmpty(Args.Item.mCurrentMissionId) && !rMissionStateManager.GetItems().Where(o => o.mSendState == SendState.Sending || o.mExecuteState == ExecuteState.Executing).Any(o => o.mExecutorId == Args.Item.mName))
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
		private void Subtask_CheckMissionSendState()
		{
			List<IMissionState> sendingMissions = rMissionStateManager.GetItems().Where(o => o.mSendState == SendState.Sending).ToList();
			for (int i = 0; i < sendingMissions.Count; ++i)
			{
				if (rVehicleInfoManager.IsExist(sendingMissions[i].mExecutorId))
				{
					if (rVehicleInfoManager.GetItem(sendingMissions[i].mExecutorId).mCurrentTarget == sendingMissions[i].mMission.mParametersString)
					{
						// 如果該車存在，且「該車的當前目標」與「任務目標」相同，標示該任務為傳送成功、執行中
						sendingMissions[i].UpdateSendState(SendState.SendSuccessed);
						sendingMissions[i].UpdateExecuteState(ExecuteState.Executing);
					}
					else
					{
						// 如果該車存在，但持續 n 秒都未達成上述條件，標示該任務為傳送失敗、執行失敗
						if (DateTime.Now.Subtract(sendingMissions[i].mLastUpdate).TotalSeconds > mTimeoutOfSendingMission)
						{
							sendingMissions[i].UpdateSendState(SendState.SendFailed);
							sendingMissions[i].UpdateExecuteState(ExecuteState.ExecuteFailed);
						}
					}
				}
				else
				{
					// 如果執行該任務的車子不存在，代表該車斷線，標示該任務為執行失敗
					sendingMissions[i].UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
			}
		}
		private void Subtask_CheckMissionExecuteState()
		{
			List<IMissionState> executingMissions = rMissionStateManager.GetItems().Where(o => o.mExecuteState == ExecuteState.Executing).ToList();
			for (int i = 0; i < executingMissions.Count; ++i)
			{
				if (rVehicleInfoManager.IsExist(executingMissions[i].mExecutorId))
				{
					ExecuteState executeState = GetMissionExecuteStatus(rVehicleInfoManager.GetItem(executingMissions[i].mExecutorId), executingMissions[i]);
					switch (executeState)
					{
						case ExecuteState.Executing:
							if (DateTime.Now.Subtract(executingMissions[i].mLastUpdate).TotalSeconds > mTimeoutOfExecutingMission)
							{
								executingMissions[i].UpdateExecuteState(ExecuteState.ExecuteFailed);
							}
							break;
						case ExecuteState.ExecuteSuccessed:
							executingMissions[i].UpdateExecuteState(ExecuteState.ExecuteSuccessed);
							break;
						case ExecuteState.ExecuteFailed:
							executingMissions[i].UpdateExecuteState(ExecuteState.ExecuteFailed);
							break;
					}
				}
				else
				{
					// 如果執行該任務的車子不存在，代表該車斷線，標示該任務為執行失敗
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
		private ExecuteState GetMissionExecuteStatus(IVehicleInfo VehicleInfo, IMissionState MissionState)
		{
			ExecuteState result = ExecuteState.Executing;
			if (IsVehicleError(VehicleInfo))
			{
				result = ExecuteState.ExecuteFailed;
			}
			else if (VehicleInfo.mCurrentState == "Charge")
			{
				if (MissionState.mMission.mMissionType == MissionType.Dock)
				{
					result = ExecuteState.ExecuteSuccessed;
				}
				else
				{
					result = ExecuteState.ExecuteFailed;
				}
			}
			else if (VehicleInfo.mCurrentState == "Idle")
			{
				switch (MissionState.mMission.mMissionType)
				{
					case MissionType.Goto:
						result = IsVehicleArrived(VehicleInfo, MissionState.mMission.mParameters[0]) ? ExecuteState.ExecuteSuccessed : ExecuteState.ExecuteFailed;
						break;
					case MissionType.GotoPoint:
						if (MissionState.mMission.mParameters.Length == 2)
						{
							result = IsVehicleArrived(VehicleInfo, int.Parse(MissionState.mMission.mParameters[0]), int.Parse(MissionState.mMission.mParameters[1])) ? ExecuteState.ExecuteSuccessed : ExecuteState.ExecuteFailed;
						}
						else if (MissionState.mMission.mParameters.Length == 3)
						{
							result = IsVehicleArrived(VehicleInfo, int.Parse(MissionState.mMission.mParameters[0]), int.Parse(MissionState.mMission.mParameters[1]), int.Parse(MissionState.mMission.mParameters[2])) ? ExecuteState.ExecuteSuccessed : ExecuteState.ExecuteFailed;
						}
						break;
					case MissionType.Dock:
						result = ExecuteState.ExecuteFailed;
						break;
				}
			}
			else if (VehicleInfo.mCurrentState == "Running")
			{
				result = ExecuteState.Executing;
			}
			return result;
		}
		private bool IsVehicleError(IVehicleInfo VehicleInfo)
		{
			bool result = false;
			switch (VehicleInfo.mCurrentState)
			{
				case "RouteNotFind":
				case "ObstacleExists":
				case "BumperTrigger":
					result = true;
					break;
				default:
					result = false;
					break;
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
