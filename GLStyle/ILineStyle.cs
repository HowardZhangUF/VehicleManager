namespace GLStyle
{
    /// <summary>
    /// 線段樣式介面
    /// </summary>
    public interface ILineStyle : IGLStyle, IStyle
    {
        /// <summary>
        /// 線段樣式
        /// </summary>
        ELinePattern Pattern { get; }

        /// <summary>
        /// 線條寬
        /// </summary>
        float Width { get; }
    }
}