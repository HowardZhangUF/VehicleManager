using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	public interface IMission
	{
		string mMissionType { get; }
		string mMissionId { get; }
		int mPriority { get; }
		string mVehicleId { get; }
		string[] mParameters { get; }

		void Set(string MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters);
		void UpdatePriority(int Priority);
	}
}
