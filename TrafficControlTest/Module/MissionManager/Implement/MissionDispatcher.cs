using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class MissionDispatcher : IMissionDispatcher
	{
		public event EventHandlerDateTime SystemStarted;
		public event EventHandlerDateTime SystemStopped;

		public bool mIsExecuting { get { return (mThdDispatchMission != null && mThdDispatchMission.IsAlive == true) ? true : false; } }

		private IMissionStateManager rMissionStateManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private Thread mThdDispatchMission = null;
		private bool[] mThdDispatchMissionExitFlag = null;

		public MissionDispatcher(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			Set(MissionStateManager, VehicleInfoManager, VehicleCommunicator);
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
		public void Set(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			Set(MissionStateManager);
			Set(VehicleInfoManager);
			Set(VehicleCommunicator);
		}
		public void Start()
		{
			InitializeThread();
		}
		public void Stop()
		{
			DestroyThread();
		}

		protected virtual void RaiseEvent_SystemStarted(bool Sync = true)
		{
			if (Sync)
			{
				SystemStarted?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStarted?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_SystemStopped(bool Sync = true)
		{
			if (Sync)
			{
				SystemStopped?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStopped?.Invoke(DateTime.Now); });
			}
		}
		private void InitializeThread()
		{
			mThdDispatchMissionExitFlag = new bool[] { false };
			mThdDispatchMission = new Thread(() => Task_DispatchMission(mThdDispatchMissionExitFlag));
			mThdDispatchMission.IsBackground = true;
			mThdDispatchMission.Start();
		}
		private void DestroyThread()
		{
			if (mThdDispatchMission != null)
			{
				if (mThdDispatchMission.IsAlive)
				{
					mThdDispatchMissionExitFlag[0] = true;
				}
				mThdDispatchMission = null;
			}
		}
		private void Task_DispatchMission(bool[] ExitFlag)
		{
			try
			{
				RaiseEvent_SystemStarted();
				while (!ExitFlag[0])
				{
					Subtask_DispatchMission();
					Thread.Sleep(1000);
				}
			}
			finally
			{
				RaiseEvent_SystemStopped();
			}
		}
		private void Subtask_DispatchMission()
		{
			List<IMissionState> executableMissions = ExtractExecutableMissions(rMissionStateManager, rVehicleInfoManager);
			if (executableMissions != null && executableMissions.Count > 0)
			{
				IMissionState mission = executableMissions.OrderBy(o => o.mMission.mPriority).ThenBy(o => o.mReceivedTimestamp).First();
				string vehicleId = string.IsNullOrEmpty(mission.mMission.mVehicleId) ? rVehicleInfoManager.GetItems().FirstOrDefault(o => o.mCurrentState == "Idle" && o.mCurrentStateDuration.TotalSeconds > 1 && string.IsNullOrEmpty(o.mCurrentMissionId))?.mName : mission.mMission.mVehicleId;
				if (!string.IsNullOrEmpty(vehicleId))
				{
					mission.UpdateSendState(Interface.SendState.Sending);
					mission.UpdateExecutorId(vehicleId);
					SendMission(vehicleId, mission.mMission);
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
					rVehicleCommunicator.SendSerializableData_Goto(IpPort, Mission.mParameters[0]);
					break;
				case MissionType.GotoPoint:
					if (Mission.mParameters.Length == 2)
					{
						rVehicleCommunicator.SendSerializableData_GotoPoint(IpPort, int.Parse(Mission.mParameters[0]), int.Parse(Mission.mParameters[1]));
					}
					else if (Mission.mParameters.Length == 3)
					{
						rVehicleCommunicator.SendSerializableData_GotoTowardPoint(IpPort, int.Parse(Mission.mParameters[0]), int.Parse(Mission.mParameters[1]), int.Parse(Mission.mParameters[2]));
					}
					break;
				case MissionType.Dock:
					rVehicleCommunicator.SendSerializableData_Dock(IpPort);
					break;
			}
		}
		private static List<IMissionState> ExtractExecutableMissions(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager)
		{
			if (MissionStateManager == null || MissionStateManager.mCount == 0) return null;
			if (VehicleInfoManager == null || VehicleInfoManager.mCount == 0) return null;

			List<IMissionState> result = null;
			result = MissionStateManager.GetItems().Where(o => (o.mSendState == Interface.SendState.Unsend && o.mExecuteState == ExecuteState.Unexecute) && ((string.IsNullOrEmpty(o.mMission.mVehicleId)) || (!string.IsNullOrEmpty(o.mMission.mVehicleId) && VehicleInfoManager[o.mMission.mVehicleId] != null && VehicleInfoManager[o.mMission.mVehicleId].mCurrentState == "Idle"))).ToList();
			return result;
		}
	}
}
