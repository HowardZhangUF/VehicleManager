using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General
{
	public interface ISystemWithConfig
	{
		event EventHandlerConfigUpdated ConfigUpdated;

		string GetConfig(string ConfigName);
		void SetConfig(string ConfigName, string NewValue);
	}
}
