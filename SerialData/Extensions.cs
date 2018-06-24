using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

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

        /// <summary>
        /// 使用 <see cref="DisplayNameAttribute"/> 組合資料
        /// </summary>
        public static string ToString<T>(this T obj, string separator)
        {
            var properties = typeof(T).GetProperties();
            var strList = properties
                    .Where((p) => p.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute != null)
                    .Select((p) => $"{(p.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute).DisplayName}: {p.GetValue(obj).ToString()}");
            return string.Join("|", strList);
        }
    }
}
