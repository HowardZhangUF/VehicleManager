using LibraryForVM;
using SerialData;

namespace VehicleSimulator
{
	public class HostMessageHandler : IHostMessageHandler
	{
		private ISimulatorInfo rSimulatorInfo = null;
		private IHostCommunicator rHostCommunicator = null;
		private ISimulatorControl rSimulatorControl = null;
		private IMoveRequestCalculator rMoveRequestCalculator = null;

		public HostMessageHandler(ISimulatorInfo ISimulatorInfo, IHostCommunicator IHostCommunicator, ISimulatorControl ISimulatorControl, IMoveRequestCalculator IMoveRequestCalculator)
		{
			Set(ISimulatorInfo, IHostCommunicator, ISimulatorControl, IMoveRequestCalculator);
		}
		public void Set(ISimulatorInfo ISimulatorInfo)
		{
			rSimulatorInfo = ISimulatorInfo;
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
		public void Set(IMoveRequestCalculator IMoveRequestCalculator)
		{
			rMoveRequestCalculator = IMoveRequestCalculator;
		}
		public void Set(ISimulatorInfo ISimulatorInfo, IHostCommunicator IHostCommunicator, ISimulatorControl ISimulatorControl, IMoveRequestCalculator IMoveRequestCalculator)
		{
			Set(ISimulatorInfo);
			Set(IHostCommunicator);
			Set(ISimulatorControl);
			Set(IMoveRequestCalculator);
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
			else if (e.Data is RequestMapList)
			{
				RequestMapList msg = e.Data as RequestMapList;
				if (!string.IsNullOrEmpty(rSimulatorInfo.mMapFilePath))
				{
					msg.Response = new System.Collections.Generic.List<string>() { System.IO.Path.GetFileNameWithoutExtension(rSimulatorInfo.mMapFilePath) + "*" };
				}
				else
				{
					msg.Response = new System.Collections.Generic.List<string>();
				}
				rHostCommunicator.SendData(msg);
			}
			else if (e.Data is GetMap)
			{
				GetMap msg = e.Data as GetMap;
				if (!string.IsNullOrEmpty(rSimulatorInfo.mMapFilePath))
				{
					msg.Response = new FileInfo(rSimulatorInfo.mMapFilePath);
				}
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
				var moveRequests = rMoveRequestCalculator.Calculate(new Point(rSimulatorInfo.mX, rSimulatorInfo.mY), GoTo.Require);
				rSimulatorControl.StartMove(GoTo.Require, moveRequests);
			}
		}
		private void HandleSerializableData_GoToPoint(GoToPoint GoToPoint)
		{
			if (!rSimulatorControl.mIsExecuting)
			{
				var moveRequests = rMoveRequestCalculator.Calculate(new Point(rSimulatorInfo.mX, rSimulatorInfo.mY), new Point(GoToPoint.Require[0], GoToPoint.Require[1]));
				rSimulatorControl.StartMove(GoToPoint.Require[0], GoToPoint.Require[1]);
			}
		}
		private void HandleSerializableData_GoToTowardPoint(GoToTowardPoint GoToTowardPoint)
		{
			if (!rSimulatorControl.mIsExecuting)
			{
				var moveRequests = rMoveRequestCalculator.Calculate(new Point(rSimulatorInfo.mX, rSimulatorInfo.mY), new Point(GoToTowardPoint.Require[0], GoToTowardPoint.Require[1]), GoToTowardPoint.Require[2]);
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
