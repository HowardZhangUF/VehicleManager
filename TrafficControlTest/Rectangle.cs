using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	public class Rectangle
	{
		/// <summary>範圍。 x 最大值</summary>
		public int XMax;

		/// <summary>範圍。 x 最小值</summary>
		public int XMin;

		/// <summary>範圍。 y 最大值</summary>
		public int YMax;

		/// <summary>範圍。 y 最小值</summary>
		public int YMin;

		/// <summary>寬</summary>
		private int _Width = -1;

		/// <summary>寬</summary>
		public int Width
		{
			get
			{
				if (_Width == -1) _Width = XMax - XMin;
				return _Width;
			}
		}

		/// <summary>高</summary>
		private int _Height = -1;

		/// <summary>高</summary>
		public int Height
		{
			get
			{
				if (_Height == -1) _Height = YMax - YMin;
				return _Height;
			}
		}

		/// <summary>建構式</summary>
		public Rectangle(int x_min, int y_min, int x_max, int y_max)
		{
			this.XMin = x_min;
			this.YMin = y_min;
			this.XMax = x_max;
			this.YMax = y_max;
		}

		/// <summary>確認點是否在自身範圍內</summary>
		public bool IsPointInside(int x, int y)
		{
			return (x >= XMin && x <= XMax && y >= YMin && y <= YMax);
		}

		/// <summary>清除</summary>
		public void Clear()
		{
			XMax = -1;
			XMin = -1;
			YMax = -1;
			YMin = -1;
			_Width = -1;
			_Height = -1;
		}

		/// <summary>轉成字串</summary>
		public override string ToString()
		{
			return $"Rectangle: ({XMin}, {YMin}) ({XMax}, {YMax})";
		}

		/// <summary>計算兩個矩形是否有重疊，若有重疊，回傳重疊區域的矩形。有共用同一個邊也算重疊</summary>
		public static bool IsOverlap(Rectangle rectangle1, Rectangle rectangle2)
		{
			if (rectangle1.XMax >= rectangle2.XMin && rectangle2.XMax >= rectangle1.XMin && rectangle1.YMax >= rectangle2.YMin && rectangle2.YMax >= rectangle1.YMin)
				return true;
			else
				return false;
		}

		/// <summary>計算兩個矩形的聯集矩形</summary>
		public static Rectangle GetUnionRectangle(Rectangle rectangle1, Rectangle rectangle2)
		{
			Rectangle result = null;
			int x_min = Math.Min(rectangle1.XMin, rectangle2.XMin);
			int y_min = Math.Min(rectangle1.YMin, rectangle2.YMin);
			int x_max = Math.Max(rectangle1.XMax, rectangle2.XMax);
			int y_max = Math.Max(rectangle1.YMax, rectangle2.YMax);
			if (x_min <= x_max && y_min <= y_max)
			{
				result = new Rectangle(x_min, y_min, x_max, y_max);
			}
			return result;
		}

		/// <summary>計算兩個矩形的交集矩形</summary>
		public static Rectangle GetIntersectionRectangle(Rectangle rectangle1, Rectangle rectangle2)
		{
			Rectangle result = null;
			int x_min = Math.Max(rectangle1.XMin, rectangle2.XMin);
			int y_min = Math.Max(rectangle1.YMin, rectangle2.YMin);
			int x_max = Math.Min(rectangle1.XMax, rectangle2.XMax);
			int y_max = Math.Min(rectangle1.YMax, rectangle2.YMax);
			if (x_min <= x_max && y_min <= y_max)
			{
				result = new Rectangle(x_min, y_min, x_max, y_max);
			}
			return result;
		}

		/// <summary>以指定點為中心，產生一個邊長為兩倍半徑大小的正方形</summary>
		public static Rectangle GenerateRectangle(int x, int y, int radius)
		{
			Rectangle result = null;
			if (radius > 0)
			{
				result = new Rectangle(x - radius, y - radius, x + radius, y + radius);
			}
			return result;
		}

		/// <summary>計算路徑線區域</summary>
		public static Rectangle CalculatePathRegion(AGVInfo agv)
		{
			Rectangle result = null;
			if (agv.Path.PathX.Count() >= 2)
			{
				result = new Rectangle((int)agv.Path.PathX[0], (int)agv.Path.PathY[0], (int)agv.Path.PathX[0], (int)agv.Path.PathY[0]);
				for (int i = 1; i < agv.Path.PathX.Count(); ++i)
				{
					if ((int)agv.Path.PathX[i] < result.XMin) result.XMin = (int)agv.Path.PathX[i];
					else if ((int)agv.Path.PathX[i] > result.XMax) result.XMax = (int)agv.Path.PathX[i];
					if ((int)agv.Path.PathY[i] < result.YMin) result.YMin = (int)agv.Path.PathY[i];
					else if ((int)agv.Path.PathY[i] > result.YMax) result.YMax = (int)agv.Path.PathY[i];
				}
				result.XMin -= agv.FrameRadius;
				result.XMax += agv.FrameRadius;
				result.YMin -= agv.FrameRadius;
				result.YMax += agv.FrameRadius;
			}
			return result;
		}
	}
}
