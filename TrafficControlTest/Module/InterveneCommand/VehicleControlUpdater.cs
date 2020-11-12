using Serialization;
using System;
using System.Linq;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.NewCommunication;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.InterveneCommand
{
	public class VehicleControlUpdater : IVehicleControlUpdater
	{
		private IVehicleControlManager rVehicleControlManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;

		public VehicleControlUpdater(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			Set(VehicleControlManager, VehicleInfoManager, VehicleCommunicator);
		}
		public void Set(IVehicleControlManager VehicleControlManager)
		{
			UnsubscribeEvent_IVehicleControlManager(rVehicleControlManager);
			rVehicleControlManager = VehicleControlManager;
			SubscribeEvent_IVehicleControlManager(rVehicleControlManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			rVehicleInfoManager = VehicleInfoManager;
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVechielCommunicator(rVehicleCommunicator);
		}
		public void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			Set(VehicleControlManager);
			Set(VehicleInfoManager);
			Set(VehicleCommunicator);
		}

		private void SubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemUpdated += HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemUpdated -= HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVechielCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SentDataSuccessed += HandleEvent_VehicleCommunicatorSentDataSuccessed;
				VehicleCommunicator.SentDataFailed += HandleEvent_VehicleCommunicatorSentDataFailed;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SentDataSuccessed -= HandleEvent_VehicleCommunicatorSentDataSuccessed;
				VehicleCommunicator.SentDataFailed -= HandleEvent_VehicleCommunicatorSentDataFailed;
			}
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleControl> Args)
		{
			if (Args.StatusName.Contains("SendState"))
			{
				if (Args.Item.mSendState == SendState.SentSuccessed || Args.Item.mSendState == SendState.SentFailed)
				{
					rVehicleControlManager.Remove(Args.ItemName);
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorSentDataSuccessed(object Sender, SentDataEventArgs Args)
		{
			if (Args.Data is Serializable)
			{
				string vehicleId = rVehicleInfoManager.GetItemByIpPort(Args.IpPort).mName;
				if (rVehicleControlManager.GetItems().Any(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending))
				{
					rVehicleControlManager.GetItems().First(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending).UpdateSendState(SendState.SentSuccessed);
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorSentDataFailed(object Sender, SentDataEventArgs Args)
		{
			if (Args.Data is Serializable && rVehicleInfoManager.IsExistByIpPort(Args.IpPort))
			{
				string vehicleId = rVehicleInfoManager.GetItemByIpPort(Args.IpPort).mName;
				if (rVehicleControlManager.GetItems().Any(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending))
				{
					rVehicleControlManager.GetItems().First(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending).UpdateSendState(SendState.SentFailed);
				}
			}
		}
	}
}
