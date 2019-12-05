﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Module.MissionManager.Implement
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
		private void HandleEvent_MissionStateManagerItemAdded(DateTime OccurTime, string Name, IMissionState Item)
		{
			rHostCommunicator.SendString($"Event=MissionCreated MissionID={Name}");
		}
		private void HandleEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IMissionState Item)
		{
			if (StateName.Contains("ExecuteState"))
			{
				string msg = null;
				switch (Item.mExecuteState)
				{
					case ExecuteState.Executing:
						msg = $"Event=MissionStarted MissionID={Name}";
						break;
					case ExecuteState.ExecuteSuccessed:
						msg = $"Event=MissionCompleted Result=Successed MissionID={Name}";
						break;
					case ExecuteState.ExecuteFailed:
						msg = $"Event=MissionCompleted Result=Failed MissionID={Name}";
						break;
				}
				if (!string.IsNullOrEmpty(msg))
				{
					rHostCommunicator.SendString(msg);
				}
			}
		}
	}
}