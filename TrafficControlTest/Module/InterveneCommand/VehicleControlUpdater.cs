using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.NewCommunication;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.InterveneCommand
{
	public class VehicleControlUpdater : SystemWithLoopTask, IVehicleControlUpdater
	{
		public int mTimeoutOfSendingVehicleControl { get; private set; } = 5;
		public int mTimeoutOfExecutingVehicleControl { get; private set; } = 600;
		public int mToleranceOfXOfArrivedTarget { get; private set; } = 500;
		public int mToleranceOfYOfArrivedTarget { get; private set; } = 500;
		public int mToleranceOfTowardOfArrivedTarget { get; private set; } = 5;
		public bool mAutoDetectNonSystemControl { get; private set; } = true;

		private IVehicleControlManager rVehicleControlManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IMapManager rMapManager = null;

		public VehicleControlUpdater(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator, IMapManager MapManager)
		{
			Set(VehicleControlManager, VehicleInfoManager, VehicleCommunicator, MapManager);
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
			SubscribeEvent_IVechielCommunicator(rVehicleCommunicator);
		}
		public void Set(IMapManager MapManager)
		{
			rMapManager = MapManager;
		}
		public void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator, IMapManager MapManager)
		{
			Set(VehicleControlManager);
			Set(VehicleInfoManager);
			Set(VehicleCommunicator);
			Set(MapManager);
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "TimePeriod", "TimeoutOfSendingVehicleControl", "TimeoutOfExecutingVehicleControl", "ToleranceOfXOfArrivedTarget", "ToleranceOfYOfArrivedTarget", "ToleranceOfTowardOfArrivedTarget", "AutoDetectNonSystemControl" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "TimeoutOfSendingVehicleControl":
					return mTimeoutOfSendingVehicleControl.ToString();
				case "TimeoutOfExecutingVehicleControl":
					return mTimeoutOfExecutingVehicleControl.ToString();
				case "ToleranceOfXOfArrivedTarget":
					return mToleranceOfXOfArrivedTarget.ToString();
				case "ToleranceOfYOfArrivedTarget":
					return mToleranceOfYOfArrivedTarget.ToString();
				case "ToleranceOfTowardOfArrivedTarget":
					return mToleranceOfTowardOfArrivedTarget.ToString();
				case "AutoDetectNonSystemControl":
					return mAutoDetectNonSystemControl.ToString();
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
				case "TimeoutOfSendingVehicleControl":
					mTimeoutOfSendingVehicleControl = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "TimeoutOfExecutingVehicleControl":
					mTimeoutOfExecutingVehicleControl = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ToleranceOfXOfArrivedTarget":
					mToleranceOfXOfArrivedTarget = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ToleranceOfYOfArrivedTarget":
					mToleranceOfYOfArrivedTarget = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "ToleranceOfTowardOfArrivedTarget":
					mToleranceOfTowardOfArrivedTarget = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "AutoDetectNonSystemControl":
					mAutoDetectNonSystemControl = bool.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override void Task()
		{
			Subtask_CheckVehicleControlSendTimeout();
			Subtask_CheckVehicleControlExecuteTimeout();
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
		private void SubscribeEvent_IVechielCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SentDataSuccessed += HandleEvent_VehicleCommunicatorSentDataSuccessed;
				VehicleCommunicator.SentDataFailed += HandleEvent_VehicleCommunicatorSentDataFailed;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SentDataSuccessed -= HandleEvent_VehicleCommunicatorSentDataSuccessed;
				VehicleCommunicator.SentDataFailed -= HandleEvent_VehicleCommunicatorSentDataFailed;
			}
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleControl> Args)
		{
			if (Args.StatusName.Contains("ExecuteState"))
			{
				switch (Args.Item.mExecuteState)
				{
					case ExecuteState.ExecuteSuccessed:
					case ExecuteState.ExecuteFailed:
						rVehicleControlManager.Remove(Args.ItemName);
						break;
				}
			}
			if (Args.StatusName.Contains("SendState"))
			{
				switch (Args.Item.mSendState)
				{
					case SendState.SendFailed:
						rVehicleControlManager.Remove(Args.ItemName);
						break;
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			if (Args.StatusName.Contains("CurrentState") || Args.StatusName.Contains("CurrentTarget"))
			{
				List<IVehicleControl> controls = GetCorrespondedVehicleControl(Args.Item, rVehicleControlManager);
				for (int i = 0; i < controls.Count; ++i)
				{
					UpdateVehicleControl(controls[i], Args.Item);
				}
			}

			if (mAutoDetectNonSystemControl)
			{
				if ((Args.StatusName.Contains("CurrentState") || Args.StatusName.Contains("CurrentTarget")) && (Args.Item.mPreviousState == "Idle" || Args.Item.mPreviousState == "ChargeIdle" || Args.Item.mPreviousState == "Charge") && Args.Item.mCurrentState == "Running" && !string.IsNullOrEmpty(Args.Item.mCurrentTarget))
				{
					string vehicleId = Args.Item.mName;
					string vehicleTarget = Args.Item.mCurrentTarget;
                    string vehicleCurrentState = Args.Item.mCurrentState;
					string vehiclePreviousState = Args.Item.mPreviousState;

					// 透過車子與當前 Target 去尋找 VehicleControlManager 是否有對應的物件
					// - 若有的話，代表車子是執行 VehicleControlManager 裡面的物件
					// - 若沒有的話，代表車子是自己動起來，有可能是其他人透過非 VM 的其他管道直接控制車子
					IVehicleControl correspondingControl = FindCorrespondingIVehicleControl(vehicleId, vehicleTarget, vehicleCurrentState, vehiclePreviousState, rVehicleControlManager, rMapManager);

					// 如果車子執行的是非 VehicleControlManager 裡的物件，則自動新增一個對應的物件至 VehicleControlManager 裡，並且標記 CauseId=VehicleSelf 資訊
					if (correspondingControl == null)
					{
						IVehicleControl newControl = GenerateIVehicleControl(vehicleId, vehicleTarget, vehicleCurrentState, vehiclePreviousState, rVehicleControlManager, rMapManager);
                        if (newControl != null)
                        {
                            newControl.UpdateSendState(SendState.SendSuccessed);
                            rVehicleControlManager.Add(newControl.mName, newControl);
                            newControl.UpdateExecuteState(ExecuteState.Executing);
                        }
					}
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorSentDataSuccessed(object Sender, SentDataEventArgs Args)
		{
			//if (Args.Data is Serializable)
			//{
			//	string vehicleId = rVehicleInfoManager.GetItemByIpPort(Args.IpPort).mName;
			//	if (rVehicleControlManager.GetItems().Any(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending))
			//	{
			//		rVehicleControlManager.GetItems().First(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending).UpdateSendState(SendState.SendSuccessed);
			//	}
			//}
		}
		private void HandleEvent_VehicleCommunicatorSentDataFailed(object Sender, SentDataEventArgs Args)
		{
			//if (Args.Data is Serializable && rVehicleInfoManager.IsExistByIpPort(Args.IpPort))
			//{
			//	string vehicleId = rVehicleInfoManager.GetItemByIpPort(Args.IpPort).mName;
			//	if (rVehicleControlManager.GetItems().Any(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending))
			//	{
			//		rVehicleControlManager.GetItems().First(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending).UpdateSendState(SendState.SendFailed);
			//	}
			//}
		}
		private void UpdateVehicleControl(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			switch (VehicleControl.mCommand)
			{
				case Command.PauseMoving:
					UpdateVehicleControlOfPauseMoving(VehicleControl, VehicleInfo);
					break;
				case Command.ResumeMoving:
					UpdateVehicleControlOfResumeMoving(VehicleControl, VehicleInfo);
					break;
				case Command.Goto:
					UpdateVehicleControlOfGoto(VehicleControl, VehicleInfo);
					break;
				case Command.GotoPoint:
					UpdateVehicleControlOfGotoPoint(VehicleControl, VehicleInfo);
					break;
				case Command.GotoTowardPoint:
					UpdateVehicleControlOfGotoTowardPoint(VehicleControl, VehicleInfo);
					break;
				case Command.Stop:
					UpdateVehicleControlOfStop(VehicleControl, VehicleInfo);
					break;
				case Command.Charge:
					UpdateVehicleControlOfCharge(VehicleControl, VehicleInfo);
					break;
				case Command.Uncharge:
					UpdateVehicleControlOfUncharge(VehicleControl, VehicleInfo);
					break;
					// 這幾種 Command 不會傳送給車子，所以亦不需要車子的資訊來更新狀態
					//case Command.Stay:
					//	break;
					//case Command.Unstay:
					//	break;
					//case Command.PauseControl:
					//	break;
					//case Command.ResumeControl:
					//	break;
			}
		}
		private void UpdateVehicleControlOfPauseMoving(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleControl.mSendState == SendState.Sending)
			{
				if (VehicleInfo.mCurrentState == "Pause")
				{
					VehicleControl.UpdateSendState(SendState.SendSuccessed);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
				}
			}
		}
		private void UpdateVehicleControlOfResumeMoving(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleControl.mSendState == SendState.Sending)
			{
				if (VehicleInfo.mCurrentState == "Running")
				{
					VehicleControl.UpdateSendState(SendState.SendSuccessed);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
				}
			}
		}
		private void UpdateVehicleControlOfGoto(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleControl.mSendState == SendState.Sending)
			{
				if (VehicleInfo.mCurrentState == "Running" && VehicleInfo.mCurrentTarget == VehicleControl.mParametersString)
				{
					VehicleControl.UpdateSendState(SendState.SendSuccessed);
					VehicleControl.UpdateExecuteState(ExecuteState.Executing);
				}
			}
			else if (VehicleControl.mExecuteState == ExecuteState.Executing)
			{
				if (VehicleInfo.mCurrentState == "RouteNotFind" || VehicleInfo.mCurrentState == "ObstacleExists" || VehicleInfo.mCurrentState == "BumperTrigger" || VehicleInfo.mCurrentState == "Alarm")
				{
					VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleOccurError);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
				else if (VehicleInfo.mCurrentState == "Charge" || VehicleInfo.mCurrentState == "ChargeIdle")
				{
					VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleGotoCharge);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
				else if (VehicleInfo.mCurrentState == "Idle")
				{
					if (IsVehicleArrived(VehicleInfo, VehicleControl.mParameters[0]))
					{
						VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
					}
					else
					{
						VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleIdleButNotArrived);
						VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
					}
				}
			}
		}
		private void UpdateVehicleControlOfGotoPoint(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleControl.mSendState == SendState.Sending)
			{
				if (VehicleInfo.mCurrentState == "Running" && VehicleInfo.mCurrentTarget == VehicleControl.mParametersString)
				{
					VehicleControl.UpdateSendState(SendState.SendSuccessed);
					VehicleControl.UpdateExecuteState(ExecuteState.Executing);
				}
			}
			else if (VehicleControl.mExecuteState == ExecuteState.Executing)
			{
				if (VehicleInfo.mCurrentState == "RouteNotFind" || VehicleInfo.mCurrentState == "ObstacleExists" || VehicleInfo.mCurrentState == "BumperTrigger" || VehicleInfo.mCurrentState == "Alarm")
				{
					VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleOccurError);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
				else if (VehicleInfo.mCurrentState == "Charge" || VehicleInfo.mCurrentState == "ChargeIdle")
				{
					VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleGotoCharge);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
				else if (VehicleInfo.mCurrentState == "Idle")
				{
					if (IsVehicleArrived(VehicleInfo, int.Parse(VehicleControl.mParameters[0]), int.Parse(VehicleControl.mParameters[1])))
					{
						VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
					}
					else
					{
						VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleIdleButNotArrived);
						VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
					}
				}
			}
		}
		private void UpdateVehicleControlOfGotoTowardPoint(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleControl.mSendState == SendState.Sending)
			{
				if (VehicleInfo.mCurrentState == "Running" && VehicleInfo.mCurrentTarget == VehicleControl.mParametersString)
				{
					VehicleControl.UpdateSendState(SendState.SendSuccessed);
					VehicleControl.UpdateExecuteState(ExecuteState.Executing);
				}
			}
			else if (VehicleControl.mExecuteState == ExecuteState.Executing)
			{
				if (VehicleInfo.mCurrentState == "RouteNotFind" || VehicleInfo.mCurrentState == "ObstacleExists" || VehicleInfo.mCurrentState == "BumperTrigger" || VehicleInfo.mCurrentState == "Alarm")
				{
					VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleOccurError);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
				else if (VehicleInfo.mCurrentState == "Charge" || VehicleInfo.mCurrentState == "ChargeIdle")
				{
					VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleGotoCharge);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
				else if (VehicleInfo.mCurrentState == "Idle")
				{
					if (IsVehicleArrived(VehicleInfo, int.Parse(VehicleControl.mParameters[0]), int.Parse(VehicleControl.mParameters[1]), int.Parse(VehicleControl.mParameters[2])))
					{
						VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
					}
					else
					{
						VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleIdleButNotArrived);
						VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
					}
				}
			}
		}
		private void UpdateVehicleControlOfStop(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleControl.mSendState == SendState.Sending)
			{
				if (VehicleInfo.mCurrentState == "Idle")
				{
					VehicleControl.UpdateSendState(SendState.SendSuccessed);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
				}
			}
		}
		private void UpdateVehicleControlOfCharge(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleControl.mSendState == SendState.Sending)
			{
				if (VehicleInfo.mCurrentState == "Running" && rMapManager.GetTowardPointMapObjects(TypeOfMapObjectOfTowardPoint.Charge).Select(o => o.mName).Contains(VehicleInfo.mCurrentTarget))
				{
					if (rMapManager.GetTowardPointMapObjects(TypeOfMapObjectOfTowardPoint.Charge).Select(o => o.mName).Any(o => o == VehicleInfo.mCurrentTarget))
					{
						VehicleControl.UpdateSendState(SendState.SendSuccessed);
						VehicleControl.UpdateExecuteState(ExecuteState.Executing);
					}
					else
					{
						VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleNotGoingtoCharge);
						VehicleControl.UpdateSendState(SendState.SendFailed);
					}
				}
			}
			else if (VehicleControl.mExecuteState == ExecuteState.Executing)
			{
				if (VehicleInfo.mCurrentState == "RouteNotFind" || VehicleInfo.mCurrentState == "ObstacleExists" || VehicleInfo.mCurrentState == "BumperTrigger" || VehicleInfo.mCurrentState == "Alarm")
				{
					VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleOccurError);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
				else if (VehicleInfo.mCurrentState == "Idle")
				{
					VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleStopped);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
				else if (VehicleInfo.mCurrentState == "Charge" || VehicleInfo.mCurrentState == "ChargeIdle")
				{
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
				}
			}
		}
		private void UpdateVehicleControlOfUncharge(IVehicleControl VehicleControl, IVehicleInfo VehicleInfo)
		{
			if (VehicleControl.mSendState == SendState.Sending)
			{
				// 目前傳送 Uncharge 給自走車 (iTS) 時，狀態會變成 Charge -> Running ，等到退出充電站完成後， Running -> Idle
				if (VehicleInfo.mCurrentState == "Running")
				{
					VehicleControl.UpdateSendState(SendState.SendSuccessed);
					VehicleControl.UpdateExecuteState(ExecuteState.Executing);
				}
			}
			else if (VehicleControl.mExecuteState == ExecuteState.Executing)
			{
				if (VehicleInfo.mCurrentState == "RouteNotFind" || VehicleInfo.mCurrentState == "ObstacleExists" || VehicleInfo.mCurrentState == "BumperTrigger" || VehicleInfo.mCurrentState == "Alarm")
				{
					VehicleControl.UpdateExecuteFailedReason(FailedReason.VehicleOccurError);
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
				else if (VehicleInfo.mCurrentState == "Idle")
				{
					VehicleControl.UpdateExecuteState(ExecuteState.ExecuteSuccessed);
				}
			}
		}
		private void Subtask_CheckVehicleControlSendTimeout()
		{
			List<IVehicleControl> sendingVehicleControls = rVehicleControlManager.GetItems().Where(o => o.mSendState == SendState.Sending).ToList();
			for (int i = 0; i < sendingVehicleControls.Count; ++i)
			{
				if (rVehicleInfoManager.IsExist(sendingVehicleControls[i].mVehicleId))
				{
					// 若指定車存在，但持續 n 秒控制的傳送狀態皆未改變，標示該控制為傳送失敗
					if (DateTime.Now.Subtract(sendingVehicleControls[i].mLastUpdated).TotalSeconds > mTimeoutOfSendingVehicleControl)
					{
						sendingVehicleControls[i].UpdateExecuteFailedReason(FailedReason.SentTimeout);
						sendingVehicleControls[i].UpdateSendState(SendState.SendFailed);
					}
				}
				else
				{
					// 若指定車不存在，標示該控制為傳送失敗
					sendingVehicleControls[i].UpdateExecuteFailedReason(FailedReason.VehicleDisconnected);
					sendingVehicleControls[i].UpdateSendState(SendState.SendFailed);
				}
			}
		}
		private void Subtask_CheckVehicleControlExecuteTimeout()
		{
			List<IVehicleControl> executingVehicleControls = rVehicleControlManager.GetItems().Where(o => o.mExecuteState == ExecuteState.Executing).ToList();
			for (int i = 0; i < executingVehicleControls.Count; ++i)
			{
				if (rVehicleInfoManager.IsExist(executingVehicleControls[i].mVehicleId))
				{
					// 若指定車存在，但持續 n 秒控制的執行狀態皆未改變，標示該控制為執行失敗
					if (DateTime.Now.Subtract(executingVehicleControls[i].mLastUpdated).TotalSeconds > mTimeoutOfExecutingVehicleControl)
					{
						executingVehicleControls[i].UpdateExecuteFailedReason(FailedReason.ExectutedTimeout);
						executingVehicleControls[i].UpdateExecuteState(ExecuteState.ExecuteFailed);
					}
				}
				else
				{
					// 若指定車不存在，標示該控制為執行失敗
					executingVehicleControls[i].UpdateExecuteFailedReason(FailedReason.VehicleDisconnected);
					executingVehicleControls[i].UpdateExecuteState(ExecuteState.ExecuteFailed);
				}
			}
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, string Target)
		{
			IMapObjectOfTowardPoint towardPointMapObject = rMapManager.GetTowardPointMapObject(Target);
			return IsVehicleArrived(VehicleInfo, towardPointMapObject.mLocation.mX, towardPointMapObject.mLocation.mY, (int)towardPointMapObject.mLocation.mToward);
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, int X, int Y)
		{
			return VehicleInfo.mCurrentState == "Idle" && Math.Abs(VehicleInfo.mLocationCoordinate.mX - X) < mToleranceOfXOfArrivedTarget && Math.Abs(VehicleInfo.mLocationCoordinate.mY - Y) < mToleranceOfYOfArrivedTarget;
		}
		private bool IsVehicleArrived(IVehicleInfo VehicleInfo, int X, int Y, int Toward)
		{
			int diffX = Math.Abs(VehicleInfo.mLocationCoordinate.mX - X);
			int diffY = Math.Abs(VehicleInfo.mLocationCoordinate.mY - Y);
			int diffToward = Math.Abs((int)(VehicleInfo.mLocationToward) - Toward);
			return VehicleInfo.mCurrentState == "Idle" && diffX < mToleranceOfXOfArrivedTarget && diffY < mToleranceOfYOfArrivedTarget && ((diffToward < mToleranceOfTowardOfArrivedTarget) || (diffToward <= 360 && diffToward > (360 - mToleranceOfTowardOfArrivedTarget)));
		}
		private static List<IVehicleControl> GetCorrespondedVehicleControl(IVehicleInfo VehicleInfo, IVehicleControlManager VehicleControlManager)
		{
			return VehicleControlManager.GetItems().Where(o => (o.mSendState == SendState.Sending || o.mExecuteState == ExecuteState.Executing) && o.mVehicleId == VehicleInfo.mName).ToList();
		}
		private static IVehicleControl FindCorrespondingIVehicleControl(string VehicleId, string VehicleTarget, string VehicleCurrentState, string VehiclePreviousState, IVehicleControlManager VehicleControlManager, IMapManager MapManager)
		{
			IVehicleControl result = null;
			// 如何當前目標是充電站，代表執行的是充電/解除充電控制
			if (MapManager.GetTowardPointMapObjects(TypeOfMapObjectOfTowardPoint.Charge).Select(o => o.mName).Any(o => o == VehicleTarget))
			{

                // 尋找 VehicleControlManager 內是否有對應的充電控制
                // 充電控制時的狀態變化： Idle -> Running -> Charge
                // 充電控制：
                // 1. Command=Goto Parameter=充電站
                // 2. Command=Charge Parameter=null
                if (VehiclePreviousState == "Idle" && VehicleCurrentState == "Running")
				{
					result = VehicleControlManager.GetItems().FirstOrDefault(o => o.mExecuteState == ExecuteState.Executing && o.mVehicleId == VehicleId && ((o.mCommand == Command.Goto && o.mParametersString == VehicleTarget) || (o.mCommand == Command.Charge && o.mParameters == null)));
				}
                // 尋找 VehicleControlManager 內是否有對應的解除充電控制
                // 解除充電控制時的狀態變化： Charge/ChargeIdle -> Running -> Idle
                // 解除充電控制：
                // 1. Command=Uncharge Parameter=null
                else if ((VehiclePreviousState == "Charge" || VehiclePreviousState == "ChargeIdle") && VehicleCurrentState == "Running")
				{
					result = VehicleControlManager.GetItems().FirstOrDefault(o => o.mExecuteState == ExecuteState.Executing && o.mVehicleId == VehicleId && o.mCommand == Command.Uncharge);
				}
			}
			// 當前目標不是充電站，代表執行的是一般的移動 (Goto/GotoPoint/GotoTowardPoint) 控制
			else
			{
				// 尋找 VehicleControlManager 是否有對應的一般的移動控制
				result = VehicleControlManager.GetItems().FirstOrDefault(o => o.mExecuteState == ExecuteState.Executing && o.mVehicleId == VehicleId && o.mParametersString == VehicleTarget);
			}
			return result;
		}
        private static IVehicleControl GenerateIVehicleControl(string VehicleId, string VehicleTarget, string VehicleCurrentState, string VehiclePreviousState, IVehicleControlManager VehicleControlManager, IMapManager MapManager)
        {
            IVehicleControl result = null;
            if (MapManager.GetTowardPointMapObjects(TypeOfMapObjectOfTowardPoint.Charge).Select(o => o.mName).Any(o => o == VehicleTarget))
            {
                if (VehiclePreviousState == "Idle" && VehicleCurrentState == "Running")
                {
                    // 充電控制
                    result = Library.Library.GenerateIVehicleControl(VehicleId, Command.Charge, null, $"{VehicleId}Self", string.Empty);
                }
                else if ((VehiclePreviousState == "Charge" || VehiclePreviousState == "ChargeIdle") && VehicleCurrentState == "Running")
                {
                    // 解除充電控制
                    result = Library.Library.GenerateIVehicleControl(VehicleId, Command.Uncharge, null, $"{VehicleId}Self", string.Empty);
                }
            }
            else
            {
                // 移動控制
                result = GenerateIVehicleControl(VehicleId, VehicleTarget, $"{VehicleId}Self");
            }
            return result;
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
