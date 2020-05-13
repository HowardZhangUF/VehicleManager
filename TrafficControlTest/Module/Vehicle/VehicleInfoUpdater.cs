using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Communication;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Mission;

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
				VehicleCommunicator.ReceivedSerializableData += HandleEvent_VehicleCommunicatorReceivedSerializableData;
				VehicleCommunicator.SentSerializableDataSuccessed += HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
			}
		}
		private void Unsubscribe_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.RemoteConnectStateChanged -= HandleEvent_VehicleCommunicatorRemoteConnectStateChanged;
				VehicleCommunicator.ReceivedSerializableData -= HandleEvent_VehicleCommunicatorReceivedSerializableData;
				VehicleCommunicator.SentSerializableDataSuccessed -= HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
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
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			if (NewState == ConnectState.Disconnected)
			{
				if (rVehicleInfoManager.IsExistByIpPort(IpPort))
				{
					rVehicleInfoManager.Remove(rVehicleInfoManager.GetItemByIpPort(IpPort).mName);
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			if (Data is Serializable)
			{
				// 處理收到的 Data 前再次確認源頭是否仍為連線中。若為連線中，則繼續處理該 Data ，反之，不處理該 Data
				if (rVehicleCommunicator.IsIpPortConnected(IpPort))
				{
					if (Data is AGVStatus)
					{
						UpdateIVehicleInfo(IpPort, Data as AGVStatus);
					}
					else if (Data is AGVPath)
					{
						UpdateIVehicleInfo(IpPort, Data as AGVPath);
					}
					else if (Data is RequestMapList && (Data as RequestMapList).Response != null)
					{
						UpdateIVehicleInfo(IpPort, (Data as RequestMapList).Response);
					}
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data)
		{
			if (Data is Serializable)
			{
				if (Data is InsertMovingBuffer)
				{
					InsertMovingBuffer tmpData = (Data as InsertMovingBuffer);
					rVehicleInfoManager.GetItemByIpPort(IpPort)?.UpdateCurrentInterveneCommand($"InsertMovingBuffer({tmpData.Require[0]},{tmpData.Require[1]})");
				}
				else if (Data is PauseMoving)
				{
					rVehicleInfoManager.GetItemByIpPort(IpPort)?.UpdateCurrentInterveneCommand("PauseMoving");
				}
			}
		}
		private void HandleEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IMissionState Item)
		{
			if (StateName.Contains("ExecuteState"))
			{
				switch (Item.mExecuteState)
				{
					case ExecuteState.Unexecute:
						break;
					case ExecuteState.Executing:
						rVehicleInfoManager.UpdateItemMissionId(Item.mExecutorId, Item.mName);
						break;
					case ExecuteState.ExecuteSuccessed:
					case ExecuteState.ExecuteFailed:
						rVehicleInfoManager.UpdateItemMissionId(Item.mExecutorId, string.Empty);
						break;
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo Item)
		{
			if (!string.IsNullOrEmpty(Item.mCurrentInterveneCommand))
			{
				if (StateName.Contains("Path") && Item.mCurrentInterveneCommand.StartsWith("InsertMovingBuffer"))
				{
					string movingBuffer = Item.mCurrentInterveneCommand.Replace("InsertMovingBuffer", string.Empty);
					if (!Item.mPathString.Contains(movingBuffer))
					{
						Item.UpdateCurrentInterveneCommand(string.Empty);
					}
				}

				if (StateName.Contains("CurrentState") && Item.mCurrentInterveneCommand.StartsWith("PauseMoving"))
				{
					if (Item.mCurrentState != "Pause")
					{
						Item.UpdateCurrentInterveneCommand(string.Empty);
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

			rVehicleInfoManager.UpdateItem(AgvStatus.Name, IpPort);
			rVehicleInfoManager.UpdateItem(AgvStatus.Name, AgvStatus.Description.ToString(), Library.Library.GenerateIPoint2D(double.IsNaN(AgvStatus.X) ? 0 : (int)AgvStatus.X, double.IsNaN(AgvStatus.Y) ? 0 : (int)AgvStatus.Y), AgvStatus.Toward, AgvStatus.GoalName, AgvStatus.Velocity, AgvStatus.MapMatch * 100, AgvStatus.Battery, AgvStatus.AlarmMessage);
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
			if (rVehicleInfoManager.IsExistByIpPort(IpPort))
			{
				IVehicleInfo tmpData = rVehicleInfoManager.GetItemByIpPort(IpPort);
				tmpData.BeginUpdate();
				if (MapList.Any(o => o.EndsWith("*")))
				{
					tmpData.UpdateCurrentMapName(MapList.First(o => o.EndsWith("*")).TrimEnd('*'));
				}
				else
				{
					tmpData.UpdateCurrentMapName(string.Empty);
				}
				tmpData.UpdateCurrentMapNameList(MapList);
				tmpData.EndUpdate();
			}
		}
	}
}
