using AsyncSocket;
using Serialization;
using System;
using System.Text;
using System.Windows.Forms;
using static LogManager.LogManager;

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
                Log.StatusChange.Add(str);
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
            server.ReceivedSerialDataEvent += ReceivedSerialDataEvent; ;

            client.ConnectStatusChangedEvent += Client_ConnectStatusChangedEvent;
            client.ReceivedSerialDataEvent += ReceivedSerialDataEvent; ;
        }

        private void ReceivedSerialDataEvent(object sender, ReceivedSerialDataEventArgs e)
        {
            if (e.Data is StringMessage)
                AddMessage(e.ReceivedTime, $"{e.RemoteInfo} 發送字串過來 >> {(e.Data as StringMessage).Message}");
            if (e.Data is ByteArray)
                AddMessage(e.ReceivedTime, $"{e.RemoteInfo} 發送陣列過來 >> {Encoding.Unicode.GetString((e.Data as ByteArray).Message)}");
        }

        #region Server

        private readonly SerialServer server = new SerialServer();

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
            if (chkServerSendByBytes.Checked)
            {
                server.Send(remote, Encoding.Unicode.GetBytes(data));
            }
            else // send by string
            {
                server.Send(remote, data);
            }
        }

        private void Server_ConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
        {
            AddMessage(e.StatusChangedTime, $"和 {e.RemoteInfo} 的連線狀態改變 >> {e.ConnectStatus}");
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
            AddMessage(e.StatusChangedTime, $"監聽狀態改變 >> {e.ListenStatus}");
            btnListening.InvokeIfNecessary(() => btnListening.Text = e.ListenStatus.ToString());
        }

        #endregion Server

        #region Client

        private readonly SerialClient client = new SerialClient();

        private void btnClientSend_Click(object sender, EventArgs e)
        {
            string data = txtClientSendData.Text;
            if (chkClientSendByBytes.Checked)
            {
                client.Send(Encoding.Unicode.GetBytes(data));
            }
            else // send by string
            {
                client.Send(data);
            }
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
            AddMessage(e.StatusChangedTime, $"連線狀態改變 >> {e.ConnectStatus}");
            btnConnect.InvokeIfNecessary(() => btnConnect.Text = e.ConnectStatus.ToString());
        }

        #endregion Client

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.SaveAll();
        }
    }
}
