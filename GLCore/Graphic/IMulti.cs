using Geometry;
using GLStyle;
using SharpGL;
using System.Collections.Generic;
using ThreadSafety;

namespace GLCore
{
    /// <summary>
    /// 複合物介面
    /// </summary>
    public interface IMulti : IGLCore
    {
        /// <summary>
        /// 樣式名稱
        /// </summary>
        string StyleName { get; set; }

        /// <summary>
        /// 透明的
        /// </summary>
        bool Transparent { get; }

        /// <summary>
        /// 繪圖
        /// </summary>
        void Draw(OpenGL gl);
    }

    /// <summary>
    /// 複合物介面
    /// </summary>
    public interface IMulti<TGeometry> : IGLCore, IMulti where TGeometry : IGeometry
    {
        /// <summary>
        /// 幾何座標集合
        /// </summary>
        ISafty<List<TGeometry>> Geometry { get; }
    }

    /// <summary>
    /// 複合物介面
    /// </summary>
    public interface IMulti<TGeometry, TStyle> : IGLCore, IMulti<TGeometry>, IMulti where TGeometry : IGeometry where TStyle : IStyle
    {
        /// <summary>
        /// 樣式
        /// </summary>
        TStyle Style { get; }
    }
}