using IniFiles;
using System.Collections.Generic;

namespace GLStyle
{
	/// <summary>
	/// AGV 樣式
	/// </summary>
	internal class AGVStyle : IAGVStyle
    {
        /// <summary>
        /// 基底樣式
        /// </summary>
        private ITowardPairStyle @base;

        /// <summary>
        /// 從 *.ini 檔案讀取樣式設定
        /// </summary>
        public AGVStyle(string path, string section)
        {
            @base = new TowardPairStyle(path, section);
            SafetyWidth = INI.Read(path, section, nameof(SafetyWidth), 1000);
            SafetyHeight = INI.Read(path, section, nameof(SafetyHeight), 1000);
            SafetyColor = new Color(INI.Read(path, section, nameof(SafetyColor), Color.Red));
            SafetyLineWidth = INI.Read(path, section, nameof(SafetyLineWidth), 1.0f);
            SafetyLinePattern = (ELinePattern)INI.Read(path, section, nameof(SafetyLinePattern), 0);
        }

        /// <summary>
        /// 底色
        /// </summary>
        public IColor BackgroundColor => @base.BackgroundColor;

        /// <summary>
        /// 長
        /// </summary>
        public float Height => @base.Height;

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImagePath => @base.ImagePath;

        /// <summary>
        /// 圖層位置
        /// </summary>
        public int Layer => @base.Layer;

        /// <summary>
        /// 線段顏色
        /// </summary>
        public IColor LineColor => @base.LineColor;

        /// <summary>
        /// 線長
        /// </summary>
        public float LineLength => @base.LineLength;

        /// <summary>
        /// 線段樣式
        /// </summary>
        public ELinePattern LinePattern => @base.LinePattern;

        /// <summary>
        /// 線條寬
        /// </summary>
        public float LineWidth => @base.LineWidth;

        /// <summary>
        /// 安全框顏色
        /// </summary>
        public IColor SafetyColor { get; }

        /// <summary>
        /// 安全框高
        /// </summary>
        public int SafetyHeight { get; }

        /// <summary>
        /// 安全框線條樣式
        /// </summary>
        public ELinePattern SafetyLinePattern { get; }

        /// <summary>
        /// 安全框線寬
        /// </summary>
        public float SafetyLineWidth { get; }

        /// <summary>
        /// 安全框寬
        /// </summary>
        public int SafetyWidth { get; }

        /// <summary>
        /// 是否在選單中顯示給使用者看
        /// </summary>
        public bool ShowOnTheMenu => @base.ShowOnTheMenu;

        /// <summary>
        /// 樣式種類
        /// </summary>
        public string StyleType => @base.StyleType;

        /// <summary>
        /// 寬
        /// </summary>
        public float Width => @base.Width;

        /// <summary>
        /// 右鍵選單中命令
        /// </summary>
        public List<string> Command => @base.Command;
    }
}