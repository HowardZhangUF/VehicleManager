using Geometry;
using System;
using System.Collections.Generic;

namespace GLUI
{
    #region 右鍵命令事件

    /// <summary>
    /// 右鍵命令事件委派
    /// </summary>
    public delegate void CommandOnClickEvent(object sender, CommandOnClickEventArgs e);

    /// <summary>
    /// 右鍵命令事件參數
    /// </summary>
    public class CommandOnClickEventArgs : EventArgs
    {
        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; set; }
    }

    #endregion 右鍵命令事件

    #region 地圖載入

    /// <summary>
    /// 地圖載入事件委派
    /// </summary>
    public delegate void LoadMapEvent(object sender, LoadMapEventArgs e);

    /// <summary>
    /// 地圖載入事件參數
    /// </summary>
    public class LoadMapEventArgs : EventArgs
    {
        /// <summary>
        /// 地圖路徑
        /// </summary>
        public string MapPath { get; set; }
    }

    #endregion 地圖載入

    #region 擦障礙點

    /// <summary>
    /// 擦障礙點事件委派
    /// </summary>
    public delegate void EraserMapEvent(object sender, EraserMapEventArgs e);

    /// <summary>
    /// 擦障礙點事件參數
    /// </summary>
    public class EraserMapEventArgs : EventArgs
    {
        /// <summary>
        /// 擦掉的範圍
        /// </summary>
        public IArea Range { get; set; }
    }

    #endregion 擦障礙點

    #region 加入障礙點

    /// <summary>
    /// 加入障礙點事件委派
    /// </summary>
    public delegate void PenMapEvent(object sender, PenMapEventArgs e);

    /// <summary>
    /// 加入障礙點事件參數
    /// </summary>
    public class PenMapEventArgs : EventArgs
    {
        /// <summary>
        /// 加入的資料
        /// </summary>
        public IEnumerable<IPair> Data { get; set; }
    }

    #endregion 加入障礙點
}