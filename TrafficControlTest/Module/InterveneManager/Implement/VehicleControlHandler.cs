using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Implement
{
	public class VehicleControlHandler : IVehicleControlHandler
	{
		public event EventHandlerDateTime SystemStarted;
		public event EventHandlerDateTime SystemStopped;

		public bool mIsExcuting { get { return (mThdHandleVehicleControl != null && mThdHandleVehicleControl.IsAlive == true) ? true : false; } }

		private IVehicleControlManager rVehicleControlManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private Thread mThdHandleVehicleControl = null;
		private bool[] mThdHandleVehicleControlExitFlag = null;

		public VehicleControlHandler(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			Set(VehicleControlManager, VehicleInfoManager, VehicleCommunicator);
		}
		public void Set(IVehicleControlManager VehicleControlManager)
		{
			UnsubscribeEvent_IVehicleControlManager(VehicleControlManager);
			rVehicleControlManager = VehicleControlManager;
			SubscribeEvent_IVehicleControlManager(VehicleControlManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(VehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(VehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(VehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(VehicleCommunicator);
		}
		public void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			Set(VehicleControlManager);
			Set(VehicleInfoManager);
			Set(VehicleCommunicator);
		}
		public void Start()
		{
			InitializeThread();
		}
		public void Stop()
		{
			DestroyThread();
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
		protected virtual void RaiseEvent_SystemStarted(bool Sync = true)
		{
			if (Sync)
			{
				SystemStarted?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStarted?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_SystemStopped(bool Sync = true)
		{
			if (Sync)
			{
				SystemStopped?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStopped?.Invoke(DateTime.Now); });
			}
		}
		private void InitializeThread()
		{
			mThdHandleVehicleControlExitFlag = new bool[] { false };
			mThdHandleVehicleControl = new Thread(() => Task_HandleVehicleControl(mThdHandleVehicleControlExitFlag));
			mThdHandleVehicleControl.IsBackground = true;
			mThdHandleVehicleControl.Start();
		}
		private void DestroyThread()
		{
			if (mThdHandleVehicleControl != null)
			{
				if (mThdHandleVehicleControl.IsAlive)
				{
					mThdHandleVehicleControlExitFlag[0] = true;
				}
				mThdHandleVehicleControl = null;
			}
		}
		private void Task_HandleVehicleControl(bool[] ExitFlag)
		{
			try
			{
				RaiseEvent_SystemStarted();
				IEnumerable<IVehicleControl> vehicleControls = null;
				while (!ExitFlag[0])
				{
					vehicleControls = rVehicleControlManager.GetItems();
					HandleVehicleControls(vehicleControls);
					vehicleControls = null;
					Thread.Sleep(300);
				}
			}
			finally
			{

				RaiseEvent_SystemStopped();
			}
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
				switch (VehicleControl.mCommand)
				{
					case Command.InsertMovingBuffer:
						rVehicleCommunicator.SendSerializableData_InsertMovingBuffer(ipPort, parameter);
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
				rVehicleControlManager.UpdateSendState(VehicleControl.mName, SendState.Sending);
			}
		}
	}
}
