using System;
using static Geometry.StandardOperate;

namespace Geometry
{
    /// <summary>
    /// 提供介於[0,360)之間的角度
    /// </summary>
    [Serializable]
    public class Angle : IAngle
    {
        private double mValue = 0;

        /// <summary>
        /// 建立 0 度角
        /// </summary>
        public Angle()
        {
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public Angle(IAngle angle)
        {
            Theta = angle.Theta;
        }

        /// <summary>
        /// 將浮點數角度轉為 <see cref="Angle"/>
        /// </summary>
        public Angle(double angle)
        {
            Theta = angle;
        }

        /// <summary>
        /// 角度值
        /// </summary>
        public double Theta { get { return mValue; } set { mValue = Normalization(value); } }

        /// <summary>
        /// 比較是否相等
        /// </summary>
        public bool Equals(IAngle other)
        {
            return ((int)(1000 * Theta)) == ((int)(1000 * other.Theta));
        }

        /// <summary>
        /// 回傳湊雜碼 (int)(1000 * Theta)
        /// </summary>
        public override int GetHashCode()
        {
            return (int)(1000 * Theta);
        }

        /// <summary>
        /// 回傳角度至小數點第二位，例如：90.00
        /// </summary>
        public override string ToString()
        {
            return Theta.ToString("F2");
        }
    }
}