using Serialization;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SerialData
{
	/// <summary>
	/// AGV 狀態
	/// </summary>
	[Serializable]
	public class AGVStatus : Serializable
	{
		/// <summary>
		/// 錯誤訊息
		/// </summary>
		[DisplayName("Alarm Message")]
		public string AlarmMessage { get; set; }

		/// <summary>
		/// 電池電量(%)
		/// </summary>
		[DisplayName("Battery(%)")]
		public double Battery { get; set; }

		/// <summary>
		/// 當前位置， X (mm)
		/// </summary>
		[DisplayName("X(mm)")]
		public double X { get; set; }

		/// <summary>
		/// 當前位置， Y (mm)
		/// </summary>
		[DisplayName("Y(mm)")]
		public double Y { get; set; }

		/// <summary>
		/// 當前位置，面向 (degree)
		/// </summary>
		[DisplayName("Toward(degree)")]
		public double Toward { get; set; }

        /// <summary>
        /// AGV 狀態描述
        /// </summary>
        [DisplayName("Description")]
        public EDescription Description { get; set; }

        /// <summary>
        /// 目標點
        /// </summary>
        [DisplayName("Goal")]
        public string GoalName { get; set; }

        /// <summary>
        /// 地圖匹配度(%)
        /// </summary>
        [DisplayName("Match(%)")]
        public double MapMatch { get; set; }

        /// <summary>
        /// AGV 名稱
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 當下實際速度(mm/s)
        /// </summary>
        [DisplayName("Velocity(mm/s)")]
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
			status.X = rnd.NextDouble(0, 1000);
			status.Y = rnd.NextDouble(0, 1000);
			status.Toward = rnd.NextDouble(0, 360);
			status.Description = EDescription.Idle;
            status.GoalName = "Castec";
            status.MapMatch = rnd.NextDouble(0, 100);
            status.Name = "AGV01";
            status.TimeStamp = DateTime.Now;
            status.TxID = 666;
            status.Velocity = rnd.NextDouble(0, 100);
            return status;
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
