using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Implement
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
			rVehicleInfoManager = VehicleInfoManager;
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
			}
		}
		private void Unsubscribe_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.RemoteConnectStateChanged -= HandleEvent_VehicleCommunicatorRemoteConnectStateChanged;
				VehicleCommunicator.ReceivedSerializableData -= HandleEvent_VehicleCommunicatorReceivedSerializableData;
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
		private void UpdateIVehicleInfo(string IpPort, AGVStatus AgvStatus)
		{
			if (!rVehicleInfoManager.IsExist(AgvStatus.Name))
			{
				IVehicleInfo tmpData = Library.Library.GenerateIVehicleInfo(AgvStatus.Name);
				tmpData.UpdateIpPort(IpPort);
				rVehicleInfoManager.Add(AgvStatus.Name, tmpData);
			}

			rVehicleInfoManager.UpdateItem(AgvStatus.Name, IpPort);
			rVehicleInfoManager.UpdateItem(AgvStatus.Name, AgvStatus.Description.ToString(), Library.Library.GenerateIPoint2D((int)AgvStatus.X, (int)AgvStatus.Y), AgvStatus.Toward, AgvStatus.GoalName, AgvStatus.Velocity, AgvStatus.MapMatch * 100, AgvStatus.Battery, AgvStatus.AlarmMessage);
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
			for (int i = 0; i < X.Count; ++i)
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
					tmpData.UpdateCurrentMapName(MapList.First(o => o.EndsWith("*")).Replace("*", ".map"));
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
