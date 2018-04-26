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
    public interface IMulti<TGeometry, TStyle> : IGLCore, IMulti where TGeometry : IGeometry where TStyle : IStyle
    {
        /// <summary>
        /// 幾何座標集合
        /// </summary>
        ISafty<List<TGeometry>> Geometry { get; }

        /// <summary>
        /// 樣式
        /// </summary>
        TStyle Style { get; }
    }

    /// <summary>
    /// 複合面
    /// </summary>
    public interface IMultiArea : IGLCore, IMulti<IArea, IAreaStyle>
    {
    }

    /// <summary>
    /// 複合點
    /// </summary>
    public interface IMultiPair : IGLCore, IMulti<IPair, IPairStyle>
    {
    }

    /// <summary>
    /// 複合線
    /// </summary>
    public interface IMultiStripLine : IGLCore, IMulti<IPair, ILineStyle>
    {
    }

    /// <summary>
    /// 複合面
    /// </summary>
    public class MultiArea : IMultiArea
    {
        /// <summary>
        /// 建立一個空物件集合
        /// </summary>
        public MultiArea(string styleName) { StyleName = styleName; }

        /// <summary>
        /// 初始化物件集合
        /// </summary>
        public MultiArea(string styleName, IEnumerable<IArea> area)
        {
            StyleName = styleName;
            Geometry.SaftyEdit(true, list => list.AddRange(area));
        }

        /// <summary>
        /// 幾何座標集合
        /// </summary>
        public ISafty<List<IArea>> Geometry { get; } = new Safty<List<IArea>>();

        /// <summary>
        /// 樣式
        /// </summary>
        public IAreaStyle Style { get { return StyleManager.GetStyle(StyleName) as IAreaStyle; } }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleName { get; set; }

        /// <summary>
        /// 透明的
        /// </summary>
        public bool Transparent { get { return Style.BackgroundColor.A != 255; } }

        /// <summary>
        /// 繪圖
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Color(Style.BackgroundColor.GetFloats());
            gl.Begin(OpenGL.GL_QUADS);
            {
                Geometry.SaftyEdit(false, list => list.ForEach(area =>
                {
                    gl.Vertex(area.Min.X, area.Min.Y, Style.Layer);
                    gl.Vertex(area.Max.X, area.Min.Y, Style.Layer);
                    gl.Vertex(area.Max.X, area.Max.Y, Style.Layer);
                    gl.Vertex(area.Min.X, area.Max.Y, Style.Layer);
                }));
            }
            gl.End();
        }
    }

    /// <summary>
    /// 複合點
    /// </summary>
    public class MultiPair : IMultiPair
    {
        /// <summary>
        /// 建立一個空物件集合
        /// </summary>
        public MultiPair(string styleName) { StyleName = styleName; }

        /// <summary>
        /// 初始化物件集合
        /// </summary>
        public MultiPair(string styleName, IEnumerable<IPair> pair)
        {
            StyleName = styleName;
            Geometry.SaftyEdit(true, list => list.AddRange(pair));
        }

        /// <summary>
        /// 幾何座標集合
        /// </summary>
        public ISafty<List<IPair>> Geometry { get; } = new Safty<List<IPair>>();

        /// <summary>
        /// 樣式
        /// </summary>
        public IPairStyle Style { get { return StyleManager.GetStyle(StyleName) as IPairStyle; } }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleName { get; set; }

        /// <summary>
        /// 透明的
        /// </summary>
        public bool Transparent { get { return Style.BackgroundColor.A != 255; } }

        /// <summary>
        /// 繪圖
        /// </summary>
        public void Draw(OpenGL gl)
        {
            if (Style.Size > 0) gl.PointSize(Style.Size);
            gl.Color(Style.BackgroundColor.GetFloats());
            gl.Begin(OpenGL.GL_POINTS);
            {
                Geometry.SaftyEdit(false, list => list.ForEach(pair => gl.Vertex(pair.X, pair.Y, Style.Layer)));
            }
            gl.End();
        }
    }

    /// <summary>
    /// 複合線
    /// </summary>
    public class MultiStripLine : IMultiStripLine
    {
        /// <summary>
        /// 建立一個空物件集合
        /// </summary>
        public MultiStripLine(string styleName) { StyleName = styleName; }

        /// <summary>
        /// 初始化物件集合
        /// </summary>
        public MultiStripLine(string styleName, IEnumerable<IPair> pair)
        {
            StyleName = styleName;
            Geometry.SaftyEdit(true, list => list.AddRange(pair));
        }

        /// <summary>
        /// 幾何座標集合
        /// </summary>
        public ISafty<List<IPair>> Geometry { get; } = new Safty<List<IPair>>();

        /// <summary>
        /// 樣式
        /// </summary>
        public ILineStyle Style { get { return StyleManager.GetStyle(StyleName) as ILineStyle; } }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleName { get; set; }

        /// <summary>
        /// 透明的
        /// </summary>
        public bool Transparent { get { return Style.BackgroundColor.A != 255; } }

        /// <summary>
        /// 繪圖
        /// </summary>
        public void Draw(OpenGL gl)
        {
            if (Style.Width > 0) gl.LineWidth(Style.Width);
            gl.Color(Style.BackgroundColor.GetFloats());
            gl.BeginStippleLine(Style.Pattern);
            {
                gl.Begin(OpenGL.GL_LINE_STRIP);
                {
                    Geometry.SaftyEdit(false, list => list.ForEach(pair => gl.Vertex(pair.X, pair.Y, Style.Layer)));
                }
                gl.End();
            }
            gl.EndStippleLine();
        }
    }
}