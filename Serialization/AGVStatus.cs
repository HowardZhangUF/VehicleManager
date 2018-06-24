using Geometry;
using System;

namespace Serialization
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

	/// <summary>
	/// <para>AGV 狀態描述</para>
	/// </summary>
	[Serializable]
	public enum EDescription
	{
		/// <summary>
		/// 閒置
		/// </summary>
		Idle,

		/// <summary>
		/// 充電
		/// </summary>
		Charge,

		/// <summary>
		/// 跑 Goal 點
		/// </summary>
		Running,

		/// <summary>
		/// 暫停
		/// </summary>
		Pause,

		/// <summary>
		/// 抵達目標點 
		/// </summary>
		Arrived,

		/// <summary>
		/// 發生錯誤
		/// </summary>
		Alarm,

		/// <summary>
		/// 偵測到障礙物
		/// </summary>
		ObstacleExists,

		/// <summary>
		/// 更新地圖
		/// </summary>
		MapUpdate,

		/// <summary>
		/// 車輛鎖定
		/// </summary>
		Lock,

		Map
	}

	/// <summary>
	/// 自訂擴充方法
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// 傳回 minValue 和 maxValue 之間的隨機浮點數。
		/// </summary>
		public static double NextDouble(this Random rnd, int minValue, int maxValue)
		{
			return rnd.Next(minValue * 1000, maxValue * 1000) / 1000.0;
		}
	}
}
