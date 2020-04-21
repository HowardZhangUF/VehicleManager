using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Configure;
using TrafficControlTest.Module.CycleMission;
using TrafficControlTest.Module.General.Interface;
using TrafficControlTest.Module.InterveneManager.Interface;
using TrafficControlTest.Module.Log;
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
					&& !mMissionDispatcher.mIsExecuting
					&& !mCycleMissionGenerator.mIsExecuting;
			}
		}

		private IConfigurator mConfigurator = null;
		private DatabaseAdapter mDatabaseAdapterOfLogRecord = null;
		private DatabaseAdapter mDatabaseAdapterOfEventRecord = null;
		private DatabaseAdapter mDatabaseAdapterOfSystemData = null;
		private ILogRecorder mLogRecorder = null;
		private IEventRecorder mEventRecorder = null;
		private IImportantEventRecorder mImportantEventRecorder = null;
		private LogExporter mLogExporter = null;
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
		private ICycleMissionGenerator mCycleMissionGenerator = null;

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
			LoadConfigFileAndUpdateSystemConfig();

			mLogRecorder.Start();
			mEventRecorder.Start();
			mAccountManager.Read();
			mImportantEventRecorder.Start();
			mVehicleCommunicator.StartListen();
			mCollisionEventDetector.Start();
			mVehicleControlHandler.Start();
			mHostCommunicator.StartListen();
			mMissionDispatcher.Start();
			mCycleMissionGenerator.Start();
		}
		public void Stop()
		{
			if (mIsAnyUserLoggedIn) mAccessControl.LogOut();
			mCycleMissionGenerator.Stop();
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

			LoadSystemConfigAndUpdateConfigFile();
		}
		public IConfigurator GetReferenceOfIConfigurator()
		{
			return mConfigurator;
		}
		public LogExporter GetReferenceOfLogExporter()
		{
			return mLogExporter;
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
		public ICycleMissionGenerator GetReferenceOfCycleMissionGenerator()
		{
			return mCycleMissionGenerator;
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
		public int GetCountOfOnlineVehicle()
		{
			return mVehicleInfoManager.mCount;
		}

		private void Constructor()
		{
			mDatabaseAdapterOfLogRecord = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Log.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mDatabaseAdapterOfEventRecord = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Event.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mDatabaseAdapterOfSystemData = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\SystemData.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mLogRecorder = GenerateILogRecorder(mDatabaseAdapterOfLogRecord);
			mEventRecorder = GenerateIEventRecorder(mDatabaseAdapterOfEventRecord);
			mAccountManager = GenerateIAccountManager(mDatabaseAdapterOfSystemData);

			SubscribeEvent_Exception();

			UnsubscribeEvent_IConfigurator(mConfigurator);
			mConfigurator = GenerateIConfigurator("Application.config");
			SubscribeEvent_IConfigurator(mConfigurator);

			UnsubscribeEvent_LogExporter(mLogExporter);
			mLogExporter = GenerateLogExporter();
			mLogExporter.AddDirectoryPaths(new List<string> { ".\\Database", ".\\Map", ".\\Exception", ".\\VMLog" });
			mLogExporter.AddFilePaths(new List<string> { ".\\Application.config" });
			SubscribeEvent_LogExporter(mLogExporter);

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
			mHostMessageAnalyzer = GenerateIHostMessageAnalyzer(mHostCommunicator, mVehicleInfoManager, mMissionStateManager, GetMissionAnalyzers());
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

			UnsubscribeEvent_ICycleMissionGenerator(mCycleMissionGenerator);
			mCycleMissionGenerator = GenerateICycleMissionGenerator(mVehicleInfoManager, mMissionStateManager);
			SubscribeEvent_ICycleMissionGenerator(mCycleMissionGenerator);
		}
		private void Destructor()
		{
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

			UnsubscribeEvent_ICycleMissionGenerator(mCycleMissionGenerator);
			mCycleMissionGenerator = null;

			UnsubscribeEvent_IImportantEventRecorder(mImportantEventRecorder);
			mImportantEventRecorder = null;

			UnsubscribeEvent_LogExporter(mLogExporter);
			mLogExporter = null;

			UnsubscribeEvent_IConfigurator(mConfigurator);
			mConfigurator = null;

			mAccountManager = null;
			mEventRecorder = null;
			mLogRecorder = null;
			mDatabaseAdapterOfSystemData = null;
			mDatabaseAdapterOfEventRecord = null;
			mDatabaseAdapterOfLogRecord = null;
		}
		private void LoadConfigFileAndUpdateSystemConfig()
		{
			mConfigurator.Load();
			mImportantEventRecorder.SetConfig("TimePeriod", mConfigurator.GetValue("ImportantEventRecorder/TimePeriod"));
			mVehicleCommunicator.SetConfig("ListenPort", mConfigurator.GetValue("VehicleCommunicator/ListenPort"));
			mVehicleCommunicator.SetConfig("TimePeriod", mConfigurator.GetValue("VehicleCommunicator/TimePeriod"));
			mCollisionEventDetector.SetConfig("TimePeriod", mConfigurator.GetValue("CollisionEventDetector/TimePeriod"));
			mVehicleControlHandler.SetConfig("TimePeriod", mConfigurator.GetValue("VehicleControlHandler/TimePeriod"));
			mHostCommunicator.SetConfig("ListenPort", mConfigurator.GetValue("HostCommunicator/ListenPort"));
			mHostCommunicator.SetConfig("TimePeriod", mConfigurator.GetValue("HostCommunicator/TimePeriod"));
			mMissionDispatcher.SetConfig("TimePeriod", mConfigurator.GetValue("MissionDispatcher/TimePeriod"));
			mMapFileManager.SetConfig("MapFileDirectory", mConfigurator.GetValue("MapFileManager/MapFileDirectory"));
			mMapManager.SetConfig("AutoLoadMap", mConfigurator.GetValue("MapManager/AutoLoadMap"));
			mCycleMissionGenerator.SetConfig("TimePeriod", mConfigurator.GetValue("CycleMissionGenerator/TimePeriod"));
		}
		private void LoadSystemConfigAndUpdateConfigFile()
		{
			mConfigurator.SetValue("CycleMissionGenerator/TimePeriod", mCycleMissionGenerator.GetConfig("TimePeriod"));
			mConfigurator.SetValue("MapManager/AutoLoadMap", mMapManager.GetConfig("AutoLoadMap"));
			mConfigurator.SetValue("MapFileManager/MapFileDirectory", mMapFileManager.GetConfig("MapFileDirectory"));
			mConfigurator.SetValue("MissionDispatcher/TimePeriod", mMissionDispatcher.GetConfig("TimePeriod"));
			mConfigurator.SetValue("HostCommunicator/TimePeriod", mHostCommunicator.GetConfig("TimePeriod"));
			mConfigurator.SetValue("HostCommunicator/ListenPort", mHostCommunicator.GetConfig("ListenPort"));
			mConfigurator.SetValue("VehicleControlHandler/TimePeriod", mVehicleControlHandler.GetConfig("TimePeriod"));
			mConfigurator.SetValue("CollisionEventDetector/TimePeriod", mCollisionEventDetector.GetConfig("TimePeriod"));
			mConfigurator.SetValue("VehicleCommunicator/TimePeriod", mVehicleCommunicator.GetConfig("TimePeriod"));
			mConfigurator.SetValue("VehicleCommunicator/ListenPort", mVehicleCommunicator.GetConfig("ListenPort"));
			mConfigurator.SetValue("ImportantEventRecorder/TimePeriod", mImportantEventRecorder.GetConfig("TimePeriod"));
			mConfigurator.Save();
		}
		private void SubscribeEvent_Exception()
		{
			System.Windows.Forms.Application.ThreadException += (sender, e) =>
			{
				string directory = ".\\Exception";
				string file = $".\\Exception\\Exception{DateTime.Now.ToString("yyyyMMdd")}.txt";
				string message = $"{DateTime.Now.ToString(TIME_FORMAT)} - [ThreadException] - {e.Exception.ToString()}\r\n";

				if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
				if (!System.IO.File.Exists(file)) System.IO.File.Create(file);
				System.IO.File.AppendAllText(file, message);
			};
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				string directory = ".\\Exception";
				string file = $".\\Exception\\Exception{DateTime.Now.ToString("yyyyMMdd")}.txt";
				string message = $"{DateTime.Now.ToString(TIME_FORMAT)} - [UnhandledException] - {e.ExceptionObject.ToString()}\r\n";

				if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
				if (!System.IO.File.Exists(file)) System.IO.File.Create(file);
				System.IO.File.AppendAllText(file, message);
			};
		}
		private void SubscribeEvent_IConfigurator(IConfigurator Configurator)
		{
			if (Configurator != null)
			{
				Configurator.ConfigLoaded += HandleEvent_ConfiguratorConfigLoaded;
				Configurator.ConfigSaved += HandleEvent_ConfiguratorConfigSaved;
				Configurator.ConfigUpdated += HandleEvent_ConfiguratorConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IConfigurator(IConfigurator Configurator)
		{
			if (Configurator != null)
			{
				Configurator.ConfigLoaded -= HandleEvent_ConfiguratorConfigLoaded;
				Configurator.ConfigSaved -= HandleEvent_ConfiguratorConfigSaved;
				Configurator.ConfigUpdated -= HandleEvent_ConfiguratorConfigUpdated;
			}
		}
		private void SubscribeEvent_LogExporter(LogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ExportStarted += HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted += HandleEvent_LogExporterExportCompleted;
			}
		}
		private void UnsubscribeEvent_LogExporter(LogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ExportStarted -= HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted -= HandleEvent_LogExporterExportCompleted;
			}
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
				VehicleCommunicator.ConfigUpdated += HandleEvent_VehicleCommunicatorConfigUpdated;
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
				VehicleCommunicator.ConfigUpdated -= HandleEvent_VehicleCommunicatorConfigUpdated;
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
				CollisionEventDetector.ConfigUpdated += HandleEvent_CollisionEventDetectorConfigUpdated;
			}
		}
		private void UnsubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStarted -= HandleEvent_CollisionEventDetectorSystemStarted;
				CollisionEventDetector.SystemStopped -= HandleEvent_CollisionEventDetectorSystemStopped;
				CollisionEventDetector.ConfigUpdated -= HandleEvent_CollisionEventDetectorConfigUpdated;
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
				VehicleControlHandler.ConfigUpdated += HandleEvent_VehicleControlHandlerConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStarted -= HandleEvent_VehicleControlHandlerSystemStarted;
				VehicleControlHandler.SystemStopped -= HandleEvent_VehicleControlHandlerSystemStopped;
				VehicleControlHandler.ConfigUpdated -= HandleEvent_VehicleControlHandlerConfigUpdated;
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
				HostCommunicator.ConfigUpdated += HandleEvent_HostCommunicatorConfigUpdated;
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
				HostCommunicator.ConfigUpdated -= HandleEvent_HostCommunicatorConfigUpdated;
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
				MissionDispatcher.ConfigUpdated += HandleEvent_MissionDispatcherConfigUpdated;
				MissionDispatcher.MissionDispatched += HandleEvent_MissionDispatcherMissionDispatched;
			}
		}
		private void UnsubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStarted -= HandleEvent_MissionDispatcherSystemStarted;
				MissionDispatcher.SystemStopped -= HandleEvent_MissionDispatcherSystemStopped;
				MissionDispatcher.ConfigUpdated -= HandleEvent_MissionDispatcherConfigUpdated;
				MissionDispatcher.MissionDispatched -= HandleEvent_MissionDispatcherMissionDispatched;
			}
		}
		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.ConfigUpdated += HandleEvent_MapFileManagerConfigUpdated;
				MapFileManager.MapFileAdded += HandleEvent_MapFileManagerMapFileAdded;
				MapFileManager.MapFileRemoved += HandleEvent_MapFileManagerMapFileRemoved;
				MapFileManager.VehicleCurrentMapSynchronized += HandleEvent_MapFileManagerVehicleCurrentMapSynchronized;
			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.ConfigUpdated -= HandleEvent_MapFileManagerConfigUpdated;
				MapFileManager.MapFileAdded -= HandleEvent_MapFileManagerMapFileAdded;
				MapFileManager.MapFileRemoved -= HandleEvent_MapFileManagerMapFileRemoved;
				MapFileManager.VehicleCurrentMapSynchronized -= HandleEvent_MapFileManagerVehicleCurrentMapSynchronized;
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.ConfigUpdated += HandleEvent_MapManagerConfigUpdated;
				MapManager.MapLoaded += HandleEvent_MapManagerMapLoaded;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.ConfigUpdated -= HandleEvent_MapManagerConfigUpdated;
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
		private void SubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStarted += HandleEvent_CycleMissionGeneratorSystemStarted;
				CycleMissionGenerator.SystemStopped += HandleEvent_CycleMissionGeneratorSystemStopped;
				CycleMissionGenerator.ConfigUpdated += HandleEvent_CycleMissionGeneratorConfigUpdated;
				CycleMissionGenerator.CycleMissionAssigned += HandleEvent_CycleMissionGeneratorCycleMissionAssigned;
				CycleMissionGenerator.CycleMissionRemoved += HandleEvent_CycleMissionGeneratorCycleMissionRemoved;
				CycleMissionGenerator.CycleMissionIndexUpdated += HandleEvent_CycleMissionGeneratorCycleMissionIndexUpdated;
			}
		}
		private void UnsubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStarted -= HandleEvent_CycleMissionGeneratorSystemStarted;
				CycleMissionGenerator.SystemStopped -= HandleEvent_CycleMissionGeneratorSystemStopped;
				CycleMissionGenerator.ConfigUpdated -= HandleEvent_CycleMissionGeneratorConfigUpdated;
				CycleMissionGenerator.CycleMissionAssigned -= HandleEvent_CycleMissionGeneratorCycleMissionAssigned;
				CycleMissionGenerator.CycleMissionRemoved -= HandleEvent_CycleMissionGeneratorCycleMissionRemoved;
				CycleMissionGenerator.CycleMissionIndexUpdated -= HandleEvent_CycleMissionGeneratorCycleMissionIndexUpdated;
			}
		}
		private void SubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStarted += HandleEvent_ImportantEventRecorderSystemStarted;
				ImportantEventRecorder.SystemStopped += HandleEvent_ImportantEventRecorderSystemStopped;
				ImportantEventRecorder.ConfigUpdated += HandleEvent_ImportantEventRecorderConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStarted -= HandleEvent_ImportantEventRecorderSystemStarted;
				ImportantEventRecorder.SystemStopped -= HandleEvent_ImportantEventRecorderSystemStopped;
				ImportantEventRecorder.ConfigUpdated -= HandleEvent_ImportantEventRecorderConfigUpdated;
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
		private void HandleEvent_ConfiguratorConfigLoaded(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "Configurator", "ConfigLoaded", string.Empty);
		}
		private void HandleEvent_ConfiguratorConfigSaved(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "Configurator", "ConfigSaved", string.Empty);
		}
		private void HandleEvent_ConfiguratorConfigUpdated(DateTime OccurTime, string Name, Configuration Item)
		{
			HandleDebugMessage(OccurTime, "Configurator", "ConfigUpdated", $"Name: {Name}, NewValue: {Item.mValue}");
			switch (Name)
			{
				case "ImportantEventRecorder/TimePeriod":
					mImportantEventRecorder.SetConfig("TimePeriod", mConfigurator.GetValue("ImportantEventRecorder/TimePeriod"));
					break;
				case "VehicleCommunicator/ListenPort":
					mVehicleCommunicator.SetConfig("ListenPort", mConfigurator.GetValue("VehicleCommunicator/ListenPort"));
					break;
				case "VehicleCommunicator/TimePeriod":
					mVehicleCommunicator.SetConfig("TimePeriod", mConfigurator.GetValue("VehicleCommunicator/TimePeriod"));
					break;
				case "CollisionEventDetector/TimePeriod":
					mCollisionEventDetector.SetConfig("TimePeriod", mConfigurator.GetValue("CollisionEventDetector/TimePeriod"));
					break;
				case "VehicleControlHandler/TimePeriod":
					mVehicleControlHandler.SetConfig("TimePeriod", mConfigurator.GetValue("VehicleControlHandler/TimePeriod"));
					break;
				case "HostCommunicator/ListenPort":
					mHostCommunicator.SetConfig("ListenPort", mConfigurator.GetValue("HostCommunicator/ListenPort"));
					break;
				case "HostCommunicator/TimePeriod":
					mHostCommunicator.SetConfig("TimePeriod", mConfigurator.GetValue("HostCommunicator/TimePeriod"));
					break;
				case "MissionDispatcher/TimePeriod":
					mMissionDispatcher.SetConfig("TimePeriod", mConfigurator.GetValue("MissionDispatcher/TimePeriod"));
					break;
				case "MapFileManager/MapFileDirectory":
					mMapFileManager.SetConfig("MapFileDirectory", mConfigurator.GetValue("MapFileManager/MapFileDirectory"));
					break;
				case "MapManager/AutoLoadMap":
					mMapManager.SetConfig("AutoLoadMap", mConfigurator.GetValue("MapManager/AutoLoadMap"));
					break;
				case "CycleMissionGenerator/TimePeriod":
					mCycleMissionGenerator.SetConfig("TimePeriod", mConfigurator.GetValue("CycleMissionGenerator/TimePeriod"));
					break;
			}
		}
		private void HandleEvent_LogExporterExportStarted(DateTime OccurTime, string DirectoryPath, List<string> Items)
		{
			HandleDebugMessage(OccurTime, "LogExporter", "ExportStarted", $"Items: {string.Join(",", Items)}");
		}
		private void HandleEvent_LogExporterExportCompleted(DateTime OccurTime, string DirectoryPath, List<string> Items)
		{
			HandleDebugMessage(OccurTime, "LogExporter", "ExportCompleted", $"Items: {string.Join(",", Items)}");
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
		private void HandleEvent_VehicleCommunicatorConfigUpdated(DateTime OccurTime, string ConfigName, string NewValue)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "ConfigUpdated", $"ConfigName: {ConfigName}, NewValue: {NewValue}");
		}
		private void HandleEvent_VehicleCommunicatorLocalListenStateChagned(DateTime OccurTime, ListenState NewState, int Port)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "LocalListenStateChanged", $"State: {NewState.ToString()}, Port: {Port}");
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
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage(OccurTime, "VehicleInfoManager", "ItemRemoved", $"Name: {Name}, Info: {VehicleInfo.ToString()}");
			RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.VehicleSystem, $"Vehicle [ {Name} ] Disconnected");
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
		private void HandleEvent_CollisionEventDetectorConfigUpdated(DateTime OccurTime, string ConfigName, string NewValue)
		{
			HandleDebugMessage(OccurTime, "CollisionEventDetector", "ConfigUpdated", $"ConfigName: {ConfigName}, NewValue: {NewValue}");
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
		private void HandleEvent_VehicleControlHandlerConfigUpdated(DateTime OccurTime, string ConfigName, string NewValue)
		{
			HandleDebugMessage(OccurTime, "VehicleControlHandler", "ConfigUpdated", $"ConfigName: {ConfigName}, NewValue: {NewValue}");
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
		private void HandleEvent_HostCommunicatorConfigUpdated(DateTime OccurTime, string ConfigName, string NewValue)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "ConfigUpdated", $"ConfigName: {ConfigName}, NewValue: {NewValue}");
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
		private void HandleEvent_MissionDispatcherConfigUpdated(DateTime OccurTime, string ConfigName, string NewValue)
		{
			HandleDebugMessage(OccurTime, "MissionDispatcher", "ConfigUpdated", $"ConfigName: {ConfigName}, NewValue: {NewValue}");
		}
		private void HandleEvent_MissionDispatcherMissionDispatched(DateTime OccurTime, IMissionState MissionState, IVehicleInfo VehicleInfo)
		{
			HandleDebugMessage(OccurTime, "MissionDispatcher", "MissionDispatched", $"MissionName: {MissionState.mName} Dispatched To VehicleName: {VehicleInfo.mName}");
		}
		private void HandleEvent_MapFileManagerConfigUpdated(DateTime OccurTime, string ConfigName, string NewValue)
		{
			HandleDebugMessage(OccurTime, "MapFileManager", "ConfigUpdated", $"ConfigName: {ConfigName}, NewValue: {NewValue}");
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
		private void HandleEvent_MapManagerConfigUpdated(DateTime OccurTime, string ConfigName, string NewValue)
		{
			HandleDebugMessage(OccurTime, "MapManager", "ConfigUpdated", $"ConfigName: {ConfigName}, NewValue: {NewValue}");
		}
		private void HandleEvent_MapManagerMapLoaded(DateTime OccurTime, string MapFileName)
		{
			HandleDebugMessage(OccurTime, "MapManager", "MapLoaded", $"MapName: {MapFileName}");
		}
		private void HandleEvent_CycleMissionGeneratorSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "CycleMissionGenerator", "SystemStarted", string.Empty);
		}
		private void HandleEvent_CycleMissionGeneratorSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "CycleMissionGenerator", "SystemStopped", string.Empty);
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionAssigned(DateTime OccurTime, string VehicleId)
		{
			HandleDebugMessage(OccurTime, "CycleMissionGenerator", "CycleMissionAssigned", $"VehicleID: {VehicleId}");
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionRemoved(DateTime OccurTime, string VehicleId)
		{
			HandleDebugMessage(OccurTime, "CycleMissionGenerator", "CycleMissionRemoved", $"VehicleID: {VehicleId}");
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionIndexUpdated(DateTime OccurTime, string VehicleId, int Index)
		{
			HandleDebugMessage(OccurTime, "CycleMissionGenerator", "CycleMissionIndexUpdated", $"VehicleID: {VehicleId}, Index: {Index.ToString()}");
		}
		private void HandleEvent_CycleMissionGeneratorConfigUpdated(DateTime OccurTime, string ConfigName, string NewValue)
		{
			HandleDebugMessage(OccurTime, "CycleMissionGenerator", "ConfigUpdated", $"ConfigName: {ConfigName}, NewValue: {NewValue}");
		}
		private void HandleEvent_ImportantEventRecorderSystemStarted(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "ImportantEventRecorder", "SystemStarted", string.Empty);
		}
		private void HandleEvent_ImportantEventRecorderSystemStopped(DateTime OccurTime)
		{
			HandleDebugMessage(OccurTime, "ImportantEventRecorder", "SystemStopped", string.Empty);
		}
		private void HandleEvent_ImportantEventRecorderConfigUpdated(DateTime OccurTime, string ConfigName, string NewValue)
		{
			HandleDebugMessage(OccurTime, "ImportantEventRecorder", "ConfigUpdated", $"ConfigName: {ConfigName}, NewValue: {NewValue}");
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
