using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Mission
{
	public class Mission : IMission
	{
		public MissionType mMissionType { get; private set; }
		public string mMissionId { get; private set; }
		public int mPriority { get; private set; }
		public string mVehicleId { get; private set; }
		public string[] mParameters { get; private set; }
		public string mParametersString { get { return mParameters == null ? string.Empty : string.Join(",", mParameters); } }

		public Mission(MissionType MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters)
		{
			Set(MissionType, MissionId, Priority, VehicleId, Parameters);
		}
		public void Set(MissionType MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters)
		{
			mMissionType = MissionType;
			mMissionId = MissionId;
			mPriority = Priority;
			mVehicleId = VehicleId;
			mParameters = Parameters;
		}
		public void UpdatePriority(int Priority)
		{
			mPriority = Priority;
		}
		public override string ToString()
		{
			return $"{mMissionType.ToString()}/{mParametersString}/{mMissionId}/{mPriority.ToString()}/{mVehicleId}";
		}
	}
}
