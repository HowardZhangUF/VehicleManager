using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.InterveneManager.Interface;

namespace TrafficControlTest.Module.InterveneManager.Implement
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
				VehicleCommunicator.SentSerializableDataSuccessed += HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
				VehicleCommunicator.SentSerializableDataFailed += HandleEvent_VehicleCommunicatorSentSerializableDataFailed;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SentSerializableDataSuccessed -= HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
				VehicleCommunicator.SentSerializableDataFailed -= HandleEvent_VehicleCommunicatorSentSerializableDataFailed;
			}
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleControl Item)
		{
			if (StateName.Contains("SendState"))
			{
				if (Item.mSendState == SendState.SentSuccessed || Item.mSendState == SendState.SentFailed)
				{
					rVehicleControlManager.Remove(Name);
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data)
		{
			if (Data is Serializable)
			{
				string vehicleId = rVehicleInfoManager.GetItemByIpPort(IpPort).mName;
				if (rVehicleControlManager.GetItems().Any(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending))
				{
					rVehicleControlManager.GetItems().First(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending).UpdateSendState(SendState.SentSuccessed);
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data)
		{
			if (Data is Serializable)
			{
				string vehicleId = rVehicleInfoManager.GetItemByIpPort(IpPort).mName;
				if (rVehicleControlManager.GetItems().Any(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending))
				{
					rVehicleControlManager.GetItems().First(o => o.mVehicleId == vehicleId && o.mSendState == SendState.Sending).UpdateSendState(SendState.SentFailed);
				}
			}
		}
	}
}
