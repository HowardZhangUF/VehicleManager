using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Interface;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Base
{
	class VehicleManagerProcess
	{
		public event EventHandlerDebugMessage DebugMessage;
		public event EventHandlerDateTime VehicleCommunicatorSystemStarted;
		public event EventHandlerDateTime VehicleCommunicatorSystemStopped;
		public event EventHandlerLocalListenState VehicleCommunicatorLocalListenStateChagned;
		public event EventHandlerRemoteConnectState VehicleCommunicatorRemoteConnectStateChagned;
		public event EventHandlerSentSerializableData VehicleCommunicatorSentSerializableData;
		public event EventHandlerReceivedSerializableData VehicleCommunicatorReceivedSerializableData;
		public event EventHandlerSentSerializableData VehicleCommunicatorSentSerializableDataSuccessed;
		public event EventHandlerSentSerializableData VehicleCommunicatorSentSerializableDataFailed;
		public event EventHandlerItem<IVehicleInfo> VehicleInfoManagerItemAdded;
		public event EventHandlerItem<IVehicleInfo> VehicleInfoManagerItemRemoved;
		public event EventHandlerItemUpdated<IVehicleInfo> VehicleInfoManagerItemUpdated;
		public event EventHandlerICollisionPair CollisionEventManagerCollisionEventAdded;
		public event EventHandlerICollisionPair CollisionEventManagerCollisionEventRemoved;
		public event EventHandlerICollisionPair CollisionEventManagerCollisionEventStateUpdated;
		public event EventHandlerDateTime CollisionEventDetectorSystemStarted;
		public event EventHandlerDateTime CollisionEventDetectorSystemStopped;
		public event EventHandlerItem<IVehicleControl> VehicleControlManagerItemAdded;
		public event EventHandlerItem<IVehicleControl> VehicleControlManagerItemRemoved;
		public event EventHandlerItemUpdated<IVehicleControl> VehicleControlManagerItemUpdated;
		public event EventHandlerDateTime VehicleControlHandlerSystemStarted;
		public event EventHandlerDateTime VehicleControlHandlerSystemStopped;
		public event EventHandlerItem<IMissionState> MissionStateManagerItemAdded;
		public event EventHandlerItem<IMissionState> MissionStateManagerItemRemoved;
		public event EventHandlerItemUpdated<IMissionState> MissionStateManagerItemUpdated;
		public event EventHandlerDateTime HostCommunicatorSystemStarted;
		public event EventHandlerDateTime HostCommunicatorSystemStopped;
		public event EventHandlerRemoteConnectState HostCommunicatorRemoteConnectStateChanged;
		public event EventHandlerLocalListenState HostCommunicatorLocalListenStateChanged;
		public event EventHandlerSentString HostCommunicatorSentString;
		public event EventHandlerReceivedString HostCommunicatorReceivedString;
		public event EventHandlerDateTime MissionDispatcherSystemStarted;
		public event EventHandlerDateTime MissionDispatcherSystemStopped;
		public event EventHandlerMapFileName MapManagerMapFileAdded;
		public event EventHandlerMapFileName MapManagerMapFileRemoved;
		public event EventHandlerVehicleNamesMapFileName MapManagerVehicleCurrentMapSynchronized;

		private IVehicleCommunicator mVehicleCommunicator = null;
		private IVehicleInfoManager mVehicleInfoManager = null;
		private IVehicleInfoUpdater mVehicleInfoUpdater = null;
		private ICollisionEventManager mCollisionEventManager = null;
		private ICollisionEventDetector mCollisionEventDetector = null;
		private IVehicleControlManager mVehicleControlManager = null;
		private ICollisionEventHandler mCollisionEventHandler = null;
		private IVehicleControlHandler mVehicleControlHandler = null;
		private IMissionStateManager mMissionStateManager = null;
		private IHostCommunicator mHostCommunicator = null;
		private IHostMessageAnalyzer mHostMessageAnalyzer = null;
		private IMissionDispatcher mMissionDispatcher = null;
		private IMissionUpdater mMissionUpdater = null;
		private IMapManager mMapManager = null;

		public VehicleManagerProcess()
		{
			Constructor();
		}
		~VehicleManagerProcess()
		{
			Destructor();
		}
		public void VehicleCommunicatorSetConfigOfListenPort(int Port)
		{
			mVehicleCommunicator.SetConfigOfListenPort(Port);
		}
		public int VehicleCommunicatorGetConfigOfListenPort()
		{
			return mVehicleCommunicator.GetConfigOfListenPort();
		}
		public void VehicleCommunicatorStartListen()
		{
			mVehicleCommunicator.StartListen();
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
		public void VehicleControlHandlerStart()
		{
			mVehicleControlHandler.Start();
		}
		public void VehicleControlHandlerStop()
		{
			mVehicleControlHandler.Stop();
		}
		public void SendCommand(string VehicleName, string Command, params string[] Paras)
		{
			if (mVehicleInfoManager.IsExist(VehicleName))
			{
				string vehicleIpPort = mVehicleInfoManager.GetItem(VehicleName).mIpPort;
				if (Command == "Goto" && Paras != null && Paras.Length == 1)
				{
					mVehicleCommunicator.SendSerializableData_Goto(vehicleIpPort, Paras[0]);
				}
				else if (Command == "GotoPoint" && Paras != null)
				{
					if (Paras.Length == 2)
					{
						mVehicleCommunicator.SendSerializableData_GotoPoint(vehicleIpPort, int.Parse(Paras[0]), int.Parse(Paras[1]));
					}
					else if (Paras.Length == 3)
					{
						mVehicleCommunicator.SendSerializableData_GotoTowardPoint(vehicleIpPort, int.Parse(Paras[0]), int.Parse(Paras[1]), int.Parse(Paras[2]));
					}
				}
				else if (Command == "Stop")
				{
					mVehicleCommunicator.SendSerializableData_Stop(vehicleIpPort);
				}
				else if (Command == "InsertMovingBuffer" && Paras != null && Paras.Length == 2)
				{
					mVehicleCommunicator.SendSerializableData_InsertMovingBuffer(vehicleIpPort, int.Parse(Paras[0]), int.Parse(Paras[1]));
				}
				else if (Command == "RemoveMovingBuffer")
				{
					mVehicleCommunicator.SendSerializableData_RemoveMovingBuffer(vehicleIpPort);
				}
				else if (Command == "PauseMoving")
				{
					mVehicleCommunicator.SendSerializableData_PauseMoving(vehicleIpPort);
				}
				else if (Command == "ResumeMoving")
				{
					mVehicleCommunicator.SendSerializableData_ResumeMoving(vehicleIpPort);
				}
				else if (Command == "RequestMapList")
				{
					mVehicleCommunicator.SendSerializableData_RequestMapList(vehicleIpPort);
				}
				else if (Command == "GetMap" && Paras != null && Paras.Length == 1)
				{
					mVehicleCommunicator.SendSerializableData_GetMap(vehicleIpPort, Paras[0]);
				}
				else if (Command == "UploadMapToAGV" && Paras != null && Paras.Length == 1)
				{
					mVehicleCommunicator.SendSerializableData_UploadMapToAGV(vehicleIpPort, mMapManager.GetMapFileFullPath(Paras[0]));
				}
				else if (Command == "ChangeMap" && Paras != null && Paras.Length == 1)
				{
					mVehicleCommunicator.SendSerializableData_ChangeMap(vehicleIpPort, Paras[0]);
				}
			}
		}
		public int GetVehicleCount()
		{
			return mVehicleInfoManager.mCount;
		}
		public List<string> GetVehicleNameList()
		{
			return mVehicleInfoManager.GetItemNames().ToList();
		}
		public IVehicleInfo GetVehicleInfo(string VehicleName)
		{
			return mVehicleInfoManager.GetItem(VehicleName);
		}
		public void HostCommunicatorSetConfigOfListenPort(int Port)
		{
			mHostCommunicator.SetConfigOfListenPort(Port);
		}
		public int HostCommunicatorGetConfigOfListenPort()
		{
			return mHostCommunicator.GetConfigOfListenPort();
		}
		public void HostCommunicatorStartListen()
		{
			mHostCommunicator.StartListen();
		}
		public void HostCommunicatorStopListen()
		{
			mHostCommunicator.StopListen();
		}
		public void MissionDispatcherStart()
		{
			mMissionDispatcher.Start();
		}
		public void MissionDispatcherStop()
		{
			mMissionDispatcher.Stop();
		}
		public void MapManagerSetConfigOfMapDirectory(string MapDirectory)
		{
			mMapManager.SetConfigOfMapDirectory(MapDirectory);
		}
		public string MapManagerGetConfigOfMapDirectory()
		{
			return mMapManager.GetConfigOfMapDirectory();
		}
		public string[] MapManagerGetLocalMapNameList()
		{
			return mMapManager.GetLocalMapNameList();
		}

		private void Constructor()
		{
			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = GenerateIVehicleCommunicator();
			SubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = GenerateIVehicleInfoManager();
			SubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);

			UnsubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);
			mVehicleInfoUpdater = GenerateIVehicleInfoUpdater(mVehicleCommunicator, mVehicleInfoManager);
			SubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);

			UnsubscribeEvent_ICollisionEventManager(mCollisionEventManager);
			mCollisionEventManager = GenerateICollisionEventManager();
			SubscribeEvent_ICollisionEventManager(mCollisionEventManager);

			UnsubscribeEvent_ICollisionEventDetector(mCollisionEventDetector);
			mCollisionEventDetector = GenerateICollisionEventDetector(mVehicleInfoManager, mCollisionEventManager);
			SubscribeEvent_ICollisionEventDetector(mCollisionEventDetector);

			UnsubscribeEvent_IVehicleControlManager(mVehicleControlManager);
			mVehicleControlManager = GenerateIVehicleControlManager();
			SubscribeEvent_IVehicleControlManager(mVehicleControlManager);

			UnsubscribeEvent_ICollisionEventHandler(mCollisionEventHandler);
			mCollisionEventHandler = GenerateICollisionEventHandler(mCollisionEventManager, mVehicleControlManager);
			SubscribeEvent_ICollisionEventHandler(mCollisionEventHandler);

			UnsubscribeEvent_IVehicleControlHandler(mVehicleControlHandler);
			mVehicleControlHandler = GenerateIVehicleControlHandler(mVehicleControlManager, mVehicleInfoManager, mVehicleCommunicator);
			SubscribeEvent_IVehicleControlHandler(mVehicleControlHandler);

			UnsubscribeEvent_IMissionStateManager(mMissionStateManager);
			mMissionStateManager = GenerateIMissionStateManager();
			SubscribeEvent_IMissionStateManager(mMissionStateManager);

			UnsubscribeEvent_IHostCommunicator(mHostCommunicator);
			mHostCommunicator = GenerateIHostCommunicator();
			SubscribeEvent_IHostCommunicator(mHostCommunicator);

			UnsubscribeEvent_IHostMessageAnalyzer(mHostMessageAnalyzer);
			mHostMessageAnalyzer = GenerateIHostMessageAnalyzer(mHostCommunicator, mMissionStateManager, GetMissionAnalyzers());
			SubscribeEvent_IHostMessageAnalyzer(mHostMessageAnalyzer);

			UnsubscribeEvent_IMissionDispatcher(mMissionDispatcher);
			mMissionDispatcher = GenerateIMissionDispatcher(mMissionStateManager, mVehicleInfoManager, mVehicleCommunicator);
			SubscribeEvent_IMissionDispatcher(mMissionDispatcher);

			UnsubscribeEvent_IMissionUpdater(mMissionUpdater);
			mMissionUpdater = GenerateIMissionUpdater(mVehicleCommunicator, mVehicleInfoManager, mMissionStateManager);
			SubscribeEvent_IMissionUpdater(mMissionUpdater);

			UnsubscribeEvent_IMapManager(mMapManager);
			mMapManager = GenerateIMapManager(mVehicleCommunicator, mVehicleInfoManager);
			SubscribeEvent_IMapManager(mMapManager);
		}
		private void Destructor()
		{
			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = null;

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = null;

			UnsubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);
			mVehicleInfoUpdater = null;

			UnsubscribeEvent_ICollisionEventManager(mCollisionEventManager);
			mCollisionEventManager = null;

			UnsubscribeEvent_ICollisionEventDetector(mCollisionEventDetector);
			mCollisionEventDetector = null;

			UnsubscribeEvent_IVehicleControlManager(mVehicleControlManager);
			mVehicleControlManager = null;

			UnsubscribeEvent_ICollisionEventHandler(mCollisionEventHandler);
			mCollisionEventHandler = null;

			UnsubscribeEvent_IVehicleControlHandler(mVehicleControlHandler);
			mVehicleControlHandler = null;

			UnsubscribeEvent_IMissionStateManager(mMissionStateManager);
			mMissionStateManager = null;

			UnsubscribeEvent_IHostCommunicator(mHostCommunicator);
			mHostCommunicator = null;

			UnsubscribeEvent_IHostMessageAnalyzer(mHostMessageAnalyzer);
			mHostMessageAnalyzer = null;

			UnsubscribeEvent_IMissionDispatcher(mMissionDispatcher);
			mMissionDispatcher = null;
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
				VehicleCommunicator.SentSerializableDataSuccessed += HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
				VehicleCommunicator.SentSerializableDataFailed += HandleEvent_VehicleCommunicatorSentSerializableDataFailed;
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
				VehicleCommunicator.SentSerializableDataSuccessed -= HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
				VehicleCommunicator.SentSerializableDataFailed -= HandleEvent_VehicleCommunicatorSentSerializableDataFailed;
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehicleInfoUpdater(IVehicleInfoUpdater VehicleInfoUpdater)
		{
			if (VehicleInfoUpdater != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleInfoUpdater(IVehicleInfoUpdater VehicleInfoUpdater)
		{
			if (VehicleInfoUpdater != null)
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
		private void SubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemAdded += HandleEvent_VehicleControlManagerItemAdded;
				VehicleControlManager.ItemRemoved += HandleEvent_VehicleControlManagerItemRemoved;
				VehicleControlManager.ItemUpdated += HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemAdded -= HandleEvent_VehicleControlManagerItemAdded;
				VehicleControlManager.ItemRemoved -= HandleEvent_VehicleControlManagerItemRemoved;
				VehicleControlManager.ItemUpdated -= HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void SubscribeEvent_ICollisionEventHandler(ICollisionEventHandler CollisionEventHandler)
		{
			if (CollisionEventHandler != null)
			{

			}
		}
		private void UnsubscribeEvent_ICollisionEventHandler(ICollisionEventHandler CollisionEventHandler)
		{
			if (CollisionEventHandler != null)
			{

			}
		}
		private void SubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStarted += HandleEvent_VehicleControlHandlerSystemStarted;
				VehicleControlHandler.SystemStopped += HandleEvent_VehicleControlHandlerSystemStopped;
			}
		}
		private void UnsubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStarted -= HandleEvent_VehicleControlHandlerSystemStarted;
				VehicleControlHandler.SystemStopped -= HandleEvent_VehicleControlHandlerSystemStopped;
			}
		}
		private void SubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded += HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemRemoved += HandleEvent_MissionStateManagerItemRemoved;
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded -= HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemRemoved -= HandleEvent_MissionStateManagerItemRemoved;
				MissionStateManager.ItemUpdated -= HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.SystemStarted += HandleEvent_HostCommunicatorSystemStarted;
				HostCommunicator.SystemStopped += HandleEvent_HostCommunicatorSystemStopped;
				HostCommunicator.LocalListenStateChanged += HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.RemoteConnectStateChanged += HandleEvent_HostCommunicatorRemoteConnectStateChanged;
				HostCommunicator.SentString += HandleEvent_HostCommunicatorSentString;
				HostCommunicator.ReceivedString += HandleEvent_HostCommunicatorReceivedString;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.SystemStarted -= HandleEvent_HostCommunicatorSystemStarted;
				HostCommunicator.SystemStopped -= HandleEvent_HostCommunicatorSystemStopped;
				HostCommunicator.LocalListenStateChanged -= HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.RemoteConnectStateChanged -= HandleEvent_HostCommunicatorRemoteConnectStateChanged;
				HostCommunicator.SentString -= HandleEvent_HostCommunicatorSentString;
				HostCommunicator.ReceivedString -= HandleEvent_HostCommunicatorReceivedString;
			}
		}
		private void SubscribeEvent_IHostMessageAnalyzer(IHostMessageAnalyzer HostMessageAnalyzer)
		{
			if (HostMessageAnalyzer != null)
			{

			}
		}
		private void UnsubscribeEvent_IHostMessageAnalyzer(IHostMessageAnalyzer HostMessageAnalyzer)
		{
			if (HostMessageAnalyzer != null)
			{

			}
		}
		private void SubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStarted += HandleEvent_MissionDispatcherSystemStarted;
				MissionDispatcher.SystemStopped += HandleEvent_MissionDispatcherSystemStopped;
			}
		}
		private void UnsubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStarted -= HandleEvent_MissionDispatcherSystemStarted;
				MissionDispatcher.SystemStopped -= HandleEvent_MissionDispatcherSystemStopped;
			}
		}
		private void SubscribeEvent_IMissionUpdater(IMissionUpdater MissionUpdater)
		{
			if (MissionUpdater != null)
			{

			}
		}
		private void UnsubscribeEvent_IMissionUpdater(IMissionUpdater MissionUpdater)
		{
			if (MissionUpdater != null)
			{

			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapFileAdded += HandleEvent_MapManagerMapFileAdded;
				MapManager.MapFileRemoved += HandleEvent_MapManagerMapFileRemoved;
				MapManager.VehicleCurrentMapSynchronized += HandleEvent_MapManagerVehicleCurrentMapSynchronized;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapFileAdded -= HandleEvent_MapManagerMapFileAdded;
				MapManager.MapFileRemoved -= HandleEvent_MapManagerMapFileRemoved;
				MapManager.VehicleCurrentMapSynchronized -= HandleEvent_MapManagerVehicleCurrentMapSynchronized;
			}
		}
		protected virtual void RaiseEvent_DebugMessage(string OccurTime, string Category, string Message, bool Sync = true)
		{
			if (Sync)
			{
				DebugMessage?.Invoke(OccurTime, Category, Message);
			}
			else
			{
				Task.Run(() => { DebugMessage?.Invoke(OccurTime, Category, Message); });
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
		protected virtual void RaiseEvent_VehicleCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorLocalListenStateChagned?.Invoke(OccurTime, NewState, Port);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorLocalListenStateChagned?.Invoke(OccurTime, NewState, Port); });
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
		protected virtual void RaiseEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorSentSerializableDataSuccessed?.Invoke(OccurTime, IpPort, Data);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorSentSerializableDataSuccessed?.Invoke(OccurTime, IpPort, Data); });
			}
		}
		protected virtual void RaiseEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCommunicatorSentSerializableDataFailed?.Invoke(OccurTime, IpPort, Data);
			}
			else
			{
				Task.Run(() => { VehicleCommunicatorSentSerializableDataFailed?.Invoke(OccurTime, IpPort, Data); });
			}
		}
		protected virtual void RaiseEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleInfoManagerItemAdded?.Invoke(OccurTime, Name, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleInfoManagerItemAdded?.Invoke(OccurTime, Name, VehicleInfo); });
			}
		}
		protected virtual void RaiseEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleInfoManagerItemRemoved?.Invoke(OccurTime, Name, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleInfoManagerItemRemoved?.Invoke(OccurTime, Name, VehicleInfo); });
			}
		}
		protected virtual void RaiseEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo VehicleInfo, bool Sync = true)
		{
			if (Sync)
			{
				VehicleInfoManagerItemUpdated?.Invoke(OccurTime, Name, StateName, VehicleInfo);
			}
			else
			{
				Task.Run(() => { VehicleInfoManagerItemUpdated?.Invoke(OccurTime, Name, StateName, VehicleInfo); });
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
		protected virtual void RaiseEvent_VehicleControlManagerItemAdded(DateTime OccurTime, string Name, IVehicleControl VehicleControl, bool Sync = true)
		{
			if (Sync)
			{
				VehicleControlManagerItemAdded?.Invoke(OccurTime, Name, VehicleControl);
			}
			else
			{
				Task.Run(() => { VehicleControlManagerItemAdded?.Invoke(OccurTime, Name, VehicleControl); });
			}
		}
		protected virtual void RaiseEvent_VehicleControlManagerItemRemoved(DateTime OccurTime, string Name, IVehicleControl VehicleControl, bool Sync = true)
		{
			if (Sync)
			{
				VehicleControlManagerItemRemoved?.Invoke(OccurTime, Name, VehicleControl);
			}
			else
			{
				Task.Run(() => { VehicleControlManagerItemRemoved?.Invoke(OccurTime, Name, VehicleControl); });
			}
		}
		protected virtual void RaiseEvent_VehicleControlManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleControl VehicleControl, bool Sync = true)
		{
			if (Sync)
			{
				VehicleControlManagerItemUpdated?.Invoke(OccurTime, Name, StateName, VehicleControl);
			}
			else
			{
				Task.Run(() => { VehicleControlManagerItemUpdated?.Invoke(OccurTime, Name, StateName, VehicleControl); });
			}
		}
		protected virtual void RaiseEvent_VehicleControlHandlerSystemStarted(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				VehicleControlHandlerSystemStarted?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { VehicleControlHandlerSystemStarted?.Invoke(OccurTime); });
			}
		}
		protected virtual void RaiseEvent_VehicleControlHandlerSystemStopped(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				VehicleControlHandlerSystemStopped?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { VehicleControlHandlerSystemStopped?.Invoke(OccurTime); });
			}
		}
		protected virtual void RaiseEvent_MissionStateManagerItemAdded(DateTime OccurTime, string MissionId, IMissionState MissionState, bool Sync = true)
		{
			if (Sync)
			{
				MissionStateManagerItemAdded?.Invoke(OccurTime, MissionId, MissionState);
			}
			else
			{
				Task.Run(() => { MissionStateManagerItemAdded?.Invoke(OccurTime, MissionId, MissionState); });
			}
		}
		protected virtual void RaiseEvent_MissionStateManagerItemRemoved(DateTime OccurTime, string MissionId, IMissionState MissionState, bool Sync = true)
		{
			if (Sync)
			{
				MissionStateManagerItemRemoved?.Invoke(OccurTime, MissionId, MissionState);
			}
			else
			{
				Task.Run(() => { MissionStateManagerItemRemoved?.Invoke(OccurTime, MissionId, MissionState); });
			}
		}
		protected virtual void RaiseEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string MissionId, string StateName, IMissionState MissionState, bool Sync = true)
		{
			if (Sync)
			{
				MissionStateManagerItemUpdated?.Invoke(OccurTime, MissionId, StateName, MissionState);
			}
			else
			{
				Task.Run(() => { MissionStateManagerItemUpdated?.Invoke(OccurTime, MissionId, StateName, MissionState); });
			}
		}
		protected virtual void RaiseEvent_HostCommunicatorSystemStarted(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				HostCommunicatorSystemStarted?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { HostCommunicatorSystemStarted?.Invoke(OccurTime); });
			}
		}
		protected virtual void RaiseEvent_HostCommunicatorSystemStopped(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				HostCommunicatorSystemStopped?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { HostCommunicatorSystemStopped?.Invoke(OccurTime); });
			}
		}
		protected virtual void RaiseEvent_HostCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port, bool Sync = true)
		{
			if (Sync)
			{
				HostCommunicatorLocalListenStateChanged?.Invoke(OccurTime, NewState, Port);
			}
			else
			{
				Task.Run(() => { HostCommunicatorLocalListenStateChanged?.Invoke(OccurTime, NewState, Port); });
			}
		}
		protected virtual void RaiseEvent_HostCommunicatorRemoteConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState, bool Sync = true)
		{
			if (Sync)
			{
				HostCommunicatorRemoteConnectStateChanged?.Invoke(OccurTime, IpPort, NewState);
			}
			else
			{
				Task.Run(() => { HostCommunicatorRemoteConnectStateChanged?.Invoke(OccurTime, IpPort, NewState); });
			}
		}
		protected virtual void RaiseEvent_HostCommunicatorSentString(DateTime OccurTime, string IpPort, string Data, bool Sync = true)
		{
			if (Sync)
			{
				HostCommunicatorSentString?.Invoke(OccurTime, IpPort, Data);
			}
			else
			{
				Task.Run(() => { HostCommunicatorSentString?.Invoke(OccurTime, IpPort, Data); });
			}
		}
		protected virtual void RaiseEvent_HostCommunicatorReceivedString(DateTime OccurTime, string IpPort, string Data, bool Sync = true)
		{
			if (Sync)
			{
				HostCommunicatorReceivedString?.Invoke(OccurTime, IpPort, Data);
			}
			else
			{
				Task.Run(() => { HostCommunicatorReceivedString?.Invoke(OccurTime, IpPort, Data); });
			}
		}
		protected virtual void RaiseEvent_MissionDispatcherSystemStarted(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				MissionDispatcherSystemStarted?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { MissionDispatcherSystemStarted?.Invoke(OccurTime); });
			}
		}
		protected virtual void RaiseEvent_MissionDispatcherSystemStopped(DateTime OccurTime, bool Sync = true)
		{
			if (Sync)
			{
				MissionDispatcherSystemStopped?.Invoke(OccurTime);
			}
			else
			{
				Task.Run(() => { MissionDispatcherSystemStopped?.Invoke(OccurTime); });
			}
		}
		protected virtual void RaiseEvent_MapManagerMapFileAdded(DateTime OccurTime, string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapManagerMapFileAdded?.Invoke(OccurTime, MapFileName);
			}
			else
			{
				Task.Run(() => { MapManagerMapFileAdded?.Invoke(OccurTime, MapFileName); });
			}
		}
		protected virtual void RaiseEvent_MapManagerMapFileRemoved(DateTime OccurTime, string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapManagerMapFileRemoved?.Invoke(OccurTime, MapFileName);
			}
			else
			{
				Task.Run(() => { MapManagerMapFileRemoved?.Invoke(OccurTime, MapFileName); });
			}
		}
		protected virtual void RaiseEvent_MapManagerVehicleCurrentMapSynchronized(DateTime OccurTime, IEnumerable<string> VehicleNames, string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapManagerVehicleCurrentMapSynchronized?.Invoke(OccurTime, VehicleNames, MapFileName);
			}
			else
			{
				Task.Run(() => { MapManagerVehicleCurrentMapSynchronized?.Invoke(OccurTime, VehicleNames, MapFileName); });
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
		private void HandleEvent_VehicleCommunicatorLocalListenStateChagned(DateTime OccurTime, ListenState NewState, int Port)
		{
			HandleDebugMessage("VehicleCommunicator", $"Local Listen State Changed. State: {NewState.ToString()}, Port: {Port}");
			RaiseEvent_VehicleCommunicatorLocalListenStateChanged(OccurTime, NewState, Port);
		}
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChagned(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage("VehicleCommunicator", $"Remote Connect State Changed. IPPort: {IpPort}, State: {NewState}");
			RaiseEvent_VehicleCommunicatorRemoteConnectStateChagned(OccurTime, IpPort, NewState);
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage("VehicleCommunicator", $"Sent Serializable Data. IPPort: {IpPort}, DataType: {Data.ToString()}");
			RaiseEvent_VehicleCommunicatorSentSerializableData(OccurTime, IpPort, Data);
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			if (!(Data is SerialData.AGVStatus) && !(Data is SerialData.AGVPath))
			{
				HandleDebugMessage("VehicleCommunicator", $"Received Serializable Data. IPPort: {IpPort}, DataType: {Data.ToString()}");
				RaiseEvent_VehicleCommunicatorReceivedSerializableData(OccurTime, IpPort, Data);
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage("VehicleCommunicator", $"Sent Serializable Data Successed. IPPort: {IpPort}, DataType: {Data.ToString()}");
			RaiseEvent_VehicleCommunicatorSentSerializableDataSuccessed(OccurTime, IpPort, Data);
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage("VehicleCommunicator", $"Sent Serializable Data Failed. IPPort: {IpPort}, DataType: {Data.ToString()}");
			RaiseEvent_VehicleCommunicatorSentSerializableDataFailed(OccurTime, IpPort, Data);
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage("VehicleInfoManager", $"Item Added. Name: {Name}, Info: {VehicleInfo.ToString()}");
			RaiseEvent_VehicleInfoManagerItemAdded(OccurTime, Name, VehicleInfo);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage("VehicleInfoManager", $"Item Removed. Name: {Name}, Info: {VehicleInfo.ToString()}");
			RaiseEvent_VehicleInfoManagerItemRemoved(OccurTime, Name, VehicleInfo);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo VehicleInfo)
		{
			//HandleDebugMessage("VehicleInfoManager", $"Item Updated. Name: {Name}, StateName: {StateName}, Info: {VehicleInfo.ToString()}");
			RaiseEvent_VehicleInfoManagerItemUpdated(OccurTime, Name, StateName, VehicleInfo);
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
			//HandleDebugMessage("CollisionEventManager", $"Collision Event StateUpdated. Name: {Name}, Info:\n{CollisionPair.ToString()}");
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
		private void HandleEvent_VehicleControlManagerItemAdded(DateTime OccurTime, string Name, IVehicleControl VehicleControl)
		{
			HandleDebugMessage("VehicleControlManager", $"Item Added. Name: {Name}, Info:\n{VehicleControl.ToString()}");
			RaiseEvent_VehicleControlManagerItemAdded(OccurTime, Name, VehicleControl);
		}
		private void HandleEvent_VehicleControlManagerItemRemoved(DateTime OccurTime, string Name, IVehicleControl VehicleControl)
		{
			HandleDebugMessage("VehicleControlManager", $"Item Removed. Name: {Name}, Info:\n{VehicleControl.ToString()}");
			RaiseEvent_VehicleControlManagerItemRemoved(OccurTime, Name, VehicleControl);
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleControl VehicleControl)
		{
			HandleDebugMessage("VehicleControlManager", $"Item StateUpdated. Name: {Name}, StateName: {StateName}, Info:\n{VehicleControl.ToString()}");
			RaiseEvent_VehicleControlManagerItemUpdated(OccurTime, Name, StateName, VehicleControl);
		}
		private void HandleEvent_VehicleControlHandlerSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage("VehicleControlHandler", "System Started.");
			RaiseEvent_VehicleControlHandlerSystemStarted(OccurTime);
		}
		private void HandleEvent_VehicleControlHandlerSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage("VehicleControlHandler", "System Stopped.");
			RaiseEvent_VehicleControlHandlerSystemStopped(OccurTime);
		}
		private void HandleEvent_MissionStateManagerItemAdded(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			HandleDebugMessage("MissionStateManager", $"Item Added. MissionID: {MissionId}, Info: {MissionState.ToString()}");
			RaiseEvent_MissionStateManagerItemAdded(OccurTime, MissionId, MissionState);
		}
		private void HandleEvent_MissionStateManagerItemRemoved(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			HandleDebugMessage("MissionStateManager", $"Item Removed. MissionID: {MissionId}, Info: {MissionState.ToString()}");
			RaiseEvent_MissionStateManagerItemRemoved(OccurTime, MissionId, MissionState);
		}
		private void HandleEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string MissionId, string StateName, IMissionState MissionState)
		{
			HandleDebugMessage("MissionStateManager", $"Item Updated. MissionID: {MissionId}, StateName: {StateName}, Info: {MissionState.ToString()}");
			RaiseEvent_MissionStateManagerItemUpdated(OccurTime, MissionId, StateName, MissionState);
		}
		private void HandleEvent_HostCommunicatorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage("HostCommunicator", $"System Started.");
			RaiseEvent_HostCommunicatorSystemStarted(OccurTime);
		}
		private void HandleEvent_HostCommunicatorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage("HostCommunicator", $"System Stopped.");
			RaiseEvent_HostCommunicatorSystemStopped(OccurTime);
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port)
		{
			HandleDebugMessage("HostCommunicator", $"Local Listen State Changed. State: {NewState.ToString()}, Port: {Port}");
			RaiseEvent_HostCommunicatorLocalListenStateChanged(OccurTime, NewState, Port);
		}
		private void HandleEvent_HostCommunicatorRemoteConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage("HostCommunicator", $"Remote Connect State Changed. IPPort: {IpPort}, State: {NewState}");
			RaiseEvent_HostCommunicatorRemoteConnectStateChanged(OccurTime, IpPort, NewState);
		}
		private void HandleEvent_HostCommunicatorSentString(DateTime OccurTime, string IpPort, string Data)
		{
			HandleDebugMessage("HostCommunicator", $"Sent String. IPPort: {IpPort}, Data: {Data}");
			RaiseEvent_HostCommunicatorSentString(OccurTime, IpPort, Data);
		}
		private void HandleEvent_HostCommunicatorReceivedString(DateTime OccurTime, string IpPort, string Data)
		{
			HandleDebugMessage("HostCommunicator", $"Received String. IPPort: {IpPort}, Data: {Data}");
			RaiseEvent_HostCommunicatorReceivedString(OccurTime, IpPort, Data);
		}
		private void HandleEvent_MissionDispatcherSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage("MissionDispatcher", $"System Started.");
			RaiseEvent_MissionDispatcherSystemStarted(OccurTime);
		}
		private void HandleEvent_MissionDispatcherSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage("MissionDispatcher", $"System Stopped.");
			RaiseEvent_MissionDispatcherSystemStopped(OccurTime);
		}
		private void HandleEvent_MapManagerMapFileAdded(DateTime OccurTime, string MapFileName)
		{
			HandleDebugMessage("MapManager", $"Map File Added. MapFileName: {MapFileName}");
			RaiseEvent_MapManagerMapFileAdded(OccurTime, MapFileName);
		}
		private void HandleEvent_MapManagerMapFileRemoved(DateTime OccurTime, string MapFileName)
		{
			HandleDebugMessage("MapManager", $"Map File Removed. MapFileName: {MapFileName}");
			RaiseEvent_MapManagerMapFileRemoved(OccurTime, MapFileName);
		}
		private void HandleEvent_MapManagerVehicleCurrentMapSynchronized(DateTime OccurTime, IEnumerable<string> VehicleNames, string MapFileName)
		{
			HandleDebugMessage("MapManager", $"Vehicle Current Map Synchronized. VehicleNames: {string.Join(",", VehicleNames)}, MapFileName: {MapFileName}");
			RaiseEvent_MapManagerVehicleCurrentMapSynchronized(OccurTime, VehicleNames, MapFileName);
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
			HandleDebugMessage(OccurTime.ToString(TIME_FORMAT), Category, Message);
		}
		private void HandleDebugMessage(string OccurTime, string Category, string Message)
		{
			RaiseEvent_DebugMessage(OccurTime, Category, Message);
			Console.WriteLine($"{OccurTime} [{Category}] - {Message}");
		}
	}
}
