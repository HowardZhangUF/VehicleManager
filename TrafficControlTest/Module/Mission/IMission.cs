﻿using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Mission
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
		string mParametersString { get; }

		void Set(MissionType MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters);
		void UpdatePriority(int Priority);
	}
}
