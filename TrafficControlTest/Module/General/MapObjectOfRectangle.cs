using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.General
{
	public class MapObjectOfRectangle : IMapObjectOfRectangle
	{
		public string mName { get; private set; }
		public IRectangle2D mRange { get; private set; }
		public TypeOfMapObjectOfRectangle mType { get; private set; }
		public string[] mParameters { get; private set; }

		public MapObjectOfRectangle(string Name, IRectangle2D Range, TypeOfMapObjectOfRectangle Type, string[] Parameters)
		{
			Set(Name, Range, Type, Parameters);
		}
		public MapObjectOfRectangle(string Name, IPoint2D MaxPoint, IPoint2D MinPoint, TypeOfMapObjectOfRectangle Type, string[] Parameters)
		{
			Set(Name, MaxPoint, MinPoint, Type, Parameters);
		}
		public MapObjectOfRectangle(string Name, int MaxX, int MaxY, int MinX, int MinY, TypeOfMapObjectOfRectangle Type, string[] Parameters)
		{
			Set(Name, MaxX, MaxY, MinX, MinY, Type, Parameters);
		}
		public void Set(string Name, IRectangle2D Range, TypeOfMapObjectOfRectangle Type, string[] Parameters)
		{
			mName = Name;
			mRange = Range;
			mType = Type;
			mParameters = Parameters;
		}
		public void Set(string Name, IPoint2D MaxPoint, IPoint2D MinPoint, TypeOfMapObjectOfRectangle Type, string[] Parameters)
		{
			Set(Name, new Rectangle2D(MaxPoint, MinPoint), Type, Parameters);
		}
		public void Set(string Name, int MaxX, int MaxY, int MinX, int MinY, TypeOfMapObjectOfRectangle Type, string[] Parameters)
		{
			Set(Name, new Point2D(MaxX, MaxY), new Point2D(MinX, MinY), Type, Parameters);
		}
	}
}
