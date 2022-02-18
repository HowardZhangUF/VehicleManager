using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.ChargeStation;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Mission
{
	/// <summary>
	/// DispatchRule 0: Priority > ReceivedTime
	/// DispatchRule 1: SpecifyVehicle > Priority > ReceivedTime (for Thinflex)
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
			List<IMissionState> executableMissions = ExtractExecutableMissions(rMissionStateManager, executableVehicles);
			result += $"DispatchRule: {mDispatchRule}, IdlePeriodThreshold: {mIdlePeriodThreshold}";
			result += $", CountOfExecutableVehicle: {executableVehicles.Count}";
			if (executableVehicles.Count > 0)
			{
				result += ", ";
				result += string.Join(", ", executableVehicles.Select(o => $"{o.mName}-{o.mCurrentState}"));
			}
			result += ", ";
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
			// MissionType :Goto, GotoPoint, Dock, Abort
			List<IVehicleInfo> idleVehicles = ExtractExecutableVehicles(rVehicleInfoManager, rVehicleControlManager, mIdlePeriodThreshold);
			List<IMissionState> executableMissions = OrderMissionCollection(ExtractExecutableMissions(rMissionStateManager, idleVehicles), mDispatchRule);
			if (executableMissions != null && executableMissions.Count > 0)
			{
				IMissionState mission = executableMissions.First();
				switch (mission.mMission.mMissionType)
				{
					case MissionType.Abort:
						DispatchMission(string.Empty, mission);
						break;
					case MissionType.Goto:
					case MissionType.GotoPoint:
					case MissionType.Dock:
						if (idleVehicles != null && idleVehicles.Count > 0)
						{
							string vehicleId = string.IsNullOrEmpty(mission.mMission.mVehicleId) ? idleVehicles.First().mName : mission.mMission.mVehicleId;
							DispatchMission(vehicleId, mission);
						}
						break;
				}
			}
		}
		/// <summary>根據任務的資訊來產生 VehicleControl 並加入至 VehicleControlManager 中</summary>
		private void DispatchMission(string VehicleId, IMissionState MissionState)
		{
			switch (MissionState.mMission.mMissionType)
			{
				case MissionType.Goto:
					// 如果自走車當前在充電站上，則產生一解除充電的指令
					if (rVehicleInfoManager[VehicleId].mCurrentState == "ChargeIdle")
					{
						IVehicleControl tmpControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.Uncharge, null, MissionState.mName, string.Empty);
						rVehicleControlManager.Add(tmpControl.mName, tmpControl);
						System.Threading.Thread.Sleep(100);
					}
					MissionState.UpdateExecutorId(VehicleId);
					IVehicleControl tmpGotoControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.Goto, MissionState.mMission.mParameters, MissionState.mName, string.Empty);
					rVehicleControlManager.Add(tmpGotoControl.mName, tmpGotoControl);
					break;
				case MissionType.GotoPoint:
					// 如果自走車當前在充電站上，則產生一解除充電的指令
					if (rVehicleInfoManager[VehicleId].mCurrentState == "ChargeIdle")
					{
						IVehicleControl tmpControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.Uncharge, null, MissionState.mName, string.Empty);
						rVehicleControlManager.Add(tmpControl.mName, tmpControl);
						System.Threading.Thread.Sleep(100);
					}
					MissionState.UpdateExecutorId(VehicleId);
					if (MissionState.mMission.mParameters.Length == 2)
					{
						IVehicleControl tmpGotoPointControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.GotoPoint, MissionState.mMission.mParameters, MissionState.mName, string.Empty);
						rVehicleControlManager.Add(tmpGotoPointControl.mName, tmpGotoPointControl);
					}
					else if (MissionState.mMission.mParameters.Length == 3)
					{
						IVehicleControl tmpGotoTowardPointControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.GotoTowardPoint, MissionState.mMission.mParameters, MissionState.mName, string.Empty);
						rVehicleControlManager.Add(tmpGotoTowardPointControl.mName, tmpGotoTowardPointControl);
					}
					break;
				case MissionType.Dock:
					// 如果自走車當前在充電站上，則產生一解除充電的指令
					if (rVehicleInfoManager[VehicleId].mCurrentState == "ChargeIdle")
					{
						IVehicleControl tmpControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.Uncharge, null, MissionState.mName, string.Empty);
						rVehicleControlManager.Add(tmpControl.mName, tmpControl);
						System.Threading.Thread.Sleep(100);
					}
					MissionState.UpdateExecutorId(VehicleId);
					IVehicleControl tmpChargeControl = Library.Library.GenerateIVehicleControl(VehicleId, Command.Charge, MissionState.mMission.mParameters, MissionState.mName, string.Empty);
					rVehicleControlManager.Add(tmpChargeControl.mName, tmpChargeControl);
					break;
				case MissionType.Abort:
					// Abort 所帶的任務識別碼參數，可能會是「客戶自訂的任務識別碼」或是「系統自動產生的任務識別碼」
					IMissionState abortMissionState = rMissionStateManager.GetItems().FirstOrDefault(o => o.mName == MissionState.mMission.mParameters.First() || o.mMission.mMissionId == MissionState.mMission.mParameters.First());
					// 如果欲終止的任務存在
					if (abortMissionState != null)
					{
						// 如果欲終止的任務已經在執行
						if (!string.IsNullOrEmpty(abortMissionState.mExecutorId))
						{
							// 在產生 Abort 的 VehicleControl 時， CauseId 會填入對應的 IMissionState 的 Name 資訊
							MissionState.UpdateExecutorId(abortMissionState.mExecutorId);
							IVehicleControl tmpAbortControl = Library.Library.GenerateIVehicleControl(abortMissionState.mExecutorId, Command.Abort, null, MissionState.mName, string.Empty);
							rVehicleControlManager.Add(tmpAbortControl.mName, tmpAbortControl);
						}
						// 如果欲終止的任務尚未執行
						else
						{
							// 欲終止的任務標記為執行失敗
							abortMissionState.UpdateFailedReason(FailedReason.CancelByHostCommand);
							abortMissionState.UpdateExecuteState(ExecuteState.ExecuteFailed);
							// Abort 任務標記為執行成功
							MissionState.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
						}
					}
					// 如果欲終止的任務不存在
					else
					{
						MissionState.UpdateFailedReason(FailedReason.ObjectNotExist);
						MissionState.UpdateExecuteState(ExecuteState.ExecuteFailed);
					}
					break;
			}
		}

		/// <summary>擷取可工作的車輛清單(狀態為閒置或充電閒置 && 錯誤訊息為空或一般 && 狀態持續時間大於一定秒數 && 當前任務識別碼為空 && 沒有被派送控制的車輛)</summary>
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
			return resultVehicles;
		}
		/// <summary>擷取可執行的任務清單(狀態為尚未執行 && 執行者為空 && (沒有指定自走車 || 指定的自走車在可工作車輛清單內))</summary>
		private static List<IMissionState> ExtractExecutableMissions(IMissionStateManager MissionStateManager, List<IVehicleInfo> ExecutableVehicles)
		{
			if (MissionStateManager == null || MissionStateManager.mCount == 0)
			{
				return new List<IMissionState>();
			}
			else
			{
				return MissionStateManager.GetItems().Where(o => o.mExecuteState == ExecuteState.Unexecute && string.IsNullOrEmpty(o.mExecutorId) && ((string.IsNullOrEmpty(o.mMission.mVehicleId)) || IsContains(ExecutableVehicles, o.mMission.mVehicleId))).ToList();
			}
		}
		/// <summary>查詢指定車輛資訊集合中有沒有包含指定車輛</summary>
		private static bool IsContains(List<IVehicleInfo> VehicleInfoCollection, string VehicleName)
		{
			return VehicleInfoCollection.Any(o => o.mName == VehicleName);
		}
		/// <summary>根據指定規則排序任務集合</summary>
		private static List<IMissionState> OrderMissionCollection(List<IMissionState> MissionStates, int OrderRule)
		{
			switch (OrderRule)
			{
				case 0:
					return OrderMissionCollectionWithRule0(MissionStates);
				case 1:
					return OrderMissionCollectionWithRule1(MissionStates);
				default:
					return OrderMissionCollectionWithRule0(MissionStates);
			}
		}
		/// <summary>排序任務集合。 OrderRule 0: Priority > ReceivedTime</summary>
		private static List<IMissionState> OrderMissionCollectionWithRule0(List<IMissionState> MissionStates)
		{
			return MissionStates.OrderBy(o => o.mMission.mPriority).ThenBy(o => o.mReceivedTimestamp).ToList();
		}
		/// <summary>排序任務集合。 OrderRule 1: SpecifyVehicle > Priority > ReceivedTime (for Thinflex)</summary>
		private static List<IMissionState> OrderMissionCollectionWithRule1(List<IMissionState> MissionStates)
		{
			List<IMissionState> result = new List<IMissionState>();
			if (MissionStates.Count > 0)
			{
				List<IMissionState> specifyVehicleMissions = MissionStates.Where(o => !string.IsNullOrEmpty(o.mMission.mVehicleId)).OrderBy(o => o.mMission.mPriority).ThenBy(o => o.mReceivedTimestamp).ToList();
				List<IMissionState> unsepcifyVehcileMissions = MissionStates.Where(o => string.IsNullOrEmpty(o.mMission.mVehicleId)).OrderBy(o => o.mMission.mPriority).ThenBy(o => o.mReceivedTimestamp).ToList();
				result.AddRange(specifyVehicleMissions);
				result.AddRange(unsepcifyVehcileMissions);
			}
			return result;
		}
	}
}
