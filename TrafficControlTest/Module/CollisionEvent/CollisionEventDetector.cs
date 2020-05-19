using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.CollisionEvent
{
	public class CollisionEventDetector : SystemWithLoopTask, ICollisionEventDetector
	{
		/// <summary>偵測會車事件時使用的鄰近點數量</summary>
		public int mNeighborPointAmount { get; set; } = 10;
		/// <summary>當 Vehicle Location Score 高於此閾值時才會於偵測該車的會車事件</summary>
		public double mVehicleLocationScoreThreshold = 30.0f;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private ICollisionEventManager rCollisionEventManager = null;

		public CollisionEventDetector(IVehicleInfoManager VehicleInfoManager, ICollisionEventManager CollisionEventManager)
		{
			Set(VehicleInfoManager, CollisionEventManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(ICollisionEventManager CollisionEventManager)
		{
			UnsubscribeEvent_ICollisionEventManager(rCollisionEventManager);
			rCollisionEventManager = CollisionEventManager;
			SubscribeEvent_ICollisionEventManager(rCollisionEventManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, ICollisionEventManager CollisionEventManager)
		{
			Set(VehicleInfoManager);
			Set(CollisionEventManager);
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "NeighborPointAmount":
					return mNeighborPointAmount.ToString();
				case "VehicleLocationScoreThreshold":
					return mVehicleLocationScoreThreshold.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					mTimePeriod = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "NeighborPointAmount":
					mNeighborPointAmount = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				case "VehicleLocationScoreThreshold":
					mVehicleLocationScoreThreshold = double.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override void Task()
		{
			Subtask_DetectCollisionEvent();
		}

		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{

			}
		}
		private void SubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{

			}
		}
		private void UnsubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{

			}
		}
		private void Subtask_DetectCollisionEvent()
		{
			if (rVehicleInfoManager != null && rVehicleInfoManager.GetItemNames() != null && rVehicleInfoManager.GetItemNames().Count() > 0)
			{
				if (IsAnyCollisionPair(rVehicleInfoManager.GetItems().Where(o => o.mLocationScore >= mVehicleLocationScoreThreshold), out IEnumerable<ICollisionPair> collisionPairs))
				{
					if (rCollisionEventManager != null)
					{
						foreach (ICollisionPair collisionPair in collisionPairs)
						{
							if (rCollisionEventManager.IsExist(collisionPair.mName))
							{
								rCollisionEventManager.Update(collisionPair.mName, collisionPair.mCollisionRegion, collisionPair.mPeriod, collisionPair.mPassPeriodOfVehicle1WithCurrentVelocity, collisionPair.mPassPeriodOfVehicle2WithCurrentVelocity, collisionPair.mPassPeriodOfVehicle1WithMaximumVeloctiy, collisionPair.mPassPeriodOfVehicle2WithMaximumVeloctiy);
							}
							else
							{
								rCollisionEventManager.Add(collisionPair.mName, collisionPair);
							}
						}
					}
				}

				// 若舊有的 Collision Pair 沒有新的、對應的 Collision Pair ，代表該 Collision Pair 被解除了
				List<string> collisionNameList = rCollisionEventManager.GetItemNames().ToList();
				if (collisionNameList != null && collisionNameList.Count > 0)
				{
					List<string> solvedCollisionPairs = rCollisionEventManager.GetItems().Where((o) => !IsCorrespondenceExist(o, collisionPairs)).Select((o) => o.mName).ToList();
					if (solvedCollisionPairs != null && solvedCollisionPairs.Count > 0)
					{
						for (int i = 0; i < solvedCollisionPairs.Count; ++i)
						{
							rCollisionEventManager.Remove(solvedCollisionPairs[i]);
						}
					}
				}
			}
		}
		private bool IsCorrespondenceExist(ICollisionPair SrcCollisionPair, IEnumerable<ICollisionPair> CollisionPairCollection)
		{
			bool result = false;
			if (CollisionPairCollection == null) return result;
			for (int i = 0; i < CollisionPairCollection.Count(); ++i)
			{
				if (CollisionPairCollection.ElementAt(i).mName == SrcCollisionPair.mName)
				{
					result = true;
					break;
				}
			}
			return result;
		}
		private bool IsAnyCollisionPair(IEnumerable<IVehicleInfo> Vehicles, out IEnumerable<ICollisionPair> CollisionPairs)
		{
			if (IsAnyPathRegionOverlapPair(Vehicles, out IEnumerable<IPathRegionOverlapPair> tmpPathRegionOverlapPairs))
			{
				if (IsAnyPathOverlapPair(tmpPathRegionOverlapPairs, out IEnumerable<IPathOverlapPair> tmpPathOverlapPairs))
				{
					if (IsAnyCollisionPair(tmpPathOverlapPairs, out IEnumerable<ICollisionPair> tmpCollisionPairs))
					{
						CollisionPairs = tmpCollisionPairs;
						return true;
					}
				}
			}

			CollisionPairs = null;
			return false;
		}
		private bool IsAnyPathRegionOverlapPair(IEnumerable<IVehicleInfo> Vehicles, out IEnumerable<IPathRegionOverlapPair> PathRegionOverlapPairs)
		{
			List<IPathRegionOverlapPair> tmpPathRegionOverlapPairs = new List<IPathRegionOverlapPair>();

			if (Vehicles != null && Vehicles.Count() > 1)
			{
				for (int i = 0; i < Vehicles.Count(); ++i)
				{
					for (int j = i + 1; j < Vehicles.Count(); ++j)
					{
						if (IsPathRegionOverlapPair(Vehicles.ElementAt(i), Vehicles.ElementAt(j), out IPathRegionOverlapPair PathRegionOverlapPair))
						{
							tmpPathRegionOverlapPairs.Add(PathRegionOverlapPair);
						}
					}
				}
			}

			if (tmpPathRegionOverlapPairs.Count > 0)
			{
				PathRegionOverlapPairs = tmpPathRegionOverlapPairs;
				return true;
			}
			else
			{
				PathRegionOverlapPairs = null;
				return false;
			}
		}
		private bool IsPathRegionOverlapPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, out IPathRegionOverlapPair PathRegionOverlapPair)
		{
			/*
			 * 輸入：兩車資訊
			 * 輸出：「路徑線區域」重疊的區域與對應的兩車，亦為「有機會」發生「路徑線」重疊的區域與對應的兩車
			 * 說明：
			 * 計算車的「路徑線區域」，其為一矩形，
			 * 接著計算兩車的「路徑線區域」是否有重疊，
			 * 若有重疊，代表兩車「有機會」發生路徑線重疊，
			 * 反之，代表兩車「不會」發生路徑線重疊。
			 */
			if (IsRectangleOverlap(Vehicle1.mPathRegion, Vehicle2.mPathRegion))
			{
				PathRegionOverlapPair = GenerateIPathRegionOverlapPair(Vehicle1, Vehicle2, GetIntersectionRectangle(Vehicle1.mPathRegion, Vehicle2.mPathRegion));
				return true;
			}
			else
			{
				PathRegionOverlapPair = null;
				return false;
			}
		}
		private bool IsAnyPathOverlapPair(IEnumerable<IPathRegionOverlapPair> PathRegionOverlapPairs, out IEnumerable<IPathOverlapPair> PathOverlapPairs)
		{
			List<IPathOverlapPair> tmpPathOverlapPairs = new List<IPathOverlapPair>();

			if (PathRegionOverlapPairs != null && PathRegionOverlapPairs.Count() > 0)
			{
				for (int i = 0; i < PathRegionOverlapPairs.Count(); ++i)
				{
					if (IsPathOverlapPair(PathRegionOverlapPairs.ElementAt(i), out IPathOverlapPair tmpPathOverlapPair))
					{
						tmpPathOverlapPairs.Add(tmpPathOverlapPair);
					}
				}
			}

			if (tmpPathOverlapPairs.Count > 0)
			{
				PathOverlapPairs = tmpPathOverlapPairs;
				return true;
			}
			else
			{
				PathOverlapPairs = null;
				return false;
			}
		}
		private bool IsPathOverlapPair(IPathRegionOverlapPair PathRegionOverlapPair, out IPathOverlapPair PathOverlapPair)
		{
			/*
			 * 輸入：「路徑線區域」重疊的區域與對應的兩車，亦為「有機會」發生「路徑線」重疊的區域與對應的兩車
			 * 輸出：「路徑線」重疊的區域與對應的兩車
			 * 說明：
			 * 為求效率，僅使用在『「路徑線區域」重疊的區域』內的「路徑線點」來計算。
			 * 先將兩車在『「路徑線區域」重疊的區域』內的「路徑線點」擷取出來，
			 *     1. 接著兩車各自計算自身的每一個「路徑線點」的方圓 n 釐米內是否有對方的「路徑線點」，
			 *     2. 接著兩車各自計算自身的每一個「路徑線點」的方圓 n 釐米內是否有對方的路徑線通過，
			 * 若有，代表此點會發生路徑線交會，不斷使用此方法來計算所有「路徑線點」與對方路徑線重疊的區域，
			 * 然後再將所有的重疊的區域合併，即可得到「路徑線」重疊的區域。
			 */
			PathOverlapPair = null;

			IRectangle2D pathRegionOverlapRegion = PathRegionOverlapPair.mOverlapRegionOfPathRegions;
			IEnumerable<IPoint2D> filteredPathPointsOfVehicle1 = PathRegionOverlapPair.mVehicle1.mPathDetail.Where((o) => IsPointInside(o, pathRegionOverlapRegion));
			IEnumerable<IPoint2D> filteredPathPointsOfVehicle2 = PathRegionOverlapPair.mVehicle2.mPathDetail.Where((o) => IsPointInside(o, pathRegionOverlapRegion));
			int frameRadiusOfVehicle1 = PathRegionOverlapPair.mVehicle1.mTotalFrameRadius;
			int frameRadiusOfVehicle2 = PathRegionOverlapPair.mVehicle2.mTotalFrameRadius;

			IEnumerable<IRectangle2D> overlapRegionsOfVehicle2OnVehiclePath1 = null;
			IEnumerable<IRectangle2D> overlapRegionsOfVehicle1OnVehiclePath2 = null;
			CalculatePathOverlapRegions(filteredPathPointsOfVehicle1, filteredPathPointsOfVehicle2, frameRadiusOfVehicle1, mNeighborPointAmount, out overlapRegionsOfVehicle2OnVehiclePath1);
			CalculatePathOverlapRegions(filteredPathPointsOfVehicle2, filteredPathPointsOfVehicle1, frameRadiusOfVehicle2, mNeighborPointAmount, out overlapRegionsOfVehicle1OnVehiclePath2);

			if ((overlapRegionsOfVehicle1OnVehiclePath2 != null && overlapRegionsOfVehicle1OnVehiclePath2.Count() > 0) || (overlapRegionsOfVehicle2OnVehiclePath1 != null && overlapRegionsOfVehicle2OnVehiclePath1.Count() > 0))
			{
				List<IRectangle2D> overlapRegions = new List<IRectangle2D>();
				if (overlapRegionsOfVehicle1OnVehiclePath2 != null && overlapRegionsOfVehicle1OnVehiclePath2.Count() > 0) overlapRegions.AddRange(overlapRegionsOfVehicle1OnVehiclePath2);
				if (overlapRegionsOfVehicle2OnVehiclePath1 != null && overlapRegionsOfVehicle2OnVehiclePath1.Count() > 0) overlapRegions.AddRange(overlapRegionsOfVehicle2OnVehiclePath1);
				overlapRegions = MergeRectangle(overlapRegions).ToList();
				PathOverlapPair = GenerateIPathOverlapPair(PathRegionOverlapPair.mVehicle1, PathRegionOverlapPair.mVehicle2, overlapRegions);
			}

			return PathOverlapPair != null ? true : false;
		}
		private bool CalculatePathOverlapRegions(IEnumerable<IPoint2D> PointsA, IEnumerable<IPoint2D> PointsB, int FrameRadius, int NeighbourAmount, out IEnumerable<IRectangle2D> OverlapRegions)
		{
			/*
			 * 以 PointA 為基準，計算 PointsA 與 PointsB 的重疊區域
			 * 計算 PointsA 的每一個點的方圓 FrameRadius 釐米內是否有 PointsB 的點。若有的話，該點周遭 FrameRadius 的矩形即為重疊區域
			 */
			OverlapRegions = null;
			List<IRectangle2D> tmpOverlapRegions = null;
			if (PointsA != null && PointsA.Count() > 0 && PointsB != null && PointsB.Count() > 0)
			{
				if (TryToBuildTreeOfPoints(PointsB, out KdTree.KdTree<int, string> treeOfPointsB))
				{
					int frameRadiusSquare = FrameRadius * FrameRadius;
					for (int i = 0; i < PointsA.Count(); ++i)
					{
						var neighbourPoints = treeOfPointsB.GetNearestNeighbours(new int[] { PointsA.ElementAt(i).mX, PointsA.ElementAt(i).mY }, NeighbourAmount);
						if (neighbourPoints.Any((o) => GetDistanceSquare(PointsA.ElementAt(i), GenerateIPoint2D(o.Point[0], o.Point[1])) < frameRadiusSquare))
						{
							IRectangle2D overlapRegion = GetRectangle(PointsA.ElementAt(i).mX, PointsA.ElementAt(i).mY, FrameRadius);
							if (tmpOverlapRegions == null)
							{
								tmpOverlapRegions = new List<IRectangle2D>();
								tmpOverlapRegions.Add(overlapRegion);
							}
							else
							{
								if (IsRectangleOverlap(tmpOverlapRegions.Last(), overlapRegion))
								{
									tmpOverlapRegions[tmpOverlapRegions.Count - 1] = GetCoverRectangle(tmpOverlapRegions.Last(), overlapRegion);
								}
								else
								{
									tmpOverlapRegions.Add(overlapRegion);
								}
							}
						}
					}

					if (tmpOverlapRegions != null && tmpOverlapRegions.Count > 0)
					{
						OverlapRegions = MergeRectangle(tmpOverlapRegions).ToList();
					}
				}
			}
			return (OverlapRegions != null && OverlapRegions.Count() > 0) ? true : false;
		}
		private bool TryToBuildTreeOfPoints(IEnumerable<IPoint2D> Points, out KdTree.KdTree<int, string> Tree)
		{
			if (Points.Count() > 0)
			{
				Tree = new KdTree.KdTree<int, string>(2, new IntMath());
				for (int i = 0; i < Points.Count(); ++i)
				{
					Tree.Add(new int[] { Points.ElementAt(i).mX, Points.ElementAt(i).mY }, i.ToString());
				}
				return true;
			}
			else
			{
				Tree = null;
				return false;
			}
		}
		private bool IsAnyCollisionPair(IEnumerable<IPathOverlapPair> PathOverlapPairs, out IEnumerable<ICollisionPair> CollisionPairs)
		{
			List<ICollisionPair> tmpCollisionPairs = new List<ICollisionPair>();

			if (PathOverlapPairs != null && PathOverlapPairs.Count() > 0)
			{
				for (int i = 0; i < PathOverlapPairs.Count(); ++i)
				{
					if (IsCollisionPair(PathOverlapPairs.ElementAt(i), out ICollisionPair tmpCollisionPair))
					{
						tmpCollisionPairs.Add(tmpCollisionPair);
					}
				}
			}

			if (tmpCollisionPairs.Count > 0)
			{
				CollisionPairs = tmpCollisionPairs;
				return true;
			}
			else
			{
				CollisionPairs = null;
				return false;
			}
		}
		private bool IsCollisionPair(IPathOverlapPair PathOverlapPair, out ICollisionPair CollisionPair)
		{
			/*
			 * 輸入：「路徑線」重疊的區域與對應的兩車
			 * 輸出：發生交會事件的區域與時間與對應的兩車
			 * 說明：
			 * 分別計算兩車進入/離開每一個『「路徑線」重疊的區域』的時間區間，
			 * 並計算時間區間是否有重疊，若有的話代表該區域將會發生交會，
			 * 將所有的「路徑線」重疊的區域都計算過一輪後，取出最早發生的時間區間與「路徑線」重疊的區域，其即是接下來將發生交會事件的資訊。
			 */
			CollisionPair = null;
			if (PathOverlapPair != null && PathOverlapPair.mOverlapRegionsOfPaths != null && PathOverlapPair.mOverlapRegionsOfPaths.Count() > 0)
			{
				List<ICollisionPair> tmpCollisionPairs = new List<ICollisionPair>();
				for (int i = 0; i < PathOverlapPair.mOverlapRegionsOfPaths.Count(); ++i)
				{
					CalculatePassInfo(PathOverlapPair.mVehicle1, PathOverlapPair.mOverlapRegionsOfPaths.ElementAt(i), out ITowardPoint2D EnterPoint1, out ITowardPoint2D ExitPoint1, out double EnterDistance1, out double ExitDistance1, out ITimePeriod PassPeriod1, out ITimePeriod PassPeriodWithMaximumVelocity1);
					CalculatePassInfo(PathOverlapPair.mVehicle2, PathOverlapPair.mOverlapRegionsOfPaths.ElementAt(i), out ITowardPoint2D EnterPoint2, out ITowardPoint2D ExitPoint2, out double EnterDistance2, out double ExitDistance2, out ITimePeriod PassPeriod2, out ITimePeriod PassPeriodWithMaximumVelocity2);
					// 1. (使用平均速度)若兩車進入與離開『「路徑線」重疊的區域』的「時間區間」有重疊時，代表兩車將會發生交會事件
					// 2. (使用最大速度)
					if (IsOverlap(PassPeriod1, PassPeriod2) || IsOverlap(PassPeriodWithMaximumVelocity1, PassPeriodWithMaximumVelocity2))
					{
						tmpCollisionPairs.Add(GenerateICollisionPair(PathOverlapPair.mVehicle1, PathOverlapPair.mVehicle2, PathOverlapPair.mOverlapRegionsOfPaths.ElementAt(i), PassPeriod1.mStart < PassPeriod2.mStart ? PassPeriod1 : PassPeriod2, PassPeriod1, PassPeriod2, PassPeriodWithMaximumVelocity1, PassPeriodWithMaximumVelocity2));
					}
				}

				if (tmpCollisionPairs.Count() > 0)
				{
					if (tmpCollisionPairs.Count() == 1)
					{
						CollisionPair = tmpCollisionPairs.First();
					}
					else
					{
						int indexOfEarliest = 0;
						DateTime timeOfEarliest = tmpCollisionPairs.ElementAt(0).mPeriod.mStart;
						for (int i = 1; i < tmpCollisionPairs.Count(); ++i)
						{
							if (tmpCollisionPairs.ElementAt(i).mPeriod.mStart < timeOfEarliest)
							{
								indexOfEarliest = i;
								timeOfEarliest = tmpCollisionPairs.ElementAt(i).mPeriod.mStart;
							}
						}
						CollisionPair = tmpCollisionPairs.ElementAt(indexOfEarliest);
					}
				}
			}
			return CollisionPair != null ? true : false;
		}
		private void CalculatePassInfo(IVehicleInfo Vehicle, IRectangle2D Region, out ITowardPoint2D EnterPoint, out ITowardPoint2D ExitPoint, out double EnterDistance, out double ExitDistance, out ITimePeriod PassPeriod, out ITimePeriod PassPeriodWithMaximumVelocity)
		{
			/*
			 * 計算車子通過指令區域的相關資訊
			 */
			EnterPoint = null;
			ExitPoint = null;
			EnterDistance = 0;
			ExitDistance = 0;
			PassPeriod = null;
			PassPeriodWithMaximumVelocity = null;

			if (Vehicle.mPath.Count() > 0)
			{
				List<IPoint2D> tmpPath = Vehicle.mPath.ToList();
				tmpPath.Insert(0, Vehicle.mLocationCoordinate);
				for (int i = 1; i < tmpPath.Count(); ++i)
				{
					IEnumerable<IPoint2D> intersectionPoints = GetIntersectionPoint(Region, tmpPath[i - 1], tmpPath[i]);
					switch (intersectionPoints.Count())
					{
						case 1:
							// 第一個點在矩形外，第二個點在矩形內
							if (!IsPointInside(tmpPath[i - 1], Region) && IsPointInside(tmpPath[i], Region))
							{
								if (EnterPoint == null)
								{
									EnterPoint = GenerateITowardPoint2D(intersectionPoints.First(), GetAngle(tmpPath[i - 1], tmpPath[i]));
									EnterDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.First());
								}
							}
							// 第一個點在矩形內，第二個點在矩形外
							else
							{
								if (ExitPoint == null)
								{
									ExitPoint = GenerateITowardPoint2D(intersectionPoints.First(), GetAngle(tmpPath[i - 1], tmpPath[i]));
									ExitDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.First());
								}
							}
							break;
						case 2:
							double distance1 = GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(0)); // 距離較小的點為進入點，距離較大的點為離開點
							double distance2 = GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(1));
							if (distance1 < distance2)
							{
								if (EnterPoint == null)
								{
									EnterPoint = GenerateITowardPoint2D(intersectionPoints.ElementAt(0), GetAngle(tmpPath[i - 1], tmpPath[i]));
									EnterDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(0));
								}
								if (ExitPoint == null)
								{
									ExitPoint = GenerateITowardPoint2D(intersectionPoints.ElementAt(1), GetAngle(tmpPath[i - 1], tmpPath[i]));
									ExitDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(1));
								}
							}
							else
							{
								if (EnterPoint == null)
								{
									EnterPoint = GenerateITowardPoint2D(intersectionPoints.ElementAt(1), GetAngle(tmpPath[i - 1], tmpPath[i]));
									EnterDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(1));
								}
								if (ExitPoint == null)
								{
									ExitPoint = GenerateITowardPoint2D(intersectionPoints.ElementAt(0), GetAngle(tmpPath[i - 1], tmpPath[i]));
									ExitDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(0));
								}
							}
							break;
						default:
							break;
					}
				}

				double velocity = !double.IsNaN(Vehicle.mAverageVelocity) ? Vehicle.mAverageVelocity : Vehicle.mVelocity;
				// 當速度為 0 時，改使用最高速度下去計算
				velocity = (velocity > -double.Epsilon && velocity < double.Epsilon) ? Vehicle.mVelocityMaximum : velocity;
				double enterSeconds = (velocity > -double.Epsilon && velocity < double.Epsilon ? 600 : (EnterDistance / velocity));
				double exitSeconds = (velocity > -double.Epsilon && velocity < double.Epsilon ? 600 : (ExitDistance / velocity));
				PassPeriod = GenerateITimePeriod(DateTime.Now.AddSeconds(enterSeconds), DateTime.Now.AddSeconds(exitSeconds));
				PassPeriodWithMaximumVelocity = GenerateITimePeriod(DateTime.Now.AddSeconds(EnterDistance / Vehicle.mVelocityMaximum), DateTime.Now.AddSeconds(ExitDistance / Vehicle.mVelocityMaximum));
			}
			else if (Vehicle.mPath.Count == 0 && Region.IsIncludePoint(Vehicle.mLocationCoordinate))
			{
				// 若車子路徑點數量為 0 (閒置/沒有在移動)，且車子座標位於交會區域 (Region) 內
				PassPeriod = GenerateITimePeriod(DateTime.Now, DateTime.MaxValue);
				PassPeriodWithMaximumVelocity = GenerateITimePeriod(DateTime.Now, DateTime.MaxValue);
			}
		}
		private bool IsOverlap(ITimePeriod Period1, ITimePeriod Period2)
		{
			bool result = false;
			if (Period1 != null && Period2 != null)
			{
				result =
				result = (Period1.mStart >= Period2.mStart && Period1.mStart <= Period2.mEnd)
					|| (Period1.mEnd >= Period2.mStart && Period1.mEnd <= Period2.mEnd)
					|| (Period2.mStart >= Period1.mStart && Period2.mStart <= Period1.mEnd)
					|| (Period2.mEnd >= Period1.mStart && Period2.mEnd <= Period1.mEnd);
			}
			return result;
		}
	}
}
