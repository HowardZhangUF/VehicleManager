using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.ChargeStation;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.InterveneCommand;
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
		private IVehicleControlManager rVehicleControlManager = null;

		public MissionDispatcher(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager, IVehicleControlManager VehicleControlManager)
		{
			Set(MissionStateManager, VehicleInfoManager, VehicleControlManager);
		} 
		public void Set(IMissionStateManager MissionStateManager)
		{
			rMissionStateManager = MissionStateManager;
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			rVehicleInfoManager = VehicleInfoManager;
		}
		public void Set(IVehicleControlManager VehicleControlManager)
		{
			rVehicleControlManager = VehicleControlManager;
		}
		public void Set(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager, IVehicleControlManager VehicleControlManager)
		{
			Set(MissionStateManager);
			Set(VehicleInfoManager);
			Set(VehicleControlManager);
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "TimePeriod", "DispatchRule", "IdlePeriodThreshold" };
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
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			List<IVehicleInfo> executableVehicles = ExtractExecutableVehicles(rVehicleInfoManager, rVehicleControlManager, mIdlePeriodThreshold);
			List<IMissionState> executableMissions = ExtractExecutableMissions(rMissionStateManager, rVehicleInfoManager, mIdlePeriodThreshold);
			result += $"DispatchRule: {mDispatchRule}, IdlePeriodThreshold: {mIdlePeriodThreshold}";
			result += $", CountOfExecutableVehicle: {executableVehicles.Count}";
			if (executableVehicles.Count > 0)
			{
				result += ", ";
				result += string.Join(", ", executableVehicles.Select(o => $"{o.mName}-{o.mCurrentState}"));
			}
			result += $", CountOfExecuableMission: {executableMissions.Count}";
			if (executableMissions.Count > 0)
			{
				result += ", ";
				result += string.Join(", ", executableMissions.Select(o => $"{o.mName}-{o.mMission.mMissionType.ToString()}-{o.mExecuteState.ToString()}"));
			}
			return result;
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
            List<IMissionState> executableMissions = ExtractExecutableMissions(rMissionStateManager, rVehicleInfoManager, mIdlePeriodThreshold);
            List<IVehicleInfo> executableVehicles = ExtractExecutableVehicles(rVehicleInfoManager, rVehicleControlManager, mIdlePeriodThreshold);
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
					if (rVehicleInfoManager[vehicleId].mCurrentState == "ChargeIdle")
					{
						IVehicleControl tmpControl = Library.Library.GenerateIVehicleControl(vehicleId, Command.Uncharge, null, mission.mName, string.Empty);
						rVehicleControlManager.Add(tmpControl.mName, tmpControl);
					}
					mission.UpdateExecutorId(vehicleId);
					SendMission(vehicleId, mission.mMission, mission.mName);
					RaiseEvent_MissionDispatched(mission, rVehicleInfoManager.GetItem(vehicleId));
				}
			}
		}
		private void Subtask_DispatchMission_1()
		{
			List<IMissionState> executableMissions = ExtractExecutableMissions(rMissionStateManager, rVehicleInfoManager, mIdlePeriodThreshold);
			List<IVehicleInfo> executableVehicles = ExtractExecutableVehicles(rVehicleInfoManager, rVehicleControlManager, mIdlePeriodThreshold);
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
					if (rVehicleInfoManager[vehicleId].mCurrentState == "ChargeIdle")
					{
						IVehicleControl tmpControl = Library.Library.GenerateIVehicleControl(vehicleId, Command.Uncharge, null, mission.mName, string.Empty);
						rVehicleControlManager.Add(tmpControl.mName, tmpControl);
					}
					mission.UpdateExecutorId(vehicleId);
					SendMission(vehicleId, mission.mMission, mission.mName);
					RaiseEvent_MissionDispatched(mission, rVehicleInfoManager.GetItem(vehicleId));
				}
			}
		}
		private void SendMission(string VehicleId, IMission Mission, string MissionId)
		{
			switch (Mission.mMissionType)
			{
				case MissionType.Goto:
					IVehicleControl tmpGotoControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.Goto, Mission.mParameters, MissionId, string.Empty);
					rVehicleControlManager.Add(tmpGotoControl.mName, tmpGotoControl);
					break;
				case MissionType.GotoPoint:
					if (Mission.mParameters.Length == 2)
					{
						IVehicleControl tmpGotoPointControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.GotoPoint, Mission.mParameters, MissionId, string.Empty);
						rVehicleControlManager.Add(tmpGotoPointControl.mName, tmpGotoPointControl);
					}
					else if (Mission.mParameters.Length == 3)
					{
						IVehicleControl tmpGotoTowardPointControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.GotoTowardPoint, Mission.mParameters, MissionId, string.Empty);
						rVehicleControlManager.Add(tmpGotoTowardPointControl.mName, tmpGotoTowardPointControl);
					}
					break;
				case MissionType.Dock:
					IVehicleControl tmpChargeControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.Charge, Mission.mParameters, MissionId, string.Empty);
					rVehicleControlManager.Add(tmpChargeControl.mName, tmpChargeControl);
					break;
			}
		}
		private static List<IMissionState> ExtractExecutableMissions(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager, int IdlePeriodThreshold)
		{
			if (MissionStateManager == null || MissionStateManager.mCount == 0) return null;
			if (VehicleInfoManager == null || VehicleInfoManager.mCount == 0) return null;

			List<IMissionState> result = null;
			result = MissionStateManager.GetItems().Where(o => o.mExecuteState == ExecuteState.Unexecute && string.IsNullOrEmpty(o.mExecutorId) && ((string.IsNullOrEmpty(o.mMission.mVehicleId)) || (!string.IsNullOrEmpty(o.mMission.mVehicleId) && VehicleInfoManager[o.mMission.mVehicleId] != null && (VehicleInfoManager[o.mMission.mVehicleId].mCurrentState == "Idle" || VehicleInfoManager[o.mMission.mVehicleId].mCurrentState == "ChargeIdle") && (string.IsNullOrEmpty(VehicleInfoManager[o.mMission.mVehicleId].mErrorMessage) || VehicleInfoManager[o.mMission.mVehicleId].mErrorMessage == "Normal") && VehicleInfoManager[o.mMission.mVehicleId].mCurrentStateDuration.TotalMilliseconds > IdlePeriodThreshold && string.IsNullOrEmpty(VehicleInfoManager[o.mMission.mVehicleId].mCurrentMissionId)))).ToList();
			return result;
		}
        private static List<IVehicleInfo> ExtractExecutableVehicles(IVehicleInfoManager VehicleInfoManager, IVehicleControlManager VehicleControlManager, int IdlePeriodThreshold)
        {
            IEnumerable<IVehicleControl> schedulingControls = VehicleControlManager.GetItems().Where(o => o.mSendState == SendState.Sending || o.mExecuteState == ExecuteState.Executing || o.mExecuteState == ExecuteState.ExecutePaused);
            IEnumerable<IVehicleInfo> idleVehicles = VehicleInfoManager.GetItems().Where(o => (o.mCurrentState == "Idle" || o.mCurrentState == "ChargeIdle") && (string.IsNullOrEmpty(o.mErrorMessage) || o.mErrorMessage == "Normal") && o.mCurrentStateDuration.TotalMilliseconds > IdlePeriodThreshold && string.IsNullOrEmpty(o.mCurrentMissionId));
            List<IVehicleInfo> resultVehicles = new List<IVehicleInfo>();
            if (idleVehicles != null && idleVehicles.Count() > 0)
            {
                foreach (IVehicleInfo vehicle in idleVehicles)
                {
                    if (!schedulingControls.Any(o => o.mVehicleId == vehicle.mName))
                    {
                        resultVehicles.Add(vehicle);
                    }
                }
            }
            // 閒置且沒有被 Scheduling 控制的車
            return resultVehicles;
        }
	}
}
