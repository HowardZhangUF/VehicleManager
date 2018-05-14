using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsyncSocket
{
    /// <summary>
    /// 非同步通訊伺服端
    /// </summary>
    public class Server
    {
        /// <summary>
        /// 監聽事件服務器字典集合
        /// </summary>
        private readonly Dictionary<string, Socket> handlerDic = new Dictionary<string, Socket>();

        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 通訊界面
        /// </summary>
        private Socket socket;

        /// <summary>
        /// 將監聽服務器加入列表中，並開始接收資料。
        /// 若列表中已經存在相同的 IP:Port 則先移除舊的連線後再新增
        /// </summary>
        private void AddNewHandler(Socket handler)
        {
            lock (key)
            {
                string ipport = handler.GetRemoteIPPort();
                if (ipport == string.Empty) return;

                // 加至列表
                Disconnect(ipport);
                handlerDic.Add(ipport, handler);

                // 建立資料接收器
                var state = new StateObject();
                state.workSocket = handler;
                var res = handler.TryBeginReceive(state.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(AsyncReceiveCallback), state);
                if (res == null) { Disconnect(handler); return; }

                var arg = new ConnectStatusChangedEventArgs()
                {
                    ConnectStatus = EConnectStatus.Connect,
                    RemoteInfo = new EndPointInfo(handler.RemoteEndPoint as IPEndPoint),
                    StatusChangedTime = DateTime.Now,
                };

                new WaitTask(() => ConnectStatusChangedEvent?.Invoke(this, arg)).Start();
            }
        }

        /// <summary>
        /// 接受連線
        /// </summary>
        private void AsyncAcceptCallback(IAsyncResult ar)
        {
            lock (key)
            {
                if (ListenStatus != EListenStatus.Listening) return;

                var listener = ar.AsyncState as Socket;
                var handler = listener.TryEndAccept(ar);
                if (handler == null) { Disconnect(handler); return; }

                AddNewHandler(handler);
                var res = socket.TryBeginAccept(new AsyncCallback(AsyncAcceptCallback), socket);
                if (res == null) StopListen();
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

                if (!handlerDic.Values.Contains(handler) || !handler.Connected) return;

                int readLength = handler.TryEndReceive(ar);
                if (readLength == 0) { Disconnect(handler); return; }

                var readData = new byte[readLength];
                Array.Copy(state.buffer, 0, readData, 0, readLength);
                var arg = new ReceivedDataEventArgs
                {
                    Data = readData,
                    RemoteInfo = new EndPointInfo(handler.RemoteEndPoint as IPEndPoint),
                    ReceivedTime = DateTime.Now,
                };

                new WaitTask(() => ReceivedDataEvent?.Invoke(this, arg)).Start();
                var res = handler.TryBeginReceive(state.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(AsyncReceiveCallback), state);
                if (res == null) Disconnect(handler);
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

                if (!handlerDic.Values.Contains(handler) || !handler.Connected) return;

                int sendLength = handler.TryEndSend(ar);
                if (sendLength == 0) Disconnect(handler);
            }
        }

        /// <summary>
        /// 連線狀態改變事件，使用 Task 發布
        /// </summary>
        public event ConnectStatusChangedEvent ConnectStatusChangedEvent;

        /// <summary>
        /// 監聽狀態改變事件，使用 Task 發布
        /// </summary>
        public event ListenStatusChangedEvent ListenStatusChangedEvent;

        /// <summary>
        /// 從遠端接收到資料的事件，使用 Task 發布
        /// </summary>
        public event ReceivedDataEvent ReceivedDataEvent;

        /// <summary>
        /// 監聽狀態
        /// </summary>
        public EListenStatus ListenStatus { get; private set; }

        /// <summary>
        /// 切斷連線
        /// </summary>
        public void Disconnect(Socket handler)
        {
            lock (key)
            {
                if (handlerDic.Values.Contains(handler))
                {
                    string ipport = string.Empty;
                    foreach (var item in handlerDic)
                    {
                        if (item.Value == handler)
                        {
                            ipport = item.Key;
                            break;
                        }
                    }
                    Disconnect(ipport);
                }
            }
        }

        /// <summary>
        /// 切斷連線
        /// </summary>
        public void Disconnect(string ipport)
        {
            lock (key)
            {
                if (handlerDic.Keys.Contains(ipport))
                {
                    var handler = handlerDic[ipport];
                    var arg = new ConnectStatusChangedEventArgs()
                    {
                        ConnectStatus = EConnectStatus.Disconnect,
                        RemoteInfo = new EndPointInfo(handler.RemoteEndPoint as IPEndPoint),
                        StatusChangedTime = DateTime.Now,
                    };

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    handlerDic.Remove(ipport);

                    new WaitTask(() => ConnectStatusChangedEvent?.Invoke(this, arg)).Start();
                }
            }
        }

        /// <summary>
        /// 切斷所有連線
        /// </summary>
        public void Disconnect()
        {
            lock (key)
            {
                while (handlerDic.Any())
                {
                    Disconnect(handlerDic.First().Key);
                }
            }
        }

        /// <summary>
        /// 發送資料至指定遠端
        /// </summary>
        public void Send(string ipport, byte[] data)
        {
            lock (key)
            {
                if (handlerDic.Keys.Contains(ipport))
                {
                    var handler = handlerDic[ipport];
                    var res = handler.TryBeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(AsyncSendCallback), handler);
                    if (res == null) Disconnect(handler);
                }
            }
        }

        /// <summary>
        /// 發送資料至指定遠端
        /// </summary>
        public void Send(string ipport, string data)
        {
            lock (key)
            {
                Send(ipport, Encoding.ASCII.GetBytes(data));
            }
        }

        /// <summary>
        /// 發送資料至所有遠端
        /// </summary>
        public void Send(byte[] data)
        {
            lock (key)
            {
                List<Socket> disconnectList = new List<Socket>();
                foreach (var handler in handlerDic.Values)
                {
                    var res = handler.TryBeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(AsyncSendCallback), handler);
                    if (res == null) disconnectList.Add(handler);
                }

                foreach (var handler in disconnectList)
                {
                    Disconnect(handler);
                }
            }
        }

        /// <summary>
        /// 發送資料至所有遠端
        /// </summary>
        public void Send(string data)
        {
            lock (key)
            {
                Send(Encoding.ASCII.GetBytes(data));
            }
        }

        /// <summary>
        /// 開始監聽連線
        /// </summary>
        public void StartListening(string ip, int port)
        {
            lock (key)
            {
                if (ListenStatus == EListenStatus.Listening) return;

                ListenStatus = EListenStatus.Listening;

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                socket.Listen(100);
                socket.IOControl(IOControlCode.KeepAliveValues, StandardOperate.GetKeepAliveSetting(), null);

                var res = socket.TryBeginAccept(new AsyncCallback(AsyncAcceptCallback), socket);
                if (res == null) { StopListen(); return; }

                var arg = new ListenStatusChangedEventArgs()
                {
                    ListenStatus = EListenStatus.Listening,
                    StatusChangedTime = DateTime.Now,
                };

                new WaitTask(() => ListenStatusChangedEvent?.Invoke(this, arg)).Start();
            }
        }

        /// <summary>
        /// 停止監聽
        /// </summary>
        public void StopListen()
        {
            lock (key)
            {
                if (ListenStatus == EListenStatus.Idle) return;

                ListenStatus = EListenStatus.Idle;

                Disconnect();
                socket?.Close();
                socket = null;

                var arg = new ListenStatusChangedEventArgs()
                {
                    ListenStatus = EListenStatus.Idle,
                    StatusChangedTime = DateTime.Now,
                };

                new WaitTask(() => ListenStatusChangedEvent?.Invoke(this, arg)).Start();
            }
        }
    }
}
