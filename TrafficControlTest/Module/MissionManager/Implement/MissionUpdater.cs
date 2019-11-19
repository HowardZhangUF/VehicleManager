using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class MissionUpdater : IMissionUpdater
	{
		private IMissionStateManager rMissionStateManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;

		public int mToleranceOfX { get; } = 500;
		public int mToleranceOfY { get; } = 500;
		public int mToleranceOfToward { get; } = 5;

		public MissionUpdater(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager)
		{
			Set(VehicleCommunicator, VehicleInfoManager, MissionStateManager);
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
			rMissionStateManager = MissionStateManager;
		}
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
			Set(MissionStateManager);
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
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo VehicleInfo)
		{
			if (rMissionStateManager.mCount > 0 && StateName.Contains("CurrentState") && !string.IsNullOrEmpty(VehicleInfo.mCurrentMissionId) && rMissionStateManager.IsExist(VehicleInfo.mCurrentMissionId))
			{
				IMissionState missionState = rMissionStateManager[VehicleInfo.mCurrentMissionId];
				switch (missionState.mMission.mMissionType)
				{
					case MissionType.Goto:
						if (IsVehicleArrived(VehicleInfo, missionState.mMission.mParameters[0]))
						{
							missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
						}
						break;
					case MissionType.GotoPoint:
						if (missionState.mMission.mParameters.Length == 2)
						{
							if (IsVehicleArrived(VehicleInfo, int.Parse(missionState.mMission.mParameters[0]), int.Parse(missionState.mMission.mParameters[1])))
							{
								missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
							}
						}
						else if (missionState.mMission.mParameters.Length == 3)
						{
							if (IsVehicleArrived(VehicleInfo, int.Parse(missionState.mMission.mParameters[0]), int.Parse(missionState.mMission.mParameters[1]), int.Parse(missionState.mMission.mParameters[2])))
							{
								missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
							}
						}
						break;
					case MissionType.Dock:
						if (IsVehicleDocked(VehicleInfo))
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
				rMissionStateManager.GetItem(missionId).UpdateSendState(Interface.SendState.SendSuccessed);
				rMissionStateManager.GetItem(missionId).UpdateExecuteState(ExecuteState.Executing);
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data)
		{
			string missionId = GetMissionId(IpPort, Data);
			if (!string.IsNullOrEmpty(missionId))
			{
				rMissionStateManager.GetItem(missionId).UpdateSendState(Interface.SendState.SendFailed);
				rMissionStateManager.GetItem(missionId).UpdateExecuteState(ExecuteState.Unexecute);
			}
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, string Target)
		{
			return VehicleInfo.mCurrentState == "Idle" && VehicleInfo.mCurrentTarget == Target;
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, int X, int Y)
		{
			return VehicleInfo.mCurrentState == "Idle" && Math.Abs(VehicleInfo.mLocationCoordinate.mX - X) < mToleranceOfX && Math.Abs(VehicleInfo.mLocationCoordinate.mY - Y) < mToleranceOfY;
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, int X, int Y, int Toward)
		{
			return VehicleInfo.mCurrentState == "Idle" && Math.Abs(VehicleInfo.mLocationCoordinate.mX - X) < mToleranceOfX && Math.Abs(VehicleInfo.mLocationCoordinate.mY - Y) < mToleranceOfY && Math.Abs((int)(VehicleInfo.mLocationToward) - Toward) < mToleranceOfToward;
		}
		private bool IsVehicleDocked(IVehicleInfo VehicleInfo)
		{
			return VehicleInfo.mCurrentState == "Docked";
		}
		/// <summary>透過 Serializable 物件的發送來源及其本身判斷是哪個任務的相關訊息並輸出任務識別碼</summary>
		private string GetMissionId(string IpPort, object Data)
		{
			string vehicleId = rVehicleInfoManager.GetItemByIpPort(IpPort).mName;
			string missionId = null;
			if (Data is GoTo)
			{
				GoTo tmpData = Data as GoTo;
				missionId = rMissionStateManager.GetItems().FirstOrDefault(o => (o.mMission.mMissionType == MissionType.Goto || o.mMission.mMissionType == MissionType.Dock) && o.mSendState == Interface.SendState.Sending && o.mExecutorId == vehicleId && o.mMission.mParameters[0] == tmpData.Require)?.mName;
			}
			else if (Data is GoToPoint)
			{
				GoToPoint tmpData = Data as GoToPoint;
				missionId = rMissionStateManager.GetItems().FirstOrDefault(o => o.mMission.mMissionType == MissionType.GotoPoint && o.mSendState == Interface.SendState.Sending && o.mExecutorId == vehicleId && o.mMission.mParameters[0] == tmpData.Require[0].ToString() && o.mMission.mParameters[1] == tmpData.Require[1].ToString())?.mName;
			}
			else if (Data is GoToTowardPoint)
			{
				GoToTowardPoint tmpData = Data as GoToTowardPoint;
				missionId = rMissionStateManager.GetItems().FirstOrDefault(o => o.mMission.mMissionType == MissionType.GotoPoint && o.mSendState == Interface.SendState.Sending && o.mExecutorId == vehicleId && o.mMission.mParameters[0] == tmpData.Require[0].ToString() && o.mMission.mParameters[1] == tmpData.Require[1].ToString() && o.mMission.mParameters[2] == tmpData.Require[2].ToString())?.mName;
			}
			return missionId;
		}
	}
}
