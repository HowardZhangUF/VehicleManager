using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.NewCommunication;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	public class ImportantEventRecorder : SystemWithLoopTask, IImportantEventRecorder
	{
		private IEventRecorder rEventRecorder = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMissionStateManager rMissionStateManager = null;
		private IHostCommunicator rHostCommunicator = null;

		public ImportantEventRecorder(IEventRecorder EventRecorder, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IHostCommunicator HostCommunicator)
		{
			Set(EventRecorder, VehicleInfoManager, MissionStateManager, HostCommunicator);
		}
		public void Set(IEventRecorder EventRecorder)
		{
			rEventRecorder = EventRecorder;
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
		public void Set(IHostCommunicator HostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = HostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}
		public void Set(IEventRecorder EventRecorder, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IHostCommunicator HostCommunicator)
		{
			Set(EventRecorder);
			Set(VehicleInfoManager);
			Set(MissionStateManager);
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
			rEventRecorder.RecordVehicleInfo(DatabaseDataOperation.Add, Args.Item);
			rEventRecorder.CreateTableOfHistoryVehicleInfo(Args.ItemName);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			rEventRecorder.RecordVehicleInfo(DatabaseDataOperation.Remove, Args.Item);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			rEventRecorder.RecordVehicleInfo(DatabaseDataOperation.Update, Args.Item);
		}
		private void HandleEvent_MissionStateManagerItemAdded(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			rEventRecorder.RecordMissionState(DatabaseDataOperation.Add, Args.Item);
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			rEventRecorder.RecordMissionState(DatabaseDataOperation.Update, Args.Item);
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(object Sender, ListenStateChangedEventArgs Args)
		{
			rEventRecorder.RecordHistoryHostCommunication(DatabaseDataOperation.Add, Args.OccurTime, "LocalListenStateChanged", Args.Port.ToString(), $"IsListened: {Args.IsListened.ToString()}");
		}
		private void HandleEvent_HostCommunicatorRemoteConnectStateChanged(object Sender, ConnectStateChangedEventArgs Args)
		{
			rEventRecorder.RecordHistoryHostCommunication(DatabaseDataOperation.Add, Args.OccurTime, "RemoteConnectStateChanged", Args.IpPort, $"IsConnected: {Args.IsConnected.ToString()}");
		}
		private void HandleEvent_HostCommunicatorSentData(object Sender, SentDataEventArgs Args)
		{
			rEventRecorder.RecordHistoryHostCommunication(DatabaseDataOperation.Add, Args.OccurTime, "SentData", Args.IpPort, $"Data: {Args.Data}");
		}
		private void HandleEvent_HostCommunicatorReceivedData(object Sender, ReceivedDataEventArgs Args)
		{
			rEventRecorder.RecordHistoryHostCommunication(DatabaseDataOperation.Add, Args.OccurTime, "RecievedData", Args.IpPort, $"Data: {Args.Data}");
		}
		private void Subtask_RecordVehicleInfo()
		{
			List<IVehicleInfo> tmpDatas = rVehicleInfoManager.GetItems().ToList();
			DateTime tmpTimestamp = DateTime.Now;
			for (int i = 0; i < tmpDatas.Count; ++i)
			{
				rEventRecorder.RecordHistoryVehicleInfo(DatabaseDataOperation.Add, tmpTimestamp.AddMilliseconds(i), tmpDatas[i]);
			}
		}
	}
}
