using IniFiles;

namespace GLStyle
{
    /// <summary>
    /// 面樣式
    /// </summary>
    internal class AreaStyle : IAreaStyle
    {
        /// <summary>
        /// 從 *.ini 檔案讀取樣式設定
        /// </summary>
        public AreaStyle(string filePath, string section)
        {
            BackgroundColor = new Color(INI.Read(filePath, section, nameof(BackgroundColor), Color.GreenYellow128));
            Layer = INI.Read(filePath, section, nameof(Layer), 0);
        }

        /// <summary>
        /// 底色
        /// </summary>
        public IColor BackgroundColor { get; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath { get; }

        /// <summary>
        /// 圖層位置
        /// </summary>
        public int Layer { get; }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleType { get { return nameof(IAreaStyle); } }
    }
}