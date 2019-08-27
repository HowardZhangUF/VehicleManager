using Geometry;
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
		public double CollisionBeginTime { get { return CollisionBeginTimeOfAGV1 < CollisionBeginTimeOfAGV2 ? CollisionBeginTimeOfAGV1 : CollisionBeginTimeOfAGV2; } }
		public double CollisionEndTime { get { return CollisionEndTimeOfAGV1 > CollisionBeginTimeOfAGV2 ? CollisionEndTimeOfAGV1 : CollisionEndTimeOfAGV2; } }
		private double CollisionBeginTimeOfAGV1;
		private double CollisionEndTimeOfAGV1;
		private double CollisionBeginTimeOfAGV2;
		private double CollisionEndTimeOfAGV2;
		public Vector2D EnterVectorOfAGV1;
		public Vector2D ExitVectorOfAGV1;
		public Vector2D EnterVectorOfAGV2;
		public Vector2D ExitVectorOfAGV2;
		private CollisionPair() { }
		public override string ToString()
		{
			string tmp = "";
			tmp += $"{AGV1.Status.Name} & {AGV2.Status.Name} will be collided.\n";
			tmp += $"The collision region is {CollisionRegion.ToString()}\n";
			tmp += $"The collision will begin in {CollisionBeginTime.ToString("F2")} seconds.\n";
			tmp += $"The {AGV1.Status.Name} Enter Direction: {EnterVectorOfAGV1.ToString()}";
			tmp += $"The {AGV2.Status.Name} Enter Direction: {EnterVectorOfAGV2.ToString()}";
			tmp += $"Angle Between Two Direction: {Vector2D.CalculateAngleOfTwoVector(EnterVectorOfAGV1, EnterVectorOfAGV2)}";
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
					double timeBegin = -1, timeEnd = -1;
					for (int i = 0; i < pair.OverlapRegions.Count(); ++i)
					{
						// 計算兩車於指定區域的交會時間
						CollisionPair tmp = null;
						tmp = CalculateTimeIntervalOfIntersection(pair.AGV1, pair.AGV2, pair.OverlapRegions[i], ref timeBegin, ref timeEnd);
						if (timeBegin >= 0 && timeEnd >= 0)
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
							double timeOfEarlier = collisionPairs[0].CollisionBeginTime;
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
		private static CollisionPair CalculateTimeIntervalOfIntersection(AGVInfo agv1, AGVInfo agv2, Rectangle intersectionRegion, ref double timeBegin, ref double timeEnd)
		{
			CollisionPair result = null;
			timeBegin = -1;
			timeEnd = -1;
			if (agv1 != null && agv2 != null && intersectionRegion != null)
			{
				double timeOfEnterOfAGV1 = -1, timeOfExitOfAGV1 = -1;
				double timeOfEnterOfAGV2 = -1, timeOfExitOfAGV2 = -1;
				Vector2D vectorOfEnterOfAGV1 = null, vectorOfExitOfAGV1 = null;
				Vector2D vectorOfEnterOfAGV2 = null, vectorOfExitOfAGV2 = null;
				CalculateTimeIntervalOfRegion(agv1, intersectionRegion, ref timeOfEnterOfAGV1, ref timeOfExitOfAGV1, ref vectorOfEnterOfAGV1, ref vectorOfExitOfAGV1);
				CalculateTimeIntervalOfRegion(agv2, intersectionRegion, ref timeOfEnterOfAGV2, ref timeOfExitOfAGV2, ref vectorOfEnterOfAGV2, ref vectorOfExitOfAGV2);
				if (timeOfEnterOfAGV1 >= 0 && timeOfExitOfAGV1 > timeOfEnterOfAGV1 && timeOfEnterOfAGV2 >= 0 && timeOfExitOfAGV2 > timeOfEnterOfAGV2 && vectorOfEnterOfAGV1 != null && vectorOfExitOfAGV1 != null && vectorOfEnterOfAGV2 != null && vectorOfExitOfAGV2 != null)
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
						result.EnterVectorOfAGV1 = vectorOfEnterOfAGV1;
						result.ExitVectorOfAGV1 = vectorOfExitOfAGV1;
						result.EnterVectorOfAGV2 = vectorOfEnterOfAGV2;
						result.ExitVectorOfAGV2 = vectorOfExitOfAGV2;
					}
				}
			}
			return result;
		}

		/// <summary>計算車子於指定區域的進入時間與離開時間</summary>
		private static void CalculateTimeIntervalOfRegion(AGVInfo agv, Rectangle region, ref double timeOfEnter, ref double timeOfExit, ref Vector2D vectorOfEnter, ref Vector2D vectorOfExit)
		{
			timeOfEnter = -1;
			timeOfExit = -1;
			vectorOfEnter = null;
			vectorOfExit = null;
			if (agv != null && region != null && agv.Status != null && agv.Status.Velocity > 0)
			{
				int indexOfEnter = -1, indexOfExit = -1;
				CalculateIndexIntervalOfRegion(agv, region, ref indexOfEnter, ref indexOfExit, ref vectorOfEnter, ref vectorOfExit);
				if (indexOfEnter >= 0 && indexOfExit > indexOfEnter && vectorOfEnter != null && vectorOfExit != null)
				{
					int distanceOfEnter = -1, distanceOfExit = -1;
					distanceOfEnter = CalculateDistanceOfTwoPointInPath(agv, 0, indexOfEnter);
					distanceOfExit = distanceOfEnter + CalculateDistanceOfTwoPointInPath(agv, indexOfEnter, indexOfExit);

					// 因為 PathPoint 每次都會重新計算，所以每次進入點的距離與當前位置的距離都差不多，需要再加上進入點的位置與交會區域的距離
					// 因為 PathPoint 每次都會重新計算，所以每次算出的 Collision Region 都不一樣
					if (indexOfEnter >= 0 && (indexOfEnter + 1) < agv.PathPoints.Count())
					{
						Pair EnterPoint = CalculateIntersectionPoint(region, agv.PathPoints[indexOfEnter], agv.PathPoints[indexOfEnter + 1]);
						if (EnterPoint != null)
						{
							distanceOfEnter += (int)CalculateDistanceOfTwoPoint(EnterPoint.X, EnterPoint.Y, agv.PathPoints[indexOfEnter].X, agv.PathPoints[indexOfEnter].Y);
						}
					}

					if (distanceOfEnter >= 0 && distanceOfExit > distanceOfEnter)
					{
						timeOfEnter = distanceOfEnter / agv.Status.Velocity;
						timeOfExit = distanceOfExit / agv.Status.Velocity;
					}
				}
			}
		}

		/// <summary>計算車子於指定區域的進入點索引與離開點索引</summary>
		private static void CalculateIndexIntervalOfRegion(AGVInfo agv, Rectangle region, ref int indexOfEnter, ref int indexOfExit, ref Vector2D vectorOfEnter, ref Vector2D vectorOfExit)
		{
			indexOfEnter = -1;
			indexOfExit = -1;
			vectorOfEnter = null;
			vectorOfExit = null;
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
							vectorOfEnter = ((i - 1) >= 0) ? Vector2D.CalculateVector2D(agv.PathPoints[i - 1].X, agv.PathPoints[i - 1].Y, agv.PathPoints[i].X, agv.PathPoints[i].Y) : Vector2D.CalculateVector2D(0, 0, 0, 0);
						}
					}
					// 計算離開點索引
					else
					{
						if (!region.IsPointInside(agv.PathPoints[i].X, agv.PathPoints[i].Y))
						{
							isInRegion = false;
							indexOfExit = i;
							vectorOfExit = Vector2D.CalculateVector2D(agv.PathPoints[i - 1].X, agv.PathPoints[i - 1].Y, agv.PathPoints[i].X, agv.PathPoints[i].Y);
							break;
						}
						// 如果到路徑線點尾端都還沒離開，則將尾端設定為離開點的索引
						else if (i == agv.PathPoints.Count - 1)
						{
							isInRegion = false;
							indexOfExit = i;
							vectorOfExit = Vector2D.CalculateVector2D(0, 0, 0, 0);
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
					result += CalculateDistanceOfTwoPoint(agv.PathPoints[i - 1].X, agv.PathPoints[i - 1].Y, agv.PathPoints[i].X, agv.PathPoints[i].Y);
				}
			}
			return (int)result;
		}

		/// <summary>計算兩點間的距離</summary>
		private static double CalculateDistanceOfTwoPoint(int x1, int y1, int x2, int y2)
		{
			return Math.Sqrt(Math.Pow(x1 - x2, 2)) + Math.Sqrt(Math.Pow(y1 - y2, 2));
		}

		/// <summary>計算指定矩形與兩點組成的線段的交點</summary>
		private static Pair CalculateIntersectionPoint(Rectangle rectangle, Pair point1, Pair point2)
		{
			Pair result = null;
			if (rectangle != null && point1 != null && point2 != null)
			{
				// 計算矩形的四個邊與兩點組成的線段的交點
				if (result == null)
				{
					result = CalculateIntersectionPoint(point1, point2, new Pair(rectangle.XMax, rectangle.YMax), new Pair(rectangle.XMax, rectangle.YMin));
				}
				if (result == null)
				{
					result = CalculateIntersectionPoint(point1, point2, new Pair(rectangle.XMax, rectangle.YMax), new Pair(rectangle.XMin, rectangle.YMax));
				}
				if (result == null)
				{
					result = CalculateIntersectionPoint(point1, point2, new Pair(rectangle.XMin, rectangle.YMin), new Pair(rectangle.XMax, rectangle.YMin));
				}
				if (result == null)
				{
					result = CalculateIntersectionPoint(point1, point2, new Pair(rectangle.XMin, rectangle.YMin), new Pair(rectangle.XMin, rectangle.YMax));
				}
			}
			return result;
		}

		/// <summary>計算兩線段的交點</summary>
		private static Pair CalculateIntersectionPoint(Pair line1Point1, Pair line1Point2, Pair line2Point1, Pair line2Point2)
		{
			/*
			參考資料：https://www.cnblogs.com/sanmubai/p/7306599.html

			線段 AB 其端點為 (xa, ya) 與 (xb, yb) ，其直線方程為：
			x = xa + lambda * (xb - xa)
			y = ya + lambda * (yb - ya)
			0 <= lambda <= 1

			線段 CD 其端點為 (xc, yc) 與 (xd, yd) ，其直線方程為：
			x = xc + micro * (xd - xc)
			y = yc + micro * (yd - yc)
			0 <= micro <= 1

			則交點應滿足：
			x = xa + lambda * (xb - xa) = xc + micro * (xd - xc)
			y = ya + lambda * (yb - ya) = yc + micro * (yd - yc)

			可整理成：
			(xb - xa) * lambda - (xd - xc) * micro = xc - xa
			(yb - ya) * lambda - (yd - yc) * micro = yc - ya

			行列式 delta 的算法為：
			A = |(xb - xa) -(xd - xc)|
				|(yb - ya) -(yd - yc)|
			delta = (xb - xa) * (-(yd - yc)) - (-(xd - xc)) * (yb - ya)
				  = (xb - xa) * (yc - yd) - (xc - xd) * (yb - ya)

			若其行列式等於零，表示線段 AB 與線段 CD 重合或平行。

			若其行列式不等於零，則可求出：
			lambda = 1 / delta * det|(xc - xa) -(xd - xc)|
									|(yc - ya) -(yd - yc)|
				   = 1 / delta * ((xc - xa) * (yc - yd) - (xc - xd) * (yc - ya))
			mircro = 1 / delta * det|(xb - xa) (xc - xa)|
									|(yb - ya) (yc - ya)|
				   = 1 / delta * ((xb - xa) * (yc - ya) - (xc - xa) * (yb - ya))
			
			需特別注意，僅有當 0 <= lambda <= 1 且 0 <= micro <= 1 時，兩線段才有相交，
			否則，交點在線段的延長線上，仍認為兩線段不相交。

			算出 lambda 與 micro 後，可得交點為：
			x = xa + lambda * (xb - xa)
			y = ya + lambda * (yb - ya)
			*/
			Pair result = null;
			if (line1Point1 != null && line1Point2 != null && line2Point1 != null && line2Point2 != null)
			{
				double delta = (line1Point2.X - line1Point1.X) * (line2Point1.Y - line2Point2.Y) - (line2Point1.X - line2Point2.X) * (line1Point2.Y - line1Point1.Y);
				if (delta <= double.Epsilon && delta >= -double.Epsilon)
				{
					result = null;
				}
				double lambda = ((line2Point1.X - line1Point1.X) * (line2Point1.Y - line2Point2.Y) - (line2Point1.X - line2Point2.X) * (line2Point1.Y - line1Point1.Y)) / delta;
				if (0 <= lambda && lambda <= 1)
				{
					double micro = ((line1Point2.X - line1Point1.X) * (line2Point1.Y - line1Point1.Y) - (line2Point1.X - line1Point1.X) * (line1Point2.Y - line1Point1.Y)) / delta;
					if (0 <= micro && micro <= 1)
					{
						int x = line1Point1.X + (int)(lambda * (line1Point2.X - line1Point1.X));
						int y = line1Point1.Y + (int)(lambda * (line1Point2.Y - line1Point1.Y));
						result = new Pair(x, y);
					}
				}
			}
			return result;
		}
	}
}
