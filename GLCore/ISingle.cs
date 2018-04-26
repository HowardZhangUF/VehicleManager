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

    /// <summary>
    /// 標示面介面
    /// </summary>
    public interface ISingleArea : IGLCore, ISingle<IArea, IAreaStyle>
    {
    }

    /// <summary>
    /// 標示線介面
    /// </summary>
    public interface ISingleLine : IGLCore, ISingle<ILine, ILineStyle>
    {
    }

    /// <summary>
    /// 標示點介面
    /// </summary>
    public interface ISinglePair : IGLCore, ISingle<IPair, IPairStyle>
    {
    }

    /// <summary>
    /// 標示物介面
    /// </summary>
    public interface ISingleTowardPair : IGLCore, ISingle<ITowardPair, ITowardPairStyle>
    {
    }

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
        public bool Transparent { get { return Style.BackgroundColor.A != 255; } }

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
        public bool Transparent { get { return Style.BackgroundColor.A != 255; } }

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