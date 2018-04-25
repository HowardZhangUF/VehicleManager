using GLStyle;
using SharpGL;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace GLCore
{
    /// <summary>
    /// GL 貼圖
    /// </summary>
    public static class GLTexture
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private static readonly object mKey = new object();

        /// <summary>
        /// 地圖轉 byte 對應資料
        /// </summary>
        private static Dictionary<string, uint> mData = new Dictionary<string, uint>();

        /// <summary>
        /// 使用貼圖繪製矩形，如果貼圖不存在，則用 color 著色
        /// </summary>
        public static void TextureBmp(this OpenGL gl, string name, double width, double height, IColor color)
        {
            lock (mKey)
            {
                if (!mData.ContainsKey(name)) ConvertBmpToBytes(gl, name);
                if (mData.ContainsKey(name))
                {
                    gl.Color(1.0, 1.0, 1.0, 1.0);
                    gl.BindTexture(OpenGL.GL_TEXTURE_2D, mData[name]);
                    gl.Enable(OpenGL.GL_TEXTURE_2D);
                    {
                        gl.Begin(OpenGL.GL_QUADS);
                        {
                            gl.TexCoord(0.0, 0.0);
                            gl.Vertex(-width / 2, -height / 2, 0);
                            gl.TexCoord(1.0, 0.0);
                            gl.Vertex(+width / 2, -height / 2, 0);
                            gl.TexCoord(1.0, 1.0);
                            gl.Vertex(+width / 2, +height / 2, 0);
                            gl.TexCoord(0.0, 1.0);
                            gl.Vertex(-width / 2, +height / 2, 0);
                        }
                        gl.End();
                    }
                    gl.Disable(OpenGL.GL_TEXTURE_2D);
                    gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
                }
                else
                {
                    gl.Color(color.GetFloats());
                    gl.Begin(OpenGL.GL_QUADS);
                    {
                        gl.Vertex(-width / 2, -height / 2, 0);
                        gl.Vertex(+width / 2, -height / 2, 0);
                        gl.Vertex(+width / 2, +height / 2, 0);
                        gl.Vertex(-width / 2, +height / 2, 0);
                    }
                    gl.End();
                }
            }
        }

        private static void ConvertBmpToBytes(OpenGL gl, string name)
        {
            lock (mKey)
            {
                Bitmap image = new Bitmap(name);
                if (image != null)
                {
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                    BitmapData bitmapdata = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                    uint[] tArray = new uint[1];
                    gl.GenTextures(1, tArray);

                    gl.BindTexture(OpenGL.GL_TEXTURE_2D, tArray[0]);

                    gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR_MIPMAP_NEAREST);
                    gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR_MIPMAP_LINEAR);

                    gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, OpenGL.GL_RGBA, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, bitmapdata.Scan0);

                    gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);

                    image.UnlockBits(bitmapdata);
                    mData.Add(name, tArray[0]);
                }
            }
        }
    }
}