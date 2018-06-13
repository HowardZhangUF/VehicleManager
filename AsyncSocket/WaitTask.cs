using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncSocket
{
    /// <summary>
    /// <para>非同步作業執行緒。</para>
    /// <para>當呼叫 <see cref="Start"/> 函式時，此物件會確保執行緒已經啟動才離開 <see cref="Start"/> 函式</para>
    /// </summary>
    public class WaitTask : IDisposable
    {
        /// <summary>
        /// 基底類別
        /// </summary>
        private Task @base { get; set; }

        /// <summary>
        /// 等待開始事件鎖
        /// </summary>
        private ManualResetEvent waitStart { get; set; }

        /// <summary>
        /// <para>建立非同步作業執行緒。</para>
        /// <para>當呼叫 <see cref="Start"/> 函式時，此物件會確保執行緒已經啟動才離開 <see cref="Start"/> 函式</para>
        /// </summary>
        public WaitTask(Action action)
        {
            // 設定尚未解鎖
            waitStart = new ManualResetEvent(false);


            @base = new Task(() =>
            {
                // 解鎖
                waitStart.Set();

                action();
            });
        }

        /// <summary>
        /// 當呼叫此函式時，此物件會確保執行緒已經啟動才離開此函式</para>
        /// </summary>
        public void Start()
        {
            @base.Start();

            // 等待解鎖
            waitStart.WaitOne();
        }

        #region Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (waitStart != null)
                    waitStart.Dispose();
                if (@base != null)
                    @base.Dispose();

                disposed = true;
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
            }
        }

        ~WaitTask()
        {
            Dispose(false);
        }

        #endregion Dispose
    }
}
