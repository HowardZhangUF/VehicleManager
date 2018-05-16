using Geometry;
using GLStyle;
using SharpGL;
using System;

namespace GLCore
{
    /// <summary>
    /// 標示物
    /// </summary>
    [Serializable]
    public class SingleTowardPair : ISingleTowardPair
    {
        /// <summary>
        /// 建構 (0,0,0) 標示物
        /// </summary>
        public SingleTowardPair(string styleName)
        {
            StyleName = styleName;
            Geometry = new TowardPair();
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public SingleTowardPair(string styleName, int x, int y, double toward)
        {
            StyleName = styleName;
            Geometry = new TowardPair(x, y, toward);
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public SingleTowardPair(string styleName, double x, double y, double toward) : this(styleName, (int)x, (int)y, toward)
        {
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public SingleTowardPair(ISingleTowardPair towardPair) : this(towardPair.StyleName, towardPair.Geometry.Position.X, towardPair.Geometry.Position.Y, towardPair.Geometry.Toward.Theta)
        {
        }


        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public SingleTowardPair(string styleName, ITowardPair towardPair) : this(styleName, towardPair.Position.X, towardPair.Position.Y, towardPair.Toward)
        {
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public SingleTowardPair(string styleName, IPair position, double toward) : this(styleName, position.X, position.Y, toward)
        {
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public SingleTowardPair(string styleName, IPair position, IAngle toward) : this(styleName, position.X, position.Y, toward.Theta)
        {
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public SingleTowardPair(string styleName, int x, int y, IAngle toward) : this(styleName, x, y, toward.Theta)
        {
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public SingleTowardPair(string styleNamedouble, double x, double y, IAngle toward) : this(styleNamedouble, (int)x, (int)y, toward.Theta)
        {
        }

        /// <summary>
        /// 是否可拖曳
        /// </summary>
        public bool CanDrag { get; private set; } = true;

        /// <summary>
        /// 幾何座標
        /// </summary>
        public ITowardPair Geometry { get; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 樣式
        /// </summary>
        public ITowardPairStyle Style { get { return StyleManager.GetStyle(StyleName) as ITowardPairStyle; } }

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
            if (Style.LineLength != 0)
                DrawWithLineLength(gl);
            else
                DrawWithoutLineLength(gl);
        }

        /// <summary>
        /// 畫邊界
        /// </summary>
        public void DrawBound(OpenGL gl)
        {
            if (Style.LineLength != 0)
                DrawBoundWithLineLength(gl);
            else
                DrawBoundWithoutLineLength(gl);
        }

        /// <summary>
        /// 顯示文字
        /// </summary>
        public void DrawText(OpenGL gl, Func<IPair, IPair> convert)
        {
            gl.DrawText(Name, Geometry.Position, convert);
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
                    Geometry.Position.X = x;
                    Geometry.Position.Y = y;
                    return true;

                case EMoveType.Toward:

                    Geometry.Toward.Theta = Math.Atan2(y - Geometry.Position.Y, x - Geometry.Position.X) * 180 / Math.PI;
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 畫邊界
        /// </summary>
        private void DrawBoundWithLineLength(OpenGL gl)
        {
            gl.Color(StyleManager.BoundStyle.BackgroundColor.GetFloats());
            gl.LineWidth(StyleManager.BoundStyle.Width);
            gl.BeginStippleLine(StyleManager.BoundStyle.Pattern);
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
                {
                    gl.Vertex(Geometry.Position.X - Style.Width / 2, Geometry.Position.Y - Style.Height / 2, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.Position.X + Style.Width / 2, Geometry.Position.Y - Style.Height / 2, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.Position.X + Style.Width / 2, Geometry.Position.Y + Style.Height / 2, StyleManager.BoundStyle.Layer);
                    gl.Vertex(Geometry.Position.X - Style.Width / 2, Geometry.Position.Y + Style.Height / 2, StyleManager.BoundStyle.Layer);
                }
                gl.End();
            }
            gl.EndStippleLine();
        }

        /// <summary>
        /// 畫邊界
        /// </summary>
        private void DrawBoundWithoutLineLength(OpenGL gl)
        {
            gl.Color(StyleManager.BoundStyle.BackgroundColor.GetFloats());
            gl.LineWidth(StyleManager.BoundStyle.Width);
            gl.PushMatrix();
            {
                gl.Translate(Geometry.Position.X, Geometry.Position.Y, 0);
                gl.Rotate(Geometry.Toward.Theta, 0, 0, 1);
                gl.BeginStippleLine(StyleManager.BoundStyle.Pattern);
                {
                    gl.Begin(OpenGL.GL_LINE_LOOP);
                    {
                        gl.Vertex(-Style.Width / 2, -Style.Height / 2, StyleManager.BoundStyle.Layer);
                        gl.Vertex(+Style.Width / 2, -Style.Height / 2, StyleManager.BoundStyle.Layer);
                        gl.Vertex(+Style.Width / 2, +Style.Height / 2, StyleManager.BoundStyle.Layer);
                        gl.Vertex(-Style.Width / 2, +Style.Height / 2, StyleManager.BoundStyle.Layer);
                    }
                    gl.End();
                }
                gl.EndStippleLine();
            }
            gl.PopMatrix();
        }

        private void DrawWithLineLength(OpenGL gl)
        {
            gl.Color(Style.BackgroundColor.GetFloats());
            gl.PushMatrix();
            {
                gl.Translate(Geometry.Position.X, Geometry.Position.Y, Style.Layer);

                // 畫本體
                gl.TextureBmp(Style.ImagePath, Style.Width, Style.Height, Style.BackgroundColor);

                // 畫線
                gl.Translate(0, 0, 1);
                if (Style.LineWidth > 0) gl.LineWidth(Style.LineWidth);
                gl.Color(Style.LineColor.GetFloats());
                gl.BeginStippleLine(Style.LinePattern);
                {
                    gl.DrawLine(Geometry.Toward, Style.LineLength);
                }
                gl.EndStippleLine();
            }
            gl.PopMatrix();
        }

        private void DrawWithoutLineLength(OpenGL gl)
        {
            gl.Color(Style.BackgroundColor.GetFloats());
            gl.PushMatrix();
            {
                gl.Translate(Geometry.Position.X, Geometry.Position.Y, Style.Layer);
                gl.Rotate(Geometry.Toward.Theta, 0, 0, 1);

                // 畫本體
                gl.TextureBmp(Style.ImagePath, Style.Width, Style.Height, Style.BackgroundColor);
            }
            gl.PopMatrix();
        }
    }
}