﻿using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.ChargeStation;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Mission
{
	/// <summary>
	/// 0 → Priority > ReceivedTime
	/// 1 → SpecifyVehicle > Priority > ReceivedTime (for Thinflex)
	/// </summary>
	public class MissionDispatcher : SystemWithLoopTask, IMissionDispatcher
	{
		public event EventHandler<MissionDispatchedEventArgs> MissionDispatched;

		public int mDispatchRule { get; private set; } = 0;
		public int mIdlePeriodThreshold { get; private set; } = 1000;

		private IMissionStateManager rMissionStateManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
        private IChargeStationInfoManager rChargeStationInfoManager = null;

		public MissionDispatcher(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator, IChargeStationInfoManager ChargeStationInfoManager)
		{
			Set(MissionStateManager, VehicleInfoManager, VehicleCommunicator, ChargeStationInfoManager);
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			rMissionStateManager = MissionStateManager;
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			rVehicleInfoManager = VehicleInfoManager;
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			rVehicleCommunicator = VehicleCommunicator;
		}
        public void Set(IChargeStationInfoManager ChargeStationInfoManager)
        {
            rChargeStationInfoManager = ChargeStationInfoManager;
        }
		public void Set(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator, IChargeStationInfoManager ChargeStationInfoManager)
		{
			Set(MissionStateManager);
			Set(VehicleInfoManager);
			Set(VehicleCommunicator);
            Set(ChargeStationInfoManager);
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "DispatchRule":
					return mDispatchRule.ToString();
				case "IdlePeriodThreshold":
					return mIdlePeriodThreshold.ToString();
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
				case "DispatchRule":
					mDispatchRule = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "IdlePeriodThreshold":
					mIdlePeriodThreshold = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override void Task()
		{
			switch (mDispatchRule)
			{
				case 0:
					Subtask_DispatchMission_0();
					break;
				case 1:
					Subtask_DispatchMission_1();
					break;
			}
		}

		protected virtual void RaiseEvent_MissionDispatched(IMissionState MissionState, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				MissionDispatched?.Invoke(this, new MissionDispatchedEventArgs(DateTime.Now, MissionState, VehicleInfo));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { MissionDispatched?.Invoke(this, new MissionDispatchedEventArgs(DateTime.Now, MissionState, VehicleInfo)); });
			}
		}
		private void Subtask_DispatchMission_0()
		{
            List<IMissionState> executableMissions = ExtractExecutableMissions(rMissionStateManager, rVehicleInfoManager);
            List<IVehicleInfo> executableVehicles = ExtractExecutableVehicles(rVehicleInfoManager, rMissionStateManager, mIdlePeriodThreshold);
			if (executableMissions != null && executableMissions.Count > 0 && executableVehicles != null && executableVehicles.Count > 0)
			{
				IMissionState mission = executableMissions.OrderBy(o => o.mMission.mPriority).ThenBy(o => o.mReceivedTimestamp).First();
                string vehicleId = string.Empty;
                if (string.IsNullOrEmpty(mission.mMission.mVehicleId))
                {
                    // 如果沒有指定車
                    vehicleId = executableVehicles.First().mName;
                }
                else
                {
                    // 如果該任務有指定車
                    if (executableVehicles.Any(o => o.mName == mission.mMission.mVehicleId))
                    {
                        vehicleId = mission.mMission.mVehicleId;
                    }
                }

				if (!string.IsNullOrEmpty(vehicleId))
				{
					mission.UpdateSendState(SendState.Sending);
					mission.UpdateExecutorId(vehicleId);
					SendMission(vehicleId, mission.mMission);
					RaiseEvent_MissionDispatched(mission, rVehicleInfoManager.GetItem(vehicleId));
				}
			}
		}
		private void Subtask_DispatchMission_1()
		{
			List<IMissionState> executableMissions = ExtractExecutableMissions(rMissionStateManager, rVehicleInfoManager);
			List<IVehicleInfo> executableVehicles = ExtractExecutableVehicles(rVehicleInfoManager, rMissionStateManager, mIdlePeriodThreshold);
			if (executableMissions != null && executableMissions.Count > 0 && executableVehicles != null && executableVehicles.Count > 0)
			{
				IMissionState mission = null;
				string vehicleId = string.Empty;
				List<IMissionState> executableSpecifyVehicleMissions = executableMissions.Where(o => !string.IsNullOrEmpty(o.mMission.mVehicleId)).ToList();
				if (executableSpecifyVehicleMissions != null && executableSpecifyVehicleMissions.Count > 0)
				{
					// 如果任務有指定車的任務，則將其優先派出
					mission = executableSpecifyVehicleMissions.First();
					if (executableVehicles.Any(o => o.mName == mission.mMission.mVehicleId))
					{
						vehicleId = mission.mMission.mVehicleId;
					}
				}
				else
				{
					// 如果任務皆沒有指定車，則派出佇列排序後的第一個任務
					mission = executableMissions.OrderBy(o => o.mMission.mPriority).ThenBy(o => o.mReceivedTimestamp).First();
					vehicleId = executableVehicles.First().mName;
				}

				if (!string.IsNullOrEmpty(vehicleId))
				{
					mission.UpdateSendState(SendState.Sending);
					mission.UpdateExecutorId(vehicleId);
					SendMission(vehicleId, mission.mMission);
					RaiseEvent_MissionDispatched(mission, rVehicleInfoManager.GetItem(vehicleId));
				}
			}
		}
		private void SendMission(string VehicleId, IMission Mission)
		{
			SendMissionByIpPort(rVehicleInfoManager[VehicleId].mIpPort, Mission);
		}
		private void SendMissionByIpPort(string IpPort, IMission Mission)
		{
			switch (Mission.mMissionType)
			{
				case MissionType.Goto:
					rVehicleCommunicator.SendDataOfGoto(IpPort, Mission.mParameters[0]);
					break;
				case MissionType.GotoPoint:
					if (Mission.mParameters.Length == 2)
					{
						rVehicleCommunicator.SendDataOfGotoPoint(IpPort, int.Parse(Mission.mParameters[0]), int.Parse(Mission.mParameters[1]));
					}
					else if (Mission.mParameters.Length == 3)
					{
						rVehicleCommunicator.SendDataOfGotoTowardPoint(IpPort, int.Parse(Mission.mParameters[0]), int.Parse(Mission.mParameters[1]), int.Parse(Mission.mParameters[2]));
					}
					break;
				case MissionType.Dock:
                    IChargeStationInfo tmpChargeStationInfo = GetClosestChargeStationInfo(rVehicleInfoManager.GetItemByIpPort(IpPort), rChargeStationInfoManager);
                    if (tmpChargeStationInfo != null)
                    {
                        rVehicleCommunicator.SendDataOfGoto(IpPort, tmpChargeStationInfo.mName);
                    }
					break;
			}
		}
		private static List<IMissionState> ExtractExecutableMissions(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager)
		{
			if (MissionStateManager == null || MissionStateManager.mCount == 0) return null;
			if (VehicleInfoManager == null || VehicleInfoManager.mCount == 0) return null;

			List<IMissionState> result = null;
			result = MissionStateManager.GetItems().Where(o => (o.mSendState == SendState.Unsend && o.mExecuteState == ExecuteState.Unexecute) && ((string.IsNullOrEmpty(o.mMission.mVehicleId)) || (!string.IsNullOrEmpty(o.mMission.mVehicleId) && VehicleInfoManager[o.mMission.mVehicleId] != null && (VehicleInfoManager[o.mMission.mVehicleId].mCurrentState == "Idle" || VehicleInfoManager[o.mMission.mVehicleId].mCurrentState == "ChargeIdle")))).ToList();
			return result;
		}
        private static List<IVehicleInfo> ExtractExecutableVehicles(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, int IdlePeriodThreshold)
        {
            IEnumerable<IMissionState> sendingAndExecutingMissions = MissionStateManager.GetItems().Where(o => o.mSendState == SendState.Sending || o.mExecuteState == ExecuteState.Executing);
            IEnumerable<IVehicleInfo> idleVehicles = VehicleInfoManager.GetItems().Where(o => (o.mCurrentState == "Idle" || o.mCurrentState == "ChargeIdle") && o.mCurrentStateDuration.TotalMilliseconds > IdlePeriodThreshold && string.IsNullOrEmpty(o.mCurrentMissionId));
            List<IVehicleInfo> resultVehicles = new List<IVehicleInfo>();
            if (idleVehicles != null && idleVehicles.Count() > 0)
            {
                foreach (IVehicleInfo vehicle in idleVehicles)
                {
                    if (!sendingAndExecutingMissions.Any(o => o.mExecutorId == vehicle.mName))
                    {
                        resultVehicles.Add(vehicle);
                    }
                }
            }
            // 閒置且沒有被 Sending 任務的車
            return resultVehicles;
        }
        private static IChargeStationInfo GetClosestChargeStationInfo(IVehicleInfo VehicleInfo, IChargeStationInfoManager ChargeStationInfoManager)
        {
            IChargeStationInfo result = null;
            if (ChargeStationInfoManager != null && ChargeStationInfoManager.mCount > 0)
            {
                result = ChargeStationInfoManager.GetItems().OrderBy(o => CalculateDistance(VehicleInfo.mLocationCoordinate, o.mLocation)).First();
            }
            return result;
        }
        private static int CalculateDistance(IPoint2D Point1, IPoint2D Point2)
        {
            return CalculateDistance(Point1.mX, Point1.mY, Point2.mX, Point2.mY);
        }
        private static int CalculateDistance(int X1, int Y1, int X2, int Y2)
        {
            return (int)Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1));
        }
	}
}
