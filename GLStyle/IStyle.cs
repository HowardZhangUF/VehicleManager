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
        /// 樣式種類
        /// </summary>
        string StyleType { get; }
    }
}