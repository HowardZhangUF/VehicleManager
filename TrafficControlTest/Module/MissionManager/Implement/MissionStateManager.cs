﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class MissionStateManager : IMissionStateManager
	{
		public event EventHandlerIMissionState MissionStateAdded;
		public event EventHandlerIMissionState MissionStateRemoved;
		public event EventHandlerIMissionStateStateUpdated MissionStateStateUpdated;

		public IMissionState this[string MissionId] => Get(MissionId);

		private Dictionary<string, IMissionState> mMissionStates = new Dictionary<string, IMissionState>();
		private readonly object mLock = new object();

		public MissionStateManager()
		{
		}
		public bool IsExist(string MissionId)
		{
			bool result = false;
			lock (mLock)
			{
				result = mMissionStates.Keys.Contains(MissionId);
			}
			return result;
		}
		public IMissionState Get(string MissionId)
		{
			IMissionState result = null;
			lock (mLock)
			{
				if (mMissionStates.Keys.Contains(MissionId))
				{
					result = mMissionStates[MissionId];
				}
			}
			return result;
		}
		public List<string> GetMissionIds()
		{
			List<string> result = null;
			lock (mLock)
			{
				if (mMissionStates.Count > 0)
				{
					result = mMissionStates.Keys.ToList();
				}
			}
			return result;
		}
		public List<IMissionState> GetList()
		{
			List<IMissionState> result = null;
			lock (mLock)
			{
				if (mMissionStates.Count > 0)
				{
					result = mMissionStates.Values.ToList();
				}
			}
			return result;
		}
		public void Add(string MissionId, IMissionState MissionState)
		{
			lock (mLock)
			{
				if (!mMissionStates.Keys.Contains(MissionId))
				{
					mMissionStates.Add(MissionId, MissionState);
					SubscribeEvent_IMissionState(mMissionStates[MissionId]);
					RaiseEvent_MissionStateAdded(mMissionStates[MissionId].mMissionId, mMissionStates[MissionId]);
				}
			}
		}
		public void Remove(string MissionId)
		{
			lock (mLock)
			{
				if (mMissionStates.Keys.Contains(MissionId))
				{
					IMissionState tmpData = mMissionStates[MissionId];
					UnsubscribeEvent_IMissionState(mMissionStates[MissionId]);
					mMissionStates.Remove(MissionId);
					RaiseEvent_MissionStateRemoved(MissionId, tmpData);
				}
			}
		}
		public void UpdateExecutorId(string MissionId, string ExecutorId)
		{
			lock (mLock)
			{
				if (mMissionStates.Keys.Contains(MissionId))
				{
					mMissionStates[MissionId].UpdateExecutorId(ExecutorId);
				}
			}
		}
		public void UpdateSendState(string MissionId, SendState SendState)
		{
			lock (mLock)
			{
				if (mMissionStates.Keys.Contains(MissionId))
				{
					mMissionStates[MissionId].UpdateSendState(SendState);
				}
			}
		}
		public void UpdateExecuteState(string MissionId, ExecuteState ExecuteState)
		{
			lock (mLock)
			{
				if (mMissionStates.Keys.Contains(MissionId))
				{
					mMissionStates[MissionId].UpdateExecuteState(ExecuteState);
				}
			}
		}

		private void SubscribeEvent_IMissionState(IMissionState MissionState)
		{
			if (MissionState != null)
			{
				MissionState.StateUpdated += HandleEvent_MissionStateStateUpdated;
			}
		}
		private void UnsubscribeEvent_IMissionState(IMissionState MissionState)
		{
			if (MissionState != null)
			{
				MissionState.StateUpdated -= HandleEvent_MissionStateStateUpdated;
			}
		}
		protected virtual void RaiseEvent_MissionStateAdded(string MissionId, IMissionState MissionState, bool Sync = true)
		{
			if (Sync)
			{
				MissionStateAdded?.Invoke(DateTime.Now, MissionId, MissionState);
			}
			else
			{
				Task.Run(() => { MissionStateAdded?.Invoke(DateTime.Now, MissionId, MissionState); });
			}
		}
		protected virtual void RaiseEvent_MissionStateRemoved(string MissionId, IMissionState MissionState, bool Sync = true)
		{
			if (Sync)
			{
				MissionStateRemoved?.Invoke(DateTime.Now, MissionId, MissionState);
			}
			else
			{
				Task.Run(() => { MissionStateRemoved?.Invoke(DateTime.Now, MissionId, MissionState); });
			}
		}
		protected virtual void RaiseEvent_MissionStateStateUpdated(string MissionId, string StateName, IMissionState MissionState, bool Sync = true)
		{
			if (Sync)
			{
				MissionStateStateUpdated?.Invoke(DateTime.Now, MissionId, StateName, MissionState);
			}
			else
			{
				Task.Run(() => { MissionStateStateUpdated?.Invoke(DateTime.Now, MissionId, StateName, MissionState); });
			}
		}
		private void HandleEvent_MissionStateStateUpdated(DateTime OccurTime, string MissionId, string StateName, IMissionState MissionState)
		{
			RaiseEvent_MissionStateStateUpdated(MissionId, StateName, MissionState);
		}
	}
}
