using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	/// <summary>
	/// 監控 IMissionStateManager 的 ItemAdded 事件，來向客戶端系統發送 MissionCreated 訊息
	/// 監控 IMissionStateManager 的 ItemUpdated 事件，來向客戶端系統發送 MissionStarted, MissionCompleted, MissionCanceled, MissionAborted 訊息
	/// </summary>
	public interface IMissionStateReporter
	{
		void Set(IMissionStateManager MissionStateManager);
		void Set(IHostCommunicator HostCommunicator);
		void Set(IMissionStateManager MissionStateManager, IHostCommunicator HostCommunicator);
	}
}
