using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.NewCommunication;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorControlManagerUpdater : IAutomaticDoorControlManagerUpdater
	{
		private IAutomaticDoorControlManager rAutomaticDoorControlManager = null;
		private IAutomaticDoorInfoManager rAutomaticDoorInfoManager = null;
		private IAutomaticDoorCommunicator rAutomaticDoorCommunicator = null;

		public AutomaticDoorControlManagerUpdater(IAutomaticDoorControlManager AutomaticDoorControlManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			Set(AutomaticDoorControlManager, AutomaticDoorInfoManager, AutomaticDoorCommunicator);
		}
		public void Set(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			UnsubscribeEvent_IAutomaticDoorControlManager(rAutomaticDoorControlManager);
			rAutomaticDoorControlManager = AutomaticDoorControlManager;
			SubscribeEvent_IAutomaticDoorControlManager(rAutomaticDoorControlManager);
		}
		public void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			UnsubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
			rAutomaticDoorInfoManager = AutomaticDoorInfoManager;
			SubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
		}
		public void Set(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			UnsubscribeEvent_IAutomaticDoorCommunicator(rAutomaticDoorCommunicator);
			rAutomaticDoorCommunicator = AutomaticDoorCommunicator;
			SubscribeEvent_IAutomaticDoorCommunicator(rAutomaticDoorCommunicator);
		}
		public void Set(IAutomaticDoorControlManager AutomaticDoorControlManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			Set(AutomaticDoorControlManager);
			Set(AutomaticDoorInfoManager);
			Set(AutomaticDoorCommunicator);
		}
		
		private void SubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				AutomaticDoorControlManager.ItemUpdated += HandleEvent_AutomaticDoorControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				AutomaticDoorControlManager.ItemUpdated -= HandleEvent_AutomaticDoorControlManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IAutomaticDoorCommunicator(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			if (AutomaticDoorCommunicator != null)
			{
				AutomaticDoorCommunicator.SentData += HandleEvent_AutomaticDoorCommunicatorSentData;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorCommunicator(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			if (AutomaticDoorCommunicator != null)
			{
				AutomaticDoorCommunicator.SentData -= HandleEvent_AutomaticDoorCommunicatorSentData;
			}
		}
		private void HandleEvent_AutomaticDoorControlManagerItemUpdated(object sender, ItemUpdatedEventArgs<IAutomaticDoorControl> e)
		{
			if (e.StatusName.Contains("SendState"))
			{
				if (e.Item.mSendState == AutomaticDoorControlCommandSendState.SentSuccessed || e.Item.mSendState == AutomaticDoorControlCommandSendState.SentFailed)
				{
					rAutomaticDoorControlManager.Remove(e.ItemName);
				}
			}
		}
		private void HandleEvent_AutomaticDoorCommunicatorSentData(object sender, SentDataEventArgs e)
		{
			if (rAutomaticDoorInfoManager.IsExisByIpPortt(e.IpPort))
			{
				IAutomaticDoorInfo tmpDoorInfo = rAutomaticDoorInfoManager.GetItemByIpPort(e.IpPort);
				if (rAutomaticDoorControlManager.GetItems().Any(o => o.mAutomaticDoorName == tmpDoorInfo.mName && o.mSendState == AutomaticDoorControlCommandSendState.Sending))
				{
					rAutomaticDoorControlManager.GetItems().First(o => o.mAutomaticDoorName == tmpDoorInfo.mName && o.mSendState == AutomaticDoorControlCommandSendState.Sending).UpdateSendState(AutomaticDoorControlCommandSendState.SentSuccessed);
				}
			}
		}
	}
}
