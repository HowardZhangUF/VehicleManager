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

    /// <summary>
    /// 標示點資訊
    /// </summary>
    public class SinglePairInfo : ISinglePairInfo
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
        /// X 座標
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y 座標
        /// </summary>
        public int Y { get; set; }
    }

    /// <summary>
    /// 標示物資訊介面
    /// </summary>
    public interface ISingleTowerPairInfo : IGLCore
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