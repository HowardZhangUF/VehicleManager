using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Implement
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
			// sample function, undone.
			bool result = false;
			if (CollisionPair != null)
			{
				if (DateTime.Now.Subtract(CollisionPair.mPeriod.mStart).TotalMilliseconds > -2000.0f)
				{
					double aaa = DateTime.Now.Subtract(CollisionPair.mPeriod.mStart).TotalMilliseconds;
					result = true;
				}
			}
			return result;
		}
		private IVehicleControl CalculateIVehicleControlOf(ICollisionPair CollisionPair)
		{
			// sample function, undone.
			// 需考量車子狀態，若車子已經完成過干預指令了，只是平均速度尚未降下來，就不用再重複送干預指令了
			IVehicleControl result = null;
			if (CollisionPair != null)
			{
				result = GenerateIVehicleControl(CollisionPair.mVehicle1.mName, Command.PauseMoving, null, CollisionPair.mName, CollisionPair.ToString());
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
