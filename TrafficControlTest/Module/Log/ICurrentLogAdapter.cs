using LibraryForVM;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	/// <summary>提供記錄 CurrentVehicleInfo 至資料庫的功能</summary>
	public interface ICurrentLogAdapter
	{
		bool mIsExecuting { get; }

		void Set(DatabaseAdapter DatabaseAdapter);
		void Start();
		void Stop();
		void RecordCurrentVehicleInfo(DatabaseDataOperation Action, IVehicleInfo VehicleInfo);
	}
}
