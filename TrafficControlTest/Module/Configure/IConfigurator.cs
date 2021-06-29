using System;
using System.Collections.Generic;
using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Configure
{
	/// <remarks>
	/// - 一般的 Config
	///		- 從介面改值 -> 更新 Configurator 類別並儲存檔案 -> 更新相應的類別
	///	- 地圖管理的 Config
	///		- 從介面改值 或 從地圖類別改值 -> 更新 Configurator 類別並儲存檔案 -> 更新地圖類別的值 與 介面的值
	/// </remarks>
	public interface IConfigurator
	{
		event EventHandler<ConfigFileLoadedEventArgs> ConfigFileLoaded;
		event EventHandler<ConfigFileSavedEventArgs> ConfigFileSaved;
		event EventHandler<ConfigurationUpdatedEventArgs> ConfigurationUpdated;

		/// <summary>語系</summary>
		Language mLanguage { get; }
		/// <summary>儲存 Configuration 檔案的名稱，例： .\\Application.config</summary>
		string mFilePath { get; }

		/// <summary>設定 Configuration 檔案的名稱</summary>
		void Set(string FileName);
		/// <summary>從檔案 (mFileName) 讀取 Configuration 。若檔案不存在則會自行生成檔案。若檔案中有缺少 Configuration 時會自行新增並更新檔案</summary>
		void Load();
		/// <summary>輸出 Configuration 至檔案 (mFileName)</summary>
		void Save();
		/// <summary>取得指定的 Configuration 值</summary>
		string GetValue(string Keyword);
		/// <summary>設定指定的 Configuration 值</summary>
		bool SetValue(string Keyword, string Value);
		/// <summary>取得所有的 Configuration</summary>
		List<string[]> GetConfigDataGridViewRowDataCollection();
	}

	public class ConfigFileLoadedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string FilePath { get; private set; }

		public ConfigFileLoadedEventArgs(DateTime OccurTime, string FilePath) : base()
		{
			this.OccurTime = OccurTime;
			this.FilePath = FilePath;
		}
	}
	public class ConfigFileSavedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string FilePath { get; private set; }

		public ConfigFileSavedEventArgs(DateTime OccurTime, string FilePath) : base()
		{
			this.OccurTime = OccurTime;
			this.FilePath = FilePath;
		}
	}
	public class ConfigurationUpdatedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string ConfigName { get; private set; }
		public Configuration Configuration { get; private set; }

		public ConfigurationUpdatedEventArgs(DateTime OccurTime, string ConfigName, Configuration Configuration) : base()
		{
			this.OccurTime = OccurTime;
			this.ConfigName = ConfigName;
			this.Configuration = Configuration;
		}
	}
}
