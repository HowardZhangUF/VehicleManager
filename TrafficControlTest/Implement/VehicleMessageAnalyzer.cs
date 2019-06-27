using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;

namespace TrafficControlTest.Implement
{
	public class VehicleMessageAnalyzer : IVehicleMessageAnalyzer
	{
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;

		public VehicleMessageAnalyzer(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(VehicleCommunicator, VehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			Unsubscribe_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			Subscribe_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			rVehicleInfoManager = VehicleInfoManager;
		}
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
		}

		private void Subscribe_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.RemoteConnectStateChanged += HandleEvent_VehicleCommunicatorRemoteConnectStateChanged;
				VehicleCommunicator.ReceivedSerializableData += HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void Unsubscribe_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.RemoteConnectStateChanged -= HandleEvent_VehicleCommunicatorRemoteConnectStateChanged;
				VehicleCommunicator.ReceivedSerializableData -= HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			if (NewState == ConnectState.Disconnected)
			{
				if (rVehicleInfoManager.IsExistByIpPort(IpPort))
				{
					rVehicleInfoManager.Remove(IpPort);
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			if (Data is Serializable)
			{
				if (Data is AGVStatus)
				{
					UpdateIVehicleInfo(IpPort, Data as AGVStatus);
				}
				else if (Data is AGVPath)
				{
					UpdateIVehicleInfo(IpPort, Data as AGVPath);
				}
			}
			else
			{
				Console.WriteLine("Received Unknown Data.");
			}
		}
		private void UpdateIVehicleInfo(string IpPort, AGVStatus AgvStatus)
		{
			if (!rVehicleInfoManager.IsExist(AgvStatus.Name))
			{
				rVehicleInfoManager.Add(IpPort, AgvStatus.Name);
			}

			rVehicleInfoManager[AgvStatus.Name].Update(IpPort);
			rVehicleInfoManager[AgvStatus.Name].Update(AgvStatus.Description.ToString(), Library.Library.GenerateIPoint2D((int)AgvStatus.X, (int)AgvStatus.Y), AgvStatus.Toward, AgvStatus.Battery, AgvStatus.Velocity, AgvStatus.GoalName, AgvStatus.AlarmMessage);
		}
		private void UpdateIVehicleInfo(string IpPort, AGVPath AgvPath)
		{
			if (!rVehicleInfoManager.IsExist(AgvPath.Name))
			{
				rVehicleInfoManager.Add(IpPort, AgvPath.Name);
			}

			rVehicleInfoManager[AgvPath.Name].Update(IpPort);
			rVehicleInfoManager[AgvPath.Name].Update(ConvertToPoints(AgvPath.PathX, AgvPath.PathY));
		}
		private IEnumerable<IPoint2D> ConvertToPoints(List<double> X, List<double> Y)
		{
			List<IPoint2D> result = new List<IPoint2D>();
			for (int i = 0; i < X.Count; ++i)
			{
				result.Add(Library.Library.GenerateIPoint2D((int)X[i], (int)Y[i]));
			}
			return result;
		}
	}
}
