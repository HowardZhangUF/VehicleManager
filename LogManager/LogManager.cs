using System;
using System.Collections.Generic;
using System.Threading;

namespace LogManager
{
    /// <summary>
    /// 紀錄管理器
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// 狀態改變之紀錄
        /// </summary>
        public Writter StatusChangeLog { get; } = new Writter("StatusChange");

        /// <summary>
        /// 例外狀況之紀錄
        /// </summary>
        public Writter ExceptionLog { get; } = new Writter("Exception");

        /// <summary>
        /// 紀錄器集合
        /// </summary>
        private readonly List<Writter> writterList;

        /// <summary>
        /// <para>紀錄管理器</para>
        /// <para>使用範例：<see cref="Log"/>.StatusChangeLog.Add("Message");</para>
        /// </summary>
        public static LogManager Log { get; } = new LogManager();

        /// <summary>
        /// 儲存檔案用的執行緒
        /// </summary>
        private Thread save { get; set; }

        /// <summary>
        /// 不公開建構子，不讓外部使用者 new
        /// </summary>
        internal LogManager()
        {
            writterList = new List<Writter>()
            {
                StatusChangeLog,ExceptionLog
            };

            save = new Thread(Save)
            {
                IsBackground = true
            };
            save.Start();
        }

        /// <summary>
        /// 儲存 <see cref="writterList"/> 無窮迴圈
        /// </summary>
        private void Save()
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
        /// 儲存全部紀錄，此函式會將 <see cref="writterList"/> 中資料逐一儲存後才離開，
        /// 可能會耗上一些時間。
        /// </summary>
        public void SaveAll()
        {
            foreach (var item in writterList) item.Save();
        }
    }
}