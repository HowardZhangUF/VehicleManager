using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorControlManager : ItemManager<IAutomaticDoorControl>, IAutomaticDoorControlManager
	{
		public void UpdateSendState(string Name, AutomaticDoorControlCommandSendState SendState)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateSendState(SendState);
			}
		}
	}
}
