using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Interface
{
	/// <summary>
	/// - 儲存物件的識別碼
	/// - 物件的資訊更新時會拋出事件
	/// </summary>
	public interface IItem
	{
		event EventHandlerIItemUpdated Updated;

		string mName { get; }
	}
}
