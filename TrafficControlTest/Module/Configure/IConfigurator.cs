using System.Collections.Generic;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Configure
{
	public interface IConfigurator
	{
		event EventHandlerDateTime ConfigLoaded;
		event EventHandlerDateTime ConfigSaved;
		event EventHandlerItem<Configuration> ConfigUpdated;

		/// <summary>語系</summary>
		Language mLanguage { get; }
		/// <summary>儲存 Configuration 檔案的名稱，例： .\\Application.config</summary>
		string mFileName { get; }

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
}
