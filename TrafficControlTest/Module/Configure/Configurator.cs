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
		 * 2. 對應類別的 GetConfig() 與 SetConfig() 方法需要更新
		 * 3. VehicleManagerProcess 的 LoadConfigFileAndUpdateSystemConfig() 與 LoadSystemConfigAndUpdateConfigFile() 方法需要更新
		 * 4. VehicleManagerProcess 的 HandleEvent_ConfiguratorConfigurationUpdated() 方法需要更新
		 */

		public event EventHandler<ConfigFileLoadedEventArgs> ConfigFileLoaded;
		public event EventHandler<ConfigFileSavedEventArgs> ConfigFileSaved;
		public event EventHandler<ConfigurationUpdatedEventArgs> ConfigurationUpdated;

		public Language mLanguage { get; private set; } = Language.Enus;
		public string mFilePath { get; private set; } = string.Empty;

		private readonly Dictionary<string, Configuration> mConfigs = new Dictionary<string, Configuration>();

		public Configurator(string FilePath)
		{
			Set(FilePath);
		}
		public void Set(string FileName)
		{
			if (!string.IsNullOrEmpty(FileName))
			{
				mFilePath = FileName;
			}
		}
		public void Load()
		{
			if (!string.IsNullOrEmpty(mFilePath))
			{
				GenerateDefaultConfiguration();
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
				".\\LogExport",
				string.Empty,
				string.Empty,
				"Base Directory of Saving Exported Log",
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
				"Prefix of Exported Log Name",
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
				"Time Format in Exported Log Name",
				"輸出日誌名稱的日期文字的格式",
				"输出日志名称的日期文字的格式"));
			defaultConfigs.Add(new Configuration(
				"ImportantEventRecorder",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"3000",
				"500",
				"10000",
				"Time Period of Recording Vehicle History",
				"記錄自走車歷史記錄的時間間隔",
				"记录自走车历史记录的时间间隔"));
			defaultConfigs.Add(new Configuration(
				"VehicleCommunicator",
				"ListenPort",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"8000",
				"1025",
				"65535",
				"Port for Vehicle Connection",
				"給自走車連線的連接埠",
				"给自走车连线的连接埠"));
			defaultConfigs.Add(new Configuration(
				"VehicleCommunicator",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"100",
				"50",
				"1000",
				"Time Period of Checking Queue and Handling Socket Send/Receive Message with Vehicle",
				"確認佇列並處理與自走車傳送/接受訊息的時間間隔",
				"确认伫列并处理与自走车传送/接受讯息的时间间隔"));
			defaultConfigs.Add(new Configuration(
				"CollisionEventDetector",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"750",
				"300",
				"1000",
				"Time Period of Detecting Collision Event",
				"偵測碰撞事件的時間間隔",
				"侦测碰撞事件的时间间隔"));
			defaultConfigs.Add(new Configuration(
				"CollisionEventDetector",
				"NeighborPointAmount",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"10",
				"3",
				"100",
				"Using Neighbor Point Amount when Detecting Collision Event",
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
				"Detecting Collision Event when Vehicle's Location Score Greater than this Threshold",
				"當自走車的定位分數高於此閾值時才會對其偵測會車事件",
				"当自走车的定位分数高于此阈值时才会对其侦测会车事件"
				));
			defaultConfigs.Add(new Configuration(
				"VehicleControlHandler",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"300",
				"100",
				"1000",
				"Time Period of Checking Queue and Sending Control Command to Vehicle",
				"確認佇列並傳送控制指令給自走車的時間間隔",
				"确认伫列并传送控制指令给自走车的时间间隔"));
			defaultConfigs.Add(new Configuration(
				"HostCommunicator",
				"ListenPort",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"9000",
				"1025",
				"65535",
				"Port for Host Connection",
				"給上位系統連線的連接埠",
				"给上位系统连线的连接埠"));
			defaultConfigs.Add(new Configuration(
				"HostCommunicator",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"100",
				"50",
				"1000",
				"Time Period of Checking Queue and Handling Received Message with Host",
				"確認佇列並處理與上位系統傳送/接受訊息的時間間隔",
				"确认伫列并处理与上位系统传送/接受讯息的时间间隔"));
			defaultConfigs.Add(new Configuration(
				"MissionDispatcher",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1000",
				"300",
				"5000",
				"Time Period of Checking Queue and Dispatching Mission to Vehicle",
				"確認佇列並分配任務給自走車的時間間隔",
				"确认伫列并分配任务给自走车的时间间隔"));
			defaultConfigs.Add(new Configuration(
				"MissionUpdater",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"300",
				"100",
				"2000",
				"Time Period of Checking Send State and Execute State of Mission",
				"確認任務傳送狀態與執行狀態的時間間隔",
				"确认任务传送状态与执行状态的时间间隔"));
			defaultConfigs.Add(new Configuration(
				"MissionUpdater",
				"TimeoutOfSendingMission",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"5",
				"1",
				"20",
				"Timeout of Checking Sending Mission Successed or Failed",
				"傳送任務後若過了此秒數仍未偵測到車子相應的變化，則判斷任務傳送失敗",
				"传送任务后若过了此秒数仍未侦测到车子相应的变化，则判断任务传送失败"));
			defaultConfigs.Add(new Configuration(
				"MissionUpdater",
				"TimeoutOfExecutingMission",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"600",
				"10",
				"6000",
				"Timeout of Checking Executing Mission Successed or Failed",
				"執行任務後若過了此秒數任務仍未完成，則判斷任務執行失敗",
				"执行任务后若过了此秒数任务仍未完成，则判断任务执行失败"));
			defaultConfigs.Add(new Configuration(
				"MissionUpdater",
				"ToleranceOfX",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"500",
				"1",
				"10000",
				"Tolerance of X of Checking Mission Is Completed or Failed",
				"判斷任務是否完成的座標 (X) 容許誤差",
				"判断任务是否完成的座标 (X) 容许误差"));
			defaultConfigs.Add(new Configuration(
				"MissionUpdater",
				"ToleranceOfY",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"500",
				"1",
				"10000",
				"Tolerance of Y of Checking Mission Is Completed or Failed",
				"判斷任務是否完成的座標 (Y) 容許誤差",
				"判断任务是否完成的座标 (Y) 容许误差"));
			defaultConfigs.Add(new Configuration(
				"MissionUpdater",
				"ToleranceOfToward",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"5",
				"1",
				"360",
				"Tolerance of Toward of Checking Mission Is Completed or Failed",
				"判斷任務是否完成的座標 (Toward) 容許誤差",
				"判断任务是否完成的座标 (Toward) 容许误差"));
			defaultConfigs.Add(new Configuration(
				"MapFileManager",
				"MapFileDirectory",
				ConfigurationType.String,
				ConfigurationLevel.Normal,
				".\\Map\\",
				string.Empty,
				string.Empty,
				"Directory Path of Saving Maps",
				"儲存地圖檔案的資料夾路徑",
				"储存地图档案的资料夹路径"));
			defaultConfigs.Add(new Configuration(
				"MapManager",
				"AutoLoadMap",
				ConfigurationType.Bool,
				ConfigurationLevel.Normal,
				"True",
				string.Empty,
				string.Empty,
				"Auto Load Map when Vehicle's Current Map Changed",
				"當自走車當前使用地圖改變時，自身重新讀取地圖",
				"当自走车当前使用地图改变时，自身重新读取地图"));
			defaultConfigs.Add(new Configuration(
				"CycleMissionGenerator",
				"TimePeriod",
				ConfigurationType.Int,
				ConfigurationLevel.Normal,
				"1500",
				"500",
				"5000",
				"Time Period of Checking Vehicle State and Generating Next Mission",
				"確認自走車狀態並產生下一個任務的時間間隔",
				"确认自走车状态并产生下一个任务的时间间隔"));

			mConfigs.Clear();
			for (int i = 0; i < defaultConfigs.Count; ++i)
			{
				mConfigs.Add(defaultConfigs[i].mFullName, defaultConfigs[i]);
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
