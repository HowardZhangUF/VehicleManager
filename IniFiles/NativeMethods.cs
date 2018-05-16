using System.Runtime.InteropServices;
using System.Text;

namespace IniFiles
{
    /// <summary>
    /// 原生方法
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// 讀取函式
        /// </summary>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetPrivateProfileString(string section, string key, string @default, StringBuilder retVal, int size, string path);

        /// <summary>
        /// 寫入函式
        /// </summary>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileString(string section, string key, string value, string path);
    }
}