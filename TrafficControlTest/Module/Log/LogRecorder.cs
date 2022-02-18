using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.NewCommunication;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	public class LogRecorder : SystemWithLoopTask, ILogRecorder
	{
		private ICurrentLogAdapter rCurrentLogAdapter = null;
		private IHistoryLogAdapter rHistoryLogAdapter = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMissionStateManager rMissionStateManager = null;
		private IVehicleControlManager rVehicleControlManager = null;
		private IAutomaticDoorControlManager rAutomaticDoorControlManager = null;
		private IHostCommunicator rHostCommunicator = null;

		public LogRecorder(ICurrentLogAdapter CurrentLogAdapter, IHistoryLogAdapter HistoryLogAdapter, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager, IAutomaticDoorControlManager AutomaticDoorControlManager, IHostCommunicator HostCommunicator)
		{
			Set(CurrentLogAdapter, HistoryLogAdapter, VehicleInfoManager, MissionStateManager, VehicleControlManager, AutomaticDoorControlManager, HostCommunicator);
		}
		public void Set(ICurrentLogAdapter CurrentLogAdapter)
		{
			rCurrentLogAdapter = CurrentLogAdapter;
		}
		public void Set(IHistoryLogAdapter HistoryLogAdapter)
		{
			rHistoryLogAdapter = HistoryLogAdapter;
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			UnsubscribeEvent_IMissionStateManager(rMissionStateManager);
			rMissionStateManager = MissionStateManager;
			SubscribeEvent_IMissionStateManager(rMissionStateManager);
		}
		public void Set(IVehicleControlManager VehicleControlManager)
		{
			UnsubscribeEvent_IVehicleControlManager(rVehicleControlManager);
			rVehicleControlManager = VehicleControlManager;
			SubscribeEvent_IVehicleControlManager(rVehicleControlManager);
		}
		public void Set(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			UnsubscribeEvent_IAutomaticDoorControlManager(rAutomaticDoorControlManager);
			rAutomaticDoorControlManager = AutomaticDoorControlManager;
			SubscribeEvent_IAutomaticDoorControlManager(rAutomaticDoorControlManager);
		}
		public void Set(IHostCommunicator HostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = HostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}
		public void Set(ICurrentLogAdapter CurrentLogAdapter, IHistoryLogAdapter HistoryLogAdapter, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager, IAutomaticDoorControlManager AutomaticDoorControlManager, IHostCommunicator HostCommunicator)
		{
			Set(CurrentLogAdapter);
			Set(HistoryLogAdapter);
			Set(VehicleInfoManager);
			Set(MissionStateManager);
			Set(VehicleControlManager);
			Set(AutomaticDoorControlManager);
			Set(HostCommunicator);
		}
		public override string GetSystemInfo()
		{
			return $"CountOfVehicle: {rVehicleInfoManager.mCount}";
		}
		public override void Task()
		{
			Subtask_RecordVehicleInfo();
		}

		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded += HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded -= HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemUpdated -= HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemAdded += HandleEvent_VehicleControlManagerItemAdded;
				VehicleControlManager.ItemUpdated += HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemAdded -= HandleEvent_VehicleControlManagerItemAdded;
				VehicleControlManager.ItemUpdated -= HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				AutomaticDoorControlManager.ItemAdded += HandleEvent_AutomaticDoorControlManagerItemAdded;
				AutomaticDoorControlManager.ItemUpdated += HandleEvent_AutomaticDoorControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				AutomaticDoorControlManager.ItemAdded -= HandleEvent_AutomaticDoorControlManagerItemAdded;
				AutomaticDoorControlManager.ItemUpdated -= HandleEvent_AutomaticDoorControlManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.LocalListenStateChanged += HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.RemoteConnectStateChanged += HandleEvent_HostCommunicatorRemoteConnectStateChanged;
				HostCommunicator.SentData += HandleEvent_HostCommunicatorSentData;
				HostCommunicator.ReceivedData += HandleEvent_HostCommunicatorReceivedData;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.LocalListenStateChanged -= HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.RemoteConnectStateChanged -= HandleEvent_HostCommunicatorRemoteConnectStateChanged;
				HostCommunicator.SentData -= HandleEvent_HostCommunicatorSentData;
				HostCommunicator.ReceivedData -= HandleEvent_HostCommunicatorReceivedData;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			rCurrentLogAdapter.RecordCurrentVehicleInfo(DatabaseDataOperation.Add, Args.Item);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			rCurrentLogAdapter.RecordCurrentVehicleInfo(DatabaseDataOperation.Remove, Args.Item);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			rCurrentLogAdapter.RecordCurrentVehicleInfo(DatabaseDataOperation.Update, Args.Item);
            if (Args.StatusName.Contains("CurrentState") || Args.StatusName.Contains("CurrentOriState") || Args.StatusName.Contains("CurrentTarget"))
            {
                rHistoryLogAdapter.RecordHistoryVehicleInfo(DatabaseDataOperation.Add, Args.OccurTime, Args.Item);
            }
		}
		private void HandleEvent_MissionStateManagerItemAdded(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			rHistoryLogAdapter.RecordHistoryMissionInfo(DatabaseDataOperation.Add, Args.Item);
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			rHistoryLogAdapter.RecordHistoryMissionInfo(DatabaseDataOperation.Update, Args.Item);
		}
		private void HandleEvent_VehicleControlManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleControl> Args)
		{
			rHistoryLogAdapter.RecordHistoryVehicleControlInfo(DatabaseDataOperation.Add, Args.Item);
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleControl> Args)
		{
			rHistoryLogAdapter.RecordHistoryVehicleControlInfo(DatabaseDataOperation.Update, Args.Item);
		}
		private void HandleEvent_AutomaticDoorControlManagerItemAdded(object Sender, ItemCountChangedEventArgs<IAutomaticDoorControl> Args)
		{
			rHistoryLogAdapter.RecordHistoryAutomaticDoorControlInfo(DatabaseDataOperation.Add, Args.Item);
		}
		private void HandleEvent_AutomaticDoorControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IAutomaticDoorControl> Args)
		{
			rHistoryLogAdapter.RecordHistoryAutomaticDoorControlInfo(DatabaseDataOperation.Update, Args.Item);
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(object Sender, ListenStateChangedEventArgs Args)
		{
			rHistoryLogAdapter.RecordHistoryHostCommunicationInfo(DatabaseDataOperation.Add, Args.OccurTime, "LocalListenStateChanged", Args.Port.ToString(), $"IsListened: {Args.IsListened.ToString()}");
		}
		private void HandleEvent_HostCommunicatorRemoteConnectStateChanged(object Sender, ConnectStateChangedEventArgs Args)
		{
			rHistoryLogAdapter.RecordHistoryHostCommunicationInfo(DatabaseDataOperation.Add, Args.OccurTime, "RemoteConnectStateChanged", Args.IpPort, $"IsConnected: {Args.IsConnected.ToString()}");
		}
		private void HandleEvent_HostCommunicatorSentData(object Sender, SentDataEventArgs Args)
		{
			rHistoryLogAdapter.RecordHistoryHostCommunicationInfo(DatabaseDataOperation.Add, Args.OccurTime, "SentData", Args.IpPort, $"Data: {Args.Data}");
		}
		private void HandleEvent_HostCommunicatorReceivedData(object Sender, ReceivedDataEventArgs Args)
		{
			rHistoryLogAdapter.RecordHistoryHostCommunicationInfo(DatabaseDataOperation.Add, Args.OccurTime, "RecievedData", Args.IpPort, $"Data: {Args.Data}");
		}
		private void Subtask_RecordVehicleInfo()
		{
			List<IVehicleInfo> tmpDatas = rVehicleInfoManager.GetItems().ToList();
			DateTime tmpTimestamp = DateTime.Now;
			for (int i = 0; i < tmpDatas.Count; ++i)
			{
				rHistoryLogAdapter.RecordHistoryVehicleInfo(DatabaseDataOperation.Add, tmpTimestamp, tmpDatas[i]);
			}
		}
	}
}
