using IniFiles;

namespace GLStyle
{
    /// <summary>
    /// 線段樣式
    /// </summary>
    internal class LineStyle : ILineStyle
    {
        /// <summary>
        /// 從 *.ini 檔案讀取樣式設定
        /// </summary>
        public LineStyle(string filePath, string section)
        {
            BackgroundColor = new Color(INI.Read(filePath, section, nameof(BackgroundColor), Color.GreenYellow128));
            Layer = INI.Read(filePath, section, nameof(Layer), 0);
            Pattern = (ELinePattern)INI.Read(filePath, section, nameof(Pattern), 0);
            Width = INI.Read(filePath, section, nameof(Width), 1.0f);
        }

        /// <summary>
        /// 底色
        /// </summary>
        public IColor BackgroundColor { get; }

        /// <summary>
        /// 圖層位置
        /// </summary>
        public int Layer { get; }

        /// <summary>
        /// 線段樣式
        /// </summary>
        public ELinePattern Pattern { get; }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleType { get { return nameof(ILineStyle); } }

        /// <summary>
        /// 線條寬
        /// </summary>
        public float Width { get; }
    }
}