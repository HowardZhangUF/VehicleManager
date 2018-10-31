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

		public static void CalculateCollisionEvent(List<AGVInfo> agvs)
		{
			// 計算「路徑線區域重疊的組合」
			List<PathRegionOverlapPair> pathRegionOverlapPairs = null;
			if (agvs != null && agvs.Count() > 1)
			{
				for (int i = 0; i < agvs.Count(); ++i)
				{
					for (int j = i + 1; j < agvs.Count(); ++j)
					{
						PathRegionOverlapPair pathRegionOverlapPair = PathRegionOverlapPair.IsPathRegionOverlap(agvs[i], agvs[j]);
						if (pathRegionOverlapPair != null)
						{
							if (pathRegionOverlapPairs == null) pathRegionOverlapPairs = new List<PathRegionOverlapPair>();
							pathRegionOverlapPairs.Add(pathRegionOverlapPair);
						}
					}
				}
			}

			// 計算「路徑線重疊的組合」
			List<PathOverlapPair> pathOverlapPairs = null;
			if (pathRegionOverlapPairs != null && pathRegionOverlapPairs.Count() > 0)
			{
				for (int i = 0; i < pathRegionOverlapPairs.Count(); ++i)
				{
					PathOverlapPair pathOverlapPair = PathOverlapPair.IsPathOverlap(pathRegionOverlapPairs[i]);
					if (pathOverlapPair != null)
					{
						if (pathOverlapPairs == null) pathOverlapPairs = new List<PathOverlapPair>();
						pathOverlapPairs.Add(pathOverlapPair);
					}
				}
			}

			// 計算「發生交會的組合」
			List<CollisionPair> collisionPairs = null;
			if (pathOverlapPairs != null && pathOverlapPairs.Count() > 0)
			{
				for (int i = 0; i < pathOverlapPairs.Count(); ++i)
				{
					CollisionPair collisionPair = CollisionPair.IsCollision(pathOverlapPairs[i]);
					if (collisionPair != null)
					{
						if (collisionPairs == null) collisionPairs = new List<CollisionPair>();
						collisionPairs.Add(collisionPair);
					}
				}
			}

			#region 資訊輸出

			// 輸出路徑線資訊
			PrintPathLines(agvs);
			PrintPathPoints(agvs);

			// 輸出「路徑線重疊的組合」的重疊區域
			PrintPathOverlapRegions(pathOverlapPairs);

			// 輸出「發生交會的組合」的交會區域
			PrintCollisionRegions(collisionPairs);

			// 輸出「發生交會的組合」的資訊
			if (collisionPairs != null && collisionPairs.Count() > 0)
			{
				for (int i = 0; i < collisionPairs.Count(); ++i)
				{
					Console.WriteLine(collisionPairs[i].ToString());
					Console.WriteLine("----------------------------------------");
				}
			}

			#endregion
		}

		public static List<AGVInfo> CreateFakeData()
		{
			AGVInfo agv1 = new AGVInfo();
			agv1.Status = AGVStatus.CreateFakeData1();
			agv1.Path = AGVPath.CreateFakeData1();
			agv1.FrameRadius = 1000;

			AGVInfo agv2 = new AGVInfo();
			agv2.Status = AGVStatus.CreateFakeData2();
			agv2.Path = AGVPath.CreateFakeData2();
			agv2.FrameRadius = 1000;

			AGVInfo agv3 = new AGVInfo();
			agv3.Status = AGVStatus.CreateFakeData3();
			agv3.Path = AGVPath.CreateFakeData3();
			agv3.FrameRadius = 1000;

			AGVInfo agv4 = new AGVInfo();
			agv4.Status = AGVStatus.CreateFakeData4();
			agv4.Path = AGVPath.CreateFakeData4();
			agv4.FrameRadius = 1000;

			AGVInfo agv5 = new AGVInfo();
			agv5.Status = AGVStatus.CreateFakeData5();
			agv5.Path = AGVPath.CreateFakeData5();
			agv5.FrameRadius = 1000;

			AGVInfo agv6 = new AGVInfo();
			agv6.Status = AGVStatus.CreateFakeData6();
			agv6.Path = AGVPath.CreateFakeData6();
			agv6.FrameRadius = 1000;

			List<AGVInfo> result = new List<AGVInfo>();
			result.Add(agv1);
			result.Add(agv2);
			result.Add(agv3);
			result.Add(agv4);
			result.Add(agv5);
			result.Add(agv6);

			return result;
		}

		public static void PrintPathPoints(List<AGVInfo> agvs)
		{
			if (agvs != null && agvs.Count() > 0)
			{
				for (int i = 0; i < agvs.Count(); ++i)
				{
					GLCMD.CMD.AddMultiPair("PathPoint", agvs[i].PathPoints);
				}
			}
		}

		public static void PrintPathLines(List<AGVInfo> agvs)
		{
			if (agvs != null && agvs.Count() > 0)
			{
				for (int i = 0; i < agvs.Count(); ++i)
				{
					GLCMD.CMD.AddMultiStripLine("PathLine", agvs[i].PathPoints);
				}
			}
		}

		public static void PrintPathOverlapRegions(List<PathOverlapPair> pathOverlapPairs)
		{
			if (pathOverlapPairs != null && pathOverlapPairs.Count() > 0)
			{
				List<Area> pathOverlapAreas = new List<Area>();
				for (int i = 0; i < pathOverlapPairs.Count(); ++i)
				{
					for (int j = 0; j < pathOverlapPairs[i].OverlapRegions.Count(); ++j)
					{
						pathOverlapAreas.Add(new Area(pathOverlapPairs[i].OverlapRegions[j].XMin, pathOverlapPairs[i].OverlapRegions[j].YMin, pathOverlapPairs[i].OverlapRegions[j].XMax, pathOverlapPairs[i].OverlapRegions[j].YMax));
					}
				}
				GLCMD.CMD.AddMultiArea("PathOverlapArea", pathOverlapAreas);
			}
		}

		public static void PrintCollisionRegions(List<CollisionPair> collisionPairs)
		{
			if (collisionPairs != null && collisionPairs.Count() > 0)
			{
				List<Area> collisionAreas = new List<Area>();
				for (int i = 0; i < collisionPairs.Count(); ++i)
				{
					collisionAreas.Add(new Area(collisionPairs[i].CollisionRegion.XMin, collisionPairs[i].CollisionRegion.YMin, collisionPairs[i].CollisionRegion.XMax, collisionPairs[i].CollisionRegion.YMax));
				}
				GLCMD.CMD.AddMultiArea("CollisionArea", collisionAreas);
			}
		}
	}
}
