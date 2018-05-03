namespace GLCore
{
    /// <summary>
    /// 標示物資訊
    /// </summary>
    public class SingleTowerPairInfo : ISingleTowerPairInfo
    {
        /// <summary>
        /// 序號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleName { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public double Toward { get; set; }

        /// <summary>
        /// X 座標
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y 座標
        /// </summary>
        public int Y { get; set; }
    }
}