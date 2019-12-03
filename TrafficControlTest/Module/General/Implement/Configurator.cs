using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;

namespace TrafficControlTest.Module.General.Implement
{
	public class Configurator : IConfigurator
	{
		public string mFileName { get; private set; } = string.Empty;

		private readonly Dictionary<string, string> mConfigs = new Dictionary<string, string>();

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
								mConfigs[keyValue[0]] = keyValue[1];
							}
						}
					}
					Save();
				}
			}
		}
		public void Save()
		{
			if (!string.IsNullOrEmpty(mFileName))
			{
				List<string> result = new List<string>();
				foreach (KeyValuePair<string, string> keyValue in mConfigs)
				{
					result.Add($"{keyValue.Key}={keyValue.Value}");
				}
				File.WriteAllLines(mFileName, result);
			}
		}
		public string GetValue(string Keyword)
		{
			if (mConfigs.Keys.Contains(Keyword))
			{
				return mConfigs[Keyword];
			}
			else
			{
				return null;
			}
		}
		public void SetValue(string Keyword, string Value)
		{
			if (mConfigs.Keys.Contains(Keyword))
			{
				mConfigs[Keyword] = Value;
			}
		}

		protected virtual void GenerateDefaultConfiguration()
		{
			mConfigs.Clear();
			mConfigs.Add("VehicleCommunicator/ListenPort", "8000");
			mConfigs.Add("HostCommunicator/ListenPort", "9000");
			mConfigs.Add("MapFileManager/MapFileDirectory", ".\\Map\\");
			mConfigs.Add("MapManager/AutoLoadMap", "true");
		}
	}
}
