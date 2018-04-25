using Geometry;
using System.Collections.Generic;
using ThreadSafety;

namespace GeometryCollection
{
    /// <summary>
    /// 具執行緒安全的點集合
    /// </summary>
    public class PairColletion : IColletion<IPair>
    {
        /// <summary>
        /// 資料
        /// </summary>
        public ISafty<List<IPair>> Data { get; } = new Safty<List<IPair>>();
    }
}
