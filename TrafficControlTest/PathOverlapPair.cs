using Geometry;
using KdTree;
using KdTree.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>路徑線重疊的組合</summary>
	public class PathOverlapPair
	{
		const int NUMBER = 10;
		public AGVInfo AGV1 = null;
		public AGVInfo AGV2 = null;
		public List<Rectangle> OverlapRegions = null;
		private PathOverlapPair() { }

		/// <summary>計算路徑線是否有重疊</summary>
		public static PathOverlapPair IsPathOverlap(PathRegionOverlapPair pair)
		{
			PathOverlapPair result = null;
			List<Rectangle> overlapRegions = null;

			//
			KdTree<int, string> agv1PathTree = BuildTree(pair.AGV1, pair.OverlapRegion);
			List<Rectangle> agv2PathOverlapRegions = CalculatePathOverlapRegions(pair.AGV2, pair.OverlapRegion, agv1PathTree);

			//
			KdTree<int, string> agv2PathTree = BuildTree(pair.AGV2, pair.OverlapRegion);
			List<Rectangle> agv1PathOverlapRegions = CalculatePathOverlapRegions(pair.AGV1, pair.OverlapRegion, agv2PathTree);

			if ((agv1PathOverlapRegions != null && agv1PathOverlapRegions.Count > 0) || (agv2PathOverlapRegions != null && agv2PathOverlapRegions.Count > 0))
			{
				overlapRegions = new List<Rectangle>();
				if (agv1PathOverlapRegions != null && agv1PathOverlapRegions.Count > 0) overlapRegions.AddRange(agv1PathOverlapRegions);
				if (agv2PathOverlapRegions != null && agv2PathOverlapRegions.Count > 0) overlapRegions.AddRange(agv2PathOverlapRegions);
				overlapRegions = MergeRectangle(overlapRegions);
				result = new PathOverlapPair();
				result.AGV1 = pair.AGV1;
				result.AGV2 = pair.AGV2;
				result.OverlapRegions = overlapRegions;
			}

			return result;
		}

		/// <summary>將指定車在 region 內的路徑點建成 KdTree</summary>
		private static KdTree<int, string> BuildTree(AGVInfo agv, Rectangle region)
		{
			KdTree<int, string> result = null;
			if (agv != null && agv.PathPoints.Count() > 0 && region != null)
			{
				result = new KdTree<int, string>(2, new IntMath());
				for (int i = 0; i < agv.PathPoints.Count(); ++i)
				{
					if (IsPointInRectangle(agv.PathPoints[i], region))
					{
						result.Add(new int[] { agv.PathPoints[i].X, agv.PathPoints[i].Y }, "");
					}
				}
			}
			return result;
		}

		/// <summary>指定點是否在指定矩形內</summary>
		private static bool IsPointInRectangle(Pair pair, Rectangle rectangle)
		{
			return (pair.X >= rectangle.XMin && pair.X <= rectangle.XMax && pair.Y >= rectangle.YMin && pair.Y <= rectangle.YMax);
		}

		/// <summary>計算指定車的每一個路徑點與指定 KdTree 的重疊範圍</summary>
		private static List<Rectangle> CalculatePathOverlapRegions(AGVInfo agv, Rectangle region, KdTree<int, string> tree)
		{
			List<Rectangle> result = null;
			if (agv != null && tree != null)
			{
				for (int i = 0; i < agv.PathPoints.Count(); ++i)
				{
					if (!IsPointInRectangle(new Pair(agv.PathPoints[i].X, agv.PathPoints[i].Y), region)) continue;

					bool isOverlap = false;
					int x =agv.PathPoints[i].X;
					int y =agv.PathPoints[i].Y;
					int frameRadius = agv.FrameRadius;
					int frameRadiusSquare = frameRadius * frameRadius;
					var neighbours = tree.GetNearestNeighbours(new int[] { x, y }, NUMBER);
					for (int j = 0; j < neighbours.Count(); ++j)
					{
						if (CalculateDistanceSquareOfTwoPoint(x, y, neighbours[j].Point[0], neighbours[j].Point[1]) <= frameRadiusSquare)
						{
							isOverlap = true;
							break;
						}
					}

					if (isOverlap)
					{
						Rectangle overlapRegion = Rectangle.GenerateRectangle(x, y, frameRadius);
						if (result == null)
						{
							result = new List<Rectangle>();
							result.Add(overlapRegion);
						}
						else
						{
							if (Rectangle.IsOverlap(result.Last(), overlapRegion))
							{
								result[result.Count - 1] = Rectangle.GetUnionRectangle(result.Last(), overlapRegion);
							}
							else
							{
								result.Add(overlapRegion);
							}
						}
					}
				}
			}

			if (result != null)
			{
				result = MergeRectangle(result);
			}

			return result;
		}

		/// <summary>計算兩點距離的平方</summary>
		private static int CalculateDistanceSquareOfTwoPoint(int x1, int y1, int x2, int y2)
		{
			return (int)(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
		}

		/// <summary>合併矩形。若其中任兩個矩形有重疊，則將其合併(聯集)成一個矩形</summary>
		private static List<Rectangle> MergeRectangle(List<Rectangle> rectangles)
		{
			List<Rectangle> result = rectangles;
			if (result != null && result.Count() > 1)
			{
				for (int i = 0; i < result.Count(); ++i)
				{
					for (int j = i + 1; j < result.Count(); ++j)
					{
						if (Rectangle.IsOverlap(result[i], result[j]))
						{
							result.Add(Rectangle.GetUnionRectangle(result[i], result[j]));
							result.RemoveAt(j);
							result.RemoveAt(i);

							// 從頭再檢查一次
							i = 0 - 1;
							j = i + 1;
							break;
						}
					}
				}
			}
			return result;
		}
	}
}
