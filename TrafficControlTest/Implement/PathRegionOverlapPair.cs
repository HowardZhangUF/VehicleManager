using TrafficControlTest.Interface;

namespace TrafficControlTest.Implement
{
	class PathRegionOverlapPair : IPathRegionOverlapPair
	{
		public IVehicleInfo mVehicle1 { get; private set; }
		public IVehicleInfo mVehicle2 { get; private set; }
		public IRectangle2D mOverlapRegionOfPathRegions { get; private set; }

		public PathRegionOverlapPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D OverlapRegionOfPathRegions)
		{
			Set(Vehicle1, Vehicle2, OverlapRegionOfPathRegions);
		}
		public void Set(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D OverlapRegionOfPathRegions)
		{
			mVehicle1 = Vehicle1;
			mVehicle2 = Vehicle2;
			mOverlapRegionOfPathRegions = OverlapRegionOfPathRegions;
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += $"============================================================";
			result += $"\n[ PathRegionOverlapPair ]";
			result += $"\nVehicles: {mVehicle1.mName} & {mVehicle2.mName}";
			result += $"\nRegion: {mOverlapRegionOfPathRegions.ToString()}";
			result += $"\n============================================================\n";
			return result;
		}
	}
}
