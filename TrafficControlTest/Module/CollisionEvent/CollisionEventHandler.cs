using System;
using System.Linq;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.CollisionEvent
{
	class CollisionEventHandler : ICollisionEventHandler
	{
		private ICollisionEventManager rCollisionEventManager = null;
		private IVehicleControlManager rVehicleControlManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;

		public CollisionEventHandler(ICollisionEventManager CollisionEventManager, IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(CollisionEventManager, VehicleControlManager, VehicleInfoManager);
		}
		public void Set(ICollisionEventManager CollisionEventManager)
		{
			UnsubscribeEvent_ICollisionEventManager(rCollisionEventManager);
			rCollisionEventManager = CollisionEventManager;
			SubscribeEvent_ICollisionEventManager(rCollisionEventManager);
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
		public void Set(ICollisionEventManager CollisionEventManager, IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(CollisionEventManager);
			Set(VehicleControlManager);
			Set(VehicleInfoManager);
		}

		private void SubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{
				CollisionEventManager.ItemAdded += HandleEvent_CollisionEventManagerItemAdded;
				CollisionEventManager.ItemRemoved += HandleEvent_CollisionEventManagerItemRemoved;
				CollisionEventManager.ItemUpdated += HandleEvent_CollisionEventManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{
				CollisionEventManager.ItemAdded -= HandleEvent_CollisionEventManagerItemAdded;
				CollisionEventManager.ItemRemoved -= HandleEvent_CollisionEventManagerItemRemoved;
				CollisionEventManager.ItemUpdated -= HandleEvent_CollisionEventManagerItemUpdated;
			}
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
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager rVehicleInfoManager)
		{
			if (rVehicleInfoManager != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager rVehicleInfoManager)
		{
			if (rVehicleInfoManager != null)
			{

			}
		}
		private void HandleEvent_CollisionEventManagerItemAdded(object Sender, ItemCountChangedEventArgs<ICollisionPair> Args)
		{
			HandleCollisionPair(Args.Item);
		}
		private void HandleEvent_CollisionEventManagerItemRemoved(object Sender, ItemCountChangedEventArgs<ICollisionPair> Args)
		{
			// 當 CollisionEvent 消失時，移除跟該 CollisionEvent 有關的且尚未被送出的 VehicleControl
			RemoveRelatedVehicleControl(Args.Item);
			// 當 CollisionEvent 消失時，讓跟該 CollisionEvent 有關的 Vehicle 恢復成沒有被干預的狀態
			UninterveneRelatedVehicle(Args.Item);
		}
		private void HandleEvent_CollisionEventManagerItemUpdated(object Sender, ItemUpdatedEventArgs<ICollisionPair> Args)
		{
			HandleCollisionPair(Args.Item);
		}
		private void HandleCollisionPair(ICollisionPair CollisionPair)
		{
			// sample function, undone.
			if (CollisionPair != null && IsProcessNecessary(CollisionPair))
			{
				IVehicleControl vehicleControl = CalculateIVehicleControlOf(CollisionPair);
				if (vehicleControl != null)
				{
					if (IsVehicleAlreadyExecutedIVehicleControl(vehicleControl.mVehicleId, vehicleControl) || IsIVehicleControlAlreadyExistedInManager(vehicleControl))
					{
						// do nothing
					}
					else // 若 IVehicleControl 尚未被 VehicleId 執行且該 IVehicleControl 不存在於 Manager 中
					{
						rVehicleControlManager.Add(vehicleControl.mName, vehicleControl);
					}
				}
				else
				{
					// do nothing
				}
			}
		}
		private void RemoveRelatedVehicleControl(ICollisionPair CollisionPair)
		{
			while (rVehicleControlManager.IsExistByCauseId(CollisionPair.mName))
			{
				string controlName = rVehicleControlManager.GetItemByCauseId(CollisionPair.mName).mName;
				if (rVehicleControlManager.GetItem(controlName).mSendState == SendState.Unsend)
				{
					rVehicleControlManager.UpdateExecuteFailedReason(controlName, FailedReason.CollisionRemoved);
					rVehicleControlManager.UpdateExecuteState(controlName, ExecuteState.ExecuteFailed);
					rVehicleControlManager.Remove(controlName);
				}
			}
		}
		private void UninterveneRelatedVehicle(ICollisionPair CollisionPair)
		{
			UninterveneVehicle(CollisionPair.mVehicle1, CollisionPair.mName, CollisionPair.ToString());
			UninterveneVehicle(CollisionPair.mVehicle2, CollisionPair.mName, CollisionPair.ToString());
		}
		private void UninterveneVehicle(IVehicleInfo VehicleInfo, string CauseId, string CauseDetail)
		{
			if (VehicleInfo != null)
			{
				// 如果「自走車已經被干預」或「有干預正在送給該自走車」
				if (IsVehicleBeenIntervened(VehicleInfo) || rVehicleControlManager.GetItems().Any(o => o.mVehicleId == VehicleInfo.mName && o.mCommand == Command.PauseMoving && o.mCauseId == CauseId && o.mSendState == SendState.Sending))
				{
					IVehicleControl vehicleControl = Library.Library.GenerateIVehicleControl(VehicleInfo.mName, Command.ResumeMoving, null, CauseId, CauseDetail);
					rVehicleControlManager.Add(vehicleControl.mName, vehicleControl);
				}
			}
		}
		private bool IsProcessNecessary(ICollisionPair CollisionPair)
		{
			bool result = false;
			if (CollisionPair != null)
			{
				// 若兩車皆未執行干預
				if (!IsVehicleBeenIntervened(CollisionPair.mVehicle1) && !IsVehicleBeenIntervened(CollisionPair.mVehicle2))
				{
					// 判斷哪台車會較晚進入 Collision Region
					if (CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart < CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart)
					{
						// 若較晚進入 Collision Region 的 Vehicle 即將進入 Collision Region
						if (DateTime.Now.Subtract(CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart).TotalMilliseconds > -3500.0f)
						{
							result = true;
						}
					}
					else
					{
						// 若較晚進入 Collision Region 的 Vehicle 即將進入 Collision Region
						if (DateTime.Now.Subtract(CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart).TotalMilliseconds > -3500.0f)
						{
							result = true;
						}
					}
				}
			}
			return result;
		}
		private IVehicleControl CalculateIVehicleControlOf(ICollisionPair CollisionPair)
		{
			IVehicleControl result = null;
			if (CollisionPair != null)
			{
				// 若兩車皆未執行干預
				if (!IsVehicleBeenIntervened(CollisionPair.mVehicle1) && !IsVehicleBeenIntervened(CollisionPair.mVehicle2))
				{
					// 對較晚進入 Collision Region 的 Vehicle 執行暫停動作
					if (CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart < CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart)
					{
						result = GenerateIVehicleControl(CollisionPair.mVehicle2.mName, Command.PauseMoving, null, CollisionPair.mName, CollisionPair.ToString());
					}
					else
					{
						result = GenerateIVehicleControl(CollisionPair.mVehicle1.mName, Command.PauseMoving, null, CollisionPair.mName, CollisionPair.ToString());
					}
				}
			}
			return result;
		}
		private bool IsVehicleBeenIntervened(IVehicleInfo VehicleInfo) // 判斷自走車是否已經被干預
		{
			return (VehicleInfo.mCurrentState == "Pause" || !string.IsNullOrEmpty(VehicleInfo.mCurrentInterveneCommand));
		}
		private bool IsVehicleAlreadyExecutedIVehicleControl(string VehicleId, IVehicleControl VehicleControl)
		{
			return rVehicleInfoManager.GetItem(VehicleId).mCurrentInterveneCommand.StartsWith(VehicleControl.mCommand.ToString());
		}
		private bool IsIVehicleControlAlreadyExistedInManager(IVehicleControl VehicleControl)
		{
			return rVehicleControlManager.GetItems().Any(o => o.mVehicleId == VehicleControl.mVehicleId && o.mCommand == VehicleControl.mCommand && o.mParametersString == VehicleControl.mParametersString && o.mCauseId == VehicleControl.mCauseId);
		}
	}
}
