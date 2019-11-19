using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Library
{
	public static class Configuration
	{
		private static readonly string mDefaultFileName = "Application";
		private static readonly string mDefaultFileExtension = ".config";
		private static readonly string mDefaultFilePath = $"./{mDefaultFileName}{mDefaultFileExtension}";
		private static readonly Dictionary<string, string> mConfigs = new Dictionary<string, string>();

		/// <summary>
		/// 讀取 Configuration (./Application.config) 。
		/// 若 Configuration 檔案不存在則會自行生成檔案。
		/// 若 Configuration 檔案中有缺少設定時會自行新增並更新 Configuration 檔案。
		/// </summary>
		public static void Load()
		{
			GenerateDefaultConfiguration();
			if (!File.Exists(mDefaultFilePath))
			{
				Save();
				return;
			}
			else
			{
				string[] datas = File.ReadAllLines(mDefaultFilePath);
				foreach (string data in datas)
				{
					string[] keyValue = data.Split('=');
					if (keyValue.Length == 2)
					{
						if (mConfigs.Keys.Contains(keyValue[0]))
						{
							mConfigs[keyValue[0]] = keyValue[1];
						}
					}
				}
				Save();
			}
		}
		/// <summary>
		/// 輸出 Configuration 至檔案 (./Application.config) 
		/// </summary>
		public static void Save()
		{
			List<string> result = new List<string>();
			foreach (KeyValuePair<string, string> keyValue in mConfigs)
			{
				result.Add($"{keyValue.Key}={keyValue.Value}");
			}
			File.WriteAllLines(mDefaultFilePath, result);
		}
		/// <summary>
		/// 取得指定的 Configuration 值
		/// </summary>
		public static string GetValue(string Category, string Name)
		{
			string keyword = $"{Category}/{Name}";
			if (mConfigs.Keys.Contains(keyword))
			{
				return mConfigs[keyword];
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 設定指定的 Configuration 值
		/// </summary>
		public static void SetValue(string Category, string Name, string Value)
		{
			string keyword = $"{Category}/{Name}";
			if (mConfigs.Keys.Contains(keyword))
			{
				mConfigs[keyword] = Value;
			}
		}

		private static void GenerateDefaultConfiguration()
		{
			mConfigs.Clear();
			mConfigs.Add("VehicleCommunicator/ListenPort", "8000");
			mConfigs.Add("HostCommunicator/ListenPort", "9000");
			mConfigs.Add("MapFileManager/MapDirectory", ".\\Map\\");
		}
	}
}
