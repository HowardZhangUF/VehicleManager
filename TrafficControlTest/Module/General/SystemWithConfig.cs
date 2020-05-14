using System;

namespace TrafficControlTest.Module.General
{
	public abstract class SystemWithConfig : ISystemWithConfig
	{
		public event EventHandler<ConfigUpdatedEventArgs> ConfigUpdated;

		public abstract string GetConfig(string ConfigName);
		public abstract void SetConfig(string ConfigName, string NewValue);

		protected virtual void RaiseEvent_ConfigUpdated(string ConfigName, string ConfigNewValue, bool Sync = true)
		{
			if (Sync)
			{
				ConfigUpdated?.Invoke(this, new ConfigUpdatedEventArgs(DateTime.Now, ConfigName, ConfigNewValue));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ConfigUpdated?.Invoke(this, new ConfigUpdatedEventArgs(DateTime.Now, ConfigName, ConfigNewValue)); });
			}
		}
	}
}
