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
	public class VehicleInfoManager : IVehicleInfoManager
	{
		public event EventHandlerIVehicleInfo VehicleAdded;
		public event EventHandlerIVehicleInfo VehicleRemoved;
		public event EventHandlerIVehicleInfo VehicleStateUpdated;

		public IVehicleInfo this[string Name] => GetVehicleInfo(Name);

		private Dictionary<string, IVehicleInfo> mVehicleInfos = new Dictionary<string, IVehicleInfo>();

		public VehicleInfoManager()
		{
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
		public void AddVehicleInfo(string IpPort, string Name)
		{
			mVehicleInfos.Add(Name, Library.Library.GenerateIVehicleInfo(Name));
			mVehicleInfos[Name].SetIpPort(IpPort);
			SubscribeEvent_IVehicleInfo(mVehicleInfos[Name]);
			RaiseEvent_VehicleAdded(mVehicleInfos[Name].mName, mVehicleInfos[Name]);
		}
		public void RemoveVehicleInfo(string IpPort)
		{
			string name = mVehicleInfos.First((o) => o.Value.mIpPort == IpPort).Key;
			string ipPort = mVehicleInfos[name].mIpPort;
			IVehicleInfo info = mVehicleInfos[name];
			UnsubscribeEvent_IVehicleInfo(mVehicleInfos[name]);
			mVehicleInfos.Remove(mVehicleInfos.First((o) => o.Value.mIpPort == IpPort).Key);
			RaiseEvent_VehicleRemoved(name, info);
		}

		private void SubscribeEvent_IVehicleInfo(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null)
			{
				VehicleInfo.StateUpdated += HandleEvent_VehicleInfoStateUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfo(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null)
			{
				VehicleInfo.StateUpdated -= HandleEvent_VehicleInfoStateUpdated;
			}
		}
		protected virtual void RaiseEvent_VehicleAdded(string Name, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleAdded?.Invoke(DateTime.Now, Name, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleAdded?.Invoke(DateTime.Now, Name, VehicleInfo); });
			}
		}
		protected virtual void RaiseEvent_VehicleRemoved(string Name, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleRemoved?.Invoke(DateTime.Now, Name, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleRemoved?.Invoke(DateTime.Now, Name, VehicleInfo); });
			}
		}
		protected virtual void RaiseEvent_VehicleStateUpdated(string Name, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleStateUpdated?.Invoke(DateTime.Now, Name, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleStateUpdated?.Invoke(DateTime.Now, Name, VehicleInfo); });
			}
		}
		private void HandleEvent_VehicleInfoStateUpdated(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			RaiseEvent_VehicleStateUpdated(Name, VehicleInfo);
		}
	}
}
