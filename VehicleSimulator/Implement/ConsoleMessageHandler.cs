using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using VehicleSimulator.Interface;

namespace VehicleSimulator.Implement
{
	class ConsoleMessageHandler : IConsoleMessageHandler
	{
		private IVehicleSimulatorInfo rVehicleSimulatorInfo = null;
		private ICommunicatorClient rCommunicatorClient = null;

		public ConsoleMessageHandler(IVehicleSimulatorInfo VehicleSimulatorInfo, ICommunicatorClient CommunicatorClient)
		{
			Set(VehicleSimulatorInfo, CommunicatorClient);
		}
		public void Set(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			UnsubscribeEvent_IVehicleSimulatorInfo(rVehicleSimulatorInfo);
			rVehicleSimulatorInfo = VehicleSimulatorInfo;
			SubscribeEvent_IVehicleSimulatorInfo(rVehicleSimulatorInfo);
		}
		public void Set(ICommunicatorClient CommunicatorClient)
		{
			UnsubscribeEvent_ICommunicatorClient(rCommunicatorClient);
			rCommunicatorClient = CommunicatorClient;
			SubscribeEvent_ICommunicatorClient(rCommunicatorClient);
		}
		public void Set(IVehicleSimulatorInfo VehicleSimulatorInfo, ICommunicatorClient CommunicatorClient)
		{
			Set(VehicleSimulatorInfo);
			Set(CommunicatorClient);
		}

		private void SubscribeEvent_IVehicleSimulatorInfo(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			if (VehicleSimulatorInfo != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehicleSimulatorInfo(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			if (VehicleSimulatorInfo != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_ICommunicatorClient(ICommunicatorClient CommunicatorClient)
		{
			if (CommunicatorClient != null)
			{
				CommunicatorClient.ReceivedSerializableData += HandleEvent_CommunicatorClientReceivedSerializableData;
			}
		}
		private void UnsubscribeEvent_ICommunicatorClient(ICommunicatorClient CommunicatorClient)
		{
			if (CommunicatorClient != null)
			{
				CommunicatorClient.ReceivedSerializableData += HandleEvent_CommunicatorClientReceivedSerializableData;
			}
		}
		private void HandleEvent_CommunicatorClientReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			if (Data is InsertMovingBuffer)
			{
				InsertMovingBuffer msg = Data as InsertMovingBuffer;
				msg.Response = false;
				HandleSerializableData_Intervene(msg);
				rCommunicatorClient.SendSerializableData(msg);
			}
			else if (Data is RemoveMovingBuffer)
			{
				RemoveMovingBuffer msg = Data as RemoveMovingBuffer;
				msg.Response = false;
				HandleSerializableData_Intervene(msg);
				rCommunicatorClient.SendSerializableData(msg);
			}
			else if (Data is PauseMoving)
			{
				PauseMoving msg = Data as PauseMoving;
				msg.Response = false;
				HandleSerializableData_Intervene(msg);
				rCommunicatorClient.SendSerializableData(msg);
			}
			else if (Data is ResumeMoving)
			{
				ResumeMoving msg = Data as ResumeMoving;
				msg.Response = false;
				HandleSerializableData_Intervene(msg);
				rCommunicatorClient.SendSerializableData(msg);
			}
		}
		private void HandleSerializableData_Intervene(InsertMovingBuffer Data)
		{
			if (Data != null && rVehicleSimulatorInfo != null && rVehicleSimulatorInfo.mIsInterveneAvailable)
			{
				if (Data.Require.Contains(","))
				{
					string[] coordinate = Data.Require.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
					if (coordinate.Length == 2)
					{
						Data.Response = true;
						rVehicleSimulatorInfo.SetInterveneCommand("InsertMovingBuffer", coordinate[0], coordinate[1]);
					}
				}
			}
		}
		private void HandleSerializableData_Intervene(RemoveMovingBuffer Data)
		{
			if (Data != null && rVehicleSimulatorInfo != null && rVehicleSimulatorInfo.mIsInterveneAvailable)
			{
				if (Data.Require == null)
				{
					if (rVehicleSimulatorInfo.mBufferTarget != null)
					{
						Data.Response = true;
						rVehicleSimulatorInfo.SetInterveneCommand("RemoveMovingBuffer");
					}
				}
			}
		}
		private void HandleSerializableData_Intervene(PauseMoving Data)
		{
			if (Data != null && rVehicleSimulatorInfo != null && rVehicleSimulatorInfo.mIsInterveneAvailable)
			{
				if (Data.Require == null)
				{
					Data.Response = true;
					rVehicleSimulatorInfo.SetInterveneCommand("PauseMoving");
				}
			}
		}
		private void HandleSerializableData_Intervene(ResumeMoving Data)
		{
			if (Data != null && rVehicleSimulatorInfo != null && rVehicleSimulatorInfo.mIsInterveneAvailable && rVehicleSimulatorInfo.mIsBeingIntervened && rVehicleSimulatorInfo.mInterveneCommand == "Pause")
			{
				if (Data.Require == null)
				{
					Data.Response = true;
					rVehicleSimulatorInfo.SetInterveneCommand("ResumeMoving");
				}
			}
		}
	}
}
