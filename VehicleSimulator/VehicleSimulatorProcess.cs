using AsyncSocket;
using Geometry;
using KdTree;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

		public event VehicleSimulator.PositionChangedEventHandler VehicleSimulatorPositionChanged;

		private Thread ReportThread;

		/// <summary>定時傳送 AGV 狀態、路徑至主控台的功能</summary>
		private void ReportTask()
		{
			try
			{
				while (true)
				{
					if (ConsoleCommunicator.IsAlive)
					{
						if (VehicleSimulators != null && VehicleSimulators.Count() > 0)
						{
							foreach (VehicleSimulator vehicleSimulator in VehicleSimulators.Values)
							{
								ConsoleCommunicator.SendSerializableData(vehicleSimulator.GetAGVStatus());
								Thread.Sleep(50);
								ConsoleCommunicator.SendSerializableData(vehicleSimulator.GetAGVPath());
								Thread.Sleep(50);
							}
						}
					}
					Thread.Sleep(500);
				}
			}
			catch (Exception ex)
			{
				DebugMessage?.Invoke(DateTime.Now, "Exception", ex.ToString());
			}
		}

		/// <summary>定時傳送 AGV 狀態、路徑至主控台的功能開啟</summary>
		private void ReportStart()
		{
			ReportThread = new Thread(ReportTask);
			ReportThread.IsBackground = true;
			ReportThread.Name = "ReportThread";
			ReportThread.Start();
		}

		/// <summary>定時傳送 AGV 狀態、路徑至主控台的功能停止</summary>
		private void ReportStop()
		{
			if (ReportThread != null)
			{
				if (ReportThread.IsAlive)
				{
					ReportThread.Abort();
				}
				ReportThread = null;
			}
		}

		/// <summary>將字串轉換成路徑。字串格式應為 (0,0),(1000,2000),(-2000,1000),(3000,-2000),(-4000,-1000)</summary>
		public List<Pair> ConvertStringToPath(string src)
		{
			List<Pair> result = null;
			if (src != string.Empty)
			{
				string[] split = src.Split(new string[] { "),(", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
				if (split.Count() >= 2)
				{
					result = new List<Pair>();
					for (int i = 0; i < split.Count(); ++i)
					{
						string[] position = split[i].Split(',');
						if (position.Count() == 2)
						{
							int x = 0, y = 0;
							if (int.TryParse(position[0], out x) && int.TryParse(position[1], out y))
							{
								result.Add(new Pair(x, y));
							}
						}
					}
				}
			}
			return result;
		}

		#region 車模擬器

		private Dictionary<string, VehicleSimulator> VehicleSimulators = new Dictionary<string, VehicleSimulator>();

		public bool DisplayVehicleSimulatorDebugMessage = true;

		public bool AddVehicleSimualtor(string name, double translationSpeed, double rotationSpeed)
		{
			bool result = false;
			if (!VehicleSimulators.Keys.Contains(name))
			{
				VehicleSimulators.Add(name, new VehicleSimulator(name, translationSpeed, rotationSpeed));
				SubscribeVehicleSimulatorEvent(VehicleSimulators[name]);
				result = true;
			}
			return result;
		}

		public bool RemoveVehicleSimulator(string name)
		{
			bool result = false;
			if (VehicleSimulators.Keys.Contains(name))
			{
				UnsubscribeVehicleSimulatorEvent(VehicleSimulators[name]);
				VehicleSimulators.Remove(name);
				result = true;
			}
			return result;
		}

		public AGVStatus GetVehicleSimulatorAGVStatus(string name)
		{
			AGVStatus result = null;
			if (VehicleSimulators.Keys.Contains(name))
			{
				result = VehicleSimulators[name].GetAGVStatus();
			}
			return result;
		}

		public AGVPath GetVehicleSimulatorAGVPath(string name)
		{
			AGVPath result = null;
			if (VehicleSimulators.Keys.Contains(name))
			{
				result = VehicleSimulators[name].GetAGVPath();
			}
			return result;
		}

		public string GetVehicleStatus(string name)
		{
			string result = "";
			if (VehicleSimulators.Keys.Contains(name))
			{
				result = VehicleSimulators[name].Status;
			}
			return result;
		}

		public void VehicleSimulatorCycleMove(string name, List<Pair> path, int cycleTimes)
		{
			if (VehicleSimulators.Keys.Contains(name))
			{
				if (VehicleSimulators[name].Status == "Stopped")
				{
					List<Pair> newPath = new List<Pair>();
					for (int i = 0; i < cycleTimes; ++i)
					{
						newPath.AddRange(path);
					}
					VehicleSimulators[name].Move(newPath);
				}
			}
		}

		public void VehicleSimulatorMove(string name, List<Pair> path)
		{
			if (VehicleSimulators.Keys.Contains(name))
			{
				if (VehicleSimulators[name].Status == "Stopped")
				{
					VehicleSimulators[name].Move(path);
				}
			}
		}

		public void VehicleSimulatorPause(string name)
		{
			if (VehicleSimulators.Keys.Contains(name))
			{
				if (VehicleSimulators[name].Status == "Moving")
				{
					VehicleSimulators[name].PauseMoving();
				}
			}
		}

		public void VehicleSimulatorResume(string name)
		{
			if (VehicleSimulators.Keys.Contains(name))
			{
				if (VehicleSimulators[name].Status == "Paused")
				{
					VehicleSimulators[name].ResumeMoving();
				}
			}
		}

		public void VehicleSimulatorStop(string name)
		{
			if (VehicleSimulators.Keys.Contains(name))
			{
				if (VehicleSimulators[name].Status == "Moving" || VehicleSimulators[name].Status == "Paused")
				{
					VehicleSimulators[name].StopMoving();
				}
			}
		}

		private void SubscribeVehicleSimulatorEvent(VehicleSimulator vehicleSimulator)
		{
			vehicleSimulator.StatusChanged += VehicleSimulator_StatusChanged;
			vehicleSimulator.PathChanged += VehicleSimulator_PathChanged;
			vehicleSimulator.PositionChanged += VehicleSimulator_PositionChanged;
		}

		private void UnsubscribeVehicleSimulatorEvent(VehicleSimulator vehicleSimulator)
		{
			vehicleSimulator.StatusChanged -= VehicleSimulator_StatusChanged;
			vehicleSimulator.PathChanged -= VehicleSimulator_PathChanged;
			vehicleSimulator.PositionChanged -= VehicleSimulator_PositionChanged;
		}

		private void VehicleSimulator_StatusChanged(string name, string status)
		{
			if (DisplayVehicleSimulatorDebugMessage)
			{
				string message = $"Vehicle: {name} Status Changed! New Status: {status}";
				DebugMessage?.Invoke(DateTime.Now, "Vehicle Simulator", message);
			}
		}

		private void VehicleSimulator_PathChanged(string name, List<Pair> path)
		{
			if (DisplayVehicleSimulatorDebugMessage)
			{
				string message = $"Vehicle: {name} Path Changed! New Path Length: {path.Count()}";
				DebugMessage?.Invoke(DateTime.Now, "Vehicle Simulator", message);
			}
		}

		private void VehicleSimulator_PositionChanged(string name, TowardPair position)
		{
			if (DisplayVehicleSimulatorDebugMessage)
			{
				string message = $"Vehicle: {name} Position Changed! New Position: ({position.Position.X}, {position.Position.Y}) {position.Toward.Theta.ToString("F2")}";
				DebugMessage?.Invoke(DateTime.Now, "Vehicle Simulator", message);
			}

			VehicleSimulatorPositionChanged?.Invoke(name, position);
		}

		#endregion

		#region 與主控台的通訊

		private ConsoleCommunicator ConsoleCommunicator = new ConsoleCommunicator();

		public bool DisplayConsoleCommunicatorDebugMessage = true;

		public void StartCommunication(string ip, int port)
		{
			ConsoleCommunicator.Start(ip, port);
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

			if (newStatus == EConnectStatus.Connect)
			{
				ReportStart();
			}
			else
			{
				ReportStop();
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
