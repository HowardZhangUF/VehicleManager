using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.General
{
	public interface IMapObjectOfTowardPoint
	{
		string mName { get; }
		ITowardPoint2D mLocation { get; }
		TypeOfMapObjectOfTowardPoint mType { get; }
		string[] mParameters { get; }

		void Set(string Name, ITowardPoint2D Location, TypeOfMapObjectOfTowardPoint Type, string[] Parameters);
		void Set(string Name, int X, int Y, double Toward, TypeOfMapObjectOfTowardPoint Type, string[] Parameters);
	}

	public enum TypeOfMapObjectOfTowardPoint
	{
		Normal,
		Charge
	}
}
