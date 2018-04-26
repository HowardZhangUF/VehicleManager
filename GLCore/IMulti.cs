using Geometry;
using GLStyle;
using SharpGL;
using System;
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
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

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
        /// 最後一次產生頂點陣列的時間
        /// </summary>
        private DateTime LastGenVertexArrayTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 頂點陣列
        /// </summary>
        private int[] VertexArray { get; set; } = null;

        /// <summary>
        /// 繪圖
        /// </summary>
        public void Draw(OpenGL gl)
        {
            lock (key)
            {
                GenVertexArray();

                if (VertexArray == null) return;

                gl.Color(Style.BackgroundColor.GetFloats());

                gl.DrawArrays(OpenGL.GL_QUADS, VertexArray.Length / 2, VertexArray);
            }
        }

        /// <summary>
        /// 產生頂點陣列，加速繪圖
        /// </summary>
        private void GenVertexArray()
        {
            lock (key)
            {
                // 若已經產生頂點陣列，則返回
                if (VertexArray != null && LastGenVertexArrayTime >= Geometry.LastEditTime) return;

                LastGenVertexArrayTime = Geometry.LastEditTime;

                VertexArray = new int[Geometry.SaftyEdit(list => list.Count * 8)];
                int index = 0;

                Geometry.SaftyEdit(false, list => list.ForEach(area =>
                {
                    VertexArray[index] = area.Min.X;
                    index++;
                    VertexArray[index] = area.Min.Y;
                    index++;
                    VertexArray[index] = area.Max.X;
                    index++;
                    VertexArray[index] = area.Min.Y;
                    index++;
                    VertexArray[index] = area.Max.X;
                    index++;
                    VertexArray[index] = area.Max.Y;
                    index++;
                    VertexArray[index] = area.Min.X;
                    index++;
                    VertexArray[index] = area.Max.Y;
                    index++;
                }));
            }
        }
    }

    /// <summary>
    /// 複合點
    /// </summary>
    public class MultiPair : IMultiPair
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

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
        /// 最後一次產生頂點陣列的時間
        /// </summary>
        private DateTime LastGenVertexArrayTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 頂點陣列
        /// </summary>
        private int[] VertexArray { get; set; } = null;

        /// <summary>
        /// 繪圖
        /// </summary>
        public void Draw(OpenGL gl)
        {
            lock (key)
            {
                GenVertexArray();

                if (VertexArray == null) return;

                if (Style.Size > 0) gl.PointSize(Style.Size);
                gl.Color(Style.BackgroundColor.GetFloats());

                gl.DrawArrays(OpenGL.GL_POINTS, VertexArray.Length / 2, VertexArray);
            }
        }

        /// <summary>
        /// 產生頂點陣列，加速繪圖
        /// </summary>
        private void GenVertexArray()
        {
            lock (key)
            {
                // 若已經產生頂點陣列，則返回
                if (VertexArray != null && LastGenVertexArrayTime >= Geometry.LastEditTime) return;

                LastGenVertexArrayTime = Geometry.LastEditTime;

                VertexArray = new int[Geometry.SaftyEdit(list => list.Count * 2)];
                int index = 0;

                Geometry.SaftyEdit(false, list => list.ForEach(pair =>
                {
                    VertexArray[index] = pair.X;
                    index++;
                    VertexArray[index] = pair.Y;
                    index++;
                }));
            }
        }
    }

    /// <summary>
    /// 複合線
    /// </summary>
    public class MultiStripLine : IMultiStripLine
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

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
        /// 最後一次產生頂點陣列的時間
        /// </summary>
        private DateTime LastGenVertexArrayTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 頂點陣列
        /// </summary>
        private int[] VertexArray { get; set; } = null;

        /// <summary>
        /// 繪圖
        /// </summary>
        public void Draw(OpenGL gl)
        {
            lock (key)
            {
                GenVertexArray();

                if (VertexArray == null) return;

                if (Style.Width > 0) gl.LineWidth(Style.Width);
                gl.Color(Style.BackgroundColor.GetFloats());

                gl.DrawArrays(OpenGL.GL_LINE_STRIP, VertexArray.Length / 2, VertexArray);
            }
        }

        /// <summary>
        /// 產生頂點陣列，加速繪圖
        /// </summary>
        private void GenVertexArray()
        {
            lock (key)
            {
                // 若已經產生頂點陣列，則返回
                if (VertexArray != null && LastGenVertexArrayTime >= Geometry.LastEditTime) return;

                LastGenVertexArrayTime = Geometry.LastEditTime;

                VertexArray = new int[Geometry.SaftyEdit(list => list.Count * 2)];
                int index = 0;

                Geometry.SaftyEdit(false, list => list.ForEach(pair =>
                {
                    VertexArray[index] = pair.X;
                    index++;
                    VertexArray[index] = pair.Y;
                    index++;
                }));
            }
        }
    }
}