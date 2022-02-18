using LibraryForVM;
using System;
using System.Collections.Generic;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Map
{
	/// <summary>
	/// Reference: IMapManager, IMapFileManager, IMapFileManagerUpdater, IVehicleCommunicator, IVehicleInfoManager
	/// - 提供載入地圖的功能，地圖載入後會發出事件
	/// - 提供同步當前地圖至所有車的功能，同步地圖開始時會發出事件
	/// - 當 IVehicleInfoManager 車的「當前使用地圖」屬性改變時，且啟用「自動讀取地圖」功能時組合地圖並讓 IMapManager 讀取
	/// </summary>
	public interface IMapManagerUpdater : ISystemWithConfig
	{
		event EventHandler<LoadMapSuccessedEventArgs> LoadMapSuccessed;
		event EventHandler<LoadMapFailedEventArgs> LoadMapFailed;
		event EventHandler<SynchronizeMapStartedEventArgs> SynchronizeMapStarted;
		event EventHandler<MapMergedEventArgs> MapMerged;

		void Set(IMapManager MapManager);
		void Set(IMapFileManager MapFileManager);
		void Set(IMapFileManagerUpdater MapFileManagerUpdater);
		void Set(IVehicleCommunicator VehicleComsmunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMapManager MapManager, IMapFileManager MapFileManager, IMapFileManagerUpdater MapFileManagerUpdater, IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager);
		void LoadMap(string MapFileFullPath);
		void SynchronizeMapToOnlineVehicles(string MapFileName);
	}

	public enum ReasonOfLoadMapFail
	{
		None,
		IsDownloadingMapFile,
		MapFileIsNotExist,
		MapFileHashIsEqualToOldMap
	}

	public class LoadMapSuccessedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string MapFileFullPath { get; private set; }

		public LoadMapSuccessedEventArgs(DateTime OccurTime, string MapFileFullPath) : base()
		{
			this.OccurTime = OccurTime;
			this.MapFileFullPath = MapFileFullPath;
		}
	}

	public class LoadMapFailedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string MapFileFullPath { get; private set; }
		public ReasonOfLoadMapFail Reason { get; private set; }

		public LoadMapFailedEventArgs(DateTime OccurTime, string MapFileFullPath, ReasonOfLoadMapFail Reason)
		{
			this.OccurTime = OccurTime;
			this.MapFileFullPath = MapFileFullPath;
			this.Reason = Reason;
		}
	}

	public class SynchronizeMapStartedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string MapFileFullPath { get; private set; }
		public IEnumerable<string> VehicleNames { get; private set; }

		public SynchronizeMapStartedEventArgs(DateTime OccurTime, string MapFileFullPath, IEnumerable<string> VehicleNames) : base()
		{
			this.OccurTime = OccurTime;
			this.MapFileFullPath = MapFileFullPath;
			this.VehicleNames = VehicleNames;
		}
	}

	public class MapMergedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public IEnumerable<string> SrcMapFileFullPaths { get; private set; }
		public string ResultMapFileFullPath { get; private set; }

		public MapMergedEventArgs(DateTime OccurTime, IEnumerable<string> SrcMapFileFullPaths, string ResultMapFileFullPath)
		{
			this.OccurTime = OccurTime;
			this.SrcMapFileFullPaths = SrcMapFileFullPaths;
			this.ResultMapFileFullPath = ResultMapFileFullPath;
		}
	}
}
