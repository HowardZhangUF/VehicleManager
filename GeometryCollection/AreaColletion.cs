using Geometry;
using System.Collections.Generic;
using ThreadSafety;

namespace GeometryCollection
{
    /// <summary>
    /// 具執行緒安全的面集合
    /// </summary>
    public class AreaColletion : IColletion<IArea>
    {
        /// <summary>
        /// 資料
        /// </summary>
        public ISafty<List<IArea>> Data { get; } = new Safty<List<IArea>>();
    }
}
