using System;

namespace Geometry
{
    /// <summary>
    /// 具有方向的點
    /// </summary>
    public interface ITowardPair : IGeometry, IEquatable<ITowardPair>
    {
        /// <summary>
        /// 座標點
        /// </summary>
        IPair Position { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        IAngle Toward { get; set; }

        /// <summary>
        /// 回傳座標，例如：-100,100,90.00
        /// </summary>
        string ToString();
    }
}