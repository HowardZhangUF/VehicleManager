using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Configure
{
	public class Configurator : IConfigurator
	{
		/*
		 * 新增一個 Configuration 時：
		 * 1. Configurator 要新增其 Default 資訊
		 * 2. 對應的 ISystemWithConfig 的 GetConfig() 與 SetConfig() 與 GetConfigNameList() 方法要更新
		 * 3. Process 的 mCollectionOfISystemWithConfig 初始化方法 Constructor() 要更新
		 */

		public event EventHandler<ConfigFileLoadedEventArgs> ConfigFileLoaded;
		public event EventHandler<ConfigFileSavedEventArgs> ConfigFileSaved;
		public event EventHandler<ConfigurationUpdatedEventArgs> ConfigurationUpdated;

		public Language mLanguage { get; private set; } = Language.Zhtw;
		public string mFilePath { get; private set; } = string.Empty;
		public ProjectType rProjectType { get; private set; } = ProjectType.Common;

		private readonly Dictionary<string, Configuration> mConfigs = new Dictionary<string, Configuration>();

		public Configurator(string FilePath, ProjectType ProjectType)
		{
			Set(FilePath, ProjectType);
		}
		public void Set(string FilePath, ProjectType ProjectType)
		{
			if (!string.IsNullOrEmpty(FilePath))
			{
				mFilePath = FilePath;
			}
			rProjectType = ProjectType;
		}
		public void Load()
		{
			if (!string.IsNullOrEmpty(mFilePath))
			{
				GenerateDefaultConfiguration();
				UpdateConfigurationUsingProjectType();
				if (!File.Exists(mFilePath))
				{
					RaiseEvent_ConfigFileLoaded();
					Save();
					return;
				}
				else
				{
					string[] datas = File.ReadAllLines(mFilePath);
					foreach (string data in datas)
					{
						string[] keyValue = data.Split('=');
						if (keyValue.Length == 2)
						{
							if (mConfigs.Keys.Contains(keyValue[0]))
							{
								mConfigs[keyValue[0]].SetValue(keyValue[1]);
							}
						}
					}
					RaiseEvent_ConfigFileLoaded();
					Save();
				}
			}
		}
		public void Save()
		{
			if (!string.IsNullOrEmpty(mFilePath))
			{
				List<string> result = new List<string>();
				foreach (Configuration config in mConfigs.Values)
				{
					result.Add(config.ToString());
				}

				string directoryPath = Path.GetDirectoryName(mFilePath);
				if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

				File.WriteAllLines(mFilePath, result);
				RaiseEvent_ConfigFileSaved();
			}
		}
		public string GetValue(string Keyword)
		{
			if (mConfigs.Keys.Contains(Keyword))
			{
				return mConfigs[Keyword].mValue;
			}
			else
			{
				return null;
			}
		}
		public bool SetValue(string Keyword, string Value)
		{
			bool result = false;
			if (mConfigs.Keys.Contains(Keyword))
			{
				if (mConfigs[Keyword].SetValue(Value))
				{
					result = true;
					RaiseEvent_ConfigurationUpdated(Keyword, mConfigs[Keyword]);
					Save();
				}
			}
			return result;
		}
		public List<string[]> GetConfigDataGridViewRowDataCollection()
		{
			return mConfigs.Values.Select(o => o.GetDataGridViewRowData(mLanguage)).ToList();
		}

		protected virtual void GenerateDefaultConfiguration()
		{
			List<Configuration> defaultConfigs = new List<Configuration>();
			defaultConfigs.Add(new Configuration(
				"LogExporter",
				"BaseDirectory",
				ConfigurationType.String,
				ConfigurationLevel.Normal,
				".\\..\\VehicleManagerData\\LogExport",
				string.Empty,
				string.Empty,
				"Base directory of saving exported log",
				"儲存輸出日誌的資料夾",
				"储存输出日志的资料夹"));
			defaultConfigs.Add(new Configuration(
				"LogExporter",
				"ExportDirectoryNamePrefix",
				ConfigurationType.String,
				ConfigurationLevel.Normal,
				"CASTEC_Log_VM_",
				string.Empty,
				string.Empty,
				"Prefix of exported log name",
				"輸出日誌名稱的前綴文字",
				"输出日志名称的前缀文字"));
			defaultConfigs.Add(new Configuration(
				"LogExporter",
				"ExportDirectoryNameTimeFormat",
				ConfigurationType.String,
				ConfigurationLevel.Normal,
				"yyyyMMdd",
				string.Empty,
				string.Empty,
				"Time format in exported log name",
				"輸出日誌名稱的日期文字的格式",
				"输出日志名称的日期文字的格式"));
			defaultConfigs.Add(new Configuration(
				"LogExporter",
				"ExportProjectInfo",
				ConfigurationType.Bool,
				ConfigurationLevel.Normal,
				"True",
				string.Empty,
				string.Empty,
				"Add project information to exported log name",
				"加入專案資訊至輸出日誌名稱",
				"加入专案资讯至输出日志名称"));
			defaultConfigs.Add(new Configuration(
				"LogRecorder",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"3000",
				"100",
				"60000",
				"Time period in millisecond of recording vehicle history",
				"記錄自走車歷史記錄的時間間隔 (ms)",
				"记录自走车历史记录的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"LogMaintainHandler",
				"DayOfMonthOfBackupCurrentLog",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"5",
				"1",
				"31",
				"Day of month of backing up current log",
				"備份當前 Log 的日期",
				"备份当前 Log 的日期"));
			defaultConfigs.Add(new Configuration(
				"LogMaintainHandler",
				"DayOfMonthOfDeleteOldLog",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"15",
				"1",
				"31",
				"Day of month of deleting back log.",
				"刪除舊 Log 的日期",
				"删除旧 Log 的日期"));
			defaultConfigs.Add(new Configuration(
				"DebugMessageHandler",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"300",
				"100",
				"60000",
				"Time period in millisecond of raising debug message event",
				"拋出偵錯訊息事件的時間間隔 (ms)",
				"抛出侦错讯息事件的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"SignificantMessageHandler",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"300",
				"100",
				"60000",
				"Time period in millisecond of raising significant message event",
				"拋出重要訊息事件(供使用者監看)的時間間隔 (ms)",
				"抛出重要讯息事件(供使用者监看)的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"TimeElapseDetector",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"700",
				"100",
				"60000",
				"Time period in millisecond of detecting whether date time changed",
				"檢測時間是否變化的時間間隔 (ms)",
				"检测时间是否变化的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"VehicleCommunicator",
				"LocalPort",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"8000",
				"1",
				"65535",
				"Port for vehicle connection",
				"給自走車連線的連接埠",
				"给自走车连线的连接埠"));
			defaultConfigs.Add(new Configuration(
				"VehicleCommunicator",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"100",
				"50",
				"60000",
				"Time period in millisecond of checking queue and handling socket send/receive message with vehicle",
				"確認佇列並處理與自走車傳送/接受訊息的時間間隔 (ms)",
				"确认伫列并处理与自走车传送/接受讯息的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"CollisionEventDetector",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"750",
				"100",
				"60000",
				"Time period in millisecond of detecting collision event",
				"偵測碰撞事件的時間間隔 (ms)",
				"侦测碰撞事件的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"CollisionEventDetector",
				"NeighborPointAmount",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"10",
				"3",
				"1000",
				"Using neighbor point amount when detecting collision event",
				"偵測會車事件時使用的鄰近點數量",
				"侦测会车事件时使用的邻近点数量"
				));
			defaultConfigs.Add(new Configuration(
				"CollisionEventDetector",
				"VehicleLocationScoreThreshold",
				ConfigurationType.Double,
				ConfigurationLevel.Normal,
				"30",
				"0",
				"100",
				"Detecting collision event when vehicle's location score in percentage is greater than or equal to this threshold",
				"當自走車的定位分數 (%) 高於等於此閾值時才會對其偵測會車事件",
				"当自走车的定位分数 (%) 高于等于此阈值时才会对其侦测会车事件"
				));
			defaultConfigs.Add(new Configuration(
				"VehicleControlHandler",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"300",
				"100",
				"60000",
				"Time period in millisecond of checking queue and sending control command to vehicle",
				"確認佇列並傳送控制指令給自走車的時間間隔 (ms)",
				"确认伫列并传送控制指令给自走车的时间间隔 (ms)"));
            defaultConfigs.Add(new Configuration(
                "VehicleControlUpdater",
                "TimePeriod",
                ConfigurationType.Int,
                ConfigurationLevel.Normal,
                "800",
                "100",
                "60000",
                "Time period in millisecond of checking control command send/execute timeout or not",
                "確認控制指令是否傳送/執行逾時的時間間隔 (ms)",
				"确认控制指令是否传送/执行逾时的时间间隔 (ms)"));
            defaultConfigs.Add(new Configuration(
                "VehicleControlUpdater",
                "TimeoutOfSendingVehicleControl",
                ConfigurationType.Int,
                ConfigurationLevel.Normal,
                "5",
                "1",
                "60",
                "Time in second of checking sending control successed or failed",
                "傳送控制後若過了此時間 (second) 仍未偵測到車子相應的變化，則判斷控制傳送失敗",
				"传送控制后若过了此时间 (second) 仍未侦测到车子相应的变化，则判断控制传送失败"));
            defaultConfigs.Add(new Configuration(
                "VehicleControlUpdater",
                "TimeoutOfExecutingVehicleControl",
                ConfigurationType.Int,
                ConfigurationLevel.Normal,
                "600",
                "10",
                "6000",
                "Time in second of checking executing control successed or failed",
                "執行控制後若過了此時間 (second) 任務仍未完成，則判斷控制執行失敗",
				"执行控制后若过了此时间 (second) 任务仍未完成，则判断控制执行失败"));
            defaultConfigs.Add(new Configuration(
                "VehicleControlUpdater",
                "ToleranceOfXOfArrivedTarget",
                ConfigurationType.Int,
                ConfigurationLevel.Normal,
                "100",
                "1",
                "10000",
                "Tolerance in millimeter of X of checking control is completed or failed",
                "判斷控制是否完成的座標 (X) 容許誤差 (mm)",
                "判断控制是否完成的座标 (X) 容许误差 (mm)"));
            defaultConfigs.Add(new Configuration(
                "VehicleControlUpdater",
                "ToleranceOfYOfArrivedTarget",
                ConfigurationType.Int,
                ConfigurationLevel.Normal,
                "100",
                "1",
                "10000",
                "Tolerance in millimeter of Y of checking control is completed or failed",
                "判斷控制是否完成的座標 (Y) 容許誤差 (mm)",
                "判断控制是否完成的座标 (Y) 容许误差 (mm)"));
            defaultConfigs.Add(new Configuration(
                "VehicleControlUpdater",
                "ToleranceOfTowardOfArrivedTarget",
                ConfigurationType.Int,
                ConfigurationLevel.Normal,
                "5",
                "1",
                "360",
                "Tolerance in degree of toward of checking control is completed or failed",
                "判斷控制是否完成的座標 (Toward) 容許誤差 (degree)",
                "判断控制是否完成的座标 (Toward) 容许误差 (degree)"));
			defaultConfigs.Add(new Configuration(
				"VehicleControlUpdater",
				"CheckCoordinateAccuracyAfterArrived",
				ConfigurationType.Bool,
				ConfigurationLevel.Normal,
				"False",
				string.Empty,
				string.Empty,
				"Check coordinate accuracy or not when vehicle finish control",
				"自走車完成控制後是否檢查座標誤差",
				"自走车完成控制后是否检查座标误差"));
			defaultConfigs.Add(new Configuration(
				"VehicleControlUpdater",
				"AutoDetectNonSystemControl",
				ConfigurationType.Bool,
				ConfigurationLevel.Normal,
				"True",
				string.Empty,
				string.Empty,
				"Auto add control to collection when vehicle is executing a control that is not from system",
				"自動偵測自走車所執行的非系統控制並將該控制加入至系統集合中",
				"自动侦测自走车所执行的非系统控制并将该控制加入至系统集合中"));
			defaultConfigs.Add(new Configuration(
				"HostCommunicator",
                "LocalPort",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"9000",
				"1",
				"65535",
				"Port for host connection",
				"給上位系統連線的連接埠",
				"给上位系统连线的连接埠"));
			defaultConfigs.Add(new Configuration(
				"HostCommunicator",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"100",
				"50",
				"60000",
				"Time period in millisecond of checking queue and handling received message with host",
				"確認佇列並處理與上位系統傳送/接受訊息的時間間隔 (ms)",
				"确认伫列并处理与上位系统传送/接受讯息的时间间隔 (ms)"));
            defaultConfigs.Add(new Configuration(
                "HostMessageAnalyzer",
                "FilterDuplicateMissionWhenReceivedCommand",
                ConfigurationType.Bool,
                ConfigurationLevel.Normal,
                "True",
                string.Empty,
                string.Empty,
                "When received command, if there already exists a mission with same content in mission collection, then the new mission will not be added.",
                "解析訊息時，若任務集合中已有相同內容的任務，則不將該新任務加入",
                "解析讯息时，若任务集合中已有相同内容的任务，则不将该新任务加入"));
            defaultConfigs.Add(new Configuration(
				"MissionDispatcher",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1000",
				"100",
				"60000",
				"Time period in millisecond of checking queue and dispatching mission to vehicle",
				"確認佇列並分配任務給自走車的時間間隔 (ms)",
				"确认伫列并分配任务给自走车的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"MissionDispatcher",
				"DispatchRule",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"0",
				"0",
				"1",
				"Dispatch mission rule. 0 -> Priority > ReceivedTime. 1 -> SpecifyVehicle > Priority > ReceivedTime. (for Thinflex project)",
				"任務派送順序。 0 為先看優先度再看接收時間點。 1 為先看是否有指定車再看優先度再看接收時間點(新陽專案使用)",
				"任务派送顺序。 0 为先看优先度再看接收时间点。 1 为先看是否有指定车再看优先度再看接收时间点(新阳专案使用)"));
			defaultConfigs.Add(new Configuration(
				"MissionDispatcher",
				"IdlePeriodThreshold",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1000",
				"100",
				"60000",
				"System will dispatch mission to vehicle if its idle time is more than this threshold (in millisecond)",
				"系統會派任務給閒置時間 (ms) 超過此閾值的自走車",
				"系统会派任务给闲置时间 (ms) 超过此阈值的自走车"));
			defaultConfigs.Add(new Configuration(
				"MissionUpdater",
				"AutoDetectNonSystemMission",
				ConfigurationType.Bool,
				ConfigurationLevel.Normal,
				"True",
				string.Empty,
				string.Empty,
				"Auto add mission to queue when vehicle is executing a mission that is not from system",
				"自動偵測自走車所執行的非系統任務並將該任務加入至系統佇列中",
				"自动侦测自走车所执行的非系统任务并将该任务加入至系统伫列中"));
			defaultConfigs.Add(new Configuration(
				"MapFileManager",
				"MapManagementSetting",
				ConfigurationType.String,
				ConfigurationLevel.Normal,
				"{\"mRegionSettings\":{\"0\":{\"mRegionId\":0,\"mRegionName\":\"Region000\",\"mRegionMember\":\"\",\"mCurrentMapName\":\"\",\"mCurrentMapRange\":\"\"}},\"mMapFileDirectory\":\".\\\\..\\\\VehicleManagerData\\\\Map\"}",
				string.Empty,
				string.Empty,
				"Setting of multi map management (is json string)",
				"多地圖管理的設定(為 json 字串)",
				"多地图管理的设定(为 json 字串)"));
			defaultConfigs.Add(new Configuration(
				"MapManagerUpdater",
				"AutoLoadMap",
				ConfigurationType.Bool,
				ConfigurationLevel.Normal,
				"True",
				string.Empty,
				string.Empty,
				"Auto load map when vehicle's current map was changed",
				"當自走車當前使用地圖改變時，自身重新讀取地圖",
				"当自走车当前使用地图改变时，自身重新读取地图"));
			defaultConfigs.Add(new Configuration(
				"MapManagerUpdater",
				"IntegratedMapFileName",
				ConfigurationType.String,
				ConfigurationLevel.Normal,
				"Integrated.map",
				string.Empty,
				string.Empty,
				"Map File Name of combine all maps",
				"組合後的地圖的檔案名稱",
				"组合后的地图的档案名称"));
			defaultConfigs.Add(new Configuration(
				"MapManagerUpdater",
				"MapManagementSetting",
				ConfigurationType.String,
				ConfigurationLevel.Normal,
				"{\"mRegionSettings\":{\"0\":{\"mRegionId\":0,\"mRegionName\":\"Region000\",\"mRegionMember\":\"\",\"mCurrentMapName\":\"\",\"mCurrentMapRange\":\"\"}},\"mMapFileDirectory\":\".\\\\..\\\\VehicleManagerData\\\\Map\"}",
				string.Empty,
				string.Empty,
				"Setting of multi map management (is json string)",
				"多地圖管理的設定(為 json 字串)",
				"多地图管理的设定(为 json 字串)"));
			defaultConfigs.Add(new Configuration(
				"CycleMissionGenerator",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1500",
				"100",
				"60000",
				"Time period in millisecond of checking vehicle state and generating next mission",
				"確認自走車狀態並產生下一個任務的時間間隔 (ms)",
				"确认自走车状态并产生下一个任务的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"AutomaticDoorCommunicator",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"5000",
				"100",
				"60000",
				"Time period in millisecond of connecting to automatic door",
				"嘗試連線至自動門的時間間隔 (ms)",
				"尝试连线至自动门的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"AutomaticDoorCommunicator",
				"AutoConnect",
				ConfigurationType.Bool,
				ConfigurationLevel.Normal,
				"True",
				string.Empty,
				string.Empty,
				"Auto connect to automatic door",
				"是否自動連線至自動門",
				"是否自动连线至自动门"));
			defaultConfigs.Add(new Configuration(
				"AutomaticDoorControlHandler",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"500",
				"100",
				"60000",
				"Time period in millisecond of handling automatic door control",
				"處理自動門控制的時間間隔 (ms)",
				"处理自动门控制的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"VehiclePassThroughAutomaticDoorEventManagerUpdater",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1000",
				"100",
				"60000",
				"Time period in millisecond of detecting vehicle pass through automatic door event",
				"偵測車子通過自動門事件的時間間隔 (ms)",
				"侦测车子通过自动门事件的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"VehiclePassThroughAutomaticDoorEventManagerUpdater",
				"OpenDoorDistance",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"2500",
				"100",
				"100000",
				"Distance threshold in millimeter of opening automatic door",
				"當車子與自動門的距離 (mm) 小於此數值時開啟自動門",
				"当车子与自动门的距离 (mm) 小于此数值时开启自动门"));
			defaultConfigs.Add(new Configuration(
				"VehiclePassThroughAutomaticDoorEventManagerUpdater",
				"CloseDoorDistance",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"2500",
				"100",
				"100000",
				"Distance threshold in millimeter of closing automatic door",
				"當車子與自動門的距離 (mm) 大於此數值時關閉自動門",
				"当车子与自动门的距离 (mm) 大于此数值时关闭自动门"));
			defaultConfigs.Add(new Configuration(
				"ChargeStationInfoManagerUpdater",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1000",
				"100",
				"60000",
				"Time period in millisecond of detecting charge station is being used or not",
				"偵測充電站是否被使用的時間間隔 (ms)",
				"侦测充电站是否被使用的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"ChargeStationInfoManagerUpdater",
				"ChargeStationLocationRangeDistance",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"-900",
				"-100000",
				"100000",
				"Distance bewteen goal location and actual location range center",
				"充電站實際位置與站點的距離",
				"充电站实际位置与站点的距离"));
			defaultConfigs.Add(new Configuration(
				"ChargeStationInfoManagerUpdater",
				"ChargeStationLocationRangeWidth",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1200",
				"100",
				"100000",
				"Actual location range width",
				"充電站實際位置範圍邊長",
				"充电站实际位置范围边长"));
			defaultConfigs.Add(new Configuration(
				"LimitVehicleCountZoneInfoManagerUpdater",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1000",
				"100",
				"60000",
				"Time period in millisecond of calculating vehicle count of limit vehicle count zone",
				"計算區域的自走車數量的時間間隔 (ms)",
				"计算区域的自走车数量的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"VehiclePassThroughLimitVehicleCountZoneEventManagerUpdater",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1000",
				"100",
				"60000",
				"Time period in millisecond of detecting vehicle pass through limit vehicle count zone event",
				"偵測車子通過限車區事件的時間間隔 (ms)",
				"侦测车子通过限车区事件的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"VehiclePassThroughLimitVehicleCountZoneEventManagerUpdater",
				"DistanceThreshold",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"2500",
				"100",
				"100000",
				"Distance threshold in millimeter of intervening vehicle",
				"當自走車與限車區的距離 (mm) 小於此數值時才進行干預",
				"当自走车与限车区的距离 (mm) 小于此数值时才进行干预"));
			defaultConfigs.Add(new Configuration(
				"VehiclePassThroughLimitVehicleCountZoneEventHandler",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1000",
				"100",
				"60000",
				"Time period in millisecond of handle vehicle pass through limit vehicle count zone event",
				"處理車子通過限車區事件的時間間隔 (ms)",
				"处理车子通过限车区事件的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"ParkStationInfoManagerUpdater",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1000",
				"100",
				"60000",
				"Time period in millisecond of detecting park station is being used or not",
				"偵測停車點是否被使用的時間間隔 (ms)",
				"侦测停车点是否被使用的时间间隔 (ms)"));
			defaultConfigs.Add(new Configuration(
				"ParkStationInfoManagerUpdater",
				"ParkStationLocationRangeWidth",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1600",
				"100",
				"100000",
				"Actual location range width",
				"停車點實際位置範圍邊長",
				"停车点实际位置范围边长"));

			mConfigs.Clear();
			for (int i = 0; i < defaultConfigs.Count; ++i)
			{
				mConfigs.Add(defaultConfigs[i].mFullName, defaultConfigs[i]);
			}
		}
		protected virtual void UpdateConfigurationUsingProjectType()
		{
			switch (rProjectType)
			{
				case ProjectType.E2029_ThinFlex:
					mConfigs["MissionDispatcher/DispatchRule"].SetValue("1");
					mConfigs["MissionDispatcher/IdlePeriodThreshold"].SetValue("5000");
					mConfigs["VehiclePassThroughAutomaticDoorEventManagerUpdater/CloseDoorDistance"].SetValue("1000");
					break;
				case ProjectType.E2113_Unimicron:
					mConfigs["MapFileManager/MapManagementSetting"].SetValue("{\"mRegionSettings\":{\"0\":{\"mRegionId\":0,\"mRegionName\":\"Region000\",\"mRegionMember\":\"\",\"mCurrentMapName\":\"combine_20220121_VM.map\",\"mCurrentMapRange\":\"\"},\"1\":{\"mRegionId\":1,\"mRegionName\":\"4F-B-RC\",\"mRegionMember\":\"iTS-P-B403,iTS-P-B405\",\"mCurrentMapName\":\"20220119_VM.map\",\"mCurrentMapRange\":\"\"},\"2\":{\"mRegionId\":2,\"mRegionName\":\"4F-B-High-Front\",\"mRegionMember\":\"iTS-P-B401\",\"mCurrentMapName\":\"\",\"mCurrentMapRange\":\"\"},\"3\":{\"mRegionId\":3,\"mRegionName\":\"4F-B-High-Back\",\"mRegionMember\":\"iTS-P-B402\",\"mCurrentMapName\":\"\",\"mCurrentMapRange\":\"\"}},\"mMapFileDirectory\":\".\\\\..\\\\VehicleManagerData\\\\Map\"}");
					mConfigs["MapManagerUpdater/MapManagementSetting"].SetValue("{\"mRegionSettings\":{\"0\":{\"mRegionId\":0,\"mRegionName\":\"Region000\",\"mRegionMember\":\"\",\"mCurrentMapName\":\"combine_20220121_VM.map\",\"mCurrentMapRange\":\"\"},\"1\":{\"mRegionId\":1,\"mRegionName\":\"4F-B-RC\",\"mRegionMember\":\"iTS-P-B403,iTS-P-B405\",\"mCurrentMapName\":\"20220119_VM.map\",\"mCurrentMapRange\":\"\"},\"2\":{\"mRegionId\":2,\"mRegionName\":\"4F-B-High-Front\",\"mRegionMember\":\"iTS-P-B401\",\"mCurrentMapName\":\"\",\"mCurrentMapRange\":\"\"},\"3\":{\"mRegionId\":3,\"mRegionName\":\"4F-B-High-Back\",\"mRegionMember\":\"iTS-P-B402\",\"mCurrentMapName\":\"\",\"mCurrentMapRange\":\"\"}},\"mMapFileDirectory\":\".\\\\..\\\\VehicleManagerData\\\\Map\"}");
					mConfigs["VehiclePassThroughLimitVehicleCountZoneEventManagerUpdater/DistanceThreshold"].SetValue("1300");
					break;
				case ProjectType.Common:
					break;
			}
		}
		protected virtual void RaiseEvent_ConfigFileLoaded(bool Sync = true)
		{
			if (Sync)
			{
				ConfigFileLoaded?.Invoke(this, new ConfigFileLoadedEventArgs(DateTime.Now, mFilePath));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ConfigFileLoaded?.Invoke(this, new ConfigFileLoadedEventArgs(DateTime.Now, mFilePath)); });
			}
		}
		protected virtual void RaiseEvent_ConfigFileSaved(bool Sync = true)
		{
			if (Sync)
			{
				ConfigFileSaved?.Invoke(this, new ConfigFileSavedEventArgs(DateTime.Now, mFilePath));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ConfigFileSaved?.Invoke(this, new ConfigFileSavedEventArgs(DateTime.Now, mFilePath)); });
			}
		}
		protected virtual void RaiseEvent_ConfigurationUpdated(string ConfigName, Configuration Configuration, bool Sync = true)
		{
			if (Sync)
			{
				ConfigurationUpdated?.Invoke(this, new ConfigurationUpdatedEventArgs(DateTime.Now, ConfigName, Configuration));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ConfigurationUpdated?.Invoke(this, new ConfigurationUpdatedEventArgs(DateTime.Now, ConfigName, Configuration)); });
			}
		}
	}
}
