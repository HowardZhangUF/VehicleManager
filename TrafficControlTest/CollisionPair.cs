using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>發生交會的組合</summary>
	public class CollisionPair
	{
		public AGVInfo AGV1 = null;
		public AGVInfo AGV2 = null;
		public DateTime OverlapBeginTime { get { return OverlapBeginTimeOfAGV1 < OverlapBeginTimeOfAGV2 ? OverlapBeginTimeOfAGV1 : OverlapBeginTimeOfAGV2; } }
		public DateTime OverlapEndTime { get { return OverlapEndTimeOfAGV1 > OverlapBeginTimeOfAGV2 ? OverlapEndTimeOfAGV1 : OverlapEndTimeOfAGV2; } }
		public DateTime OverlapBeginTimeOfAGV1;
		public DateTime OverlapEndTimeOfAGV1;
		public DateTime OverlapBeginTimeOfAGV2;
		public DateTime OverlapEndTimeOfAGV2;
		public double EnterAngleOfAGV1;
		public double LeaveAngleOfAGV1;
		public double EnterAngleOfAGV2;
		public double LeaveAngleOfAGV2;
		private CollisionPair() { }
		public static CollisionPair IsCollision(PathOverlapPair pair)
		{
			CollisionPair result = null;

			return result;
		}
	}
}
