using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Implement;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	/// <summary>
	/// - 儲存所有的任務資訊及狀態 (IMissionState)
	/// - 提供 Add(), Remove(), IsExist(), GetItem(), GetItems(), GetItemNames(), mCount 等方法、屬性供外部使用
	/// - 當物件的資訊(IVehicleInfo) 新增、移除、資訊更新時發生時會拋出事件
	/// </summary>
	public interface IMissionStateManager : IItemManager<IMissionState>
	{
		IMissionState this[string MissionId] { get; }

		bool IsExistByHostMissionId(string HostMissionId);
		void UpdateExecutorId(string MissionId, string ExecutorId);
		void UpdateSendState(string MissionId, SendState SendState);
		void UpdateExecuteState(string MissionId, ExecuteState ExecuteState);
	}
}
