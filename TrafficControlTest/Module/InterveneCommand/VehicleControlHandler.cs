using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Module.ChargeStation;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.InterveneCommand
{
	public class VehicleControlHandler : SystemWithLoopTask, IVehicleControlHandler
	{
		private IVehicleControlManager rVehicleControlManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IChargeStationInfoManager rChargeStationInfoManager = null;

		public VehicleControlHandler(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			Set(VehicleControlManager, VehicleInfoManager, VehicleCommunicator);
		}
		public void Set(IVehicleControlManager VehicleControlManager)
		{
			UnsubscribeEvent_IVehicleControlManager(rVehicleControlManager);
			rVehicleControlManager = VehicleControlManager;
			SubscribeEvent_IVehicleControlManager(rVehicleControlManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IChargeStationInfoManager ChargeStationInfoManager)
		{
			rChargeStationInfoManager = ChargeStationInfoManager;
		}
		public void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			Set(VehicleControlManager);
			Set(VehicleInfoManager);
			Set(VehicleCommunicator);
		}
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			result += $"CountOfVehicleControl: {rVehicleControlManager.mCount}";
			if (rVehicleControlManager.mCount > 0)
			{
				result += ", ";
				result += string.Join(", ", rVehicleControlManager.GetItems().Select(o => $"{o.mName}-{o.mVehicleId}-{o.mCommand.ToString()}-{o.mSendState.ToString()}-{o.mExecuteState.ToString()}"));
			}
			return result;
		}
		public override void Task()
		{
			Subtask_HandleVehicleControls();
		}

		private void SubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{

			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{

			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{

			}
		}
		private void Subtask_HandleVehicleControls()
		{
			HandleVehicleControls(rVehicleControlManager.GetItems().OrderBy(o => o.mReceivedTimestamp));
		}
		private void HandleVehicleControls(IEnumerable<IVehicleControl> VehicleControls)
		{
			if (VehicleControls != null && VehicleControls.Count() > 0)
			{
				foreach (IVehicleControl vehicleControl in VehicleControls)
				{
					HandleVehicleControl(vehicleControl);
				}
			}
		}
		private void HandleVehicleControl(IVehicleControl VehicleControl)
		{
			if (VehicleControl.mSendState == SendState.Unsend && VehicleControl.mExecuteState == ExecuteState.Unexecute)
			{
				IVehicleInfo vehicleInfo = rVehicleInfoManager.GetItem(VehicleControl.mVehicleId);
				switch (VehicleControl.mCommand)
				{
					case Command.PauseMoving:
						HandleVehicleControlOfPauseMoving(VehicleControl, vehicleInfo);
						break;
					case Command.ResumeMoving:
						HandleVehicleControlOfResumeMoving(VehicleControl, vehicleInfo);
						break;
					case Command.Goto:
						HandleVehicleControlOfGoto(VehicleControl, vehicleInfo);
						break;
					case Command.GotoPoint:
						HandleVehicleControlOfGotoPoint(VehicleControl, vehicleInfo);
						break;
					case Command.GotoTowardPoint:
						HandleVehicleControlOfGotoTowardPoint(VehicleControl, vehicleInfo);
						break;
					case Command.Stop:
						HandleVehicleControlOfStop(VehicleControl, vehicleInfo);
						break;
					case Command.Charge:
						HandleVehicleControlOfCharge(VehicleControl, vehicleInfo);
						break;
					case Command.Uncharge:
						HandleVehicleControlOfUncharge(VehicleControl, vehicleInfo);
						break;
					case Command.Stay:
						HandleVehicleControlOfStay(VehicleControl, vehicleInfo);
						break;
					case Command.Unstay:
						HandleVehicleControlOfUnstay(VehicleControl, vehicleInfo);
						break;
					case Command.PauseControl:
						HandleVehicleControlOfPauseControl(VehicleControl, vehicleInfo);
						break;
					case Command.ResumeControl:
						HandleVehicleControlOfResumeControl(VehicleControl, vehicleInfo);
						break;
					case Command.Abort:
						HandleVehicleControlOfAbortControl(VehicleControl, vehicleInfo);
						break;
				}
			}
		}
		private void HandleVehicleControlOfPauseMoving(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo.mCurrentState == "Running")
			{
				VehicleControl.UpdateSendState(SendState.Sending);
				rVehicleCommunicator.SendDataOfPauseMoving(VehicleInfo.mIpPort);
			}
		}
		private void HandleVehicleControlOfResumeMoving(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo.mCurrentState == "Pause")
			{
				VehicleControl.UpdateSendState(SendState.Sending);
				rVehicleCommunicator.SendDataOfResumeMoving(VehicleInfo.mIpPort);
			}
		}
		private void HandleVehicleControlOfGoto(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			// 情境一：沒有執行任務時，且狀態為閒置，也沒有排程 Normal Control 時，且沒有在執行 Stay Control
			// 情境二：有執行任務時，且狀態為閒置，也沒有排程 Normal Control 時，且當前 Control 已暫停時，且沒有在執行 Stay Control
			if ((string.IsNullOrEmpty(VehicleInfo.mCurrentMissionId) && (VehicleInfo.mCurrentState == "Idle" || VehicleInfo.mCurrentState == "ChargeIdle") && (string.IsNullOrEmpty(VehicleInfo.mErrorMessage) || VehicleInfo.mErrorMessage == "Normal") && !IsVehicleExecutingNormalControl(VehicleInfo, rVehicleControlManager) && !IsVehicleExecutingStayControl(VehicleInfo, rVehicleControlManager))
				|| (!string.IsNullOrEmpty(VehicleInfo.mCurrentMissionId) && (VehicleInfo.mCurrentState == "Idle" || VehicleInfo.mCurrentState == "ChargeIdle") && (string.IsNullOrEmpty(VehicleInfo.mErrorMessage) || VehicleInfo.mErrorMessage == "Normal") && !IsVehicleExecutingNormalControl(VehicleInfo, rVehicleControlManager) && IsVehiclePausedNormalControl(VehicleInfo, rVehicleControlManager) && !IsVehicleExecutingStayControl(VehicleInfo, rVehicleControlManager)))
			{
				VehicleControl.UpdateSendState(SendState.Sending);
				rVehicleCommunicator.SendDataOfGoto(VehicleInfo.mIpPort, VehicleControl.mParameters[0]);
			}
		}
		private void HandleVehicleControlOfGotoPoint(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if ((string.IsNullOrEmpty(VehicleInfo.mCurrentMissionId) && (VehicleInfo.mCurrentState == "Idle" || VehicleInfo.mCurrentState == "ChargeIdle") && (string.IsNullOrEmpty(VehicleInfo.mErrorMessage) || VehicleInfo.mErrorMessage == "Normal") && !IsVehicleExecutingNormalControl(VehicleInfo, rVehicleControlManager) && !IsVehicleExecutingStayControl(VehicleInfo, rVehicleControlManager))
				|| (!string.IsNullOrEmpty(VehicleInfo.mCurrentMissionId) && (VehicleInfo.mCurrentState == "Idle" || VehicleInfo.mCurrentState == "ChargeIdle") && (string.IsNullOrEmpty(VehicleInfo.mErrorMessage) || VehicleInfo.mErrorMessage == "Normal") && !IsVehicleExecutingNormalControl(VehicleInfo, rVehicleControlManager) && IsVehiclePausedNormalControl(VehicleInfo, rVehicleControlManager) && !IsVehicleExecutingStayControl(VehicleInfo, rVehicleControlManager)))
			{
				VehicleControl.UpdateSendState(SendState.Sending);
				rVehicleCommunicator.SendDataOfGotoPoint(VehicleInfo.mIpPort, int.Parse(VehicleControl.mParameters[0]), int.Parse(VehicleControl.mParameters[1]));
			}
		}
		private void HandleVehicleControlOfGotoTowardPoint(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if ((string.IsNullOrEmpty(VehicleInfo.mCurrentMissionId) && (VehicleInfo.mCurrentState == "Idle" || VehicleInfo.mCurrentState == "ChargeIdle") && (string.IsNullOrEmpty(VehicleInfo.mErrorMessage) || VehicleInfo.mErrorMessage == "Normal") && !IsVehicleExecutingNormalControl(VehicleInfo, rVehicleControlManager) && !IsVehicleExecutingStayControl(VehicleInfo, rVehicleControlManager))
				|| (!string.IsNullOrEmpty(VehicleInfo.mCurrentMissionId) && (VehicleInfo.mCurrentState == "Idle" || VehicleInfo.mCurrentState == "ChargeIdle") && (string.IsNullOrEmpty(VehicleInfo.mErrorMessage) || VehicleInfo.mErrorMessage == "Normal") && !IsVehicleExecutingNormalControl(VehicleInfo, rVehicleControlManager) && IsVehiclePausedNormalControl(VehicleInfo, rVehicleControlManager) && !IsVehicleExecutingStayControl(VehicleInfo, rVehicleControlManager)))
			{
				VehicleControl.UpdateSendState(SendState.Sending);
				rVehicleCommunicator.SendDataOfGotoTowardPoint(VehicleInfo.mIpPort, int.Parse(VehicleControl.mParameters[0]), int.Parse(VehicleControl.mParameters[1]), int.Parse(VehicleControl.mParameters[2]));
			}
		}
		private void HandleVehicleControlOfStop(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (true/*VehicleInfo.mCurrentState == "Running" || VehicleInfo.mCurrentState == "RouteNotFind" || VehicleInfo.mCurrentState == "ObstacleExists" || VehicleInfo.mCurrentState == "BumperTrigger"*/)
			{
				VehicleControl.UpdateSendState(SendState.Sending);
				rVehicleCommunicator.SendDataOfStop(VehicleInfo.mIpPort);
			}
		}
		private void HandleVehicleControlOfCharge(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if ((string.IsNullOrEmpty(VehicleInfo.mCurrentMissionId) && VehicleInfo.mCurrentState == "Idle" && (string.IsNullOrEmpty(VehicleInfo.mErrorMessage) || VehicleInfo.mErrorMessage == "Normal") && !IsVehicleExecutingNormalControl(VehicleInfo, rVehicleControlManager) && !IsVehicleExecutingStayControl(VehicleInfo, rVehicleControlManager)))
			{
				VehicleControl.UpdateSendState(SendState.Sending);
				rVehicleCommunicator.SendDataOfCharge(VehicleInfo.mIpPort);
			}
		}
		private void HandleVehicleControlOfUncharge(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if ((VehicleInfo.mCurrentState == "Charge" || VehicleInfo.mCurrentState == "ChargeIdle") && (string.IsNullOrEmpty(VehicleInfo.mErrorMessage) || VehicleInfo.mErrorMessage == "Normal") && !IsVehicleExecutingNormalControl(VehicleInfo, rVehicleControlManager))
			{
				VehicleControl.UpdateSendState(SendState.Sending);
				rVehicleCommunicator.SendDataOfUncharge(VehicleInfo.mIpPort);
			}
		}
		private void HandleVehicleControlOfStay(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo.mCurrentState == "Idle" && !IsVehicleExecutingStayControl(VehicleInfo, rVehicleControlManager))
			{
				VehicleControl.UpdateExecuteState(ExecuteState.Executing);
			}
		}
		private void HandleVehicleControlOfUnstay(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo.mCurrentState == "Idle" && IsVehicleExecutingStayControl(VehicleInfo, rVehicleControlManager))
			{
				IVehicleControl correspondingStayControl = rVehicleControlManager.GetItems().Where(O => O.mVehicleId == VehicleInfo.mName).First(o => o.mCommand == Command.Stay && o.mExecuteState == ExecuteState.Executing);
				correspondingStayControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
				VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
			}
		}
		private void HandleVehicleControlOfPauseControl(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (IsVehicleExecutingNormalControl(VehicleInfo, rVehicleControlManager))
			{
				IVehicleControl correspondingNormalControl = rVehicleControlManager.GetItems().Where(o => o.mVehicleId == VehicleInfo.mName).First(o => (o.mCommand == Command.Goto || o.mCommand == Command.GotoPoint || o.mCommand == Command.GotoTowardPoint || o.mCommand == Command.Charge || o.mCommand == Command.Uncharge) && (o.mSendState == SendState.Sending || o.mExecuteState == ExecuteState.Executing));
				correspondingNormalControl.UpdateExecuteState(ExecuteState.ExecutePaused);
				VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
			}
		}
		private void HandleVehicleControlOfResumeControl(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (IsVehiclePausedNormalControl(VehicleInfo, rVehicleControlManager))
			{
				IVehicleControl correspondingPauseControl = rVehicleControlManager.GetItems().Where(o => o.mVehicleId == VehicleInfo.mName).First(o => (o.mCommand == Command.Goto || o.mCommand == Command.GotoPoint || o.mCommand == Command.GotoTowardPoint || o.mCommand == Command.Charge || o.mCommand == Command.Uncharge) && o.mExecuteState == ExecuteState.ExecutePaused);
				correspondingPauseControl.UpdateExecuteState(ExecuteState.Unexecute);
				correspondingPauseControl.UpdateSendState(SendState.Unsend);
				HandleVehicleControl(correspondingPauseControl);
				VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
			}
		}
		private void HandleVehicleControlOfAbortControl(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			VehicleControl.UpdateExecuteState(ExecuteState.Executing);
			// 將佇列中所有符合條件的 Control 中斷
			List<IVehicleControl> controls = rVehicleControlManager.GetItems().ToList();
			for (int i = 0; i < controls.Count; ++i)
			{
				if (controls[i].mName != VehicleControl.mName) // 不是自身 Control
				{
					if (controls[i].mVehicleId == VehicleControl.mVehicleId) // Control 的 VehicleId 為欲中斷的自走車
					{
						controls[i].UpdateExecuteFailedReason(FailedReason.CancelByHostCommand);
						controls[i].UpdateExecuteState(ExecuteState.ExecuteFailed);
					}
				}
			}
			// 傳送 Stop 指令給指定車
			rVehicleCommunicator.SendDataOfStop(VehicleInfo.mIpPort);
			VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
		}

		private static bool IsVehicleExecutingNormalControl(IVehicleInfo VehicleInfo, IVehicleControlManager VehicleControlManager)
		{
			return IsVehicleExecutingNormalControl(VehicleInfo.mName, VehicleControlManager);
		}
		private static bool IsVehicleExecutingNormalControl(string VehicleId, IVehicleControlManager VehicleControlManager)
		{
			return GetCorrespondingExecutingNormalControl(VehicleId, VehicleControlManager) == null ? false : true;
		}
		private static bool IsVehiclePausedNormalControl(IVehicleInfo VehicleInfo, IVehicleControlManager VehicleControlManager)
		{
			return IsVehiclePausedNormalControl(VehicleInfo.mName, VehicleControlManager);
		}
		private static bool IsVehiclePausedNormalControl(string VehicleId, IVehicleControlManager VehicleControlManager)
		{
			return GetCorrespondingPausedNormalControl(VehicleId, VehicleControlManager) == null ? false : true;
		}
		private static bool IsVehicleExecutingStayControl(IVehicleInfo VehicleInfo, IVehicleControlManager VehicleControlManager)
		{
			return IsVehicleExecutingStayControl(VehicleInfo.mName, VehicleControlManager);
		}
		private static bool IsVehicleExecutingStayControl(string VehicleId, IVehicleControlManager VehicleControlManager)
		{
			return GetCorrespondingExecutingStayControl(VehicleId, VehicleControlManager) == null ? false : true;
		}
		private static IVehicleControl GetCorrespondingExecutingNormalControl(string VehicleId, IVehicleControlManager VehicleControlManager)
		{
			return VehicleControlManager.GetItems().Where(o => o.mVehicleId == VehicleId).FirstOrDefault(o => (o.mCommand == Command.Goto || o.mCommand == Command.GotoPoint || o.mCommand == Command.GotoTowardPoint || o.mCommand == Command.Charge || o.mCommand == Command.Uncharge) && (o.mSendState == SendState.Sending || o.mExecuteState == ExecuteState.Executing));
		}
		private static IVehicleControl GetCorrespondingPausedNormalControl(string VehicleId, IVehicleControlManager VehicleControlManager)
		{
			return VehicleControlManager.GetItems().Where(o => o.mVehicleId == VehicleId).FirstOrDefault(o => (o.mCommand == Command.Goto || o.mCommand == Command.GotoPoint || o.mCommand == Command.GotoTowardPoint || o.mCommand == Command.Charge || o.mCommand == Command.Uncharge) && o.mExecuteState == ExecuteState.ExecutePaused);
		}
		private static IVehicleControl GetCorrespondingExecutingStayControl(string VehicleId, IVehicleControlManager VehicleControlManager)
		{
			IVehicleControl tmp = VehicleControlManager.GetItems().Where(O => O.mVehicleId == VehicleId).FirstOrDefault(o => o.mCommand == Command.Stay && o.mExecuteState == ExecuteState.Executing);
			return VehicleControlManager.GetItems().Where(O => O.mVehicleId == VehicleId).FirstOrDefault(o => o.mCommand == Command.Stay && o.mExecuteState == ExecuteState.Executing);
		}
	}
}
