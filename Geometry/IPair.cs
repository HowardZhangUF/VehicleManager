using System;

namespace Geometry
{
    /// <summary>
    /// 座標
    /// </summary>
    public interface IPair : IGeometry, IEquatable<IPair>
    {
        /// <summary>
        /// X 座標
        /// </summary>
        int X { get; set; }

        /// <summary>
        /// Y 座標
        /// </summary>
        int Y { get; set; }

        /// <summary>
        /// 回傳座標，例如：-100,100
        /// </summary>
        string ToString();
    }
}