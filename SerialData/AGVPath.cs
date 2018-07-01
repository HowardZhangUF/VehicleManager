using Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SerialData
{
    /// <summary>
    /// AGV 路徑
    /// </summary>
    [Serializable]
    public class AGVPath : Serializable
    {
        /// <summary>
        /// AGV 名稱
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// AGV 路徑， X
        /// </summary>
        [DisplayName("PathX")]
        public List<double> PathX { get; set; }

		/// <summary>
		/// AGV 路徑， Y
		/// </summary>
		[DisplayName("PathY")]
		public List<double> PathY { get; set; }

        /// <summary>
        /// 產生假資料
        /// </summary>
        public static AGVPath CreateFakeData()
        {
            Random rnd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")));
            AGVPath path = new AGVPath();
			path.PathX = new List<double>();
			path.PathX.Add(rnd.NextDouble(0, 1000));
			path.PathX.Add(rnd.NextDouble(0, 1000));
			path.PathX.Add(rnd.NextDouble(0, 1000));
			path.PathX.Add(rnd.NextDouble(0, 1000));
			path.PathY = new List<double>();
			path.PathY.Add(rnd.NextDouble(0, 1000));
			path.PathY.Add(rnd.NextDouble(0, 1000));
			path.PathY.Add(rnd.NextDouble(0, 1000));
			path.PathY.Add(rnd.NextDouble(0, 1000));
            path.Name = "AGV01";
            path.TimeStamp = DateTime.Now;
            path.TxID = 666;
            return path;
        }

        /// <summary>
        /// 使用 <see cref="DisplayNameAttribute"/> 組合資料
        /// </summary>
        public override string ToString()
        {
            return this.ToString("|");
        }
    }
}
