using Geometry;

namespace MapReader
{
    /// <summary>
    /// 目標點
    /// </summary>
    public class Goal : IMapReader
    {
        /// <summary>
        /// 目標點名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 幾何座標
        /// </summary>
        public ITowardPair TowardPair { get; set; }

        /// <summary>
        /// 種類
        /// </summary>
        public string TypeName { get; set; }
    }
}
