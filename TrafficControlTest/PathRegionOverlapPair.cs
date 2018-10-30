using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>路徑線區域重疊的組合</summary>
	public class PathRegionOverlapPair
	{
		public AGVInfo AGV1 = null;
		public AGVInfo AGV2 = null;
		public Rectangle OverlapRegion = null;
		private PathRegionOverlapPair() { }

		/// <summary>計算路徑線區域是否有重疊</summary>
		public static PathRegionOverlapPair IsPathRegionOverlap(AGVInfo agv1, AGVInfo agv2)
		{
			PathRegionOverlapPair result = null;
			if (Rectangle.IsOverlap(agv1.PathRegion, agv2.PathRegion))
			{
				result = new PathRegionOverlapPair();
				result.AGV1 = agv1;
				result.AGV2 = agv2;
				result.OverlapRegion = Rectangle.GetIntersectionRectangle(agv1.PathRegion, agv2.PathRegion);
			}
			return result;
		}
	}
}
