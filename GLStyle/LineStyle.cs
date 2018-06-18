﻿using IniFiles;
using System.Collections.Generic;
using System.Linq;

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
        public LineStyle(string path, string section)
        {
            BackgroundColor = new Color(INI.Read(path, section, nameof(BackgroundColor), Color.GreenYellow128));
            Layer = INI.Read(path, section, nameof(Layer), 0);
            Pattern = (ELinePattern)INI.Read(path, section, nameof(Pattern), 0);
            ShowOnTheMenu = INI.Read(path, section, nameof(ShowOnTheMenu), false);
            Width = INI.Read(path, section, nameof(Width), 1.0f);
            Command = StandardOperate.ReadCommandFromINI(path, section).ToList();
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
        /// 是否在選單中顯示給使用者看
        /// </summary>
        public bool ShowOnTheMenu { get; }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleType { get { return nameof(ILineStyle); } }

        /// <summary>
        /// 線條寬
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// 右鍵選單中命令
        /// </summary>
        public List<string> Command { get; }
    }
}