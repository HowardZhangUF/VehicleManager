namespace GLCore
{
    /// <summary>
    /// 標示線資訊
    /// </summary>
    public class SingleLineInfo : ISingleLineInfo
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
        /// 起點 X 座標
        /// </summary>
        public int X0 { get; set; }

        /// <summary>
        /// 起點 Y 座標
        /// </summary>
        public int Y0 { get; set; }

        /// <summary>
        /// 終點 X 座標
        /// </summary>
        public int X1 { get; set; }

        /// <summary>
        /// 終點 Y 座標
        /// </summary>
        public int Y1 { get; set; }
    }
}