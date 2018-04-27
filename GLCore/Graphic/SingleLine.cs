using Geometry;
using GLStyle;
using SharpGL;
using System;

namespace GLCore
{
    /// <summary>
    /// 標示線
    /// </summary>
    [Serializable]
    public class SingleLine : ISingleLine
    {
        /// <summary>
        /// 由兩點座標建構標示線
        /// </summary>
        public SingleLine(string styleName, int x0, int y0, int x1, int y1)
        {
            StyleName = styleName;
            Geometry = new Line(x0, y0, x1, y1);
        }

        /// <summary>
        /// 由兩點座標建構標示線
        /// </summary>
        public SingleLine(string styleName, double x0, double y0, double x1, double y1) : this(styleName, (int)x0, (int)y0, (int)x1, (int)y1)
        {
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public SingleLine(ISingleLine line) : this(line.StyleName, line.Geometry.Begin.X, line.Geometry.Begin.Y, line.Geometry.End.X, line.Geometry.End.Y)
        {
        }

        /// <summary>
        /// 由兩點座標建構標示線
        /// </summary>
        public SingleLine(string styleName, IPair beg, IPair end) : this(styleName, beg.X, beg.Y, end.X, end.Y)
        {
        }

        /// <summary>
        /// 建構 (0,0,0,0) 標示線
        /// </summary>
        public SingleLine(string styleName)
        {
            StyleName = styleName;
            Geometry = new Line();
        }

        /// <summary>
        /// 是否可拖曳
        /// </summary>
        public bool CanDrag { get; private set; } = true;

        /// <summary>
        /// 幾何座標
        /// </summary>
        public ILine Geometry { get; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

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
            if (Style == null) return;
            if (Style.Width > 0) gl.LineWidth(Style.Width);
            gl.Color(Style.BackgroundColor.GetFloats());
            gl.BeginStippleLine(Style.Pattern);
            {
                gl.Begin(OpenGL.GL_LINES);
                {
                    gl.Vertex(Geometry.Begin.X, Geometry.Begin.Y, Style.Layer);
                    gl.Vertex(Geometry.End.X, Geometry.End.Y, Style.Layer);
                }
                gl.End();
            }
            gl.EndStippleLine();
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
                    gl.Vertex(Geometry.Begin.X, Geometry.Begin.Y, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.End.X, Geometry.Begin.Y, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.End.X, Geometry.End.Y, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.Begin.X, Geometry.End.Y, StyleManager.BoundStyle.Layer);
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
                    int cx = (Geometry.Begin.X + Geometry.End.X) / 2;
                    int cy = (Geometry.Begin.Y + Geometry.End.Y) / 2;

                    Geometry.Begin.X += (x - cx);
                    Geometry.Begin.Y += (y - cy);
                    Geometry.End.X += (x - cx);
                    Geometry.End.Y += (y - cy);
                    return true;

                case EMoveType.Begin:
                    Geometry.Begin.X = x;
                    Geometry.Begin.Y = y;
                    return true;

                case EMoveType.End:
                    Geometry.End.X = x;
                    Geometry.End.Y = y;
                    return true;

                default:
                    return false;
            }
        }
    }
}