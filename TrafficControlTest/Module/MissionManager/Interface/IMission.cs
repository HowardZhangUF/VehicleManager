using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	/// <summary>
	/// - 儲存任務的資訊
	/// </summary>
	public interface IMission
	{
		MissionType mMissionType { get; }
		string mMissionId { get; }
		int mPriority { get; }
		string mVehicleId { get; }
		string[] mParameters { get; }

		void Set(MissionType MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters);
		void UpdatePriority(int Priority);
	}
}
