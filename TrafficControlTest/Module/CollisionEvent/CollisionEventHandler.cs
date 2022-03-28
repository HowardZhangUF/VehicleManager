using LibraryForVM;
using System;
using System.Linq;
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
			if (CollisionPair != null)
			{
				// 如果兩車都尚未被暫停
				if (!IsVehicleBeenPaused(CollisionPair.mVehicle1) && !IsVehicleBeenPaused(CollisionPair.mVehicle2))
				{
					// 如果事件即將發生
					if (IsEventAboutToHappen(CollisionPair))
					{
						IVehicleControl vehicleControl = CalculateIVehicleControl(CollisionPair);
						if (vehicleControl != null)
						{
							if (!IsAlreadyExist(vehicleControl, rVehicleControlManager))
							{
								rVehicleControlManager.Add(vehicleControl.mName, vehicleControl);
							}
						}
					}
				}
				// 下方目的：會車框有時候會移動，原本被暫停的車應該恢復動作，等到再次靠近會車框的時候再做暫停
				// 如果已經干預某台車了
				else if ((IsVehicleBeenPaused(CollisionPair.mVehicle1) && IsVehicleInterveneBy(CollisionPair.mVehicle1, CollisionPair.mName)) || (IsVehicleBeenPaused(CollisionPair.mVehicle2) && IsVehicleInterveneBy(CollisionPair.mVehicle2, CollisionPair.mName)))
				{
					// 確認是不是遠離會車框，如果是，則讓該車恢復動作
					if (IsEventFarToHappen(CollisionPair, 4000))
					{
						string pausedVehicleId = IsVehicleBeenPaused(CollisionPair.mVehicle1) ? CollisionPair.mVehicle1.mName : CollisionPair.mVehicle2.mName;
						IVehicleControl vehicleControl = GenerateIVehicleControl(pausedVehicleId, Command.ResumeMoving, null, CollisionPair.mName, CollisionPair.ToString() + "/HandleCollisionPair1");
						if (vehicleControl != null)
						{
							if (!IsAlreadyExist(vehicleControl, rVehicleControlManager))
							{
								rVehicleControlManager.Add(vehicleControl.mName, vehicleControl);
							}
						}
					}
				}
			}
		}
		private void RemoveRelatedVehicleControl(ICollisionPair CollisionPair)
		{
			while (rVehicleControlManager.IsExistByCauseId(CollisionPair.mName))
			{
				IVehicleControl control = rVehicleControlManager.GetItemByCauseId(CollisionPair.mName);
				if (control != null && control.mSendState == SendState.Unsend)
				{
					rVehicleControlManager.UpdateExecuteFailedReason(control.mName, FailedReason.CollisionRemoved);
					rVehicleControlManager.UpdateExecuteState(control.mName, ExecuteState.ExecuteFailed);
				}
				System.Threading.Thread.Sleep(200);
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
				// 如果「自走車已經被干預且是因為此 Collision 」或「有干預正在送給該自走車」
				if ((IsVehicleBeenPaused(VehicleInfo) && IsVehicleInterveneBy(VehicleInfo, CauseId)) || rVehicleControlManager.GetItems().Any(o => o.mVehicleId == VehicleInfo.mName && o.mCommand == Command.PauseMoving && o.mCauseId == CauseId && o.mSendState == SendState.Sending))
				{
					IVehicleControl vehicleControl = Library.Library.GenerateIVehicleControl(VehicleInfo.mName, Command.ResumeMoving, null, CauseId, CauseDetail + "/UninterveneVehicle1");
					rVehicleControlManager.Add(vehicleControl.mName, vehicleControl);
				}
			}
		}

		private static bool IsVehicleBeenPaused(IVehicleInfo VehicleInfo)
		{
			return VehicleInfo.mCurrentState == "Pause";
		}
		private static bool IsVehicleInterveneBy(IVehicleInfo VehicleInfo, string CauseId)
		{
			return VehicleInfo.mCurrentInterveneCause == CauseId;
		}
		private static bool IsEventAboutToHappen(ICollisionPair CollisionPair, float TimeThreshold = 3500.0f)
		{
			bool result = false;
			if (CollisionPair != null)
			{
				// 若較晚進入 Collision Region 的 Vehicle 即將進入 Collision Region
				if (CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart < CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart)
				{
					if (DateTime.Now.Subtract(CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart).TotalMilliseconds > -1 * TimeThreshold)
					{
						result = true;
					}
				}
				else
				{
					if (DateTime.Now.Subtract(CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart).TotalMilliseconds > -1 * TimeThreshold)
					{
						result = true;
					}
				}
			}
			return result;
		}
		private static bool IsEventFarToHappen(ICollisionPair CollisionPair, float TimeThreshold = 3500.0f)
		{
			bool result = false;
			if (CollisionPair != null)
			{
				// 若較晚進入 Collision Region 的 Vehicle 即將進入 Collision Region
				if (CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart < CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart)
				{
					if (DateTime.Now.Subtract(CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart).TotalMilliseconds < -1 * TimeThreshold)
					{
						result = true;
					}
				}
				else
				{
					if (DateTime.Now.Subtract(CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart).TotalMilliseconds < -1 * TimeThreshold)
					{
						result = true;
					}
				}
			}
			return result;
		}
		private static IVehicleControl CalculateIVehicleControl(ICollisionPair CollisionPair)
		{
			IVehicleControl result = null;
			if (CollisionPair != null)
			{
				// 如果兩車都已經在會車框裡面了
				if (CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart <= DateTime.Now && CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart <= DateTime.Now)
				{
					// 如果兩車的終點不同，讓較晚出來的車執行暫停
					if (CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mEnd != CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mEnd)
					{
						if (CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mEnd < CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mEnd)
						{
							result = GenerateIVehicleControl(CollisionPair.mVehicle2.mName, Command.PauseMoving, null, CollisionPair.mName, CollisionPair.ToString() + "/CalculateIVehicleControl1");
						}
						else
						{
							result = GenerateIVehicleControl(CollisionPair.mVehicle1.mName, Command.PauseMoving, null, CollisionPair.mName, CollisionPair.ToString() + "/CalculateIVehicleControl2");
						}
					}
					// 如果兩車的終點相同，讓路徑線長度較長的車執行暫停
					else
					{
						if (GeometryAlgorithm.GetDistance(CollisionPair.mVehicle1.mPath) < GeometryAlgorithm.GetDistance(CollisionPair.mVehicle2.mPath))
						{
							result = GenerateIVehicleControl(CollisionPair.mVehicle2.mName, Command.PauseMoving, null, CollisionPair.mName, CollisionPair.ToString() + "/CalculateIVehicleControl3");
						}
						else
						{
							result = GenerateIVehicleControl(CollisionPair.mVehicle1.mName, Command.PauseMoving, null, CollisionPair.mName, CollisionPair.ToString() + "/CalculateIVehicleControl4");
						}
					}
				}
				else
				{
					// 對較晚進入 Collision Region 的 Vehicle 執行暫停動作
					if (CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart < CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart)
					{
						result = GenerateIVehicleControl(CollisionPair.mVehicle2.mName, Command.PauseMoving, null, CollisionPair.mName, CollisionPair.ToString() + "/CalculateIVehicleControl5");
					}
					else
					{
						result = GenerateIVehicleControl(CollisionPair.mVehicle1.mName, Command.PauseMoving, null, CollisionPair.mName, CollisionPair.ToString() + "/CalculateIVehicleControl6");
					}
				}
			}
			return result;
		}
		private static bool IsAlreadyExist(IVehicleControl VehicleControl, IVehicleControlManager VehicleControlManager)
		{
			return VehicleControlManager.GetItems().Any(o => o.mVehicleId == VehicleControl.mVehicleId && o.mCommand == VehicleControl.mCommand && o.mParametersString == VehicleControl.mParametersString && o.mCauseId == VehicleControl.mCauseId);
		}
	}
}
