namespace GLStyle
{
    /// <summary>
    /// AGV 樣式介面
    /// </summary>
    public interface IAGVStyle : IGLStyle, IStyle, ITowardPairStyle
    {
        /// <summary>
        /// 安全框顏色
        /// </summary>
        IColor SafetyColor { get; }

        /// <summary>
        /// 安全框高
        /// </summary>
        int SafetyHeight { get; }

        /// <summary>
        /// 安全框線條樣式
        /// </summary>
        ELinePattern SafetyLinePattern { get; }

        /// <summary>
        /// 安全框線寬
        /// </summary>
        float SafetyLineWidth { get; }

        /// <summary>
        /// 安全框寬
        /// </summary>
        int SafetyWidth { get; }
    }
}