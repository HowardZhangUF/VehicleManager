using System;

namespace Geometry
{
    /// <summary>
    /// 提供介於[0,360)之間的角度
    /// </summary>
    public interface IAngle : IGeometry, IEquatable<IAngle>
    {
        /// <summary>
        /// 角度值
        /// </summary>
        double Theta { get; set; }

        /// <summary>
        /// 回傳角度至小數點第二位，例如：90.00
        /// </summary>
        string ToString();
    }
}