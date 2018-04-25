using Geometry;
using System.Collections.Generic;
using ThreadSafety;

namespace GeometryCollection
{
    /// <summary>
    /// 具執行緒安全的線集合
    /// </summary>
    public class LineColletion : IColletion<ILine>
    {
        /// <summary>
        /// 資料
        /// </summary>
        public ISafty<List<ILine>> Data { get; } = new Safty<List<ILine>>();
    }
}
