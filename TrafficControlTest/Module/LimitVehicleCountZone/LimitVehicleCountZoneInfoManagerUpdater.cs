﻿using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public class LimitVehicleCountZoneInfoManagerUpdater : SystemWithLoopTask, ILimitVehicleCountZoneInfoManagerUpdater
	{
		private ILimitVehicleCountZoneInfoManager rLimitVehicleCountZoneInfoManager = null;
		private IMapManager rMapManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;

		public LimitVehicleCountZoneInfoManagerUpdater(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(LimitVehicleCountZoneInfoManager, MapManager, VehicleInfoManager);
		}
		public void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			UnsubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
			rLimitVehicleCountZoneInfoManager = LimitVehicleCountZoneInfoManager;
			SubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(LimitVehicleCountZoneInfoManager);
			Set(MapManager);
			Set(VehicleInfoManager);
		}
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			result += $"CountOfLimitVehicleCountZoneInfo: {rLimitVehicleCountZoneInfoManager.mCount}, CountOfVehicleInfo: {rVehicleInfoManager.mCount}";
			return result;
		}
		public override void Task()
		{
			Subtask_CalculateVehicleNameListInLimitVehicleCountZoneInfo();
		}

		private void SubscribeEvent_ILimitVehicleCountZoneInfoManager(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			if (LimitVehicleCountZoneInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_ILimitVehicleCountZoneInfoManager(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			if (LimitVehicleCountZoneInfoManager != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged += HandleEvent_IMapManagerMapChanged;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged -= HandleEvent_IMapManagerMapChanged;
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				// do nothing
			}
		}
		private void HandleEvent_IMapManagerMapChanged(object sender, MapChangedEventArgs e)
		{
			rLimitVehicleCountZoneInfoManager.RemoveAll();
			List<IMapObjectOfRectangle> singleVehicleInfos = rMapManager.GetRectangleMapObjects(TypeOfMapObjectOfRectangle.SingleVehicle);
			if (singleVehicleInfos != null && singleVehicleInfos.Count > 0)
			{
                Dictionary<int, int> unionCollection = CalculateUnionCollection(singleVehicleInfos);


                for (int i = 0; i < singleVehicleInfos.Count; ++i)
				{
					// SingleVehicle 區塊沒有額外命名，所以名字採用流水號，允需車數量固定為 1
					if (unionCollection.ContainsKey(i))
					{
						ILimitVehicleCountZoneInfo tmp = Library.Library.GenerateILimitVehicleCountZoneInfo(i.ToString().PadLeft(3, '0'), singleVehicleInfos[i].mRange, 1, true, unionCollection[i]);
						rLimitVehicleCountZoneInfoManager.Add(tmp.mName, tmp);
					}
					else
					{
						ILimitVehicleCountZoneInfo tmp = Library.Library.GenerateILimitVehicleCountZoneInfo(i.ToString().PadLeft(3, '0'), singleVehicleInfos[i].mRange, 1, false, 0);
						rLimitVehicleCountZoneInfoManager.Add(tmp.mName, tmp);
					}
				}
			}
		}
		private void Subtask_CalculateVehicleNameListInLimitVehicleCountZoneInfo()
		{
			List<ILimitVehicleCountZoneInfo> tmpLimitVehicleCountZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().ToList();            
            if (tmpLimitVehicleCountZoneInfos.Count > 0)
			{
				List<List<Tuple<string, DateTime>>> newDatas = new List<List<Tuple<string, DateTime>>>();
				// 計算每一個 ILimitVehicleCountZoneInfo 的 CurrentVehicleNameList 資訊
				for (int i = 0; i < tmpLimitVehicleCountZoneInfos.Count; ++i)
				{
					List<Tuple<string, DateTime>> tmpCurrentVehicleNameList = new List<Tuple<string, DateTime>>();
					List<string> tmpVehicleNames = rVehicleInfoManager.GetItems().Where(o => tmpLimitVehicleCountZoneInfos[i].mRange.IsIncludePoint(o.mLocationCoordinate)).Select(o => o.mName).ToList();
					for (int j = 0; j < tmpVehicleNames.Count; ++j)
					{
						string vehicleName = tmpVehicleNames[j];
						DateTime enterTimestamp = DateTime.Now;
						if (tmpLimitVehicleCountZoneInfos[i].mIsUnioned) // 若該車早已進入聯集區域，則沿用該時間戳
						{
							int unionId = tmpLimitVehicleCountZoneInfos[i].mUnionId;
							List<ILimitVehicleCountZoneInfo> unionedZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().Where(o => o.mUnionId == unionId).ToList();
							for (int k = 0; k < unionedZoneInfos.Count; ++k)
							{
								if (unionedZoneInfos[k].ContainsVehicle(vehicleName))
								{
									if (unionedZoneInfos[k].GetVehicleEnterTimestamp(vehicleName) < enterTimestamp)
									{
										enterTimestamp = unionedZoneInfos[k].GetVehicleEnterTimestamp(vehicleName);
									}
								}
							}
						}
						tmpCurrentVehicleNameList.Add(new Tuple<string, DateTime>(vehicleName, enterTimestamp));
					}
					newDatas.Add(tmpCurrentVehicleNameList);
				}

				// 將所有 ILimitVehicleCountZoneInfo 的資訊都計算完後，再一次更新
				for (int i = 0; i < tmpLimitVehicleCountZoneInfos.Count; ++i)
				{
					rLimitVehicleCountZoneInfoManager.UpdateCurrentVehicleNameList(tmpLimitVehicleCountZoneInfos[i].mName, newDatas[i]);
				}
			}

        }
		/// <summary> 創建 相鄰List </summary>

		private static Dictionary<int, Stack<int>> CreateRectAdjacent(List<IMapObjectOfRectangle> Rectangles)
        {
			Dictionary<int, Stack<int>> RectAdjacent = new Dictionary<int, Stack<int>>(); //長方形相鄰字典
			for (int i = 0; i < Rectangles.Count; i++)
			{
				for (int j = 0; j < Rectangles.Count; j++)
				{
					if (i != j && GeometryAlgorithm.IsRectangleOverlap(Rectangles[i].mRange, Rectangles[j].mRange))
					{
						if (!RectAdjacent.ContainsKey(i))
							RectAdjacent[i] = new Stack<int>();
						RectAdjacent[i].Push(j);
					}
				}
			}
			return RectAdjacent;
		}
		/// <summary>創建 長方形相鄰聯集集合 </summary>
		private static List<HashSet<int>>CreateUnions(Dictionary<int, Stack<int>> RectAdjacent)
        {
			List<HashSet<int>> Unions = new List<HashSet<int>>(); //長方形相鄰聯集 集合
			HashSet<int> Union;//長方形相鄰聯集
			foreach (var rect in RectAdjacent)
			{
				Union = new HashSet<int>();
				if (rect.Value.Count == 0)
					continue;

				Union = FindUnionDFS(Union, RectAdjacent, rect.Key, RectAdjacent[rect.Key]);
				Unions.Add(Union);
			}
			return Unions;
		}
		/// <summary>使用DFS演算法 尋找相鄰長方形聯集</summary>
		private static HashSet<int> FindUnionDFS(HashSet<int> Union, Dictionary<int, Stack<int>> RectAdjacent, int RectVertice, Stack<int> NextToRectVertice)
		{

			Union.Add(RectVertice);
			while (NextToRectVertice.Count != 0)
			{
				RectVertice = NextToRectVertice.Pop();

				FindUnionDFS(Union, RectAdjacent, RectVertice, RectAdjacent[RectVertice]);
			}
			return Union;
		}
		private static Dictionary<int,int>CalculateUnionCollection(List<IMapObjectOfRectangle> Rectangles)
        {
			Dictionary<int, int> result = new Dictionary<int, int>(); // key = 區塊編號, value = union id
			int currentUnionId = 65536; // union id 從 65536 開始
			Dictionary<int, Stack<int>> RectAdjacent = CreateRectAdjacent(Rectangles); //長方形相鄰字典
			List<HashSet<int>> Unions = CreateUnions(RectAdjacent); //長方形相鄰聯集 集合

			foreach (var Union in Unions)
			{
				foreach (var RectNumber in Union)
					result[RectNumber] = currentUnionId;
				currentUnionId++;
			}

			// 範例輸出：
			// key value
			// 1   65536
			// 3   65536
			// 4   65537
			// 5   65537
			// 7   65536
			// 其中 1, 3, 7 是 Union ， 4, 5 是 Union
			return result;
		}

		
	}
}
