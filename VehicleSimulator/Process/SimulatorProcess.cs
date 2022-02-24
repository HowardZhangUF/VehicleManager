using LibraryForVM;
using SerialData;
using System;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class SimulatorProcess
	{
		public event EventHandler<DebugMessageEventArgs> DebugMessage;

		private ISimulatorInfo mSimulatorInfo = null;
		private ISimulatorControl mSimulatorControl = null;
		private IHostCommunicator mHostCommunicator = null;
		private IMoveRequestCalculator mMoveRequestCalculator = null;
		private IHostMessageHandler mHostMessageHandler = null;
		private ISimulatorInfoReporter mSimulatorInfoReporter = null;

		public SimulatorProcess(string SimulatorName)
		{
			Constructor(SimulatorName);
		}
		~SimulatorProcess()
		{
			Destructor();
		}
		public void Start()
		{
			mSimulatorControl.SetConfig("TimePeriod", "50");
			mHostCommunicator.SetConfig("TimePeriod", "100");
			mHostCommunicator.SetConfig("RemoteIpPort", "127.0.0.1:8000");
			mSimulatorInfoReporter.SetConfig("TimePeriod", "100");

			mHostCommunicator.Start();
			mHostCommunicator.Connect();
			mSimulatorInfoReporter.Start();
		}
		public void Stop()
		{
			mSimulatorInfoReporter.Stop();
			mHostCommunicator.Stop();
			if (mHostCommunicator.mIsConnected) mHostCommunicator.Disconnect();
		}
		public void SetMap(string MapFilePath)
		{
			mMoveRequestCalculator.SetMap(MapFilePath);
			mSimulatorInfo.SetMapFilePath(MapFilePath);
		}
		public ISimulatorInfo GetReferenceOfISimulatorInfo()
		{
			return mSimulatorInfo;
		}
		public ISimulatorControl GetReferenceOfISimulatorControl()
		{
			return mSimulatorControl;
		}
		public IHostCommunicator GetReferenceOfIHostCommunicator()
		{
			return mHostCommunicator;
		}

		protected virtual void RaiseEvent_DebugMessage(string OccurTime, string Category, string SubCategory, string Message, bool Sync = true)
		{
			if (Sync)
			{
				DebugMessage?.Invoke(this, new DebugMessageEventArgs(OccurTime, Category, SubCategory, Message));
			}
			else
			{
				Task.Run(() => { DebugMessage?.Invoke(this, new DebugMessageEventArgs(OccurTime, Category, SubCategory, Message)); });
			}
		}

		private void Constructor(string SimulatorName)
		{
			UnsubscribeEvent(mSimulatorInfo);
			mSimulatorInfo = new SimulatorInfo(SimulatorName);
			SubscribeEvent(mSimulatorInfo);

			// 給予模擬器隨意初始位置
			Random random = new Random();
			mSimulatorInfo.SetLocation(random.Next(-10000, 10000), random.Next(-10000, 10000), random.Next(359));

			UnsubscribeEvent(mSimulatorControl);
			mSimulatorControl = new SimulatorControl(mSimulatorInfo);
			SubscribeEvent(mSimulatorControl);

			UnsubscribeEvent(mHostCommunicator);
			mHostCommunicator = new HostCommunicator();
			SubscribeEvent(mHostCommunicator);

			UnsubscribeEvent(mMoveRequestCalculator);
			mMoveRequestCalculator = new MoveRequestCalculator();
			SubscribeEvent(mMoveRequestCalculator);

			UnsubscribeEvent(mHostMessageHandler);
			mHostMessageHandler = new HostMessageHandler(mSimulatorInfo, mHostCommunicator, mSimulatorControl, mMoveRequestCalculator);
			SubscribeEvent(mHostMessageHandler);

			UnsubscribeEvent(mSimulatorInfoReporter);
			mSimulatorInfoReporter = new SimulatorInfoReporter(mSimulatorInfo, mHostCommunicator);
			SubscribeEvent(mSimulatorInfoReporter);
		}
		private void Destructor()
		{
			UnsubscribeEvent(mSimulatorInfoReporter);
			mSimulatorInfoReporter = null;

			UnsubscribeEvent(mHostMessageHandler);
			mHostMessageHandler = null;

			UnsubscribeEvent(mHostCommunicator);
			mHostCommunicator = null;

			UnsubscribeEvent(mSimulatorControl);
			mSimulatorControl = null;

			UnsubscribeEvent(mSimulatorInfo);
			mSimulatorInfo = null;
		}
		private void SubscribeEvent(ISimulatorInfo ISimulatorInfo)
		{
			if (ISimulatorInfo != null)
			{
				ISimulatorInfo.StatusUpdated += HandleEvent_ISimulatorInfoStatusUpdated;
			}
		}
		private void UnsubscribeEvent(ISimulatorInfo ISimulatorInfo)
		{
			if (ISimulatorInfo != null)
			{
				ISimulatorInfo.StatusUpdated -= HandleEvent_ISimulatorInfoStatusUpdated;
			}
		}
		private void SubscribeEvent(ISimulatorControl ISimulatorControl)
		{
			if (ISimulatorControl != null)
			{
				ISimulatorControl.SystemStatusChanged += HandleEvent_ISimulatorControlSystemStatusChanged;
				ISimulatorControl.ConfigUpdated += HandleEvent_ISimulatorControlConfigUpdated;
			}
		}
		private void UnsubscribeEvent(ISimulatorControl ISimulatorControl)
		{
			if (ISimulatorControl != null)
			{
				ISimulatorControl.SystemStatusChanged -= HandleEvent_ISimulatorControlSystemStatusChanged;
				ISimulatorControl.ConfigUpdated -= HandleEvent_ISimulatorControlConfigUpdated;
			}
		}
		private void SubscribeEvent(IHostCommunicator IHostCommunicator)
		{
			if (IHostCommunicator != null)
			{
				IHostCommunicator.SystemStatusChanged += HandleEvent_IHostCommunicatorSystemStatusChanged;
				IHostCommunicator.ConfigUpdated += HandleEvent_IHostCommunicatorConfigUpdated;
				IHostCommunicator.ConnectStateChanged += HandleEvent_IHostCommunicatorConnectStateChanged;
				IHostCommunicator.SentData += HandleEvent_IHostCommunicatorSentData;
				IHostCommunicator.ReceivedData += HandleEvent_IHostCommunicatorReceivedData;
			}
		}
		private void UnsubscribeEvent(IHostCommunicator IHostCommunicator)
		{
			if (IHostCommunicator != null)
			{
				IHostCommunicator.SystemStatusChanged -= HandleEvent_IHostCommunicatorSystemStatusChanged;
				IHostCommunicator.ConfigUpdated -= HandleEvent_IHostCommunicatorConfigUpdated;
				IHostCommunicator.ConnectStateChanged -= HandleEvent_IHostCommunicatorConnectStateChanged;
				IHostCommunicator.SentData -= HandleEvent_IHostCommunicatorSentData;
				IHostCommunicator.ReceivedData -= HandleEvent_IHostCommunicatorReceivedData;
			}
		}
		private void SubscribeEvent(IMoveRequestCalculator IMoveRequestCalculator)
		{
			if (IMoveRequestCalculator != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent(IMoveRequestCalculator IMoveRequestCalculator)
		{
			if (IMoveRequestCalculator != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent(IHostMessageHandler IHostMessageHandler)
		{
			if (IHostMessageHandler != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent(IHostMessageHandler IHostMessageHandler)
		{
			if (IHostMessageHandler != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent(ISimulatorInfoReporter ISimulatorInfoReporter)
		{
			if (ISimulatorInfoReporter != null)
			{
				ISimulatorInfoReporter.SystemStatusChanged += HandleEvent_ISimulatorInfoReporterSystemStatusChanged;
				ISimulatorInfoReporter.ConfigUpdated += HandleEvent_ISimulatorInfoReporterConfigUpdated;
			}
		}
		private void UnsubscribeEvent(ISimulatorInfoReporter ISimulatorInfoReporter)
		{
			if (ISimulatorInfoReporter != null)
			{
				ISimulatorInfoReporter.SystemStatusChanged -= HandleEvent_ISimulatorInfoReporterSystemStatusChanged;
				ISimulatorInfoReporter.ConfigUpdated -= HandleEvent_ISimulatorInfoReporterConfigUpdated;
			}
		}
		private void HandleEvent_ISimulatorInfoStatusUpdated(object sender, StatusUpdatedEventArgs e)
		{
			if (e.StatusName.Contains("X") || e.StatusName.Contains("Y") || e.StatusName.Contains("Toward")) return;
			HandleDebugMessage(e.OccurTime, "ISimulatorInfo", "StatusUpdated", $"Name: {e.ItemName}, StatusName:{e.StatusName}");
		}
		private void HandleEvent_ISimulatorControlSystemStatusChanged(object sender, SystemStatusChangedEventArgs e)
		{
			HandleDebugMessage(e.OccurTime, "ISimulatorControl", "SystemStatusChanged", $"SystemStatus: {e.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_ISimulatorControlConfigUpdated(object sender, ConfigUpdatedEventArgs e)
		{
			HandleDebugMessage(e.OccurTime, "ISimulatorControl", "ConfigUpdated", $"ConfigName: {e.ConfigName}, ConfigNewValue: {e.ConfigNewValue}");
		}
		private void HandleEvent_IHostCommunicatorSystemStatusChanged(object sender, SystemStatusChangedEventArgs e)
		{
			HandleDebugMessage(e.OccurTime, "IHostCommunicator", "SystemStatusChanged", $"SystemStatus: {e.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_IHostCommunicatorConfigUpdated(object sender, ConfigUpdatedEventArgs e)
		{
			HandleDebugMessage(e.OccurTime, "IHostCommunicator", "ConfigUpdated", $"ConfigName: {e.ConfigName}, ConfigNewValue: {e.ConfigNewValue}");
		}
		private void HandleEvent_IHostCommunicatorConnectStateChanged(object sender, ConnectStateChangedEventArgs e)
		{
			HandleDebugMessage(e.OccurTime, "IHostCommunicator", "ConnectStateChanged", $"IPPort: {e.IpPort}, IsConnected: {e.IsConnected.ToString()}");
		}
		private void HandleEvent_IHostCommunicatorSentData(object sender, SentDataEventArgs e)
		{
			if (e.Data is AGVStatus || e.Data is AGVPath) return;
			HandleDebugMessage(e.OccurTime, "IHostCommunicator", "SentData", $"IPPort: {e.IpPort}, DataType: {e.Data.ToString()}");
		}
		private void HandleEvent_IHostCommunicatorReceivedData(object sender, ReceivedDataEventArgs e)
		{
			HandleDebugMessage(e.OccurTime, "IHostCommunicator", "ReceivedData", $"IPPort: {e.IpPort}, DataType: {e.Data.ToString()}");
		}
		private void HandleEvent_ISimulatorInfoReporterSystemStatusChanged(object sender, SystemStatusChangedEventArgs e)
		{
			HandleDebugMessage(e.OccurTime, "ISimulatorInfoReporter", "SystemStatusChanged", $"SystemStatus: {e.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_ISimulatorInfoReporterConfigUpdated(object sender, ConfigUpdatedEventArgs e)
		{
			HandleDebugMessage(e.OccurTime, "ISimulatorInfoReporter", "ConfigUpdated", $"ConfigName: {e.ConfigName}, ConfigNewValue: {e.ConfigNewValue}");
		}
		private void HandleDebugMessage(string Message)
		{
			HandleDebugMessage("None", Message);
		}
		private void HandleDebugMessage(string Category, string Message)
		{
			HandleDebugMessage(DateTime.Now, Category, Message);
		}
		private void HandleDebugMessage(DateTime OccurTime, string Category, string Message)
		{
			HandleDebugMessage(OccurTime, Category, "None", Message);
		}
		private void HandleDebugMessage(DateTime OccurTime, string Category, string SubCategory, string Message)
		{
			HandleDebugMessage(OccurTime.ToString("yyyy/MM/dd HH:mm:ss.fff"), Category, SubCategory, Message);
		}
		private void HandleDebugMessage(string OccurTime, string Category, string SubCategory, string Message)
		{
			Console.WriteLine($"{OccurTime} [{Category}] [{SubCategory}] - {Message}");
			RaiseEvent_DebugMessage(OccurTime, Category, SubCategory, Message);
		}
	}

	public class DebugMessageEventArgs : EventArgs
	{
		public string OccurTime { get; private set; }
		public string Category { get; private set; }
		public string SubCategory { get; private set; }
		public string Message { get; private set; }

		public DebugMessageEventArgs(string OccurTime, string Category, string SubCategory, string Message) : base()
		{
			this.OccurTime = OccurTime;
			this.Category = Category;
			this.SubCategory = SubCategory;
			this.Message = Message;
		}
	}
}
