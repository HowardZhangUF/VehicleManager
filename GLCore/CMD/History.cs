using System;
using System.Collections.Generic;

namespace GLCore
{
    /// <summary>
    /// FILO 歷史紀錄
    /// </summary>
    public class History
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private object key = new object();

        /// <summary>
        /// 建立歷史紀錄。 <paramref name="maxLine"/> 為最大記錄筆數
        /// </summary>
        public History(int maxLine)
        {
            MaxLine = maxLine;
        }

        /// <summary>
        /// 旗標，從 0 ~ Flag-1 表示已經做的步驟，從 Flag ~ Data.Count-1 表示復原的總數
        /// </summary>
        public int Flag { get; private set; }

        /// <summary>
        /// 紀錄最大筆數
        /// </summary>
        public int MaxLine { get; }

        /// <summary>
        /// 歷史資料
        /// </summary>
        private List<string> Data { get; } = new List<string>();

        /// <summary>
        /// 將 <see cref="Flag"/> 向後移
        /// </summary>
        public bool Backward(int step)
        {
            lock (key)
            {
                if (Flag < MaxLine)
                {
                    Flag = Math.Min(Flag + step, Data.Count);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 清除紀錄
        /// </summary>
        public void Clear()
        {
            lock (key)
            {
                Flag = 0;
                Data.Clear();
            }
        }

        /// <summary>
        /// 將 <see cref="Flag"/> 向前移
        /// </summary>
        public bool Forward(int step)
        {
            lock (key)
            {
                if (Flag > 0)
                {
                    Flag = Math.Max(Flag - step, 0);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 獲得已做的歷史紀錄
        /// </summary>
        public IEnumerable<string> GetDoHistory()
        {
            lock (key)
            {
                for (int ii = 0; ii < Flag; ii++)
                {
                    yield return Data[ii];
                }
            }
        }

        /// <summary>
        /// 獲得已復原的歷史紀錄
        /// </summary>
        public IEnumerable<string> GetUndoHistory()
        {
            lock (key)
            {
                for (int ii = Flag; ii < Data.Count; ii++)
                {
                    yield return Data[ii];
                }
            }
        }

        /// <summary>
        /// 加入一筆紀錄。若紀錄已滿，則回傳第一筆紀錄，否則回傳 <see cref="string.Empty"/>
        /// </summary>
        public string Push(string newData)
        {
            lock (key)
            {
                Data.RemoveRange(Flag, Data.Count - Flag);
                Data.Add(newData);
                if (Data.Count > MaxLine)
                {
                    var overflow = Data[0];
                    Data.RemoveAt(0);
                    Flag = Data.Count;
                    return overflow;
                }
                Flag = Data.Count;
                return string.Empty;
            }
        }
    }
}