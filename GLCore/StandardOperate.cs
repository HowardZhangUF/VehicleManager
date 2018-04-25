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