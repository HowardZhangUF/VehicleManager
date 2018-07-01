using Serialization;
using System;

namespace SerialData
{
    /// <summary>
    /// 自動回應設定
    /// </summary>
    [Serializable]
    public class AutoReport : Serializable
    {
        /// <summary>
        /// 啟用/禁用自動回應
        /// </summary>
        public bool Enabled { get; set; }
    }

    /// <summary>
    /// 執行定位
    /// </summary>
    [Serializable]
    public class DoPositionComfirm : Serializable
    {
    }

    /// <summary>
    /// Goto
    /// </summary>
    [Serializable]
    public class DoRunningByGoalName : Serializable
    {
        /// <summary>
        /// Goal 或 Dock 的名稱
        /// </summary>
        public string GoalName { get; set; }
    }

    /// <summary>
    /// 執行定位
    /// </summary>
    [Serializable]
    public class FindPath : Serializable
    {
        /// <summary>
        /// Goal 或 Dock 的名稱
        /// </summary>
        public string GoalName { get; set; }
    }

    /// <summary>
    /// 獲取所有 *.Map 的文件名稱
    /// </summary>
    [Serializable]
    public class RequestMapList : Serializable
    {
    }

    /// <summary>
    /// 獲取所有 *.Ori 的文件名稱
    /// </summary>
    [Serializable]
    public class RequestOriList : Serializable
    {
    }

    /// <summary>
    /// 要求回應雷射資料
    /// </summary>
    [Serializable]
    public class RequestLaser : Serializable
    {
    }

    /// <summary>
    /// 讀取 *.Map 檔
    /// </summary>
    [Serializable]
    public class GetMap : Serializable
    {
        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 讀取 *.Ori 檔
    /// </summary>
    [Serializable]
    public class GetOri : Serializable
    {
        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 是否啟動馬達
    /// </summary>
    [Serializable]
    public class SetServoMode : Serializable
    {
        /// <summary>
        /// 啟用/關閉馬達
        /// </summary>
        public bool Enabled { get; set; }
    }

    /// <summary>
    /// 設定手動速度
    /// </summary>
    [Serializable]
    public class SetManualVelocity : Serializable
    {
        /// <summary>
        /// 速度 mm/sec
        /// </summary>
        public int Velocity { get; set; }
    }
}