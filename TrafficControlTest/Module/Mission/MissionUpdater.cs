using SerialData;
using System;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Communication;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Mission
{
	public class MissionUpdater : IMissionUpdater
	{
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMissionStateManager rMissionStateManager = null;
		private IMapManager rMapManager = null;

		public int mToleranceOfX { get; } = 500;
		public int mToleranceOfY { get; } = 500;
		public int mToleranceOfToward { get; } = 5;

		public MissionUpdater(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMapManager MapManager)
		{
			Set(VehicleCommunicator, VehicleInfoManager, MissionStateManager, MapManager);
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
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMapManager MapManager)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
			Set(MissionStateManager);
			Set(MapManager);
		}

		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SentSerializableDataSuccessed += HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
				VehicleCommunicator.SentSerializableDataFailed += HandleEvent_VehicleCommunicatorSentSerializableDataFailed;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SentSerializableDataSuccessed -= HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
				VehicleCommunicator.SentSerializableDataFailed -= HandleEvent_VehicleCommunicatorSentSerializableDataFailed;
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
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			if (!string.IsNullOrEmpty(Item.mCurrentMissionId))
			{
				IMissionState missionState = rMissionStateManager[Item.mCurrentMissionId];
				if (missionState != null)
				{
					missionState.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo VehicleInfo)
		{
			if (rMissionStateManager.mCount > 0 && StateName.Contains("CurrentState") && !string.IsNullOrEmpty(VehicleInfo.mCurrentMissionId) && rMissionStateManager.IsExist(VehicleInfo.mCurrentMissionId))
			{
				IMissionState missionState = rMissionStateManager[VehicleInfo.mCurrentMissionId];
				switch (VehicleInfo.mCurrentState)
				{
					case "RouteNotFind":
					case "ObstacleExists":
					case "BumperTrigger":
						missionState.UpdateExecuteState(ExecuteState.ExecuteFailed);
						break;
					case "Idle":
						switch (missionState.mMission.mMissionType)
						{
							case MissionType.Goto:
								if (IsVehicleArrived(VehicleInfo, missionState.mMission.mParameters[0]))
								{
									missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
								}
								else
								{
									missionState.UpdateExecuteState(ExecuteState.ExecuteFailed);
								}
								break;
							case MissionType.GotoPoint:
								if (missionState.mMission.mParameters.Length == 2)
								{
									if (IsVehicleArrived(VehicleInfo, int.Parse(missionState.mMission.mParameters[0]), int.Parse(missionState.mMission.mParameters[1])))
									{
										missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
									}
									else
									{
										missionState.UpdateExecuteState(ExecuteState.ExecuteFailed);
									}
								}
								else if (missionState.mMission.mParameters.Length == 3)
								{
									if (IsVehicleArrived(VehicleInfo, int.Parse(missionState.mMission.mParameters[0]), int.Parse(missionState.mMission.mParameters[1]), int.Parse(missionState.mMission.mParameters[2])))
									{
										missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
									}
									else
									{
										missionState.UpdateExecuteState(ExecuteState.ExecuteFailed);
									}
								}
								break;
							case MissionType.Dock:
								missionState.UpdateExecuteState(ExecuteState.ExecuteFailed);
								break;
						}
						break;
					case "Charge":
						if (missionState.mMission.mMissionType == MissionType.Dock)
						{
							missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
						}
						break;
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data)
		{
			string missionId = GetMissionId(IpPort, Data);
			if (!string.IsNullOrEmpty(missionId))
			{
				rMissionStateManager.GetItem(missionId).UpdateSendState(SendState.SendSuccessed);
				rMissionStateManager.GetItem(missionId).UpdateExecuteState(ExecuteState.Executing);
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data)
		{
			string missionId = GetMissionId(IpPort, Data);
			if (!string.IsNullOrEmpty(missionId))
			{
				rMissionStateManager.GetItem(missionId).UpdateSendState(SendState.SendFailed);
				rMissionStateManager.GetItem(missionId).UpdateExecuteState(ExecuteState.Unexecute);
			}
		}
		private void HandleEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IMissionState Item)
		{
			if (StateName.Contains("ExecuteState"))
			{
				switch (Item.mExecuteState)
				{
					case ExecuteState.ExecuteSuccessed:
					case ExecuteState.ExecuteFailed:
						rMissionStateManager.Remove(Name);
						break;
				}
			}
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
		private bool IsVehicleDocked(IVehicleInfo VehicleInfo)
		{
			return VehicleInfo.mCurrentState == "Charge";
		}
		/// <summary>透過 Serializable 物件的發送來源及其本身判斷是哪個任務的相關訊息並輸出任務識別碼</summary>
		private string GetMissionId(string IpPort, object Data)
		{
			string vehicleId = rVehicleInfoManager.GetItemByIpPort(IpPort)?.mName;
			string missionId = null;
			if (!string.IsNullOrEmpty(vehicleId))
			{
				if (Data is GoTo)
				{
					GoTo tmpData = Data as GoTo;
					missionId = rMissionStateManager.GetItems().FirstOrDefault(o => (o.mMission.mMissionType == MissionType.Goto || o.mMission.mMissionType == MissionType.Dock) && o.mSendState == SendState.Sending && o.mExecutorId == vehicleId && o.mMission.mParameters[0] == tmpData.Require)?.mName;
				}
				else if (Data is GoToPoint)
				{
					GoToPoint tmpData = Data as GoToPoint;
					missionId = rMissionStateManager.GetItems().FirstOrDefault(o => o.mMission.mMissionType == MissionType.GotoPoint && o.mSendState == SendState.Sending && o.mExecutorId == vehicleId && o.mMission.mParameters[0] == tmpData.Require[0].ToString() && o.mMission.mParameters[1] == tmpData.Require[1].ToString())?.mName;
				}
				else if (Data is GoToTowardPoint)
				{
					GoToTowardPoint tmpData = Data as GoToTowardPoint;
					missionId = rMissionStateManager.GetItems().FirstOrDefault(o => o.mMission.mMissionType == MissionType.GotoPoint && o.mSendState == SendState.Sending && o.mExecutorId == vehicleId && o.mMission.mParameters[0] == tmpData.Require[0].ToString() && o.mMission.mParameters[1] == tmpData.Require[1].ToString() && o.mMission.mParameters[2] == tmpData.Require[2].ToString())?.mName;
				}
			}
			return missionId;
		}
	}
}
