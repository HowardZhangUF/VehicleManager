using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public interface IAutomaticDoorInfoManager : IItemManager<IAutomaticDoorInfo>
	{
		bool IsExisByIpPortt(string IpPort);
		IAutomaticDoorInfo GetItemByIpPort(string IpPort);
	}
}
