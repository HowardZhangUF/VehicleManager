using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.General.Interface;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	/// <summary>
	/// - Reference: IHostCommunicator, IMissionStateManager, IMissionAnalyzer
	/// - 監控 IHostCommunicator 的 ReceivedData 事件及使用 IMissionAnalyzer 的解析成功與否來使用 IMissionStateManager 的 Add() 方法
	/// - 若解析成功/失敗則使用 IHostCommunicator 回覆解析成功/失敗訊息
	/// </summary>
	public interface IHostMessageAnalyzer
	{
		void Set(IHostCommunicator HostCommunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IMissionAnalyzer[] MissionAnalyzers);
		void Set(IHostCommunicator HostCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMissionAnalyzer[] MissionAnalyzers);
	}
}
