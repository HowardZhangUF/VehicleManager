using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.Map;

namespace TrafficControlTest.Module.AutomaticDoor
{
	/// <summary>
	/// - 根據 IMapManager 的 LoadMapSuccessed 事件來新增/移除 IAutomaticDoorInfoManager 的成員
	/// - 根據 IAutomaticDoorCommunicator 的 RemoteConnectStateChanged/SentData 事件來更新 IAutomaticDoorInfoManager 的成員的屬性
	/// </summary>
	public interface IAutomaticDoorInfoManagerUpdater
	{
		void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager);
		void Set(IMapManager MapManager);
		void Set(IAutomaticDoorCommunicator AutomaticDoorCommunicator);
		void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager, IMapManager MapManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator);
	}
}
