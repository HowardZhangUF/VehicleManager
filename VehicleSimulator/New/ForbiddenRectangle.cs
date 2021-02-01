using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator.New
{
	public class ForbiddenRectangle : Rectangle
	{
		public ForbiddenRectangle() : base()
		{

		}
		public ForbiddenRectangle(int X1, int Y1, int X2, int Y2) : base(X1, Y1, X2, Y2)
		{

		}
	}
}
