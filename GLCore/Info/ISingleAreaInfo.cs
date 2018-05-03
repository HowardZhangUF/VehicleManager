namespace GLCore
{
    /// <summary>
    /// 標示面資訊介面
    /// </summary>
    public interface ISingleAreaInfo : IGLCore
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
        /// 最小 X 座標
        /// </summary>
        int MinX { get; set; }

        /// <summary>
        /// 最小 Y 座標
        /// </summary>
        int MinY { get; set; }

        /// <summary>
        /// 最大 X 座標
        /// </summary>
        int MaxX { get; set; }

        /// <summary>
        /// 最大 Y 座標
        /// </summary>
        int MaxY { get; set; }
    }
}