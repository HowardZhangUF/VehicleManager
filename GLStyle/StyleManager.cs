using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThreadSafety;

namespace GLStyle
{
    /// <summary>
    /// 樣式管理器
    /// </summary>
    public static class StyleManager
    {
        /// <summary>
        /// 樣式列表，key = 樣式名稱, vakue = 樣式
        /// </summary>
        private static ISafty<Dictionary<string, IStyle>> StyleTable { get; } = new Safty<Dictionary<string, IStyle>>(new Dictionary<string, IStyle>());

        /// <summary>
        /// 依據名稱從樣式列表(<see cref="StyleTable"/>)中選擇對應的樣式，若樣式不存在，則返回 null
        /// </summary>
        public static IStyle GetStyle(string styleName)
        {
            return StyleTable.SaftyEdit(dic => dic
            .Where(item => item.Key == styleName)
            .Select(item => item.Value)).FirstOrDefault() ?? null;
        }

        /// <summary>
        /// 讀取所有除了不顯示在右鍵選單的樣式名稱
        /// </summary>
        public static IEnumerable<string> GetStyleNames()
        {
            return StyleTable.SaftyEdit(dic => dic
            .Where(item => item.Value.ShowOnTheMenu)
            .Select(item => item.Key));
        }

        /// <summary>
        /// 讀取所有的樣式名稱
        /// </summary>
        public static IEnumerable<string> GetAllStyleNames()
        {
            return StyleTable.SaftyEdit(dic => dic
            .Select(item => item.Key));
        }

        /// <summary>
        /// 讀取所有除了不顯示在右鍵選單的指定樣式名稱
        /// </summary>
        public static IEnumerable<string> GetStyleNames(string styleType)
        {
            return StyleTable.SaftyEdit(dic => dic
            .Where(item => item.Value.ShowOnTheMenu && item.Value.StyleType == styleType)
            .Select(item => item.Key));
        }

        /// <summary>
        /// 讀取所有的指定樣式名稱
        /// </summary>
        public static IEnumerable<string> GetAllStyleNames(string styleType)
        {
            return StyleTable.SaftyEdit(dic => dic
            .Where(item => item.Value.StyleType == styleType)
            .Select(item => item.Key));
        }

        /// <summary>
        /// 取得樣式種類。若 <paramref name="styleName"/> 不存在則回傳 <see cref="string.Empty"/>
        /// </summary>
        public static string GetStyleType(string styleName)
        {
            return StyleTable.SaftyEdit(dic => dic
            .Where(item => item.Key == styleName)
            .Select(item => item.Value.StyleType)).FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// <para>從 *.ini 檔案讀取樣式設定</para>
        /// <para>只能讀取一次</para>
        /// </summary>
        public static void LoadStyle(string filePath)
        {
            StyleTable.SaftyEdit(true, table =>
            {
                if (table.Count != 0) return;

                var names = File.ReadAllLines(filePath)
                    .Select(line => line.Trim())
                    .Where(line => line.StartsWith("[") && line.EndsWith("]"))
                    .Select(line => line.Substring(1, line.Length - 2));

                table.Clear();
                foreach (var name in names)
                {
                    var type = IniFiles.INI.Read(filePath, name, "Type", "");
                    switch (type)
                    {
                        case nameof(IPairStyle):
                            table.Add(name, new PairStyle(filePath, name));
                            break;

                        case nameof(ILineStyle):
                            table.Add(name, new LineStyle(filePath, name));
                            break;

                        case nameof(IAreaStyle):
                            table.Add(name, new AreaStyle(filePath, name));
                            break;

                        case nameof(ITowardPairStyle):
                            table.Add(name, new TowardPairStyle(filePath, name));
                            break;
                    }
                }
            });
        }

        /// <summary>
        /// 邊界樣式
        /// </summary>
        public static class BoundStyle
        {
            /// <summary>
            /// 底色
            /// </summary>
            public static IColor BackgroundColor { get; } = new Color(Color.Red);

            /// <summary>
            /// 圖層
            /// </summary>
            public static int Layer { get; } = 100;

            /// <summary>
            /// 線條樣式
            /// </summary>
            public static ELinePattern Pattern { get; } = ELinePattern._1111111111111111;

            /// <summary>
            /// 寬度
            /// </summary>
            public static float Width { get; } = 3.0f;
        }
    }
}