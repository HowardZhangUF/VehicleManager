namespace GLCore
{
    /// <summary>
    /// 標示物資訊介面
    /// </summary>
    public interface ISingleTowardPairInfo : IGLCore
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
        /// 方向
        /// </summary>
        double Toward { get; set; }

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