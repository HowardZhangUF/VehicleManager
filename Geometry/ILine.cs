using System;

namespace Geometry
{
    /// <summary>
    /// 線段
    /// </summary>
    public interface ILine : IGeometry, IEquatable<ILine>
    {
        /// <summary>
        /// 起點座標
        /// </summary>
        IPair Begin { get; set; }

        /// <summary>
        /// 終點座標
        /// </summary>
        IPair End { get; set; }

        /// <summary>
        /// 回傳座標，例如：-100,100,0,200(前面兩個數值代表起始座標，後面代表終點座標)
        /// </summary>
        string ToString();
    }
}