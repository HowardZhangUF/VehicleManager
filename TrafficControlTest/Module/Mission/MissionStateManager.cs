using System.Linq;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Mission
{
	public class MissionStateManager : ItemManager<IMissionState>, IMissionStateManager
	{
		public MissionStateManager()
		{
		}
		public bool IsExistByHostMissionId(string HostMissionId)
		{
			return mItems.Values.Any(o => o.mMission.mMissionId == HostMissionId);
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
