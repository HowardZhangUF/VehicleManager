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
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using VehicleSimulator.Interface;

namespace VehicleSimulator.UserInterface
{
	public partial class VehicleSimulatorGUI : Form
	{
		public VehicleSimulatorGUI()
		{
			InitializeComponent();
			Constructor();
		}
		private void Constructor()
		{
			Constructor_VehicleManagerProcess();
		}
		private void Destructor()
		{
			Destructor_VehicleManagerProcess();
		}
		private void HandleException(Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
		}
		private bool GetIpPort(string Src, out string Ip, out int Port)
		{
			bool result = false;
			Ip = string.Empty;
			Port = 0;

			if (Src.Contains(":"))
			{
				string[] tmp = Src.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
				if (tmp.Length == 2)
				{
					Ip = tmp[0];
					Port = int.Parse(tmp[1]);
					result = true;
				}
			}
			return result;
		}
		private bool GetPath(string Src, out IEnumerable<IPoint2D> Path)
		{
			bool result = false;
			Path = null;

			string[] points = Src.Split(new string[] { "\r\n", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
			if (points.Length > 0)
			{
				List<IPoint2D> tmpPoints = new List<IPoint2D>();
				foreach (string point in points)
				{
					string[] coordinate = point.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
					tmpPoints.Add(TrafficControlTest.Library.Library.GenerateIPoint2D(int.Parse(coordinate[0]), int.Parse(coordinate[1])));
				}
				Path = tmpPoints;
				result = true;
			}
			return result;
		}
		private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
		{
			try
			{

			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}
		private void VehicleSimulatorGUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				Destructor();
			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}

		private void UpdateGui_ToolStripItemBackColor(Control ParentContainer, ToolStripItem Control, Color BackColor)
		{
			ParentContainer.InvokeIfNecessary(() => Control.BackColor = BackColor);
		}
		private void UpdateGui_ControlBackColor(Control Control, Color BackColor)
		{
			Control.InvokeIfNecessary((c) => c.BackColor = BackColor);
		}

		#region VehicleSimulatorProcess
		private Base.VehicleSimulatorProcess mCore;

		public void HostConnect(string Ip, int Port)
		{
			mCore.CommunicatorClientStartConnect(Ip, Port);
		}
		public void HostDisconnect()
		{
			mCore.CommunicatorClientStopConnect();
		}
		public void VehicleSimulatorStateReportSystemStart()
		{
			mCore.VehicleStateReporterStart();
		}
		public void VehicleSimulatorStateReportSystemStop()
		{
			mCore.VehicleStateReporterStop();
		}
		public void VehicleSimulatorStartMove(IEnumerable<IPoint2D> Path)
		{
			mCore.VehicleSimulatorInfoStartMove(Path);
		}
		public void VehicleSimulatorStopMove()
		{
			mCore.VehicleSimulatorInfoStopMove();
		}
		public void VehicleSimulatorPauseMove()
		{
			mCore.VehicleSimulatorInfoPauseMove();
		}
		public void VehicleSimulatorResumeMove()
		{
			mCore.VehicleSimulatorInfoResumeMove();
		}

		private void Constructor_VehicleManagerProcess()
		{
			UnsubscribeEvent_VehicleSimulatorProcess(mCore);
			mCore = new Base.VehicleSimulatorProcess();
			SubscribeEvent_VehicleSimulatorProcess(mCore);

		}
		private void Destructor_VehicleManagerProcess()
		{
			UnsubscribeEvent_VehicleSimulatorProcess(mCore);
			mCore = null;
		}
		private void SubscribeEvent_VehicleSimulatorProcess(Base.VehicleSimulatorProcess VehicleSimulatorProcess)
		{
			if (VehicleSimulatorProcess != null)
			{
				VehicleSimulatorProcess.VehicleSimulatorInfoStateUpdated += HandleEvent_VehicleSimulatorProcessVehicleSimulatorInfoStateUpdated;
				VehicleSimulatorProcess.CommunicatorClientSystemStarted += HandleEvent_VehicleSimulatorProcessCommunicatorClientSystemStarted;
				VehicleSimulatorProcess.CommunicatorClientSystemStopped += HandleEvent_VehicleSimulatorProcessCommunicatorClientSystemStopped;
				VehicleSimulatorProcess.CommunicatorClientConnectStateChanged += HandleEvent_VehicleSimulatorProcessCommunicatorClientConnectStateChanged;
				VehicleSimulatorProcess.CommunicatorClientSentSerializableData += HandleEvent_VehicleSimulatorProcessCommunicatorClientSentSerializableData;
				VehicleSimulatorProcess.CommunicatorClientReceivedSerializableData += HandleEvent_VehicleSimulatorProcessCommunicatorClientReceivedSerializableData;
				VehicleSimulatorProcess.VehicleStateReporterSystemStarted += HandleEvent_VehicleSimulatorProcessVehicleStateReporterSystemStarted;
				VehicleSimulatorProcess.VehicleStateReporterSystemStopped += HandleEvent_VehicleSimulatorProcessVehicleStateReporterSystemStopped;
			}
		}
		private void UnsubscribeEvent_VehicleSimulatorProcess(Base.VehicleSimulatorProcess VehicleSimulatorProcess)
		{
			if (VehicleSimulatorProcess != null)
			{
				VehicleSimulatorProcess.VehicleSimulatorInfoStateUpdated -= HandleEvent_VehicleSimulatorProcessVehicleSimulatorInfoStateUpdated;
				VehicleSimulatorProcess.CommunicatorClientSystemStarted -= HandleEvent_VehicleSimulatorProcessCommunicatorClientSystemStarted;
				VehicleSimulatorProcess.CommunicatorClientSystemStopped -= HandleEvent_VehicleSimulatorProcessCommunicatorClientSystemStopped;
				VehicleSimulatorProcess.CommunicatorClientConnectStateChanged -= HandleEvent_VehicleSimulatorProcessCommunicatorClientConnectStateChanged;
				VehicleSimulatorProcess.CommunicatorClientSentSerializableData -= HandleEvent_VehicleSimulatorProcessCommunicatorClientSentSerializableData;
				VehicleSimulatorProcess.CommunicatorClientReceivedSerializableData -= HandleEvent_VehicleSimulatorProcessCommunicatorClientReceivedSerializableData;
				VehicleSimulatorProcess.VehicleStateReporterSystemStarted -= HandleEvent_VehicleSimulatorProcessVehicleStateReporterSystemStarted;
				VehicleSimulatorProcess.VehicleStateReporterSystemStopped -= HandleEvent_VehicleSimulatorProcessVehicleStateReporterSystemStopped;
			}
		}
		private void HandleEvent_VehicleSimulatorProcessVehicleSimulatorInfoStateUpdated(DateTime OccurTime, string Name, IVehicleSimulatorInfo VehicleSimulatorInfo)
		{

		}
		private void HandleEvent_VehicleSimulatorProcessCommunicatorClientSystemStarted(DateTime OccurTime)
		{

		}
		private void HandleEvent_VehicleSimulatorProcessCommunicatorClientSystemStopped(DateTime OccurTime)
		{

		}
		private void HandleEvent_VehicleSimulatorProcessCommunicatorClientConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			UpdateGui_ToolStripItemBackColor(statusStrip1, statusLabelHostConnectState, NewState == ConnectState.Connected ? Color.LightGreen : Color.LightPink);
			UpdateGui_ToolStripItemBackColor(menuStrip1, menuHostConnection, NewState == ConnectState.Connected ? Color.LightGreen : Color.LightPink);
		}
		private void HandleEvent_VehicleSimulatorProcessCommunicatorClientSentSerializableData(DateTime OccurTime, string IpPort, object Data)
		{

		}
		private void HandleEvent_VehicleSimulatorProcessCommunicatorClientReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{

		}
		private void HandleEvent_VehicleSimulatorProcessVehicleStateReporterSystemStarted(DateTime OccurTime)
		{

		}
		private void HandleEvent_VehicleSimulatorProcessVehicleStateReporterSystemStopped(DateTime OccurTime)
		{

		}
		#endregion

		private void menuHostConnect_Click(object sender, EventArgs e)
		{
			if (GetIpPort(menuHostIpPort.Text, out string Ip, out int Port))
			{
				mCore.CommunicatorClientStartConnect(Ip, Port);
			}
		}
		private void btnVehicleSimulatorStartMove_Click(object sender, EventArgs e)
		{
			if (GetPath(txtVehicleSimulatorPath.Text, out IEnumerable<IPoint2D> Path))
			{
				VehicleSimulatorStartMove(Path);
			}
		}
		private void btnVehicleSimulatorStopMove_Click(object sender, EventArgs e)
		{
			VehicleSimulatorStopMove();
		}
		private void btnVehicleSimulatorPauseMove_Click(object sender, EventArgs e)
		{
			VehicleSimulatorPauseMove();
		}
		private void btnVehicleSimulatorResumeMove_Click(object sender, EventArgs e)
		{
			VehicleSimulatorResumeMove();
		}
	}
}
