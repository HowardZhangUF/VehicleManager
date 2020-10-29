using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Mission
{
	public class MissionDispatcher : SystemWithLoopTask, IMissionDispatcher
	{
		public event EventHandler<MissionDispatchedEventArgs> MissionDispatched;

		private IMissionStateManager rMissionStateManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;

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
		public override void Task()
		{
			Subtask_DispatchMission();
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
		private void Subtask_DispatchMission()
		{
            List<IMissionState> executableMissions = ExtractExecutableMissions(rMissionStateManager, rVehicleInfoManager);
            List<IVehicleInfo> executableVehicles = ExtractExecutableVehicles(rVehicleInfoManager, rMissionStateManager);
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
			result = MissionStateManager.GetItems().Where(o => (o.mSendState == SendState.Unsend && o.mExecuteState == ExecuteState.Unexecute) && ((string.IsNullOrEmpty(o.mMission.mVehicleId)) || (!string.IsNullOrEmpty(o.mMission.mVehicleId) && VehicleInfoManager[o.mMission.mVehicleId] != null && VehicleInfoManager[o.mMission.mVehicleId].mCurrentState == "Idle"))).ToList();
			return result;
		}
        private static List<IVehicleInfo> ExtractExecutableVehicles(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager)
        {
            IEnumerable<IMissionState> sendingAndExecutingMissions = MissionStateManager.GetItems().Where(o => o.mSendState == SendState.Sending || o.mExecuteState == ExecuteState.Executing);
            IEnumerable<IVehicleInfo> idleVehicles = VehicleInfoManager.GetItems().Where(o => o.mCurrentState == "Idle" && o.mCurrentStateDuration.TotalSeconds > 1 && string.IsNullOrEmpty(o.mCurrentMissionId));
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
