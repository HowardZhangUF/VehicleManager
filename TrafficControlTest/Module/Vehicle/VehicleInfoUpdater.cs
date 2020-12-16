using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.NewCommunication;

namespace TrafficControlTest.Module.Vehicle
{
	public class VehicleInfoUpdater : IVehicleInfoUpdater
	{
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IMissionStateManager rMissionStateManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;

		public VehicleInfoUpdater(IVehicleCommunicator VehicleCommunicator, IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(VehicleCommunicator, MissionStateManager, VehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			Unsubscribe_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			Subscribe_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			Unsubscribe_IMissionStateManager(rMissionStateManager);
			rMissionStateManager = MissionStateManager;
			Subscribe_IMissionStateManager(rMissionStateManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator, IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(VehicleCommunicator);
			Set(MissionStateManager);
			Set(VehicleInfoManager);
		}

		private void Subscribe_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.RemoteConnectStateChanged += HandleEvent_VehicleCommunicatorRemoteConnectStateChanged;
				VehicleCommunicator.ReceivedData += HandleEvent_VehicleCommunicatorReceivedData;
				VehicleCommunicator.SentDataSuccessed += HandleEvent_VehicleCommunicatorSentDataSuccessed;
			}
		}
		private void Unsubscribe_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
            {
                VehicleCommunicator.RemoteConnectStateChanged -= HandleEvent_VehicleCommunicatorRemoteConnectStateChanged;
                VehicleCommunicator.ReceivedData -= HandleEvent_VehicleCommunicatorReceivedData;
                VehicleCommunicator.SentDataSuccessed -= HandleEvent_VehicleCommunicatorSentDataSuccessed;
            }
		}
		private void Subscribe_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void Unsubscribe_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemUpdated -= HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChanged(object Sender, ConnectStateChangedEventArgs Args)
		{
			if (Args.IsConnected == false)
			{
				if (rVehicleInfoManager.IsExistByIpPort(Args.IpPort))
				{
					rVehicleInfoManager.Remove(rVehicleInfoManager.GetItemByIpPort(Args.IpPort).mName);
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorReceivedData(object Sender, ReceivedDataEventArgs Args)
		{
			if (Args.Data is Serializable)
			{
				// 處理收到的 Data 前再次確認源頭是否仍為連線中。若為連線中，則繼續處理該 Data ，反之，不處理該 Data
				if (rVehicleCommunicator.mClientIpPorts.Contains(Args.IpPort))
				{
					if (Args.Data is AGVStatus)
					{
						UpdateIVehicleInfo(Args.IpPort, Args.Data as AGVStatus);
					}
					else if (Args.Data is AGVPath)
					{
						UpdateIVehicleInfo(Args.IpPort, Args.Data as AGVPath);
					}
					else if (Args.Data is RequestMapList && (Args.Data as RequestMapList).Response != null)
					{
						UpdateIVehicleInfo(Args.IpPort, (Args.Data as RequestMapList).Response);
					}
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorSentDataSuccessed(object Sender, SentDataEventArgs Args)
		{
			if (Args.Data is Serializable)
			{
				if (Args.Data is InsertMovingBuffer)
				{
					InsertMovingBuffer tmpData = (Args.Data as InsertMovingBuffer);
					rVehicleInfoManager.GetItemByIpPort(Args.IpPort)?.UpdateCurrentInterveneCommand($"InsertMovingBuffer({tmpData.Require[0]},{tmpData.Require[1]})");
				}
				else if (Args.Data is PauseMoving)
				{
					rVehicleInfoManager.GetItemByIpPort(Args.IpPort)?.UpdateCurrentInterveneCommand("PauseMoving");
				}
			}
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			if (Args.StatusName.Contains("ExecuteState"))
			{
				switch (Args.Item.mExecuteState)
				{
					case ExecuteState.Unexecute:
						break;
					case ExecuteState.Executing:
						rVehicleInfoManager.UpdateItemMissionId(Args.Item.mExecutorId, Args.Item.mName);
						break;
					case ExecuteState.ExecuteSuccessed:
					case ExecuteState.ExecuteFailed:
						rVehicleInfoManager.UpdateItemMissionId(Args.Item.mExecutorId, string.Empty);
						break;
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			if (!string.IsNullOrEmpty(Args.Item.mCurrentInterveneCommand))
			{
				if (Args.StatusName.Contains("Path") && Args.Item.mCurrentInterveneCommand.StartsWith("InsertMovingBuffer"))
				{
					string movingBuffer = Args.Item.mCurrentInterveneCommand.Replace("InsertMovingBuffer", string.Empty);
					if (!Args.Item.mPathString.Contains(movingBuffer))
					{
						Args.Item.UpdateCurrentInterveneCommand(string.Empty);
					}
				}

				if (Args.StatusName.Contains("CurrentState") && Args.Item.mCurrentInterveneCommand.StartsWith("PauseMoving"))
				{
					if (Args.Item.mCurrentState != "Pause")
					{
						Args.Item.UpdateCurrentInterveneCommand(string.Empty);
					}
				}
			}
		}
		private void UpdateIVehicleInfo(string IpPort, AGVStatus AgvStatus)
		{
			if (!rVehicleInfoManager.IsExist(AgvStatus.Name))
			{
				IVehicleInfo tmpData = Library.Library.GenerateIVehicleInfo(AgvStatus.Name);
				tmpData.UpdateIpPort(IpPort);
				rVehicleInfoManager.Add(AgvStatus.Name, tmpData);
			}

			// 因應 iTSLibrary 的 EDescription 新增了 Operating 與 PathPlanning 項目，暫且先將二狀態轉換為 Running ，好讓原有系統能正常運作
			// Lock 時代表自走車被鎖定，仍屬於 Running 狀態
			string newState = AgvStatus.Description.ToString();
			if (newState == EDescription.Operating.ToString() || newState == EDescription.PathPlanning.ToString() || newState == EDescription.Lock.ToString()) newState = EDescription.Running.ToString();
			rVehicleInfoManager.UpdateItem(AgvStatus.Name, IpPort);
			rVehicleInfoManager.UpdateItem(AgvStatus.Name, newState, Library.Library.GenerateIPoint2D(double.IsNaN(AgvStatus.X) ? 0 : (int)AgvStatus.X, double.IsNaN(AgvStatus.Y) ? 0 : (int)AgvStatus.Y), double.IsNaN(AgvStatus.Toward) ? 0 : AgvStatus.Toward, AgvStatus.GoalName, double.IsNaN(AgvStatus.Velocity) ? 0 : AgvStatus.Velocity, double.IsNaN(AgvStatus.MapMatch) ? 0 : AgvStatus.MapMatch * 100, double.IsNaN(AgvStatus.Battery) ? 0 : AgvStatus.Battery, AgvStatus.AlarmMessage);
		}
		private void UpdateIVehicleInfo(string IpPort, AGVPath AgvPath)
		{
			if (!rVehicleInfoManager.IsExist(AgvPath.Name))
			{
				IVehicleInfo tmpData = Library.Library.GenerateIVehicleInfo(AgvPath.Name);
				tmpData.UpdateIpPort(IpPort);
				rVehicleInfoManager.Add(AgvPath.Name, tmpData);
			}

			rVehicleInfoManager.UpdateItem(AgvPath.Name, IpPort);
			rVehicleInfoManager.UpdateItem(AgvPath.Name, ConvertToPoints(AgvPath.PathX, AgvPath.PathY));
		}
		private IEnumerable<IPoint2D> ConvertToPoints(List<double> X, List<double> Y)
		{
			List<IPoint2D> result = new List<IPoint2D>();
			for (int i = 0; i < X.Count && i < Y.Count; ++i)
			{
				result.Add(Library.Library.GenerateIPoint2D((int)X[i], (int)Y[i]));
			}
			return result;
		}
		private void UpdateIVehicleInfo(string IpPort, List<string> MapList)
		{
			// 從 iTS 拿到的 MapList 資訊：
			// 1. 沒有副檔名 (.map)
			// 2. 名稱為 * 符號結尾的代表其為當前使用地圖
			if (rVehicleInfoManager.IsExistByIpPort(IpPort))
			{
				IVehicleInfo tmpData = rVehicleInfoManager.GetItemByIpPort(IpPort);
				tmpData.BeginUpdate();
				if (MapList.Any(o => o.EndsWith("*")))
				{
					tmpData.UpdateCurrentMapName(MapList.First(o => o.EndsWith("*")).TrimEnd('*') + ".map");
				}
				else
				{
					tmpData.UpdateCurrentMapName(string.Empty);
				}
				tmpData.UpdateCurrentMapNameList(MapList.Select(o => o + ".map"));
				tmpData.EndUpdate();
			}
		}
	}
}
