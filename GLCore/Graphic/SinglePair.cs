using Geometry;
using GLStyle;
using SharpGL;
using System;

namespace GLCore
{
    /// <summary>
    /// 標示點
    /// </summary>
    [Serializable]
    public class SinglePair : ISinglePair
    {
        /// <summary>
        /// 由座標建構標示點
        /// </summary>
        public SinglePair(string styleName, int x, int y)
        {
            StyleName = styleName;
            Geometry = new Pair(x, y);
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public SinglePair(ISinglePair pair) : this(pair.StyleName, pair.Geometry.X, pair.Geometry.Y)
        {
        }

        /// <summary>
        /// 建構 (0,0) 標示點
        /// </summary>
        public SinglePair(string styleName)
        {
            StyleName = styleName;
            Geometry = new Pair();
        }

        /// <summary>
        /// 由座標建構標示點
        /// </summary>
        public SinglePair(string styleName, double x, double y) : this(styleName, (int)x, (int)y)
        {
        }

        /// <summary>
        /// 是否可拖曳
        /// </summary>
        public bool CanDrag { get; private set; } = true;

        /// <summary>
        /// 幾何座標
        /// </summary>
        public IPair Geometry { get; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

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
            if (Style == null) return;
            if (Style.Size > 0) gl.PointSize(Style.Size);
            gl.Color(Style.BackgroundColor.GetFloats());
            gl.Begin(OpenGL.GL_POINTS);
            {
                gl.Vertex(Geometry.X, Geometry.Y, Style.Layer);
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
                    gl.Vertex(Geometry.X - Style.Size / 2, Geometry.Y - Style.Size / 2, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.X + Style.Size / 2, Geometry.Y - Style.Size / 2, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.X + Style.Size / 2, Geometry.Y + Style.Size / 2, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.X - Style.Size / 2, Geometry.Y + Style.Size / 2, StyleManager.BoundStyle.Layer);
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
            gl.DrawText(Name, Geometry, convert);
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
                    Geometry.X = x;
                    Geometry.Y = y;
                    return true;

                default:
                    return false;
            }
        }
    }
}