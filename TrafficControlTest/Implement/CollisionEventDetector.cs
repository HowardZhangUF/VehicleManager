using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleCommunicator;

namespace TrafficControlTest.Implement
{
	public class CollisionEventDetector : ICollisionEventDetector
	{
		public event EventHandlerDateTime SystemStarted;
		public event EventHandlerDateTime SystemStopped;

		public bool mIsExcuting { get { return (mThdDetectCollisionEvent != null && mThdDetectCollisionEvent.IsAlive == true) ? true : false; } }

		private IVehicleInfoManager rVehicleInfoManager = null;
		private ICollisionEventManager rCollisionEventManager = null;
		private Thread mThdDetectCollisionEvent = null;

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
			mThdDetectCollisionEvent = new Thread(Task_DetectCollisionEvent);
			mThdDetectCollisionEvent.IsBackground = true;
			mThdDetectCollisionEvent.Start();
		}
		private void DestroyThread()
		{
			if (mThdDetectCollisionEvent != null)
			{
				if (mThdDetectCollisionEvent.IsAlive)
				{
					mThdDetectCollisionEvent.Abort();
				}
				mThdDetectCollisionEvent = null;
			}
		}
		private void Task_DetectCollisionEvent()
		{
			try
			{
				RaiseEvent_SystemStarted();
				while (true)
				{
					Subtask_DetectCollisionEvent();
					Thread.Sleep(750);
				}
			}
			catch (ThreadAbortException Ex)
			{
				Console.WriteLine(Ex.ToString());
			}
			finally
			{
				Subtask_DetectCollisionEvent();
				RaiseEvent_SystemStopped();
			}
		}
		private void Subtask_DetectCollisionEvent()
		{
			if (rVehicleInfoManager != null && rVehicleInfoManager.GetNames().Count > 0)
			{
				if (Library.Library.IsAnyCollisionPair(rVehicleInfoManager.GetList(), out IEnumerable<ICollisionPair> collisionPairs))
				{
					if (rCollisionEventManager != null)
					{
						foreach (ICollisionPair collisionPair in collisionPairs)
						{
							if (rCollisionEventManager.IsExist(collisionPair.mName))
							{
								rCollisionEventManager.Update(collisionPair.mName, collisionPair.mCollisionRegion, collisionPair.mPeriod);
							}
							else
							{
								rCollisionEventManager.Add(collisionPair.mName, collisionPair);
							}
						}
					}
				}
			}
		}
	}
}
