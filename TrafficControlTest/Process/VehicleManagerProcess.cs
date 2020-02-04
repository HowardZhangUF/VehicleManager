using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Interface;
using TrafficControlTest.Module.InterveneManager.Interface;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Process
{
	public class VehicleManagerProcess
	{
		public event EventHandlerSignificantEvent SignificantEvent;
		public event EventHandlerLogInOutEvent AccessControlUserLogIn;
		public event EventHandlerLogInOutEvent AccessControlUserLogOut;
		public event EventHandlerLocalListenState VehicleCommunicatorLocalListenStateChagned;
		public event EventHandlerItem<IVehicleInfo> VehicleInfoManagerItemAdded;
		public event EventHandlerItem<IVehicleInfo> VehicleInfoManagerItemRemoved;

		public bool mIsAnyUserLoggedIn { get { return (mAccessControl != null && !string.IsNullOrEmpty(mAccessControl.mCurrentUser)); } }
		public string mCurrentLoggedInUserName { get { return (mAccessControl != null && !string.IsNullOrEmpty(mAccessControl.mCurrentUser) ? mAccessControl.mCurrentUser : string.Empty); } }

		private bool mIsAllSystemStopped
		{
			get
			{
				return !mImportantEventRecorder.mIsExecuting
					&& !mVehicleCommunicator.mIsExecuting
					&& !mCollisionEventDetector.mIsExecuting
					&& !mVehicleControlHandler.mIsExecuting
					&& !mHostCommunicator.mIsExecuting
					&& !mMissionDispatcher.mIsExecuting;
			}
		}

		private IConfigurator mConfigurator = null;
		private DatabaseAdapter mDatabaseAdapterOfLogRecord = null;
		private DatabaseAdapter mDatabaseAdapterOfEventRecord = null;
		private DatabaseAdapter mDatabaseAdapterOfData = null;
		private ILogRecorder mLogRecorder = null;
		private IEventRecorder mEventRecorder = null;
		private IImportantEventRecorder mImportantEventRecorder = null;
		private IAccountManager mAccountManager = null;
		private IAccessControl mAccessControl = null;
		private IVehicleCommunicator mVehicleCommunicator = null;
		private IVehicleInfoManager mVehicleInfoManager = null;
		private ICollisionEventManager mCollisionEventManager = null;
		private ICollisionEventDetector mCollisionEventDetector = null;
		private IVehicleControlManager mVehicleControlManager = null;
		private ICollisionEventHandler mCollisionEventHandler = null;
		private IVehicleControlHandler mVehicleControlHandler = null;
		private IVehicleControlUpdater mVehicleControlUpdater = null;
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
			mLogRecorder.Start();
			mEventRecorder.Start();
			mAccountManager.Read();
			mImportantEventRecorder.Start();
			mVehicleCommunicator.StartListen();
			mCollisionEventDetector.Start();
			mVehicleControlHandler.Start();
			mHostCommunicator.StartListen();
			mMissionDispatcher.Start();
		}
		public void Stop()
		{
			if (mIsAnyUserLoggedIn) mAccessControl.LogOut();
			mMissionDispatcher.Stop();
			mHostCommunicator.StopListen();
			mVehicleControlHandler.Stop();
			mCollisionEventDetector.Stop();
			mVehicleCommunicator.StopListen();
			mImportantEventRecorder.Stop();
			mAccountManager.Save();

			DateTime tmp = DateTime.Now;
			while (!mIsAllSystemStopped)
			{
				if (DateTime.Now.Subtract(tmp).TotalSeconds > 5) break;
				System.Threading.Thread.Sleep(100);
			}

			mEventRecorder.Stop();
			mLogRecorder.Stop();
			tmp = DateTime.Now;
			while (mEventRecorder.mIsExecuting || mLogRecorder.mIsExecuting)
			{
				if (DateTime.Now.Subtract(tmp).TotalSeconds > 5) break;
				System.Threading.Thread.Sleep(100);
			}
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
		public DatabaseAdapter GetReferenceOfDatabaseAdapterOfLogRecord()
		{
			return mDatabaseAdapterOfLogRecord;
		}
		public DatabaseAdapter GetReferenceOfDatabaseAdapterOfEventRecord()
		{
			return mDatabaseAdapterOfEventRecord;
		}
		public bool AccessControlLogIn(string Password)
		{
			return mAccessControl.LogIn(Password);
		}
		public bool AccessControlLogOut()
		{
			return mAccessControl.LogOut();
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
		public string[] MapFileManagerGetLocalMapNameList()
		{
			return mMapFileManager.GetLocalMapNameList();
		}
		public string[] MapManagerGetGoalNameList()
		{
			return mMapManager.GetGoalNameList();
		}

		private void Constructor()
		{
			mConfigurator = GenerateIConfigurator("Application.config");
			mDatabaseAdapterOfLogRecord = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Log.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mDatabaseAdapterOfEventRecord = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Event.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mDatabaseAdapterOfData = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Data.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mLogRecorder = GenerateILogRecorder(mDatabaseAdapterOfLogRecord);
			mEventRecorder = GenerateIEventRecorder(mDatabaseAdapterOfEventRecord);
			mAccountManager = GenerateIAccountManager(mDatabaseAdapterOfData);

			SubscribeEvent_Exception();

			UnsubscribeEvent_IAccessControl(mAccessControl);
			mAccessControl = GenerateIAccessControl(mAccountManager);
			SubscribeEvent_IAccessControl(mAccessControl);

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
			mCollisionEventHandler = GenerateICollisionEventHandler(mCollisionEventManager, mVehicleControlManager, mVehicleInfoManager);
			SubscribeEvent_ICollisionEventHandler(mCollisionEventHandler);

			UnsubscribeEvent_IVehicleControlHandler(mVehicleControlHandler);
			mVehicleControlHandler = GenerateIVehicleControlHandler(mVehicleControlManager, mVehicleInfoManager, mVehicleCommunicator);
			SubscribeEvent_IVehicleControlHandler(mVehicleControlHandler);

			UnsubscribeEvent_IVehicleControlUpdater(mVehicleControlUpdater);
			mVehicleControlUpdater = GenerateIVehicleControlUpdater(mVehicleControlManager, mVehicleInfoManager, mVehicleCommunicator);
			SubscribeEvent_IVehicleControlUpdater(mVehicleControlUpdater);

			UnsubscribeEvent_IMissionStateManager(mMissionStateManager);
			mMissionStateManager = GenerateIMissionStateManager();
			SubscribeEvent_IMissionStateManager(mMissionStateManager);

			UnsubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);
			mVehicleInfoUpdater = GenerateIVehicleInfoUpdater(mVehicleCommunicator, mMissionStateManager, mVehicleInfoManager);
			SubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);

			UnsubscribeEvent_IHostCommunicator(mHostCommunicator);
			mHostCommunicator = GenerateIHostCommunicator();
			SubscribeEvent_IHostCommunicator(mHostCommunicator);

			UnsubscribeEvent_IImportantEventRecorder(mImportantEventRecorder);
			mImportantEventRecorder = GenerateIImportantEventRecorder(mEventRecorder, mVehicleInfoManager, mMissionStateManager, mHostCommunicator);
			SubscribeEvent_IImportantEventRecorder(mImportantEventRecorder);

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

			UnsubscribeEvent_IAccessControl(mAccessControl);
			mAccessControl = null;

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

			UnsubscribeEvent_IVehicleControlUpdater(mVehicleControlUpdater);
			mVehicleControlUpdater = null;

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

			UnsubscribeEvent_IImportantEventRecorder(mImportantEventRecorder);
			mImportantEventRecorder = null;

			mAccountManager = null;
			mEventRecorder = null;
			mLogRecorder = null;
			mDatabaseAdapterOfData = null;
			mDatabaseAdapterOfEventRecord = null;
			mDatabaseAdapterOfLogRecord = null;
			mConfigurator = null;
		}
		private void SubscribeEvent_Exception()
		{
			System.Windows.Forms.Application.ThreadException += (sender, e) =>
			{
				System.IO.File.AppendAllText($"Exception{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString(TIME_FORMAT)} - {e.Exception.ToString()}\r\n");
			};
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				System.IO.File.AppendAllText($"Exception{DateTime.Now.ToString("yyyyMMdd")}.txt", $"{DateTime.Now.ToString(TIME_FORMAT)} - {e.ExceptionObject.ToString()}\r\n");
			};
		}
		private void SubscribeEvent_IAccessControl(IAccessControl AccessControl)
		{
			if (AccessControl != null)
			{
				AccessControl.UserLogIn += HandleEvent_AccessControlUserLogIn;
				AccessControl.UserLogOut += HandleEvent_AccessControlUserLogOut;
			}
		}
		private void UnsubscribeEvent_IAccessControl(IAccessControl AccessControl)
		{
			if (AccessControl != null)
			{
				AccessControl.UserLogIn -= HandleEvent_AccessControlUserLogIn;
				AccessControl.UserLogOut -= HandleEvent_AccessControlUserLogOut;
			}
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
				// do nothing
			}
		}
		private void UnsubscribeEvent_ICollisionEventHandler(ICollisionEventHandler CollisionEventHandler)
		{
			if (CollisionEventHandler != null)
			{
				// do nothing
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
		private void SubscribeEvent_IVehicleControlUpdater(IVehicleControlUpdater VehicleControlUpdater)
		{
			if (VehicleControlUpdater != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehicleControlUpdater(IVehicleControlUpdater VehicleControlUpdater)
		{
			if (VehicleControlUpdater != null)
			{
				// do nothing
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
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehicleInfoUpdater(IVehicleInfoUpdater VehicleInfoUpdater)
		{
			if (VehicleInfoUpdater != null)
			{
				// do nothing
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
				// do nothing
			}
		}
		private void UnsubscribeEvent_IHostMessageAnalyzer(IHostMessageAnalyzer HostMessageAnalyzer)
		{
			if (HostMessageAnalyzer != null)
			{
				// do nothing
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
				// do nothing
			}
		}
		private void UnsubscribeEvent_IMissionStateReporter(IMissionStateReporter MissionStateReporter)
		{
			if (MissionStateReporter != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMissionUpdater(IMissionUpdater MissionUpdater)
		{
			if (MissionUpdater != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IMissionUpdater(IMissionUpdater MissionUpdater)
		{
			if (MissionUpdater != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStarted += HandleEvent_ImportantEventRecorderSystemStarted;
				ImportantEventRecorder.SystemStopped += HandleEvent_ImportantEventRecorderSystemStopped;
			}
		}
		private void UnsubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStarted -= HandleEvent_ImportantEventRecorderSystemStarted;
				ImportantEventRecorder.SystemStopped -= HandleEvent_ImportantEventRecorderSystemStopped;
			}
		}
		protected virtual void RaiseEvent_SignificantEvent(DateTime OccurTime, SignificantEventCategory Category, string Info, bool Sync = true)
		{
			RaiseEvent_SignificantEvent(OccurTime.ToString(TIME_FORMAT), Category.ToString(), Info, Sync);
		}
		protected virtual void RaiseEvent_SignificantEvent(string OccurTime, string Category, string Info, bool Sync = true)
		{
			if (Sync)
			{
				SignificantEvent?.Invoke(OccurTime, Category, Info);
			}
			else
			{
				Task.Run(() => { SignificantEvent?.Invoke(OccurTime, Category, Info); });
			}
		}
		protected virtual void RaiseEvent_AccessControlUserLogIn(DateTime OccurTime, string Name, AccountRank Rank, bool Sync = true)
		{
			if (Sync)
			{
				AccessControlUserLogIn?.Invoke(OccurTime, Name, Rank);
			}
			else
			{
				Task.Run(() => { AccessControlUserLogIn?.Invoke(OccurTime, Name, Rank); });
			}
		}
		protected virtual void RaiseEvent_AccessControlUserLogOut(DateTime OccurTime, string Name, AccountRank Rank, bool Sync = true)
		{
			if (Sync)
			{
				AccessControlUserLogOut?.Invoke(OccurTime, Name, Rank);
			}
			else
			{
				Task.Run(() => { AccessControlUserLogOut?.Invoke(OccurTime, Name, Rank); });
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
		private void HandleEvent_AccessControlUserLogIn(DateTime OccurTime, string Name, AccountRank Rank)
		{
			HandleDebugMessage(OccurTime, "AccessControl", "UserLogIn", $"Name: {Name}, Rank: {Rank.ToString()}");
			RaiseEvent_AccessControlUserLogIn(OccurTime, Name, Rank);
		}
		private void HandleEvent_AccessControlUserLogOut(DateTime OccurTime, string Name, AccountRank Rank)
		{
			HandleDebugMessage(OccurTime, "AccessControl", "UserLogOut", $"Name: {Name}, Rank: {Rank.ToString()}");
			RaiseEvent_AccessControlUserLogOut(OccurTime, Name, Rank);
		}
		private void HandleEvent_VehicleCommunicatorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SystemStarted", string.Empty);
		}
		private void HandleEvent_VehicleCommunicatorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SystemStopped", string.Empty);
		}
		private void HandleEvent_VehicleCommunicatorLocalListenStateChagned(DateTime OccurTime, ListenState NewState, int Port)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "LocalListenStateChanged", $"State: {NewState.ToString()}, Port: {Port}");
			RaiseEvent_VehicleCommunicatorLocalListenStateChanged(OccurTime, NewState, Port);
		}
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChagned(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "RemoteConnectStateChanged", $"IPPort: {IpPort}, State: {NewState}");
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SentData", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			// 常態事件不做 General Log 記錄(避免資料庫儲存太多的資訊)，也不使用 Console.WriteLine() 顯示(避免資訊過多)
			if (!(Data is SerialData.AGVStatus) && !(Data is SerialData.AGVPath))
			{
				HandleDebugMessage(OccurTime, "VehicleCommunicator", "ReceivedData", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SentDataSuccessed", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SentDataFailed", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage(OccurTime, "VehicleInfoManager", "ItemAdded", $"Name: {Name}, Info: {VehicleInfo.ToString()}");
			RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.VehicleSystem, $"Vehicle [ {Name} ] Connected");
			RaiseEvent_VehicleInfoManagerItemAdded(OccurTime, Name, VehicleInfo);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage(OccurTime, "VehicleInfoManager", "ItemRemoved", $"Name: {Name}, Info: {VehicleInfo.ToString()}");
			RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.VehicleSystem, $"Vehicle [ {Name} ] Disconnected");
			RaiseEvent_VehicleInfoManagerItemRemoved(OccurTime, Name, VehicleInfo);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo VehicleInfo)
		{
			// 僅有重要的狀態 (CurrentState, CurrentTarget, AlarmMessage, CurrentMissionId, CurrentInterveneCommand, CurrentMapName) 變化時才做 General Log 記錄與使用 Console.WriteLine() 顯示
			if (StateName.Contains("CurrentState") || StateName.Contains("CurrentTarget") || StateName.Contains("AlarmMessage") || StateName.Contains("CurrentMissionId") || StateName.Contains("CurrentInterveneCommand") || StateName.Contains("CurrentMapName"))
			{
				HandleDebugMessage(OccurTime, "VehicleInfoManager", "ItemUpdated", $"Name: {Name}, StateName: {StateName}, Info: {VehicleInfo.ToString()}");
			}
		}
		private void HandleEvent_CollisionEventManagerCollisionEventAdded(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleDebugMessage(OccurTime, "CollisionEventManager", "ItemAdded", $"Name: {Name}, Info:{CollisionPair.ToString()}");
		}
		private void HandleEvent_CollisionEventManagerCollisionEventRemoved(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleDebugMessage(OccurTime, "CollisionEventManager", "ItemRemoved", $"Name: {Name}, Info:{CollisionPair.ToString()}");
		}
		private void HandleEvent_CollisionEventManagerCollisionEventStateUpdated(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			HandleDebugMessage(OccurTime, "CollisionEventManager", "ItemUpdated", $"Name: {Name}, Info:{CollisionPair.ToString()}");
		}
		private void HandleEvent_CollisionEventDetectorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "CollisionEventDetector", "SystemStarted", string.Empty);
		}
		private void HandleEvent_CollisionEventDetectorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "CollisionEventDetector", "SystemStopped", string.Empty);
		}
		private void HandleEvent_VehicleControlManagerItemAdded(DateTime OccurTime, string Name, IVehicleControl VehicleControl)
		{
			HandleDebugMessage(OccurTime, "VehicleControlManager", "ItemAdded", $"Name: {Name}, Info:{VehicleControl.ToString()}");
		}
		private void HandleEvent_VehicleControlManagerItemRemoved(DateTime OccurTime, string Name, IVehicleControl VehicleControl)
		{
			HandleDebugMessage(OccurTime, "VehicleControlManager", "ItemRemoved", $"Name: {Name}, Info:{VehicleControl.ToString()}");
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleControl VehicleControl)
		{
			HandleDebugMessage(OccurTime, "VehicleControlManager", "ItemUpdated", $"Name: {Name}, StateName: {StateName}, Info:{VehicleControl.ToString()}");
		}
		private void HandleEvent_VehicleControlHandlerSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "VehicleControlHandler", "SystemStarted", string.Empty);
		}
		private void HandleEvent_VehicleControlHandlerSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "VehicleControlHandler", "SystemStopped", string.Empty);
		}
		private void HandleEvent_MissionStateManagerItemAdded(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			HandleDebugMessage(OccurTime, "MissionStateManager", "ItemAdded", $"MissionID: {MissionId}, Info: {MissionState.ToString()}");
			RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.MissionSystem, $"Mission [ {MissionState.GetMissionId()} ] Created");
		}
		private void HandleEvent_MissionStateManagerItemRemoved(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			HandleDebugMessage(OccurTime, "MissionStateManager", "ItemRemoved", $"MissionID: {MissionId}, Info: {MissionState.ToString()}");
		}
		private void HandleEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string MissionId, string StateName, IMissionState MissionState)
		{
			HandleDebugMessage(OccurTime, "MissionStateManager", "ItemUpdated", $"MissionID: {MissionId}, StateName: {StateName}, Info: {MissionState.ToString()}");
			if (StateName.Contains("ExecutionStartTimestamp"))
			{
				RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.MissionSystem, $"Mission [ {MissionState.GetMissionId()} ] Started by Vehicle [ {MissionState.mExecutorId} ]");
			}
			else if (StateName.Contains("ExecutionStopTimestamp"))
			{
				RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.MissionSystem, $"Mission [ {MissionState.GetMissionId()} ] Completed [ {MissionState.mExecuteState.ToString().Replace("Execute", string.Empty)} ] by Vehicle [ {MissionState.mExecutorId} ]");
			}
		}
		private void HandleEvent_HostCommunicatorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "SystemStarted", string.Empty);
		}
		private void HandleEvent_HostCommunicatorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "SystemStopped", string.Empty);
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "LocalListenStateChanged", $"State: {NewState.ToString()}, Port: {Port}");
		}
		private void HandleEvent_HostCommunicatorRemoteConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "RemoteConnectStateChanged", $"IPPort: {IpPort}, State: {NewState}");
			if (NewState == ConnectState.Connected)
			{
				RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.HostSystem, $"Host [ {IpPort} ] Connected");
			}
			else if (NewState == ConnectState.Disconnected)
			{
				RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.HostSystem, $"Host [ {IpPort} ] Disconnected");
			}
		}
		private void HandleEvent_HostCommunicatorSentString(DateTime OccurTime, string IpPort, string Data)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "SentString", $"IPPort: {IpPort}, Data: {Data}");
			RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.HostSystem, $"Sent Message [ {IpPort} ] [ {Data} ]");
		}
		private void HandleEvent_HostCommunicatorReceivedString(DateTime OccurTime, string IpPort, string Data)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "ReceivedString", $"IPPort: {IpPort}, Data: {Data}");
			RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.HostSystem, $"Received Message [ {IpPort} ] [ {Data} ]");
		}
		private void HandleEvent_MissionDispatcherSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "MissionDispatcher", "SystemStarted", string.Empty);
		}
		private void HandleEvent_MissionDispatcherSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "MissionDispatcher", "SystemStopped", string.Empty);
		}
		private void HandleEvent_MapFileManagerMapFileAdded(DateTime OccurTime, string MapFileName)
		{
			HandleDebugMessage(OccurTime, "MapFileManager", "ItemAdded", $"MapFileName: {MapFileName}");
		}
		private void HandleEvent_MapFileManagerMapFileRemoved(DateTime OccurTime, string MapFileName)
		{
			HandleDebugMessage(OccurTime, "MapFileManager", "ItemRemoved", $"MapFileName: {MapFileName}");
		}
		private void HandleEvent_MapFileManagerVehicleCurrentMapSynchronized(DateTime OccurTime, IEnumerable<string> VehicleNames, string MapFileName)
		{
			HandleDebugMessage(OccurTime, "MapFileManager", "VehicleCurrentMapSynchronized", $"VehicleNames: {string.Join(",", VehicleNames)}, MapFileName: {MapFileName}");
		}
		private void HandleEvent_MapManagerMapLoaded(DateTime OccurTime, string MapFileName)
		{
			HandleDebugMessage(OccurTime, "MapManager", "MapLoaded", $"MapName: {MapFileName}");
		}
		private void HandleEvent_ImportantEventRecorderSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "ImportantEventRecorder", "SystemStarted", string.Empty);
		}
		private void HandleEvent_ImportantEventRecorderSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "ImportantEventRecorder", "SystemStopped", string.Empty);
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
		}
	}
}
