using Geometry;
using Serialization;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialData
{
    /// <summary>
	/// AGV 狀態
	/// </summary>
	[Serializable]
    public class AGVStatus : ISerializable
    {
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string AlarmMessage { get; set; }

        /// <summary>
        /// 電池電量(%)
        /// </summary>
        public double Battery { get; set; }

        /// <summary>
        /// 當下位置，單位 mm、deg
        /// </summary>
        public ITowardPair Data { get; set; } = new TowardPair();

        /// <summary>
        /// AGV 狀態描述
        /// </summary>
        public EDescription Description { get; set; }

        /// <summary>
        /// 目標點
        /// </summary>
        public string GoalName { get; set; }

        /// <summary>
        /// 地圖匹配度(%)
        /// </summary>
        public double MapMatch { get; set; }

        /// <summary>
        /// AGV 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// <para>安全框(mm)</para>
        /// </summary>
        public IArea SafetyArea { get; set; } = new Area(-400, -300, 400, 300);

        /// <summary>
        /// 訊息時間戳
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 流水號，用來識別遠端訊息回應的對象
        /// </summary>
        public uint TxID { get; set; }

        /// <summary>
        /// 當下實際速度(mm/s)
        /// </summary>
        public double Velocity { get; set; }

        /// <summary>
        /// 產生假資料
        /// </summary>
        public static AGVStatus CreateFakeData()
        {
            Random rnd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")));
            AGVStatus status = new AGVStatus();
            status.AlarmMessage = "No Alarm";
            status.Battery = rnd.NextDouble(0, 100);
            status.Data = new TowardPair(rnd.NextDouble(0, 1000), rnd.NextDouble(0, 1000), rnd.NextDouble(0, 360));
            status.Description = EDescription.Idle;
            status.GoalName = "Castec";
            status.MapMatch = rnd.NextDouble(0, 100);
            status.Name = "AGV01";
            status.SafetyArea = new Area(-400, -300, 400, 300);
            status.TimeStamp = DateTime.Now;
            status.TxID = 666;
            status.Velocity = rnd.NextDouble(0, 100);
            return status;
        }
    }
}
