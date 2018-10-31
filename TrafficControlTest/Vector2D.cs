using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>二維向量</summary>
	public class Vector2D
	{
		public int XComponent;
		public int YComponent;
		public override string ToString()
		{
			string tmp = "";
			tmp += $"Vector: {XComponent}i + {YComponent}j\n";
			return tmp;
		}

		/// <summary>計算兩點的二維度的向量</summary>
		public static Vector2D CalculateVector2D(int x1, int y1, int x2, int y2)
		{
			Vector2D result = null;
			result = new Vector2D();
			result.XComponent = x2 - x1;
			result.YComponent = y2 - y1;
			return result;
		}
	}
}
