using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorControlHandler : SystemWithLoopTask, IAutomaticDoorControlHandler
	{
		private IAutomaticDoorControlManager rAutomaticDoorControlManager = null;
		private IAutomaticDoorInfoManager rAutomaticDoorInfoManager = null;
		private IAutomaticDoorCommunicator rAutomaticDoorCommunicator = null;

		public AutomaticDoorControlHandler(IAutomaticDoorControlManager AutomaticDoorControlManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator)
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
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			result += $"CountOfAutomaticDoorControl: {rAutomaticDoorControlManager.mCount}";
			if (rAutomaticDoorControlManager.mCount > 0)
			{
				result += ", ";
				result += string.Join(", ", rAutomaticDoorControlManager.GetItems().Select(o => $"{o.mAutomaticDoorName}-{o.mCommand.ToString()}-{o.mSendState.ToString()}"));
			}
			return result;
		}
		public override void Task()
		{
			Subtask_HandleAutomaticDoorControls();
		}
		
		private void SubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				// do nothing
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
		private void Subtask_HandleAutomaticDoorControls()
		{
			HandleIAutomaticDoorControls(rAutomaticDoorControlManager.GetItems());
		}
		private void HandleIAutomaticDoorControls(IEnumerable<IAutomaticDoorControl> AutomaticDoorControls)
		{
			if (AutomaticDoorControls != null && AutomaticDoorControls.Count() > 0)
			{
				foreach (IAutomaticDoorControl automaticDoorControl in AutomaticDoorControls)
				{
					HandleIAutomaticDoorControls(automaticDoorControl);
				}
			}
		}
		private void HandleIAutomaticDoorControls(IAutomaticDoorControl AutomaticDoorControl)
		{
			if (rAutomaticDoorInfoManager.IsExist(AutomaticDoorControl.mAutomaticDoorName))
			{
				IAutomaticDoorInfo automaticDoorInfo = rAutomaticDoorInfoManager.GetItem(AutomaticDoorControl.mAutomaticDoorName);
				if (AutomaticDoorControl.mSendState == AutomaticDoorControlCommandSendState.Unsend && automaticDoorInfo.mIsConnected)
				{
					AutomaticDoorControl.UpdateSendState(AutomaticDoorControlCommandSendState.Sending);
					switch (AutomaticDoorControl.mCommand)
					{
						case AutomaticDoorControlCommand.Open:
							rAutomaticDoorCommunicator.SendData(automaticDoorInfo.mIpPort, $"Command=OpenDoor ID={AutomaticDoorControl.mAutomaticDoorName}");
							break;
						case AutomaticDoorControlCommand.Close:
							rAutomaticDoorCommunicator.SendData(automaticDoorInfo.mIpPort, $"Command=CloseDoor ID={AutomaticDoorControl.mAutomaticDoorName}");
							break;
					}
				}
			}
		}
	}
}
