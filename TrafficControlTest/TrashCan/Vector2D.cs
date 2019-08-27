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
		public double XComponent;
		public double YComponent;
		private double _Magnitude = 0;
		public double Magnitude
		{
			get
			{
				if (_Magnitude == 0) _Magnitude = Math.Sqrt(DotProduct(this, this));
				return _Magnitude;
			}
		}
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

		/// <summary>計算兩個向量的內積</summary>
		public static double DotProduct(Vector2D v1, Vector2D v2)
		{
			double result = 0;
			if (v1 != null && v2 != null)
			{
				result = v1.XComponent * v2.XComponent + v1.YComponent * v2.YComponent;
			}
			return result;
		}

		/// <summary>計算指定向量的單位向量</summary>
		public static Vector2D NormalizeVector(Vector2D v)
		{
			Vector2D result = null;
			if (v != null)
			{
				result = new Vector2D();
				result.XComponent = v.XComponent / v.Magnitude;
				result.YComponent = v.YComponent / v.Magnitude;
			}
			return result;
		}

		/// <summary>計算兩向量的夾角</summary>
		public static int CalculateAngleOfTwoVector(Vector2D v1, Vector2D v2)
		{
			int result = -1;
			if (v1 != null && v2 != null)
			{
				result = (int)(Math.Acos(DotProduct(v1, v2) / (v1.Magnitude * v2.Magnitude)) / Math.PI * 180);
			}
			return result;
		}
	}
}
