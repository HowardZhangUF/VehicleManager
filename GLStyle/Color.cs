using System;
using System.Linq;

namespace GLStyle
{
    /// <summary>
    /// 提供 Byte 格式的的顏色操作介面
    /// </summary>
    [Serializable]
    public class Color : IColor
    {
        /// <summary>
        /// 基本色
        /// </summary>
        public const string Black = "0,0,0,255";

        /// <summary>
        /// 基本色
        /// </summary>
        public const string Firebrick128 = "178,34,34,128";

        /// <summary>
        /// 基本色
        /// </summary>
        public const string GreenYellow128 = "173,255,47,128";

        /// <summary>
        /// 基本色
        /// </summary>
        public const string Red = "255,0,0,255";

        /// <summary>
        /// 建立不透明的顏色
        /// </summary>
        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// 不透明黑色
        /// </summary>
        public Color()
        {
        }

        /// <summary>
        /// 複製建構子
        /// </summary>
        public Color(IColor color) : this(color.R, color.G, color.B, color.A)
        {
        }

        /// <summary>
        /// 由 DotNetColor 轉為不透明的 GL Color
        /// </summary>
        public Color(System.Drawing.Color color) : this(color.R, color.G, color.B, color.A)
        {
        }

        /// <summary>
        /// 由 DotNetColor 轉為 GL Color，並決定 apha 值
        /// </summary>
        public Color(System.Drawing.Color color, byte a) : this(color.R, color.G, color.B, a)
        {
        }

        /// <summary>
        /// 由字串建立顏色，字串格式為 R,G,B,A ，用逗號分隔，例如： 128,128,128,255
        /// </summary>
        public Color(string rgba)
        {
            var elements = rgba.Split(',')
                .Select(elm => Convert.ToByte(elm));

            R = elements.ElementAt(0);
            G = elements.ElementAt(1);
            B = elements.ElementAt(2);
            A = elements.ElementAt(3);
        }

        /// <summary>
        /// 透明度(A=255表示不透明)
        /// </summary>
        public byte A { get; set; } = 255;

        /// <summary>
        /// 藍
        /// </summary>
        public byte B { get; set; } = 0;

        /// <summary>
        /// 綠
        /// </summary>
        public byte G { get; set; } = 0;

        /// <summary>
        /// 紅
        /// </summary>
        public byte R { get; set; } = 0;

        /// <summary>
        /// 回傳 .Net 顏色定義
        /// </summary>
        public System.Drawing.Color DotNetColor()
        {
            return System.Drawing.Color.FromArgb(A, R, G, B);
        }

        /// <summary>
        /// 獲得介於[0,1]之間的浮點數陣列
        /// </summary>
        public float[] GetFloats() => new float[] { R / 255f, G / 255f, B / 255f, A / 255f };
    }
}