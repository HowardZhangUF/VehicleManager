using System;

namespace Geometry
{
    /// <summary>
    /// 面，自動調整 Max 與 Min 使得 Max 總是維持在左上角，Min 總是在右下角
    /// </summary>
    public interface IArea : IGeometry, IEquatable<IArea>
    {
        /// <summary>
        /// 最大值座標
        /// </summary>
        IPair Max { get; set; }

        /// <summary>
        /// 最小值座標
        /// </summary>
        IPair Min { get; set; }

        /// <summary>
        /// 回傳座標，例如：-100,100,0,200(前面兩個數值代表最小值座標，後面代表最大值座標)
        /// </summary>
        string ToString();
    }
}