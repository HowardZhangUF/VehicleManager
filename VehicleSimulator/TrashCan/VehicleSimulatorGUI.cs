using Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using VehicleSimulator.Interface;

namespace VehicleSimulator.UserInterface
{
	public partial class VehicleSimulatorGUI : Form
	{
		public int dgvVehicleStateHeight = 0;
		public bool showVehicleStateDetail = false;

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
					tmpPoints.Add(new Point2D(int.Parse(coordinate[0]), int.Parse(coordinate[1])));
				}
				Path = tmpPoints;
				result = true;
			}
			return result;
		}
		private bool GetCoordinate(string Src, out string X, out string Y)
		{
			bool result = false;
			X = string.Empty;
			Y = string.Empty;

			string[] point = Src.Split(new string[] { ",", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
			if (point.Length == 2)
			{
				X = point[0];
				Y = point[1];
				result = true;
			}
			return result;
		}
		private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
		{
			try
			{
				UpdateGui_InitializeDgvVehicleState();
				UpdateGui_ToolStripItemText(statusStrip1, statusLabelLocation, $"{mCore.GetVehicleState().mName} ({mCore.GetVehicleState().mPosition.mX},{mCore.GetVehicleState().mPosition.mY},{mCore.GetVehicleState().mToward.ToString("F2")})");
				UpdateGui_UpdateDataGridViewColumnsValue(dgvVehicleState, 1, mCore.GetVehicleState().ToStringArray());
				dgvVehicleStateHeight = dgvVehicleState.Height;
				showVehicleStateDetail = true;

				UpdateGui_DisplayVehicleStateDetail(false);
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
		private void UpdateGui_ToolStripItemText(Control ParentContainer, ToolStripItem Control, string Text)
		{
			ParentContainer.InvokeIfNecessary(() => Control.Text = Text);
		}
		private void UpdateGui_ControlBackColor(Control Control, Color BackColor)
		{
			Control.InvokeIfNecessary((c) => c.BackColor = BackColor);
		}
		private void UpdateGui_InitializeDgvVehicleState()
		{
			DataGridView dgv = dgvVehicleState;

			dgv.RowHeadersVisible = false;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToResizeRows = false;
			dgv.AllowUserToResizeColumns = false;
			dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dgv.MultiSelect = false;

			dgv.Columns.Add("Keyword", "Keyword");
			dgv.Columns[0].Width = 200;
			dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dgv.Columns.Add("Value", "Value");
			dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
				column.ReadOnly = true;
			}

			dgv.Rows.Add("Name");
			dgv.Rows.Add("State");
			dgv.Rows.Add("Position");
			dgv.Rows.Add("Toward");
			dgv.Rows.Add("Target");
			dgv.Rows.Add("Buffer Target");
			dgv.Rows.Add("Translation Velocity");
			dgv.Rows.Add("Rotation Velocity");
			dgv.Rows.Add("Map Match");
			dgv.Rows.Add("Battery");
			dgv.Rows.Add("Path Blocked");
			dgv.Rows.Add("Alarm Message");
			dgv.Rows.Add("Safety Frame Radius");
			dgv.Rows.Add("Is Intervene Available");
			dgv.Rows.Add("Is Being Intervened");
			dgv.Rows.Add("Intervene Command");
			dgv.Rows.Add("Path");
		}
		private void UpdateGui_UpdateDataGridViewColumnsValue(DataGridView Dgv, int ColIdx, string[] Values)
		{
			Dgv.InvokeIfNecessary(() =>
			{
				if (Values != null && Dgv.Rows.Count == Values.Length && Dgv.Columns.Count > ColIdx)
				{
					for (int i = 0; i < Dgv.Rows.Count; ++i)
					{
						if (Dgv.Rows[i].Cells[ColIdx].Value?.ToString() != Values[i])
						{
							Dgv.Rows[i].Cells[ColIdx].Value = Values[i];
						}
					}
				}
			});
		}
		private void UpdateGui_DisplayVehicleStateDetail(bool Display)
		{
			this.InvokeIfNecessary(() =>
			{
				if (Display != showVehicleStateDetail)
				{
					if (Display)
					{
						UpdateGui_ToolStripItemBackColor(menuStrip1, menuShowVehicleStateDetail, Color.LightGreen);
						UpdateGui_ToolStripItemText(menuStrip1, menuShowVehicleStateDetail, "Hide Detail");
						Height += dgvVehicleStateHeight;
						showVehicleStateDetail = true;
					}
					else
					{
						UpdateGui_ToolStripItemBackColor(menuStrip1, menuShowVehicleStateDetail, Color.LightPink);
						UpdateGui_ToolStripItemText(menuStrip1, menuShowVehicleStateDetail, "Show Detail");
						Height -= dgvVehicleStateHeight;
						showVehicleStateDetail = false;
					}
				}
			});
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
		public void VehicleSimulatorIntervene(string Command, params string[] Paras)
		{
			mCore.VehicleSimulatorInfoSetInterveneCommand(Command, Paras);
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
			UpdateGui_ToolStripItemText(statusStrip1, statusLabelLocation, $"{VehicleSimulatorInfo.mName} ({VehicleSimulatorInfo.mPosition.mX},{VehicleSimulatorInfo.mPosition.mY},{VehicleSimulatorInfo.mToward.ToString("F2")})");
			UpdateGui_UpdateDataGridViewColumnsValue(dgvVehicleState, 1, VehicleSimulatorInfo.ToStringArray());
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
		private void menuHostDisconnect_Click(object sender, EventArgs e)
		{
			mCore.CommunicatorClientStopConnect();
		}
		private void menuShowVehicleStateDetail_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayVehicleStateDetail(!showVehicleStateDetail);
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
		private void btnVehicleSimulatorInterveneInsert_Click(object sender, EventArgs e)
		{
			if (GetCoordinate(txtInterveneParameter.Text, out string X, out string Y))
			{
				VehicleSimulatorIntervene("InsertMovingBuffer", X, Y);
			}
		}
		private void btnVehicleSimulatorInterveneCancelInsert_Click(object sender, EventArgs e)
		{
			VehicleSimulatorIntervene("RemoveMovingBuffer");
		}
		private void btnVehicleSimulatorIntervenePause_Click(object sender, EventArgs e)
		{
			VehicleSimulatorIntervene("PauseMoving");
		}
		private void btnVehicleSimulatorInterveneResume_Click(object sender, EventArgs e)
		{
			VehicleSimulatorIntervene("ResumeMoving");
		}
	}
}
