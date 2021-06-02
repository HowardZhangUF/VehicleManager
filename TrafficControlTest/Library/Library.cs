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
using TrafficControlTest.Module.Communication;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.Configure;
using TrafficControlTest.Module.CycleMission;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Log;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.VehiclePassThroughAutomaticDoor;

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

		public static T GetDeepClone<T>(T o)
		{
			using (var ms = new System.IO.MemoryStream())
			{
				var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				formatter.Serialize(ms, o);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}
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
		public static IPoint2D GenerateIPoint2D(int X, int Y)
		{
			return new Point2D(X, Y);
		}
		public static ITowardPoint2D GenerateITowardPoint2D(int X, int Y, double Toward)
		{
			return new TowardPoint2D(X, Y, Toward);
		}
		public static ITowardPoint2D GenerateITowardPoint2D(IPoint2D Point, double Toward)
		{
			return new TowardPoint2D(Point.mX, Point.mY, Toward);
		}
		public static IVector2D GenerateIVector2D(double XComponent, double YComponent)
		{
			return new Vector2D(XComponent, YComponent);
		}
		public static IRectangle2D GenerateIRectangle2D(IPoint2D MaxPoint, IPoint2D MinPoint)
		{
			return new Rectangle2D(MaxPoint, MinPoint);
		}
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
		public static ICommunicatorClient GenerateICommunicatorClient()
		{
			return new CommunicatorClient();
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
		public static ILogRecorder GenerateILogRecorder(DatabaseAdapter DatabaseAdapter)
		{
			return new LogRecorder(DatabaseAdapter);
		}
		public static IEventRecorder GenerateIEventRecorder(DatabaseAdapter DatabaseAdapter)
		{
			return new EventRecorder(DatabaseAdapter);
		}
		public static IImportantEventRecorder GenerateIImportantEventRecorder(IEventRecorder EventRecorder, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IVehicleControlManager VehicleControlManager, IAutomaticDoorControlManager AutomaticDoorControlManager, IHostCommunicator HostCommunicator)
		{
			return new ImportantEventRecorder(EventRecorder, VehicleInfoManager, MissionStateManager, VehicleControlManager, AutomaticDoorControlManager, HostCommunicator);
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

		#region IPoint2D
		/// <summary>計算向量 (End - Start) 與 X+ 的夾角。範圍為 -180 ~ 180 ，單位為 degree</summary>
		public static double GetAngle(IPoint2D Start, IPoint2D End)
		{
			return Math.Atan2(End.mY - Start.mY, End.mX - Start.mX) / Math.PI * 180.0f;
		}
		public static double GetDistance(IPoint2D Point1, IPoint2D Point2)
		{
			return Math.Sqrt(GetDistanceSquare(Point1, Point2));
		}
		/// <summary>計算點集合依序連起來的線段的長度總和</summary>
		public static double GetDistance(IEnumerable<IPoint2D> Points)
		{
			double result = 0;
			if (Points.Count() > 1)
			{
				for (int i = 1; i < Points.Count(); ++i)
				{
					result += GetDistance(Points.ElementAt(i - 1), Points.ElementAt(i));
				}
			}
			return result;
		}
		public static int GetDistanceSquare(IPoint2D Point1, IPoint2D Point2)
		{
			return (int)(Math.Pow(Point2.mX - Point1.mX, 2) + Math.Pow(Point2.mY - Point1.mY, 2));
		}
		/// <summary>計算指定矩形與指定線段(由兩點組成)的交點。最多有可能會有兩個交點</summary>
		public static IEnumerable<IPoint2D> GetIntersectionPoint(IRectangle2D Rectangle, IPoint2D Point1, IPoint2D Point2)
		{
			List<IPoint2D> result = null;
			if (Rectangle != null && Point1 != null && Point2 != null)
			{
				result = new List<IPoint2D>();
				// 計算矩形的四個邊與兩點組成的線段的交點
				IPoint2D tmp = null;
				tmp = GetIntersectionPoint(Point1, Point2, GenerateIPoint2D(Rectangle.mMaxX, Rectangle.mMaxY), GenerateIPoint2D(Rectangle.mMaxX, Rectangle.mMinY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
				tmp = GetIntersectionPoint(Point1, Point2, GenerateIPoint2D(Rectangle.mMaxX, Rectangle.mMaxY), GenerateIPoint2D(Rectangle.mMinX, Rectangle.mMaxY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
				tmp = GetIntersectionPoint(Point1, Point2, GenerateIPoint2D(Rectangle.mMinX, Rectangle.mMinY), GenerateIPoint2D(Rectangle.mMaxX, Rectangle.mMinY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
				tmp = GetIntersectionPoint(Point1, Point2, GenerateIPoint2D(Rectangle.mMinX, Rectangle.mMinY), GenerateIPoint2D(Rectangle.mMinX, Rectangle.mMaxY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
			}
			return result;
		}
		/// <summary>計算兩線段(由兩點組成)的交點</summary>
		public static IPoint2D GetIntersectionPoint(IPoint2D Line1Point1, IPoint2D Line1Point2, IPoint2D Line2Point1, IPoint2D Line2Point2)
		{
			/*
			 * 參考資料：https://www.cnblogs.com/sanmubai/p/7306599.html
			 * 
			 * 線段 AB 其端點為 (xa, ya) 與 (xb, yb) ，其直線方程為：
			 * x = xa + lambda * (xb - xa)
			 * y = ya + lambda * (yb - ya)
			 * 0 <= lambda <= 1
			 * 
			 * 線段 CD 其端點為 (xc, yc) 與 (xd, yd) ，其直線方程為：
			 * x = xc + micro * (xd - xc)
			 * y = yc + micro * (yd - yc)
			 * 0 <= micro <= 1
			 * 
			 * 則交點應滿足：
			 * x = xa + lambda * (xb - xa) = xc + micro * (xd - xc)
			 * y = ya + lambda * (yb - ya) = yc + micro * (yd - yc)
			 * 
			 * 可整理成：
			 * (xb - xa) * lambda - (xd - xc) * micro = xc - xa
			 * (yb - ya) * lambda - (yd - yc) * micro = yc - ya
			 * 
			 * 行列式 delta 的算法為：
			 * A = |(xb - xa) -(xd - xc)|
			 * 	   |(yb - ya) -(yd - yc)|
			 * delta = (xb - xa) * (-(yd - yc)) - (-(xd - xc)) * (yb - ya)
			 * 	     = (xb - xa) * (yc - yd)    - (xc - xd)    * (yb - ya)
			 * 
			 * 若其行列式等於零，表示線段 AB 與線段 CD 重合或平行。
			 * 
			 * 若其行列式不等於零，則可求出：
			 * lambda = 1 / delta * det|(xc - xa) -(xd - xc)|
			 * 						   |(yc - ya) -(yd - yc)|
			 * 		  = 1 / delta * ((xc - xa) * (yc - yd) - (xc - xd) * (yc - ya))
			 * mircro = 1 / delta * det|(xb - xa) (xc - xa)|
			 * 						   |(yb - ya) (yc - ya)|
			 * 	      = 1 / delta * ((xb - xa) * (yc - ya) - (xc - xa) * (yb - ya))
			 * 
			 * 需特別注意，僅有當 0 <= lambda <= 1 且 0 <= micro <= 1 時，兩線段才有相交，
			 * 否則，交點在線段的延長線上，仍認為兩線段不相交。
			 * 
			 * 算出 lambda 與 micro 後，可得交點為：
			 * x = xa + lambda * (xb - xa)
			 * y = ya + lambda * (yb - ya)
			 */
			IPoint2D result = null;
			if (Line1Point1 != null && Line1Point2 != null && Line2Point1 != null && Line2Point2 != null)
			{
				double delta = (Line1Point2.mX - Line1Point1.mX) * (Line2Point1.mY - Line2Point2.mY) - (Line2Point1.mX - Line2Point2.mX) * (Line1Point2.mY - Line1Point1.mY);
				if (delta <= double.Epsilon && delta >= -double.Epsilon) //若 delta 為 0
				{
					result = null;
				}
				else
				{
					double lambda = ((Line2Point1.mX - Line1Point1.mX) * (Line2Point1.mY - Line2Point2.mY) - (Line2Point1.mX - Line2Point2.mX) * (Line2Point1.mY - Line1Point1.mY)) / delta;
					if (0 <= lambda && lambda <= 1)
					{
						double micro = ((Line1Point2.mX - Line1Point1.mX) * (Line2Point1.mY - Line1Point1.mY) - (Line2Point1.mX - Line1Point1.mX) * (Line1Point2.mY - Line1Point1.mY)) / delta;
						if (0 <= micro && micro <= 1)
						{
							int x = Line1Point1.mX + (int)(lambda * (Line1Point2.mX - Line1Point1.mX));
							int y = Line1Point1.mY + (int)(lambda * (Line1Point2.mY - Line1Point1.mY));
							result = GenerateIPoint2D(x, y);
						}
					}
				}
			}
			return result;
		}
		/// <summary>將一條線轉換成點集合(不包含該線的兩端點)</summary>
		public static IEnumerable<IPoint2D> ConvertLineToPoints(IPoint2D Start, IPoint2D End, int Interval)
		{
			List<IPoint2D> result = new List<IPoint2D>();

			int diffX = Math.Abs(Start.mX - End.mX);
			int diffY = Math.Abs(Start.mY - End.mY);
			// 斜率為無限大
			if (diffX == 0)
			{
				if (Start.mY < End.mY)
				{
					for (int i = Start.mY + Interval; i < End.mY; i += Interval)
					{
						result.Add(GenerateIPoint2D(Start.mX, i));
					}
				}
				else
				{
					for (int i = Start.mY - Interval; i > End.mY; i -= Interval)
					{
						result.Add(GenerateIPoint2D(Start.mX, i));
					}
				}
			}
			// 斜率為零
			else if (diffY == 0)
			{
				if (Start.mX < End.mX)
				{
					for (int i = Start.mX + Interval; i < End.mX; i += Interval)
					{
						result.Add(GenerateIPoint2D(i, Start.mY));
					}
				}
				else
				{
					for (int i = Start.mX - Interval; i > End.mX; i -= Interval)
					{
						result.Add(GenerateIPoint2D(i, Start.mY));
					}
				}
			}
			else
			{
				// y = mx + c
				// x = (y - c) / m
				double m = (double)(Start.mY - End.mY) / (Start.mX - End.mX);
				double c = Start.mY - m * Start.mX;
				double radian = Math.Atan((Start.mY - End.mY) / (Start.mX - End.mX));
				double distance = Math.Abs((Interval * Math.Cos(radian)));
				if (Start.mX < End.mX)
				{
					for (double i = Start.mX + distance; i < End.mX; i += distance)
					{
						double y = Math.Round(m * i + c, 0, MidpointRounding.AwayFromZero);
						result.Add(GenerateIPoint2D((int)Math.Round(i, 0, MidpointRounding.AwayFromZero), (int)y));
					}
				}
				else
				{
					for (double i = Start.mX - distance; i > End.mX; i -= distance)
					{
						double y = Math.Round(m * i + c, 0, MidpointRounding.AwayFromZero);
						result.Add(GenerateIPoint2D((int)Math.Round(i, 0, MidpointRounding.AwayFromZero), (int)y));
					}
				}
			}
			return result;
		}
		public static string ConvertToString(IEnumerable<IPoint2D> Points)
		{
			string result = string.Empty;
			result = string.Join(" ", Points.Select((o) => o.ToString()));
			return result;
		}
		#endregion

		#region IVector2D
		public static IVector2D GetVector(int X1, int Y1, int X2, int Y2)
		{
			IVector2D result = null;
			result = GenerateIVector2D(X2 - X1, Y2 - Y1);
			return result;
		}
		public static IVector2D GetNormalizeVector(IVector2D Vector)
		{
			IVector2D result = null;
			result = GenerateIVector2D(Vector.mXComponent / Vector.mMagnitude, Vector.mYComponent / Vector.mMagnitude);
			return result;
		}
		public static double GetDotProduct(IVector2D Vector1, IVector2D Vector2)
		{
			double result = 0;
			if (Vector1 != null && Vector2 != null)
			{
				result = Vector1.mXComponent * Vector2.mXComponent + Vector1.mYComponent * Vector2.mYComponent;
			}
			return result;
		}
		public static double GetAngleOfTwoVector(IVector2D Vector1, IVector2D Vector2)
		{
			double result = -1;
			if (Vector1 != null && Vector2 != null)
			{
				result = (Math.Acos(GetDotProduct(Vector1, Vector2) / (Vector1.mMagnitude * Vector2.mMagnitude)) / Math.PI * 180);
			}
			return result;
		}
		#endregion

		#region IRectangle2D
		/// <summary>計算指定點與指定矩形的邊的距離</summary>
		public static int GetDistanceBetweenPointAndRectangleEdge(IPoint2D Point, IRectangle2D Rectangle)
		{
			int result = 0;
			if (!Rectangle.IsIncludePoint(Point))
			{
				IPoint2D rectangleCenter = GenerateIPoint2D((Rectangle.mMaxX + Rectangle.mMinX) / 2, (Rectangle.mMaxY + Rectangle.mMinY) / 2);
				IEnumerable<IPoint2D> intersectionPoints = GetIntersectionPoint(Rectangle, Point, rectangleCenter);
				if (intersectionPoints.Count() == 1)
				{
					result = (int)GetDistance(Point, intersectionPoints.ElementAt(0));
				}
			}
			return result;
		}
		/// <summary>判斷兩個矩形是否有重疊。共用同一個邊不算重疊</summary>
		public static bool IsRectangleOverlap(IRectangle2D Rectangle1, IRectangle2D Rectangle2)
		{
			if (Rectangle1 != null && Rectangle2 != null && Rectangle1.mMaxX > Rectangle2.mMinX && Rectangle2.mMaxX > Rectangle1.mMinX && Rectangle1.mMaxY > Rectangle2.mMinY && Rectangle2.mMaxY > Rectangle1.mMinY)
				return true;
			else
				return false;
		}
		/// <summary>判斷指定點是否在指定矩形內。在矩形邊上也算是在矩形內</summary>
		public static bool IsPointInside(IPoint2D Point, IRectangle2D Rectangle)
		{
			return (Point.mX >= Rectangle.mMinX && Point.mX <= Rectangle.mMaxX && Point.mY >= Rectangle.mMinY && Point.mY <= Rectangle.mMaxY);
		}
		/// <summary>判斷指定線段(點集合)是否有穿越指定矩形</summary>
		public static bool IsLinePassThroughRectangle(IEnumerable<IPoint2D> Points, IRectangle2D Rectangle)
		{
			bool result = false;
			for (int i = 0; i < Points.Count() - 1; ++i)
			{
				if (GetIntersectionPoint(Rectangle, Points.ElementAt(i), Points.ElementAt(i + 1)).Count() > 0)
				{
					result = true;
					break;
				}
			}
			return result;
		}
		/// <summary>計算能涵蓋兩個指定矩形的矩形</summary>
		public static IRectangle2D GetCoverRectangle(IRectangle2D Rectangle1, IRectangle2D Rectangle2)
		{
			IRectangle2D result = null;
			int x_max = Math.Max(Rectangle1.mMaxX, Rectangle2.mMaxX);
			int y_max = Math.Max(Rectangle1.mMaxY, Rectangle2.mMaxY);
			int x_min = Math.Min(Rectangle1.mMinX, Rectangle2.mMinX);
			int y_min = Math.Min(Rectangle1.mMinY, Rectangle2.mMinY);
			if (x_min < x_max && y_min < y_max)
			{
				result = GenerateIRectangle2D(GenerateIPoint2D(x_max, y_max), GenerateIPoint2D(x_min, y_min));
			}
			return result;
		}
		/// <summary>計算兩個矩形的交集矩形</summary>
		public static IRectangle2D GetIntersectionRectangle(IRectangle2D Rectangle1, IRectangle2D Rectangle2)
		{
			IRectangle2D result = null;
			int x_max = Math.Min(Rectangle1.mMaxX, Rectangle2.mMaxX);
			int y_max = Math.Min(Rectangle1.mMaxY, Rectangle2.mMaxY);
			int x_min = Math.Max(Rectangle1.mMinX, Rectangle2.mMinX);
			int y_min = Math.Max(Rectangle1.mMinY, Rectangle2.mMinY);
			if (x_min < x_max && y_min < y_max)
			{
				result = GenerateIRectangle2D(GenerateIPoint2D(x_max, y_max), GenerateIPoint2D(x_min, y_min));
			}
			return result;
		}
		/// <summary>以指定點為中心，產生一個邊長為 Radius * 2 大小的正方形</summary>
		public static IRectangle2D GetRectangle(int X, int Y, int Radius)
		{
			IRectangle2D result = null;
			if (Radius > 0)
			{
				result = GenerateIRectangle2D(GenerateIPoint2D(X + Radius, Y + Radius), GenerateIPoint2D(X - Radius, Y - Radius));
			}
			return result;
		}
		/// <summary>將指定 Rectangle 放大。以其中心為基準， X 軸方向增加 OffsetX ， Y 軸方向增加 OffsetY</summary>
		public static IRectangle2D GetAmplifyRectangle(IRectangle2D Rectangle, int OffsetX, int OffsetY)
		{
			IRectangle2D result = null;
			int x_max = Rectangle.mMaxX + OffsetX;
			int y_max = Rectangle.mMaxY + OffsetY;
			int x_min = Rectangle.mMinX - OffsetX;
			int y_min = Rectangle.mMinY - OffsetY;
			result = GenerateIRectangle2D(GenerateIPoint2D(x_max, y_max), GenerateIPoint2D(x_min, y_min));
			return result;
		}
		/// <summary>計算能涵蓋指定 Point 集合的矩形</summary>
		public static IRectangle2D GetCoverRectangle(IEnumerable<IPoint2D> Points)
		{
			IRectangle2D result = null;
			if (Points != null && Points.Count() > 0)
			{
				result = GenerateIRectangle2D(GetDeepClone(Points.ElementAt(0)), GetDeepClone(Points.ElementAt(0)));
				for (int i = 1; i < Points.Count(); ++i)
				{
					if		(Points.ElementAt(i).mX < result.mMinX) result.mMinPoint.mX = Points.ElementAt(i).mX;
					else if (Points.ElementAt(i).mX > result.mMaxX) result.mMaxPoint.mX = Points.ElementAt(i).mX;
					if		(Points.ElementAt(i).mY < result.mMinY) result.mMinPoint.mY = Points.ElementAt(i).mY;
					else if (Points.ElementAt(i).mY > result.mMaxY) result.mMaxPoint.mY = Points.ElementAt(i).mY;
				}
			}
			return result;
		}
		/// <summary>合併矩形。若其中任兩個矩形有重疊，則將其合併成一個矩形</summary>
		public static IEnumerable<IRectangle2D> MergeRectangle(IEnumerable<IRectangle2D> Rectangles)
		{
			List<IRectangle2D> result = null;
			result = GetDeepClone(Rectangles).ToList();
			if (result != null && result.Count() > 1)
			{
				for (int i = 0; i < result.Count(); ++i)
				{
					for (int j = i + 1; j < result.Count(); ++j)
					{
						if (IsRectangleOverlap(result[i], result[j]))
						{
							result.Add(GetCoverRectangle(result[i], result[j]));
							result.RemoveAt(j);
							result.RemoveAt(i);

							// 回頭再檢查一次
							i = 0 - 1;
							j = i + 1;
							break;
						}
					}
				}
			}
			return result;
		}
		#endregion
	}

	public static class EventHandlerLibrary
	{
		public delegate void EventHandlerDateTime(DateTime OccurTime);
		public delegate void EventHandlerRemoteConnectState(DateTime OccurTime, string IpPort, ConnectState NewState);
		public delegate void EventHandlerLocalListenState(DateTime OccurTime, ListenState NewState, int Port);
		public delegate void EventHandlerSentSerializableData(DateTime OccurTime, string IpPort, object Data);
		public delegate void EventHandlerReceivedSerializableData(DateTime OccurTime, string IpPort, object Data);
		public delegate void EventHandlerSentString(DateTime OccurTime, string IpPort, string Data);
		public delegate void EventHandlerReceivedString(DateTime OccurTime, string IpPort, string Data);
	}
}
