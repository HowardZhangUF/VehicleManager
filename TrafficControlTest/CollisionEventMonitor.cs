using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>監控當前的車子是否會發生碰撞事件。使用前需先設定要參考的 AGVInfoManager</summary>
	class CollisionEventMonitor
	{
		public delegate void SystemEventHandler(DateTime timeStamp);

		public event SystemEventHandler SystemStarted;

		public event SystemEventHandler SystemStopped;

		public delegate void CollisionPairEventHandler(CollisionPair collisionPair);

		public event CollisionPairEventHandler CollisionOccured;

		public event CollisionPairEventHandler CollisionUpdated;

		public event CollisionPairEventHandler CollisionSolved;

		private List<CollisionPair> CollisionPairs = null;

		private AGVInfoManager AGVInfoManager = null;

		public bool IsAlive { get { return !MainTaskExitFlag; } }

		private bool MainTaskExitFlag = true;

		private Thread MainThread;

		public void SetAGVInfoManager(AGVInfoManager agvInfoManager)
		{
			if (agvInfoManager != null)
			{
				AGVInfoManager = agvInfoManager;
				AGVInfoManager.AGVAdded += AGVInfoManager_AGVAdded;
				AGVInfoManager.AGVRemoved += AGVInfoManager_AGVRemoved;
			}
		}

		public void ClearAGVInfoManager()
		{
			if (AGVInfoManager != null)
			{
				AGVInfoManager.AGVAdded -= AGVInfoManager_AGVAdded;
				AGVInfoManager.AGVRemoved -= AGVInfoManager_AGVRemoved;
				AGVInfoManager = null;
			}
		}

		private void AGVInfoManager_AGVAdded(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (AGVInfoManager.GetAGVNames().Count() > 1)
			{
				if (MainTaskExitFlag)
				{
					Start();
				}
			}
		}

		private void AGVInfoManager_AGVRemoved(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (AGVInfoManager.GetAGVNames().Count() == 0)
			{
				if (!MainTaskExitFlag)
				{
					Stop();
				}
			}
		}

		private void Start()
		{
			if (MainThread == null)
			{
				MainTaskExitFlag = false;
				MainThread = new Thread(MainTask);
				MainThread.IsBackground = true;
				MainThread.Start();
			}
			else
			{
				if (MainThread.IsAlive)
				{
					MainTaskExitFlag = false;
				}
			}
		}

		private void Stop()
		{
			MainThread = null;
			MainTaskExitFlag = true;
		}

		private void MainTask()
		{
			try
			{
				SystemStarted?.Invoke(DateTime.Now);
				while (!MainTaskExitFlag)
				{
					if (AGVInfoManager != null)
					{
						// Calculate Collision Pairs
						List<CollisionPair> newCollisionPairs = CalculateCollisionEvent(AGVInfoManager.GetAGVInfos().Where((a) => a.Status.Description == SerialData.EDescription.Running).ToList());
						
						// Compare Old Pairs with New Pairs
						// If Collision Pair is Already Exist, Raise Update Event
						// If Collision Pair is not Exist Before, Raise Occur Event
						// If Original Collision Pair didn't Find the Corresponding Collision Pair, Raise Solved Event
						CollisionPairs = IntegrateOldCollisionPairsWithNewCollisionPairs(CollisionPairs, newCollisionPairs);

						if (CollisionPairs != null && CollisionPairs.Count() > 0)
						{
							// If Collision Event will Occur in xxx seconds, then Start Control
						}
					}
					Thread.Sleep(750);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				SystemStopped?.Invoke(DateTime.Now);
			}
		}

		/// <summary>整合舊的 Collision Pairs 與新的 Collision Pairs</summary>
		private List<CollisionPair> IntegrateOldCollisionPairsWithNewCollisionPairs(List<CollisionPair> oldPairs, List<CollisionPair> newPairs)
		{
			List<CollisionPair> result = null;

			// 若已有舊的 Collision Pairs ，則將其與新的 CollisionPairs 進行比較
			if (oldPairs != null)
			{
				for (int i = 0; i < oldPairs.Count(); ++i)
				{
					bool found = FindCorrespondingCollisionPair(oldPairs[i], newPairs);
					if (found)
					{
						if (result == null) result = new List<CollisionPair>();
						result.Add(oldPairs[i]);
					}
				}
			}

			// 將新的 Collision Pairs 加入
			if (newPairs != null && newPairs.Count() > 0)
			{
				if (result == null) result = new List<CollisionPair>();
				for (int i = 0; i < newPairs.Count(); ++i)
				{
					result.Add(newPairs[i]);
					CollisionOccured?.Invoke(newPairs[i]);
				}
			}

			return result;
		}

		/// <summary>幫舊的 Collision Pair 從新的 Collision Pairs 裡面找尋對應的 Collision Pair</summary>
		private bool FindCorrespondingCollisionPair(CollisionPair oldPair, List<CollisionPair> newPairs)
		{
			bool foundCorrespondence = false;

			if (newPairs != null)
			{
				for (int i = 0; i < newPairs.Count(); ++i)
				{
					if (IsCorresponding(oldPair, newPairs[i]))
					{
						oldPair = newPairs[i];
						newPairs.RemoveAt(i);
						foundCorrespondence = true;
					}
				}
			}

			// 若有找到對應的 Collision Pair ，則將舊的 Collision Pair 更新
			// 若沒有找到，則將舊的 Collision Pair 刪除
			if (foundCorrespondence)
			{
				CollisionUpdated?.Invoke(oldPair);
			}
			else
			{
				CollisionSolved?.Invoke(oldPair);
			}

			return foundCorrespondence;
		}

		private bool IsCorresponding(CollisionPair collisionPair1, CollisionPair collisionPair2)
		{
			bool result = false;
			if (collisionPair1.AGV1.Status.Name == collisionPair2.AGV1.Status.Name && collisionPair1.AGV2.Status.Name == collisionPair2.AGV2.Status.Name)
			{
				result = true;
			}
			else if (collisionPair1.AGV1.Status.Name == collisionPair2.AGV2.Status.Name && collisionPair1.AGV2.Status.Name == collisionPair2.AGV1.Status.Name)
			{
				result = true;
			}
			return result;
		}

		/// <summary>從所有車中，計算發生交會的組合</summary>
		private static List<CollisionPair> CalculateCollisionEvent(List<AGVInfo> agvs)
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

			// 輸出「路徑線重疊的組合」的資訊
			//if (pathOverlapPairs != null && pathOverlapPairs.Count > 0)
			//{
			//	for (int i = 0; i < pathOverlapPairs.Count(); ++i)
			//	{
			//		Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + pathOverlapPairs[i].ToString());
			//	}
			//}

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

			// 輸出「發生交會的組合」的資訊
			if (collisionPairs != null && collisionPairs.Count() > 0)
			{
				for (int i = 0; i < collisionPairs.Count(); ++i)
				{
					Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + collisionPairs[i].ToString());
				}
			}

			return collisionPairs;
		}
	}
}
