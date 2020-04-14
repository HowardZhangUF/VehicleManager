using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Interface
{
	public interface ISystemWithConfig
	{
		event EventHandlerConfigUpdated ConfigUpdated;

		string GetConfig(string ConfigName);
		void SetConfig(string ConfigName, string NewValue);
	}
}
