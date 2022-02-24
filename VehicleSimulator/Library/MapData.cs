using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class MapData
	{
		public string mMapFilePath { get; private set; } = string.Empty;
		public Rectangle mRange { get; private set; } = new Rectangle();
		public List<ForbiddenRectangle> mForbiddenRectangles { get; private set; } = new List<ForbiddenRectangle>();
		public List<OneWayRectangle> mOneWayRectangles { get; private set; } = new List<OneWayRectangle>();
		public List<ObstaclePoint> mObstaclePoints { get; private set; } = new List<ObstaclePoint>();

		public MapData()
		{

		}
		public void SetMapFilePath(string MapFilePath)
		{
			if (File.Exists(MapFilePath))
			{
				mMapFilePath = MapFilePath;
			}
		}
		public void SetRange(Rectangle Range)
		{
			if (Range != null)
			{
				mRange.Set(Range.mMin.mX, Range.mMin.mY, Range.mMax.mX, Range.mMax.mY);
			}
		}
		public void SetForbiddenRectangles(List<ForbiddenRectangle> ForbiddenRectangles)
		{
			mForbiddenRectangles.Clear();
			if (ForbiddenRectangles != null && ForbiddenRectangles.Count > 0)
			{
				mForbiddenRectangles.AddRange(ForbiddenRectangles);
			}
		}
		public void SetOneWayRectangles(List<OneWayRectangle> OneWayRectangles)
		{
			mOneWayRectangles.Clear();
			if (OneWayRectangles != null && OneWayRectangles.Count > 0)
			{
				mOneWayRectangles.AddRange(OneWayRectangles);
			}
		}
		public void SetObstaclePoints(List<ObstaclePoint> ObstaclePoints)
		{
			mObstaclePoints.Clear();
			if (ObstaclePoints != null && ObstaclePoints.Count > 0)
			{
				mObstaclePoints.AddRange(ObstaclePoints);
			}
		}
	}
}
