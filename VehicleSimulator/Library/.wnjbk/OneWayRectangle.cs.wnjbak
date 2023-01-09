using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class OneWayRectangle : Rectangle
	{
		public int mDirection { get; private set; } = 0;

		public OneWayRectangle()
		{

		}
		public OneWayRectangle(int X1, int Y1, int X2, int Y2, int Direction)
		{
			Set(X1, Y1, X2, Y2, Direction);
		}
		public void Set(int X1, int Y1, int X2, int Y2, int Direction)
		{
			Set(X1, Y1, X2, Y2);
			mDirection = Direction;
		}
		public override string ToString(string Separator = ",")
		{
			return $"{base.ToString()}{Separator}{mDirection}";
		}
		public new static OneWayRectangle FromString(string String, string Separator = ",")
		{
			string[] tmp = String.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
			return new OneWayRectangle(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2]), int.Parse(tmp[3]), int.Parse(tmp[4]));
		}
	}
}
