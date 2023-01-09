using LibraryForVM;
using System;
using System.Collections.Generic;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.CycleMission
{
	public class CycleMissionGenerator : SystemWithLoopTask, ICycleMissionGenerator
	{
		public event EventHandler<CycleMissionAssignedEventArgs> CycleMissionAssigned;
		public event EventHandler<CycleMissionUnassignedEventArgs> CycleMissionUnassigned;
		public event EventHandler<CycleMissionExecutedIndexChangedEventArgs> CycleMissionExecutedIndexChanged;

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
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
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
			RaiseEvent_CycleMissionAssigned(VehicleId, mCollectionOfMissionList[VehicleId]);
		}
		public void UnassignCycleMission(string VehicleId)
		{
			mCollectionOfVehicleId.Remove(VehicleId);
			mCollectionOfMissionList.Remove(VehicleId);
			mCollectionOfCurrentMissionIndex.Remove(VehicleId);
			RaiseEvent_CycleMissionUnassigned(VehicleId);
		}
		public bool IsAssigned(string VehicleId)
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
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			result += $"CountOfVehicleBeAssignedCycleMission: {mCollectionOfVehicleId.Count}";
			if (mCollectionOfVehicleId.Count > 0)
			{
				foreach (string vehicleId in mCollectionOfVehicleId)
				{
					result += $", Vehicle:{vehicleId}-Count:{mCollectionOfMissionList[vehicleId].Length}-Index:{mCollectionOfCurrentMissionIndex[vehicleId]}";
				}
			}
			return result;
		}
		public override void Task()
		{
			Subtask_GenerateMission();
		}

		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object sender, ItemCountChangedEventArgs<IVehicleInfo> e)
		{
			if (IsAssigned(e.ItemName))
			{
				UnassignCycleMission(e.ItemName);
			}
		}
		protected virtual void RaiseEvent_CycleMissionAssigned(string VehicleId, string[] Missions, bool Sync = true)
		{
			if (Sync)
			{
				CycleMissionAssigned?.Invoke(this, new CycleMissionAssignedEventArgs(DateTime.Now, VehicleId, Missions));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { CycleMissionAssigned?.Invoke(this, new CycleMissionAssignedEventArgs(DateTime.Now, VehicleId, Missions)); });
			}
		}
		protected virtual void RaiseEvent_CycleMissionUnassigned(string VehicleId, bool Sync = true)
		{
			if (Sync)
			{
				CycleMissionUnassigned?.Invoke(this, new CycleMissionUnassignedEventArgs(DateTime.Now, VehicleId));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { CycleMissionUnassigned?.Invoke(this, new CycleMissionUnassignedEventArgs(DateTime.Now, VehicleId)); });
			}
		}
		protected virtual void RaiseEvent_CycleMissionExecutedIndexChanged(string VehicleId, int Index, bool Sync = true)
		{
			if (Sync)
			{
				CycleMissionExecutedIndexChanged?.Invoke(this, new CycleMissionExecutedIndexChangedEventArgs(DateTime.Now, VehicleId, Index));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { CycleMissionExecutedIndexChanged?.Invoke(this, new CycleMissionExecutedIndexChangedEventArgs(DateTime.Now, VehicleId, Index)); });
			}
		}
		private void Subtask_GenerateMission()
		{
			for (int i = 0; i < mCollectionOfVehicleId.Count; ++i)
			{
				// 如果自走車閒置，且大於 1 秒，且任務集合中還沒有任務是派給該車的(從 MissionID 去判斷)
				if ((rVehicleInfoManager.GetItem(mCollectionOfVehicleId[i]).mCurrentState == "Idle" || rVehicleInfoManager.GetItem(mCollectionOfVehicleId[i]).mCurrentState == "ChargeIdle") && rVehicleInfoManager.GetItem(mCollectionOfVehicleId[i]).mCurrentStateDuration.TotalSeconds > 1 && !rMissionStateManager.IsExistByHostMissionId(GenerateHostMissionId(mCollectionOfVehicleId[i])))
				{
					mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]] = mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]] == mCollectionOfMissionList[mCollectionOfVehicleId[i]].Length - 1 ? 0 : mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]] + 1;
					IMissionState missionState = GenerateIMissionState(mCollectionOfVehicleId[i], mCollectionOfMissionList[mCollectionOfVehicleId[i]][mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]]]);
					rMissionStateManager.Add(missionState.mName, missionState);
					RaiseEvent_CycleMissionExecutedIndexChanged(mCollectionOfVehicleId[i], mCollectionOfCurrentMissionIndex[mCollectionOfVehicleId[i]]);
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
					result = Library.Library.GenerateIMission(Library.MissionType.Goto, GenerateHostMissionId(VehicleId), 50, VehicleId, tmpStr);
					break;
				case 2:
				case 3:
					result = Library.Library.GenerateIMission(Library.MissionType.GotoPoint, GenerateHostMissionId(VehicleId), 50, VehicleId, tmpStr);
					break;
				default:
					break;
			}
			return result;
		}
		private string GenerateHostMissionId(string VehicleId)
		{
			return $"CycleFor{VehicleId}";
		}
	}
}
