using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Implement
{
	public class CollisionEventDetector : ICollisionEventDetector
	{
		public event EventHandlerDateTime SystemStarted;
		public event EventHandlerDateTime SystemStopped;

		public bool mIsExecuting
		{
			get
			{
				return _IsExecuting;
			}
			private set
			{
				_IsExecuting = value;
				if (_IsExecuting) RaiseEvent_SystemStarted();
				else RaiseEvent_SystemStopped();
			}
		}

		private IVehicleInfoManager rVehicleInfoManager = null;
		private ICollisionEventManager rCollisionEventManager = null;
		private Thread mThdDetectCollisionEvent = null;
		private bool[] mThdDetectCollisionEventExitFlag = null;
		private bool _IsExecuting = false;

		public CollisionEventDetector(IVehicleInfoManager VehicleInfoManager, ICollisionEventManager CollisionEventManager)
		{
			Set(VehicleInfoManager, CollisionEventManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(ICollisionEventManager CollisionEventManager)
		{
			UnsubscribeEvent_ICollisionEventManager(rCollisionEventManager);
			rCollisionEventManager = CollisionEventManager;
			SubscribeEvent_ICollisionEventManager(rCollisionEventManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, ICollisionEventManager CollisionEventManager)
		{
			Set(VehicleInfoManager);
			Set(CollisionEventManager);
		}
		public void Start()
		{
			InitializeThread();
		}
		public void Stop()
		{
			DestroyThread();
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
		private void SubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{

			}
		}
		private void UnsubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{

			}
		}
		protected virtual void RaiseEvent_SystemStarted(bool Sync = true)
		{
			if (Sync)
			{
				SystemStarted?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStarted?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_SystemStopped(bool Sync = true)
		{
			if (Sync)
			{
				SystemStopped?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStopped?.Invoke(DateTime.Now); });
			}
		}
		private void InitializeThread()
		{
			mThdDetectCollisionEventExitFlag = new bool[] { false };
			mThdDetectCollisionEvent = new Thread(() => Task_DetectCollisionEvent(mThdDetectCollisionEventExitFlag));
			mThdDetectCollisionEvent.IsBackground = true;
			mThdDetectCollisionEvent.Start();
		}
		private void DestroyThread()
		{
			if (mThdDetectCollisionEvent != null)
			{
				if (mThdDetectCollisionEvent.IsAlive)
				{
					mThdDetectCollisionEventExitFlag[0] = true;
				}
				mThdDetectCollisionEvent = null;
			}
		}
		private void Task_DetectCollisionEvent(bool[] ExitFlag)
		{
			try
			{
				mIsExecuting = true;
				while (!ExitFlag[0])
				{
					Subtask_DetectCollisionEvent();
					Thread.Sleep(750);
				}
			}
			finally
			{
				Subtask_DetectCollisionEvent();
				mIsExecuting = false;
			}
		}
		private void Subtask_DetectCollisionEvent()
		{
			if (rVehicleInfoManager != null && rVehicleInfoManager.GetItemNames() != null && rVehicleInfoManager.GetItemNames().Count() > 0)
			{
				if (IsAnyCollisionPair(rVehicleInfoManager.GetItems(), out IEnumerable<ICollisionPair> collisionPairs))
				{
					if (rCollisionEventManager != null)
					{
						foreach (ICollisionPair collisionPair in collisionPairs)
						{
							if (rCollisionEventManager.IsExist(collisionPair.mName))
							{
								rCollisionEventManager.Update(collisionPair.mName, collisionPair.mCollisionRegion, collisionPair.mPeriod, collisionPair.mPassPeriodOfVehicle1WithCurrentVelocity, collisionPair.mPassPeriodOfVehicle2WithCurrentVelocity, collisionPair.mPassPeriodOfVehicle1WithMaximumVeloctiy, collisionPair.mPassPeriodOfVehicle2WithMaximumVeloctiy);
							}
							else
							{
								rCollisionEventManager.Add(collisionPair.mName, collisionPair);
							}
						}
					}
				}

				// 若舊有的 Collision Pair 沒有新的、對應的 Collision Pair ，代表該 Collision Pair 被解除了
				List<string> collisionNameList = rCollisionEventManager.GetNames();
				if (collisionNameList != null && collisionNameList.Count > 0)
				{
					List<string> solvedCollisionPairs = rCollisionEventManager.GetList().Where((o) => !IsCorrespondenceExist(o, collisionPairs)).Select((o) => o.mName).ToList();
					if (solvedCollisionPairs != null && solvedCollisionPairs.Count > 0)
					{
						for (int i = 0; i < solvedCollisionPairs.Count; ++i)
						{
							rCollisionEventManager.Remove(solvedCollisionPairs[i]);
						}
					}
				}
			}
		}
	}
}
