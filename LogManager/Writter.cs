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
            lock (key)
            {
                try
                {
                    if (data.Count > 0)
                    {
                        var file = new StreamWriter($"{Path}-{DateTime.Today.ToString("yyMMdd")}.txt", true);
                        foreach (var item in data)
                        {
                            file.WriteLine(item);
                        }
                        data.Clear();
                        file.Close();
                    }
                }
                finally
                {
                    LastSaveTime = DateTime.Now;
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
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 最後存檔時間
        /// </summary>
        public DateTime LastSaveTime { get; private set; } = DateTime.Now;
    }
}