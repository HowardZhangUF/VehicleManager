
using LibraryForVM;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.CollisionEvent
{
	/// <summary>「路徑線區域」重疊的組合</summary>
	public interface IPathRegionOverlapPair
	{
		IVehicleInfo mVehicle1 { get; }
		IVehicleInfo mVehicle2 { get; }
		/// <summary>「路徑線區域」重疊的區域。亦為「有機會」發生「路徑線」重疊的區域</summary>
		IRectangle2D mOverlapRegionOfPathRegions { get; }

		void Set(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D OverlapRegionOfPathRegions);
		string ToString();
	}
}
