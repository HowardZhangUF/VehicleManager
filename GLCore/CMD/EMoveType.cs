namespace GLCore
{
    /// <summary>
    /// 移動方式
    /// </summary>
    public enum EMoveType
    {
        /// <summary>
        /// 不移動
        /// </summary>
        Stop,

        /// <summary>
        /// 移動中心點
        /// </summary>
        Center,

        /// <summary>
        /// 移動最大值座標
        /// </summary>
        Max,

        /// <summary>
        /// 移動最小值座標
        /// </summary>
        Min,

        /// <summary>
        /// 移動起點座標
        /// </summary>
        Begin,

        /// <summary>
        /// 移動終點座標
        /// </summary>
        End,

        /// <summary>
        /// 移動方向角
        /// </summary>
        Toward,
    }
}