using LibraryForVM;
using System;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.InterveneCommand;

namespace TrafficControlTest.Module.Mission
{
	public class MissionStateReporter : IMissionStateReporter
	{
		private IMissionStateManager rMissionStateManager = null;
		private IHostCommunicator rHostCommunicator = null;

		public MissionStateReporter(IMissionStateManager MissionStateManager, IHostCommunicator HostCommunicator)
		{
			Set(MissionStateManager, HostCommunicator);
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
		public void Set(IMissionStateManager MissionStateManager, IHostCommunicator HostCommunicator)
		{
			Set(MissionStateManager);
			Set(HostCommunicator);
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

			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{

			}
		}
		private void HandleEvent_MissionStateManagerItemAdded(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			rHostCommunicator.SendData($"Event=MissionCreated MissionID={Args.Item.GetMissionId()} Mission={Args.Item.mMission.mMissionType.ToString()} Parameter={Args.Item.mMission.mParametersString}");
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			if (Args.StatusName.Contains("ExecuteState"))
			{
				string msg = null;
				switch (Args.Item.mExecuteState)
				{
					case ExecuteState.Executing:
						msg = $"Event=MissionStarted MissionID={Args.Item.GetMissionId()} ExecutorID={Args.Item.mExecutorId}";
						break;
					case ExecuteState.ExecuteSuccessed:
						msg = $"Event=MissionCompleted Result=Successed MissionID={Args.Item.GetMissionId()}";
						break;
					case ExecuteState.ExecuteFailed:
						msg = $"Event=MissionCompleted Result=Failed MissionID={Args.Item.GetMissionId()}";
						break;
				}
				if (!string.IsNullOrEmpty(msg))
				{
					rHostCommunicator.SendData(msg);
				}
			}
		}
	}
}
