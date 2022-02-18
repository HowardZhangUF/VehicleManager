using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.General
{
	public class MapObjectOfTowardPoint : IMapObjectOfTowardPoint
	{
		public string mName { get; private set; }
		public ITowardPoint2D mLocation { get; private set; }
		public TypeOfMapObjectOfTowardPoint mType { get; private set; }
		public string[] mParameters { get; private set; }
		
		public MapObjectOfTowardPoint(string Name, ITowardPoint2D Location, TypeOfMapObjectOfTowardPoint Type, string[] Parameters)
		{
			Set(Name, Location, Type, Parameters);
		}
		public MapObjectOfTowardPoint(string Name, int X, int Y, double Toward, TypeOfMapObjectOfTowardPoint Type, string[] Parameters)
		{
			Set(Name, X, Y, Toward, Type, Parameters);
		}
		public void Set(string Name, ITowardPoint2D Location, TypeOfMapObjectOfTowardPoint Type, string[] Parameters)
		{
			mName = Name;
			mLocation = Location;
			mType = Type;
			mParameters = Parameters;
		}
		public void Set(string Name, int X, int Y, double Toward, TypeOfMapObjectOfTowardPoint Type, string[] Parameters)
		{
			Set(Name, new TowardPoint2D(X, Y, Toward), Type, Parameters);
		}
	}
}
