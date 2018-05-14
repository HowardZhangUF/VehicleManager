using Geometry;
using GLStyle;
using SharpGL;
using System;

namespace GLCore
{
    /// <summary>
    /// AGV
    /// </summary>
    public class AGV : IAGV
    {
        /// <summary>
        /// 基底類別
        /// </summary>
        private SingleTowardPair @base = null;

        /// <summary>
        /// 建構 (0,0,0) 標示物
        /// </summary>
        public AGV(string styleName)
        {
            @base = new SingleTowardPair(styleName);
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public AGV(string styleName, int x, int y, double toward)
        {
            @base = new SingleTowardPair(styleName, x, y, toward);
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public AGV(string styleName, double x, double y, double toward) : this(styleName, (int)x, (int)y, toward)
        {
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public AGV(ISingleTowardPair towardPair) : this(towardPair.StyleName, towardPair.Geometry.Position.X, towardPair.Geometry.Position.Y, towardPair.Geometry.Toward.Theta)
        {
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public AGV(string styleName, IPair position, double toward) : this(styleName, position.X, position.Y, toward)
        {
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public AGV(string styleName, IPair position, IAngle toward) : this(styleName, position.X, position.Y, toward.Theta)
        {
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public AGV(string styleName, int x, int y, IAngle toward) : this(styleName, x, y, toward.Theta)
        {
        }

        /// <summary>
        /// 建構具有方向的標示物
        /// </summary>
        public AGV(string styleNamedouble, double x, double y, IAngle toward) : this(styleNamedouble, (int)x, (int)y, toward.Theta)
        {
        }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get { return @base.Name; } set { @base.Name = value; } }

        /// <summary>
        /// 樣式
        /// </summary>
        public IAGVStyle Style { get { return StyleManager.GetStyle(StyleName) as IAGVStyle; } }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleName { get { return @base.StyleName; } set { @base.StyleName = value; } }

        /// <summary>
        /// 是否可拖曳
        /// </summary>
        public bool CanDrag => @base.CanDrag;

        /// <summary>
        /// 幾何座標
        /// </summary>
        public ITowardPair Geometry => @base.Geometry;

        /// <summary>
        /// 透明的
        /// </summary>
        public bool Transparent => @base.Transparent;

        /// <summary>
        /// 繪圖
        /// </summary>
        public void Draw(OpenGL gl)
        {
            // 畫車子
            @base.Draw(gl);

            // 畫安全框
            gl.Color(Style.SafetyColor.GetFloats());
            gl.LineWidth(Style.SafetyLineWidth);
            gl.PushMatrix();
            {
                gl.Translate(Geometry.Position.X, Geometry.Position.Y, 0);
                gl.Rotate(Geometry.Toward.Theta, 0, 0, 1);
                gl.BeginStippleLine(Style.SafetyLinePattern);
                {
                    gl.Begin(OpenGL.GL_LINE_LOOP);
                    {
                        gl.Vertex(-Style.SafetyWidth / 2, -Style.SafetyHeight / 2, Style.Layer);
                        gl.Vertex(+Style.SafetyWidth / 2, -Style.SafetyHeight / 2, Style.Layer);
                        gl.Vertex(+Style.SafetyWidth / 2, +Style.SafetyHeight / 2, Style.Layer);
                        gl.Vertex(-Style.SafetyWidth / 2, +Style.SafetyHeight / 2, Style.Layer);
                    }
                    gl.End();
                }
                gl.EndStippleLine();
            }
            gl.PopMatrix();
        }

        /// <summary>
        /// 畫邊界
        /// </summary>
        public void DrawBound(OpenGL gl)
        {
            @base.DrawBound(gl);
        }

        /// <summary>
        /// 顯示文字
        /// </summary>
        public void DrawText(OpenGL gl, Func<IPair, IPair> convert)
        {
            @base.DrawText(gl, convert);
        }

        /// <summary>
        /// 移動物件，若無法移動則回傳 false
        /// </summary>
        /// <param name="ctrl">控制點</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public bool Move(EMoveType ctrl, int x, int y)
        {
            return @base.Move(ctrl, x, y);
        }
    }
}