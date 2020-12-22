using System;

namespace TrafficControlTest.Module.General
{
	public interface ISystemWithConfig
	{
		event EventHandler<ConfigUpdatedEventArgs> ConfigUpdated;

		string[] GetConfigNameList();
		string GetConfig(string ConfigName);
		void SetConfig(string ConfigName, string NewValue);
	}

	public class ConfigUpdatedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string ConfigName { get; private set; }
		public string ConfigNewValue { get; private set; }

		public ConfigUpdatedEventArgs(DateTime OccurTime, string ConfigName, string ConfigNewValue) : base()
		{
			this.OccurTime = OccurTime;
			this.ConfigName = ConfigName;
			this.ConfigNewValue = ConfigNewValue;
		}
	}
}
