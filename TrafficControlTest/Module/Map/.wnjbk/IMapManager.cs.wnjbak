using System;
using System.Collections.Generic;

namespace TrafficControlTest.Module.Map
{
	/// <summary>
	/// - 提供設定地圖資料的功能
	/// - 提供取得當前地圖檔名稱、 Hash 、地圖物件清單的功能
	/// - 地圖變更後會發出事件
	/// </summary>
	public interface IMapManager
	{
		event EventHandler<MapChangedEventArgs> MapChanged;

		string mCurrentMapFileName { get; }
		string mCurrentMapFileNameWithoutExtension { get; }
		string mCurrentMapFileHash { get; }
		List<IMapObjectOfTowardPoint> mTowardPointMapObjects { get; }
		List<IMapObjectOfRectangle> mRectangleMapObjects { get; }

		void SetMapData(string MapFileName, string MapFileHash, List<IMapObjectOfTowardPoint> TowardPointMapObjects, List<IMapObjectOfRectangle> RectangleMapObjects);
		List<IMapObjectOfTowardPoint> GetTowardPointMapObjects(TypeOfMapObjectOfTowardPoint Type);
		List<IMapObjectOfRectangle> GetRectangleMapObjects(TypeOfMapObjectOfRectangle Type);
		IMapObjectOfTowardPoint GetTowardPointMapObject(string Name);
		IMapObjectOfRectangle GetRectangleMapObject(string Name);
	}

	public class MapChangedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string MapFileName { get; private set; }
		public string MapFileHash { get; private set; }

		public MapChangedEventArgs(DateTime OccurTime, string MapFileName, string MapFileHash) : base()
		{
			this.OccurTime = OccurTime;
			this.MapFileName = MapFileName;
			this.MapFileHash = MapFileHash;
		}
	}
}
