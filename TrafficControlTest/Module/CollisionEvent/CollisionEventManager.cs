﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CollisionEvent
{
	public class CollisionEventManager : ICollisionEventManager
	{
		public event EventHandlerICollisionPair CollisionEventAdded;
		public event EventHandlerICollisionPair CollisionEventRemoved;
		public event EventHandlerICollisionPair CollisionEventStateUpdated;

		public ICollisionPair this[string Name] { get { return Get(Name); } }

		private Dictionary<string, ICollisionPair> mCollisionPairs = new Dictionary<string, ICollisionPair>();

		public CollisionEventManager()
		{
		}
		public bool IsExist(string Name)
		{
			return mCollisionPairs.Keys.Contains(Name);
		}
		public ICollisionPair Get(string Name)
		{
			if (IsExist(Name))
			{
				return mCollisionPairs[Name];
			}
			else
			{
				return null;
			}
		}
		public List<string> GetNames()
		{
			if (mCollisionPairs.Count > 0)
			{
				return mCollisionPairs.Keys.ToList();
			}
			else
			{
				return null;
			}
		}
		public List<ICollisionPair> GetList()
		{
			if (mCollisionPairs.Count > 0)
			{
				return mCollisionPairs.Values.ToList();
			}
			else
			{
				return null;
			}
		}
		public void Add(string Name, ICollisionPair Data)
		{
			if (!IsExist(Name))
			{
				mCollisionPairs.Add(Name, Data);
				SubscribeEvent_ICollisionPair(mCollisionPairs[Name]);
				RaiseEvent_CollisionEventAdded(mCollisionPairs[Name].mName, mCollisionPairs[Name]);
			}
			else
			{
				Update(Name, Data.mCollisionRegion, Data.mPeriod, Data.mPassPeriodOfVehicle1WithCurrentVelocity, Data.mPassPeriodOfVehicle2WithCurrentVelocity, Data.mPassPeriodOfVehicle1WithMaximumVeloctiy, Data.mPassPeriodOfVehicle2WithMaximumVeloctiy);
			}
		}
		public void Remove(string Name)
		{
			if (IsExist(Name))
			{
				ICollisionPair tmpData = mCollisionPairs[Name];
				UnsubscribeEvent_ICollisionPair(mCollisionPairs[Name]);
				mCollisionPairs.Remove(Name);
				RaiseEvent_CollisionEventRemoved(Name, tmpData);
			}
		}
		public void Update(string Name, IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy)
		{
			if (IsExist(Name))
			{
				mCollisionPairs[Name].Update(CollisionRegion, Period, PassPeriodOfVehicle1WithCurrentVelocity, PassPeriodOfVehicle2WithCurrentVelocity, PassPeriodOfVehicle1WithMaximumVeloctiy, PassPeriodOfVehicle2WithMaximumVeloctiy);
			}
		}

		private void SubscribeEvent_ICollisionPair(ICollisionPair CollisionPair)
		{
			if (CollisionPair != null)
			{
				CollisionPair.StateUpdated += HandleEvent_CollisionPairStateUpdated;
			}
		}
		private void UnsubscribeEvent_ICollisionPair(ICollisionPair CollisionPair)
		{
			if (CollisionPair != null)
			{
				CollisionPair.StateUpdated -= HandleEvent_CollisionPairStateUpdated;
			}
		}
		protected virtual void RaiseEvent_CollisionEventAdded(string Name, ICollisionPair CollisionPair, bool Sync = true)
		{
			if (Sync)
			{
				CollisionEventAdded?.Invoke(DateTime.Now, Name, CollisionPair);
			}
			else
			{
				Task.Run(() => { CollisionEventAdded?.Invoke(DateTime.Now, Name, CollisionPair); });
			}
		}
		protected virtual void RaiseEvent_CollisionEventRemoved(string Name, ICollisionPair CollisionPair, bool Sync = true)
		{
			if (Sync)
			{
				CollisionEventRemoved?.Invoke(DateTime.Now, Name, CollisionPair);
			}
			else
			{
				Task.Run(() => { CollisionEventRemoved?.Invoke(DateTime.Now, Name, CollisionPair); });
			}
		}
		protected virtual void RaiseEvent_CollisionEventStateUpdated(string Name, ICollisionPair CollisionPair, bool Sync = true)
		{
			if (Sync)
			{
				CollisionEventStateUpdated?.Invoke(DateTime.Now, Name, CollisionPair);
			}
			else
			{
				Task.Run(() => { CollisionEventStateUpdated?.Invoke(DateTime.Now, Name, CollisionPair); });
			}
		}
		private void HandleEvent_CollisionPairStateUpdated(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			RaiseEvent_CollisionEventStateUpdated(Name, CollisionPair);
		}
	}
}
