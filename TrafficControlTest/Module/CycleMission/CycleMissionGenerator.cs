using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.General.Implement;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CycleMission
{
	public class CycleMissionGenerator : SystemWithLoopTask, ICycleMissionGenerator
	{
		public event EventHandlerCycleMissionAssigned CycleMissionAssigned;
		public event EventHandlerCycleMissionRemoved CycleMissionRemoved;
		public event EventHandlerCycleMissionIndexUpdated CycleMissionIndexUpdated;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMissionStateManager rMissionStateManager = null;
		private List<string> mCollectionOfVehicleId = new List<string>();
		private Dictionary<string, string[]> mCollectionOfMissionList = new Dictionary<string, string[]>();
		private Dictionary<string, int> mCollectionOfCurrentMissionIndex = new Dictionary<string, int>();

		public CycleMissionGenerator(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager)
		{
			Set(VehicleInfoManager, MissionStateManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			rVehicleInfoManager = VehicleInfoManager;
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			rMissionStateManager = MissionStateManager;
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager)
		{
			Set(VehicleInfoManager);
			Set(MissionStateManager);
		}
		public void AssignCycleMission(string VehicleId, string[] Targets, int StartIndex = 0)
		{
			mCollectionOfVehicleId.Add(VehicleId);
			mCollectionOfMissionList.Add(VehicleId, Targets);
			mCollectionOfCurrentMissionIndex.Add(VehicleId, StartIndex == 0 ? Targets.Length - 1 : StartIndex - 1);
			RaiseEvent_CycleMissionAssigned(VehicleId);
		}
		public void RemoveCycleMission(string VehicleId)
		{
			mCollectionOfVehicleId.Remove(VehicleId);
			mCollectionOfMissionList.Remove(VehicleId);
			mCollectionOfCurrentMissionIndex.Remove(VehicleId);
			RaiseEvent_CycleMissionRemoved(VehicleId);
		}
		public bool GetAssigned(string VehicleId)
		{
			return mCollectionOfVehicleId.Contains(VehicleId);
		}
		public string[] GetMissionList(string VehicleId)
		{
			return mCollectionOfVehicleId.Contains(VehicleId) ? mCollectionOfMissionList[VehicleId] : null;
		}
		public int GetCurrentMissionIndex(string VehicleId)
		{
			return mCollectionOfVehicleId.Contains(VehicleId) ? mCollectionOfCurrentMissionIndex[VehicleId] : -1;
		}
		public override void Task()
		{
			Subtask_GenerateMission();
		}

		protected virtual void RaiseEvent_CycleMissionAssigned(string VehicleId, bool Sync = true)
		{
			if (Sync)
			{
				CycleMissionAssigned?.Invoke(DateTime.Now, VehicleId);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { CycleMissionAssigned?.Invoke(DateTime.Now, VehicleId); });
			}
		}
		protected virtual void RaiseEvent_CycleMissionRemoved(string VehicleId, bool Sync = true)
		{
			if (Sync)
			{
				CycleMissionRemoved?.Invoke(DateTime.Now, VehicleId);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { CycleMissionRemoved?.Invoke(DateTime.Now, VehicleId); });
			}
		}
		protected virtual void RaiseEvent_CycleMissionIndexUpdated(string VehicleId, int Index, bool Sync = true)
		{
			if (Sync)
			{
				CycleMissionIndexUpdated?.Invoke(DateTime.Now, VehicleId, Index);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { CycleMissionIndexUpdated?.Invoke(DateTime.Now, VehicleId, Index); });
			}
		}
		private void Subtask_GenerateMission()
		{
			for (int i = 0; i < mCollectionOfVehicleId.Count; ++i)
			{
				if (rVehicleInfoManager.GetItem(mCollectionOfVehicleId[i]).mCurrentState == "Idle" && rVehicleInfoManager.GetItem(mCollectionOfVehicleId[i]).mCurrentStateDuration.TotalSeconds > 1)
				{
					mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]] = mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]] == mCollectionOfMissionList[mCollectionOfVehicleId[i]].Length - 1 ? 0 : mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]] + 1;
					IMissionState missionState = GenerateIMissionState(mCollectionOfVehicleId[i], mCollectionOfMissionList[mCollectionOfVehicleId[i]][mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]]]);
					rMissionStateManager.Add(missionState.mName, missionState);
					RaiseEvent_CycleMissionIndexUpdated(mCollectionOfVehicleId[i], mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]]);
				}
			}
		}
		private IMissionState GenerateIMissionState(string VehicleId, string Target)
		{
			IMission tmp = GenerateIMission(VehicleId, Target);
			if (tmp != null)
			{
				return Library.Library.GenerateIMissionState(tmp);
			}
			else
			{
				return null;
			}
		}
		private IMission GenerateIMission(string VehicleId, string Target)
		{
			IMission result = null;
			string[] tmpStr = Target.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			switch (tmpStr.Length)
			{
				case 1:
					result = Library.Library.GenerateIMission(Library.MissionType.Goto, $"CycleFor{VehicleId}", 50, VehicleId, tmpStr);
					break;
				case 2:
				case 3:
					result = Library.Library.GenerateIMission(Library.MissionType.GotoPoint, $"CycleFor{VehicleId}", 50, VehicleId, tmpStr);
					break;
				default:
					break;
			}
			return result;
		}
	}
}
