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
	public enum DatabaseDataOperation
	{
		Add,
		Remove,
		Update
	}

	public interface ILogRecorder
	{
		void Set(DatabaseAdapter DatabaseAdapter);
		void Start();
		void Stop();
		void RecordGeneralLog(string Timestamp, string Category, string SubCategory, string Message);
		void RecordVehicleInfo(DatabaseDataOperation Action, IVehicleInfo VehicleInfo);
		void RecordMissionState(DatabaseDataOperation Action, IMissionState MissionState);
	}
}
