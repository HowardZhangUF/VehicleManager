using System;

namespace SerialData
{
    /// <summary>
    /// 自訂擴充方法
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 傳回 minValue 和 maxValue 之間的隨機浮點數。
        /// </summary>
        public static double NextDouble(this Random rnd, int minValue, int maxValue)
        {
            return rnd.Next(minValue * 1000, maxValue * 1000) / 1000.0;
        }
    }
}
