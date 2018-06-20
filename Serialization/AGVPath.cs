using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
	/// <summary>
	/// AGV 路徑
	/// </summary>
	[Serializable]
	public class AGVPath : ISerializable
	{
		/// <summary>
		/// AGV 名稱
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// AGV 路徑
		/// </summary>
		public List<IPair> Path { get; set; }

		/// <summary>
		/// 訊息時間戳
		/// </summary>
		public DateTime TimeStamp { get; set; } = DateTime.Now;

		/// <summary>
		/// 流水號，用來識別遠端訊息回應的對象
		/// </summary>
		public uint TxID { get; set; }

		/// <summary>
		/// 產生假資料
		/// </summary>
		public static AGVPath CreateFakeData()
		{
			Random rnd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")));
			AGVPath path = new AGVPath();
			path.Path = new List<IPair>();
			path.Path.Add(new Pair(rnd.Next(0, 100), rnd.Next(0, 100)));
			path.Path.Add(new Pair(rnd.Next(0, 100), rnd.Next(0, 100)));
			path.Path.Add(new Pair(rnd.Next(0, 100), rnd.Next(0, 100)));
			path.Path.Add(new Pair(rnd.Next(0, 100), rnd.Next(0, 100)));
			path.Path.Add(new Pair(rnd.Next(0, 100), rnd.Next(0, 100)));
			path.Path.Add(new Pair(rnd.Next(0, 100), rnd.Next(0, 100)));
			path.Name = "AGV01";
			path.TimeStamp = DateTime.Now;
			path.TxID = 666;
			return path;
		}
	}
}
