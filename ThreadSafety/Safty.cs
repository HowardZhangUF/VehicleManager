using System;

namespace ThreadSafety
{
    /// <summary>
    /// 提供執行緒安全的操作物件
    /// </summary>
    public class Safty<T> : ISafty<T>
    {
        private T data = default(T);
        private object mKey = new object();
        private DateTime mLastEditTime = DateTime.Now;

        /// <summary>
        /// 建構具執行緒安全的操作物件
        /// </summary>
        public Safty(T newData)
        {
            data = newData;
        }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime LastEditTime { get { lock (mKey) return mLastEditTime; } private set { lock (mKey) mLastEditTime = value; } }

        /// <summary>
        /// 執行緒安全操作，若操作過程中會改變資料，請將 <paramref name="isDataChange"/> 設為 True
        /// </summary>
        public void SaftyEdit(bool isDataChange, Action<T> action)
        {
            lock (mKey)
            {
                action(data);
                if (isDataChange) LastEditTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 執行緒安全操作，請勿在這裡修改資料
        /// </summary>
        public TResult SaftyEdit<TResult>(Func<T, TResult> func)
        {
            lock (mKey)
            {
                return func(data);
            }
        }
    }
}