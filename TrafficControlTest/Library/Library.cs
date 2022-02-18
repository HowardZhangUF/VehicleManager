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
using TrafficControlTest.Module.NewCommunication;
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

	public static class Library
	{
		public const string TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

		private static byte[] mKey = Encoding.ASCII.GetBytes("castec27");
		private static byte[] mIv = Encoding.ASCII.GetBytes("27635744");
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
		public static string EncryptString(string Src)
		{
			string result = string.Empty;
			using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
			{
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(mKey, mIv), CryptoStreamMode.Write))
					{
						byte[] srcByte = Encoding.ASCII.GetBytes(Src);
						cs.Write(srcByte, 0, srcByte.Length);
						cs.FlushFinalBlock();
						result = Convert.ToBase64String(ms.ToArray());
					}
				}
			}
			return result;
		}
		public static string DecryptString(string Src)
		{
			string result = string.Empty;
			using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
			{
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(mKey, mIv), CryptoStreamMode.Write))
					{
						byte[] srcByte = Convert.FromBase64String(Src);
						cs.Write(srcByte, 0, srcByte.Length);
						cs.FlushFinalBlock();
						result = Encoding.ASCII.GetString(ms.ToArray());
					}
				}
			}
			return result;
		}

		#region Factory
		//public static IPoint2D GenerateIPoint2D(int X, int Y)
		//{
		//	return new Point2D(X, Y);
		//}
		//public static ITowardPoint2D GenerateITowardPoint2D(int X, int Y, double Toward)
		//{
		//	return new TowardPoint2D(X, Y, Toward);
		//}
		//public static ITowardPoint2D GenerateITowardPoint2D(IPoint2D Point, double Toward)
		//{
		//	return new TowardPoint2D(Point.mX, Point.mY, Toward);
		//}
		//public static IVector2D GenerateIVector2D(double XComponent, double YComponent)
		//{
		//	return new Vector2D(XComponent, YComponent);
		//}
		//public static IRectangle2D GenerateIRectangle2D(IPoint2D MaxPoint, IPoint2D MinPoint)
		//{
		//	return new Rectangle2D(MaxPoint, MinPoint);
		//}
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
			return new CollisionPair(Vehicle1, Vehicle2, CollisionRegion, Period, PassPeriodOfVehicle1WithCurrentVelocity, PassPeriodOfVehicle2WithCurrentVelocity, PassPeriodOfVehicle1WithMaximumVeloctiy, PassPeriodOfVehicle2WithMaximumVeloctiy);
		}
		public static ITimePeriod GenerateITimePeriod(DateTime Start, DateTime End)
		{
			return new TimePeriod(Start, End);
		}
		public static IVehicleInfo GenerateIVehicleInfo(string Name)
		{
			return new VehicleInfo(Name);
		}
		public static IVehicleCommunicator GenerateIVehicleCommunicator()
		{
			return new VehicleCommunicator();
		}
		public static IVehicleInfoManager GenerateIVehicleInfoManager()
		{
			return new VehicleInfoManager();
		}
		public static IVehicleInfoUpdater GenerateIVehicleInfoUpdater(IVehicleCommunicator VehicleCommunicator, IMissionStateManager MissionStateManager, IVehicleInfoManager VehicleInfoManager)
		{
			return new VehicleInfoUpdater(VehicleCommunicator, MissionStateManager, VehicleInfoManager);
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
			return new VehicleControl(VehicleId, Command, Parameters, CauseId, CauseDetail);
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
			return new MissionState(Mission);
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
		public static IMapFileManager GenerateIMapFileManager()
		{
			return new MapFileManager();
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
		public static IConfigurator GenerateIConfigurator(string FileName)
		{
			return new Configurator(FileName);
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
		public static ILogExporter GenerateILogExporter()
		{
			return new LogExporter();
		}
		public static IAutomaticDoorInfo GenerateIAutomaticDoorInfo(string Name, IRectangle2D Range, string IpPort)
		{
			return new AutomaticDoorInfo(Name, Range, IpPort);
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
			return new AutomaticDoorControl(AutomaticDoorName, Command, Cause);
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
			return new VehiclePassThroughAutomaticDoorEvent(VehicleName, AutomaticDoorName, Distance);
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
		public static IChargeStationInfo GenerateIChargeStationInfo(string Name, ITowardPoint2D Location)
        {
            return new ChargeStationInfo(Name, Location);
        }
        public static IChargeStationInfoManager GenerateIChargeStationInfoManager()
        {
            return new ChargeStationInfoManager();
        }
        public static IChargeStationInfoManagerUpdater GenerateIChargeStationInfoManagerUpdater(IChargeStationInfoManager ChargeStationInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
        {
            return new ChargeStationInfoManagerUpdater(ChargeStationInfoManager, MapManager, VehicleInfoManager);
        }
        public static ILimitVehicleCountZoneInfo GenerateILimitVehicleCountZoneInfo(string Name, IRectangle2D Range, int MaxVehicleCount)
		{
			return new LimitVehicleCountZoneInfo(Name, Range, MaxVehicleCount);
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
			return new VehiclePassThroughLimitVehicleCountZoneEvent(VehicleInfo, LimitVehicleCountZoneInfo, Distance);
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
