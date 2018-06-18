using System;
using System.Collections.Generic;
using System.Threading;

namespace LogManager
{
    /// <summary>
    /// Log 紀錄器管理器
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// 狀態改變 Log 紀錄
        /// </summary>
        public Writter StatusChange { get; } = new Writter("StatusChange");

        /// <summary>
        /// 例外狀況 Log 紀錄
        /// </summary>
        public Writter Exception { get; } = new Writter("Exception");

        /// <summary>
        /// Log 集合
        /// </summary>
        private readonly List<Writter> writterList;

        /// <summary>
        /// <para>Log 紀錄器管理器</para>
        /// <para>範例：<see cref="Log"/>.StatusChange.Add("Message");</para>
        /// </summary>
        public static LogManager Log { get; } = new LogManager();

        /// <summary>
        /// 儲存檔案用的執行緒
        /// </summary>
        private Thread thread { get; set; }

        /// <summary>
        /// 不公開建構子，不讓外部使用者 new
        /// </summary>
        internal LogManager()
        {
            writterList = new List<Writter>()
            {
                StatusChange,Exception
            };

            thread = new Thread(SaveLog)
            {
                IsBackground = true
            };
            thread.Start();
        }

        /// <summary>
        /// 儲存 Log 資料
        /// </summary>
        private void SaveLog()
        {
            while (true)
            {
                foreach (var item in writterList)
                {
                    if (item.Count >= 10 || (DateTime.Now - item.LastSaveTime).TotalSeconds >= 10)
                    {
                        item.Save();
                    }
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 儲存全部
        /// </summary>
        public void SaveAll()
        {
            foreach (var item in writterList) item.Save();
        }
    }
}