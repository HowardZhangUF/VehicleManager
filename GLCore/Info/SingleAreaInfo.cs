namespace GLCore
{
    /// <summary>
    /// 標示面資訊
    /// </summary>
    public class SingleAreaInfo : ISingleAreaInfo
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
        /// 最小 X 座標
        /// </summary>
        public int MinX { get; set; }

        /// <summary>
        /// 最小 Y 座標
        /// </summary>
        public int MinY { get; set; }

        /// <summary>
        /// 最大 X 座標
        /// </summary>
        public int MaxX { get; set; }

        /// <summary>
        /// 最大 Y 座標
        /// </summary>
        public int MaxY { get; set; }
    }
}