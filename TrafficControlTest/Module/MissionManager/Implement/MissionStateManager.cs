using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Implement;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class MissionStateManager : ItemManager<IMissionState>, IMissionStateManager
	{
		public IMissionState this[string MissionId] => GetItem(MissionId);

		public MissionStateManager()
		{
		}
		public void UpdateExecutorId(string MissionId, string ExecutorId)
		{
			lock (mLock)
			{
				if (mItems.Keys.Contains(MissionId))
				{
					mItems[MissionId].UpdateExecutorId(ExecutorId);
				}
			}
		}
		public void UpdateSendState(string MissionId, SendState SendState)
		{
			lock (mLock)
			{
				if (mItems.Keys.Contains(MissionId))
				{
					mItems[MissionId].UpdateSendState(SendState);
				}
			}
		}
		public void UpdateExecuteState(string MissionId, ExecuteState ExecuteState)
		{
			lock (mLock)
			{
				if (mItems.Keys.Contains(MissionId))
				{
					mItems[MissionId].UpdateExecuteState(ExecuteState);
				}
			}
		}
	}
}
