using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Mission;
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
				HostCommunicator.SentString += HandleEvent_HostCommunicatorSentString;
				HostCommunicator.ReceivedString += HandleEvent_HostCommunicatorReceivedString;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.LocalListenStateChanged -= HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.RemoteConnectStateChanged -= HandleEvent_HostCommunicatorRemoteConnectStateChanged;
				HostCommunicator.SentString -= HandleEvent_HostCommunicatorSentString;
				HostCommunicator.ReceivedString -= HandleEvent_HostCommunicatorReceivedString;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			rEventRecorder.RecordVehicleInfo(DatabaseDataOperation.Add, Item);
			rEventRecorder.CreateTableOfHistoryVehicleInfo(Name);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			rEventRecorder.RecordVehicleInfo(DatabaseDataOperation.Remove, Item);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo Item)
		{
			rEventRecorder.RecordVehicleInfo(DatabaseDataOperation.Update, Item);
		}
		private void HandleEvent_MissionStateManagerItemAdded(DateTime OccurTime, string Name, IMissionState Item)
		{
			rEventRecorder.RecordMissionState(DatabaseDataOperation.Add, Item);
		}
		private void HandleEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IMissionState Item)
		{
			rEventRecorder.RecordMissionState(DatabaseDataOperation.Update, Item);
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port)
		{
			rEventRecorder.RecordHistoryHostCommunication(DatabaseDataOperation.Add, OccurTime, "LocalListenStateChanged", Port.ToString(), $"State: {NewState.ToString()}");
		}
		private void HandleEvent_HostCommunicatorRemoteConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			rEventRecorder.RecordHistoryHostCommunication(DatabaseDataOperation.Add, OccurTime, "RemoteConnectStateChanged", IpPort, $"State: {NewState}");
		}
		private void HandleEvent_HostCommunicatorSentString(DateTime OccurTime, string IpPort, string Data)
		{
			rEventRecorder.RecordHistoryHostCommunication(DatabaseDataOperation.Add, OccurTime, "SentData", IpPort, $"Data: {Data}");
		}
		private void HandleEvent_HostCommunicatorReceivedString(DateTime OccurTime, string IpPort, string Data)
		{
			rEventRecorder.RecordHistoryHostCommunication(DatabaseDataOperation.Add, OccurTime, "RecievedData", IpPort, $"Data: {Data}");
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
