using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLUI
{
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
}
