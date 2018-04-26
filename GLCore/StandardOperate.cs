using Geometry;
using GLStyle;
using SharpGL;
using System;

namespace GLCore
{
    /// <summary>
    /// 標準操作
    /// </summary>
    public static class StandardOperate
    {
        /// <summary>
        /// 顯示文字字體
        /// </summary>
        public static System.Drawing.Font TextFont { get; } = new System.Drawing.Font("Arial", 16);

        /// <summary>
        /// 文字顏色
        /// </summary>
        private static IColor TextColor { get; } = new Color(Color.Black);

        /// <summary>
        /// <para>提供使用頂點數據組繪製 2D 物件的操作方法</para>
        /// <para><paramref name="mode"/> 表示繪圖模式，<paramref name="count"/> 為資料組數量</para>
        /// <para>繪圖模式參考 OpenGL 原生的 DrawArray() 函式</para>
        /// </summary>
        public static void DrawArrays(this OpenGL gl, uint mode, int count, int[] array)
        {
            if (count <= 0 || array == null) return;
            gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
            gl.VertexPointer(2, 0, array);
            gl.DrawArrays(mode, 0, count);
            gl.DisableClientState(OpenGL.GL_VERTEX_ARRAY);
        }

        /// <summary>
        /// 印出所有的 Text 資訊
        /// </summary>
        public static void DrawText(this OpenGL gl, string text, IPair glPosition, Func<IPair, IPair> convert)
        {
            if (string.IsNullOrEmpty(text)) return;
            IPair screen = convert(glPosition);
            gl.DrawText(screen.X, screen.Y, TextColor.R / 255.0f, TextColor.G / 255.0f, TextColor.B / 255.0f, TextFont.Name, TextFont.Size, text);
        }

        public static void InitialText(this OpenGL gl)
        {
            gl.DrawText(0, 0, TextColor.R / 255.0f, TextColor.G / 255.0f, TextColor.B / 255.0f, TextFont.Name, TextFont.Size, "");
        }
    }
}