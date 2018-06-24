using System;

namespace SerialData
{
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

        /// <summary>
        /// 這個是什麼？ - Wood
        /// </summary>
        Map
    }
}
