using System;
using System.Net;
using System.Net.Sockets;

namespace AsyncSocket
{
    /// <summary>
    /// 標準操作錯
    /// </summary>
    public static class StandardOperate
    {
        /// <summary>
        /// 嘗試關閉 <see cref="Socket"/> 連接並釋放所有相關資源。
        /// </summary>
        public static void TryClose(this Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
            }

            try
            {
                socket.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 嘗試結束非同步連線要求，若失敗則回傳 null
        /// </summary>
        public static IAsyncResult TryEndConnect(this Socket handler, IAsyncResult ar)
        {
            try
            {
                handler.EndConnect(ar);
                return ar;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 建立 keepalive 作業所需的輸入資料
        /// </summary>
        /// <param name="onOff">是否啟用1:on ,0:off</param>
        /// <param name="keepAliveTime">當沒收到client的ack時，等待多久才通知斷線(millisecond)</param>
        /// <param name="keepAliveInterval">偵測間隔(millisecond)</param>
        /// </summary>
        public static byte[] GetKeepAliveSetting(int OnOff = 1, int KeepAliveTime = 1000, int KeepAliveInterval = 1000)
        {
            var buffer = new byte[12];
            BitConverter.GetBytes(OnOff).CopyTo(buffer, 0);
            BitConverter.GetBytes(KeepAliveTime).CopyTo(buffer, 4);
            BitConverter.GetBytes(KeepAliveInterval).CopyTo(buffer, 8);

            return buffer;
        }

        /// <summary>
        /// 取得本地位址 ，若本地不存在，則回傳 <see cref="string.Empty"/>
        /// </summary>
        public static string GetLocalIP(this Socket socket)
        {
            var local = socket?.LocalEndPoint as IPEndPoint;
            return local?.Address.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 取得本地 IP:Port ，若本地不存在，則回傳 <see cref="string.Empty"/>
        /// </summary>
        public static string GetLocalIPPort(this Socket socket)
        {
            var local = socket?.LocalEndPoint as IPEndPoint;
            if (local == null) return string.Empty;
            return local.Address.ToString() + ":" + local.Port.ToString();
        }

        /// <summary>
        /// 取得本地埠號 ，若本地不存在，則回傳 -1
        /// </summary>
        public static int GetLocalPort(this Socket socket)
        {
            var local = socket?.LocalEndPoint as IPEndPoint;
            return local?.Port ?? -1;
        }

        /// <summary>
        /// 取得遠端位址 ，若遠端不存在，則回傳 <see cref="string.Empty"/>
        /// </summary>
        public static string GetRemoteIP(this Socket socket)
        {
            var remote = socket?.RemoteEndPoint as IPEndPoint;
            return remote?.Address.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 取得遠端 IP:Port ，若遠端不存在，則回傳 <see cref="string.Empty"/>
        /// </summary>
        public static string GetRemoteIPPort(this Socket socket)
        {
            var remote = socket?.RemoteEndPoint as IPEndPoint;
            if (remote == null) return string.Empty;
            return remote.Address.ToString() + ":" + remote.Port.ToString();
        }

        /// <summary>
        /// 取得遠端埠號 ，若遠端不存在，則回傳 -1
        /// </summary>
        public static int GetRemotePort(this Socket socket)
        {
            var remote = socket?.RemoteEndPoint as IPEndPoint;
            return remote?.Port ?? -1;
        }

        /// <summary>
        /// 嘗試開始非同步接受連線，若失敗則回傳 null
        /// </summary>
        public static IAsyncResult TryBeginAccept(this Socket socket, AsyncCallback callback, object state)
        {
            try
            {
                return socket.BeginAccept(callback, state);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 嘗試開始非同步連線，若失敗則回傳 null
        /// </summary>
        public static IAsyncResult TryBeginConnect(this Socket handler, EndPoint remoteEP, AsyncCallback callback, object state)
        {
            try
            {
                return handler.BeginConnect(remoteEP, callback, state);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 嘗試開始非同步接收，若失敗則回傳 null
        /// </summary>
        public static IAsyncResult TryBeginReceive(this Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            try
            {
                return socket.BeginReceive(buffer, offset, size, socketFlags, callback, state);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 嘗試開始非同步發送資料，若失敗則回傳 null
        /// </summary>
        public static IAsyncResult TryBeginSend(this Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            try
            {
                return socket.BeginSend(buffer, offset, size, socketFlags, callback, state);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 嘗試建立監聽服務器(Handler)，若建立失敗回傳 null 表示無法監聽
        /// </summary>
        public static Socket TryEndAccept(this Socket listener, IAsyncResult ar)
        {
            try
            {
                return listener.EndAccept(ar);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 嘗試結束非同步接收，若失敗則回傳 0
        /// </summary>
        public static int TryEndReceive(this Socket handler, IAsyncResult ar)
        {
            try
            {
                return handler.EndReceive(ar);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 嘗試結束非同步發送，若失敗則回傳 0
        /// </summary>
        public static int TryEndSend(this Socket handler, IAsyncResult ar)
        {
            try
            {
                return handler.EndSend(ar);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
