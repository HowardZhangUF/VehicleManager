using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	/// <summary>儲存所有的預計對自動門執行的控制</summary>
	public interface IAutomaticDoorControlManager : IItemManager<IAutomaticDoorControl>
	{
		/// <summary>更新指定資料</summary>
		void UpdateSendState(string Name, AutomaticDoorControlCommandSendState SendState);
	}
}
