using AsyncSocket;
using System;
using System.Text;
using System.Windows.Forms;

namespace LittleGhost
{
    public partial class frmMain : Form
    {
        private readonly object key = new object();

        private string message = string.Empty;

        private void AddMessage(DateTime timeMark, string str)
        {
            lock (key)
            {
                message = $"{timeMark.ToString("hh:mm:ss.fff")}: {str}\r\n" + message;
            }
        }

        private void tUpdateMessage_Tick(object sender, EventArgs e)
        {
            lock (key)
            {
                txtMessage.Text = message;
            }
        }

        public frmMain()
        {
            InitializeComponent();
            server.ConnectStatusChangedEvent += Server_ConnectStatusChangedEvent;
            server.ListenStatusChangedEvent += Server_ListenStatusChangedEvent;
            server.ReceivedDataEvent += Server_ReceivedDataEvent;

            client.ConnectStatusChangedEvent += Client_ConnectStatusChangedEvent;
            client.ReceivedDataEvent += Client_ReceivedDataEvent;
        }

        #region Server

        private readonly Server server = new Server();

        private void btnListening_Click(object sender, EventArgs e)
        {
            switch (server.ListenStatus)
            {
                case EListenStatus.Idle:
                    server.StartListening("127.0.0.1", (int)nmrServerPort.Value);
                    break;

                case EListenStatus.Listening:
                    server.StopListen();
                    break;

                default:
                    break;
            }
        }

        private void btnServerSend_Click(object sender, EventArgs e)
        {
            string remote = cmbRemoteList.Text;
            string data = txtServerSendData.Text;
            server.Send(remote, data);
        }

        private void Server_ConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
        {
            AddMessage(e.StatusChangedTime, $"{e.RemoteInfo} >> {e.ConnectStatus}");
            switch (e.ConnectStatus)
            {
                case EConnectStatus.Disconnect:
                    cmbRemoteList.InvokeIfNecessary(() => cmbRemoteList.Items.Remove(e.RemoteInfo.ToString()));
                    break;

                case EConnectStatus.Connect:
                    cmbRemoteList.InvokeIfNecessary(() => cmbRemoteList.Items.Add(e.RemoteInfo.ToString()));
                    break;

                default:
                    break;
            }
        }

        private void Server_ListenStatusChangedEvent(object sender, ListenStatusChangedEventArgs e)
        {
            AddMessage(e.StatusChangedTime, $"Server >> {e.ListenStatus}");
            btnListening.InvokeIfNecessary(() => btnListening.Text = e.ListenStatus.ToString());
        }

        private void Server_ReceivedDataEvent(object sender, ReceivedDataEventArgs e)
        {
            AddMessage(e.ReceivedTime, $"{e.RemoteInfo} >> {Encoding.ASCII.GetString(e.Data)}");
        }

        #endregion Server

        #region Client

        private readonly Client client = new Client();

        private void btnClientSend_Click(object sender, EventArgs e)
        {
            string data = txtClientSendData.Text;
            client.Send(data);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            switch (client.ConnectStatus)
            {
                case EConnectStatus.Disconnect:
                    client.Connect(txtClientIP.Text, (int)nmrClientPort.Value);
                    break;

                case EConnectStatus.Connect:
                    client.Disconnect();
                    break;

                default:
                    break;
            }
        }


        private void Client_ConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
        {
            AddMessage(e.StatusChangedTime, $"Client >> {e.ConnectStatus}");
            btnConnect.InvokeIfNecessary(() => btnConnect.Text = e.ConnectStatus.ToString());
        }

        private void Client_ReceivedDataEvent(object sender, ReceivedDataEventArgs e)
        {
            AddMessage(e.ReceivedTime, $"{e.RemoteInfo} >> {Encoding.ASCII.GetString(e.Data)}");
        }

        #endregion Client
    }
}
