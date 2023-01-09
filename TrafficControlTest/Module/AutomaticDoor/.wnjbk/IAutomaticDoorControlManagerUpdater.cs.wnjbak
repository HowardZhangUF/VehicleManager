using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.AutomaticDoor
{
	/// <summary>
	/// - 根據 IAutomaticDoorInfoManager 的 ItemUpdated 事件，當 SendState 變成 SendSuccessed/SendFailed 時使用 Remove() 方法將其移除
	/// - 根據 IAutomaticDoorCommunicator 的 SentData 事件來更新對應的 IAutomaticDoorControl 的 SendState
	/// </summary>
	public interface IAutomaticDoorControlManagerUpdater
	{
		void Set(IAutomaticDoorControlManager AutomaticDoorControlManager);
		void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager);
		void Set(IAutomaticDoorCommunicator AutomaticDoorCommunicator);
		void Set(IAutomaticDoorControlManager AutomaticDoorControlManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator);
	}
}
