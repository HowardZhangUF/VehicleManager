using Geometry;
using IniFiles;
using SharpGL;
using System;
using System.Collections.Generic;

namespace GLStyle
{
	/// <summary>
	/// 標準操作
	/// </summary>
	public static class StandardOperate
    {
        /// <summary>
        /// 開啟虛線
        /// </summary>
        public static void BeginStippleLine(this OpenGL gl, ELinePattern style, int factor = 5)
        {
            if (style == ELinePattern._1111111111111111) return;
            ushort pattern = Convert.ToUInt16(style.ToString().Substring(1), 2);
            gl.Enable(OpenGL.GL_LINE_STIPPLE);
            gl.LineStipple(factor, pattern);
        }

        /// <summary>
        /// <para>提供使用頂點數據組繪製 2D 物件的操作方法</para>
        /// <para><paramref name="mode"/> 表示繪圖模式，<paramref name="count"/> 為資料組數量</para>
        /// <para>繪圖模式參考 OpenGL 原生的 DrawArray() 函式</para>
        /// </summary>
        public static void DrawArray(this OpenGL gl, uint mode, int count, int[] array)
        {
            if (count <= 0 || array == null) return;
            gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
            gl.VertexPointer(2, 0, array);
            gl.DrawArrays(mode, 0, count);
            gl.DisableClientState(OpenGL.GL_VERTEX_ARRAY);
        }

        /// <summary>
        /// 在原點繪製線段
        /// </summary>
        public static void DrawLine(this OpenGL gl, IAngle angle, float length)
        {
            gl.Begin(OpenGL.GL_LINES);
            {
                gl.Vertex(0, 0, 0);
                gl.Vertex(length * Math.Cos(angle.Theta * Math.PI / 180.0), length * Math.Sin(angle.Theta * Math.PI / 180.0), 0);
            }
            gl.End();
        }

        /// <summary>
        /// 在原點繪製矩形
        /// </summary>
        public static void DrawRectangle(this OpenGL gl, float width, float height)
        {
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(-width / 2, -height / 2);
                gl.Vertex(+width / 2, -height / 2);
                gl.Vertex(+width / 2, +height / 2);
                gl.Vertex(-width / 2, +height / 2);
            }
            gl.End();
        }

        /// <summary>
        /// 關閉虛線
        /// </summary>
        public static void EndStippleLine(this OpenGL gl)
        {
            gl.Disable(OpenGL.GL_LINE_STIPPLE);
        }

        /// <summary>
        /// 從INI設定檔中讀取右鍵選單中命令
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> ReadCommandFromINI(string path, string section)
        {
            for (int ii = 0; true; ii++)
            {
                string cmd = INI.Read(path, section, $"Command{ii}", string.Empty);

                if (string.IsNullOrEmpty(cmd)) yield break;

                // cmd not null
                yield return cmd;
            }
        }
    }
}