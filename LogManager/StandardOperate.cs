using System;
using System.Runtime.CompilerServices;

namespace LogManager
{
    /// <summary>
    /// 標準操作
    /// </summary>
    public static class StandardOperate
    {
        /// <summary>
        /// 儲存例外狀況
        /// </summary>
        public static void WriteLog(this Exception ex, [CallerLineNumber] int line = 0, [CallerMemberName] string member = "", [CallerFilePath] string path = "")
        {
            LogManager.Log.ExceptionLog.Add(ex, line, member, path);
        }

        /// <summary>
        /// 儲存例外狀況
        /// </summary>
        public static void WriteLog(this Exception ex, string message, [CallerLineNumber] int line = 0, [CallerMemberName] string member = "", [CallerFilePath] string path = "")
        {
            LogManager.Log.ExceptionLog.Add(ex, message, line, member, path);
        }
    }
}