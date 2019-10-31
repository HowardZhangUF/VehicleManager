using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Implement;

namespace TrafficControlTest.Implement
{
	public class VehicleInfoManager : ItemManager<IVehicleInfo>, IVehicleInfoManager
	{
		public IVehicleInfo this[string Name] { get { return GetItem(Name); } }

		public VehicleInfoManager()
		{
		}
		public bool IsExistByIpPort(string IpPort)
		{
			return mItems.Values.Any(o => o.mIpPort == IpPort);
		}
		public IVehicleInfo GetItemByIpPort(string IpPort)
		{
			return (IsExistByIpPort(IpPort) ? mItems.Values.First((o) => o.mIpPort == IpPort) : null);
		}
		public void UpdateItem(string Name, string State, IPoint2D Position, double Toward, double Battery, double Velocity, string Target, string AlarmMessage, bool IsInterveneAvailable, bool IsBeingIntervened, string InterveneCommand)
		{
			if (IsExist(Name))
			{
				mItems[Name].Update(State, Position, Toward, Battery, Velocity, Target, AlarmMessage, IsInterveneAvailable, IsBeingIntervened, InterveneCommand);
			}
		}
		public void UpdateItem(string Name, IEnumerable<IPoint2D> Path)
		{
			if (IsExist(Name))
			{
				mItems[Name].Update(Path);
			}
		}
		public void UpdateItem(string Name, string IpPort)
		{
			if (IsExist(Name))
			{
				mItems[Name].Update(IpPort);
			}
		}
	}
}
