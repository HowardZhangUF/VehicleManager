using System;

namespace ThreadSafety
{
    /// <summary>
    /// 提供執行緒安全的操作介面
    /// </summary>
    public interface ISafty<T> : IThreadSafety
    {
        /// <summary>
        /// 最後修改時間
        /// </summary>
        DateTime LastEditTime { get; }

        /// <summary>
        /// 執行緒安全操作，若操作過程中會改變資料，請將 <paramref name="isDataChange"/> 設為 True
        /// </summary>
        void SaftyEdit(bool isDataChange, Action<T> action);

        /// <summary>
        /// 執行緒安全操作，請勿在這裡修改資料
        /// </summary>
        TResult SaftyEdit<TResult>(Func<T, TResult> func);
    }
}