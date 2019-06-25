using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleInfoManager;

namespace TrafficControlTest.Implement
{
	class VehicleInfoManager : IVehicleInfoManager
	{
		public event EventHandlerVehicleInfo VehicleAdded;
		public event EventHandlerVehicleInfo VehicleRemoved;
		public event EventHandlerVehicleInfo VehicleStateUpdated;

		public IVehicleInfo this[string Name] => GetVehicleInfo(Name);

		private IVehicleCommunicator rVehicleCommunicator = null;
		private Dictionary<string, IVehicleInfo> mVehicleInfos = new Dictionary<string, IVehicleInfo>();

		public VehicleInfoManager(IVehicleCommunicator VehicleCommunicator)
		{
			SetVehicleCommunicator(VehicleCommunicator);
		}
		public void SetVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public bool IsVehicleExist(string Name)
		{
			return mVehicleInfos.Keys.Contains(Name);
		}
		public bool IsVehicleExistByIpPort(string IpPort)
		{
			return mVehicleInfos.Any((o) => o.Value.mIpPort == IpPort);
		}
		public IVehicleInfo GetVehicleInfo(string Name)
		{
			if (IsVehicleExist(Name))
			{
				return mVehicleInfos[Name];
			}
			else
			{
				return null;
			}
		}
		public IVehicleInfo GetVehicleInfoByIpPort(string IpPort)
		{
			if (IsVehicleExistByIpPort(IpPort))
			{
				return mVehicleInfos.First((o) => o.Value.mIpPort == IpPort).Value;
			}
			else
			{
				return null;
			}
		}
		public List<string> GetVehicleNameList()
		{
			if (mVehicleInfos.Count > 0)
			{
				return mVehicleInfos.Keys.ToList();
			}
			else
			{
				return null;
			}
		}
		public List<IVehicleInfo> GetVehicleInfoList()
		{
			if (mVehicleInfos.Count > 0)
			{
				return mVehicleInfos.Values.ToList();
			}
			else
			{
				return null;
			}
		}

		private void AddVehicleInfo(string IpPort, string Name)
		{
			mVehicleInfos.Add(Name, Library.Library.GenerateIVehicleInfo(Name));
			mVehicleInfos[Name].mIpPort = IpPort;
			SubscribeEvent_IVehicleInfo(mVehicleInfos[Name]);
			RaiseEvent_VehicleAdded(mVehicleInfos[Name].mName, mVehicleInfos[Name].mIpPort, mVehicleInfos[Name]);
		}
		private void RemoveVehicleInfo(string IpPort)
		{
			string name = mVehicleInfos.First((o) => o.Value.mIpPort == IpPort).Key;
			string ipPort = mVehicleInfos[name].mIpPort;
			IVehicleInfo info = mVehicleInfos[name];
			UnsubscribeEvent_IVehicleInfo(mVehicleInfos[name]);
			mVehicleInfos.Remove(mVehicleInfos.First((o) => o.Value.mIpPort == IpPort).Key);
			RaiseEvent_VehicleRemoved(name, ipPort, info);
		}
		private void SubscribeEvent_IVehicleInfo(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null)
			{
				VehicleInfo.VehicleStateUpdated += HandleEvent_VehicleInfoVehicleStateUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfo(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null)
			{
				VehicleInfo.VehicleStateUpdated -= HandleEvent_VehicleInfoVehicleStateUpdated;
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.VehicleConnectStateChanged += HandleEvent_VehicleCommunicatorVehicleConnectStateChanged;
				VehicleCommunicator.ReceivedSerializableData += HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.VehicleConnectStateChanged -= HandleEvent_VehicleCommunicatorVehicleConnectStateChanged;
				VehicleCommunicator.ReceivedSerializableData -= HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		protected virtual void RaiseEvent_VehicleAdded(string Name, string IpPort, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleAdded?.Invoke(DateTime.Now, Name, IpPort, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleAdded?.Invoke(DateTime.Now, Name, IpPort, VehicleInfo); });
			}
		}
		protected virtual void RaiseEvent_VehicleRemoved(string Name, string IpPort, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleRemoved?.Invoke(DateTime.Now, Name, IpPort, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleRemoved?.Invoke(DateTime.Now, Name, IpPort, VehicleInfo); });
			}
		}
		protected virtual void RaiseEvent_VehicleStateUpdated(string Name, string IpPort, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleStateUpdated?.Invoke(DateTime.Now, Name, IpPort, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleStateUpdated?.Invoke(DateTime.Now, Name, IpPort, VehicleInfo); });
			}
		}
		private void HandleEvent_VehicleInfoVehicleStateUpdated(DateTime OccurTime, string Name, string IpPort, IVehicleInfo VehicleInfo)
		{
			RaiseEvent_VehicleStateUpdated(Name, IpPort, VehicleInfo);
		}
		private void HandleEvent_VehicleCommunicatorVehicleConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			if (NewState == ConnectState.Disconnected)
			{
				if (IsVehicleExistByIpPort(IpPort))
				{
					RemoveVehicleInfo(IpPort);
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
			if (!IsVehicleExist(AgvStatus.Name))
			{
				AddVehicleInfo(IpPort, AgvStatus.Name);
			}

			mVehicleInfos[AgvStatus.Name].mAlarmMessage = AgvStatus.AlarmMessage;
			mVehicleInfos[AgvStatus.Name].mBattery = AgvStatus.Battery;
			mVehicleInfos[AgvStatus.Name].mState = AgvStatus.Description.ToString();
			mVehicleInfos[AgvStatus.Name].mToward = AgvStatus.Toward;
			mVehicleInfos[AgvStatus.Name].mVelocity = AgvStatus.Velocity;
			mVehicleInfos[AgvStatus.Name].mPosition = Library.Library.GenerateIPoint2D((int)AgvStatus.X, (int)AgvStatus.Y);
			mVehicleInfos[AgvStatus.Name].mIpPort = IpPort;
		}
		private void UpdateIVehicleInfo(string IpPort, AGVPath AgvPath)
		{
			if (!IsVehicleExist(AgvPath.Name))
			{
				AddVehicleInfo(IpPort, AgvPath.Name);
			}

			mVehicleInfos[AgvPath.Name].mPath = ConvertToPoints(AgvPath.PathX, AgvPath.PathY);
			mVehicleInfos[AgvPath.Name].mIpPort = IpPort;
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
