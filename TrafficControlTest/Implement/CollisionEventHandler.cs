﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleCommunicator;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Implement
{
	class CollisionEventHandler : ICollisionEventHandler
	{
		private ICollisionEventManager rCollisionEventManager = null;
		private IVehicleControlManager rVehicleControlManager = null;

		public CollisionEventHandler(ICollisionEventManager CollisionEventManager, IVehicleControlManager VehicleControlManager)
		{
			Set(CollisionEventManager, VehicleControlManager);
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
		public void Set(ICollisionEventManager CollisionEventManager, IVehicleControlManager VehicleControlManager)
		{
			Set(CollisionEventManager);
			Set(VehicleControlManager);
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
		private void HandleEvent_CollisionEventManagerCollisionEventAdded(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleCollisionPair(CollisionPair);
		}
		private void HandleEvent_CollisionEventManagerCollisionEventRemoved(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			// 當 CollisionEvent 消失時，移除跟該 CollisionEvent 有關的 VehicleControl
			RemoveRelatedVehicleControl(CollisionPair);
			// 當 CollisionEvent 消失時，讓跟該 CollisionEvent 有關的 Vehicle 恢復成沒有被干預的狀態
			UninterveneVehicle(CollisionPair);
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
				IVehicleControl vehicleControl = CalculateSolutionOf(CollisionPair);
				if (vehicleControl != null)
				{
					if (IsSolutionExist(vehicleControl))
					{

					}
					else
					{
						rVehicleControlManager.Add(vehicleControl.mName, vehicleControl);
					}
				}
				else
				{

				}
			}
		}
		private void RemoveRelatedVehicleControl(ICollisionPair CollisionPair)
		{
			if (rVehicleControlManager.IsCauseExist(CollisionPair.mName))
			{
				string controlName = rVehicleControlManager.GetViaCause(CollisionPair.mName).mName;
				rVehicleControlManager.Remove(controlName);
			}
		}
		private void UninterveneVehicle(ICollisionPair CollisionPair)
		{
			UninterveneVehicle(CollisionPair.mVehicle1);
			UninterveneVehicle(CollisionPair.mVehicle2);
		}
		private void UninterveneVehicle(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null && VehicleInfo.mIsBeingIntervened)
			{
				if (VehicleInfo.mInterveneCommand.StartsWith("Insert"))
				{
					IVehicleControl vehicleControl = GenerateIVehicleControl(VehicleInfo.mName, Command.RemoveMovingBuffer, null, string.Empty, string.Empty);
					rVehicleControlManager.Add(vehicleControl.mName, vehicleControl);
				}
				else if (VehicleInfo.mInterveneCommand.StartsWith("Pause"))
				{
					IVehicleControl vehicleControl = GenerateIVehicleControl(VehicleInfo.mName, Command.ResumeMoving, null, string.Empty, string.Empty);
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
		private IVehicleControl CalculateSolutionOf(ICollisionPair CollisionPair)
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
		private bool IsSolutionExist(IVehicleControl VehicleControl)
		{
			return rVehicleControlManager.IsExist(VehicleControl.mName);
		}
	}
}
