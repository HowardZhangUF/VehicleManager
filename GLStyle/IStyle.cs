using System.Collections.Generic;

namespace GLStyle
{
    /// <summary>
    /// 樣式介面
    /// </summary>
    public interface IStyle : IGLStyle
    {
        /// <summary>
        /// 底色
        /// </summary>
        IColor BackgroundColor { get; }

        /// <summary>
        /// 圖層位置
        /// </summary>
        int Layer { get; }

        /// <summary>
        /// 是否在選單中顯示給使用者看
        /// </summary>
        bool ShowOnTheMenu { get; }

        /// <summary>
        /// 樣式種類
        /// </summary>
        string StyleType { get; }

        /// <summary>
        /// 右鍵選單中命令
        /// </summary>
        List<string> Command { get; }
    }
}