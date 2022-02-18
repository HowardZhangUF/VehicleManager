using LibraryForVM;
using TrafficControlTest.Module.InterveneCommand;

namespace TrafficControlTest.Module.Mission
{
	/// <summary>
	/// - 儲存所有的任務資訊及狀態 (IMissionState)
	/// - 提供 Add(), Remove(), IsExist(), GetItem(), GetItems(), GetItemNames(), mCount 等方法、屬性供外部使用
	/// - 當物件的資訊(IVehicleInfo) 新增、移除、資訊更新時發生時會拋出事件
	/// </summary>
	public interface IMissionStateManager : IItemManager<IMissionState>
	{
		bool IsExistByHostMissionId(string HostMissionId);
		void UpdateExecutorId(string MissionId, string ExecutorId);
		void UpdateExecuteState(string MissionId, ExecuteState ExecuteState);
        void UpdateFailedReason(string MissionId, FailedReason FailedReason);
	}
}
