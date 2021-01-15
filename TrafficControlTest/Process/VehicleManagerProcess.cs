using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Account;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.ChargeStation;
using TrafficControlTest.Module.CollisionEvent;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.Configure;
using TrafficControlTest.Module.CycleMission;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Log;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.NewCommunication;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.VehiclePassThroughAutomaticDoor;
using static TrafficControlTest.Library.EventHandlerLibrary;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Process
{
	public class VehicleManagerProcess
	{
		public event EventHandler<DebugMessageEventArgs> DebugMessage;
		public event EventHandler<SignificantEventEventArgs> SignificantEvent;
		public event EventHandler<UserLogChangedEventArgs> AccessControlUserLogChanged;
		public event EventHandler<DestructProgressChangedEventArgs> DestructProgressChanged;

		public bool mIsAnyUserLoggedIn { get { return (mAccessControl != null && !string.IsNullOrEmpty(mAccessControl.mCurrentUser)); } }
		public string mCurrentLoggedInUserName { get { return (mAccessControl != null && !string.IsNullOrEmpty(mAccessControl.mCurrentUser) ? mAccessControl.mCurrentUser : string.Empty); } }

		private bool mIsAllSystemStopped
		{
			get
			{
				return !mCollectionOfISystemWithLoopTask.Any(o => o.mIsExecuting);
			}
		}

		private List<ISystemWithConfig> mCollectionOfISystemWithConfig = new List<ISystemWithConfig>();
		private List<ISystemWithLoopTask> mCollectionOfISystemWithLoopTask = new List<ISystemWithLoopTask>();
		private IConfigurator mConfigurator = null;
		private DatabaseAdapter mDatabaseAdapterOfLogRecord = null;
		private DatabaseAdapter mDatabaseAdapterOfEventRecord = null;
		private DatabaseAdapter mDatabaseAdapterOfSystemData = null;
		private ILogRecorder mLogRecorder = null;
		private IEventRecorder mEventRecorder = null;
		private IImportantEventRecorder mImportantEventRecorder = null;
		private ITimeElapseDetector mTimeElapseDetector = null;
		private ILogExporter mLogExporter = null;
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
		private IChargeStationInfoManager mChargeStationInfoManager = null;
		private IMissionDispatcher mMissionDispatcher = null;
		private IMapFileManager mMapFileManager = null;
		private IMapManager mMapManager = null;
		private IMissionStateReporter mMissionStateReporter = null;
		private IMissionUpdater mMissionUpdater = null;
		private ICycleMissionGenerator mCycleMissionGenerator = null;
		private IAutomaticDoorInfoManager mAutomaticDoorInfoManager = null;
		private IAutomaticDoorCommunicator mAutomaticDoorCommunicator = null;
		private IAutomaticDoorInfoManagerUpdater mAutomaticDoorInfoManagerUpdater = null;
		private IAutomaticDoorCommunicatorUpdater mAutomaticDoorCommunicatorUpdater = null;
		private IAutomaticDoorControlManager mAutomaticDoorControlManager = null;
		private IAutomaticDoorControlManagerUpdater mAutomaticDoorControlManagerUpdater = null;
		private IAutomaticDoorControlHandler mAutomaticDoorControlHandler = null;
		private IVehiclePassThroughAutomaticDoorEventManager mVehiclePassThroughAutomaticDoorEventManager = null;
		private IVehiclePassThroughAutomaticDoorEventManagerUpdater mVehiclePassThroughAutomaticDoorEventManagerUpdater = null;
		private IVehiclePassThroughAutomaticDoorEventHandler mVehiclePassThroughAutomaticDoorEventHandler = null;
		private IChargeStationInfoManagerUpdater mChargeStationInfoManagerUpdater = null;

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
			mTimeElapseDetector.Start();
			mVehicleCommunicator.Start();
			mVehicleCommunicator.StartListen();
			mCollisionEventDetector.Start();
			mVehicleControlHandler.Start();
			mVehicleControlUpdater.Start();
			mHostCommunicator.Start();
			mHostCommunicator.StartListen();
			mMissionDispatcher.Start();
			mCycleMissionGenerator.Start();
			mAutomaticDoorCommunicator.Start();
			mAutomaticDoorControlHandler.Start();
			mVehiclePassThroughAutomaticDoorEventManagerUpdater.Start();
		}
		public void Stop()
		{
			if (mIsAnyUserLoggedIn) mAccessControl.LogOut();
			mVehiclePassThroughAutomaticDoorEventManagerUpdater.Stop();
			mAutomaticDoorControlHandler.Stop();
			mAutomaticDoorCommunicator.Stop();
			mCycleMissionGenerator.Stop();
			mMissionDispatcher.Stop();
			mHostCommunicator.StopListen();
			mHostCommunicator.Stop();
			mVehicleControlUpdater.Stop();
			mVehicleControlHandler.Stop();
			mCollisionEventDetector.Stop();
			mVehicleCommunicator.StopListen();
			mVehicleCommunicator.Stop();
			mTimeElapseDetector.Stop();
			mImportantEventRecorder.Stop();
			mAccountManager.Save();

			RaiseEvent_DestructProgressChanged(0);

			DateTime tmp = DateTime.Now;
			while (!mIsAllSystemStopped)
			{
				if (DateTime.Now.Subtract(tmp).TotalSeconds > 5) break;
				RaiseEvent_DestructProgressChanged(GetCurrentThreadClosingProgress());
				System.Threading.Thread.Sleep(100);
			}

			mEventRecorder.Stop();
			mLogRecorder.Stop();
			tmp = DateTime.Now;
			while (mEventRecorder.mIsExecuting || mLogRecorder.mIsExecuting)
			{
				if (DateTime.Now.Subtract(tmp).TotalSeconds > 5) break;
				RaiseEvent_DestructProgressChanged(GetCurrentThreadClosingProgress());
				System.Threading.Thread.Sleep(100);
			}

			RaiseEvent_DestructProgressChanged(100);

			LoadSystemConfigAndUpdateConfigFile();
		}
		public IConfigurator GetReferenceOfIConfigurator()
		{
			return mConfigurator;
		}
		public ILogExporter GetReferenceOfILogExporter()
		{
			return mLogExporter;
		}
		public IImportantEventRecorder GetReferenceOfIImportantEventRecorder()
		{
			return mImportantEventRecorder;
		}
		public ITimeElapseDetector GetReferenceOfITimeElapseDetector()
		{
			return mTimeElapseDetector;
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
		public ICollisionEventDetector GetReferenceOfICollisionEventDetector()
		{
			return mCollisionEventDetector;
		}
		public IVehicleControlManager GetReferenceOfIVehicleControlManager()
		{
			return mVehicleControlManager;
		}
		public IVehicleControlHandler GetReferenceOfIVehicleControlHandler()
		{
			return mVehicleControlHandler;
		}
		public IMissionStateManager GetReferenceOfIMissionStateManager()
		{
			return mMissionStateManager;
		}
		public IHostCommunicator GetReferenceOfIHostCommunicator()
		{
			return mHostCommunicator;
		}
		public IMissionDispatcher GetReferenceOfIMissionDispatcher()
		{
			return mMissionDispatcher;
		}
		public IMissionUpdater GetReferenceOfIMissionUpdater()
		{
			return mMissionUpdater;
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
		public IAutomaticDoorInfoManager GetReferenceOfAutomaticDoorInfoManager()
		{
			return mAutomaticDoorInfoManager;
		}
		public IAutomaticDoorControlManager GetReferenceOfIAutomaticDoorControlManager()
		{
			return mAutomaticDoorControlManager;
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

			UnsubscribeEvent_ILogExporter(mLogExporter);
			mLogExporter = GenerateILogExporter();
			mLogExporter.AddDirectoryPaths(new List<string> { ".\\Database", ".\\Map", ".\\Exception", ".\\VMLog" });
			mLogExporter.AddFilePaths(new List<string> { ".\\Application.config" });
			SubscribeEvent_ILogExporter(mLogExporter);

			UnsubscribeEvent_ITimeElapseDetecotr(mTimeElapseDetector);
			mTimeElapseDetector = GenerateITimeElapseDetector();
			SubscribeEvent_ITimeElapseDetecotr(mTimeElapseDetector);

			UnsubscribeEvent_IAccessControl(mAccessControl);
			mAccessControl = GenerateIAccessControl(mAccountManager);
			SubscribeEvent_IAccessControl(mAccessControl);

			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = GenerateIVehicleCommunicator();
			SubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = GenerateIVehicleInfoManager();
			SubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);

			UnsubscribeEvent_IMapFileManager(mMapFileManager);
			mMapFileManager = GenerateIMapFileManager();
			SubscribeEvent_IMapFileManager(mMapFileManager);

			UnsubscribeEvent_IMapManager(mMapManager);
			mMapManager = GenerateIMapManager(mVehicleCommunicator, mVehicleInfoManager, mMapFileManager);
			SubscribeEvent_IMapManager(mMapManager);

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
			mVehicleControlUpdater = GenerateIVehicleControlUpdater(mVehicleControlManager, mVehicleInfoManager, mVehicleCommunicator, mMapManager);
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

			UnsubscribeEvent_IChargeStationInfoManager(mChargeStationInfoManager);
			mChargeStationInfoManager = GenerateIChargeStationInfoManager();
			SubscribeEvent_IChargeStationInfoManager(mChargeStationInfoManager);

			UnsubscribeEvent_IMissionDispatcher(mMissionDispatcher);
			mMissionDispatcher = GenerateIMissionDispatcher(mMissionStateManager, mVehicleInfoManager, mVehicleControlManager);
			SubscribeEvent_IMissionDispatcher(mMissionDispatcher);

			UnsubscribeEvent_IMissionStateReporter(mMissionStateReporter);
			mMissionStateReporter = GenerateIMissionStateReporter(mMissionStateManager, mHostCommunicator);
			SubscribeEvent_IMissionStateReporter(mMissionStateReporter);

			UnsubscribeEvent_IMissionUpdater(mMissionUpdater);
			mMissionUpdater = GenerateIMissionUpdater(mVehicleInfoManager, mMissionStateManager, mVehicleControlManager);
			SubscribeEvent_IMissionUpdater(mMissionUpdater);

			UnsubscribeEvent_ICycleMissionGenerator(mCycleMissionGenerator);
			mCycleMissionGenerator = GenerateICycleMissionGenerator(mVehicleInfoManager, mMissionStateManager);
			SubscribeEvent_ICycleMissionGenerator(mCycleMissionGenerator);

			UnsubscribeEvent_IAutomaticDoorInfoManager(mAutomaticDoorInfoManager);
			mAutomaticDoorInfoManager = GenerateIAutomaticDoorInfoManager();
			SubscribeEvent_IAutomaticDoorInfoManager(mAutomaticDoorInfoManager);

			UnsubscribeEvent_IAutomaticDoorCommunicator(mAutomaticDoorCommunicator);
			mAutomaticDoorCommunicator = GenerateIAutomaticDoorCommunicator();
			SubscribeEvent_IAutomaticDoorCommunicator(mAutomaticDoorCommunicator);

			UnsubscribeEvent_IAutomaticDoorInfoManagerUpdater(mAutomaticDoorInfoManagerUpdater);
			mAutomaticDoorInfoManagerUpdater = GenerateIAutomaticDoorInfoManagerUpdater(mAutomaticDoorInfoManager, mMapManager, mAutomaticDoorCommunicator);
			SubscribeEvent_IAutomaticDoorInfoManagerUpdater(mAutomaticDoorInfoManagerUpdater);

			UnsubscribeEvent_IAutomaticDoorCommunicatorUpdater(mAutomaticDoorCommunicatorUpdater);
			mAutomaticDoorCommunicatorUpdater = GenerateIAutomaticDoorCommunicatorUpdater(mAutomaticDoorCommunicator, mAutomaticDoorInfoManager);
			SubscribeEvent_IAutomaticDoorCommunicatorUpdater(mAutomaticDoorCommunicatorUpdater);

			UnsubscribeEvent_IAutomaticDoorControlManager(mAutomaticDoorControlManager);
			mAutomaticDoorControlManager = GenerateIAutomaticDoorControlManager();
			SubscribeEvent_IAutomaticDoorControlManager(mAutomaticDoorControlManager);

			UnsubscribeEvent_IAutomaticDoorControlManagerUpdater(mAutomaticDoorControlManagerUpdater);
			mAutomaticDoorControlManagerUpdater = GenerateIAutomaticDoorControlManagerUpdater(mAutomaticDoorControlManager, mAutomaticDoorInfoManager, mAutomaticDoorCommunicator);
			SubscribeEvent_IAutomaticDoorControlManagerUpdater(mAutomaticDoorControlManagerUpdater);

			UnsubscribeEvent_IAutomaticDoorControlHandler(mAutomaticDoorControlHandler);
			mAutomaticDoorControlHandler = GenerateIAutomaticDoorControlHandler(mAutomaticDoorControlManager, mAutomaticDoorInfoManager, mAutomaticDoorCommunicator);
			SubscribeEvent_IAutomaticDoorControlHandler(mAutomaticDoorControlHandler);

			UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(mVehiclePassThroughAutomaticDoorEventManager);
			mVehiclePassThroughAutomaticDoorEventManager = GenerateIVehiclePassThroughAutomaticDoorEventManager();
			SubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(mVehiclePassThroughAutomaticDoorEventManager);

			UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManagerUpdater(mVehiclePassThroughAutomaticDoorEventManagerUpdater);
			mVehiclePassThroughAutomaticDoorEventManagerUpdater = GenerateIVehiclePassThroughAutomaticDoorEventManagerUpdater(mVehicleInfoManager, mAutomaticDoorInfoManager, mVehiclePassThroughAutomaticDoorEventManager);
			SubscribeEvent_IVehiclePassThroughAutomaticDoorEventManagerUpdater(mVehiclePassThroughAutomaticDoorEventManagerUpdater);

			UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventHandler(mVehiclePassThroughAutomaticDoorEventHandler);
			mVehiclePassThroughAutomaticDoorEventHandler = GenerateIVehiclePassThroughAutomaticDoorEventHandler(mVehiclePassThroughAutomaticDoorEventManager, mAutomaticDoorControlManager);
			SubscribeEvent_IVehiclePassThroughAutomaticDoorEventHandler(mVehiclePassThroughAutomaticDoorEventHandler);

			UnsubscribeEvent_IChargeStationInfoManagerUpdater(mChargeStationInfoManagerUpdater);
			mChargeStationInfoManagerUpdater = GenerateIChargeStationInfoManagerUpdater(mChargeStationInfoManager, mMapManager, mVehicleInfoManager);
			SubscribeEvent_IChargeStationInfoManagerUpdater(mChargeStationInfoManagerUpdater);

			mCollectionOfISystemWithConfig.Add(mLogExporter);
			mCollectionOfISystemWithConfig.Add(mImportantEventRecorder);
			mCollectionOfISystemWithConfig.Add(mTimeElapseDetector);
			mCollectionOfISystemWithConfig.Add(mVehicleCommunicator);
			mCollectionOfISystemWithConfig.Add(mCollisionEventDetector);
			mCollectionOfISystemWithConfig.Add(mVehicleControlHandler);
			mCollectionOfISystemWithConfig.Add(mVehicleControlUpdater);
			mCollectionOfISystemWithConfig.Add(mHostCommunicator);
			mCollectionOfISystemWithConfig.Add(mHostMessageAnalyzer);
			mCollectionOfISystemWithConfig.Add(mMissionDispatcher);
			mCollectionOfISystemWithConfig.Add(mMissionUpdater);
			mCollectionOfISystemWithConfig.Add(mMapFileManager);
			mCollectionOfISystemWithConfig.Add(mMapManager);
			mCollectionOfISystemWithConfig.Add(mCycleMissionGenerator);
			mCollectionOfISystemWithConfig.Add(mAutomaticDoorCommunicator);
			mCollectionOfISystemWithConfig.Add(mAutomaticDoorControlHandler);
			mCollectionOfISystemWithConfig.Add(mVehiclePassThroughAutomaticDoorEventManagerUpdater);
			mCollectionOfISystemWithConfig.Add(mChargeStationInfoManagerUpdater);

			mCollectionOfISystemWithLoopTask.Add(mImportantEventRecorder);
			mCollectionOfISystemWithLoopTask.Add(mTimeElapseDetector);
			mCollectionOfISystemWithLoopTask.Add(mVehicleCommunicator);
			mCollectionOfISystemWithLoopTask.Add(mCollisionEventDetector);
			mCollectionOfISystemWithLoopTask.Add(mVehicleControlHandler);
			mCollectionOfISystemWithLoopTask.Add(mVehicleControlUpdater);
			mCollectionOfISystemWithLoopTask.Add(mHostCommunicator);
			mCollectionOfISystemWithLoopTask.Add(mMissionDispatcher);
			mCollectionOfISystemWithLoopTask.Add(mCycleMissionGenerator);
			mCollectionOfISystemWithLoopTask.Add(mAutomaticDoorCommunicator);
			mCollectionOfISystemWithLoopTask.Add(mAutomaticDoorControlHandler);
			mCollectionOfISystemWithLoopTask.Add(mVehiclePassThroughAutomaticDoorEventManagerUpdater);
		}
		private void Destructor()
		{
			mCollectionOfISystemWithLoopTask.Clear();

			mCollectionOfISystemWithConfig.Clear();

			UnsubscribeEvent_IAccessControl(mAccessControl);
			mAccessControl = null;

			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = null;

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = null;

			UnsubscribeEvent_IMapFileManager(mMapFileManager);
			mMapFileManager = null;

			UnsubscribeEvent_IMapManager(mMapManager);
			mMapManager = null;

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

			UnsubscribeEvent_IChargeStationInfoManager(mChargeStationInfoManager);
			mChargeStationInfoManager = null;

			UnsubscribeEvent_IMissionDispatcher(mMissionDispatcher);
			mMissionDispatcher = null;

			UnsubscribeEvent_IMissionStateReporter(mMissionStateReporter);
			mMissionStateReporter = null;

			UnsubscribeEvent_IMissionUpdater(mMissionUpdater);
			mMissionUpdater = null;

			UnsubscribeEvent_ICycleMissionGenerator(mCycleMissionGenerator);
			mCycleMissionGenerator = null;

			UnsubscribeEvent_IAutomaticDoorInfoManager(mAutomaticDoorInfoManager);
			mAutomaticDoorInfoManager = null;

			UnsubscribeEvent_IAutomaticDoorCommunicator(mAutomaticDoorCommunicator);
			mAutomaticDoorCommunicator = null;

			UnsubscribeEvent_IAutomaticDoorInfoManagerUpdater(mAutomaticDoorInfoManagerUpdater);
			mAutomaticDoorInfoManagerUpdater = null;

			UnsubscribeEvent_IAutomaticDoorCommunicatorUpdater(mAutomaticDoorCommunicatorUpdater);
			mAutomaticDoorCommunicatorUpdater = null;

			UnsubscribeEvent_IAutomaticDoorControlManager(mAutomaticDoorControlManager);
			mAutomaticDoorControlManager = null;

			UnsubscribeEvent_IAutomaticDoorControlManagerUpdater(mAutomaticDoorControlManagerUpdater);
			mAutomaticDoorControlManagerUpdater = null;

			UnsubscribeEvent_IAutomaticDoorControlHandler(mAutomaticDoorControlHandler);
			mAutomaticDoorControlHandler = null;

			UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(mVehiclePassThroughAutomaticDoorEventManager);
			mVehiclePassThroughAutomaticDoorEventManager = null;

			UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManagerUpdater(mVehiclePassThroughAutomaticDoorEventManagerUpdater);
			mVehiclePassThroughAutomaticDoorEventManagerUpdater = null;

			UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventHandler(mVehiclePassThroughAutomaticDoorEventHandler);
			mVehiclePassThroughAutomaticDoorEventHandler = null;

			UnsubscribeEvent_IChargeStationInfoManagerUpdater(mChargeStationInfoManagerUpdater);
			mChargeStationInfoManagerUpdater = null;

			UnsubscribeEvent_IImportantEventRecorder(mImportantEventRecorder);
			mImportantEventRecorder = null;

			UnsubscribeEvent_ILogExporter(mLogExporter);
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
		private ISystemWithConfig GetCorrespondingObjectOfISystemWithConfig(string ObjectName)
		{
			foreach (ISystemWithConfig systemWithConfig in mCollectionOfISystemWithConfig)
			{
				if (systemWithConfig.GetType().Name == ObjectName)
				{
					return systemWithConfig;
				}
			}
			return null;
		}
		private void LoadConfigFileAndUpdateSystemConfig()
		{
			mConfigurator.Load();
			foreach (ISystemWithConfig systemWithConfig in mCollectionOfISystemWithConfig)
			{
				foreach (string configName in systemWithConfig.GetConfigNameList())
				{
					systemWithConfig.SetConfig(configName, mConfigurator.GetValue($"{systemWithConfig.GetType().Name}/{configName}"));
				}
			}
		}
		private void LoadSystemConfigAndUpdateConfigFile()
		{
			foreach (ISystemWithConfig systemWithConfig in mCollectionOfISystemWithConfig)
			{
				foreach (string configName in systemWithConfig.GetConfigNameList())
				{
					mConfigurator.SetValue($"{systemWithConfig.GetType().Name}/{configName}", systemWithConfig.GetConfig(configName));
				}
			}
			mConfigurator.Save();
		}
		private void SubscribeEvent_Exception()
		{
			System.Windows.Forms.Application.ThreadException += (sender, e) =>
			{
				string directory = ".\\Exception";
				string file = $".\\Exception\\ExceptionThread{DateTime.Now.ToString("yyyyMMdd")}.txt";
				string message = $"{DateTime.Now.ToString(TIME_FORMAT)} - [ThreadException] - {e.Exception.ToString()}\r\n";

				if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
				if (!System.IO.File.Exists(file)) System.IO.File.Create(file).Close();
				System.IO.File.AppendAllText(file, message);
			};
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				string directory = ".\\Exception";
				string file = $".\\Exception\\ExceptionUnhandled{DateTime.Now.ToString("yyyyMMdd")}.txt";
				string message = $"{DateTime.Now.ToString(TIME_FORMAT)} - [UnhandledException] - {e.ExceptionObject.ToString()}\r\n";

				if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
				if (!System.IO.File.Exists(file)) System.IO.File.Create(file).Close();
				System.IO.File.AppendAllText(file, message);
			};
		}
		private void SubscribeEvent_IConfigurator(IConfigurator Configurator)
		{
			if (Configurator != null)
			{
				Configurator.ConfigFileLoaded += HandleEvent_ConfiguratorConfigFileLoaded;
				Configurator.ConfigFileSaved += HandleEvent_ConfiguratorConfigFileSaved;
				Configurator.ConfigurationUpdated += HandleEvent_ConfiguratorConfigurationUpdated;
			}
		}
		private void UnsubscribeEvent_IConfigurator(IConfigurator Configurator)
		{
			if (Configurator != null)
			{
				Configurator.ConfigFileLoaded -= HandleEvent_ConfiguratorConfigFileLoaded;
				Configurator.ConfigFileSaved -= HandleEvent_ConfiguratorConfigFileSaved;
				Configurator.ConfigurationUpdated -= HandleEvent_ConfiguratorConfigurationUpdated;
			}
		}
		private void SubscribeEvent_ILogExporter(ILogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ConfigUpdated += HandleEvent_LogExporterConfigUpdated;
				LogExporter.ExportStarted += HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted += HandleEvent_LogExporterExportCompleted;
			}
		}
		private void UnsubscribeEvent_ILogExporter(ILogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ConfigUpdated -= HandleEvent_LogExporterConfigUpdated;
				LogExporter.ExportStarted -= HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted -= HandleEvent_LogExporterExportCompleted;
			}
		}
		private void SubscribeEvent_ITimeElapseDetecotr(ITimeElapseDetector TimeElapseDetector)
		{
			if (TimeElapseDetector != null)
			{
				TimeElapseDetector.SystemStatusChanged += HandleEvent_TimeElapseDetectorSystemStatusChanged;
				TimeElapseDetector.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				TimeElapseDetector.ConfigUpdated += HandleEvent_TimeElapseDetectorConfigUpdated;
				TimeElapseDetector.YearChanged += HandleEvent_TimeElapseDetectorYearChanged;
				TimeElapseDetector.MonthChanged += HandleEvent_TimeElapseDetectorMonthChanged;
				TimeElapseDetector.DayChanged += HandleEvent_TimeElapseDetectorDayChanged;
				TimeElapseDetector.HourChanged += HandleEvent_TimeElapseDetectorHourChanged;
				TimeElapseDetector.MinuteChanged += HandleEvent_TimeElapseDetectorMinuteChanged;
			}
		}
		private void UnsubscribeEvent_ITimeElapseDetecotr(ITimeElapseDetector TimeElapseDetector)
		{
			if (TimeElapseDetector != null)
			{
				TimeElapseDetector.SystemStatusChanged -= HandleEvent_TimeElapseDetectorSystemStatusChanged;
				TimeElapseDetector.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				TimeElapseDetector.ConfigUpdated -= HandleEvent_TimeElapseDetectorConfigUpdated;
				TimeElapseDetector.YearChanged -= HandleEvent_TimeElapseDetectorYearChanged;
				TimeElapseDetector.MonthChanged -= HandleEvent_TimeElapseDetectorMonthChanged;
				TimeElapseDetector.DayChanged -= HandleEvent_TimeElapseDetectorDayChanged;
				TimeElapseDetector.HourChanged -= HandleEvent_TimeElapseDetectorHourChanged;
				TimeElapseDetector.MinuteChanged -= HandleEvent_TimeElapseDetectorMinuteChanged;
			}
		}
		private void SubscribeEvent_IAccessControl(IAccessControl AccessControl)
		{
			if (AccessControl != null)
			{
				AccessControl.UserLogChanged += HandleEvent_AccessControlUserLogChanged;
			}
		}
		private void UnsubscribeEvent_IAccessControl(IAccessControl AccessControl)
		{
			if (AccessControl != null)
			{
				AccessControl.UserLogChanged -= HandleEvent_AccessControlUserLogChanged;
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SystemStatusChanged += HandleEvent_VehicleCommunicatorSystemStatusChanged;
				VehicleCommunicator.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				VehicleCommunicator.ConfigUpdated += HandleEvent_VehicleCommunicatorConfigUpdated;
				VehicleCommunicator.LocalListenStateChanged += HandleEvent_VehicleCommunicatorLocalListenStateChagned;
				VehicleCommunicator.RemoteConnectStateChanged += HandleEvent_VehicleCommunicatorRemoteConnectStateChagned;
				VehicleCommunicator.SentData += HandleEvent_VehicleCommunicatorSentData;
				VehicleCommunicator.ReceivedData += HandleEvent_VehicleCommunicatorReceivedData;
				VehicleCommunicator.SentDataSuccessed += HandleEvent_VehicleCommunicatorSentDataSuccessed;
				VehicleCommunicator.SentDataFailed += HandleEvent_VehicleCommunicatorSentDataFailed;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SystemStatusChanged -= HandleEvent_VehicleCommunicatorSystemStatusChanged;
				VehicleCommunicator.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				VehicleCommunicator.ConfigUpdated -= HandleEvent_VehicleCommunicatorConfigUpdated;
				VehicleCommunicator.LocalListenStateChanged -= HandleEvent_VehicleCommunicatorLocalListenStateChagned;
				VehicleCommunicator.RemoteConnectStateChanged -= HandleEvent_VehicleCommunicatorRemoteConnectStateChagned;
				VehicleCommunicator.SentData -= HandleEvent_VehicleCommunicatorSentData;
				VehicleCommunicator.ReceivedData -= HandleEvent_VehicleCommunicatorReceivedData;
				VehicleCommunicator.SentDataSuccessed -= HandleEvent_VehicleCommunicatorSentDataSuccessed;
				VehicleCommunicator.SentDataFailed -= HandleEvent_VehicleCommunicatorSentDataFailed;
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
		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.ConfigUpdated += HandleEvent_MapFileManagerConfigUpdated;
				MapFileManager.MapFileAdded += HandleEvent_MapFileManagerMapFileAdded;
				MapFileManager.MapFileRemoved += HandleEvent_MapFileManagerMapFileRemoved;
			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.ConfigUpdated -= HandleEvent_MapFileManagerConfigUpdated;
				MapFileManager.MapFileAdded -= HandleEvent_MapFileManagerMapFileAdded;
				MapFileManager.MapFileRemoved -= HandleEvent_MapFileManagerMapFileRemoved;
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.ConfigUpdated += HandleEvent_MapManagerConfigUpdated;
				MapManager.LoadMapSuccessed += HandleEvent_MapManagerLoadMapSuccessed;
				MapManager.LoadMapFailed += HandleEvent_MapManagerLoadMapFailed;
				MapManager.SynchronizeMapStarted += HandleEvent_MapManagerSynchronizeMapStarted;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.ConfigUpdated -= HandleEvent_MapManagerConfigUpdated;
				MapManager.LoadMapSuccessed -= HandleEvent_MapManagerLoadMapSuccessed;
				MapManager.LoadMapFailed -= HandleEvent_MapManagerLoadMapFailed;
				MapManager.SynchronizeMapStarted -= HandleEvent_MapManagerSynchronizeMapStarted;
			}
		}
		private void SubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{
				CollisionEventManager.ItemAdded += HandleEvent_CollisionEventManagerItemAdded;
				CollisionEventManager.ItemRemoved += HandleEvent_CollisionEventManagerItemRemoved;
				CollisionEventManager.ItemUpdated += HandleEvent_CollisionEventManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{
				CollisionEventManager.ItemAdded -= HandleEvent_CollisionEventManagerItemAdded;
				CollisionEventManager.ItemRemoved -= HandleEvent_CollisionEventManagerItemRemoved;
				CollisionEventManager.ItemUpdated -= HandleEvent_CollisionEventManagerItemUpdated;
			}
		}
		private void SubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStatusChanged += HandleEvent_CollisionEventDetectorSystemStatusChanged;
				CollisionEventDetector.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				CollisionEventDetector.ConfigUpdated += HandleEvent_CollisionEventDetectorConfigUpdated;
			}
		}
		private void UnsubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStatusChanged -= HandleEvent_CollisionEventDetectorSystemStatusChanged;
				CollisionEventDetector.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
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
				VehicleControlHandler.SystemStatusChanged += HandleEvent_VehicleControlHandlerSystemStatusChanged;
				VehicleControlHandler.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				VehicleControlHandler.ConfigUpdated += HandleEvent_VehicleControlHandlerConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStatusChanged -= HandleEvent_VehicleControlHandlerSystemStatusChanged;
				VehicleControlHandler.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				VehicleControlHandler.ConfigUpdated -= HandleEvent_VehicleControlHandlerConfigUpdated;
			}
		}
		private void SubscribeEvent_IVehicleControlUpdater(IVehicleControlUpdater VehicleControlUpdater)
		{
			if (VehicleControlUpdater != null)
			{
				VehicleControlUpdater.SystemStatusChanged += HandleEvent_VehicleControlUpdaterSystemStatusChanged;
				VehicleControlUpdater.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				VehicleControlUpdater.ConfigUpdated += HandleEvent_VehicleControlUpdaterConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlUpdater(IVehicleControlUpdater VehicleControlUpdater)
		{
			if (VehicleControlUpdater != null)
			{
				VehicleControlUpdater.SystemStatusChanged -= HandleEvent_VehicleControlUpdaterSystemStatusChanged;
				VehicleControlUpdater.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				VehicleControlUpdater.ConfigUpdated -= HandleEvent_VehicleControlUpdaterConfigUpdated;
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
				HostCommunicator.SystemStatusChanged += HandleEvent_HostCommunicatorSystemStatusChanged;
				HostCommunicator.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				HostCommunicator.ConfigUpdated += HandleEvent_HostCommunicatorConfigUpdated;
				HostCommunicator.LocalListenStateChanged += HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.RemoteConnectStateChanged += HandleEvent_HostCommunicatorRemoteConnectStateChanged;
				HostCommunicator.SentData += HandleEvent_HostCommunicatorSentData;
				HostCommunicator.ReceivedData += HandleEvent_HostCommunicatorReceivedData;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.SystemStatusChanged -= HandleEvent_HostCommunicatorSystemStatusChanged;
				HostCommunicator.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				HostCommunicator.ConfigUpdated -= HandleEvent_HostCommunicatorConfigUpdated;
				HostCommunicator.LocalListenStateChanged -= HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.RemoteConnectStateChanged -= HandleEvent_HostCommunicatorRemoteConnectStateChanged;
				HostCommunicator.SentData -= HandleEvent_HostCommunicatorSentData;
				HostCommunicator.ReceivedData -= HandleEvent_HostCommunicatorReceivedData;
			}
		}
		private void SubscribeEvent_IHostMessageAnalyzer(IHostMessageAnalyzer HostMessageAnalyzer)
		{
			if (HostMessageAnalyzer != null)
			{
				HostMessageAnalyzer.ConfigUpdated += HandleEvent_HostMessageAnalyzerConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IHostMessageAnalyzer(IHostMessageAnalyzer HostMessageAnalyzer)
		{
			if (HostMessageAnalyzer != null)
			{
				HostMessageAnalyzer.ConfigUpdated -= HandleEvent_HostMessageAnalyzerConfigUpdated;
			}
		}
		private void SubscribeEvent_IChargeStationInfoManager(IChargeStationInfoManager ChargeStationInfoManager)
		{
			if (ChargeStationInfoManager != null)
			{
				ChargeStationInfoManager.ItemAdded += HandleEvent_ChargeStationInfoManagerItemAdded;
				ChargeStationInfoManager.ItemRemoved += HandleEvent_ChargeStationInfoManagerItemRemoved;
				ChargeStationInfoManager.ItemUpdated += HandleEvent_ChargeStationInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IChargeStationInfoManager(IChargeStationInfoManager ChargeStationInfoManager)
		{
			if (ChargeStationInfoManager != null)
			{
				ChargeStationInfoManager.ItemAdded -= HandleEvent_ChargeStationInfoManagerItemAdded;
				ChargeStationInfoManager.ItemRemoved -= HandleEvent_ChargeStationInfoManagerItemRemoved;
				ChargeStationInfoManager.ItemUpdated -= HandleEvent_ChargeStationInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStatusChanged += HandleEvent_MissionDispatcherSystemStatusChanged;
				MissionDispatcher.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				MissionDispatcher.ConfigUpdated += HandleEvent_MissionDispatcherConfigUpdated;
				MissionDispatcher.MissionDispatched += HandleEvent_MissionDispatcherMissionDispatched;
			}
		}
		private void UnsubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStatusChanged -= HandleEvent_MissionDispatcherSystemStatusChanged;
				MissionDispatcher.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				MissionDispatcher.ConfigUpdated -= HandleEvent_MissionDispatcherConfigUpdated;
				MissionDispatcher.MissionDispatched -= HandleEvent_MissionDispatcherMissionDispatched;
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
				MissionUpdater.ConfigUpdated += HandleEvent_MissionUpdaterConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IMissionUpdater(IMissionUpdater MissionUpdater)
		{
			if (MissionUpdater != null)
			{
				MissionUpdater.ConfigUpdated -= HandleEvent_MissionUpdaterConfigUpdated;
			}
		}
		private void SubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStatusChanged += HandleEvent_CycleMissionGeneratorSystemStatusChanged;
				CycleMissionGenerator.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				CycleMissionGenerator.ConfigUpdated += HandleEvent_CycleMissionGeneratorConfigUpdated;
				CycleMissionGenerator.CycleMissionAssigned += HandleEvent_CycleMissionGeneratorCycleMissionAssigned;
				CycleMissionGenerator.CycleMissionUnassigned += HandleEvent_CycleMissionGeneratorCycleMissionUnassigned;
				CycleMissionGenerator.CycleMissionExecutedIndexChanged += HandleEvent_CycleMissionGeneratorCycleExecutedIndexChanged;
			}
		}
		private void UnsubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStatusChanged -= HandleEvent_CycleMissionGeneratorSystemStatusChanged;
				CycleMissionGenerator.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				CycleMissionGenerator.ConfigUpdated -= HandleEvent_CycleMissionGeneratorConfigUpdated;
				CycleMissionGenerator.CycleMissionAssigned -= HandleEvent_CycleMissionGeneratorCycleMissionAssigned;
				CycleMissionGenerator.CycleMissionUnassigned -= HandleEvent_CycleMissionGeneratorCycleMissionUnassigned;
				CycleMissionGenerator.CycleMissionExecutedIndexChanged -= HandleEvent_CycleMissionGeneratorCycleExecutedIndexChanged;
			}
		}
		private void SubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStatusChanged += HandleEvent_ImportantEventRecorderSystemStatusChanged;
				ImportantEventRecorder.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				ImportantEventRecorder.ConfigUpdated += HandleEvent_ImportantEventRecorderConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStatusChanged -= HandleEvent_ImportantEventRecorderSystemStatusChanged;
				ImportantEventRecorder.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				ImportantEventRecorder.ConfigUpdated -= HandleEvent_ImportantEventRecorderConfigUpdated;
			}
		}
		private void SubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				AutomaticDoorInfoManager.ItemAdded += HandleEvent_AutomaticDoorInfoManagerItemAdded;
				AutomaticDoorInfoManager.ItemRemoved += HandleEvent_AutomaticDoorInfoManagerItemRemoved;
				AutomaticDoorInfoManager.ItemUpdated += HandleEvent_AutomaticDoorInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				AutomaticDoorInfoManager.ItemAdded -= HandleEvent_AutomaticDoorInfoManagerItemAdded;
				AutomaticDoorInfoManager.ItemRemoved -= HandleEvent_AutomaticDoorInfoManagerItemRemoved;
				AutomaticDoorInfoManager.ItemUpdated -= HandleEvent_AutomaticDoorInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IAutomaticDoorCommunicator(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			if (AutomaticDoorCommunicator != null)
			{
				AutomaticDoorCommunicator.SystemStatusChanged += HandleEvent_AutomaticDoorCommunicatorSystemStatusChanged;
				AutomaticDoorCommunicator.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				AutomaticDoorCommunicator.ConfigUpdated += HandleEvent_AutomaticDoorCommunicatorConfigUpdated;
				AutomaticDoorCommunicator.ClientAdded += HandleEvent_AutomaticDoorCommunicatorClientAdded;
				AutomaticDoorCommunicator.ClientRemoved += HandleEvent_AutomaticDoorCommunicatorClientRemoved;
				AutomaticDoorCommunicator.RemoteConnectStateChanged += HandleEvent_AutomaticDoorCommunicatorRemoteConnectStateChanged;
				AutomaticDoorCommunicator.SentData += HandleEvent_AutomaticDoorCommunicatorSentData;
				AutomaticDoorCommunicator.ReceivedData += HandleEvent_AutomaticDoorCommunicatorReceivedData;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorCommunicator(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			if (AutomaticDoorCommunicator != null)
			{
				AutomaticDoorCommunicator.SystemStatusChanged -= HandleEvent_AutomaticDoorCommunicatorSystemStatusChanged;
				AutomaticDoorCommunicator.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				AutomaticDoorCommunicator.ConfigUpdated -= HandleEvent_AutomaticDoorCommunicatorConfigUpdated;
				AutomaticDoorCommunicator.ClientAdded -= HandleEvent_AutomaticDoorCommunicatorClientAdded;
				AutomaticDoorCommunicator.ClientRemoved -= HandleEvent_AutomaticDoorCommunicatorClientRemoved;
				AutomaticDoorCommunicator.RemoteConnectStateChanged -= HandleEvent_AutomaticDoorCommunicatorRemoteConnectStateChanged;
				AutomaticDoorCommunicator.SentData -= HandleEvent_AutomaticDoorCommunicatorSentData;
				AutomaticDoorCommunicator.ReceivedData -= HandleEvent_AutomaticDoorCommunicatorReceivedData;
			}
		}
		private void SubscribeEvent_IAutomaticDoorInfoManagerUpdater(IAutomaticDoorInfoManagerUpdater AutomaticDoorInfoManagerUpdater)
		{
			if (AutomaticDoorInfoManagerUpdater != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorInfoManagerUpdater(IAutomaticDoorInfoManagerUpdater AutomaticDoorInfoManagerUpdater)
		{
			if (AutomaticDoorInfoManagerUpdater != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IAutomaticDoorCommunicatorUpdater(IAutomaticDoorCommunicatorUpdater AutomaticDoorCommunicatorUpdater)
		{
			if (AutomaticDoorCommunicatorUpdater != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorCommunicatorUpdater(IAutomaticDoorCommunicatorUpdater AutomaticDoorCommunicatorUpdater)
		{
			if (AutomaticDoorCommunicatorUpdater != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				AutomaticDoorControlManager.ItemAdded += HandleEvent_AutomaticDoorControlManagerItemAdded;
				AutomaticDoorControlManager.ItemRemoved += HandleEvent_AutomaticDoorControlManagerItemRemoved;
				AutomaticDoorControlManager.ItemUpdated += HandleEvent_AutomaticDoorControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				AutomaticDoorControlManager.ItemAdded -= HandleEvent_AutomaticDoorControlManagerItemAdded;
				AutomaticDoorControlManager.ItemRemoved -= HandleEvent_AutomaticDoorControlManagerItemRemoved;
				AutomaticDoorControlManager.ItemUpdated -= HandleEvent_AutomaticDoorControlManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IAutomaticDoorControlManagerUpdater(IAutomaticDoorControlManagerUpdater AutomaticDoorControlManagerUpdater)
		{
			if (AutomaticDoorControlManagerUpdater != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorControlManagerUpdater(IAutomaticDoorControlManagerUpdater AutomaticDoorControlManagerUpdater)
		{
			if (AutomaticDoorControlManagerUpdater != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IAutomaticDoorControlHandler(IAutomaticDoorControlHandler AutomaticDoorControlHandler)
		{
			if (AutomaticDoorControlHandler != null)
			{
				AutomaticDoorControlHandler.SystemStatusChanged += HandleEvent_AutomaticDoorControlHandlerSystemStatusChanged;
				AutomaticDoorControlHandler.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				AutomaticDoorControlHandler.ConfigUpdated += HandleEvent_AutomaticDoorControlHandlerConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorControlHandler(IAutomaticDoorControlHandler AutomaticDoorControlHandler)
		{
			if (AutomaticDoorControlHandler != null)
			{
				AutomaticDoorControlHandler.SystemStatusChanged -= HandleEvent_AutomaticDoorControlHandlerSystemStatusChanged;
				AutomaticDoorControlHandler.SystemInfoReported -= HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				AutomaticDoorControlHandler.ConfigUpdated -= HandleEvent_AutomaticDoorControlHandlerConfigUpdated;
			}
		}
		private void SubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			if (VehiclePassThroughAutomaticDoorEventManager != null)
			{
				VehiclePassThroughAutomaticDoorEventManager.ItemAdded += HandleEvent_VehiclePassThroughAutomaticDoorEventManagerItemAdded;
				VehiclePassThroughAutomaticDoorEventManager.ItemRemoved += HandleEvent_VehiclePassThroughAutomaticDoorEventManagerItemRemoved;
				VehiclePassThroughAutomaticDoorEventManager.ItemUpdated += HandleEvent_VehiclePassThroughAutomaticDoorEventManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManager(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			if (VehiclePassThroughAutomaticDoorEventManager != null)
			{
				VehiclePassThroughAutomaticDoorEventManager.ItemAdded -= HandleEvent_VehiclePassThroughAutomaticDoorEventManagerItemAdded;
				VehiclePassThroughAutomaticDoorEventManager.ItemRemoved -= HandleEvent_VehiclePassThroughAutomaticDoorEventManagerItemRemoved;
				VehiclePassThroughAutomaticDoorEventManager.ItemUpdated -= HandleEvent_VehiclePassThroughAutomaticDoorEventManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehiclePassThroughAutomaticDoorEventManagerUpdater(IVehiclePassThroughAutomaticDoorEventManagerUpdater VehiclePassThroughAutomaticDoorEventManagerUpdater)
		{
			if (VehiclePassThroughAutomaticDoorEventManagerUpdater != null)
			{
				VehiclePassThroughAutomaticDoorEventManagerUpdater.SystemStatusChanged += HandleEvent_VehiclePassThroughAutomaticDoorEventManagerUpdaterSystemStatusChanged;
				VehiclePassThroughAutomaticDoorEventManagerUpdater.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				VehiclePassThroughAutomaticDoorEventManagerUpdater.ConfigUpdated += HandleEvent_VehiclePassThroughAutomaticDoorEventManagerUpdaterConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventManagerUpdater(IVehiclePassThroughAutomaticDoorEventManagerUpdater VehiclePassThroughAutomaticDoorEventManagerUpdater)
		{
			if (VehiclePassThroughAutomaticDoorEventManagerUpdater != null)
			{
				VehiclePassThroughAutomaticDoorEventManagerUpdater.SystemStatusChanged -= HandleEvent_VehiclePassThroughAutomaticDoorEventManagerUpdaterSystemStatusChanged;
				VehiclePassThroughAutomaticDoorEventManagerUpdater.SystemInfoReported += HandleEvent_ISystemWithLoopTaskSystemInfoReported;
				VehiclePassThroughAutomaticDoorEventManagerUpdater.ConfigUpdated -= HandleEvent_VehiclePassThroughAutomaticDoorEventManagerUpdaterConfigUpdated;
			}
		}
		private void SubscribeEvent_IVehiclePassThroughAutomaticDoorEventHandler(IVehiclePassThroughAutomaticDoorEventHandler VehiclePassThroughAutomaticDoorEventHandler)
		{
			if (VehiclePassThroughAutomaticDoorEventHandler != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehiclePassThroughAutomaticDoorEventHandler(IVehiclePassThroughAutomaticDoorEventHandler VehiclePassThroughAutomaticDoorEventHandler)
		{
			if (VehiclePassThroughAutomaticDoorEventHandler != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IChargeStationInfoManagerUpdater(IChargeStationInfoManagerUpdater ChargeStationInfoManagerUpdater)
		{
			if (ChargeStationInfoManagerUpdater != null)
			{
				ChargeStationInfoManagerUpdater.ConfigUpdated += HandleEvent_ChargeStationInfoManagerUpdaterConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IChargeStationInfoManagerUpdater(IChargeStationInfoManagerUpdater ChargeStationInfoManagerUpdater)
		{
			if (ChargeStationInfoManagerUpdater != null)
			{
				ChargeStationInfoManagerUpdater.ConfigUpdated -= HandleEvent_ChargeStationInfoManagerUpdaterConfigUpdated;
			}
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
		protected virtual void RaiseEvent_SignificantEvent(DateTime OccurTime, SignificantEventCategory Category, string Info, bool Sync = true)
		{
			RaiseEvent_SignificantEvent(OccurTime.ToString(TIME_FORMAT), Category.ToString(), Info, Sync);
		}
		protected virtual void RaiseEvent_SignificantEvent(string OccurTime, string Category, string Info, bool Sync = true)
		{
			if (Sync)
			{
				SignificantEvent?.Invoke(this, new SignificantEventEventArgs(OccurTime, Category, Info));
			}
			else
			{
				Task.Run(() => { SignificantEvent?.Invoke(this, new SignificantEventEventArgs(OccurTime, Category, Info)); });
			}
		}
		protected virtual void RaiseEvent_AccessControlUserLogChanged(DateTime OccurTime, string UserName, AccountRank UserRank, bool IsLogin, bool Sync = true)
		{
			if (Sync)
			{
				AccessControlUserLogChanged?.Invoke(this, new UserLogChangedEventArgs(OccurTime, UserName, UserRank, IsLogin));
			}
			else
			{
				Task.Run(() => { AccessControlUserLogChanged?.Invoke(this, new UserLogChangedEventArgs(OccurTime, UserName, UserRank, IsLogin)); });
			}
		}
		protected virtual void RaiseEvent_DestructProgressChanged(int ProgressValue, bool Sync = true)
		{
			if (Sync)
			{
				DestructProgressChanged?.Invoke(this, new DestructProgressChangedEventArgs(DateTime.Now, ProgressValue));
			}
			else
			{
				Task.Run(() => { DestructProgressChanged?.Invoke(this, new DestructProgressChangedEventArgs(DateTime.Now, ProgressValue)); });
			}
		}
		private void HandleEvent_ISystemWithLoopTaskSystemInfoReported(object Sender, SystemInfoReportedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, Sender.GetType().Name, "SystemInfoReported", $"SystemInfo: {Args.SystemInfo}");
		}
		private void HandleEvent_ConfiguratorConfigFileLoaded(object Sender, ConfigFileLoadedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "Configurator", "ConfigFileLoaded", $"FilePath: {Args.FilePath}");
		}
		private void HandleEvent_ConfiguratorConfigFileSaved(object Sender, ConfigFileSavedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "Configurator", "ConfigFileSaved", $"FilePath: {Args.FilePath}");
		}
		private void HandleEvent_ConfiguratorConfigurationUpdated(object Sender, ConfigurationUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "Configurator", "ConfigurationUpdated", $"Name: {Args.ConfigName}, ConfigNewValue: {Args.Configuration.mValue}");
			string fullConfigName = Args.ConfigName; // Example: LogExporter/BaseDirectory, VehicleControlUpdater/ToleranceOfYOfArrivedTarget, HostCommunicator/TimePeriod
			string objectName = fullConfigName.Substring(0, fullConfigName.IndexOf('/'));
			string objectConfigName = fullConfigName.Substring(fullConfigName.IndexOf('/') + 1);
			ISystemWithConfig systemWithConfig = GetCorrespondingObjectOfISystemWithConfig(objectName);
			if (systemWithConfig != null)
			{
				systemWithConfig.SetConfig(objectConfigName, mConfigurator.GetValue(fullConfigName));
			}
		}
		private void HandleEvent_LogExporterConfigUpdated(object sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "LogExporter", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_LogExporterExportStarted(object Sender, LogExportedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "LogExporter", "ExportStarted", $"Directory: {Args.DirectoryPath}, Items: {string.Join(", ", Args.Items)}");
		}
		private void HandleEvent_LogExporterExportCompleted(object Sender, LogExportedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "LogExporter", "ExportCompleted", $"Directory: {Args.DirectoryPath}, Items: {string.Join(", ", Args.Items)}");
		}
		private void HandleEvent_TimeElapseDetectorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "TimeElapseDetector", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_TimeElapseDetectorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "TimeElapseDetector", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_TimeElapseDetectorYearChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "TimeElapseDetector", "YearChagned", Args.ToString());
		}
		private void HandleEvent_TimeElapseDetectorMonthChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "TimeElapseDetector", "MonthChanged", Args.ToString());
		}
		private void HandleEvent_TimeElapseDetectorDayChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "TimeElapseDetector", "DayChagned", Args.ToString());
		}
		private void HandleEvent_TimeElapseDetectorHourChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "TimeElapseDetector", "HourChagned", Args.ToString());
		}
		private void HandleEvent_TimeElapseDetectorMinuteChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "TimeElapseDetector", "MinuteChagned", Args.ToString());
		}
		private void HandleEvent_AccessControlUserLogChanged(object Sender, UserLogChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AccessControl", "UserLogChanged", $"Name: {Args.UserName}, Rank: {Args.UserRank.ToString()}, IsLogin: {Args.IsLogin.ToString()}");
			RaiseEvent_AccessControlUserLogChanged(Args.OccurTime, Args.UserName, Args.UserRank, Args.IsLogin);
		}
		private void HandleEvent_VehicleCommunicatorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_VehicleCommunicatorLocalListenStateChagned(object Sender, ListenStateChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "LocalListenStateChanged", $"Port: {Args.Port}, IsListened: {Args.IsListened.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChagned(object Sender, ConnectStateChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "RemoteConnectStateChanged", $"IPPort: {Args.IpPort}, IsConnected: {Args.IsConnected.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorSentData(object Sender, SentDataEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "SentData", $"IPPort: {Args.IpPort}, DataType: {Args.Data.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorReceivedData(object Sender, ReceivedDataEventArgs Args)
		{
			// 常態事件不做 General Log 記錄(避免資料庫儲存太多的資訊)，也不使用 Console.WriteLine() 顯示(避免資訊過多)
			if (!(Args.Data is SerialData.AGVStatus) && !(Args.Data is SerialData.AGVPath))
			{
				HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "ReceivedData", $"IPPort: {Args.IpPort}, DataType: {Args.Data.ToString()}");
			}
		}
		private void HandleEvent_VehicleCommunicatorSentDataSuccessed(object Sender, SentDataEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "SentDataSuccessed", $"IPPort: {Args.IpPort}, DataType: {Args.Data.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorSentDataFailed(object Sender, SentDataEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "SentDataFailed", $"IPPort: {Args.IpPort}, DataType: {Args.Data.ToString()}");
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleInfoManager", "ItemAdded", $"Name: {Args.ItemName}, Info: {Args.Item.ToString()}");
			RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.VehicleSystem, $"Vehicle [ {Args.ItemName} ] Connected");
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleInfoManager", "ItemRemoved", $"Name: {Args.ItemName}, Info: {Args.Item.ToString()}");
			RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.VehicleSystem, $"Vehicle [ {Args.ItemName} ] Disconnected");
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			// 僅有重要的狀態 (CurrentState, CurrentTarget, AlarmMessage, CurrentMissionId, CurrentInterveneCommand, CurrentMapName) 變化時才做 General Log 記錄與使用 Console.WriteLine() 顯示
			if (Args.StatusName.Contains("CurrentState") || Args.StatusName.Contains("CurrentTarget") || Args.StatusName.Contains("AlarmMessage") || Args.StatusName.Contains("CurrentMissionId") || Args.StatusName.Contains("CurrentInterveneCommand") || Args.StatusName.Contains("CurrentMapName") || Args.StatusName.Contains("IsTranslating") || Args.StatusName.Contains("IsRotating"))
			{
				HandleDebugMessage(Args.OccurTime, "VehicleInfoManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info: {Args.Item.ToString()}");
			}
		}
		private void HandleEvent_MapFileManagerConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapFileManager", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_MapFileManagerMapFileAdded(object Sender, MapFileCountChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapFileManager", "ItemAdded", $"MapFileName: {Args.MapFileName}");
		}
		private void HandleEvent_MapFileManagerMapFileRemoved(object Sender, MapFileCountChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapFileManager", "ItemRemoved", $"MapFileName: {Args.MapFileName}");
		}
		private void HandleEvent_MapManagerConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapManager", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_MapManagerLoadMapSuccessed(object Sender, LoadMapSuccessedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapManager", "LoadMapSuccessed", $"MapName: {Args.MapFileName}");
		}
		private void HandleEvent_MapManagerLoadMapFailed(object Sender, LoadMapFailedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapManager", "LoadMapFailed", $"MapName: {Args.MapFileName}, Reason: {Args.Reason.ToString()}");
		}
		private void HandleEvent_MapManagerSynchronizeMapStarted(object Sender, SynchronizeMapStartedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapFileManager", "SynchronizeMapStarted", $"MapFileName: {Args.MapFileName}, VehicleNames: {string.Join(",", Args.VehicleNames)}");
		}
		private void HandleEvent_CollisionEventManagerItemAdded(object Sender, ItemCountChangedEventArgs<ICollisionPair> Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventManager", "ItemAdded", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_CollisionEventManagerItemRemoved(object Sender, ItemCountChangedEventArgs<ICollisionPair> Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventManager", "ItemRemoved", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_CollisionEventManagerItemUpdated(object Sender, ItemUpdatedEventArgs<ICollisionPair> Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_CollisionEventDetectorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventDetector", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_CollisionEventDetectorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventDetector", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_VehicleControlManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleControl> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlManager", "ItemAdded", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_VehicleControlManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleControl> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlManager", "ItemRemoved", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleControl> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_VehicleControlHandlerSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlHandler", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_VehicleControlHandlerConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlHandler", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_VehicleControlUpdaterSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlUpdater", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_VehicleControlUpdaterConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlUpdater", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_MissionStateManagerItemAdded(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionStateManager", "ItemAdded", $"MissionID: {Args.ItemName}, Info: {Args.Item.ToString()}");
			RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.MissionSystem, $"Mission [ {Args.Item.GetMissionId()} ] Created");
		}
		private void HandleEvent_MissionStateManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionStateManager", "ItemRemoved", $"MissionID: {Args.ItemName}, Info: {Args.Item.ToString()}");
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionStateManager", "ItemUpdated", $"MissionID: {Args.ItemName}, StatusName:{Args.StatusName}, Info: {Args.Item.ToString()}");
			if (Args.StatusName.Contains("ExecutionStartTimestamp"))
			{
				RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.MissionSystem, $"Mission [ {Args.Item.GetMissionId()} ] Started by Vehicle [ {Args.Item.mExecutorId} ]");
			}
			else if (Args.StatusName.Contains("ExecutionStopTimestamp"))
			{
				RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.MissionSystem, $"Mission [ {Args.Item.GetMissionId()} ] Completed [ {Args.Item.mExecuteState.ToString().Replace("Execute", string.Empty)} ] by Vehicle [ {Args.Item.mExecutorId} ]");
			}
		}
		private void HandleEvent_HostCommunicatorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "HostCommunicator", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_HostCommunicatorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "HostCommunicator", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(object Sender, ListenStateChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "HostCommunicator", "LocalListenStateChanged", $"Port: {Args.Port}, IsListened: {Args.IsListened.ToString()}");
		}
		private void HandleEvent_HostCommunicatorRemoteConnectStateChanged(object Sender, ConnectStateChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "HostCommunicator", "RemoteConnectStateChanged", $"IPPort: {Args.IpPort}, IsConnected: {Args.IsConnected}");
			if (Args.IsConnected)
			{
				RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.HostSystem, $"Host [ {Args.IpPort} ] Connected");
			}
			else
			{
				RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.HostSystem, $"Host [ {Args.IpPort} ] Disconnected");
			}
		}
		private void HandleEvent_HostCommunicatorSentData(object Sender, SentDataEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "HostCommunicator", "SentData", $"IPPort: {Args.IpPort}, Data: {Args.Data}");
			RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.HostSystem, $"Sent Message [ {Args.IpPort} ] [ {Args.Data} ]");
		}
		private void HandleEvent_HostCommunicatorReceivedData(object Sender, ReceivedDataEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "HostCommunicator", "ReceivedData", $"IPPort: {Args.IpPort}, Data: {Args.Data}");
			RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.HostSystem, $"Received Message [ {Args.IpPort} ] [ {Args.Data} ]");
		}
		private void HandleEvent_HostMessageAnalyzerConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "HostMessageAnalyzer", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_MissionDispatcherSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionDispatcher", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_MissionDispatcherConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionDispatcher", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_MissionDispatcherMissionDispatched(object Sender, MissionDispatchedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionDispatcher", "MissionDispatched", $"MissionName: {Args.MissionState.mName} Dispatched To VehicleName: {Args.VehicleInfo.mName}");
		}
		private void HandleEvent_MissionUpdaterConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionUpdater", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_CycleMissionGeneratorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionAssigned(object Sender, CycleMissionAssignedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "CycleMissionAssigned", $"VehicleID: {Args.VehicleId}, Missions: ({string.Join(")(", Args.Missions)})");
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionUnassigned(object Sender, CycleMissionUnassignedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "CycleMissionUnassigned", $"VehicleID: {Args.VehicleId}");
		}
		private void HandleEvent_CycleMissionGeneratorCycleExecutedIndexChanged(object Sender, CycleMissionExecutedIndexChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "CycleMissionExecutedIndexChanged", $"VehicleID: {Args.VehicleId}, Index: {Args.Index.ToString()}");
		}
		private void HandleEvent_CycleMissionGeneratorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_ImportantEventRecorderSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "ImportantEventRecorder", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_ImportantEventRecorderConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "ImportantEventRecorder", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_AutomaticDoorInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IAutomaticDoorInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorInfoManager", "ItemAdded", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_AutomaticDoorInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IAutomaticDoorInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorInfoManager", "ItemRemoved", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_AutomaticDoorInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IAutomaticDoorInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorInfoManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_AutomaticDoorCommunicatorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorCommunicator", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_AutomaticDoorCommunicatorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorCommunicator", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_AutomaticDoorCommunicatorClientAdded(object Sender, ClientAddedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorCommunicator", "ClientAdded", $"IPPort: {Args.IpPort}");
		}
		private void HandleEvent_AutomaticDoorCommunicatorClientRemoved(object Sender, ClientRemovedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorCommunicator", "ClientRemoved", $"IPPort: {Args.IpPort}");
		}
		private void HandleEvent_AutomaticDoorCommunicatorRemoteConnectStateChanged(object Sender, ConnectStateChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorCommunicator", "RemoteConnectStateChanged", $"IPPort: {Args.IpPort}, IsConnected: {Args.IsConnected.ToString()}");
		}
		private void HandleEvent_AutomaticDoorCommunicatorSentData(object Sender, SentDataEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorCommunicator", "SentData", $"IPPort: {Args.IpPort}, Data: {Args.Data}");
		}
		private void HandleEvent_AutomaticDoorCommunicatorReceivedData(object Sender, ReceivedDataEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorCommunicator", "ReceivedData", $"IPPort: {Args.IpPort}, Data: {Args.Data}");
		}
		private void HandleEvent_AutomaticDoorControlManagerItemAdded(object Sender, ItemCountChangedEventArgs<IAutomaticDoorControl> Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorControlManager", "ItemAdded", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_AutomaticDoorControlManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IAutomaticDoorControl> Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorControlManager", "ItemRemoved", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_AutomaticDoorControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IAutomaticDoorControl> Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorControlManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_AutomaticDoorControlHandlerSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorControlHandler", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_AutomaticDoorControlHandlerConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AutomaticDoorControlHandler", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_VehiclePassThroughAutomaticDoorEventManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehiclePassThroughAutomaticDoorEvent> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehiclePassThroughAutomaticDoorEventManager", "ItemAdded", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_VehiclePassThroughAutomaticDoorEventManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehiclePassThroughAutomaticDoorEvent> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehiclePassThroughAutomaticDoorEventManager", "ItemRemoved", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_VehiclePassThroughAutomaticDoorEventManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehiclePassThroughAutomaticDoorEvent> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehiclePassThroughAutomaticDoorEventManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_VehiclePassThroughAutomaticDoorEventManagerUpdaterSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehiclePassThroughAutomaticDoorEventManagerUpdater", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_VehiclePassThroughAutomaticDoorEventManagerUpdaterConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehiclePassThroughAutomaticDoorEventManagerUpdater", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_ChargeStationInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IChargeStationInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "ChargeStationInfoManager", "ItemAdded", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_ChargeStationInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IChargeStationInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "ChargeStationInfoManager", "ItemRemoved", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_ChargeStationInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IChargeStationInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "ChargeStationInfoManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_ChargeStationInfoManagerUpdaterConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "ChargeStationInfoManagerUpdater", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
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
		private int GetCurrentThreadClosingProgress()
		{
			return mCollectionOfISystemWithLoopTask.Where(o => !o.mIsExecuting).Count() * 100 / mCollectionOfISystemWithLoopTask.Count;
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
	public class SignificantEventEventArgs : EventArgs
	{
		public string OccurTime { get; private set; }
		public string Category { get; private set; }
		public string Info { get; private set; }

		public SignificantEventEventArgs(string OccurTime, string Category, string Info) : base()
		{
			this.OccurTime = OccurTime;
			this.Category = Category;
			this.Info = Info;
		}
	}
	public class DestructProgressChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public int ProgressValue { get; private set; }

		public DestructProgressChangedEventArgs(DateTime OccurTime, int ProgressValue)
		{
			this.OccurTime = OccurTime;
			this.ProgressValue = ProgressValue;
		}
	}
}
