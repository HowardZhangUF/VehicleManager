using Geometry;
using GLStyle;
using SharpGL;
using System;

namespace GLCore
{
    /// <summary>
    /// 標示物介面
    /// </summary>
    public interface ISingle : IGLCore, IDragable
    {
        /// <summary>
        /// 名稱
        /// </summary>
        string Name { get; set; }

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

        /// <summary>
        /// 畫邊界
        /// </summary>
        void DrawBound(OpenGL gl);

        /// <summary>
        /// 顯示文字
        /// </summary>
        void DrawText(OpenGL gl, Func<IPair, IPair> convert);
    }

    /// <summary>
    /// 標示物介面
    /// </summary>
    public interface ISingle<TGeometry, TStyle> : IGLCore, ISingle where TGeometry : IGeometry where TStyle : IStyle
    {
        /// <summary>
        /// 幾何座標
        /// </summary>
        TGeometry Geometry { get; }

        /// <summary>
        /// 樣式
        /// </summary>
        TStyle Style { get; }
    }
}