using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Map
{
	public class MapFileManager : SystemWithConfig, IMapFileManager
	{
		public event EventHandler<MapFileCountChangedEventArgs> MapFileAdded;
		public event EventHandler<MapFileCountChangedEventArgs> MapFileRemoved;

		private string mMapFileDirectory { get; set; } = string.Empty;

		public MapFileManager()
		{
		}
		public string[] GetLocalMapFileNameList()
		{
			string[] result = null;
			if (Directory.Exists(mMapFileDirectory))
			{
				result = Directory.GetFiles(mMapFileDirectory, "*.map", SearchOption.TopDirectoryOnly);
				result = result.Select(o => new System.IO.FileInfo(o).Name).ToArray();
			}
			return result;
		}
		public string[] GetLocalMapFileNameWithoutExtensionList()
		{
			return GetLocalMapFileNameList().Select(o => o.Replace(".map", string.Empty)).ToArray();
		}
		public string GetMapFileFullPath(string MapFileName)
		{
			string result = string.Empty;
			if (File.Exists(Path.Combine(mMapFileDirectory, MapFileName)))
			{
				result = Path.Combine(mMapFileDirectory, MapFileName);
			}
			return result;
		}
		public string GetMapFileFullPath2(string MapFileNameWithoutExtension)
		{
			return GetMapFileFullPath(MapFileNameWithoutExtension + ".map");
		}
		public void AddMapFile(string MapFileName, byte[] MapData)
		{
			if (!string.IsNullOrEmpty(MapFileName) && MapData != null)
			{
				if (!Directory.Exists(mMapFileDirectory)) Directory.CreateDirectory(mMapFileDirectory);
				File.WriteAllBytes(Path.Combine(mMapFileDirectory, MapFileName), MapData);
				RaiseEvent_MapFileAdded(MapFileName);
			}
		}
		public void AddMapFile2(string MapFileNameWithoutExtension, byte[] MapData)
		{
			AddMapFile(MapFileNameWithoutExtension + ".map", MapData);
		}
		public void RemoveMapFile(string MapFileName)
		{
			if (File.Exists(Path.Combine(mMapFileDirectory, MapFileName)))
			{
				File.Delete(Path.Combine(mMapFileDirectory, MapFileName));
				RaiseEvent_MapFileRemoved(MapFileName);
			}
		}
		public void RemoveMapFile2(string MapFileNameWithoutExtension)
		{
			RemoveMapFile(MapFileNameWithoutExtension + ".map");
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "MapFileDirectory" };
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "MapFileDirectory":
					return mMapFileDirectory;
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "MapFileDirectory":
					mMapFileDirectory = NewValue;
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}

		protected virtual void RaiseEvent_MapFileAdded(string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapFileAdded?.Invoke(this, new MapFileCountChangedEventArgs(DateTime.Now, MapFileName));
			}
			else
			{
				Task.Run(() => { MapFileAdded?.Invoke(this, new MapFileCountChangedEventArgs(DateTime.Now, MapFileName)); });
			}
		}
		protected virtual void RaiseEvent_MapFileRemoved(string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapFileRemoved?.Invoke(this, new MapFileCountChangedEventArgs(DateTime.Now, MapFileName));
			}
			else
			{
				Task.Run(() => { MapFileRemoved?.Invoke(this, new MapFileCountChangedEventArgs(DateTime.Now, MapFileName)); });
			}
		}
	}
}
