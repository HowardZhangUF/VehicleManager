using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Communication;
using TrafficControlTest.Module.General;
using VehicleSimulator.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;
using static TrafficControlTest.Library.Library;
using static VehicleSimulator.Library.EventHandlerLibraryOfIVehicleSimulator;
using static VehicleSimulator.Library.Library;

namespace VehicleSimulator.Base
{
	public class VehicleSimulatorProcess
	{
		public event EventHandlerIVehicleSimulator VehicleSimulatorInfoStateUpdated;
		public event EventHandlerDateTime CommunicatorClientSystemStarted;
		public event EventHandlerDateTime CommunicatorClientSystemStopped;
		public event EventHandlerRemoteConnectState CommunicatorClientConnectStateChanged;
		public event EventHandlerSentSerializableData CommunicatorClientSentSerializableData;
		public event EventHandlerReceivedSerializableData CommunicatorClientReceivedSerializableData;
		public event EventHandlerDateTime VehicleStateReporterSystemStarted;
		public event EventHandlerDateTime VehicleStateReporterSystemStopped;

		private IVehicleSimulatorInfo mVehicleSimulatorInfo = null;
		private ICommunicatorClient mCommunicatorClient = null;
		private IVehicleStateReporter mVehicleStateReporter = null;
		private IConsoleMessageHandler mConsoleMessageHandler = null;

		public VehicleSimulatorProcess()
		{
			Constructor();
		}
		~VehicleSimulatorProcess()
		{
			Destructor();
		}
		public void VehicleSimulatorInfoStartMove(IEnumerable<IPoint2D> Path)
		{
			mVehicleSimulatorInfo.StartMove(Path);
		}
		public void VehicleSimulatorInfoStopMove()
		{
			mVehicleSimulatorInfo.StopMove();
		}
		public void VehicleSimulatorInfoPauseMove()
		{
			mVehicleSimulatorInfo.PauseMove();
		}
		public void VehicleSimulatorInfoResumeMove()
		{
			mVehicleSimulatorInfo.ResumeMove();
		}
		public void VehicleSimulatorInfoSetInterveneCommand(string Command, params string[] Paras)
		{
			mVehicleSimulatorInfo.SetInterveneCommand(Command, Paras);
		}
		public void CommunicatorClientStartConnect(string Ip, int Port)
		{
			if (mCommunicatorClient.mConnectState != ConnectState.Connected)
			{
				mCommunicatorClient.StartConnect(Ip, Port);
			}
		}
		public void CommunicatorClientStopConnect()
		{
			if (mCommunicatorClient.mConnectState == ConnectState.Connected)
			{
				mCommunicatorClient.StopConnect();
			}
		}
		public void VehicleStateReporterStart()
		{
			if (!mVehicleStateReporter.mIsExcuting)
			{
				mVehicleStateReporter.Start();
			}
		}
		public void VehicleStateReporterStop()
		{
			if (mVehicleStateReporter.mIsExcuting)
			{
				mVehicleStateReporter.Stop();
			}
		}
		public IVehicleSimulatorInfo GetVehicleState()
		{
			return mVehicleSimulatorInfo;
		}

		private void Constructor()
		{
			UnsubscribeEvent_IVehicleSimulatorInfo(mVehicleSimulatorInfo);
			mVehicleSimulatorInfo = GenerateIVehicleSimulatorInfo("Vehicle" + DateTime.Now.ToString("fff"));
			SubscribeEvent_IVehicleSimulatorInfo(mVehicleSimulatorInfo);

			UnsubscribeEvent_ICommunicatorClient(mCommunicatorClient);
			mCommunicatorClient = GenerateICommunicatorClient();
			SubscribeEvent_ICommunicatorClient(mCommunicatorClient);

			UnsubscribeEvent_IVehicleStateReporter(mVehicleStateReporter);
			mVehicleStateReporter = GenerateIVehicleStateReporter(mVehicleSimulatorInfo, mCommunicatorClient);
			SubscribeEvent_IVehicleStateReporter(mVehicleStateReporter);

			UnsubscribeEvent_IConsoleMessageHandler(mConsoleMessageHandler);
			mConsoleMessageHandler = GenerateIConsoleMessageHandler(mVehicleSimulatorInfo, mCommunicatorClient);
			SubscribeEvent_IConsoleMessageHandler(mConsoleMessageHandler);
		}
		private void Destructor()
		{
			UnsubscribeEvent_IVehicleSimulatorInfo(mVehicleSimulatorInfo);
			mVehicleSimulatorInfo = null;

			UnsubscribeEvent_ICommunicatorClient(mCommunicatorClient);
			mCommunicatorClient = null;

			UnsubscribeEvent_IVehicleStateReporter(mVehicleStateReporter);
			mVehicleStateReporter = null;
		}
		private void SubscribeEvent_IVehicleSimulatorInfo(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			if (VehicleSimulatorInfo != null)
			{
				VehicleSimulatorInfo.StateUpdated += HandleEvent_VehicleSimulatorInfoStateUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleSimulatorInfo(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			if (VehicleSimulatorInfo != null)
			{
				VehicleSimulatorInfo.StateUpdated -= HandleEvent_VehicleSimulatorInfoStateUpdated;
			}
		}
		private void SubscribeEvent_ICommunicatorClient(ICommunicatorClient CommunicatorClient)
		{
			if (CommunicatorClient != null)
			{
				CommunicatorClient.SystemStarted += HandleEvent_CommunicatorClientSystemStarted;
				CommunicatorClient.SystemStopped += HandleEvent_CommunicatorClientSystemStopped;
				CommunicatorClient.ConnectStateChanged += HandleEvent_CommunicatorClientConnectStateChanged;
				CommunicatorClient.SentSerializableData += HandleEvent_CommunicatorClientSentSerializableData;
				CommunicatorClient.ReceivedSerializableData += HandleEvent_CommunicatorClientReceivedSerializableData;
			}
		}
		private void UnsubscribeEvent_ICommunicatorClient(ICommunicatorClient CommunicatorClient)
		{
			if (CommunicatorClient != null)
			{
				CommunicatorClient.SystemStarted -= HandleEvent_CommunicatorClientSystemStarted;
				CommunicatorClient.SystemStopped -= HandleEvent_CommunicatorClientSystemStopped;
				CommunicatorClient.ConnectStateChanged -= HandleEvent_CommunicatorClientConnectStateChanged;
				CommunicatorClient.SentSerializableData -= HandleEvent_CommunicatorClientSentSerializableData;
				CommunicatorClient.ReceivedSerializableData -= HandleEvent_CommunicatorClientReceivedSerializableData;
			}
		}
		private void SubscribeEvent_IVehicleStateReporter(IVehicleStateReporter VehicleStateReporter)
		{
			if (VehicleStateReporter != null)
			{
				VehicleStateReporter.SystemStarted += HandleEvent_VehicleStateReporterSystemStarted;
				VehicleStateReporter.SystemStopped += HandleEvent_VehicleStateReporterSystemStopped;
			}
		}
		private void UnsubscribeEvent_IVehicleStateReporter(IVehicleStateReporter VehicleStateReporter)
		{
			if (VehicleStateReporter != null)
			{
				VehicleStateReporter.SystemStarted -= HandleEvent_VehicleStateReporterSystemStarted;
				VehicleStateReporter.SystemStopped -= HandleEvent_VehicleStateReporterSystemStopped;
			}
		}
		private void SubscribeEvent_IConsoleMessageHandler(IConsoleMessageHandler mConsoleMessageHandler)
		{
			if (mConsoleMessageHandler != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IConsoleMessageHandler(IConsoleMessageHandler mConsoleMessageHandler)
		{
			if (mConsoleMessageHandler != null)
			{
				// do nothing
			}
		}
		private void HandleEvent_VehicleSimulatorInfoStateUpdated(DateTime OccurTime, string Name, IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			HandleDebugMessage("VehicleSimulatorInfoStateUpdated", $"Vehicle Simulator State Updated. Name: {Name}, Info: {VehicleSimulatorInfo.ToString()}");
			RaiseEvent_VehicleSimulatorInfoStateUpdated(OccurTime, Name, VehicleSimulatorInfo);
		}
		private void HandleEvent_CommunicatorClientSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage("CommunicatorClient", "System Started.");
			RaiseEvent_CommunicatorClientSystemStarted(OccurTime);
		}
		private void HandleEvent_CommunicatorClientSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage("CommunicatorClient", "System Stopped.");
			RaiseEvent_CommunicatorClientSystemStopped(OccurTime);
		}
		private void HandleEvent_CommunicatorClientConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage("CommunicatorClient", $"Connect State Changed. IPPort: {IpPort}, State: {NewState}");
			RaiseEvent_CommunicatorClientConnectStateChanged(OccurTime, IpPort, NewState);
		}
		private void HandleEvent_CommunicatorClientSentSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage("CommunicatorClient", $"Sent Serializable Data. IPPort: {IpPort}, DataType: {Data.GetType().ToString()}");
			RaiseEvent_CommunicatorClientSentSerializableData(OccurTime, IpPort, Data);
		}
		private void HandleEvent_CommunicatorClientReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage("CommunicatorClient", $"Received Serializable Data. IPPort: {IpPort}, DataType: {Data.GetType().ToString()}");
			RaiseEvent_CommunicatorClientReceivedSerializableData(OccurTime, IpPort, Data);
		}
		private void HandleEvent_VehicleStateReporterSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage("VehicleStateReporter", "System Started.");
			RaiseEvent_VehicleStateReporterSystemStarted(OccurTime);
		}
		private void HandleEvent_VehicleStateReporterSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage("VehicleStateReporter", "System Stopped.");
			RaiseEvent_VehicleStateReporterSystemStopped(OccurTime);
		}
		private void RaiseEvent_VehicleSimulatorInfoStateUpdated(DateTime OccurTime, string Name, IVehicleSimulatorInfo VehicleSimulatorInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleSimulatorInfoStateUpdated?.Invoke(OccurTime, Name, VehicleSimulatorInfo);
			}
			else
			{
				Task.Run(() => { VehicleSimulatorInfoStateUpdated?.Invoke(OccurTime, Name, VehicleSimulatorInfo); });
			}
		}
		private void RaiseEvent_CommunicatorClientSystemStarted(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				CommunicatorClientSystemStarted?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { CommunicatorClientSystemStarted?.Invoke(OccurTime); });
			}
		}
		private void RaiseEvent_CommunicatorClientSystemStopped(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				CommunicatorClientSystemStopped?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { CommunicatorClientSystemStopped?.Invoke(OccurTime); });
			}
		}
		private void RaiseEvent_CommunicatorClientConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState, bool Sync = true)
		{
			if (Sync)
			{
				CommunicatorClientConnectStateChanged?.Invoke(OccurTime, IpPort, NewState);
			}
			else
			{
				Task.Run(() => { CommunicatorClientConnectStateChanged?.Invoke(OccurTime, IpPort, NewState); });
			}
		}
		private void RaiseEvent_CommunicatorClientSentSerializableData(DateTime OccurTime, string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				CommunicatorClientSentSerializableData?.Invoke(OccurTime, IpPort, Data);
			}
			else
			{
				Task.Run(() => { CommunicatorClientSentSerializableData?.Invoke(OccurTime, IpPort, Data); });
			}
		}
		private void RaiseEvent_CommunicatorClientReceivedSerializableData(DateTime OccurTime, string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				CommunicatorClientReceivedSerializableData?.Invoke(OccurTime, IpPort, Data);
			}
			else
			{
				Task.Run(() => { CommunicatorClientReceivedSerializableData?.Invoke(OccurTime, IpPort, Data); });
			}
		}
		private void RaiseEvent_VehicleStateReporterSystemStarted(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				VehicleStateReporterSystemStarted?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { VehicleStateReporterSystemStarted?.Invoke(OccurTime); });
			}
		}
		private void RaiseEvent_VehicleStateReporterSystemStopped(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				VehicleStateReporterSystemStopped?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { VehicleStateReporterSystemStopped?.Invoke(OccurTime); });
			}
		}
		private void HandleDebugMessage(string Message)
		{
			Console.WriteLine(DateTime.Now.ToString(TIME_FORMAT) + " " + Message);
		}
		private void HandleDebugMessage(string Category, string Message)
		{
			HandleDebugMessage($"[{Category}] - {Message}");
		}
	}
}
