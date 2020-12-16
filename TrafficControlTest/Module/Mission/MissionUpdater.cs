using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Mission
{
	public class MissionUpdater : SystemWithConfig, IMissionUpdater
	{
		public bool mAutoDetectNonSystemMission { get; private set; } = true;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMissionStateManager rMissionStateManager = null;
		private IVehicleControlManager rVehicleControlManager = null;

		public MissionUpdater(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager)
		{
			Set(VehicleInfoManager, MissionStateManager, VehicleControlManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			UnsubscribeEvent_IMissionStateManager(rMissionStateManager);
			rMissionStateManager = MissionStateManager;
			SubscribeEvent_IMissionStateManager(rMissionStateManager);
		}
		public void Set(IVehicleControlManager VehicleControlManager)
		{
			UnsubscribeEvent_IVehicleControlManager(rVehicleControlManager);
			rVehicleControlManager = VehicleControlManager;
			SubscribeEvent_IVehicleControlManager(rVehicleControlManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager)
		{
			Set(VehicleInfoManager);
			Set(MissionStateManager);
			Set(VehicleControlManager);
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "AutoDetectNonSystemMission":
					return mAutoDetectNonSystemMission.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "AutoDetectNonSystemMission":
					mAutoDetectNonSystemMission = bool.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
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
		private void SubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemUpdated -= HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemUpdated += HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemUpdated -= HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			// 當車子進入執行任務狀態且上一狀態不是干預狀態
			if ((Args.StatusName.Contains("CurrentState") || Args.StatusName.Contains("CurrentTarget")) && Args.Item.mCurrentState == "Running" && !string.IsNullOrEmpty(Args.Item.mCurrentTarget))
			{
				IMissionState correspondingMission = rMissionStateManager.GetItems().FirstOrDefault(o => o.mExecutorId == Args.Item.mName && o.mMission.mParametersString == Args.Item.mCurrentTarget);
				IVehicleControl correspondingControl = rVehicleControlManager.GetItems().FirstOrDefault(o => o.mVehicleId == Args.Item.mName && o.mParametersString == Args.Item.mCurrentTarget);

				if (correspondingMission == null && correspondingControl == null)
				{
					// 代表是自走車自行產生的動作
					if (mAutoDetectNonSystemMission)
					{
						IMissionState tmpMissionState = GenerateIMissionState(Args.Item.mName, Args.Item.mCurrentTarget);
						tmpMissionState.UpdateExecutorId(Args.Item.mName);
						tmpMissionState.UpdateSourceIpPort($"{Args.Item.mName}Self");
						tmpMissionState.UpdateExecuteState(ExecuteState.Executing);
						rMissionStateManager.Add(tmpMissionState.mName, tmpMissionState);

						IVehicleControl tmpControl = GenerateIVehicleControl(Args.Item.mName, Args.Item.mCurrentTarget, tmpMissionState.mName);
						tmpControl.UpdateSendState(SendState.SendSuccessed);
						tmpControl.UpdateExecuteState(ExecuteState.Executing);
						rVehicleControlManager.Add(tmpControl.mName, tmpControl);
					}
				}
				else if (correspondingMission == null && correspondingControl != null)
				{
					// 代表是透過 VM 介面產生的動作
					if (mAutoDetectNonSystemMission)
					{
						IMissionState tmpMissionState = GenerateIMissionState(Args.Item.mName, Args.Item.mCurrentTarget);
						tmpMissionState.UpdateExecutorId(Args.Item.mName);
						tmpMissionState.UpdateSourceIpPort($"VMManual");
						tmpMissionState.UpdateExecuteState(ExecuteState.Executing);
						rMissionStateManager.Add(tmpMissionState.mName, tmpMissionState);
					}
				}
			}
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			// 當 IMissionState 結束時，將其從 Manager 中移除
			if (Args.StatusName.Contains("ExecuteState"))
			{
				switch (Args.Item.mExecuteState)
				{
					case ExecuteState.ExecuteSuccessed:
					case ExecuteState.ExecuteFailed:
						rMissionStateManager.Remove(Args.ItemName);
						break;
				}
			}
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleControl> Args)
		{
			// 當 IVehicleControl 的狀態更新時，將對應的 IMissionState 狀態也更新
			IMissionState correspondingMissionState = rMissionStateManager.GetItems().FirstOrDefault(o => o.mExecutorId == Args.Item.mVehicleId && o.mMission.mParametersString == Args.Item.mParametersString);
			if (correspondingMissionState != null)
			{
				if (Args.StatusName.Contains("SendState"))
				{
					switch (Args.Item.mSendState)
					{
						case SendState.SendFailed:
							correspondingMissionState.UpdateExecuteState(ExecuteState.ExecuteFailed);
							break;
					}
				}
				if (Args.StatusName.Contains("ExecuteState"))
				{
					switch (Args.Item.mExecuteState)
					{
						case ExecuteState.Executing:
						case ExecuteState.ExecuteSuccessed:
						case ExecuteState.ExecuteFailed:
							correspondingMissionState.UpdateExecuteState(Args.Item.mExecuteState);
							break;
					}
				}
				if (Args.StatusName.Contains("FailedReason"))
				{
					correspondingMissionState.UpdateFailedReason(Args.Item.mFailedReason);
				}
			}
		}
		private static IMissionState GenerateIMissionState(string VehicleId, string Target)
		{
			IMission tmpMission = null;
			if (Target.Contains("(") && Target.Contains(")") && Target.Contains(","))
			{
				// Target 為座標
				string[] tmpSplit = Target.Split(new string[] { "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
				switch (tmpSplit.Length)
				{
					case 2:
						tmpMission = Library.Library.GenerateIMission(MissionType.GotoPoint, string.Empty, 50, VehicleId, tmpSplit);
						break;
					case 3:
						tmpMission = Library.Library.GenerateIMission(MissionType.GotoPoint, string.Empty, 50, VehicleId, tmpSplit);
						break;
				}
			}
			else
			{
				// Target 為站點
				tmpMission = Library.Library.GenerateIMission(MissionType.Goto, string.Empty, 50, VehicleId, new string[] { Target });
			}

			if (tmpMission != null)
			{
				return Library.Library.GenerateIMissionState(tmpMission);
			}
			else
			{
				return null;
			}
		}
		private static IVehicleControl GenerateIVehicleControl(string VehicleId, string Target, string CauseId)
		{
			IVehicleControl result = null;
			if (Target.Contains("(") && Target.Contains(")") && Target.Contains(","))
			{
				// Target 為座標
				string[] tmpSplit = Target.Split(new string[] { "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
				switch (tmpSplit.Length)
				{
					case 2:
						result = Library.Library.GenerateIVehicleControl(VehicleId, Command.GotoPoint, tmpSplit, CauseId, string.Empty);
						break;
					case 3:
						result = Library.Library.GenerateIVehicleControl(VehicleId, Command.GotoTowardPoint, tmpSplit, CauseId, string.Empty);
						break;
				}
			}
			else
			{
				// Target 為站點
				result = Library.Library.GenerateIVehicleControl(VehicleId, Command.Goto, new string[] { Target }, CauseId, string.Empty);
			}
			return result;
		}
	}
}
