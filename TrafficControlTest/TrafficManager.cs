using Geometry;
using GLCore;
using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	class TrafficManager
	{
		public static void test()
		{
			// 產生車子假資料
			AGVInfo agv1 = new AGVInfo();
			agv1.Status = AGVStatus.CreateFakeData1();
			agv1.Path = AGVPath.CreateFakeData1();
			agv1.FrameRadius = 300;

			// 繪製車子路徑線點與路徑線區域
			GLCMD.CMD.AddMultiPair("@ObstaclePoints", agv1.PathPoints);
			List<Area> aaa = new List<Area>();
			aaa.Add(new Area(agv1.PathRegion.XMin, agv1.PathRegion.YMin, agv1.PathRegion.XMax, agv1.PathRegion.YMax));
			GLCMD.CMD.AddMultiArea("ForbiddenArea", aaa);

			// 產生車子假資料
			AGVInfo agv2 = new AGVInfo();
			agv2.Status = AGVStatus.CreateFakeData2();
			agv2.Path = AGVPath.CreateFakeData2();
			agv2.FrameRadius = 300;

			// 繪製車子路徑線點與路徑線區域
			GLCMD.CMD.AddMultiPair("@ObstaclePoints", agv2.PathPoints);
			List<Area> bbb = new List<Area>();
			bbb.Add(new Area(agv2.PathRegion.XMin, agv2.PathRegion.YMin, agv2.PathRegion.XMax, agv2.PathRegion.YMax));
			GLCMD.CMD.AddMultiArea("ForbiddenArea", bbb);

			// 計算「路徑線區域重疊的組合」與「路徑線重疊的組合」與「發生交會的組合」
			PathRegionOverlapPair pair1 = PathRegionOverlapPair.IsPathRegionOverlap(agv1, agv2);
			PathOverlapPair pair2 = PathOverlapPair.IsPathOverlap(pair1);
			CollisionPair pair3 = CollisionPair.IsCollision(pair2);

			if (pair1 != null) Console.WriteLine(pair1.ToString());
			if (pair2 != null) Console.WriteLine(pair2.ToString());
			if (pair3 != null) Console.WriteLine(pair3.ToString());

			// 繪製「路徑線區域重疊的組合的重疊區域」與「路徑線重疊的組合的重疊區域」
			List<Area> ccc = new List<Area>();
			ccc.Add(new Area(pair1.OverlapRegion.XMin, pair1.OverlapRegion.YMin, pair1.OverlapRegion.XMax, pair1.OverlapRegion.YMax));
			for (int i = 0; i < pair2.OverlapRegions.Count(); ++i)
				ccc.Add(new Area(pair2.OverlapRegions[i].XMin, pair2.OverlapRegions[i].YMin, pair2.OverlapRegions[i].XMax, pair2.OverlapRegions[i].YMax));
			ccc.Add(new Area(pair3.CollisionRegion.XMin, pair3.CollisionRegion.YMin, pair3.CollisionRegion.XMax, pair3.CollisionRegion.YMax));
			GLCMD.CMD.AddMultiArea("ForbiddenArea", ccc);
		}
	}
}
