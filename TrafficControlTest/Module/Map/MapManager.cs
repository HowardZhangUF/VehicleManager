using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Map
{
	public class MapManager : IMapManager
	{
		public event EventHandler<MapChangedEventArgs> MapChanged;

		public string mCurrentMapFileName { get; private set; } = string.Empty;
		public string mCurrentMapFileNameWithoutExtension { get { return mCurrentMapFileName.Substring(0, mCurrentMapFileName.LastIndexOf('.')); } }
		public string mCurrentMapFileHash { get; private set; } = string.Empty;
		public List<IMapObjectOfTowardPoint> mTowardPointMapObjects { get; private set; } = new List<IMapObjectOfTowardPoint>();
		public List<IMapObjectOfRectangle> mRectangleMapObjects { get; private set; } = new List<IMapObjectOfRectangle>();

		public MapManager()
		{

		}
		public void SetMapData(string MapFileName, string MapFileHash, List<IMapObjectOfTowardPoint> TowardPointMapObjects, List<IMapObjectOfRectangle> RectangleMapObjects)
		{
			mCurrentMapFileName = MapFileName;
			mCurrentMapFileHash = MapFileHash;
			mTowardPointMapObjects.Clear();
			mTowardPointMapObjects.AddRange(TowardPointMapObjects);
			mRectangleMapObjects.Clear();
			mRectangleMapObjects.AddRange(RectangleMapObjects);
			RaiseEvent_MapChanged(mCurrentMapFileName, mCurrentMapFileHash);
		}
		public List<IMapObjectOfTowardPoint> GetTowardPointMapObjects(TypeOfMapObjectOfTowardPoint Type)
		{
			return mTowardPointMapObjects.Where(o => o.mType == Type).ToList();
		}
		public List<IMapObjectOfRectangle> GetRectangleMapObjects(TypeOfMapObjectOfRectangle Type)
		{
			return mRectangleMapObjects.Where(o => o.mType == Type).ToList();
		}
		public IMapObjectOfTowardPoint GetTowardPointMapObject(string Name)
		{
			return mTowardPointMapObjects.FirstOrDefault(o => o.mName == Name);
		}
		public IMapObjectOfRectangle GetRectangleMapObject(string Name)
		{
			return mRectangleMapObjects.FirstOrDefault(o => o.mName == Name);
		}

		protected virtual void RaiseEvent_MapChanged(string MapFileName, string MapFileHash, bool Sync = true)
		{
			if (Sync)
			{
				MapChanged?.Invoke(this, new MapChangedEventArgs(DateTime.Now, MapFileName, MapFileHash));
			}
			else
			{
				Task.Run(() => { MapChanged?.Invoke(this, new MapChangedEventArgs(DateTime.Now, MapFileName, MapFileHash)); });
			}
		}
	}
}
