using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>發生交會的組合</summary>
	public class CollisionPair
	{
		public AGVInfo AGV1 = null;
		public AGVInfo AGV2 = null;
		public Rectangle CollisionRegion = null;
		public int CollisionBeginTime { get { return CollisionBeginTimeOfAGV1 < CollisionBeginTimeOfAGV2 ? CollisionBeginTimeOfAGV1 : CollisionBeginTimeOfAGV2; } }
		public int CollisionEndTime { get { return CollisionEndTimeOfAGV1 > CollisionBeginTimeOfAGV2 ? CollisionEndTimeOfAGV1 : CollisionEndTimeOfAGV2; } }
		private int CollisionBeginTimeOfAGV1;
		private int CollisionEndTimeOfAGV1;
		private int CollisionBeginTimeOfAGV2;
		private int CollisionEndTimeOfAGV2;
		public double EnterAngleOfAGV1;
		public double ExitAngleOfAGV1;
		public double EnterAngleOfAGV2;
		public double ExitAngleOfAGV2;
		private CollisionPair() { }
		public override string ToString()
		{
			string tmp = "";
			tmp += $"{AGV1.Status.Name} & {AGV2.Status.Name} will be collided.\n";
			tmp += $"The collision region is {CollisionRegion.ToString()}\n";
			tmp += $"The collision will begin in {CollisionBeginTime} seconds.\n";
			return tmp;
		}

		/// <summary>計算是否會發生交會</summary>
		public static CollisionPair IsCollision(PathOverlapPair pair)
		{
			CollisionPair result = null;
			if (pair != null)
			{
				if (pair.AGV1 != null && pair.AGV2 != null && pair.OverlapRegions != null && pair.OverlapRegions.Count() > 0)
				{
					List<CollisionPair> collisionPairs = null;
					int timeBegin = -1, timeEnd = -1;
					for (int i = 0; i < pair.OverlapRegions.Count(); ++i)
					{
						// 計算兩車於指定區域的交會時間
						CollisionPair tmp = null;
						tmp = CalculateTimeIntervalOfIntersection(pair.AGV1, pair.AGV2, pair.OverlapRegions[i], ref timeBegin, ref timeEnd);
						if (timeBegin > 0 && timeEnd > 0)
						{
							if (collisionPairs == null) collisionPairs = new List<CollisionPair>();
							collisionPairs.Add(tmp);
						}
					}

					// 找出最早發生的交會事件
					if (collisionPairs != null)
					{
						if (collisionPairs.Count == 1)
						{
							result = collisionPairs.Last();
						}
						else if (collisionPairs.Count > 1)
						{
							int indexOfEarlier = 0;
							int timeOfEarlier = collisionPairs[0].CollisionBeginTime;
							for (int i = 1; i < collisionPairs.Count(); ++i)
							{
								if (collisionPairs[i].CollisionBeginTime < timeOfEarlier)
								{
									indexOfEarlier = i;
									timeOfEarlier = collisionPairs[i].CollisionBeginTime;
								}
							}
							result = collisionPairs[indexOfEarlier];
						}
					}
				}
			}
			return result;
		}

		/// <summary>計算兩車於指定區域的交會時間</summary>
		private static CollisionPair CalculateTimeIntervalOfIntersection(AGVInfo agv1, AGVInfo agv2, Rectangle intersectionRegion, ref int timeBegin, ref int timeEnd)
		{
			CollisionPair result = null;
			timeBegin = -1;
			timeEnd = -1;
			if (agv1 != null && agv2 != null && intersectionRegion != null)
			{
				int timeOfEnterOfAGV1 = -1, timeOfExitOfAGV1 = -1;
				int timeOfEnterOfAGV2 = -1, timeOfExitOfAGV2 = -1;
				CalculateTimeIntervalOfRegion(agv1, intersectionRegion, ref timeOfEnterOfAGV1, ref timeOfExitOfAGV1);
				CalculateTimeIntervalOfRegion(agv2, intersectionRegion, ref timeOfEnterOfAGV2, ref timeOfExitOfAGV2);
				if (timeOfEnterOfAGV1 >= 0 && timeOfExitOfAGV1 > timeOfEnterOfAGV1 && timeOfEnterOfAGV2 >= 0 && timeOfExitOfAGV2 > timeOfEnterOfAGV2)
				{
					// 判斷兩車的時間是否有重疊
					if ((timeOfEnterOfAGV1 >= timeOfEnterOfAGV2 && timeOfEnterOfAGV1 <= timeOfExitOfAGV2)
						|| (timeOfExitOfAGV1 >= timeOfEnterOfAGV2 && timeOfExitOfAGV1 <= timeOfExitOfAGV2)
						|| (timeOfEnterOfAGV2 >= timeOfEnterOfAGV1 && timeOfEnterOfAGV2 <= timeOfExitOfAGV1)
						|| (timeOfExitOfAGV2 >= timeOfEnterOfAGV1 && timeOfExitOfAGV2 <= timeOfExitOfAGV1))
					{
						timeBegin = timeOfEnterOfAGV1 < timeOfEnterOfAGV2 ? timeOfEnterOfAGV1 : timeOfEnterOfAGV2;
						timeEnd = timeOfExitOfAGV1 > timeOfExitOfAGV2 ? timeOfExitOfAGV1 : timeOfExitOfAGV2;
						result = new CollisionPair();
						result.AGV1 = agv1;
						result.AGV2 = agv2;
						result.CollisionRegion = intersectionRegion;
						result.CollisionBeginTimeOfAGV1 = timeOfEnterOfAGV1;
						result.CollisionEndTimeOfAGV1 = timeOfExitOfAGV1;
						result.CollisionBeginTimeOfAGV2 = timeOfEnterOfAGV2;
						result.CollisionEndTimeOfAGV2 = timeOfExitOfAGV2;
					}
				}
			}
			return result;
		}

		/// <summary>計算車子於指定區域的進入時間與離開時間</summary>
		private static void CalculateTimeIntervalOfRegion(AGVInfo agv, Rectangle region, ref int timeOfEnter, ref int timeOfExit)
		{
			timeOfEnter = -1;
			timeOfExit = -1;
			if (agv != null && region != null && agv.Status != null && agv.Status.Velocity > 0)
			{
				int indexOfEnter = -1, indexOfExit = -1;
				CalculateIndexIntervalOfRegion(agv, region, ref indexOfEnter, ref indexOfExit);
				if (indexOfEnter >= 0 && indexOfExit > indexOfEnter)
				{
					int distanceOfEnter = -1, distanceOfExit = -1;
					distanceOfEnter = CalculateDistanceOfTwoPointInPath(agv, 0, indexOfEnter);
					distanceOfExit = distanceOfEnter + CalculateDistanceOfTwoPointInPath(agv, indexOfEnter, indexOfExit);
					if (distanceOfEnter >= 0 && distanceOfExit > distanceOfEnter)
					{
						timeOfEnter = (int)(distanceOfEnter / agv.Status.Velocity);
						timeOfExit = (int)(distanceOfExit / agv.Status.Velocity);
					}
				}
			}
		}

		/// <summary>計算車子於指定區域的進入點索引與離開點索引</summary>
		private static void CalculateIndexIntervalOfRegion(AGVInfo agv, Rectangle region, ref int indexOfEnter, ref int indexOfExit)
		{
			indexOfEnter = -1;
			indexOfExit = -1;
			if (agv != null && region != null && agv.PathPoints != null)
			{
				bool isInRegion = false;
				for (int i = 0; i < agv.PathPoints.Count(); ++i)
				{
					// 計算進入點索引
					if (!isInRegion)
					{
						if (region.IsPointInside(agv.PathPoints[i].X, agv.PathPoints[i].Y))
						{
							isInRegion = true;
							indexOfEnter = ((i - 1) >= 0) ? (i - 1) : i;
						}
					}
					// 計算離開點索引
					else
					{
						if (!region.IsPointInside(agv.PathPoints[i].X, agv.PathPoints[i].Y))
						{
							isInRegion = false;
							indexOfExit = i;
							break;
						}
						// 如果到路徑線點尾端都還沒離開，則將尾端設定為離開點的索引
						else if (i == agv.PathPoints.Count - 1)
						{
							isInRegion = false;
							indexOfExit = i;
						}
					}
				}
			}
		}

		/// <summary>計算車子路徑中，兩點間的距離</summary>
		private static int CalculateDistanceOfTwoPointInPath(AGVInfo agv, int indexOfBegin, int indexOfEnd)
		{
			double result = 0.0f;
			if (agv != null && agv.PathPoints != null && indexOfBegin >= 0 && indexOfEnd < agv.PathPoints.Count && indexOfBegin < indexOfEnd)
			{
				for (int i = indexOfBegin + 1; i <= indexOfEnd; ++i)
				{
					result += Math.Sqrt(Math.Pow(agv.PathPoints[i - 1].X - agv.PathPoints[i].X, 2) + Math.Pow(agv.PathPoints[i - 1].Y - agv.PathPoints[i].Y, 2));
				}
			}
			return (int)result;
		}
	}
}
