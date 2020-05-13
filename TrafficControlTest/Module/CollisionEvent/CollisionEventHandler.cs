using System;
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
				rCollisionEventManager.CollisionEventAdded += HandleEvent_CollisionEventManagerCollisionEventAdded;
				rCollisionEventManager.CollisionEventRemoved += HandleEvent_CollisionEventManagerCollisionEventRemoved;
				rCollisionEventManager.CollisionEventStateUpdated += HandleEvent_CollisionEventManagerCollisionEventStateUpdated;
			}
		}
		private void UnsubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{
				rCollisionEventManager.CollisionEventAdded -= HandleEvent_CollisionEventManagerCollisionEventAdded;
				rCollisionEventManager.CollisionEventRemoved -= HandleEvent_CollisionEventManagerCollisionEventRemoved;
				rCollisionEventManager.CollisionEventStateUpdated -= HandleEvent_CollisionEventManagerCollisionEventStateUpdated;
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
		private void HandleEvent_CollisionEventManagerCollisionEventAdded(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleCollisionPair(CollisionPair);
		}
		private void HandleEvent_CollisionEventManagerCollisionEventRemoved(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			// 當 CollisionEvent 消失時，移除跟該 CollisionEvent 有關的且尚未被送出的 VehicleControl
			RemoveRelatedVehicleControl(CollisionPair);
			// 當 CollisionEvent 消失時，讓跟該 CollisionEvent 有關的 Vehicle 恢復成沒有被干預的狀態
			UninterveneRelatedVehicle(CollisionPair);
		}
		private void HandleEvent_CollisionEventManagerCollisionEventStateUpdated(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleCollisionPair(CollisionPair);
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
			if (rVehicleControlManager.IsExistByCauseId(CollisionPair.mName))
			{
				string controlName = rVehicleControlManager.GetItemByCauseId(CollisionPair.mName).mName;
				if (rVehicleControlManager.GetItem(controlName).mSendState == SendState.Unsend)
				{
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
			if (VehicleInfo != null && !string.IsNullOrEmpty(VehicleInfo.mCurrentInterveneCommand))
			{
				if (VehicleInfo.mCurrentInterveneCommand.StartsWith("InsertMovingBuffer"))
				{
					IVehicleControl vehicleControl = GenerateIVehicleControl(VehicleInfo.mName, Command.RemoveMovingBuffer, null, CauseId, CauseDetail);
					rVehicleControlManager.Add(vehicleControl.mName, vehicleControl);
				}
				else if (VehicleInfo.mCurrentInterveneCommand.StartsWith("PauseMoving"))
				{
					IVehicleControl vehicleControl = GenerateIVehicleControl(VehicleInfo.mName, Command.ResumeMoving, null, CauseId, CauseDetail);
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
				if (string.IsNullOrEmpty(CollisionPair.mVehicle1.mCurrentInterveneCommand) && string.IsNullOrEmpty(CollisionPair.mVehicle2.mCurrentInterveneCommand))
				{
					// 判斷哪台車會較晚進入 Collision Region
					if (CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart < CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart)
					{
						// 若較晚進入 Collision Region 的 Vehicle 即將進入 Collision Region
						if (DateTime.Now.Subtract(CollisionPair.mPassPeriodOfVehicle2WithCurrentVelocity.mStart).TotalMilliseconds > -2000.0f)
						{
							result = true;
						}
					}
					else
					{
						// 若較晚進入 Collision Region 的 Vehicle 即將進入 Collision Region
						if (DateTime.Now.Subtract(CollisionPair.mPassPeriodOfVehicle1WithCurrentVelocity.mStart).TotalMilliseconds > -2000.0f)
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
				if (string.IsNullOrEmpty(CollisionPair.mVehicle1.mCurrentInterveneCommand) && string.IsNullOrEmpty(CollisionPair.mVehicle2.mCurrentInterveneCommand))
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
		private bool IsVehicleAlreadyExecutedIVehicleControl(string VehicleId, IVehicleControl VehicleControl)
		{
			return rVehicleInfoManager.GetItem(VehicleId).mCurrentInterveneCommand.StartsWith(VehicleControl.mCommand.ToString());
		}
		private bool IsIVehicleControlAlreadyExistedInManager(IVehicleControl VehicleControl)
		{
			return rVehicleControlManager.IsExist(VehicleControl.mName);
		}
	}
}
