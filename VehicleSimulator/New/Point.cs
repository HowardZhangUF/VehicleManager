using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator.New
{
	public class Point
	{
		public int mX { get; private set; }
		public int mY { get; private set; }

		public Point(int X, int Y)
		{
			Set(X, Y);
		}
		public void Set(int X, int Y)
		{
			mX = X;
			mY = Y;
		}
	}
}
