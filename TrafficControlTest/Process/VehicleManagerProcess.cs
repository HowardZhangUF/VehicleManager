using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibraryOfICollisionEventManager;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleCommunicator;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleInfoManager;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Base
{
	class VehicleManagerProcess
	{
		public event EventHandlerDateTime VehicleCommunicatorSystemStarted;
		public event EventHandlerDateTime VehicleCommunicatorSystemStopped;
		public event EventHandlerLocalListenState VehicleCommunicatorLocalListenStateChagned;
		public event EventHandlerRemoteConnectState VehicleCommunicatorRemoteConnectStateChagned;
		public event EventHandlerSentSerializableData VehicleCommunicatorSentSerializableData;
		public event EventHandlerReceivedSerializableData VehicleCommunicatorReceivedSerializableData;
		public event EventHandlerIVehicleInfo VehicleInfoManagerVehicleAdded;
		public event EventHandlerIVehicleInfo VehicleInfoManagerVehicleRemoved;
		public event EventHandlerIVehicleInfo VehicleInfoManagerVehicleStateUpdated;
		public event EventHandlerICollisionPair CollisionEventManagerCollisionEventAdded;
		public event EventHandlerICollisionPair CollisionEventManagerCollisionEventRemoved;
		public event EventHandlerICollisionPair CollisionEventManagerCollisionEventStateUpdated;
		public event EventHandlerDateTime CollisionEventDetectorSystemStarted;
		public event EventHandlerDateTime CollisionEventDetectorSystemStopped;

		private IVehicleCommunicator mVehicleCommunicator = null;
		private IVehicleInfoManager mVehicleInfoManager = null;
		private IVehicleMessageAnalyzer mVehicleMessageAnalyzer = null;
		private ICollisionEventManager mCollisionEventManager = null;
		private ICollisionEventDetector mCollisionEventDetector = null;

		public VehicleManagerProcess()
		{
			Constructor();
		}
		~VehicleManagerProcess()
		{
			Destructor();
		}
		public void VehicleCommunicatorStartListen(int Port)
		{
			mVehicleCommunicator.StartListen(Port);
		}
		public void VehicleCommunicatorStopListen()
		{
			mVehicleCommunicator.StopListen();
		}
		public void CollisionEventDetectorStart()
		{
			mCollisionEventDetector.Start();
		}
		public void CollisionEventDetectorStop()
		{
			mCollisionEventDetector.Stop();
		}
		public void SendCommand(string VehicleName, string Command, params string[] Paras)
		{
			if (mVehicleInfoManager.IsExist(VehicleName))
			{
				if (Command == "InsertMovingBuffer" && Paras != null && Paras.Length == 1)
				{
					mVehicleCommunicator.SendSerializableData_InsertMovingBuffer(mVehicleInfoManager.Get(VehicleName).mIpPort, Paras[0]);
				}
				else if (Command == "RemoveMovingBuffer")
				{
					mVehicleCommunicator.SendSerializableData_RemoveMovingBuffer(mVehicleInfoManager.Get(VehicleName).mIpPort);
				}
				else if (Command == "PauseMoving")
				{
					mVehicleCommunicator.SendSerializableData_PauseMoving(mVehicleInfoManager.Get(VehicleName).mIpPort);
				}
				else if (Command == "ResumeMoving")
				{
					mVehicleCommunicator.SendSerializableData_ResumeMoving(mVehicleInfoManager.Get(VehicleName).mIpPort);
				}
			}
		}
		public List<string> GetVehicleNameList()
		{
			return mVehicleInfoManager.GetNames();
		}

		private void Constructor()
		{
			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = GenerateIVehicleCommunicator();
			SubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = GenerateIVehicleInfoManager();
			SubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);

			UnsubscribeEvent_IVehicleMessageAnalyzer(mVehicleMessageAnalyzer);
			mVehicleMessageAnalyzer = GenerateIVehicleMessageAnalyzer(mVehicleCommunicator, mVehicleInfoManager);
			SubscribeEvent_IVehicleMessageAnalyzer(mVehicleMessageAnalyzer);

			UnsubscribeEvent_ICollisionEventManager(mCollisionEventManager);
			mCollisionEventManager = GenerateICollisionEventManager();
			SubscribeEvent_ICollisionEventManager(mCollisionEventManager);

			UnsubscribeEvent_ICollisionEventDetector(mCollisionEventDetector);
			mCollisionEventDetector = GenerateICollisionEventDetector(mVehicleInfoManager, mCollisionEventManager);
			SubscribeEvent_ICollisionEventDetector(mCollisionEventDetector);
		}
		private void Destructor()
		{
			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = null;

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = null;

			UnsubscribeEvent_IVehicleMessageAnalyzer(mVehicleMessageAnalyzer);
			mVehicleMessageAnalyzer = null;

			UnsubscribeEvent_ICollisionEventManager(mCollisionEventManager);
			mCollisionEventManager = null;

			UnsubscribeEvent_ICollisionEventDetector(mCollisionEventDetector);
			mCollisionEventDetector = null;
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SystemStarted += HandleEvent_VehicleCommunicatorSystemStarted;
				VehicleCommunicator.SystemStopped += HandleEvent_VehicleCommunicatorSystemStopped;
				VehicleCommunicator.LocalListenStateChanged += HandleEvent_VehicleCommunicatorLocalListenStateChagned;
				VehicleCommunicator.RemoteConnectStateChanged += HandleEvent_VehicleCommunicatorRemoteConnectStateChagned;
				VehicleCommunicator.SentSerializableData += HandleEvent_VehicleCommunicatorSentSerializableData;
				VehicleCommunicator.ReceivedSerializableData += HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SystemStarted -= HandleEvent_VehicleCommunicatorSystemStarted;
				VehicleCommunicator.SystemStopped -= HandleEvent_VehicleCommunicatorSystemStopped;
				VehicleCommunicator.LocalListenStateChanged -= HandleEvent_VehicleCommunicatorLocalListenStateChagned;
				VehicleCommunicator.RemoteConnectStateChanged -= HandleEvent_VehicleCommunicatorRemoteConnectStateChagned;
				VehicleCommunicator.SentSerializableData -= HandleEvent_VehicleCommunicatorSentSerializableData;
				VehicleCommunicator.ReceivedSerializableData -= HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.VehicleAdded += HandleEvent_VehicleInfoManagerVehicleAdded;
				VehicleInfoManager.VehicleRemoved += HandleEvent_VehicleInfoManagerVehicleRemoved;
				VehicleInfoManager.VehicleStateUpdated += HandleEvent_VehicleInfoManagerVehicleStateUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.VehicleAdded -= HandleEvent_VehicleInfoManagerVehicleAdded;
				VehicleInfoManager.VehicleRemoved -= HandleEvent_VehicleInfoManagerVehicleRemoved;
				VehicleInfoManager.VehicleStateUpdated -= HandleEvent_VehicleInfoManagerVehicleStateUpdated;
			}
		}
		private void SubscribeEvent_IVehicleMessageAnalyzer(IVehicleMessageAnalyzer VehicleMessageAnalyzer)
		{
			if (VehicleMessageAnalyzer != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleMessageAnalyzer(IVehicleMessageAnalyzer VehicleMessageAnalyzer)
		{
			if (VehicleMessageAnalyzer != null)
			{

			}
		}
		private void SubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{
				CollisionEventManager.CollisionEventAdded += HandleEvent_CollisionEventManagerCollisionEventAdded;
				CollisionEventManager.CollisionEventRemoved += HandleEvent_CollisionEventManagerCollisionEventRemoved;
				CollisionEventManager.CollisionEventStateUpdated += HandleEvent_CollisionEventManagerCollisionEventStateUpdated;
			}
		}
		private void UnsubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{
				CollisionEventManager.CollisionEventAdded -= HandleEvent_CollisionEventManagerCollisionEventAdded;
				CollisionEventManager.CollisionEventRemoved -= HandleEvent_CollisionEventManagerCollisionEventRemoved;
				CollisionEventManager.CollisionEventStateUpdated -= HandleEvent_CollisionEventManagerCollisionEventStateUpdated;
			}
		}
		private void SubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStarted += HandleEvent_CollisionEventDetectorSystemStarted;
				CollisionEventDetector.SystemStopped += HandleEvent_CollisionEventDetectorSystemStopped;
			}
		}
		private void UnsubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStarted -= HandleEvent_CollisionEventDetectorSystemStarted;
				CollisionEventDetector.SystemStopped -= HandleEvent_CollisionEventDetectorSystemStopped;
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorSystemStarted(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorSystemStarted?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorSystemStarted?.Invoke(OccurTime); });
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorSystemStopped(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorSystemStopped?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorSystemStopped?.Invoke(OccurTime); });
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorLocalListenStateChagned?.Invoke(OccurTime, NewState);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorLocalListenStateChagned?.Invoke(OccurTime, NewState); });
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorRemoteConnectStateChagned(DateTime OccurTime, string IpPort, ConnectState NewState, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorRemoteConnectStateChagned?.Invoke(OccurTime, IpPort, NewState);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorRemoteConnectStateChagned?.Invoke(OccurTime, IpPort, NewState); });
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorSentSerializableData(DateTime OccurTime, string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorSentSerializableData?.Invoke(OccurTime, IpPort, Data);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorSentSerializableData?.Invoke(OccurTime, IpPort, Data); });
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorReceivedSerializableData?.Invoke(OccurTime, IpPort, Data);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorReceivedSerializableData?.Invoke(OccurTime, IpPort, Data); });
			}
		}
		protected virtual void RaiseEvent_VehicleInfoManagerVehicleAdded(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleInfoManagerVehicleAdded?.Invoke(OccurTime, Name, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleInfoManagerVehicleAdded?.Invoke(OccurTime, Name, VehicleInfo); });
			}
		}
		protected virtual void RaiseEvent_VehicleInfoManagerVehicleRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleInfoManagerVehicleRemoved?.Invoke(OccurTime, Name, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleInfoManagerVehicleRemoved?.Invoke(OccurTime, Name, VehicleInfo); });
			}
		}
		protected virtual void RaiseEvent_VehicleInfoManagerVehicleStateUpdated(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleInfoManagerVehicleStateUpdated?.Invoke(OccurTime, Name, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleInfoManagerVehicleStateUpdated?.Invoke(OccurTime, Name, VehicleInfo); });
			}
		}
		protected virtual void RaiseEvent_CollisionEventManagerCollisionEventAdded(DateTime OccurTime, string Name, ICollisionPair CollisionPair, bool Sync = true)
		{
			if (Sync)
			{
				CollisionEventManagerCollisionEventAdded?.Invoke(OccurTime, Name, CollisionPair);
			}
			else
			{
				Task.Run(() => { CollisionEventManagerCollisionEventAdded?.Invoke(OccurTime, Name, CollisionPair); });
			}
		}
		protected virtual void RaiseEvent_CollisionEventManagerCollisionEventRemoved(DateTime OccurTime, string Name, ICollisionPair CollisionPair, bool Sync = true)
		{
			if (Sync)
			{
				CollisionEventManagerCollisionEventRemoved?.Invoke(OccurTime, Name, CollisionPair);
			}
			else
			{
				Task.Run(() => { CollisionEventManagerCollisionEventRemoved?.Invoke(OccurTime, Name, CollisionPair); });
			}
		}
		protected virtual void RaiseEvent_CollisionEventManagerCollisionEventStateUpdated(DateTime OccurTime, string Name, ICollisionPair CollisionPair, bool Sync = true)
		{
			if (Sync)
			{
				CollisionEventManagerCollisionEventStateUpdated?.Invoke(OccurTime, Name, CollisionPair);
			}
			else
			{
				Task.Run(() => { CollisionEventManagerCollisionEventStateUpdated?.Invoke(OccurTime, Name, CollisionPair); });
			}
		}
		protected virtual void RaiseEvent_CollisionEventDetectorSystemStarted(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				CollisionEventDetectorSystemStarted?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { CollisionEventDetectorSystemStarted?.Invoke(OccurTime); });
			}
		}
		protected virtual void RaiseEvent_CollisionEventDetectorSystemStopped(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				CollisionEventDetectorSystemStopped?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { CollisionEventDetectorSystemStopped?.Invoke(OccurTime); });
			}
		}
		private void HandleEvent_VehicleCommunicatorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage("VehicleCommunicator", "System Started.");
			RaiseEvent_VehicleCommunicatorSystemStarted(OccurTime);
		}
		private void HandleEvent_VehicleCommunicatorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage("VehicleCommunicator", "System Stopped.");
			RaiseEvent_VehicleCommunicatorSystemStopped(OccurTime);
		}
		private void HandleEvent_VehicleCommunicatorLocalListenStateChagned(DateTime OccurTime, ListenState NewState)
		{
			HandleDebugMessage("VehicleCommunicator", $"Local Listen State Changed. State: {NewState.ToString()}");
			RaiseEvent_VehicleCommunicatorLocalListenStateChanged(OccurTime, NewState);
		}
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChagned(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage("VehicleCommunicator", $"Remote Connect State Changed. IPPort: {IpPort}, State: {NewState}");
			RaiseEvent_VehicleCommunicatorRemoteConnectStateChagned(OccurTime, IpPort, NewState);
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage("VehicleCommunicator", $"Sent Serializable Data. IPPort: {IpPort}, DataType: {Data.GetType().ToString()}");
			RaiseEvent_VehicleCommunicatorSentSerializableData(OccurTime, IpPort, Data);
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage("VehicleCommunicator", $"Received Serializable Data. IPPort: {IpPort}, DataType: {Data.GetType().ToString()}");
			RaiseEvent_VehicleCommunicatorReceivedSerializableData(OccurTime, IpPort, Data);
		}
		private void HandleEvent_VehicleInfoManagerVehicleAdded(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage("VehicleInfoManager", $"Vehicle Added. Name: {Name}, Info: {VehicleInfo.ToString()}");
			RaiseEvent_VehicleInfoManagerVehicleAdded(OccurTime, Name, VehicleInfo);
		}
		private void HandleEvent_VehicleInfoManagerVehicleRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage("VehicleInfoManager", $"Vehicle Removed. Name: {Name}, Info: {VehicleInfo.ToString()}");
			RaiseEvent_VehicleInfoManagerVehicleRemoved(OccurTime, Name, VehicleInfo);
		}
		private void HandleEvent_VehicleInfoManagerVehicleStateUpdated(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage("VehicleInfoManager", $"Vehicle State Updated. Name: {Name}, Info: {VehicleInfo.ToString()}");
			RaiseEvent_VehicleInfoManagerVehicleStateUpdated(OccurTime, Name, VehicleInfo);
		}
		private void HandleEvent_CollisionEventManagerCollisionEventAdded(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleDebugMessage("CollisionEventManager", $"Collision Event Added. Name: {Name}, Info:\n{CollisionPair.ToString()}");
			RaiseEvent_CollisionEventManagerCollisionEventAdded(OccurTime, Name, CollisionPair);
		}
		private void HandleEvent_CollisionEventManagerCollisionEventRemoved(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleDebugMessage("CollisionEventManager", $"Collision Event Removed. Name: {Name}, Info:\n{CollisionPair.ToString()}");
			RaiseEvent_CollisionEventManagerCollisionEventRemoved(OccurTime, Name, CollisionPair);
		}
		private void HandleEvent_CollisionEventManagerCollisionEventStateUpdated(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleDebugMessage("CollisionEventManager", $"Collision Event StateUpdated. Name: {Name}, Info:\n{CollisionPair.ToString()}");
			RaiseEvent_CollisionEventManagerCollisionEventStateUpdated(OccurTime, Name, CollisionPair);
		}
		private void HandleEvent_CollisionEventDetectorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage("CollisionEventDetector", "System Started.");
			RaiseEvent_CollisionEventDetectorSystemStarted(OccurTime);
		}
		private void HandleEvent_CollisionEventDetectorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage("CollisionEventDetector", "System Stopped.");
			RaiseEvent_CollisionEventDetectorSystemStopped(OccurTime);
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
