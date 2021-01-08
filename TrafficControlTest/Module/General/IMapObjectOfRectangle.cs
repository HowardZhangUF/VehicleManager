using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.General
{
	public interface IMapObjectOfRectangle
	{
		string mName { get; }
		IRectangle2D mRange { get; }
		TypeOfMapObjectOfRectangle mType { get; }
		string[] mParameters { get; }

		void Set(string Name, IRectangle2D Range, TypeOfMapObjectOfRectangle Type, string[] Parameters);
		void Set(string Name, IPoint2D MaxPoint, IPoint2D MinPoint, TypeOfMapObjectOfRectangle Type, string[] Parameters);
		void Set(string Name, int MaxX, int MaxY, int MinX, int MinY, TypeOfMapObjectOfRectangle Type, string[] Parameters);
	}

	public enum TypeOfMapObjectOfRectangle
	{
		None,
		Foribdden,
		Oneway,
		PathPlanning,
		AutomaticDoor
	}
}
