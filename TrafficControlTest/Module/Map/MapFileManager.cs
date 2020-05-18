using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Map
{
	public class MapFileManager : SystemWithConfig, IMapFileManager
	{
		public event EventHandlerMapFileName MapFileAdded;
		public event EventHandlerMapFileName MapFileRemoved;

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
				MapFileAdded?.Invoke(DateTime.Now, MapFileName);
			}
			else
			{
				Task.Run(() => { MapFileAdded?.Invoke(DateTime.Now, MapFileName); });
			}
		}
		protected virtual void RaiseEvent_MapFileRemoved(string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapFileRemoved?.Invoke(DateTime.Now, MapFileName);
			}
			else
			{
				Task.Run(() => { MapFileRemoved?.Invoke(DateTime.Now, MapFileName); });
			}
		}
	}
}
