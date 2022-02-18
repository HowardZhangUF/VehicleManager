using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorCommunicatorUpdater : IAutomaticDoorCommunicatorUpdater
	{
		private IAutomaticDoorCommunicator rAutomaticDoorCommunicator = null;
		private IAutomaticDoorInfoManager rAutomaticDoorInfoManager = null;

		public AutomaticDoorCommunicatorUpdater(IAutomaticDoorCommunicator AutomaticDoorCommunicator, IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			Set(AutomaticDoorCommunicator, AutomaticDoorInfoManager);
		}
		public void Set(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			UnsubscribeEvent_IAutomaticDoorCommunicator(rAutomaticDoorCommunicator);
			rAutomaticDoorCommunicator = AutomaticDoorCommunicator;
			SubscribeEvent_IAutomaticDoorCommunicator(rAutomaticDoorCommunicator);
		}
		public void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			UnsubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
			rAutomaticDoorInfoManager = AutomaticDoorInfoManager;
			SubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
		}
		public void Set(IAutomaticDoorCommunicator AutomaticDoorCommunicator, IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			Set(AutomaticDoorCommunicator);
			Set(AutomaticDoorInfoManager);
		}

		private void SubscribeEvent_IAutomaticDoorCommunicator(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			if (AutomaticDoorCommunicator != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorCommunicator(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			if (AutomaticDoorCommunicator != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				AutomaticDoorInfoManager.ItemAdded += HandleEvent_AutomaticDoorInfoManagerItemAdded;
				AutomaticDoorInfoManager.ItemRemoved += HandleEvent_AutomaticDoorInfoManagerItemRemoved;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				AutomaticDoorInfoManager.ItemAdded -= HandleEvent_AutomaticDoorInfoManagerItemAdded;
				AutomaticDoorInfoManager.ItemRemoved -= HandleEvent_AutomaticDoorInfoManagerItemRemoved;
			}
		}
		private void HandleEvent_AutomaticDoorInfoManagerItemAdded(object sender, ItemCountChangedEventArgs<IAutomaticDoorInfo> e)
		{
			List<string> currentClientList = rAutomaticDoorCommunicator.GetClientList();
			if (currentClientList == null || currentClientList.Count == 0 || !currentClientList.Contains(e.Item.mIpPort))
			{
				if (!string.IsNullOrEmpty(e.Item.mIpPort))
				{
					rAutomaticDoorCommunicator.Add(e.Item.mIpPort);
				}
			}
		}
		private void HandleEvent_AutomaticDoorInfoManagerItemRemoved(object sender, ItemCountChangedEventArgs<IAutomaticDoorInfo> e)
		{
			if (rAutomaticDoorCommunicator.GetClientList().Contains(e.Item.mIpPort))
			{
				rAutomaticDoorCommunicator.Remove(e.Item.mIpPort);
			}
		}
	}
}
