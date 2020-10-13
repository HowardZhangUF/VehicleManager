using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public interface IAutomaticDoorInfoManager : IItemManager<IAutomaticDoorInfo>
	{
		bool IsExisByIpPortt(string IpPort);
		IAutomaticDoorInfo GetItemByIpPort(string IpPort);
	}
}
