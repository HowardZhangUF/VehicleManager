namespace GLStyle
{
    /// <summary>
    /// 標示物樣式介面
    /// </summary>
    public interface ITowardPairStyle : IGLStyle, IStyle
    {
        /// <summary>
        /// 長
        /// </summary>
        float Height { get; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        string ImagePath { get; }

        /// <summary>
        /// 線段顏色
        /// </summary>
        IColor LineColor { get; }

        /// <summary>
        /// 線長
        /// </summary>
        float LineLength { get; }

        /// <summary>
        /// 線段樣式
        /// </summary>
        ELinePattern LinePattern { get; }

        /// <summary>
        /// 線條寬
        /// </summary>
        float LineWidth { get; }

        /// <summary>
        /// 寬
        /// </summary>
        float Width { get; }
    }
}