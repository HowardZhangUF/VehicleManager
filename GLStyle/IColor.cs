namespace GLStyle
{
    /// <summary>
    /// 提供 Byte 格式的的顏色操作介面
    /// </summary>
    public interface IColor : IGLStyle
    {
        /// <summary>
        /// 透明度(A=255表示不透明)
        /// </summary>
        byte A { get; set; }

        /// <summary>
        /// 藍
        /// </summary>
        byte B { get; set; }

        /// <summary>
        /// 綠
        /// </summary>
        byte G { get; set; }

        /// <summary>
        /// 紅
        /// </summary>
        byte R { get; set; }

        /// <summary>
        /// 回傳 .Net 顏色定義
        /// </summary>
        System.Drawing.Color DotNetColor();

        /// <summary>
        /// 獲得介於[0,1]之間的浮點數陣列
        /// </summary>
        float[] GetFloats();
    }
}