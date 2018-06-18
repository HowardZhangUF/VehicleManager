using System;
using System.Collections.Generic;
using System.IO;

namespace LogManager
{
    /// <summary>
    /// Log 紀錄器
    /// </summary>
    public class Writter
    {
        /// <summary>
        /// 不公開建構子，不讓外部使用者 new
        /// </summary>
        internal Writter(string path)
        {
            Path = path;
        }

        /// <summary>
        /// 存檔路徑，不含副檔名
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// 尚未存檔的資料
        /// </summary>
        private readonly List<string> data = new List<string>();

        /// <summary>
        /// 尚未存檔的資料數目
        /// </summary>
        public int Count { get { lock (key) return data.Count; } }

        /// <summary>
        /// 存檔
        /// </summary>
        internal void Save()
        {
            lock (sKey)
            {
                lock (key)
                {
                    try
                    {
                        if (data.Count > 0)
                        {
                            // 將來改為 DB 版本，也是將資料庫儲存方法寫在這裡
                            // 儲存時由於 static readonly object sKey 執行緒鎖
                            // 可以保證同一個時間只有一個人在寫資料庫
                            // 避免發生不可預期的錯誤
                            var file = new StreamWriter($"{Path}-{DateTime.Today.ToString("yyMMdd")}.txt", true);
                            foreach (var item in data)
                            {
                                file.WriteLine(item);
                            }
                            file.Close();
                            data.Clear();
                        }
                    }
                    finally
                    {
                        LastSaveTime = DateTime.Now;
                    }
                }
            }
        }

        /// <summary>
        /// 加入新資料
        /// </summary>
        public void Add(string data)
        {
            string newLine = $"[{Now}]:{data}";
            lock (key)
            {
                this.data.Add(newLine);
            }
        }

        /// <summary>
        /// 加入新資料
        /// </summary>
        public void Add(Exception ex)
        {
            string newLine = $"[{Now}]:src={ex.Source};msg={ex.Message}";
            lock (key)
            {
                this.data.Add(newLine);
            }
        }

        /// <summary>
        /// 加入新資料
        /// </summary>
        public void Add(Exception ex, string data)
        {
            string newLine = $"[{Now}]:src={ex.Source};msg={ex.Message};data={data}";
            lock (key)
            {
                this.data.Add(newLine);
            }
        }

        /// <summary>
        /// 當前時間字串
        /// </summary>
        public string Now { get { return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff"); } }

        /// <summary>
        /// 執行緒鎖，用來鎖自身的 <see cref="Add"/>, <see cref="Count"/>, <see cref="Save"/> 等函式
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 執行緒鎖，用 static 來保證同一個時間只會有一個檔案被寫入
        /// </summary>
        private static readonly object sKey = new object();

        /// <summary>
        /// 最後存檔時間
        /// </summary>
        public DateTime LastSaveTime { get; private set; } = DateTime.Now;
    }
}