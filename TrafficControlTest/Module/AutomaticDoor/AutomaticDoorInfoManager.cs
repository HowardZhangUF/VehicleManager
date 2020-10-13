using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorInfoManager : ItemManager<IAutomaticDoorInfo>, IAutomaticDoorInfoManager
	{
		public bool IsExisByIpPortt(string IpPort)
		{
			return mItems.Values.Any(o => o.mIpPort == IpPort);
		}
		public IAutomaticDoorInfo GetItemByIpPort(string IpPort)
		{
			return (IsExisByIpPortt(IpPort) ? mItems.Values.First((o) => o.mIpPort == IpPort) : null);
		}
	}
}
