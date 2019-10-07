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
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleCommunicator;

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
			UnsubscribeEvent_IVehicleInfoManager(VehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(VehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
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
		// 未完成，檢查任務完成的條件太隨便
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
							if (VehicleInfo.mState == "Idle")
							{
								missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
							}
							break;
						case "GotoPoint":
							if (VehicleInfo.mState == "Idle")
							{
								missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
							}
							break;
						case "Dock":
							if (VehicleInfo.mState == "Docked")
							{
								missionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
							}
							break;
					}
				}
			}
		}
		// 未完成，這個感覺不應該放在這個 Class 裡面
		private void HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data)
		{
			string missionId = GetMissionId(IpPort, Data);
			if (!string.IsNullOrEmpty(missionId))
			{
				rMissionStateManager.Get(missionId).UpdateSendState(Interface.SendState.SendSuccessed);
				rMissionStateManager.Get(missionId).UpdateExecuteState(ExecuteState.Executing);
			}
		}
		// 未完成，這個感覺不應該放在這個 Class 裡面
		private void HandleEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data)
		{
			string missionId = GetMissionId(IpPort, Data);
			if (!string.IsNullOrEmpty(missionId))
			{
				rMissionStateManager.Get(missionId).UpdateSendState(Interface.SendState.SendFailed);
				rMissionStateManager.Get(missionId).UpdateExecuteState(ExecuteState.Unexecute);
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
				IMissionState mission = executableMissions.OrderBy(o => o.mMission.mPriority).First();
				string vehicleId = string.IsNullOrEmpty(mission.mMission.mVehicleId) ? rVehicleInfoManager.GetList().First(o => o.mState == "Idle").mName : mission.mMission.mVehicleId;
				SendMission(vehicleId, mission.mMission);
				mission.UpdateSendState(Interface.SendState.Sending);
				mission.UpdateExecutorId(vehicleId);
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
				case "Goto":
					rVehicleCommunicator.SendSerializableData_Goto(IpPort, Mission.mParameters[0]);
					break;
				case "GotoPoint":
					if (Mission.mParameters.Length == 2)
					{
						rVehicleCommunicator.SendSerializableData_GotoPoint(IpPort, int.Parse(Mission.mParameters[0]), int.Parse(Mission.mParameters[1]));
					}
					else if (Mission.mParameters.Length == 3)
					{
						rVehicleCommunicator.SendSerializableData_GotoTowardPoint(IpPort, int.Parse(Mission.mParameters[0]), int.Parse(Mission.mParameters[1]), int.Parse(Mission.mParameters[2]));
					}
					break;
				case "Dock":
					rVehicleCommunicator.SendSerializableData_Dock(IpPort);
					break;
			}
		}
		/// <summary>透過 Serializable 物件的來源及其本身判斷是哪個任務的相關訊息並輸出任務識別碼</summary>
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
		private static List<IMissionState> ExtractExecutableMissions(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager)
		{
			if (MissionStateManager == null || MissionStateManager.mCount == 0) return null;
			if (VehicleInfoManager == null || VehicleInfoManager.mCount == 0) return null;

			List<IMissionState> result = null;
			result = MissionStateManager.GetList().Where(o => (o.mSendState == Interface.SendState.Unsend && o.mExecuteState == ExecuteState.Unexecute) && ((string.IsNullOrEmpty(o.mMission.mVehicleId)) || (!string.IsNullOrEmpty(o.mMission.mVehicleId) && VehicleInfoManager[o.mMission.mVehicleId] != null && VehicleInfoManager[o.mMission.mVehicleId].mState == "Idle"))).ToList();
			return result;
		}
	}
}
