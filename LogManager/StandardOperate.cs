using System;

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
        public static void WriteLog(this Exception ex)
        {
            LogManager.Log.ExceptionLog.Add(ex);
        }

        /// <summary>
        /// 儲存例外狀況
        /// </summary>
        public static void WriteLog(this Exception ex, string data)
        {
            LogManager.Log.ExceptionLog.Add(ex, data);
        }
    }
}