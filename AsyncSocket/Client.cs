using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WaitTask;

namespace AsyncSocket
{
    /// <summary>
    /// 非同步通訊用戶端
    /// </summary>
    public class Client : IClient, IDisposable
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 通訊界面
        /// </summary>
        private Socket socket;

        /// <summary>
        /// 接受連線
        /// </summary>
        private void AsyncConnectCallback(IAsyncResult ar)
        {
            lock (key)
            {
                var handler = ar.AsyncState as Socket;
                var res = handler.TryEndConnect(ar);
                if (res == null) { Disconnect(); return; }

				ConnectStatus = EConnectStatus.Connect;

				var arg = new ConnectStatusChangedEventArgs()
				{
					ConnectStatus = EConnectStatus.Connect,
					RemoteInfo = new EndPointInfo(socket.RemoteEndPoint as IPEndPoint),
					StatusChangedTime = DateTime.Now,
				};

				new WaitTask.WaitTask(() => ConnectStatusChangedEvent?.Invoke(this, arg)).Start();

				// 建立資料接收器
				var state = new StateObject();
                state.workSocket = handler;
                res = handler.TryBeginReceive(state.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(AsyncReceiveCallback), state);
                if (res == null) Disconnect();
            }
        }

        /// <summary>
        /// 資料接收器
        /// </summary>
        private void AsyncReceiveCallback(IAsyncResult ar)
        {
            lock (key)
            {
                var state = ar.AsyncState as StateObject;
                var handler = state.workSocket;

                int readLength = handler.TryEndReceive(ar);
                if (readLength == 0) { Disconnect(); return; }

                var readData = new byte[readLength];
                Array.Copy(state.buffer, 0, readData, 0, readLength);
                var arg = new ReceivedDataEventArgs
                {
                    Data = readData,
                    RemoteInfo = new EndPointInfo(handler.RemoteEndPoint as IPEndPoint),
                    ReceivedTime = DateTime.Now,
                };

                new WaitTask.WaitTask(() => ReceivedDataEvent?.Invoke(this, arg)).Start();
                var res = handler.TryBeginReceive(state.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(AsyncReceiveCallback), state);
                if (res == null) Disconnect();
            }
        }

        /// <summary>
        /// 資料發送完畢
        /// </summary>
        private void AsyncSendCallback(IAsyncResult ar)
        {
            lock (key)
            {
                var handler = ar.AsyncState as Socket;

                int sendLength = handler.TryEndSend(ar);
                if (sendLength == 0) Disconnect();
            }
        }

        /// <summary>
        /// 連線狀態改變事件，使用 Task 發布
        /// </summary>
        public event ConnectStatusChangedEvent ConnectStatusChangedEvent;

        /// <summary>
        /// 從遠端接收到資料的事件，使用 Task 發布
        /// </summary>
        public event ReceivedDataEvent ReceivedDataEvent;

        /// <summary>
        /// 連線狀態
        /// </summary>
        public EConnectStatus ConnectStatus { get; private set; }

        /// <summary>
        /// 遠端位址
        /// </summary>
        public string RemoteIP { get; private set; }

        /// <summary>
        /// 遠端埠號
        /// </summary>
        public int RemotePort { get; private set; }

        /// <summary>
        /// 連線
        /// </summary>
        public void Connect(string ip, int port)
        {
            lock (key)
            {
                if (ConnectStatus == EConnectStatus.Connect) return;

                RemoteIP = ip;
                RemotePort = port;
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.SendBufferSize = StateObject.BUFFER_SIZE;
				socket.ReceiveBufferSize = StateObject.BUFFER_SIZE;
				socket.IOControl(IOControlCode.KeepAliveValues, StandardOperate.GetKeepAliveSetting(), null);

                var res = socket.TryBeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), new AsyncCallback(AsyncConnectCallback), socket);
                if (res == null) { Disconnect(); return; }
            }
        }

        /// <summary>
        /// 斷線
        /// </summary>
        public void Disconnect()
        {
            lock (key)
            {
                if (ConnectStatus == EConnectStatus.Disconnect) return;

                ConnectStatus = EConnectStatus.Disconnect;

                socket?.TryClose();
                socket = null;

                var arg = new ConnectStatusChangedEventArgs()
                {
                    ConnectStatus = EConnectStatus.Disconnect,
                    RemoteInfo = new EndPointInfo(RemoteIP, RemotePort),
                    StatusChangedTime = DateTime.Now,
                };

                new WaitTask.WaitTask(() => ConnectStatusChangedEvent?.Invoke(this, arg)).Start();
            }
        }

        /// <summary>
        /// 發送資料至遠端
        /// </summary>
        public void Send(byte[] data)
        {
            lock (key)
            {
                if (ConnectStatus == EConnectStatus.Disconnect) return;

                var res = socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(AsyncSendCallback), socket);
                if (res == null) Disconnect();
            }
        }

        /// <summary>
        /// 發送資料至遠端
        /// </summary>
        public void Send(string data)
        {
            lock (key)
            {
                if (ConnectStatus == EConnectStatus.Disconnect) return;

                Send(Encoding.ASCII.GetBytes(data));
            }
        }

        #region Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (socket != null)
                {
                    socket.Dispose();
                }

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

        ~Client()
        {
            Dispose(false);
        }

        #endregion Dispose
    }
}
