using System.Linq;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.InterveneCommand;

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
        public void UpdateFailedReason(string MissionId, FailedReason FailedReason)
        {
            lock (mLock)
            {
                if (mItems.Keys.Contains(MissionId))
                {
                    mItems[MissionId].UpdateFailedReason(FailedReason);
                }
            }
        }
	}
}
