namespace GLCore
{
    /// <summary>
    /// 標示線資訊介面
    /// </summary>
    public interface ISingleLineInfo : IGLCore
    {
        /// <summary>
        /// 序號
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        string StyleName { get; set; }

        /// <summary>
        /// 起點 X 座標
        /// </summary>
        int X0 { get; set; }

        /// <summary>
        /// 起點 Y 座標
        /// </summary>
        int Y0 { get; set; }

        /// <summary>
        /// 終點 X 座標
        /// </summary>
        int X1 { get; set; }

        /// <summary>
        /// 終點 Y 座標
        /// </summary>
        int Y1 { get; set; }
    }
}