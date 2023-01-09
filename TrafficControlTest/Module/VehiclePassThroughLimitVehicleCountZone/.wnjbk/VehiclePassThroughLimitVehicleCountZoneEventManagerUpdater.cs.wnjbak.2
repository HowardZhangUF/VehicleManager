using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.LimitVehicleCountZone;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.VehiclePassThroughAutomaticDoor;

namespace TrafficControlTest.Module.VehiclePassThroughLimitVehicleCountZone
{
	public class VehiclePassThroughLimitVehicleCountZoneEventManagerUpdater : SystemWithLoopTask, IVehiclePassThroughLimitVehicleCountZoneEventManagerUpdater
	{
		public int mDistanceThreshold { get; private set; } = 2500;

		private IVehiclePassThroughLimitVehicleCountZoneEventManager rVehiclePassThroughLimitVehicleCountZoneEventManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private ILimitVehicleCountZoneInfoManager rLimitVehicleCountZoneInfoManager = null;

		public VehiclePassThroughLimitVehicleCountZoneEventManagerUpdater(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleInfoManager VehicleInfoManager, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			Set(VehiclePassThroughLimitVehicleCountZoneEventManager, VehicleInfoManager, LimitVehicleCountZoneInfoManager);
		}
		public void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager)
		{
			UnsubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(rVehiclePassThroughLimitVehicleCountZoneEventManager);
			rVehiclePassThroughLimitVehicleCountZoneEventManager = VehiclePassThroughLimitVehicleCountZoneEventManager;
			SubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(rVehiclePassThroughLimitVehicleCountZoneEventManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);

		}
		public void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			UnsubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
			rLimitVehicleCountZoneInfoManager = LimitVehicleCountZoneInfoManager;
			SubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
		}
		public void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleInfoManager VehicleInfoManager, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			Set(VehiclePassThroughLimitVehicleCountZoneEventManager);
			Set(VehicleInfoManager);
			Set(LimitVehicleCountZoneInfoManager);
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "TimePeriod", "DistanceThreshold" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "DistanceThreshold":
					return mDistanceThreshold.ToString();
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
				case "DistanceThreshold":
					mDistanceThreshold = int.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			List<IVehicleInfo> vehicleInfos = rVehicleInfoManager.GetItems().Where(o => o.mCurrentState == "Running" || o.mCurrentState == "Operating" || o.mCurrentState == "Pause").ToList();
			List<ILimitVehicleCountZoneInfo> zoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().ToList();
			List<IVehiclePassThroughLimitVehicleCountZoneEvent> eventInfos = rVehiclePassThroughLimitVehicleCountZoneEventManager.GetItems().ToList();
			result += $"VehicleRunningOperatingPauseCount: {vehicleInfos.Count}";
			result += $", LimitVehicleCountZoneCount: {zoneInfos.Count}";
			result += $", VehiclePassThroughLimitVehicleCountZoneEventCount: {eventInfos.Count}";
			return result;
		}
		public override void Task()
		{
			Subtask_DetectVehiclePassThroughLimitVehicleCountZoneEvent();
		}

		private void SubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager)
		{
			if (VehiclePassThroughLimitVehicleCountZoneEventManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager)
		{
			if (VehiclePassThroughLimitVehicleCountZoneEventManager != null)
			{
				// do nothing
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
		private void Subtask_DetectVehiclePassThroughLimitVehicleCountZoneEvent()
		{
			List<IVehicleInfo> vehicleInfos = rVehicleInfoManager.GetItems().ToList();
			List<ILimitVehicleCountZoneInfo> limitVehicleCountZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().ToList();
			List<IVehiclePassThroughLimitVehicleCountZoneEvent> currentEvents = new List<IVehiclePassThroughLimitVehicleCountZoneEvent>();
			List<IVehiclePassThroughLimitVehicleCountZoneEvent> lastEvents = rVehiclePassThroughLimitVehicleCountZoneEventManager.GetItems().ToList();
			if (vehicleInfos != null && vehicleInfos.Count > 0 && limitVehicleCountZoneInfos != null && limitVehicleCountZoneInfos.Count > 0)
			{
				// 計算當前的事件集合
				for (int i = 0; i < vehicleInfos.Count; ++i)
				{
					for (int j = 0; j < limitVehicleCountZoneInfos.Count; ++j)
					{
						//若限車區 讓車字典含值
						if (limitVehicleCountZoneInfos[j].mLetgo.ContainsKey(vehicleInfos[i].mName))
						{
							limitCountZoneLetgoCleanup(vehicleInfos[i], vehicleInfos[i].mCurrentState, limitVehicleCountZoneInfos, j);
						}
						// 如果自走車已經走到限車區內
						if (IsVehicleInLimitVehicleCountZone(vehicleInfos[i], limitVehicleCountZoneInfos[j]))
						{
							if (IsILimitVehicleCountZoneFull(limitVehicleCountZoneInfos[j]))
							{
								// 如果自走車不在該區域的允許移動名單內
								if (!IsVehicleAllowedMoveBeforeLimitVehicleCountZone(vehicleInfos[i], limitVehicleCountZoneInfos[j]))
								{
                                    //Console.WriteLine($"車輛{vehicleInfos[i].mName},進入第一個if 條件:{!IsVehicleAllowedMoveBeforeLimitVehicleCountZone(vehicleInfos[i], limitVehicleCountZoneInfos[j])}");
									IVehiclePassThroughLimitVehicleCountZoneEvent tmp = Library.Library.GenerateIVehiclePassThroughLimitVehicleCountZoneEvent(vehicleInfos[i], limitVehicleCountZoneInfos[j], 0);
									currentEvents.Add(tmp);
								}
							}
						}
						// 如果自走車還沒走到限車區內
						else
						{
							//其他車 且不包含會去Park or Dock的車
							List<IVehicleInfo> OtherVehicleInfos = vehicleInfos.Where(o => o.mName != vehicleInfos[i].mName && !o.mCurrentTarget.Contains("Park") && !o.mCurrentTarget.Contains("Dock") && o.mCurrentMapName == vehicleInfos[i].mCurrentMapName).ToList();
							// 如果車子路徑線有穿越限車區
							if (IsVehiclePassThroughLimitVehicleCountZone(vehicleInfos[i], limitVehicleCountZoneInfos[j]))
							{
								//自走車 是否為會去Park或Dock的車
								if(vehicleInfos[i].mCurrentTarget.Contains("Park") || vehicleInfos[i].mCurrentTarget.Contains("Dock"))//主要車 前往park or dock
								{
									// 自走車是否會擋到其他不會去Park的車
									if (IsParkCarWillBlockOtherCarPassLimitZone(vehicleInfos[i], OtherVehicleInfos, limitVehicleCountZoneInfos[j]))
                                    {
                                        Console.WriteLine($"停止車輛事件觸發");
										IVehiclePassThroughLimitVehicleCountZoneEvent tmp = Library.Library.GenerateIVehiclePassThroughLimitVehicleCountZoneEvent(vehicleInfos[i], limitVehicleCountZoneInfos[j], 0);
										currentEvents.Add(tmp);
										rVehiclePassThroughLimitVehicleCountZoneEventManager.UpdateState(vehicleInfos[i].mName, limitVehicleCountZoneInfos[j].mName, PassThroughState.WillLetgo);
									}
									
									
								}

								// 該限車區為滿的
								if (IsILimitVehicleCountZoneFull(limitVehicleCountZoneInfos[j]))
								{
									int distance = GetDistanceBetweenVehicleAndLimitVehicleCountZoneAlongPathLine(vehicleInfos[i], limitVehicleCountZoneInfos[j]);
									// 車子不是限車區裡面的車，且即將通過限車區
									// 加上距離大於 0 的條件是為了避免運算溢位的錯誤
									// 距離等於 0 代表車子在限車區內部
									if (!IsVehicleAllowedMoveBeforeLimitVehicleCountZone(vehicleInfos[i], limitVehicleCountZoneInfos[j]) && distance >= 0 && distance < mDistanceThreshold)
									{
										//Console.WriteLine($"車輛{vehicleInfos[i].mName},進入第二個if 條件:{!IsVehicleAllowedMoveBeforeLimitVehicleCountZone(vehicleInfos[i],limitVehicleCountZoneInfos[j])}");
										
										IVehiclePassThroughLimitVehicleCountZoneEvent tmp = Library.Library.GenerateIVehiclePassThroughLimitVehicleCountZoneEvent(vehicleInfos[i], limitVehicleCountZoneInfos[j], distance);
										currentEvents.Add(tmp);
									}
								}
							}
						}
					}
				}

				// 新的事件集合與舊的事件集合比較 (Add, Update Item 判斷)
				for (int i = 0; i < currentEvents.Count; ++i)
				{
					// 如果 Event 已存在，更新之，反之，新增之
					if (rVehiclePassThroughLimitVehicleCountZoneEventManager.IsExist(currentEvents[i].rVehicleInfo.mName, currentEvents[i].rLimitVehicleCountZoneInfo.mName))
					{
						rVehiclePassThroughLimitVehicleCountZoneEventManager.UpdateDistance(currentEvents[i].rVehicleInfo.mName, currentEvents[i].rLimitVehicleCountZoneInfo.mName, currentEvents[i].mDistance);
						rVehiclePassThroughLimitVehicleCountZoneEventManager.UpdateState(currentEvents[i].rVehicleInfo.mName, currentEvents[i].rLimitVehicleCountZoneInfo.mName, currentEvents[i].mState);
					}
					else
					{
						rVehiclePassThroughLimitVehicleCountZoneEventManager.Add(currentEvents[i].mName, currentEvents[i]);
					}
				}

				// 舊的事件集合與新的事件集合比較 (Remove Item 判斷)
				for (int i = 0; i < lastEvents.Count; ++i)
				{
					// 如果舊的 Event 在當前事件集合中找不到對應的項目，則代表該 Event 已結束
					if (!currentEvents.Any(o => o.rVehicleInfo.mName == lastEvents[i].rVehicleInfo.mName && o.rLimitVehicleCountZoneInfo.mName == lastEvents[i].rLimitVehicleCountZoneInfo.mName))
					{
						rVehiclePassThroughLimitVehicleCountZoneEventManager.Remove(lastEvents[i].mName);
					}
				}
			}
		}
		/// <summary>計算指定 IVehicleInfo 是否在指定 ILimitVehicleCountZoneInfo 區域內</summary>
		private bool IsVehicleInLimitVehicleCountZone(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			return LimitVehicleCountZoneInfo.mRange.IsIncludePoint(VehicleInfo.mLocationCoordinate);
		}
		/// <summary>計算指定 IVehicleInfo 的路徑是否有穿越指定 ILimitVehicleCountZoneInfo 區域</summary>
		private bool IsVehiclePassThroughLimitVehicleCountZone(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			List<IPoint2D> fullPath = new List<IPoint2D>();
			fullPath.Add(VehicleInfo.mLocationCoordinate);
			fullPath.AddRange(VehicleInfo.mPath);
			return GeometryAlgorithm.IsLinePassThroughRectangle(fullPath, LimitVehicleCountZoneInfo.mRange);
		}
		/// <summary>查詢指定的 ILimitVehicleCountZoneInfo 是否已滿車</summary>
		private bool IsILimitVehicleCountZoneFull(ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			bool result = false;
			if (GetCurrentVehicleCount(LimitVehicleCountZoneInfo, rLimitVehicleCountZoneInfoManager) >= LimitVehicleCountZoneInfo.mMaxVehicleCount)
			{
				result = true;
			}
			return result;
		}
		/// <summary>啟動讓車 每個Park車 至多對應到一台</summary>		
		private void limitCountZoneLetgoOnceActivate(IVehicleInfo ParkVehicleInfo, IVehicleInfo Othervehicle, List<ILimitVehicleCountZoneInfo> limitVehicleCountZoneInfos, int limitVehicleIndex)
        {
			if(limitVehicleCountZoneInfos[limitVehicleIndex].mIsUnioned)
            {
				int unionId = limitVehicleCountZoneInfos[limitVehicleIndex].mUnionId;
				List<ILimitVehicleCountZoneInfo> unionedZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().Where(o => o.mUnionId == unionId).ToList();
				foreach (var i in unionedZoneInfos)
                {
					limitVehicleCountZoneInfos[Int32.Parse(i.mName)].mLetgo[ParkVehicleInfo.mName] = Othervehicle.mName;
					//Console.WriteLine($"Park車{ParkVehicleInfo.mName}增讓{limitVehicleCountZoneInfos[limitVehicleIndex].mLetgo[ParkVehicleInfo.mName]}");
				}
					
			}
			else
            {				
				limitVehicleCountZoneInfos[limitVehicleIndex].mLetgo[ParkVehicleInfo.mName] = Othervehicle.mName;
                //Console.WriteLine($"Park車{ParkVehicleInfo.mName}增讓{limitVehicleCountZoneInfos[limitVehicleIndex].mLetgo[ParkVehicleInfo.mName]}");
            }
        }
		/// <summary> 清空限車區內的讓車 </summary>
		private void limitCountZoneLetgoCleanup(IVehicleInfo vehicleInfo, String VehicleCurrentState,List<ILimitVehicleCountZoneInfo> limitVehicleCountZoneInfos,int limitVehicleIndex)
        {
			
			if (VehicleCurrentState == "Pause")
			{				
				if (IsVehicleInLimitVehicleCountZone(vehicleInfo, limitVehicleCountZoneInfos[limitVehicleIndex]))
				{
					if (limitVehicleCountZoneInfos[limitVehicleIndex].mIsUnioned)
					{
						int unionId = limitVehicleCountZoneInfos[limitVehicleIndex].mUnionId;
						List<ILimitVehicleCountZoneInfo> unionedZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().Where(o => o.mUnionId == unionId).ToList();
                        Console.WriteLine($"聯集清消");
						foreach (var i in unionedZoneInfos)
						{
                            //Console.WriteLine($"限車區號碼{i.mName},unionID{i.mUnionId},Park車{vehicleInfo.mName}狀態{vehicleInfo.mCurrentState} 持續時間{vehicleInfo.mCurrentStateDuration}位置{vehicleInfo.mLocationCoordinate} ");
                            //Console.WriteLine($"{limitVehicleCountZoneInfos[Int32.Parse(i.mName)]}");
                            //Console.WriteLine($"讓車{limitVehicleCountZoneInfos[Int32.Parse(i.mName)].mLetgo[vehicleInfo.mName]}");
                            limitVehicleCountZoneInfos[Int32.Parse(i.mName)].mLetgo.Remove(vehicleInfo.mName);
						}


					}
					else
					{
                        //Console.WriteLine($"限車區號碼{limitVehicleCountZoneInfos[limitVehicleIndex].mName},Park車{vehicleInfo.mName}狀態{vehicleInfo.mCurrentState} 持續時間{vehicleInfo.mCurrentStateDuration}位置{vehicleInfo.mLocationCoordinate} ");
                        //Console.WriteLine($"{limitVehicleCountZoneInfos[limitVehicleIndex]}");
                        //Console.WriteLine($"讓車{limitVehicleCountZoneInfos[limitVehicleIndex].mLetgo[vehicleInfo.mName]}");
                                
						limitVehicleCountZoneInfos[limitVehicleIndex].mLetgo.Remove(vehicleInfo.mName);
					}
				}		
			}
			else if(VehicleCurrentState!="Running")
			{
                //Console.WriteLine($"清消限車區號碼:{limitVehicleIndex},Park車{vehicleInfo.mName}狀態{vehicleInfo.mCurrentState}");
                //Console.WriteLine($"清消車:{limitVehicleCountZoneInfos[limitVehicleIndex].mLetgo[vehicleInfo.mName]}");
				limitVehicleCountZoneInfos[limitVehicleIndex].mLetgo.Remove(vehicleInfo.mName);

			}
			
			
        }
		/// <summary>去Park或Dock的車 是否會阻礙其他車(異向)通過限車區</summary>
		private bool IsParkCarWillBlockOtherCarPassLimitZone(IVehicleInfo VehicleInfo, List<IVehicleInfo> OtherVehicleInfos, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
        {			
			foreach (var car in OtherVehicleInfos)
			{
				//若Park內含有 A車則 B車跳過不執行
				if(LimitVehicleCountZoneInfo.mLetgo.Count!=0 && LimitVehicleCountZoneInfo.mLetgo[VehicleInfo.mName]!=car.mName)
                {
					continue;
                }
				//若其他車為停止狀態 則park車不會於該限車區碰到其他車 (主要防止人為或其他狀況導致其他車停止)
				//若park車距離該限車區 小於閾值 予以放行
				if(car.mCurrentState!="Pause" && LimitVehicleCountZoneInfo.mCurrentVehicleNameList.Count==0 )
				{

					if (LimitVehicleCountZoneInfo.mIsUnioned)//如果欲查詢的 zone 有與其他 zone 聯集 (可先看單一zone的)
					{
						
                        
						int unionId = LimitVehicleCountZoneInfo.mUnionId;
						List<ILimitVehicleCountZoneInfo> unionedZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().Where(o => o.mUnionId == unionId).ToList();
						
						
						if (unionedZoneInfos.Any(o => GeometryAlgorithm.IsAnyPointInside(car.mPath, o.mRange)) && !unionedZoneInfos.Any(o=>IsVehicleInLimitVehicleCountZone(VehicleInfo,o)))
						{
							//Console.WriteLine($"進入聯集區域 條件二");
							IPoint2D MainCarEnterpoint = VehicleInfo.mPath.Where(o => unionedZoneInfos.Any(s => GeometryAlgorithm.IsPointInside(o, s.mRange))).First();
							IPoint2D OtherCarEnterpoint = car.mPath.Where(o => unionedZoneInfos.Any(s => GeometryAlgorithm.IsPointInside(o, s.mRange))).First();

							IPoint2D MainCarLeavePoint = VehicleInfo.mPath.Where(o => unionedZoneInfos.Any(s => GeometryAlgorithm.IsPointInside(o, s.mRange))).Last();
							IPoint2D OtherCarLeavePoint = car.mPath.Where(o => unionedZoneInfos.Any(s => GeometryAlgorithm.IsPointInside(o, s.mRange))).Last();

							

                            //Console.WriteLine($"絕對距離:{GeometryAlgorithm.GetDistance(MainCarEnterpoint,OtherCarEnterpoint)}");

							double distance = GeometryAlgorithm.GetDistance(VehicleInfo.mLocationCoordinate,MainCarEnterpoint);
							//Console.WriteLine($"車名{VehicleInfo.mName},目前座標{VehicleInfo.mLocationCoordinate},進入點:{MainCarEnterpoint},距離長方形:{distance}");

							//park車與其他車限車區進入點>50 =>判斷為異向
							//distance 的設定為防止park車太過接近限車區擋到其他車輛(若小於閾值 放行park車)
							if (GeometryAlgorithm.GetDistance(MainCarEnterpoint, OtherCarEnterpoint)>50 && distance > mDistanceThreshold && distance < 4000)
							{
								//Console.WriteLine($"進入條件三:主車{VehicleInfo.mName} 其餘車{car.mName}");
								//擷取總距離A(主車(park車)目前位置 至 主車(park車)限車區離開點) /車速
								int MainCarLeaveNode = VehicleInfo.mPath.IndexOf(MainCarLeavePoint) + 1; //主車目前位置 至離開點 之節點數
								double DistanceA = GeometryAlgorithm.GetDistance(VehicleInfo.mPath.ToList().GetRange(0, MainCarLeaveNode));
								double TimeA = DistanceA / VehicleInfo.mTranslationVelocity;


								//擷取總距離B(其他車目前位置  至   其他車限車區進入點)  /車速
								int OtherCarEnterNode = car.mPath.IndexOf(OtherCarEnterpoint) + 1; //其他車目前位置 至進入點 之節點數
								double DistanceB = GeometryAlgorithm.GetDistance(car.mPath.ToList().GetRange(0, OtherCarEnterNode));
								double TimeB = DistanceB / car.mTranslationVelocity;
								//若 時間B' > A' 代表Park車離開時 其他車還未進來 該限車區(不會擋到,回傳false)
								//反之 則更新其他車輛 進入限車區的時間
								//讓車機制 只讓一次
								if (TimeB <= TimeA)
								{
									Console.WriteLine($"進入最後一個條件 主車{VehicleInfo.mName},其他車{car.mName}:車速{car.mTranslationVelocity}");
									//List<Tuple<string, DateTime>> tmpCurrentVehicleNameList = new List<Tuple<string, DateTime>>();
									

									//tmpCurrentVehicleNameList.Add(new Tuple<string, DateTime>(car.mName, DateTime.Now));
									//if(!LimitVehicleCountZoneInfo.ContainsVehicle(VehicleInfo.mName))
									//	tmpCurrentVehicleNameList.Add(new Tuple<string, DateTime>(VehicleInfo.mName, DateTime.Now.AddSeconds(1)));

									//LimitVehicleCountZoneInfo.UpdateCurrentVehicleNameList(tmpCurrentVehicleNameList);

									//rLimitVehicleCountZoneInfoManager.UpdateCurrentVehicleNameList(LimitVehicleCountZoneInfo.mName, tmpCurrentVehicleNameList);
                                    
									var LimitVehicleCountZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().ToList();
									int LimitVehicleIndex = Int32.Parse(LimitVehicleCountZoneInfo.mName);
									limitCountZoneLetgoOnceActivate(VehicleInfo, car, LimitVehicleCountZoneInfos, LimitVehicleIndex);
									return true;
								}

							}


						}
					}
					else //若查詢的zone為單一zone
					{
					//判斷其餘車輛的路徑 有無經過同一限車區
						if (GeometryAlgorithm.IsAnyPointInside(car.mPath, LimitVehicleCountZoneInfo.mRange))
						{
							//Console.WriteLine($"進入條件二:主車{VehicleInfo.mName} 其餘車{car.mName}");
							//主要車、其餘車初入該限車區進入點 
							IPoint2D MainCarEnterpoint = VehicleInfo.mPath.Where(o => GeometryAlgorithm.IsPointInside(o, LimitVehicleCountZoneInfo.mRange)).First();
							IPoint2D OtherCarEnterpoint = car.mPath.Where(o => GeometryAlgorithm.IsPointInside(o, LimitVehicleCountZoneInfo.mRange)).First();

							//主要車、其餘車 限車區離去點
							IPoint2D MainCarLeavePoint = VehicleInfo.mPath.Where(o => GeometryAlgorithm.IsPointInside(o, LimitVehicleCountZoneInfo.mRange)).Last();
							IPoint2D OtherCarLeavePoint = car.mPath.Where(o => GeometryAlgorithm.IsPointInside(o, LimitVehicleCountZoneInfo.mRange)).Last();

							//Console.WriteLine($"主車  進入點:{MainCarEnterpoint}  主車  離開點:{MainCarLeavePoint}");
							//Console.WriteLine($"其他車進入點:{OtherCarEnterpoint} 其他車離開點:{OtherCarLeavePoint}");
							//兩向量 分別為 主要車輛(park車) 與其餘車輛 之進入點  與  其他車輛之離開點  的向量差
							
							

							//Console.WriteLine($"絕對距離:{GeometryAlgorithm.GetDistance(MainCarEnterpoint, OtherCarEnterpoint)}");

							double distance = GeometryAlgorithm.GetDistance(VehicleInfo.mLocationCoordinate, MainCarEnterpoint);
							//Console.WriteLine($"車名{VehicleInfo.mName},目前座標{VehicleInfo.mLocationCoordinate},進入點:{MainCarEnterpoint},距離長方形:{distance}");
							//park車與其他車限車區進入點>50 =>判斷為異向
							//distance 的設定為防止park車太過接近限車區擋到其他車輛(若小於閾值 放行park車)  
							if (GeometryAlgorithm.GetDistance(MainCarEnterpoint,OtherCarEnterpoint)>50 && distance>mDistanceThreshold &&distance < 4000)
							{
								//Console.WriteLine($"進入條件三:主車{VehicleInfo.mName} 其餘車{car.mName}");
								//擷取總距離A(主車(park車)目前位置 至 主車(park車)限車區離開點) /車速
								int MainCarLeaveNode = VehicleInfo.mPath.IndexOf(MainCarLeavePoint) + 1; //主車目前位置 至離開點 之節點數
								double DistanceA = GeometryAlgorithm.GetDistance(VehicleInfo.mPath.ToList().GetRange(0, MainCarLeaveNode));
								double TimeA = DistanceA / VehicleInfo.mTranslationVelocity;


								//擷取總距離B(其他車目前位置  至   其他車限車區進入點)  /車速
								int OtherCarEnterNode = car.mPath.IndexOf(OtherCarEnterpoint) + 1; //其他車目前位置 至進入點 之節點數
								double DistanceB = GeometryAlgorithm.GetDistance(car.mPath.ToList().GetRange(0, OtherCarEnterNode));
								double TimeB = DistanceB / car.mTranslationVelocity;
								//若 時間B' > A' 代表Park車離開時 其他車還未進來 該限車區(不會擋到,回傳false)
								//反之 則回傳True, 並啟用讓一次車機制

								if (TimeB <= TimeA)
								{
									Console.WriteLine($"進入最後一個條件 主車{VehicleInfo.mName},其他車{car.mName}:車速{car.mTranslationVelocity}");
									//var VehicleInfos=rVehicleInfoManager.GetItems().ToList();
									//int i = VehicleInfos.FindIndex(o => o == VehicleInfo);
									//VehicleInfos[i].mLetgo[VehicleInfo.mName] = car.mName;
									var LimitVehicleCountZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().ToList();
									int LimitVehicleIndex = Int32.Parse(LimitVehicleCountZoneInfo.mName);
									limitCountZoneLetgoOnceActivate(VehicleInfo,car, LimitVehicleCountZoneInfos, LimitVehicleIndex);
									return true;
									//List<Tuple<string, DateTime>> tmpCurrentVehicleNameList = new List<Tuple<string, DateTime>>();
									
									//tmpCurrentVehicleNameList.Add(new Tuple<string, DateTime>(car.mName,DateTime.Now));
                                    

                                    //rLimitVehicleCountZoneInfoManager.UpdateCurrentVehicleNameList(LimitVehicleCountZoneInfo.mName, tmpCurrentVehicleNameList);
         //                           Console.WriteLine($"限車區名字{LimitVehicleCountZoneInfo.mName}");
									//Console.WriteLine($"<最後條件>目前限車區內含有的車輛名單{String.Join(",", LimitVehicleCountZoneInfo.mCurrentVehicleNameList)}");
								}

							}
						}
					}
				}

					
					
			}
			
			return false;

		}

		/// <summary>計算指定 IVehicleInfo 在到達指定 ILimitVehicleCountZoneInfo 前是否允許移動</summary>
		private bool IsVehicleAllowedMoveBeforeLimitVehicleCountZone(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			return GetCurrentVehicleNameList(LimitVehicleCountZoneInfo, rLimitVehicleCountZoneInfoManager).Take(LimitVehicleCountZoneInfo.mMaxVehicleCount).Contains(VehicleInfo.mName);
		}


        /// <summary>計算指定 IVehicleInfo 與指定 ILimitVehicleCountZoneInfo 的距離(沿著路徑線計算)</summary>
        private int GetDistanceBetweenVehicleAndLimitVehicleCountZoneAlongPathLine(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo)
		{
			int result = 0;
			if (!LimitVehicleCountZoneInfo.mRange.IsIncludePoint(VehicleInfo.mLocationCoordinate))
			{
				if (VehicleInfo.mPathDetail != null && VehicleInfo.mPathDetail.Count > 0)
				{
					List<IPoint2D> fullPath = new List<IPoint2D>();
					fullPath.Add(VehicleInfo.mLocationCoordinate);
					fullPath.AddRange(VehicleInfo.mPathDetail);

					for (int i = 0; i < fullPath.Count - 1; ++i)
					{
						List<IPoint2D> intersectionPoint = GeometryAlgorithm.GetIntersectionPoint(LimitVehicleCountZoneInfo.mRange, fullPath[i], fullPath[i + 1]).ToList();
						if (intersectionPoint != null && intersectionPoint.Count > 0)
						{
							// 找到交點
							List<IPoint2D> points = fullPath.Take(i + 1).ToList();
							points.Add(intersectionPoint.ElementAt(0));
							result = (int)GeometryAlgorithm.GetDistance(points);
							break;
						}
					}
				}
			}
			return result;
		}

		/// <summary>查詢指定的 ILimitVehicleCountZoneInfo 的當前資訊。若該區有其他區聯集，會一同計算</summary>
		private static List<Tuple<string, DateTime>> GetCurrentVehicleList(ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			if (LimitVehicleCountZoneInfo.mIsUnioned) // 如果欲查詢的 zone 有與其他 zone 聯集
			{
				int unionId = LimitVehicleCountZoneInfo.mUnionId;
				List<ILimitVehicleCountZoneInfo> unionedZoneInfos = LimitVehicleCountZoneInfoManager.GetItems().Where(o => o.mUnionId == unionId).ToList();

				Dictionary<string, Tuple<string, DateTime>> result = new Dictionary<string, Tuple<string, DateTime>>();
				for (int i = 0; i < unionedZoneInfos.Count; ++i)
				{
					for (int j = 0; j < unionedZoneInfos[i].mCurrentVehicleNameList.Count; ++j)
					{
						string vehicleName = unionedZoneInfos[i].mCurrentVehicleNameList[j].Item1;
						DateTime enterTimestamp = unionedZoneInfos[i].mCurrentVehicleNameList[j].Item2;
						if (result.ContainsKey(vehicleName)) // 如果項目重複
						{
							if (enterTimestamp < result[vehicleName].Item2) // 進入時間點使用最小值
							{
								result[vehicleName] = unionedZoneInfos[i].mCurrentVehicleNameList[j];
							}
						}
						else
						{
							result.Add(vehicleName, unionedZoneInfos[i].mCurrentVehicleNameList[j]);
						}
					}
				}
				return result.Values.OrderBy(o => o.Item2).ToList();
			}
			else
			{
				return LimitVehicleCountZoneInfo.mCurrentVehicleNameList.OrderBy(o => o.Item2).ToList();
			}
		}
		/// <summary>查詢指定的 ILimitVehicleCountZoneInfo 的當前自走車清單。若該區有其他區聯集，會一同計算</summary>
		private static List<string> GetCurrentVehicleNameList(ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			return GetCurrentVehicleList(LimitVehicleCountZoneInfo, LimitVehicleCountZoneInfoManager).Select(o => o.Item1).ToList();
		}
		/// <summary>查詢指定的 ILimitVehicleCountZoneInfo 的當前自走車數量。若該區有其他區聯集，會一同計算</summary>
		private static int GetCurrentVehicleCount(ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			return GetCurrentVehicleList(LimitVehicleCountZoneInfo, LimitVehicleCountZoneInfoManager).Count();
		}
	}
}
