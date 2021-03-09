using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.NewCommunication;

namespace VehicleSimulator.New
{
	public class HostMessageHandler : IHostMessageHandler
	{
		private IHostCommunicator rHostCommunicator = null;
		private ISimulatorControl rSimulatorControl = null;

		public HostMessageHandler(IHostCommunicator IHostCommunicator, ISimulatorControl ISimulatorControl)
		{
			Set(IHostCommunicator, ISimulatorControl);
		}
		public void Set(IHostCommunicator IHostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = IHostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}
		public void Set(ISimulatorControl ISimulatorControl)
		{
			rSimulatorControl = ISimulatorControl;
		}
		public void Set(IHostCommunicator IHostCommunicator, ISimulatorControl ISimulatorControl)
		{
			Set(IHostCommunicator);
			Set(ISimulatorControl);
		}

		private void SubscribeEvent_IHostCommunicator(IHostCommunicator IHostCommunicator)
		{
			if (IHostCommunicator != null)
			{
				IHostCommunicator.ReceivedData += HandleEvent_IHostCommunicatorReceivedData;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator IHostCommunicator)
		{
			if (IHostCommunicator != null)
			{
				IHostCommunicator.ReceivedData -= HandleEvent_IHostCommunicatorReceivedData;
			}
		}
		private void HandleEvent_IHostCommunicatorReceivedData(object sender, ReceivedDataEventArgs e)
		{
			if (e.Data is PauseMoving)
			{
				PauseMoving msg = e.Data as PauseMoving;
				msg.Response = false;
				HandleSerializableData_PauseMoving(msg);
				rHostCommunicator.SendData(msg);
			}
			else if (e.Data is ResumeMoving)
			{
				ResumeMoving msg = e.Data as ResumeMoving;
				msg.Response = false;
				HandleSerializableData_ResumeMoving(msg);
				rHostCommunicator.SendData(msg);
			}
			else if (e.Data is GoTo)
			{
				GoTo msg = e.Data as GoTo;
				msg.Response = false;
				HandleSerializableData_GoTo(msg);
				rHostCommunicator.SendData(msg);
			}
			else if (e.Data is GoToPoint)
			{
				GoToPoint msg = e.Data as GoToPoint;
				msg.Response = false;
				HandleSerializableData_GoToPoint(msg);
				rHostCommunicator.SendData(msg);
			}
			else if (e.Data is GoToTowardPoint)
			{
				GoToTowardPoint msg = e.Data as GoToTowardPoint;
				msg.Response = false;
				HandleSerializableData_GoToTowardPoint(msg);
				rHostCommunicator.SendData(msg);
			}
			else if (e.Data is Stop)
			{
				Stop msg = e.Data as Stop;
				msg.Response = false;
				HandleSerializableData_Stop(msg);
				rHostCommunicator.SendData(msg);
			}
		}
		private void HandleSerializableData_PauseMoving(PauseMoving PauseMoving)
		{
			if (rSimulatorControl.mIsExecuting)
			{
				rSimulatorControl.PauseMove();
			}
		}
		private void HandleSerializableData_ResumeMoving(ResumeMoving ResumeMoving)
		{
			if (rSimulatorControl.mIsExecuting)
			{
				rSimulatorControl.ResumeMove();
			}
		}
		private void HandleSerializableData_GoTo(GoTo GoTo)
		{
			if (!rSimulatorControl.mIsExecuting)
			{
				Random random = new Random();
				rSimulatorControl.StartMove(GoTo.Require, random.Next(-10000,10000), random.Next(-10000, 10000));
			}
		}
		private void HandleSerializableData_GoToPoint(GoToPoint GoToPoint)
		{
			if (!rSimulatorControl.mIsExecuting)
			{
				rSimulatorControl.StartMove(GoToPoint.Require[0], GoToPoint.Require[1]);
			}
		}
		private void HandleSerializableData_GoToTowardPoint(GoToTowardPoint GoToTowardPoint)
		{
			if (!rSimulatorControl.mIsExecuting)
			{
				rSimulatorControl.StartMove(GoToTowardPoint.Require[0], GoToTowardPoint.Require[1], GoToTowardPoint.Require[2]);
			}
		}
		private void HandleSerializableData_Stop(Stop msg)
		{
			if (rSimulatorControl.mIsExecuting)
			{
				rSimulatorControl.StopMove();
			}
		}
	}
}
