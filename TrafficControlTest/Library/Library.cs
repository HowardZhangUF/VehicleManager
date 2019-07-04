using KdTree;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Implement;
using TrafficControlTest.Interface;

namespace TrafficControlTest.Library
{
	public enum ConnectState
	{
		Closed,
		Connected,
		Disconnected
	}

	public enum ListenState
	{
		Closed,
		Listening
	}

	public static class Library
	{
		public const string TIME_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";

		public static T GetDeepClone<T>(T o)
		{
			using (var ms = new System.IO.MemoryStream())
			{
				var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				formatter.Serialize(ms, o);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}

		#region Factory
		public static IPoint2D GenerateIPoint2D(int X, int Y)
		{
			return new Point2D(X, Y);
		}
		public static ITowardPoint2D GenerateITowardPoint2D(int X, int Y, double Toward)
		{
			return new TowardPoint2D(X, Y, Toward);
		}
		public static ITowardPoint2D GenerateITowardPoint2D(IPoint2D Point, double Toward)
		{
			return new TowardPoint2D(Point.mX, Point.mY, Toward);
		}
		public static IVector2D GenerateIVector2D(double XComponent, double YComponent)
		{
			return new Implement.Vector2D(XComponent, YComponent);
		}
		public static IRectangle2D GenerateIRectangle2D(IPoint2D MaxPoint, IPoint2D MinPoint)
		{
			return new Rectangle2D(MaxPoint, MinPoint);
		}
		public static IPathRegionOverlapPair GenerateIPathRegionOverlapPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D OverlapRegionOfPathRegions)
		{
			return new Implement.PathRegionOverlapPair(Vehicle1, Vehicle2, OverlapRegionOfPathRegions);
		}
		public static IPathOverlapPair GenerateIPathOverlapPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IEnumerable<IRectangle2D> OverlapRegionsOfPaths)
		{
			return new Implement.PathOverlapPair(Vehicle1, Vehicle2, OverlapRegionsOfPaths);
		}
		public static ICollisionPair GenerateICollisionPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D CollisionRegion, ITimePeriod Period)
		{
			return new Implement.CollisionPair(Vehicle1, Vehicle2, CollisionRegion, Period);
		}
		public static ITimePeriod GenerateITimePeriod(DateTime Start, DateTime End)
		{
			return new TimePeriod(Start, End);
		}
		public static IVehicleInfo GenerateIVehicleInfo(string Name)
		{
			return new VehicleInfo(Name);
		}
		public static IVehicleCommunicator GenerateIVehicleCommunicator()
		{
			return new VehicleCommunicator();
		}
		public static ICommunicatorClient GenerateICommunicatorClient()
		{
			return new CommunicatorClient();
		}
		public static IVehicleInfoManager GenerateIVehicleInfoManager()
		{
			return new VehicleInfoManager();
		}
		public static IVehicleMessageAnalyzer GenerateIVehicleMessageAnalyzer(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			return new VehicleMessageAnalyzer(VehicleCommunicator, VehicleInfoManager);
		}
		public static ICollisionEventManager GenerateICollisionEventManager()
		{
			return new CollisionEventManager();
		}
		public static ICollisionEventDetector GenerateICollisionEventDetector(IVehicleInfoManager VehicleInfoManager, ICollisionEventManager CollisionEventManager)
		{
			return new CollisionEventDetector(VehicleInfoManager, CollisionEventManager);
		}
		#endregion

		#region IPoint2D
		/// <summary>計算向量 (End - Start) 與 X+ 的夾角。範圍為 -180 ~ 180 ，單位為 degree</summary>
		public static double GetAngle(IPoint2D Start, IPoint2D End)
		{
			return Math.Atan2(End.mY - Start.mY, End.mX - Start.mX) / Math.PI * 180.0f;
		}
		public static double GetDistance(IPoint2D Point1, IPoint2D Point2)
		{
			return Math.Sqrt(GetDistanceSquare(Point1, Point2));
		}
		/// <summary>計算點集合依序連起來的線段的長度總和</summary>
		public static double GetDistance(IEnumerable<IPoint2D> Points)
		{
			double result = 0;
			if (Points.Count() > 1)
			{
				for (int i = 1; i < Points.Count(); ++i)
				{
					result += GetDistance(Points.ElementAt(i - 1), Points.ElementAt(i));
				}
			}
			return result;
		}
		public static int GetDistanceSquare(IPoint2D Point1, IPoint2D Point2)
		{
			return (int)(Math.Pow(Point2.mX - Point1.mX, 2) + Math.Pow(Point2.mY - Point1.mY, 2));
		}
		/// <summary>計算指定矩形與指定線段(由兩點組成)的交點。最多有可能會有兩個交點</summary>
		/// <remarks></remarks>
		public static IEnumerable<IPoint2D> GetIntersectionPoint(IRectangle2D Rectangle, IPoint2D Point1, IPoint2D Point2)
		{
			List<IPoint2D> result = null;
			if (Rectangle != null && Point1 != null && Point2 != null)
			{
				result = new List<IPoint2D>();
				// 計算矩形的四個邊與兩點組成的線段的交點
				IPoint2D tmp = null;
				tmp = GetIntersectionPoint(Point1, Point2, GenerateIPoint2D(Rectangle.mMaxX, Rectangle.mMaxY), GenerateIPoint2D(Rectangle.mMaxX, Rectangle.mMinY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
				tmp = GetIntersectionPoint(Point1, Point2, GenerateIPoint2D(Rectangle.mMaxX, Rectangle.mMaxY), GenerateIPoint2D(Rectangle.mMinX, Rectangle.mMaxY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
				tmp = GetIntersectionPoint(Point1, Point2, GenerateIPoint2D(Rectangle.mMinX, Rectangle.mMinY), GenerateIPoint2D(Rectangle.mMaxX, Rectangle.mMinY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
				tmp = GetIntersectionPoint(Point1, Point2, GenerateIPoint2D(Rectangle.mMinX, Rectangle.mMinY), GenerateIPoint2D(Rectangle.mMinX, Rectangle.mMaxY));
				if (tmp != null && !result.Any((o) => o.ToString() == tmp.ToString())) result.Add(tmp);
			}
			return result;
		}
		/// <summary>計算兩線段(由兩點組成)的交點</summary>
		public static IPoint2D GetIntersectionPoint(IPoint2D Line1Point1, IPoint2D Line1Point2, IPoint2D Line2Point1, IPoint2D Line2Point2)
		{
			/*
			 * 參考資料：https://www.cnblogs.com/sanmubai/p/7306599.html
			 * 
			 * 線段 AB 其端點為 (xa, ya) 與 (xb, yb) ，其直線方程為：
			 * x = xa + lambda * (xb - xa)
			 * y = ya + lambda * (yb - ya)
			 * 0 <= lambda <= 1
			 * 
			 * 線段 CD 其端點為 (xc, yc) 與 (xd, yd) ，其直線方程為：
			 * x = xc + micro * (xd - xc)
			 * y = yc + micro * (yd - yc)
			 * 0 <= micro <= 1
			 * 
			 * 則交點應滿足：
			 * x = xa + lambda * (xb - xa) = xc + micro * (xd - xc)
			 * y = ya + lambda * (yb - ya) = yc + micro * (yd - yc)
			 * 
			 * 可整理成：
			 * (xb - xa) * lambda - (xd - xc) * micro = xc - xa
			 * (yb - ya) * lambda - (yd - yc) * micro = yc - ya
			 * 
			 * 行列式 delta 的算法為：
			 * A = |(xb - xa) -(xd - xc)|
			 * 	   |(yb - ya) -(yd - yc)|
			 * delta = (xb - xa) * (-(yd - yc)) - (-(xd - xc)) * (yb - ya)
			 * 	     = (xb - xa) * (yc - yd)    - (xc - xd)    * (yb - ya)
			 * 
			 * 若其行列式等於零，表示線段 AB 與線段 CD 重合或平行。
			 * 
			 * 若其行列式不等於零，則可求出：
			 * lambda = 1 / delta * det|(xc - xa) -(xd - xc)|
			 * 						   |(yc - ya) -(yd - yc)|
			 * 		  = 1 / delta * ((xc - xa) * (yc - yd) - (xc - xd) * (yc - ya))
			 * mircro = 1 / delta * det|(xb - xa) (xc - xa)|
			 * 						   |(yb - ya) (yc - ya)|
			 * 	      = 1 / delta * ((xb - xa) * (yc - ya) - (xc - xa) * (yb - ya))
			 * 
			 * 需特別注意，僅有當 0 <= lambda <= 1 且 0 <= micro <= 1 時，兩線段才有相交，
			 * 否則，交點在線段的延長線上，仍認為兩線段不相交。
			 * 
			 * 算出 lambda 與 micro 後，可得交點為：
			 * x = xa + lambda * (xb - xa)
			 * y = ya + lambda * (yb - ya)
			 */
			IPoint2D result = null;
			if (Line1Point1 != null && Line1Point2 != null && Line2Point1 != null && Line2Point2 != null)
			{
				double delta = (Line1Point2.mX - Line1Point1.mX) * (Line2Point1.mY - Line2Point2.mY) - (Line2Point1.mX - Line2Point2.mX) * (Line1Point2.mY - Line1Point1.mY);
				if (delta <= double.Epsilon && delta >= -double.Epsilon) //若 delta 為 0
				{
					result = null;
				}
				else
				{
					double lambda = ((Line2Point1.mX - Line1Point1.mX) * (Line2Point1.mY - Line2Point2.mY) - (Line2Point1.mX - Line2Point2.mX) * (Line2Point1.mY - Line1Point1.mY)) / delta;
					if (0 <= lambda && lambda <= 1)
					{
						double micro = ((Line1Point2.mX - Line1Point1.mX) * (Line2Point1.mY - Line1Point1.mY) - (Line2Point1.mX - Line1Point1.mX) * (Line1Point2.mY - Line1Point1.mY)) / delta;
						if (0 <= micro && micro <= 1)
						{
							int x = Line1Point1.mX + (int)(lambda * (Line1Point2.mX - Line1Point1.mX));
							int y = Line1Point1.mY + (int)(lambda * (Line1Point2.mY - Line1Point1.mY));
							result = GenerateIPoint2D(x, y);
						}
					}
				}
			}
			return result;
		}
		/// <summary>將一條線轉換成點集合(不包含該線的兩端點)</summary>
		public static IEnumerable<IPoint2D> ConvertLineToPoints(IPoint2D Start, IPoint2D End, int Interval)
		{
			List<IPoint2D> result = new List<IPoint2D>();

			int diffX = Math.Abs(Start.mX - End.mX);
			int diffY = Math.Abs(Start.mY - End.mY);
			// 斜率為無限大
			if (diffX == 0)
			{
				if (Start.mY < End.mY)
				{
					for (int i = Start.mY + Interval; i < End.mY; i += Interval)
					{
						result.Add(GenerateIPoint2D(Start.mX, i));
					}
				}
				else
				{
					for (int i = Start.mY - Interval; i > End.mY; i -= Interval)
					{
						result.Add(GenerateIPoint2D(Start.mX, i));
					}
				}
			}
			// 斜率為零
			else if (diffY == 0)
			{
				if (Start.mX < End.mX)
				{
					for (int i = Start.mX + Interval; i < End.mX; i += Interval)
					{
						result.Add(GenerateIPoint2D(i, Start.mY));
					}
				}
				else
				{
					for (int i = Start.mX - Interval; i > End.mX; i -= Interval)
					{
						result.Add(GenerateIPoint2D(i, Start.mY));
					}
				}
			}
			else
			{
				// y = mx + c
				// x = (y - c) / m
				double m = (double)(Start.mY - End.mY) / (Start.mX - End.mX);
				double c = Start.mY - m * Start.mX;
				double radian = Math.Atan((Start.mY - End.mY) / (Start.mX - End.mX));
				int distance = Math.Abs((int)(Interval * Math.Cos(radian)));
				if (Start.mX < End.mX)
				{
					for (int i = Start.mX + distance; i < End.mX; i += distance)
					{
						double y = Math.Round(m * i + c, 0, MidpointRounding.AwayFromZero);
						result.Add(GenerateIPoint2D(i, (int)y));
					}
				}
				else
				{
					for (int i = Start.mX - distance; i > End.mX; i -= distance)
					{
						double y = Math.Round(m * i + c, 0, MidpointRounding.AwayFromZero);
						result.Add(GenerateIPoint2D(i, (int)y));
					}
				}
			}
			return result;
		}
		public static string ConvertToString(IEnumerable<IPoint2D> Points)
		{
			string result = string.Empty;
			result = string.Join(" ", Points.Select((o) => o.ToString()));
			return result;
		}
		#endregion

		#region IVector2D
		public static IVector2D GetVector(int X1, int Y1, int X2, int Y2)
		{
			IVector2D result = null;
			result = GenerateIVector2D(X2 - X1, Y2 - Y1);
			return result;
		}
		public static IVector2D GetNormalizeVector(IVector2D Vector)
		{
			IVector2D result = null;
			result = GenerateIVector2D(Vector.mXComponent / Vector.mMagnitude, Vector.mYComponent / Vector.mMagnitude);
			return result;
		}
		public static double GetDotProduct(IVector2D Vector1, IVector2D Vector2)
		{
			double result = 0;
			if (Vector1 != null && Vector2 != null)
			{
				result = Vector1.mXComponent * Vector2.mXComponent + Vector1.mYComponent * Vector2.mYComponent;
			}
			return result;
		}
		public static double GetAngleOfTwoVector(IVector2D Vector1, IVector2D Vector2)
		{
			double result = -1;
			if (Vector1 != null && Vector2 != null)
			{
				result = (Math.Acos(GetDotProduct(Vector1, Vector2) / (Vector1.mMagnitude * Vector2.mMagnitude)) / Math.PI * 180);
			}
			return result;
		}
		#endregion

		#region IRectangle2D
		/// <summary>判斷兩個矩形是否有重疊。共用同一個邊不算重疊</summary>
		public static bool IsRectangleOverlap(IRectangle2D Rectangle1, IRectangle2D Rectangle2)
		{
			if (Rectangle1 != null && Rectangle2 != null && Rectangle1.mMaxX > Rectangle2.mMinX && Rectangle2.mMaxX > Rectangle1.mMinX && Rectangle1.mMaxY > Rectangle2.mMinY && Rectangle2.mMaxY > Rectangle1.mMinY)
				return true;
			else
				return false;
		}
		/// <summary>判斷指定點是否在指定矩形內。在矩形邊上也算是在矩形內</summary>
		public static bool IsPointInside(IPoint2D Point, IRectangle2D Rectangle)
		{
			return (Point.mX >= Rectangle.mMinX && Point.mX <= Rectangle.mMaxX && Point.mY >= Rectangle.mMinY && Point.mY <= Rectangle.mMaxY);
		}
		/// <summary>計算能涵蓋兩個指定矩形的矩形</summary>
		public static IRectangle2D GetCoverRectangle(IRectangle2D Rectangle1, IRectangle2D Rectangle2)
		{
			IRectangle2D result = null;
			int x_max = Math.Max(Rectangle1.mMaxX, Rectangle2.mMaxX);
			int y_max = Math.Max(Rectangle1.mMaxY, Rectangle2.mMaxY);
			int x_min = Math.Min(Rectangle1.mMinX, Rectangle2.mMinX);
			int y_min = Math.Min(Rectangle1.mMinY, Rectangle2.mMinY);
			if (x_min < x_max && y_min < y_max)
			{
				result = GenerateIRectangle2D(GenerateIPoint2D(x_max, y_max), GenerateIPoint2D(x_min, y_min));
			}
			return result;
		}
		/// <summary>計算兩個矩形的交集矩形</summary>
		public static IRectangle2D GetIntersectionRectangle(IRectangle2D Rectangle1, IRectangle2D Rectangle2)
		{
			IRectangle2D result = null;
			int x_max = Math.Min(Rectangle1.mMaxX, Rectangle2.mMaxX);
			int y_max = Math.Min(Rectangle1.mMaxY, Rectangle2.mMaxY);
			int x_min = Math.Max(Rectangle1.mMinX, Rectangle2.mMinX);
			int y_min = Math.Max(Rectangle1.mMinY, Rectangle2.mMinY);
			if (x_min < x_max && y_min < y_max)
			{
				result = GenerateIRectangle2D(GenerateIPoint2D(x_max, y_max), GenerateIPoint2D(x_min, y_min));
			}
			return result;
		}
		/// <summary>以指定點為中心，產生一個邊長為 Radius * 2 大小的正方形</summary>
		public static IRectangle2D GetRectangle(int X, int Y, int Radius)
		{
			IRectangle2D result = null;
			if (Radius > 0)
			{
				result = GenerateIRectangle2D(GenerateIPoint2D(X + Radius, Y + Radius), GenerateIPoint2D(X - Radius, Y - Radius));
			}
			return result;
		}
		/// <summary>將指定 Rectangle 放大。以其中心為基準， X 軸方向增加 OffsetX ， Y 軸方向增加 OffsetY</summary>
		public static IRectangle2D GetAmplifyRectangle(IRectangle2D Rectangle, int OffsetX, int OffsetY)
		{
			IRectangle2D result = null;
			int x_max = Rectangle.mMaxX + OffsetX;
			int y_max = Rectangle.mMaxY + OffsetY;
			int x_min = Rectangle.mMinX - OffsetX;
			int y_min = Rectangle.mMinY - OffsetY;
			result = GenerateIRectangle2D(GenerateIPoint2D(x_max, y_max), GenerateIPoint2D(x_min, y_min));
			return result;
		}
		/// <summary>計算能涵蓋指定 Point 集合的矩形</summary>
		public static IRectangle2D GetCoverRectangle(IEnumerable<IPoint2D> Points)
		{
			IRectangle2D result = null;
			if (Points.Count() > 1)
			{
				result = GenerateIRectangle2D(GetDeepClone(Points.ElementAt(0)), GetDeepClone(Points.ElementAt(0)));
				for (int i = 1; i < Points.Count(); ++i)
				{
					if		(Points.ElementAt(i).mX < result.mMinX) result.mMinPoint.mX = Points.ElementAt(i).mX;
					else if (Points.ElementAt(i).mX > result.mMaxX) result.mMaxPoint.mX = Points.ElementAt(i).mX;
					if		(Points.ElementAt(i).mY < result.mMinY) result.mMinPoint.mY = Points.ElementAt(i).mY;
					else if (Points.ElementAt(i).mY > result.mMaxY) result.mMaxPoint.mY = Points.ElementAt(i).mY;
				}
			}
			return result;
		}
		/// <summary>合併矩形。若其中任兩個矩形有重疊，則將其合併成一個矩形</summary>
		public static IEnumerable<IRectangle2D> MergeRectangle(IEnumerable<IRectangle2D> Rectangles)
		{
			List<IRectangle2D> result = null;
			result = GetDeepClone(Rectangles).ToList();
			if (result != null && result.Count() > 1)
			{
				for (int i = 0; i < result.Count(); ++i)
				{
					for (int j = i + 1; j < result.Count(); ++j)
					{
						if (IsRectangleOverlap(result[i], result[j]))
						{
							result.Add(GetCoverRectangle(result[i], result[j]));
							result.RemoveAt(j);
							result.RemoveAt(i);

							// 回頭再檢查一次
							i = 0 - 1;
							j = i + 1;
							break;
						}
					}
				}
			}
			return result;
		}
		#endregion

		#region IPathRegionOverlapPair
		/// <summary>判斷兩車是否會發生「路徑線區域」重疊的狀況。若有，輸出其區域資訊</summary>
		private static bool IsPathRegionOverlapPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, out IPathRegionOverlapPair PathRegionOverlapPair)
		{
			/*
			 * 輸入：兩車資訊
			 * 輸出：「路徑線區域」重疊的區域與對應的兩車，亦為「有機會」發生「路徑線」重疊的區域與對應的兩車
			 * 說明：
			 * 計算車的「路徑線區域」，其為一矩形，
			 * 接著計算兩車的「路徑線區域」是否有重疊，
			 * 若有重疊，代表兩車「有機會」發生路徑線重疊，
			 * 反之，代表兩車「不會」發生路徑線重疊。
			 */
			if (IsRectangleOverlap(Vehicle1.mPathRegion, Vehicle2.mPathRegion))
			{
				PathRegionOverlapPair = GenerateIPathRegionOverlapPair(Vehicle1, Vehicle2, GetIntersectionRectangle(Vehicle1.mPathRegion, Vehicle2.mPathRegion));
				return true;
			}
			else
			{
				PathRegionOverlapPair = null;
				return false;
			}
		}
		private static bool IsAnyPathRegionOverlapPair(IEnumerable<IVehicleInfo> Vehicles, out IEnumerable<IPathRegionOverlapPair> PathRegionOverlapPairs)
		{
			List<IPathRegionOverlapPair> tmpPathRegionOverlapPairs = new List<IPathRegionOverlapPair>();

			if (Vehicles != null && Vehicles.Count() > 1)
			{
				for (int i = 0; i < Vehicles.Count(); ++i)
				{
					for (int j = i + 1; j < Vehicles.Count(); ++j)
					{
						if (IsPathRegionOverlapPair(Vehicles.ElementAt(i), Vehicles.ElementAt(j), out IPathRegionOverlapPair PathRegionOverlapPair))
						{
							tmpPathRegionOverlapPairs.Add(PathRegionOverlapPair);
						}
					}
				}
			}

			if (tmpPathRegionOverlapPairs.Count > 0)
			{
				PathRegionOverlapPairs = tmpPathRegionOverlapPairs;
				return true;
			}
			else
			{
				PathRegionOverlapPairs = null;
				return false;
			}
		}
		#endregion

		#region IPathOverlapPair
		/// <summary>鄰近點數量。當需要使用鄰近點資料時，會使用鄰近 n 個點的資料， n 即是此變數值</summary>
		public static int mConsideredNeighbourAmount { get; set; } = 10;

		/// <summary>判斷兩車在『「路徑線區域」重疊的區域』內是否會發生「路徑線」重疊的狀況。若有，輸出其區域資訊。此種區域可能會有多個</summary>
		private static bool IsPathOverlapPair(IPathRegionOverlapPair PathRegionOverlapPair, out IPathOverlapPair PathOverlapPair)
		{
			/*
			 * 輸入：「路徑線區域」重疊的區域與對應的兩車，亦為「有機會」發生「路徑線」重疊的區域與對應的兩車
			 * 輸出：「路徑線」重疊的區域與對應的兩車
			 * 說明：
			 * 為求效率，僅使用在『「路徑線區域」重疊的區域』內的「路徑線點」來計算。
			 * 先將兩車在『「路徑線區域」重疊的區域』內的「路徑線點」擷取出來，
			 *     1. 接著兩車各自計算自身的每一個「路徑線點」的方圓 n 釐米內是否有對方的「路徑線點」，
			 *     2. 接著兩車各自計算自身的每一個「路徑線點」的方圓 n 釐米內是否有對方的路徑線通過，
			 * 若有，代表此點會發生路徑線交會，不斷使用此方法來計算所有「路徑線點」與對方路徑線重疊的區域，
			 * 然後再將所有的重疊的區域合併，即可得到「路徑線」重疊的區域。
			 */
			PathOverlapPair = null;

			IRectangle2D pathRegionOverlapRegion = PathRegionOverlapPair.mOverlapRegionOfPathRegions;
			IEnumerable<IPoint2D> filteredPathPointsOfVehicle1 = PathRegionOverlapPair.mVehicle1.mPathDetail.Where((o) => IsPointInside(o, pathRegionOverlapRegion));
			IEnumerable<IPoint2D> filteredPathPointsOfVehicle2 = PathRegionOverlapPair.mVehicle2.mPathDetail.Where((o) => IsPointInside(o, pathRegionOverlapRegion));
			int frameRadiusOfVehicle1 = PathRegionOverlapPair.mVehicle1.mTotalFrameRadius;
			int frameRadiusOfVehicle2 = PathRegionOverlapPair.mVehicle2.mTotalFrameRadius;

			IEnumerable<IRectangle2D> overlapRegionsOfVehicle2OnVehiclePath1 = null;
			IEnumerable<IRectangle2D> overlapRegionsOfVehicle1OnVehiclePath2 = null;
			CalculatePathOverlapRegions(filteredPathPointsOfVehicle1, filteredPathPointsOfVehicle2, frameRadiusOfVehicle1, mConsideredNeighbourAmount, out overlapRegionsOfVehicle2OnVehiclePath1);
			CalculatePathOverlapRegions(filteredPathPointsOfVehicle2, filteredPathPointsOfVehicle1, frameRadiusOfVehicle2, mConsideredNeighbourAmount, out overlapRegionsOfVehicle1OnVehiclePath2);

			if ((overlapRegionsOfVehicle1OnVehiclePath2 != null && overlapRegionsOfVehicle1OnVehiclePath2.Count() > 0) || (overlapRegionsOfVehicle2OnVehiclePath1 != null && overlapRegionsOfVehicle2OnVehiclePath1.Count() > 0))
			{
				List<IRectangle2D> overlapRegions = new List<IRectangle2D>();
				if (overlapRegionsOfVehicle1OnVehiclePath2 != null && overlapRegionsOfVehicle1OnVehiclePath2.Count() > 0) overlapRegions.AddRange(overlapRegionsOfVehicle1OnVehiclePath2);
				if (overlapRegionsOfVehicle2OnVehiclePath1 != null && overlapRegionsOfVehicle2OnVehiclePath1.Count() > 0) overlapRegions.AddRange(overlapRegionsOfVehicle2OnVehiclePath1);
				overlapRegions = MergeRectangle(overlapRegions).ToList();
				PathOverlapPair = GenerateIPathOverlapPair(PathRegionOverlapPair.mVehicle1, PathRegionOverlapPair.mVehicle2, overlapRegions);
			}

			return PathOverlapPair != null ? true : false;
		}
		private static bool IsAnyPathOverlapPair(IEnumerable<IPathRegionOverlapPair> PathRegionOverlapPairs, out IEnumerable<IPathOverlapPair> PathOverlapPairs)
		{
			List<IPathOverlapPair> tmpPathOverlapPairs = new List<IPathOverlapPair>();

			if (PathRegionOverlapPairs != null && PathRegionOverlapPairs.Count() > 0)
			{
				for (int i = 0; i < PathRegionOverlapPairs.Count(); ++i)
				{
					if (IsPathOverlapPair(PathRegionOverlapPairs.ElementAt(i), out IPathOverlapPair tmpPathOverlapPair))
					{
						tmpPathOverlapPairs.Add(tmpPathOverlapPair);
					}
				}
			}

			if (tmpPathOverlapPairs.Count > 0)
			{
				PathOverlapPairs = tmpPathOverlapPairs;
				return true;
			}
			else
			{
				PathOverlapPairs = null;
				return false;
			}
		}
		private static bool TryToBuildTreeOfPoints(IEnumerable<IPoint2D> Points, out KdTree<int, string> Tree)
		{
			if (Points.Count() > 0)
			{
				Tree = new KdTree<int, string>(2, new IntMath());
				for (int i = 0; i < Points.Count(); ++i)
				{
					Tree.Add(new int[] { Points.ElementAt(i).mX, Points.ElementAt(i).mY }, i.ToString());
				}
				return true;
			}
			else
			{
				Tree = null;
				return false;
			}
		}
		/// <summary>以 PointA 為基準，計算 PointsA 與 PointsB 的重疊區域</summary>
		/// <remarks>計算 PointsA 的每一個點的方圓 FrameRadius 釐米內是否有 PointsB 的點。若有的話，該點周遭 FrameRadius 的矩形即為重疊區域</remarks>
		private static bool CalculatePathOverlapRegions(IEnumerable<IPoint2D> PointsA, IEnumerable<IPoint2D> PointsB, int FrameRadius, int NeighbourAmount, out IEnumerable<IRectangle2D> OverlapRegions)
		{
			OverlapRegions = null;
			List<IRectangle2D> tmpOverlapRegions = null;
			if (PointsA != null && PointsA.Count() > 0 && PointsB != null && PointsB.Count() > 0)
			{
				if (TryToBuildTreeOfPoints(PointsB, out KdTree<int, string> treeOfPointsB))
				{
					int frameRadiusSquare = FrameRadius * FrameRadius;
					for (int i = 0; i < PointsA.Count(); ++i)
					{
						var neighbourPoints = treeOfPointsB.GetNearestNeighbours(new int[] { PointsA.ElementAt(i).mX, PointsA.ElementAt(i).mY }, NeighbourAmount);
						if (neighbourPoints.Any((o) => GetDistanceSquare(PointsA.ElementAt(i), GenerateIPoint2D(o.Point[0], o.Point[1])) < frameRadiusSquare))
						{
							IRectangle2D overlapRegion = GetRectangle(PointsA.ElementAt(i).mX, PointsA.ElementAt(i).mY, FrameRadius);
							if (tmpOverlapRegions == null)
							{
								tmpOverlapRegions = new List<IRectangle2D>();
								tmpOverlapRegions.Add(overlapRegion);
							}
							else
							{
								if (IsRectangleOverlap(tmpOverlapRegions.Last(), overlapRegion))
								{
									tmpOverlapRegions[tmpOverlapRegions.Count - 1] = GetCoverRectangle(tmpOverlapRegions.Last(), overlapRegion);
								}
								else
								{
									tmpOverlapRegions.Add(overlapRegion);
								}
							}
						}
					}

					if (tmpOverlapRegions != null && tmpOverlapRegions.Count > 0)
					{
						OverlapRegions = MergeRectangle(tmpOverlapRegions).ToList();
					}
				}
			}
			return (OverlapRegions != null && OverlapRegions.Count() > 0) ? true : false;
		}
		#endregion

		#region ICollisionPair
		public static bool IsAnyCollisionPair(IEnumerable<IVehicleInfo> Vehicles, out IEnumerable<ICollisionPair> CollisionPairs)
		{
			try
			{
				if (IsAnyPathRegionOverlapPair(Vehicles, out IEnumerable<IPathRegionOverlapPair> tmpPathRegionOverlapPairs))
				{
					if (IsAnyPathOverlapPair(tmpPathRegionOverlapPairs, out IEnumerable<IPathOverlapPair> tmpPathOverlapPairs))
					{
						if (IsAnyCollisionPair(tmpPathOverlapPairs, out IEnumerable<ICollisionPair> tmpCollisionPairs))
						{
							CollisionPairs = tmpCollisionPairs;
							return true;
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Console.WriteLine(Ex.ToString());
			}

			CollisionPairs = null;
			return false;
		}
		/// <summary>判斷兩車在『「路徑線」重疊的區域』內是否會發生交會。若有，輸出其交會資訊</summary>
		private static bool IsCollisionPair(IPathOverlapPair PathOverlapPair, out ICollisionPair CollisionPair)
		{
			/*
			 * 輸入：「路徑線」重疊的區域與對應的兩車
			 * 輸出：發生交會事件的區域與時間與對應的兩車
			 * 說明：
			 * 分別計算兩車進入/離開每一個『「路徑線」重疊的區域』的時間區間，
			 * 並計算時間區間是否有重疊，若有的話代表該區域將會發生交會，
			 * 將所有的「路徑線」重疊的區域都計算過一輪後，取出最早發生的時間區間與「路徑線」重疊的區域，其即是接下來將發生交會事件的資訊。
			 */
			CollisionPair = null;
			if (PathOverlapPair != null && PathOverlapPair.mOverlapRegionsOfPaths != null && PathOverlapPair.mOverlapRegionsOfPaths.Count() > 0)
			{
				List<ICollisionPair> tmpCollisionPairs = new List<ICollisionPair>();
				for (int i = 0; i < PathOverlapPair.mOverlapRegionsOfPaths.Count(); ++i)
				{
					CalculatePassInfo(PathOverlapPair.mVehicle1, PathOverlapPair.mOverlapRegionsOfPaths.ElementAt(i), out ITowardPoint2D EnterPoint1, out ITowardPoint2D ExitPoint1, out double EnterDistance1, out double ExitDistance1, out ITimePeriod PassPeriod1);
					CalculatePassInfo(PathOverlapPair.mVehicle2, PathOverlapPair.mOverlapRegionsOfPaths.ElementAt(i), out ITowardPoint2D EnterPoint2, out ITowardPoint2D ExitPoint2, out double EnterDistance2, out double ExitDistance2, out ITimePeriod PassPeriod2);
					if (IsOverlap(PassPeriod1, PassPeriod2))
					{
						tmpCollisionPairs.Add(GenerateICollisionPair(PathOverlapPair.mVehicle1, PathOverlapPair.mVehicle2, PathOverlapPair.mOverlapRegionsOfPaths.ElementAt(i), PassPeriod1.mStart < PassPeriod2.mStart ? PassPeriod1 : PassPeriod2));
					}
				}

				if (tmpCollisionPairs.Count() > 0)
				{
					if (tmpCollisionPairs.Count() == 1)
					{
						CollisionPair = tmpCollisionPairs.First();
					}
					else
					{
						int indexOfEarliest = 0;
						DateTime timeOfEarliest = tmpCollisionPairs.ElementAt(0).mPeriod.mStart;
						for (int i = 1; i < tmpCollisionPairs.Count(); ++i)
						{
							if (tmpCollisionPairs.ElementAt(i).mPeriod.mStart < timeOfEarliest)
							{
								indexOfEarliest = i;
								timeOfEarliest = tmpCollisionPairs.ElementAt(i).mPeriod.mStart;
							}
						}
						CollisionPair = tmpCollisionPairs.ElementAt(indexOfEarliest);
					}
				}
			}
			return CollisionPair != null ? true : false;
		}
		private static bool IsAnyCollisionPair(IEnumerable<IPathOverlapPair> PathOverlapPairs, out IEnumerable<ICollisionPair> CollisionPairs)
		{
			List<ICollisionPair> tmpCollisionPairs = new List<ICollisionPair>();

			if (PathOverlapPairs != null && PathOverlapPairs.Count() > 0)
			{
				for (int i = 0; i < PathOverlapPairs.Count(); ++i)
				{
					if (IsCollisionPair(PathOverlapPairs.ElementAt(i), out ICollisionPair tmpCollisionPair))
					{
						tmpCollisionPairs.Add(tmpCollisionPair);
					}
				}
			}

			if (tmpCollisionPairs.Count > 0)
			{
				CollisionPairs = tmpCollisionPairs;
				return true;
			}
			else
			{
				CollisionPairs = null;
				return false;
			}
		}
		/// <summary>計算車子通過指令區域的相關資訊</summary>
		private static void CalculatePassInfo(IVehicleInfo Vehicle, IRectangle2D Region, out ITowardPoint2D EnterPoint, out ITowardPoint2D ExitPoint, out double EnterDistance, out double ExitDistance, out ITimePeriod PassPeriod)
		{
			EnterPoint = null;
			ExitPoint = null;
			EnterDistance = 0;
			ExitDistance = 0;
			PassPeriod = null;

			if (Vehicle.mPath.Count() > 0)
			{
				List<IPoint2D> tmpPath = Vehicle.mPath.ToList();
				tmpPath.Insert(0, Vehicle.mPosition);
				for (int i = 1; i < tmpPath.Count(); ++i)
				{
					IEnumerable<IPoint2D> intersectionPoints = GetIntersectionPoint(Region, tmpPath[i - 1], tmpPath[i]);
					switch (intersectionPoints.Count())
					{
						case 1:
							// 第一個點在矩形外，第二個點在矩形內
							if (!IsPointInside(tmpPath[i - 1], Region) && IsPointInside(tmpPath[i], Region))
							{
								if (EnterPoint == null)
								{
									EnterPoint = GenerateITowardPoint2D(intersectionPoints.First(), GetAngle(tmpPath[i - 1], tmpPath[i]));
									EnterDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.First());
								}
							}
							// 第一個點在矩形內，第二個點在矩形外
							else
							{
								if (ExitPoint == null)
								{
									ExitPoint = GenerateITowardPoint2D(intersectionPoints.First(), GetAngle(tmpPath[i - 1], tmpPath[i]));
									ExitDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.First());
								}
							}
							break;
						case 2:
							double distance1 = GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(0)); // 距離較小的點為進入點，距離較大的點為離開點
							double distance2 = GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(1));
							if (distance1 < distance2)
							{
								if (EnterPoint == null)
								{
									EnterPoint = GenerateITowardPoint2D(intersectionPoints.ElementAt(0), GetAngle(tmpPath[i - 1], tmpPath[i]));
									EnterDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(0));
								}
								if (ExitPoint == null)
								{
									ExitPoint = GenerateITowardPoint2D(intersectionPoints.ElementAt(1), GetAngle(tmpPath[i - 1], tmpPath[i]));
									ExitDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(1));
								}
							}
							else
							{
								if (EnterPoint == null)
								{
									EnterPoint = GenerateITowardPoint2D(intersectionPoints.ElementAt(1), GetAngle(tmpPath[i - 1], tmpPath[i]));
									EnterDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(1));
								}
								if (ExitPoint == null)
								{
									ExitPoint = GenerateITowardPoint2D(intersectionPoints.ElementAt(0), GetAngle(tmpPath[i - 1], tmpPath[i]));
									ExitDistance = GetDistance(tmpPath.Take(i)) + GetDistance(tmpPath[i - 1], intersectionPoints.ElementAt(0));
								}
							}
							break;
						default:
							break;
					}
				}

				double velocity = !double.IsNaN(Vehicle.mAverageVelocity) ? Vehicle.mAverageVelocity : Vehicle.mVelocity;
				PassPeriod = GenerateITimePeriod(DateTime.Now.AddSeconds(EnterDistance / velocity), DateTime.Now.AddSeconds(ExitDistance / velocity));
			}
		}
		#endregion

		#region ITimePeriod
		public static bool IsOverlap(ITimePeriod Period1, ITimePeriod Period2)
		{
			bool result = false;
			if (Period1 != null && Period2 != null)
			{
				result =
				result = (Period1.mStart >= Period2.mStart && Period1.mStart <= Period2.mEnd)
					|| (Period1.mEnd >= Period2.mStart && Period1.mEnd <= Period2.mEnd)
					|| (Period2.mStart >= Period1.mStart && Period2.mStart <= Period1.mEnd)
					|| (Period2.mEnd >= Period1.mStart && Period2.mEnd <= Period1.mEnd);
			}
			return result;
		}
		public static ITimePeriod GetUnion(ITimePeriod Period1, ITimePeriod Period2)
		{
			ITimePeriod result = null;
			if (Period1 != null && Period2 != null)
			{
				DateTime Start = Period1.mStart < Period2.mStart ? Period1.mStart : Period2.mStart;
				DateTime End = Period1.mEnd > Period2.mEnd ? Period1.mEnd : Period2.mEnd;
				result = GenerateITimePeriod(Start, End);
			}
			return result;
		}
		#endregion
	}

	public static class EventHandlerLibraryOfIVehicleInfo
	{
		public delegate void EventHandlerIVehicleInfo(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo);
	}

	public static class EventHandlerLibraryOfIVehicleInfoManager
	{
		public delegate void EventHandlerIVehicleInfo(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo);
	}

	public static class EventHandlerLibraryOfIVehicleCommunicator
	{
		public delegate void EventHandlerDateTime(DateTime OccurTime);
		public delegate void EventHandlerRemoteConnectState(DateTime OccurTime, string IpPort, ConnectState NewState);
		public delegate void EventHandlerLocalListenState(DateTime OccurTime, ListenState NewState);
		public delegate void EventHandlerSentSerializableData(DateTime OccurTime, string IpPort, object Data);
		public delegate void EventHandlerReceivedSerializableData(DateTime OccurTime, string IpPort, object Data);
	}

	public static class EventHandlerLibraryOfICollisionEventManager
	{
		public delegate void EventHandlerICollisionPair(DateTime OccurTime, string Name, ICollisionPair CollisionPair);
	}
}
