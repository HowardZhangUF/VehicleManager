using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadSafety;

namespace GeometryCollection
{
    /// <summary>
    /// 具執行緒安全的幾何集合介面
    /// </summary>
    public interface IColletion<TGeometry> : IGeometryCollection where TGeometry : IGeometry
    {
        /// <summary>
        /// 資料
        /// </summary>
        ISafty<List<TGeometry>> Data { get; }
    }
}
