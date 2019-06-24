using System.Collections.Generic;

namespace TrafficControlTest.Interface
{
	/// <summary>「路徑線」重疊的組合</summary>
	public interface IPathOverlapPair
	{
		IVehicleInfo mVehicle1 { get; }
		IVehicleInfo mVehicle2 { get; }
		/// <summary>「路徑線」重疊的區域。可能會有多個</summary>
		IEnumerable<IRectangle2D> mOverlapRegionsOfPaths { get; }

		void Set(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IEnumerable<IRectangle2D> OverlapRegionsOfPaths);
		string ToString();
	}
}
