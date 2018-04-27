namespace GLCore
{
    /// <summary>
    /// 遞增序號管理器
    /// </summary>
    public class SerialNumberManager
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private object key = new object();

        /// <summary>
        /// 編號
        /// </summary>
        private int number = 0;

        /// <summary>
        /// 取得下一個不重複的號碼
        /// </summary>
        public int Next()
        {
            lock (key)
            {
                number++;
                return number;
            }
        }
    }
}