using System;
using System.Text;
using static IniFiles.NativeMethods;

namespace IniFiles
{
    /// <summary>
    /// *ini 讀寫器
    /// </summary>
    public static class INI
    {
        /// <summary>
        /// 讀取字串
        /// </summary>
        public static string Read(string filePath, string section, string key, string @default, int stringLength = 512)
        {
            StringBuilder val = new StringBuilder(stringLength);
            int bufLen = GetPrivateProfileString(section, key, @default, val, stringLength, filePath);
            return val.ToString();
        }

        /// <summary>
        /// 讀取整數
        /// </summary>
        public static int Read(string filePath, string section, string key, int @default)
        {
            string intStr = Read(filePath, section, key, Convert.ToString(@default));
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception)
            {
                return @default;
            }
        }

        /// <summary>
        /// 讀取小數
        /// </summary>
        public static double Read(string filePath, string section, string key, double @default)
        {
            string intStr = Read(filePath, section, key, Convert.ToString(@default));
            try
            {
                return Convert.ToDouble(intStr);
            }
            catch (Exception)
            {
                return @default;
            }
        }

        /// <summary>
        /// 讀取小數
        /// </summary>
        public static float Read(string filePath, string section, string key, float @default)
        {
            string intStr = Read(filePath, section, key, Convert.ToString(@default));
            try
            {
                return (float)Convert.ToDouble(intStr);
            }
            catch (Exception)
            {
                return @default;
            }
        }

        /// <summary>
        /// 讀取布林值
        /// </summary>
        public static bool Read(string filePath, string section, string key, bool @default)
        {
            string intStr = Read(filePath, section, key, Convert.ToString(@default));
            try
            {
                return Convert.ToBoolean(intStr);
            }
            catch (Exception)
            {
                return @default;
            }
        }

        /// <summary>
        /// 寫入字串
        /// </summary>
        public static bool Write(string filePath, string section, string key, string value)
        {
            return WritePrivateProfileString(section, key, value, filePath);
        }

        /// <summary>
        /// 寫入整數
        /// </summary>
        public static bool Write(string filePath, string section, string key, int value)
        {
            return WritePrivateProfileString(section, key, value.ToString(), filePath);
        }

        /// <summary>
        /// 寫入小數
        /// </summary>
        public static bool Write(string filePath, string section, string key, double value)
        {
            return WritePrivateProfileString(section, key, value.ToString(), filePath);
        }

        /// <summary>
        /// 寫入布林值
        /// </summary>
        public static bool Write(string filePath, string section, string key, bool value)
        {
            return WritePrivateProfileString(section, key, value.ToString(), filePath);
        }
    }
}