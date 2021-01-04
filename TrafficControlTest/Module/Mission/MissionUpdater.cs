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
		public override string[] GetConfigNameList()
		{
			return new string[] { "AutoDetectNonSystemMission" };
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
				VehicleControlManager.ItemAdded += HandleEvent_VehicleControlManagerItemAdded;
				VehicleControlManager.ItemUpdated += HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemAdded -= HandleEvent_VehicleControlManagerItemAdded;
				VehicleControlManager.ItemUpdated -= HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{

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
		private void HandleEvent_VehicleControlManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleControl> Args)
		{
			if (mAutoDetectNonSystemMission)
			{
				IVehicleControl newControl = Args.Item;
				// 從 MissionStateManager 中尋找對應的 MissionState
				IMissionState correspondingMissionState = FindCorrespondingIMissionState(Args.Item, rMissionStateManager);

				if (correspondingMissionState == null)
				{
					// 若該控制的產生原因為自走車本身或是手動透過系統新增的，則新增對應的 MissionState 至系統中
					if (newControl.mCauseId.Contains("Self") || newControl.mCauseId.Contains("Manual"))
					{
						IMissionState newMissionState = GenerateIMissionState(Args.Item);
						if (newMissionState != null)
						{
							newMissionState.UpdateExecutorId(newControl.mVehicleId);
							newMissionState.UpdateSourceIpPort(newControl.mCauseId);
							rMissionStateManager.Add(newMissionState.mName, newMissionState);
						}
					}
				}
			}
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleControl> Args)
		{
			// 當 IVehicleControl 的狀態更新時，將對應的 IMissionState 狀態也更新
			IMissionState correspondingMissionState = FindCorrespondingIMissionState(Args.Item, rMissionStateManager);
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
		private static IMissionState FindCorrespondingIMissionState(IVehicleControl VehicleControl, IMissionStateManager MissionStateManager)
		{
			IMissionState result = null;
			if (VehicleControl.mCommand == Command.Charge)
			{
				result = MissionStateManager.GetItems().FirstOrDefault(o => o.mExecutorId == VehicleControl.mVehicleId && o.mMission.mMissionType == MissionType.Dock);
			}
			else if (VehicleControl.mCommand == Command.Uncharge)
			{
				// 目前沒有解除充電的任務，所以一定找不到對應的 MissionState
			}
			else if (VehicleControl.mCommand == Command.Goto || VehicleControl.mCommand == Command.GotoPoint || VehicleControl.mCommand == Command.GotoTowardPoint)
			{
				result = MissionStateManager.GetItems().FirstOrDefault(o => o.mExecutorId == VehicleControl.mVehicleId && o.mMission.mParametersString == VehicleControl.mParametersString);
			}
			return result;
		}
		private static IMissionState GenerateIMissionState(IVehicleControl VehicleControl)
		{
			IMissionState result = null;
			if (VehicleControl.mCommand == Command.Charge)
			{
				result = Library.Library.GenerateIMissionState(Library.Library.GenerateIMission(MissionType.Dock, string.Empty, 50, VehicleControl.mVehicleId, null));
			}
			else if (VehicleControl.mCommand == Command.Uncharge)
			{
				// 目前沒有解除充電的任務，所以一定會是 null
			}
			else if (VehicleControl.mCommand == Command.Goto || VehicleControl.mCommand == Command.GotoPoint || VehicleControl.mCommand == Command.GotoTowardPoint)
			{
				result = GenerateIMissionState(VehicleControl.mVehicleId, VehicleControl.mParametersString);
			}
			return result;
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
	}
}
