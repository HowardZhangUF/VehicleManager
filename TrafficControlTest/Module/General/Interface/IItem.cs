using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Interface
{
	public interface IItem
	{
		event EventHandlerIItemUpdated Updated;

		string mName { get; }
	}
}
