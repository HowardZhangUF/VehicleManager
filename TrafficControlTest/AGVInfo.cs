using Geometry;
using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>AGV 資訊</summary>
	public class AGVInfo
	{
		/// <summary>AGV 狀態</summary>
		public AGVStatus Status = null;

		/// <summary>AGV 路徑</summary>
		public AGVPath Path = null;

		/// <summary>AGV 路徑線點</summary>
		public List<Pair> _PathPoints = null;

		/// <summary>AGV 路徑線點</summary>
		public List<Pair> PathPoints
		{
			get
			{
				if (_PathPoints == null) _PathPoints = AGVInfo.CalculatePathPoints(Path, FrameRadius);
				return _PathPoints;
			}
		}

		/// <summary>AGV 路徑線區域</summary>
		private Rectangle _PathRegion = null;

		/// <summary>AGV 路徑線區域</summary>
		public Rectangle PathRegion
		{
			get
			{
				if (_PathRegion == null) _PathRegion = Rectangle.CalculatePathRegion(this);
				return _PathRegion;
			}
		}

		/// <summary>車框半徑。車框 = 車身安全框 + Buffer 框</summary>
		public int FrameRadius = 2000;

		/// <summary>AGV 圖像識別碼</summary>
		public int AGVIconID = -1;

		/// <summary>AGV 路徑圖像識別碼</summary>
		public int PathIconID = -1;

		/// <summary>AGV 的 IP 與 Port 。格式為 IP:Port</summary>
		public string IPPort = string.Empty;

		/// <summary>清除 AGV 路徑線點</summary>
		public void ClearPathPoints()
		{
			_PathPoints = null;
		}

		/// <summary>清除 AGV 路徑線區域</summary>
		public void ClearPathRegion()
		{
			_PathRegion = null;
		}

		/// <summary>將路徑節點轉換成點集合</summary>
		private static List<Pair> CalculatePathPoints(AGVPath path, int frameRadius)
		{
			List<Pair> result = null;
			if (path.PathX.Count() >= 2)
			{
				result = new List<Pair>();
				for (int i = 1; i < path.PathX.Count(); ++i)
				{
					result.Add(new Pair(path.PathX[i - 1], path.PathY[i - 1]));
					result.AddRange(ConvertLineToPoints(new Pair(path.PathX[i - 1], path.PathY[i - 1]), new Pair(path.PathX[i], path.PathY[i]), frameRadius));
					if (i == path.PathX.Count() - 1) result.Add(new Pair(path.PathX[i], path.PathY[i]));
				}
			}
			return result;
		}

		/// <summary>將一條線轉換成點集合(不包含該線的兩端點)</summary>
		private static List<Pair> ConvertLineToPoints(Pair src, Pair dst, int frameRadius)
		{
			List<Pair> result = new List<Pair>();

			int diffX = Math.Abs(src.X - dst.X);
			int diffY = Math.Abs(src.Y - dst.Y);
			// 斜率為無限大
			if (diffX == 0)
			{
				if (src.Y < dst.Y)
				{
					for (int i = src.Y + frameRadius; i < dst.Y; i += frameRadius)
					{
						result.Add(new Pair(src.X, i));
					}
				}
				else
				{
					for (int i = src.Y - frameRadius; i > dst.Y; i -= frameRadius)
					{
						result.Add(new Pair(src.X, i));
					}
				}
			}
			// 斜率為零
			else if (diffY == 0)
			{
				if (src.X < dst.Y)
				{
					for (int i = src.X + frameRadius; i < dst.X; i += frameRadius)
					{
						result.Add(new Pair(i, src.Y));
					}
				}
				else
				{
					for (int i = src.X - frameRadius; i > dst.X; i -= frameRadius)
					{
						result.Add(new Pair(i, src.Y));
					}
				}
			}
			else
			{
				// y = mx + c
				// x = (y - c) / m
				double m = (double)(src.Y - dst.Y) / (src.X - dst.X);
				double c = src.Y - m * src.X;
				double radian = Math.Atan((src.Y - dst.Y) / (src.X - dst.X));
				int distance = Math.Abs((int)(frameRadius * Math.Cos(radian)));
				if (src.X < dst.X)
				{
					for (int i = src.X + distance; i < dst.X; i += distance)
					{
						double y = Math.Round(m * i + c, 0, MidpointRounding.AwayFromZero);
						result.Add(new Pair(i, (int)y));
					}
				}
				else
				{
					for (int i = src.X - distance; i > dst.X; i -= distance)
					{
						double y = Math.Round(m * i + c, 0, MidpointRounding.AwayFromZero);
						result.Add(new Pair(i, (int)y));
					}
				}
			}
			return result;
		}
	}
}
