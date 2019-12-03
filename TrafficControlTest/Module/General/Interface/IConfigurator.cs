using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.General.Interface
{
	public interface IConfigurator
	{
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
		void SetValue(string Keyword, string Value);
	}
}
