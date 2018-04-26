using IniFiles;

namespace GLStyle
{
    /// <summary>
    /// 標示物樣式
    /// </summary>
    internal class TowardPairStyle : ITowardPairStyle
    {
        /// <summary>
        /// 從 *.ini 檔案讀取樣式設定
        /// </summary>
        public TowardPairStyle(string filePath, string section)
        {
            BackgroundColor = new Color(INI.Read(filePath, section, nameof(BackgroundColor), Color.GreenYellow128));
            Height = INI.Read(filePath, section, nameof(Height), 1000.0f);
            ImagePath = INI.Read(filePath, section, nameof(ImagePath), "");
            Layer = INI.Read(filePath, section, nameof(Layer), 0);
            LineColor = new Color(INI.Read(filePath, section, nameof(LineColor), Color.Firebrick128));
            LineLength = INI.Read(filePath, section, nameof(LineLength), 1000.0f);
            LinePattern = (ELinePattern)INI.Read(filePath, section, nameof(LinePattern), 0);
            LineWidth = INI.Read(filePath, section, nameof(LineWidth), 1.0f);
            ShowOnTheMenu = INI.Read(filePath, section, nameof(ShowOnTheMenu), false);
            Width = INI.Read(filePath, section, nameof(Width), 1500.0f);
        }

        /// <summary>
        /// 底色
        /// </summary>
        public IColor BackgroundColor { get; }

        /// <summary>
        /// 長
        /// </summary>
        public float Height { get; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath { get; }

        /// <summary>
        /// 圖層位置
        /// </summary>
        public int Layer { get; }

        /// <summary>
        /// 線段顏色
        /// </summary>
        public IColor LineColor { get; }

        /// <summary>
        /// 線長
        /// </summary>
        public float LineLength { get; }

        /// <summary>
        /// 線段樣式
        /// </summary>
        public ELinePattern LinePattern { get; }

        /// <summary>
        /// 線條寬
        /// </summary>
        public float LineWidth { get; }

        /// <summary>
        /// 是否在選單中顯示給使用者看
        /// </summary>
        public bool ShowOnTheMenu { get; }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleType { get { return nameof(ITowardPairStyle); } }

        /// <summary>
        /// 寬
        /// </summary>
        public float Width { get; }
    }
}