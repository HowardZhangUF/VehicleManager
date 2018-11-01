using AsyncSocket;
using KdTree;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	class VehicleSimulatorProcess
	{
		public VehicleSimulatorProcess()
		{
			SubscribeConsoleCommunicatorEvent();
		}

		~VehicleSimulatorProcess()
		{
			UnsubscribeConsoleCommunicatorEvent();
		}

		public delegate void DebugMessageEventHandler(DateTime timeStamp, string category, string message);
		public event DebugMessageEventHandler DebugMessage;

		#region 與主控台的通訊

		ConsoleCommunicator ConsoleCommunicator = new ConsoleCommunicator();

		public bool DisplayConsoleCommunicatorDebugMessage = true;

		public void StartCommunication()
		{
			ConsoleCommunicator.Start("127.0.0.1", 8000);
		}

		public void StopCommunication()
		{
			ConsoleCommunicator.Stop();
		}

		public void SendSerializableData(Serializable data)
		{
			ConsoleCommunicator.SendSerializableData(data);
		}

		private void SubscribeConsoleCommunicatorEvent()
		{
			ConsoleCommunicator.ConnectStatusChanged += ConsoleCommunicator_ConnectStatusChanged;
			ConsoleCommunicator.ReceivedRequestMapListData += ConsoleCommunicator_ReceivedRequestMapListData;
			ConsoleCommunicator.ReceivedGetMapData += ConsoleCommunicator_ReceivedGetMapData;
			ConsoleCommunicator.ReceivedUploadMapToAGVData += ConsoleCommunicator_ReceivedUploadMapToAGVData;
			ConsoleCommunicator.ReceivedChangeMapData += ConsoleCommunicator_ReceivedChangeMapData;
		}

		private void UnsubscribeConsoleCommunicatorEvent()
		{
			ConsoleCommunicator.ConnectStatusChanged -= ConsoleCommunicator_ConnectStatusChanged;
			ConsoleCommunicator.ReceivedRequestMapListData -= ConsoleCommunicator_ReceivedRequestMapListData;
			ConsoleCommunicator.ReceivedGetMapData -= ConsoleCommunicator_ReceivedGetMapData;
			ConsoleCommunicator.ReceivedUploadMapToAGVData -= ConsoleCommunicator_ReceivedUploadMapToAGVData;
			ConsoleCommunicator.ReceivedChangeMapData -= ConsoleCommunicator_ReceivedChangeMapData;
		}

		private void ConsoleCommunicator_ConnectStatusChanged(DateTime occurTime, EndPointInfo remoteInfo, EConnectStatus newStatus)
		{
			if (DisplayConsoleCommunicatorDebugMessage)
			{
				string message = $"Connect Status Changed. IP: {remoteInfo.ToString()} New Status: {newStatus.ToString()}.";
				DebugMessage?.Invoke(occurTime, "Console Communicator", message);
			}
		}

		private void ConsoleCommunicator_ReceivedRequestMapListData(DateTime receivedTime, EndPointInfo remoteInfo, RequestMapList data)
		{
			if (DisplayConsoleCommunicatorDebugMessage)
			{
				string message = $"Received RequestMapList Command. IP: {remoteInfo.ToString()}.";
				DebugMessage?.Invoke(receivedTime, "Console Communicator", message);
			}
		}

		private void ConsoleCommunicator_ReceivedGetMapData(DateTime receivedTime, EndPointInfo remoteInfo, GetMap data)
		{
			if (DisplayConsoleCommunicatorDebugMessage)
			{
				string message = $"Received GetMap Command. IP: {remoteInfo.ToString()}.";
				DebugMessage?.Invoke(receivedTime, "Console Communicator", message);
			}
		}

		private void ConsoleCommunicator_ReceivedUploadMapToAGVData(DateTime receivedTime, EndPointInfo remoteInfo, UploadMapToAGV data)
		{
			if (DisplayConsoleCommunicatorDebugMessage)
			{
				string message = $"Received UploadMapToAGV Command. IP: {remoteInfo.ToString()}.";
				DebugMessage?.Invoke(receivedTime, "Console Communicator", message);
			}
		}

		private void ConsoleCommunicator_ReceivedChangeMapData(DateTime receivedTime, EndPointInfo remoteInfo, ChangeMap data)
		{
			if (DisplayConsoleCommunicatorDebugMessage)
			{
				string message = $"Received UploadMapToAGV Command. IP: {remoteInfo.ToString()}.";
				DebugMessage?.Invoke(receivedTime, "Console Communicator", message);
			}
		}

		#endregion
	}
}
