using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Implement
{
	public abstract class SystemWithConfig : ISystemWithConfig
	{
		public event EventHandlerConfigUpdated ConfigUpdated;

		public abstract string GetConfig(string ConfigName);
		public abstract void SetConfig(string ConfigName, string NewValue);

		protected virtual void RaiseEvent_ConfigUpdated(string ConfigName, string NewValue, bool Sync = true)
		{
			if (Sync)
			{
				ConfigUpdated?.Invoke(DateTime.Now, ConfigName, NewValue);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ConfigUpdated?.Invoke(DateTime.Now, ConfigName, NewValue); });
			}
		}
	}
}
