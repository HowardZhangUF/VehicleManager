using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
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
				VehicleInfoManager.VehicleStateUpdated += HandleEvent_VehicleInfoManagerVehicleStateUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.VehicleStateUpdated -= HandleEvent_VehicleInfoManagerVehicleStateUpdated;
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
		private void HandleEvent_VehicleInfoManagerVehicleStateUpdated(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			if (rMissionStateManager.mCount > 0)
			{
				IMissionState missionState = rMissionStateManager.GetList().FirstOrDefault(o => o.mExecuteState == ExecuteState.Executing && o.mExecutorId == Name);
				if (missionState != null)
				{
					switch (missionState.mMission.mMissionType)
					{
						case "Goto":
							if (IsVehicleArrived(VehicleInfo, missionState.mMission.mParameters[0]))
							{
								missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
							}
							break;
						case "GotoPoint":
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
						case "Dock":
							if (IsVehicleDocked(VehicleInfo))
							{
								missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
							}
							break;
					}
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data)
		{
			string missionId = GetMissionId(IpPort, Data);
			if (!string.IsNullOrEmpty(missionId))
			{
				rMissionStateManager.Get(missionId).UpdateSendState(Interface.SendState.SendSuccessed);
				rMissionStateManager.Get(missionId).UpdateExecuteState(ExecuteState.Executing);
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data)
		{
			string missionId = GetMissionId(IpPort, Data);
			if (!string.IsNullOrEmpty(missionId))
			{
				rMissionStateManager.Get(missionId).UpdateSendState(Interface.SendState.SendFailed);
				rMissionStateManager.Get(missionId).UpdateExecuteState(ExecuteState.Unexecute);
			}
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, string Target)
		{
			return VehicleInfo.mState == "Idle" && VehicleInfo.mTarget == Target;
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, int X, int Y)
		{
			return VehicleInfo.mState == "Idle" && Math.Abs(VehicleInfo.mPosition.mX - X) < mToleranceOfX && Math.Abs(VehicleInfo.mPosition.mY - Y) < mToleranceOfY;
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, int X, int Y, int Toward)
		{
			return VehicleInfo.mState == "Idle" && Math.Abs(VehicleInfo.mPosition.mX - X) < mToleranceOfX && Math.Abs(VehicleInfo.mPosition.mY - Y) < mToleranceOfY && Math.Abs((int)(VehicleInfo.mToward) - Toward) < mToleranceOfToward;
		}
		private bool IsVehicleDocked(IVehicleInfo VehicleInfo)
		{
			return VehicleInfo.mState == "Docked";
		}
		/// <summary>透過 Serializable 物件的發送來源及其本身判斷是哪個任務的相關訊息並輸出任務識別碼</summary>
		private string GetMissionId(string IpPort, object Data)
		{
			string vehicleId = rVehicleInfoManager.GetByIpPort(IpPort).mName;
			string missionId = null;
			if (Data is GoTo)
			{
				missionId = rMissionStateManager.GetList().First(o => o.mMission.mMissionType == "Goto" && o.mSendState == Interface.SendState.Sending && o.mExecutorId == vehicleId).mMissionId;
			}
			else if (Data is GoToPoint)
			{
				missionId = rMissionStateManager.GetList().First(o => o.mMission.mMissionType == "GotoPoint" && o.mSendState == Interface.SendState.Sending && o.mExecutorId == vehicleId).mMissionId;
			}
			else if (Data is GoToTowardPoint)
			{
				missionId = rMissionStateManager.GetList().First(o => o.mMission.mMissionType == "GotoPoint" && o.mSendState == Interface.SendState.Sending && o.mExecutorId == vehicleId).mMissionId;
			}
			else if (Data is Charge)
			{
				missionId = rMissionStateManager.GetList().First(o => o.mMission.mMissionType == "Dock" && o.mSendState == Interface.SendState.Sending && o.mExecutorId == vehicleId).mMissionId;
			}
			return missionId;
		}
	}
}
