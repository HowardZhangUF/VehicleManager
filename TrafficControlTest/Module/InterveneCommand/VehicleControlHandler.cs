using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.InterveneCommand
{
	public class VehicleControlHandler : SystemWithLoopTask, IVehicleControlHandler
	{
		private IVehicleControlManager rVehicleControlManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;

		public VehicleControlHandler(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
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
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			Set(VehicleControlManager);
			Set(VehicleInfoManager);
			Set(VehicleCommunicator);
		}
		public override void Task()
		{
			Subtask_HandleVehicleControls();
		}

		private void SubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{

			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{

			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{

			}
		}
		private void Subtask_HandleVehicleControls()
		{
			HandleVehicleControls(rVehicleControlManager.GetItems());
		}
		private void HandleVehicleControls(IEnumerable<IVehicleControl> VehicleControls)
		{
			if (VehicleControls != null && VehicleControls.Count() > 0)
			{
				foreach (IVehicleControl vehicleControl in VehicleControls)
				{
					HandleVehicleControl(vehicleControl);
				}
			}
		}
		private void HandleVehicleControl(IVehicleControl VehicleControl)
		{
			if (VehicleControl.mSendState == SendState.Unsend)
			{
				string vehicleId = VehicleControl.mVehicleId;
				string ipPort = rVehicleInfoManager.GetItem(vehicleId).mIpPort;
				string parameter = VehicleControl.mParameters == null ? null : VehicleControl.mParameters[0];
				rVehicleControlManager.UpdateSendState(VehicleControl.mName, SendState.Sending);
				switch (VehicleControl.mCommand)
				{
					case Command.InsertMovingBuffer:
						rVehicleCommunicator.SendSerializableData_InsertMovingBuffer(ipPort, int.Parse(VehicleControl.mParameters[0]), int.Parse(VehicleControl.mParameters[1]));
						break;
					case Command.RemoveMovingBuffer:
						rVehicleCommunicator.SendSerializableData_RemoveMovingBuffer(ipPort);
						break;
					case Command.PauseMoving:
						rVehicleCommunicator.SendSerializableData_PauseMoving(ipPort);
						break;
					case Command.ResumeMoving:
						rVehicleCommunicator.SendSerializableData_ResumeMoving(ipPort);
						break;
					default:
						break;
				}
			}
		}
	}
}
