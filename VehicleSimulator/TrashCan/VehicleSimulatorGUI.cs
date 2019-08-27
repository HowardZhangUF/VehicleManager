using Geometry;
using GLCore;
using GLStyle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static VehicleSimulator.VehicleSimulator;

namespace VehicleSimulator
{
	public partial class VehicleSimulatorGUI : Form
	{
		private VehicleSimulatorProcess process = new VehicleSimulatorProcess();

		public VehicleSimulatorGUI()
		{
			InitializeComponent();

			StyleManager.LoadStyle("Style.ini");
		}

		private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
		{
			SubscribeVehicleSimulatorProcessEvent();

			List<Pair> path1 = new List<Pair>();
			//path1.Add(new Pair(3000, -5000));
			path1.Add(new Pair(-10000, 1000));

			List<Pair> path2 = new List<Pair>();
			//path2.Add(new Pair(3000, 4000));
			path2.Add(new Pair(-1000, -9000));

			process.AddVehicleSimualtor("AGV01", 500, 40, 7000, -1000);
			process.AddVehicleSimualtor("AGV02", 500, 40, 1000, 9000);
			process.VehicleSimulatorMove("AGV01", path1);
			process.VehicleSimulatorMove("AGV02", path2);
		}

		private void VehicleSimulatorGUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			UnsubscribeVehicleSimulatorProcessEvent();
		}

		private void btnConnectRemote_Click(object sender, EventArgs e)
		{
			if (!process.IsCommunicationAlive)
				process.StartCommunication(txtRemoteIP.Text, int.Parse(txtRemotePort.Text));
			else
				process.StopCommunication();
		}

		private void chkVehicleSimulator_CheckedChanged(object sender, EventArgs e)
		{
			process.DisplayVehicleSimulatorDebugMessage = chkVehicleSimulator.Checked;
			rtxtDebugMessage.Focus();
		}

		private void chkConsoleCommunicator_CheckedChanged(object sender, EventArgs e)
		{
			process.DisplayConsoleCommunicatorDebugMessage = chkConsoleCommunicator.Checked;
			rtxtDebugMessage.Focus();
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// 切換分頁到 DebugMessage 時，自動 Focus 到 RichTextbox 上，好讓其能自動捲動
			if (tabControl1.SelectedTab == tpDebugMsg)
				rtxtDebugMessage.Focus();
		}

		private void cmenuItemClearRichTextbox_Click(object sender, EventArgs e)
		{
			rtxtDebugMessage.Clear();
		}

		#region Map Process

		Dictionary<string, int> VehicleIconIDs = new Dictionary<string, int>();

		Dictionary<string, int> VehiclePathIconIDs = new Dictionary<string, int>();

		private void AddAGVIcon(string name)
		{
			if (!VehicleIconIDs.Keys.Contains(name) && !VehiclePathIconIDs.Keys.Contains(name))
			{
				VehicleIconIDs.Add(name, GLCMD.CMD.SerialNumber.Next());
				GLCMD.CMD.AddAGV(VehicleIconIDs[name], name);

				int pathIconID = GLCMD.CMD.AddMultiStripLine("PathLine", null);
				VehiclePathIconIDs.Add(name, pathIconID);
			}
		}

		private void RemoveAGVIcon(string name)
		{
			if (VehicleIconIDs.Keys.Contains(name) && VehiclePathIconIDs.Keys.Contains(name))
			{
				GLCMD.CMD.DeleteAGV(VehicleIconIDs[name]);
				GLCMD.CMD.DeleteMulti(VehiclePathIconIDs[name]);
				VehicleIconIDs.Remove(name);
				VehiclePathIconIDs.Remove(name);
			}
		}

		private void UpdateAGVIcon(string name, int x, int y, double toward)
		{
			if (VehicleIconIDs.Keys.Contains(name))
			{
				GLCMD.CMD.AddAGV(VehicleIconIDs[name], name, x, y, toward);
			}
		}

		private void UpdateAGVPathIcon(string name, List<Pair> path)
		{
			if (VehiclePathIconIDs.Keys.Contains(name))
			{
				GLCMD.CMD.SaftyEditMultiGeometry<IPair>(VehiclePathIconIDs[name], true, (line) =>
				{
					line.Clear();
					line.AddRangeIfNotNull(path);
				});
			}
		}

		#endregion

		#region Main Process

		private void SubscribeVehicleSimulatorProcessEvent()
		{
			process.VehicleSimulatorAdded += Process_VehicleSimulatorAdded;
			process.VehicleSimulatorRemoved += Process_VehicleSimulatorRemoved;
			process.VehicleSimulatorPositionChanged += Process_VehicleSimulatorPositionChanged;
			process.VehicleSimulatorStatusChanged += Process_VehicleSimulatorStatusChanged;
			process.ConsoleConnectStatusChanged += Process_ConsoleConnectStatusChanged;
			process.ConsoleReportStarted += Process_ConsoleReportStarted;
			process.ConsoleReportStopped += Process_ConsoleReportStopped;
			process.DebugMessage += Process_DebugMessage;
		}

		private void UnsubscribeVehicleSimulatorProcessEvent()
		{
			process.VehicleSimulatorAdded -= Process_VehicleSimulatorAdded;
			process.VehicleSimulatorRemoved -= Process_VehicleSimulatorRemoved;
			process.VehicleSimulatorPositionChanged -= Process_VehicleSimulatorPositionChanged;
			process.VehicleSimulatorStatusChanged -= Process_VehicleSimulatorStatusChanged;
			process.ConsoleConnectStatusChanged -= Process_ConsoleConnectStatusChanged;
			process.ConsoleReportStarted -= Process_ConsoleReportStarted;
			process.ConsoleReportStopped -= Process_ConsoleReportStopped;
			process.DebugMessage -= Process_DebugMessage;
		}

		private void Process_VehicleSimulatorAdded(string name)
		{
			AddAGVIcon(name);
		}

		private void Process_VehicleSimulatorRemoved(string name)
		{
			RemoveAGVIcon(name);
		}

		private void Process_VehicleSimulatorPositionChanged(string name, TowardPair position, List<Pair> path)
		{
			UpdateAGVIcon(name, position.Position.X, position.Position.Y, position.Toward.Theta);
			List<Pair> tmp = path.DeepClone();
			tmp.Insert(0, new Pair(position.Position.X, position.Position.Y));
			UpdateAGVPathIcon(name, tmp);
		}

		private void Process_VehicleSimulatorStatusChanged(string name, VehicleStatus status)
		{
		}

		private void Process_ConsoleConnectStatusChanged(DateTime occurTime, AsyncSocket.EndPointInfo remoteInfo, AsyncSocket.EConnectStatus newStatus)
		{
			if (newStatus == AsyncSocket.EConnectStatus.Connect)
				btnConnectRemote.InvokeIfNecessary(() => btnConnectRemote.BackColor = Color.LightGreen);
			else
				btnConnectRemote.InvokeIfNecessary(() => btnConnectRemote.BackColor = Color.LightPink);
		}

		private void Process_ConsoleReportStarted()
		{
		}

		private void Process_ConsoleReportStopped()
		{
		}

		private void Process_DebugMessage(DateTime timeStamp, string category, string message)
		{
			rtxtDebugMessage.InvokeIfNecessary(() =>
			{
				if (chkRtxtDebugMsgAutoScroll.Checked)
					rtxtDebugMessage.AppendText(new DebugMessage(timeStamp, category, message).ToString() + "\n");
				else
					rtxtDebugMessage.Text += new DebugMessage(timeStamp, category, message).ToString() + "\n";
			});
		}

		#endregion
	}
}
