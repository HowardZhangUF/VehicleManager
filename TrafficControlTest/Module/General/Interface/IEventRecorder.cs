using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Module.General.Interface
{
	/// <summary>提供記錄 Event (CurrentVehicleInfo, HistoryVehicleInfo, AllMissionState ... etc.) 至資料庫的功能</summary>
	public interface IEventRecorder
	{
		bool mIsExecuting { get; }

		void Set(DatabaseAdapter DatabaseAdapter);
		void Start();
		void Stop();
		void CreateTableOfHistoryVehicleInfo(string VehicleName);
		void RecordVehicleInfo(DatabaseDataOperation Action, IVehicleInfo VehicleInfo);
		void RecordHistoryVehicleInfo(DatabaseDataOperation Action, DateTime Timestamp, IVehicleInfo VehicleInfo);
		void RecordMissionState(DatabaseDataOperation Action, IMissionState MissionState);
	}
}
