using Geometry;
using GLStyle;
using SharpGL;
using System;
using System.Collections.Generic;
using ThreadSafety;

namespace GLCore
{
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
        public ISafty<List<IArea>> Geometry { get; } = new Safty<List<IArea>>(new List<IArea>());

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
}