using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.AutomaticDoor
{
	/// <summary>
	/// - 根據 IAutomaticDoorInfoManager 的 Added/Removed 事件來新增/移除 IAutomaticDoorCommunicator 的通訊功能
	/// </summary>
	public interface IAutomaticDoorCommunicatorUpdater
	{
		void Set(IAutomaticDoorCommunicator AutomaticDoorCommunicator);
		void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager);
		void Set(IAutomaticDoorCommunicator AutomaticDoorCommunicator, IAutomaticDoorInfoManager AutomaticDoorInfoManager);
	}
}
