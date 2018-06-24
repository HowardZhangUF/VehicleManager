using Geometry;
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
    public class AGVPath : ISerializable
    {
        /// <summary>
        /// AGV 名稱
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// AGV 路徑
        /// </summary>
        [DisplayName("Path")]
        public List<IPair> Path { get; set; }

        /// <summary>
        /// 訊息時間戳
        /// </summary>
        [DisplayName("Time Stamp")]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 流水號，用來識別遠端訊息回應的對象
        /// </summary>
        [DisplayName("TxID")]
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

        /// <summary>
        /// 使用 <see cref="DisplayNameAttribute"/> 組合資料
        /// </summary>
        public override string ToString()
        {
            return this.ToString("|");
        }
    }
}
