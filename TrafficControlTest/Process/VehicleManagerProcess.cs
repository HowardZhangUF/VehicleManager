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

namespace TrafficControlTest.Process
{
	public class VehicleManagerProcess
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
		public event EventHandlerMapFileName MapFileManagerMapFileAdded;
		public event EventHandlerMapFileName MapFileManagerMapFileRemoved;
		public event EventHandlerVehicleNamesMapFileName MapFileManagerVehicleCurrentMapSynchronized;
		public event EventHandlerMapFileName MapManagerMapLoaded;

		private IConfigurator mConfigurator = null;
		private DatabaseAdapter mDatabaseAdapter = null;
		private ILogRecorder mLogRecorder = null;
		private IVehicleCommunicator mVehicleCommunicator = null;
		private IVehicleInfoManager mVehicleInfoManager = null;
		private ICollisionEventManager mCollisionEventManager = null;
		private ICollisionEventDetector mCollisionEventDetector = null;
		private IVehicleControlManager mVehicleControlManager = null;
		private ICollisionEventHandler mCollisionEventHandler = null;
		private IVehicleControlHandler mVehicleControlHandler = null;
		private IMissionStateManager mMissionStateManager = null;
		private IVehicleInfoUpdater mVehicleInfoUpdater = null;
		private IHostCommunicator mHostCommunicator = null;
		private IHostMessageAnalyzer mHostMessageAnalyzer = null;
		private IMissionDispatcher mMissionDispatcher = null;
		private IMapFileManager mMapFileManager = null;
		private IMapManager mMapManager = null;
		private IMissionStateReporter mMissionStateReporter = null;
		private IMissionUpdater mMissionUpdater = null;

		public VehicleManagerProcess()
		{
			Constructor();
		}
		~VehicleManagerProcess()
		{
			Destructor();
		}
		/// <summary>
		/// 系統開始
		/// </summary>
		/// <remarks>
		/// 系統開始不放在建構式，而是需要外部(介面層)呼叫才開始的原因為：為了讓外部(介面層)訂閱完此物件的所有事件，再讓此物件開始執行，避免外部(介面層)遺漏事件
		/// </remarks>
		public void Start()
		{
			LogRecorderStart();
			VehicleCommunicatorStartListen();
			CollisionEventDetectorStart();
			VehicleControlHandlerStart();
			HostCommunicatorStartListen();
			MissionDispatcherStart();
		}
		public void Stop()
		{
			MissionDispatcherStop();
			HostCommunicatorStopListen();
			VehicleControlHandlerStop();
			CollisionEventDetectorStop();
			VehicleCommunicatorStopListen();
			LogRecorderStop();
		}
		public IVehicleCommunicator GetReferenceOfIVehicleCommunicator()
		{
			return mVehicleCommunicator;
		}
		public IVehicleInfoManager GetReferenceOfIVehicleInfoManager()
		{
			return mVehicleInfoManager;
		}
		public ICollisionEventManager GetReferenceOfICollisionEventManager()
		{
			return mCollisionEventManager;
		}
		public IMissionStateManager GetReferenceOfIMissionStateManager()
		{
			return mMissionStateManager;
		}
		public IMapFileManager GetReferenceOfIMapFileManager()
		{
			return mMapFileManager;
		}
		public IMapManager GetReferenceOfIMapManager()
		{
			return mMapManager;
		}
		public DatabaseAdapter GetReferenceOfDatabaseAdapter()
		{
			return mDatabaseAdapter;
		}
		public void LogRecorderStart()
		{
			mLogRecorder.Start();
		}
		public void LogRecorderStop()
		{
			mLogRecorder.Stop();
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
					mVehicleCommunicator.SendSerializableData_UploadMapToAGV(vehicleIpPort, mMapFileManager.GetMapFileFullPath(Paras[0]));
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
		public void MapFileManagerSetConfigOfMapFileDirectory(string MapFileDirectory)
		{
			mMapFileManager.SetConfigOfMapFileDirectory(MapFileDirectory);
		}
		public string MapFileManagerGetConfigOfMapFileDirectory()
		{
			return mMapFileManager.GetConfigOfMapFileDirectory();
		}
		public string[] MapFileManagerGetLocalMapNameList()
		{
			return mMapFileManager.GetLocalMapNameList();
		}
		public void MapManagerSetConfigOfAutoLoadMap(bool Enable)
		{
			mMapManager.SetConfigOfAutoLoadMap(Enable);
		}
		public bool MapManagerGetConfigOfAutoLoadMap()
		{
			return mMapManager.GetConfigOfAutoLoadMap();
		}
		public string[] MapManagerGetGoalNameList()
		{
			return mMapManager.GetGoalNameList();
		}

		private void Constructor()
		{
			mConfigurator = GenerateIConfigurator("Application.config");
			mDatabaseAdapter = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Log.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mLogRecorder = GenerateILogRecorder(mDatabaseAdapter);

			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = GenerateIVehicleCommunicator();
			SubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = GenerateIVehicleInfoManager();
			SubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);

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

			UnsubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);
			mVehicleInfoUpdater = GenerateIVehicleInfoUpdater(mVehicleCommunicator, mMissionStateManager, mVehicleInfoManager);
			SubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);

			UnsubscribeEvent_IHostCommunicator(mHostCommunicator);
			mHostCommunicator = GenerateIHostCommunicator();
			SubscribeEvent_IHostCommunicator(mHostCommunicator);

			UnsubscribeEvent_IHostMessageAnalyzer(mHostMessageAnalyzer);
			mHostMessageAnalyzer = GenerateIHostMessageAnalyzer(mHostCommunicator, mMissionStateManager, GetMissionAnalyzers());
			SubscribeEvent_IHostMessageAnalyzer(mHostMessageAnalyzer);

			UnsubscribeEvent_IMissionDispatcher(mMissionDispatcher);
			mMissionDispatcher = GenerateIMissionDispatcher(mMissionStateManager, mVehicleInfoManager, mVehicleCommunicator);
			SubscribeEvent_IMissionDispatcher(mMissionDispatcher);

			UnsubscribeEvent_IMapFileManager(mMapFileManager);
			mMapFileManager = GenerateIMapFileManager(mVehicleCommunicator, mVehicleInfoManager);
			SubscribeEvent_IMapFileManager(mMapFileManager);

			UnsubscribeEvent_IMapManager(mMapManager);
			mMapManager = GenerateIMapManager(mVehicleInfoManager, mMapFileManager);
			SubscribeEvent_IMapManager(mMapManager);

			UnsubscribeEvent_IMissionStateReporter(mMissionStateReporter);
			mMissionStateReporter = GenerateIMissionStateReporter(mMissionStateManager, mHostCommunicator);
			SubscribeEvent_IMissionStateReporter(mMissionStateReporter);

			UnsubscribeEvent_IMissionUpdater(mMissionUpdater);
			mMissionUpdater = GenerateIMissionUpdater(mVehicleCommunicator, mVehicleInfoManager, mMissionStateManager, mMapManager);
			SubscribeEvent_IMissionUpdater(mMissionUpdater);

			mConfigurator.Load();
			mVehicleCommunicator.SetConfigOfListenPort(int.Parse(mConfigurator.GetValue("VehicleCommunicator/ListenPort")));
			mHostCommunicator.SetConfigOfListenPort(int.Parse(mConfigurator.GetValue("HostCommunicator/ListenPort")));
			mMapFileManager.SetConfigOfMapFileDirectory(mConfigurator.GetValue("MapFileManager/MapFileDirectory"));
			mMapManager.SetConfigOfAutoLoadMap(bool.Parse(mConfigurator.GetValue("MapManager/AutoLoadMap")));
		}
		private void Destructor()
		{
			mConfigurator.SetValue("MapManager/AutoLoadMap", mMapManager.GetConfigOfAutoLoadMap().ToString());
			mConfigurator.SetValue("MapFileManager/MapFileDirectory", mMapFileManager.GetConfigOfMapFileDirectory());
			mConfigurator.SetValue("HostCommunicator/ListenPort", mHostCommunicator.GetConfigOfListenPort().ToString());
			mConfigurator.SetValue("VehicleCommunicator/ListenPort", mVehicleCommunicator.GetConfigOfListenPort().ToString());
			mConfigurator.Save();

			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = null;

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = null;

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

			UnsubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);
			mVehicleInfoUpdater = null;

			UnsubscribeEvent_IHostCommunicator(mHostCommunicator);
			mHostCommunicator = null;

			UnsubscribeEvent_IHostMessageAnalyzer(mHostMessageAnalyzer);
			mHostMessageAnalyzer = null;

			UnsubscribeEvent_IMissionDispatcher(mMissionDispatcher);
			mMissionDispatcher = null;

			UnsubscribeEvent_IMissionStateReporter(mMissionStateReporter);
			mMissionStateReporter = null;

			UnsubscribeEvent_IMissionUpdater(mMissionUpdater);
			mMissionUpdater = null;

			mLogRecorder = null;
			mDatabaseAdapter = null;
			mConfigurator = null;
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
		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.MapFileAdded += HandleEvent_MapFileManagerMapFileAdded;
				MapFileManager.MapFileRemoved += HandleEvent_MapFileManagerMapFileRemoved;
				MapFileManager.VehicleCurrentMapSynchronized += HandleEvent_MapFileManagerVehicleCurrentMapSynchronized;
			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.MapFileAdded -= HandleEvent_MapFileManagerMapFileAdded;
				MapFileManager.MapFileRemoved -= HandleEvent_MapFileManagerMapFileRemoved;
				MapFileManager.VehicleCurrentMapSynchronized -= HandleEvent_MapFileManagerVehicleCurrentMapSynchronized;
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapLoaded += HandleEvent_MapManagerMapLoaded;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapLoaded -= HandleEvent_MapManagerMapLoaded;
			}
		}
		private void SubscribeEvent_IMissionStateReporter(IMissionStateReporter MissionStateReporter)
		{
			if (MissionStateReporter != null)
			{

			}
		}
		private void UnsubscribeEvent_IMissionStateReporter(IMissionStateReporter MissionStateReporter)
		{
			if (MissionStateReporter != null)
			{

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
		protected virtual void RaiseEvent_DebugMessage(string OccurTime, string Category, string SubCategory, string Message, bool Sync = true)
		{
			if (Sync)
			{
				DebugMessage?.Invoke(OccurTime, Category, SubCategory, Message);
			}
			else
			{
				Task.Run(() => { DebugMessage?.Invoke(OccurTime, Category, SubCategory, Message); });
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
		protected virtual void RaiseEvent_MapFileManagerMapFileAdded(DateTime OccurTime, string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapFileManagerMapFileAdded?.Invoke(OccurTime, MapFileName);
			}
			else
			{
				Task.Run(() => { MapFileManagerMapFileAdded?.Invoke(OccurTime, MapFileName); });
			}
		}
		protected virtual void RaiseEvent_MapFileManagerMapFileRemoved(DateTime OccurTime, string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapFileManagerMapFileRemoved?.Invoke(OccurTime, MapFileName);
			}
			else
			{
				Task.Run(() => { MapFileManagerMapFileRemoved?.Invoke(OccurTime, MapFileName); });
			}
		}
		protected virtual void RaiseEvent_MapFileManagerVehicleCurrentMapSynchronized(DateTime OccurTime, IEnumerable<string> VehicleNames, string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapFileManagerVehicleCurrentMapSynchronized?.Invoke(OccurTime, VehicleNames, MapFileName);
			}
			else
			{
				Task.Run(() => { MapFileManagerVehicleCurrentMapSynchronized?.Invoke(OccurTime, VehicleNames, MapFileName); });
			}
		}
		protected virtual void RaiseEvent_MapManagerMapLoaded(DateTime OccurTime, string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapManagerMapLoaded?.Invoke(OccurTime, MapFileName);
			}
			else
			{
				Task.Run(() => { MapManagerMapLoaded?.Invoke(OccurTime, MapFileName); });
			}
		}
		private void HandleEvent_VehicleCommunicatorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SystemStarted", string.Empty);
			RaiseEvent_VehicleCommunicatorSystemStarted(OccurTime);
		}
		private void HandleEvent_VehicleCommunicatorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SystemStopped", string.Empty);
			RaiseEvent_VehicleCommunicatorSystemStopped(OccurTime);
		}
		private void HandleEvent_VehicleCommunicatorLocalListenStateChagned(DateTime OccurTime, ListenState NewState, int Port)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "LocalListenStateChanged", $"State: {NewState.ToString()}, Port: {Port}");
			RaiseEvent_VehicleCommunicatorLocalListenStateChanged(OccurTime, NewState, Port);
		}
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChagned(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "RemoteConnectStateChanged", $"IPPort: {IpPort}, State: {NewState}");
			RaiseEvent_VehicleCommunicatorRemoteConnectStateChagned(OccurTime, IpPort, NewState);
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SentData", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
			RaiseEvent_VehicleCommunicatorSentSerializableData(OccurTime, IpPort, Data);
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			if (!(Data is SerialData.AGVStatus) && !(Data is SerialData.AGVPath))
			{
				HandleDebugMessage(OccurTime, "VehicleCommunicator", "ReceivedData", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
				RaiseEvent_VehicleCommunicatorReceivedSerializableData(OccurTime, IpPort, Data);
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SentDataSuccessed", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
			RaiseEvent_VehicleCommunicatorSentSerializableDataSuccessed(OccurTime, IpPort, Data);
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SentDataFailed", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
			RaiseEvent_VehicleCommunicatorSentSerializableDataFailed(OccurTime, IpPort, Data);
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage(OccurTime, "VehicleInfoManager", "ItemAdded", $"Name: {Name}, Info: {VehicleInfo.ToString()}");
			mLogRecorder.RecordVehicleInfo(DatabaseDataOperation.Add, VehicleInfo);
			RaiseEvent_VehicleInfoManagerItemAdded(OccurTime, Name, VehicleInfo);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage(OccurTime, "VehicleInfoManager", "ItemRemoved", $"Name: {Name}, Info: {VehicleInfo.ToString()}");
			mLogRecorder.RecordVehicleInfo(DatabaseDataOperation.Remove, VehicleInfo);
			RaiseEvent_VehicleInfoManagerItemRemoved(OccurTime, Name, VehicleInfo);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage(OccurTime, "VehicleInfoManager", "ItemUpdated", $"Name: {Name}, StateName: {StateName}, Info: {VehicleInfo.ToString()}");
			mLogRecorder.RecordVehicleInfo(DatabaseDataOperation.Update, VehicleInfo);
			RaiseEvent_VehicleInfoManagerItemUpdated(OccurTime, Name, StateName, VehicleInfo);
		}
		private void HandleEvent_CollisionEventManagerCollisionEventAdded(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleDebugMessage(OccurTime, "CollisionEventManager", "ItemAdded", $"Name: {Name}, Info:\n{CollisionPair.ToString()}");
			RaiseEvent_CollisionEventManagerCollisionEventAdded(OccurTime, Name, CollisionPair);
		}
		private void HandleEvent_CollisionEventManagerCollisionEventRemoved(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleDebugMessage(OccurTime, "CollisionEventManager", "ItemRemoved", $"Name: {Name}, Info:\n{CollisionPair.ToString()}");
			RaiseEvent_CollisionEventManagerCollisionEventRemoved(OccurTime, Name, CollisionPair);
		}
		private void HandleEvent_CollisionEventManagerCollisionEventStateUpdated(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleDebugMessage(OccurTime, "CollisionEventManager", "ItemUpdated", $"Name: {Name}, Info:\n{CollisionPair.ToString()}");
			RaiseEvent_CollisionEventManagerCollisionEventStateUpdated(OccurTime, Name, CollisionPair);
		}
		private void HandleEvent_CollisionEventDetectorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "CollisionEventDetector", "SystemStarted", string.Empty);
			RaiseEvent_CollisionEventDetectorSystemStarted(OccurTime);
		}
		private void HandleEvent_CollisionEventDetectorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "CollisionEventDetector", "SystemStopped", string.Empty);
			RaiseEvent_CollisionEventDetectorSystemStopped(OccurTime);
		}
		private void HandleEvent_VehicleControlManagerItemAdded(DateTime OccurTime, string Name, IVehicleControl VehicleControl)
		{
			HandleDebugMessage(OccurTime, "VehicleControlManager", "ItemAdded", $"Name: {Name}, Info:\n{VehicleControl.ToString()}");
			RaiseEvent_VehicleControlManagerItemAdded(OccurTime, Name, VehicleControl);
		}
		private void HandleEvent_VehicleControlManagerItemRemoved(DateTime OccurTime, string Name, IVehicleControl VehicleControl)
		{
			HandleDebugMessage(OccurTime, "VehicleControlManager", "ItemRemoved", $"Name: {Name}, Info:\n{VehicleControl.ToString()}");
			RaiseEvent_VehicleControlManagerItemRemoved(OccurTime, Name, VehicleControl);
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleControl VehicleControl)
		{
			HandleDebugMessage(OccurTime, "VehicleControlManager", "ItemUpdated", $"Name: {Name}, StateName: {StateName}, Info:\n{VehicleControl.ToString()}");
			RaiseEvent_VehicleControlManagerItemUpdated(OccurTime, Name, StateName, VehicleControl);
		}
		private void HandleEvent_VehicleControlHandlerSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "VehicleControlHandler", "SystemStarted", string.Empty);
			RaiseEvent_VehicleControlHandlerSystemStarted(OccurTime);
		}
		private void HandleEvent_VehicleControlHandlerSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "VehicleControlHandler", "SystemStopped", string.Empty);
			RaiseEvent_VehicleControlHandlerSystemStopped(OccurTime);
		}
		private void HandleEvent_MissionStateManagerItemAdded(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			HandleDebugMessage(OccurTime, "MissionStateManager", "ItemAdded", $"MissionID: {MissionId}, Info: {MissionState.ToString()}");
			mLogRecorder.RecordMissionState(DatabaseDataOperation.Add, MissionState);
			RaiseEvent_MissionStateManagerItemAdded(OccurTime, MissionId, MissionState);
		}
		private void HandleEvent_MissionStateManagerItemRemoved(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			HandleDebugMessage(OccurTime, "MissionStateManager", "ItemRemoved", $"MissionID: {MissionId}, Info: {MissionState.ToString()}");
			mLogRecorder.RecordMissionState(DatabaseDataOperation.Remove, MissionState);
			RaiseEvent_MissionStateManagerItemRemoved(OccurTime, MissionId, MissionState);
		}
		private void HandleEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string MissionId, string StateName, IMissionState MissionState)
		{
			HandleDebugMessage(OccurTime, "MissionStateManager", "ItemUpdated", $"MissionID: {MissionId}, StateName: {StateName}, Info: {MissionState.ToString()}");
			mLogRecorder.RecordMissionState(DatabaseDataOperation.Update, MissionState);
			RaiseEvent_MissionStateManagerItemUpdated(OccurTime, MissionId, StateName, MissionState);
		}
		private void HandleEvent_HostCommunicatorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "SystemStarted", string.Empty);
			RaiseEvent_HostCommunicatorSystemStarted(OccurTime);
		}
		private void HandleEvent_HostCommunicatorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "SystemStopped", string.Empty);
			RaiseEvent_HostCommunicatorSystemStopped(OccurTime);
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "LocalListenStateChanged", $"State: {NewState.ToString()}, Port: {Port}");
			RaiseEvent_HostCommunicatorLocalListenStateChanged(OccurTime, NewState, Port);
		}
		private void HandleEvent_HostCommunicatorRemoteConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "RemoteConnectStateChanged", $"IPPort: {IpPort}, State: {NewState}");
			RaiseEvent_HostCommunicatorRemoteConnectStateChanged(OccurTime, IpPort, NewState);
		}
		private void HandleEvent_HostCommunicatorSentString(DateTime OccurTime, string IpPort, string Data)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "SentString", $"IPPort: {IpPort}, Data: {Data}");
			RaiseEvent_HostCommunicatorSentString(OccurTime, IpPort, Data);
		}
		private void HandleEvent_HostCommunicatorReceivedString(DateTime OccurTime, string IpPort, string Data)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "ReceivedString", $"IPPort: {IpPort}, Data: {Data}");
			RaiseEvent_HostCommunicatorReceivedString(OccurTime, IpPort, Data);
		}
		private void HandleEvent_MissionDispatcherSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "MissionDispatcher", "SystemStarted", string.Empty);
			RaiseEvent_MissionDispatcherSystemStarted(OccurTime);
		}
		private void HandleEvent_MissionDispatcherSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "MissionDispatcher", "SystemStopped", string.Empty);
			RaiseEvent_MissionDispatcherSystemStopped(OccurTime);
		}
		private void HandleEvent_MapFileManagerMapFileAdded(DateTime OccurTime, string MapFileName)
		{
			HandleDebugMessage(OccurTime, "MapFileManager", "ItemAdded", $"MapFileName: {MapFileName}");
			RaiseEvent_MapFileManagerMapFileAdded(OccurTime, MapFileName);
		}
		private void HandleEvent_MapFileManagerMapFileRemoved(DateTime OccurTime, string MapFileName)
		{
			HandleDebugMessage(OccurTime, "MapFileManager", "ItemRemoved", $"MapFileName: {MapFileName}");
			RaiseEvent_MapFileManagerMapFileRemoved(OccurTime, MapFileName);
		}
		private void HandleEvent_MapFileManagerVehicleCurrentMapSynchronized(DateTime OccurTime, IEnumerable<string> VehicleNames, string MapFileName)
		{
			HandleDebugMessage(OccurTime, "MapFileManager", "VehicleCurrentMapSynchronized", $"VehicleNames: {string.Join(",", VehicleNames)}, MapFileName: {MapFileName}");
			RaiseEvent_MapFileManagerVehicleCurrentMapSynchronized(OccurTime, VehicleNames, MapFileName);
		}
		private void HandleEvent_MapManagerMapLoaded(DateTime OccurTime, string MapFileName)
		{
			HandleDebugMessage(OccurTime, "MapManager", "MapLoaded", $"MapName: {MapFileName}");
			RaiseEvent_MapManagerMapLoaded(OccurTime, MapFileName);
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
			HandleDebugMessage(OccurTime.ToString(TIME_FORMAT), Category, SubCategory, Message);
		}
		private void HandleDebugMessage(string OccurTime, string Category, string SubCategory, string Message)
		{
			Console.WriteLine($"{OccurTime} [{Category}] [{SubCategory}] - {Message}");
			mLogRecorder.RecordGeneralLog(OccurTime, Category, SubCategory, Message);
			RaiseEvent_DebugMessage(OccurTime, Category, SubCategory, Message);
		}
	}
}
