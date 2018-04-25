namespace GLStyle
{
    /// <summary>
    /// 點樣式介面
    /// </summary>
    public interface IPairStyle : IGLStyle, IStyle
    {
        /// <summary>
        /// 點大小
        /// </summary>
        float Size { get; }
    }
}