using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class Goal : Point
	{
		public int mToward { get; private set; } = 0;
		public string mName { get; private set; } = string.Empty;

		public Goal()
		{
			Set(0, 0, 0, string.Empty);
		}
		public Goal(int X, int Y, int Toward, string Name)
		{
			Set(X, Y, Toward, Name);
		}
		public void Set(int X, int Y, int Toward, string Name)
		{
			Set(X, Y);
			mToward = Toward;
			mName = Name;
		}
		public override string ToString()
		{
			return ToString();
		}
		public override string ToString(string Separator = ",")
		{
			return $"({mName}{Separator}{mX}{Separator}{mY}{Separator}{mToward})";
		}
	}
}
