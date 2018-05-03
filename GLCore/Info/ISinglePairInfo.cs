namespace GLCore
{
    /// <summary>
    /// 標示點資訊介面
    /// </summary>
    public interface ISinglePairInfo : IGLCore
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
        /// X 座標
        /// </summary>
        int X { get; set; }

        /// <summary>
        /// Y 座標
        /// </summary>
        int Y { get; set; }
    }
}