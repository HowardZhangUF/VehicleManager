using Library;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.CollisionEvent
{
	class PathOverlapPair : IPathOverlapPair
	{
		public IVehicleInfo mVehicle1 { get; private set; }
		public IVehicleInfo mVehicle2 { get; private set; }
		public IEnumerable<IRectangle2D> mOverlapRegionsOfPaths { get; private set; }

		public PathOverlapPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IEnumerable<IRectangle2D> OverlapRegionsOfPaths)
		{
			Set(Vehicle1, Vehicle2, OverlapRegionsOfPaths);
		}
		public void Set(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IEnumerable<IRectangle2D> OverlapRegionsOfPaths)
		{
			mVehicle1 = Vehicle1;
			mVehicle2 = Vehicle2;
			mOverlapRegionsOfPaths = OverlapRegionsOfPaths;
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += $"============================================================";
			result += $"\n[ PathOverlapPair ]";
			result += $"\nVehicles: {mVehicle1.mName} & {mVehicle2.mName}";
			for (int i = 0; i < mOverlapRegionsOfPaths.Count(); ++i)
			{
				result += $"\nRegion{i + 1}: {mOverlapRegionsOfPaths.ElementAt(i).ToString()}";
			}
			result += $"\n============================================================";
			return result;
		}
	}
}
