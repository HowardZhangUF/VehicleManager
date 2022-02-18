using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.VehiclePassThroughAutomaticDoor
{
	public class VehiclePassThroughAutomaticDoorEventHandler : IVehiclePassThroughAutomaticDoorEventHandler
	{
		private IVehiclePassThroughAutomaticDoorEventManager rVehiclePassThroughAutomaticDoorEventManager = null;
		private IAutomaticDoorControlManager rAutomaticDoorControlManager = null;

		public VehiclePassThroughAutomaticDoorEventHandler(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager, IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			Set(VehiclePassThroughAutomaticDoorEventManager, AutomaticDoorControlManager);
		}
		public void Set(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(rVehiclePassThroughAutomaticDoorEventManager);
			rVehiclePassThroughAutomaticDoorEventManager = VehiclePassThroughAutomaticDoorEventManager;
			SubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(rVehiclePassThroughAutomaticDoorEventManager);
		}
		public void Set(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			UnsubscribeEvent_IAutomaticDoorControlManager(rAutomaticDoorControlManager);
			rAutomaticDoorControlManager = AutomaticDoorControlManager;
			SubscribeEvent_IAutomaticDoorControlManager(rAutomaticDoorControlManager);
		}
		public void Set(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager, IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			Set(VehiclePassThroughAutomaticDoorEventManager);
			Set(AutomaticDoorControlManager);
		}
		
		private void SubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			if (VehiclePassThroughAutomaticDoorEventManager != null)
			{
				VehiclePassThroughAutomaticDoorEventManager.ItemAdded += HandleEvent_IVehiclePassThroughAutomaticDoorEventManagerItemAdded;
				VehiclePassThroughAutomaticDoorEventManager.ItemRemoved += HandleEvent_IVehiclePassThroughAutomaticDoorEventManagerItemRemoved;
			}
		}
		private void UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			if (VehiclePassThroughAutomaticDoorEventManager != null)
			{
				VehiclePassThroughAutomaticDoorEventManager.ItemAdded -= HandleEvent_IVehiclePassThroughAutomaticDoorEventManagerItemAdded;
				VehiclePassThroughAutomaticDoorEventManager.ItemRemoved -= HandleEvent_IVehiclePassThroughAutomaticDoorEventManagerItemRemoved;
			}
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
		private void HandleEvent_IVehiclePassThroughAutomaticDoorEventManagerItemAdded(object sender, ItemCountChangedEventArgs<IVehiclePassThroughAutomaticDoorEvent> e)
		{
			string automaticDoorName = e.Item.mAutomaticDoorName;
			IAutomaticDoorControl control = Library.Library.GenerateIAutomaticDoorControl(automaticDoorName, AutomaticDoorControlCommand.Open, e.ItemName);
			rAutomaticDoorControlManager.Add(control.mName, control);
		}
		private void HandleEvent_IVehiclePassThroughAutomaticDoorEventManagerItemRemoved(object sender, ItemCountChangedEventArgs<IVehiclePassThroughAutomaticDoorEvent> e)
		{
			string automaticDoorName = e.Item.mAutomaticDoorName;
			// 如果該自動門已經沒有其他車要通過
			if (!rVehiclePassThroughAutomaticDoorEventManager.GetItems().Any(o => o.mAutomaticDoorName == automaticDoorName))
			{
				IAutomaticDoorControl control = Library.Library.GenerateIAutomaticDoorControl(automaticDoorName, AutomaticDoorControlCommand.Close, e.ItemName);
				rAutomaticDoorControlManager.Add(control.mName, control);
			}
		}
	}
}
