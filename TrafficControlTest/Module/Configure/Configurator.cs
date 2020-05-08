using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Configure;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.General.Implement
{
	public class Configurator : IConfigurator
	{
		/*
		 * 新增一個 Configuration 時：
		 * 1. Configurator 要新增其 Default 資訊
		 * 2. 對應類別的 GetConfig() 與 SetConfig() 方法需要更新
		 * 3. VehicleManagerProcess 的 LoadConfigFileAndUpdateSystemConfig() 與 LoadSystemConfigAndUpdateConfigFile() 方法需要更新
		 * 4. VehicleManagerProcess 的 HandleEvent_ConfiguratorConfigUpdated() 方法需要更新
		 */

		public event EventHandlerDateTime ConfigLoaded;
		public event EventHandlerDateTime ConfigSaved;
		public event EventHandlerItem<Configuration> ConfigUpdated;

		public Language mLanguage { get; private set; } = Language.Enus;
		public string mFileName { get; private set; } = string.Empty;

		private readonly Dictionary<string, Configuration> mConfigs = new Dictionary<string, Configuration>();

		public Configurator(string FileName)
		{
			Set(FileName);
		}
		public void Set(string FileName)
		{
			if (!string.IsNullOrEmpty(FileName))
			{
				mFileName = FileName;
			}
		}
		public void Load()
		{
			if (!string.IsNullOrEmpty(mFileName))
			{
				GenerateDefaultConfiguration();
				if (!File.Exists(mFileName))
				{
					RaiseEvent_ConfigLoaded();
					Save();
					return;
				}
				else
				{
					string[] datas = File.ReadAllLines(mFileName);
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
					RaiseEvent_ConfigLoaded();
					Save();
				}
			}
		}
		public void Save()
		{
			if (!string.IsNullOrEmpty(mFileName))
			{
				List<string> result = new List<string>();
				foreach (Configuration config in mConfigs.Values)
				{
					result.Add(config.ToString());
				}
				File.WriteAllLines(mFileName, result);
				RaiseEvent_ConfigSaved();
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
					RaiseEvent_ConfigUpdated(Keyword, mConfigs[Keyword]);
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
		protected virtual void RaiseEvent_ConfigLoaded(bool Sync = true)
		{
			if (Sync)
			{
				ConfigLoaded?.Invoke(DateTime.Now);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ConfigLoaded?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_ConfigSaved(bool Sync = true)
		{
			if (Sync)
			{
				ConfigSaved?.Invoke(DateTime.Now);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ConfigSaved?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_ConfigUpdated(string Name, Configuration Item, bool Sync = true)
		{
			if (Sync)
			{
				ConfigUpdated?.Invoke(DateTime.Now, Name, Item);
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ConfigUpdated?.Invoke(DateTime.Now, Name, Item); });
			}
		}
	}
}
