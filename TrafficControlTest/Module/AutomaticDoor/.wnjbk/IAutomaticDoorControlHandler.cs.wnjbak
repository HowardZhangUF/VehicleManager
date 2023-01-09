using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.AutomaticDoor
{
	/// <summary>
	/// - 當 IAutomaticDoorControlManager 裡面有東西時，將其使用 IAutomaticDoorCommunicator 傳送給指定的自動門
	/// </summary>
	public interface IAutomaticDoorControlHandler : ISystemWithLoopTask
	{
		void Set(IAutomaticDoorControlManager AutomaticDoorControlManager);
		void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager);
		void Set(IAutomaticDoorCommunicator AutomaticDoorCommunicator);
		void Set(IAutomaticDoorControlManager AutomaticDoorControlManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator);
	}
}
