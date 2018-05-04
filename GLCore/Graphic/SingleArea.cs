using Geometry;
using GLStyle;
using SharpGL;
using System;

namespace GLCore
{
    /// <summary>
    /// 標示面
    /// </summary>
    [Serializable]
    public class SingleArea : ISingleArea
    {
        /// <summary>
        /// 建構 (0,0,0,0) 的標示面
        /// </summary>
        public SingleArea(string styleName)
        {
            StyleName = styleName;
            Geometry = new Area();
        }

        /// <summary>
        /// 由兩點座標建構標示面
        /// </summary>
        public SingleArea(string styleName, IPair min, IPair max)
        {
            StyleName = styleName;
            Geometry = new Area(min, max);
        }

        /// <summary>
        /// 由中心座標及長寬建構標示面
        /// </summary>
        public SingleArea(string styleName, IPair center, int width, int height)
        {
            StyleName = styleName;
            Geometry = new Area(center, width, height);
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public SingleArea(ISingleArea area)
        {
            StyleName = area.StyleName;
            Geometry = new Area(area.Geometry);
        }

        /// <summary>
        /// 由兩點座標建構標示面
        /// </summary>
        public SingleArea(string styleName, double x0, double y0, double x1, double y1)
        {
            StyleName = styleName;
            Geometry = new Area(x0, y0, x1, y1);
        }

        /// <summary>
        /// 由兩點座標建構標示面
        /// </summary>
        public SingleArea(string styleName, int x0, int y0, int x1, int y1)
        {
            StyleName = styleName;
            Geometry = new Area(x0, y0, x1, y1);
        }

        /// <summary>
        /// 是否可拖曳
        /// </summary>
        public bool CanDrag { get; private set; } = true;

        /// <summary>
        /// 幾何座標
        /// </summary>
        public IArea Geometry { get; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

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
        public bool Transparent { get { return Style?.BackgroundColor.A != 255; } }

        /// <summary>
        /// 繪圖
        /// </summary>
        public void Draw(OpenGL gl)
        {
            if (Style == null) return;
            gl.Color(Style.BackgroundColor.GetFloats());
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(Geometry.Min.X, Geometry.Min.Y, Style.Layer);
                gl.Vertex(Geometry.Max.X, Geometry.Min.Y, Style.Layer);
                gl.Vertex(Geometry.Max.X, Geometry.Max.Y, Style.Layer);
                gl.Vertex(Geometry.Min.X, Geometry.Max.Y, Style.Layer);
            }
            gl.End();
        }

        /// <summary>
        /// 畫邊界
        /// </summary>
        public void DrawBound(OpenGL gl)
        {
            gl.Color(StyleManager.BoundStyle.BackgroundColor.GetFloats());
            gl.LineWidth(StyleManager.BoundStyle.Width);
            gl.BeginStippleLine(StyleManager.BoundStyle.Pattern);
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
                {
                    gl.Vertex(Geometry.Min.X, Geometry.Min.Y, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.Max.X, Geometry.Min.Y, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.Max.X, Geometry.Max.Y, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.Min.X, Geometry.Max.Y, StyleManager.BoundStyle.Layer);
                }
                gl.End();
            }
            gl.EndStippleLine();
        }

        /// <summary>
        /// 顯示文字
        /// </summary>
        public void DrawText(OpenGL gl, Func<IPair, IPair> convert)
        {
            gl.DrawText(Name, Geometry.Center(), convert);
        }

        /// <summary>
        /// 移動物件，若無法移動則回傳 false
        /// </summary>
        /// <param name="ctrl">控制點</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public bool Move(EMoveType ctrl, int x, int y)
        {
            if (!CanDrag) return false;

            switch (ctrl)
            {
                case EMoveType.Center:
                    int w = Geometry.Max.X - Geometry.Min.X;
                    int h = Geometry.Max.Y - Geometry.Min.Y;
                    Geometry.Set(x - w / 2, y - h / 2, x + w / 2, y + h / 2);
                    return true;

                case EMoveType.Min:
                    Geometry.Set(x, y, Geometry.Max.X, Geometry.Max.Y);
                    return true;

                case EMoveType.Max:
                    Geometry.Set(Geometry.Min.X, Geometry.Min.Y, x, y);
                    return true;

                default:
                    return false;
            }
        }
    }
}