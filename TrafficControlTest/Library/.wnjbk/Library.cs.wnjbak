using LibraryForVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TrafficControlTest.Module.Account;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.ChargeStation;
using TrafficControlTest.Module.CollisionEvent;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.Configure;
using TrafficControlTest.Module.CycleMission;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.LimitVehicleCountZone;
using TrafficControlTest.Module.Log;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.ParkStation;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.VehiclePassThroughAutomaticDoor;
using TrafficControlTest.Module.VehiclePassThroughLimitVehicleCountZone;

namespace TrafficControlTest.Library
{
	public enum Language
	{
		Enus,
		Zhtw,
		Zhcn
	}

	public enum ConnectState
	{
		Closed,
		Connected,
		Disconnected
	}

	public enum ListenState
	{
		Closed,
		Listening
	}

	public enum MissionType
	{
		Goto,
		GotoPoint,
		Dock,
		Abort
	}

	public enum SignificantEventCategory
	{
		VehicleSystem,
		MissionSystem,
		HostSystem,
		AutomaticDoorSystem
	}

	public enum AccountRank
	{
		Software,
		Service,
		Customer,
		None
	}

	internal static class Library
	{
		public const string TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

		private static object mLockOfICollisionPair = new object();
		private static object mLockOfIVehicleInfo = new object();
		private static object mLockOfIVehicleControl = new object();
		private static object mLockOfIMissionState = new object();
		private static object mLockOfIAutomaticDoorInfo = new object();
		private static object mLockOfIAutomaticDoorControl = new object();
		private static object mLockOfIVehiclePassThroughAutomaticDoorEvent = new object();
		private static object mLockOfIChargeStationInfo = new object();
		private static object mLockOfILimitVehicleCountZoneInfo = new object();
		private static object mLockOfIVehiclePassThroughLimitVehicleCountZoneEvent = new object();
		private static object mLockOfIParkStationInfo = new object();

		public static bool ConvertToDictionary(string Source, out Dictionary<string, string> ResultDictionary, string SeperateSymbol = " ", string EqualSymbol = "=")
		{
			ResultDictionary = null;
			string[] datas = Source.Split(new string[] { SeperateSymbol }, StringSplitOptions.RemoveEmptyEntries);
			if (datas.Length > 0)
			{
				for (int i = 0; i < datas.Length; ++i)
				{
					if (datas[i].Contains(EqualSymbol))
					{
						if (ResultDictionary == null) ResultDictionary = new Dictionary<string, string>();
						int equalIndex = datas[i].LastIndexOf(EqualSymbol);
						string key = datas[i].Substring(0, equalIndex);
						string value = datas[i].Substring(equalIndex + 1);
						if (!ResultDictionary.Keys.Contains(key))
						{
							ResultDictionary.Add(key, value);
						}
						else
						{
							// 若有重複的 Key 則創建字典失敗。代表 Source 有重複資訊
							ResultDictionary = null;
							break;
						}
					}
					else
					{
						// 若有子字串中不包含 EqualSymbol 則創建字典失敗。代表 Source 有多餘資訊
						ResultDictionary = null;
						break;
					}
				}
			}
			return (ResultDictionary == null || ResultDictionary.Count == 0) ? false : true;
		}

		#region Factory
		public static IPathRegionOverlapPair GenerateIPathRegionOverlapPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D OverlapRegionOfPathRegions)
		{
			return new PathRegionOverlapPair(Vehicle1, Vehicle2, OverlapRegionOfPathRegions);
		}
		public static IPathOverlapPair GenerateIPathOverlapPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IEnumerable<IRectangle2D> OverlapRegionsOfPaths)
		{
			return new PathOverlapPair(Vehicle1, Vehicle2, OverlapRegionsOfPaths);
		}
		public static ICollisionPair GenerateICollisionPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy)
		{
			lock (mLockOfICollisionPair)
			{
				System.Threading.Thread.Sleep(5);
				return new CollisionPair(Vehicle1, Vehicle2, CollisionRegion, Period, PassPeriodOfVehicle1WithCurrentVelocity, PassPeriodOfVehicle2WithCurrentVelocity, PassPeriodOfVehicle1WithMaximumVeloctiy, PassPeriodOfVehicle2WithMaximumVeloctiy);
			}
		}
		public static ITimePeriod GenerateITimePeriod(DateTime Start, DateTime End)
		{
			return new TimePeriod(Start, End);
		}
		public static IVehicleInfo GenerateIVehicleInfo(string Name)
		{
			lock (mLockOfIVehicleInfo)
			{
				System.Threading.Thread.Sleep(5);
				return new VehicleInfo(Name);
			}
		}
		public static IVehicleCommunicator GenerateIVehicleCommunicator()
		{
			return new VehicleCommunicator();
		}
		public static IVehicleInfoManager GenerateIVehicleInfoManager()
		{
			return new VehicleInfoManager();
		}
		public static IVehicleInfoUpdater GenerateIVehicleInfoUpdater(IVehicleCommunicator VehicleCommunicator, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager)
		{
			return new VehicleInfoUpdater(VehicleCommunicator, MissionStateManager, VehicleControlManager, VehicleInfoManager);
		}
		public static ICollisionEventManager GenerateICollisionEventManager()
		{
			return new CollisionEventManager();
		}
		public static ICollisionEventDetector GenerateICollisionEventDetector(IVehicleInfoManager VehicleInfoManager, ICollisionEventManager CollisionEventManager)
		{
			return new CollisionEventDetector(VehicleInfoManager, CollisionEventManager);
		}
		public static IVehicleControl GenerateIVehicleControl(string VehicleId, Command Command, string[] Parameters, string CauseId, string CauseDetail)
		{
			lock (mLockOfIVehicleControl)
			{
				System.Threading.Thread.Sleep(5);
				return new VehicleControl(VehicleId, Command, Parameters, CauseId, CauseDetail);
			}
		}
		public static IVehicleControlManager GenerateIVehicleControlManager()
		{
			return new VehicleControlManager();
		}
		public static ICollisionEventHandler GenerateICollisionEventHandler(ICollisionEventManager CollisionEventManager, IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager)
		{
			return new CollisionEventHandler(CollisionEventManager, VehicleControlManager, VehicleInfoManager);
		}
		public static IVehicleControlHandler GenerateIVehicleControlHandler(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator)
		{
			return new VehicleControlHandler(VehicleControlManager, VehicleInfoManager, VehicleCommunicator);
		}
		public static IVehicleControlUpdater GenerateIVehicleControlUpdater(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator, IMapManager MapManager)
		{
			return new VehicleControlUpdater(VehicleControlManager, VehicleInfoManager, VehicleCommunicator, MapManager);
		}
		public static IMission GenerateIMission(MissionType MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters)
		{
			return new Mission(MissionType, MissionId, Priority, VehicleId, Parameters);
		}
		public static IMissionState GenerateIMissionState(IMission Mission)
		{
			lock (mLockOfIMissionState)
			{
				System.Threading.Thread.Sleep(5);
				return new MissionState(Mission);
			}
		}
		public static IMissionStateManager GenerateIMissionStateManager()
		{
			return new MissionStateManager();
		}
		public static IMissionAnalyzer GetMissionAnalyzer(MissionType MissionType)
		{
			IMissionAnalyzer result = null;
			switch (MissionType)
			{
				case MissionType.Goto:
					result = GotoMissionAnalyzer.mInstance;
					break;
				case MissionType.GotoPoint:
					result = GotoPointMissionAnalyzer.mInstance;
					break;
				case MissionType.Dock:
					result = DockMissionAnalyzer.mInstance;
					break;
				case MissionType.Abort:
					result = AbortMissionAnalyzer.mInstance;
					break;
				default:
					break;
			}
			return result;
		}
		public static IMissionAnalyzer[] GetMissionAnalyzers()
		{
			return new IMissionAnalyzer[] { DockMissionAnalyzer.mInstance, GotoPointMissionAnalyzer.mInstance, GotoMissionAnalyzer.mInstance, AbortMissionAnalyzer.mInstance };
		}
		public static IHostCommunicator GenerateIHostCommunicator()
		{
			return new HostCommunicator();
		}
		public static IHostMessageAnalyzer GenerateIHostMessageAnalyzer(IHostCommunicator HostCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMissionAnalyzer[] MissionAnalyzers)
		{
			return new HostMessageAnalyzer(HostCommunicator, VehicleInfoManager, MissionStateManager, MissionAnalyzers);
		}
		public static IMissionDispatcher GenerateIMissionDispatcher(IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager, IVehicleControlManager VehicleControlManager)
		{
			return new MissionDispatcher(MissionStateManager, VehicleInfoManager, VehicleControlManager);
		}
		public static IMissionUpdater GenerateIMissionUpdater(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager)
		{
			return new MissionUpdater(VehicleInfoManager, MissionStateManager, VehicleControlManager);
		}
		public static IMapFileManager GenerateIMapFileManager(IConfigurator Configurator)
		{
			return new MapFileManager(Configurator);
		}
		public static IMapManager GenerateIMapManager()
		{
			return new MapManager();
		}
		public static IMapFileManagerUpdater GenerateIMapFileManagerUpdater(IMapFileManager MapFileManager, IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			return new MapFileManagerUpdater(MapFileManager, VehicleCommunicator, VehicleInfoManager);
		}
		public static IMapManagerUpdater GenerateIMapManagerUpdater(IMapManager MapManager, IMapFileManager MapFileManager, IMapFileManagerUpdater MapFileManagerUpdater, IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			return new MapManagerUpdater(MapManager, MapFileManager, MapFileManagerUpdater, VehicleCommunicator, VehicleInfoManager);
		}
		public static IMissionStateReporter GenerateIMissionStateReporter(IMissionStateManager MissionStateManager, IHostCommunicator HostCommunicator)
		{
			return new MissionStateReporter(MissionStateManager, HostCommunicator);
		}
		public static DatabaseAdapter GenerateDatabaseAdapter(string DatabaseServerAddressIp, string DatabaseServerAddressPort, string UserAccount, string UserPassword, string InitialDatabase, bool PingBeforeBuildConnection)
		{
			return new SqliteDatabaseAdapter(DatabaseServerAddressIp, DatabaseServerAddressPort, UserAccount, UserPassword, InitialDatabase, PingBeforeBuildConnection);
		}
		public static IHistoryLogAdapter GenerateIHistoryLogAdapter(DatabaseAdapter DatabaseAdapter)
		{
			return new HistoryLogAdapter(DatabaseAdapter);
		}
		public static ICurrentLogAdapter GenerateICurrentLogAdapter(DatabaseAdapter DatabaseAdapter)
		{
			return new CurrentLogAdapter(DatabaseAdapter);
		}
		public static ILogRecorder GenerateILogRecorder(ICurrentLogAdapter CurrentLogAdapter, IHistoryLogAdapter HistoryLogAdapter, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager, IAutomaticDoorControlManager AutomaticDoorControlManager, IHostCommunicator HostCommunicator)
		{
			return new LogRecorder(CurrentLogAdapter, HistoryLogAdapter, VehicleInfoManager, MissionStateManager, VehicleControlManager, AutomaticDoorControlManager, HostCommunicator);
		}
		public static ILogMaintainHandler GenerateILogMaintainHandler(IHistoryLogAdapter HistoryLogAdapter, ITimeElapseDetector TimeElapseDetector)
		{
			return new LogMaintainHandler(HistoryLogAdapter, TimeElapseDetector);
		}
		public static IConfigurator GenerateIConfigurator(string FileName, ProjectType ProjectType)
		{
			return new Configurator(FileName, ProjectType);
		}
		public static IAccount GenerateIAccount(string Name, string Password, AccountRank Rank)
		{
			return new Account(Name, Password, Rank);
		}
		public static IAccountManager GenerateIAccountManager(DatabaseAdapter DatabaseAdapter)
		{
			return new AccountManager(DatabaseAdapter);
		}
		public static IAccessControl GenerateIAccessControl(IAccountManager AccountManager)
		{
			return new AccessControl(AccountManager);
		}
		public static ICycleMissionGenerator GenerateICycleMissionGenerator(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager)
		{
			return new CycleMissionGenerator(VehicleInfoManager, MissionStateManager);
		}
		public static ILogExporter GenerateILogExporter(ProjectType ProjectType)
		{
			return new LogExporter(ProjectType);
		}
		public static IAutomaticDoorInfo GenerateIAutomaticDoorInfo(string Name, IRectangle2D Range, string IpPort)
		{
			lock (mLockOfIAutomaticDoorInfo)
			{
				System.Threading.Thread.Sleep(5);
				return new AutomaticDoorInfo(Name, Range, IpPort);
			}
		}
		public static IAutomaticDoorInfoManager GenerateIAutomaticDoorInfoManager()
		{
			return new AutomaticDoorInfoManager();
		}
		public static IAutomaticDoorCommunicator GenerateIAutomaticDoorCommunicator()
		{
			return new AutomaticDoorCommunicator();
		}
		public static IAutomaticDoorInfoManagerUpdater GenerateIAutomaticDoorInfoManagerUpdater(IAutomaticDoorInfoManager AutomaticDoorInfoManager, IMapManager MapManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			return new AutomaticDoorInfoManagerUpdater(AutomaticDoorInfoManager, MapManager, AutomaticDoorCommunicator);
		}
		public static IAutomaticDoorCommunicatorUpdater GenerateIAutomaticDoorCommunicatorUpdater(IAutomaticDoorCommunicator AutomaticDoorCommunicator, IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			return new AutomaticDoorCommunicatorUpdater(AutomaticDoorCommunicator, AutomaticDoorInfoManager);
		}
		public static IAutomaticDoorControl GenerateIAutomaticDoorControl(string AutomaticDoorName, AutomaticDoorControlCommand Command, string Cause)
		{
			lock (mLockOfIAutomaticDoorControl)
			{
				System.Threading.Thread.Sleep(5);
				return new AutomaticDoorControl(AutomaticDoorName, Command, Cause);
			}
		}
		public static IAutomaticDoorControlManager GenerateIAutomaticDoorControlManager()
		{
			return new AutomaticDoorControlManager();
		}
		public static IAutomaticDoorControlManagerUpdater GenerateIAutomaticDoorControlManagerUpdater(IAutomaticDoorControlManager AutomaticDoorControlManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			return new AutomaticDoorControlManagerUpdater(AutomaticDoorControlManager, AutomaticDoorInfoManager, AutomaticDoorCommunicator);
		}
		public static IAutomaticDoorControlHandler GenerateIAutomaticDoorControlHandler(IAutomaticDoorControlManager AutomaticDoorControlManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			return new AutomaticDoorControlHandler(AutomaticDoorControlManager, AutomaticDoorInfoManager, AutomaticDoorCommunicator);
		}
		public static IVehiclePassThroughAutomaticDoorEvent GenerateIVehiclePassThroughAutomaticDoorEvent(string VehicleName, string AutomaticDoorName, int Distance)
		{
			lock (mLockOfIVehiclePassThroughAutomaticDoorEvent)
			{
				System.Threading.Thread.Sleep(5);
				return new VehiclePassThroughAutomaticDoorEvent(VehicleName, AutomaticDoorName, Distance);
			}
		}
		public static IVehiclePassThroughAutomaticDoorEventManager GenerateIVehiclePassThroughAutomaticDoorEventManager()
		{
			return new VehiclePassThroughAutomaticDoorEventManager();
		}
		public static IVehiclePassThroughAutomaticDoorEventManagerUpdater GenerateIVehiclePassThroughAutomaticDoorEventManagerUpdater(IVehicleInfoManager VehicleInfoManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager)
		{
			return new VehiclePassThroughAutomaticDoorEventManagerUpdater(VehicleInfoManager, AutomaticDoorInfoManager, VehiclePassThroughAutomaticDoorEventManager);
		}
		public static IVehiclePassThroughAutomaticDoorEventHandler GenerateIVehiclePassThroughAutomaticDoorEventHandler(IVehiclePassThroughAutomaticDoorEventManager VehiclePassThroughAutomaticDoorEventManager, IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			return new VehiclePassThroughAutomaticDoorEventHandler(VehiclePassThroughAutomaticDoorEventManager, AutomaticDoorControlManager);
		}
		public static IChargeStationInfo GenerateIChargeStationInfo(string Name, ITowardPoint2D Location, IRectangle2D LocationRange)
        {
			lock (mLockOfIChargeStationInfo)
			{
				System.Threading.Thread.Sleep(5);
				return new ChargeStationInfo(Name, Location, LocationRange);
			}
        }
        public static IChargeStationInfoManager GenerateIChargeStationInfoManager()
        {
            return new ChargeStationInfoManager();
        }
        public static IChargeStationInfoManagerUpdater GenerateIChargeStationInfoManagerUpdater(IChargeStationInfoManager ChargeStationInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
        {
            return new ChargeStationInfoManagerUpdater(ChargeStationInfoManager, MapManager, VehicleInfoManager);
        }
        public static ILimitVehicleCountZoneInfo GenerateILimitVehicleCountZoneInfo(string Name, IRectangle2D Range, int MaxVehicleCount, bool IsUnioned, int UnionId)
		{
			lock (mLockOfILimitVehicleCountZoneInfo)
			{
				System.Threading.Thread.Sleep(5);
				return new LimitVehicleCountZoneInfo(Name, Range, MaxVehicleCount, IsUnioned, UnionId);
			}
		}
		public static ILimitVehicleCountZoneInfoManager GenerateILimitVehicleCountZoneInfoManager()
		{
			return new LimitVehicleCountZoneInfoManager();
		}
		public static ILimitVehicleCountZoneInfoManagerUpdater GenerateILimitVehicleCountZoneInfoManagerUpdater(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			return new LimitVehicleCountZoneInfoManagerUpdater(LimitVehicleCountZoneInfoManager, MapManager, VehicleInfoManager);
		}
		public static IVehiclePassThroughLimitVehicleCountZoneEvent GenerateIVehiclePassThroughLimitVehicleCountZoneEvent(IVehicleInfo VehicleInfo, ILimitVehicleCountZoneInfo LimitVehicleCountZoneInfo, int Distance)
		{
			lock (mLockOfIVehiclePassThroughLimitVehicleCountZoneEvent)
			{
				System.Threading.Thread.Sleep(5);
				return new VehiclePassThroughLimitVehicleCountZoneEvent(VehicleInfo, LimitVehicleCountZoneInfo, Distance);
			}
		}
		public static IVehiclePassThroughLimitVehicleCountZoneEventManager GenerateIVehiclePassThroughLimitVehicleCountZoneEventManager()
		{
			return new VehiclePassThroughLimitVehicleCountZoneEventManager();
		}
		public static IVehiclePassThroughLimitVehicleCountZoneEventManagerUpdater GenerateIVehiclePassThroughLimitVehicleCountZoneEventManagerUpdater(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleInfoManager VehicleInfoManager, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			return new VehiclePassThroughLimitVehicleCountZoneEventManagerUpdater(VehiclePassThroughLimitVehicleCountZoneEventManager, VehicleInfoManager, LimitVehicleCountZoneInfoManager);
		}
		public static IVehiclePassThroughLimitVehicleCountZoneEventHandler GenerateIVehiclePassThroughLimitVehicleCountZoneEventHandler(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleControlManager VehicleControlManager)
		{
			return new VehiclePassThroughLimitVehicleCountZoneEventHandler(VehiclePassThroughLimitVehicleCountZoneEventManager, VehicleControlManager);
		}
		public static IParkStationInfo GenerateIParkStationInfo(string Name, ITowardPoint2D Location, IRectangle2D LocationRange)
		{
			lock (mLockOfIParkStationInfo)
			{
				System.Threading.Thread.Sleep(5);
				return new ParkStationInfo(Name, Location, LocationRange);
			}
		}
		public static IParkStationInfoManager GenerateIParkStationInfoManager()
		{
			return new ParkStationInfoManager();
		}
		public static IParkStationInfoManagerUpdater GenerateIParkStationInfoManagerUpdater(IParkStationInfoManager ParkStationInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			return new ParkStationInfoManagerUpdater(ParkStationInfoManager, MapManager, VehicleInfoManager);
		}

		public static ITimeElapseDetector GenerateITimeElapseDetector()
		{
			return new TimeElapseDetector();
		}
		public static IDebugMessageHandler GenerateIDebugMessageHandler()
		{
			return new DebugMessageHandler();
		}
		public static ISignificantMessageHandler GenerateISignificantMessageHandler()
		{
			return new SignificantMessageHandler();
		}
		#endregion
	}
}
