using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LibraryForVM;

namespace VehicleSimulator
{
	public partial class UcSimulatorInfo : UserControl
	{
		private SimulatorProcess rSimulatorProcess = null;
		private ISimulatorInfo rSimulatorInfo = null;
		private ISimulatorControl rSimulatorControl = null;
		private IMoveRequestCalculator rMoveRequestCalculator = null;
		private IHostCommunicator rHostCommunicator = null;

		public UcSimulatorInfo()
		{
			InitializeComponent();
			UpdateGui_InitializeDgvSimulatorInfo();
		}
		public UcSimulatorInfo(SimulatorProcess SimulatorProcess)
		{
			InitializeComponent();
			UpdateGui_InitializeDgvSimulatorInfo();
			Set(SimulatorProcess);
		}
		public void Set(SimulatorProcess SimulatorProcess)
		{
			UnsubscribeEvent_SimulatorProcess(rSimulatorProcess);
			rSimulatorProcess = SimulatorProcess;
			SubscribeEvent_SimulatorProcess(rSimulatorProcess);
			UpdateGui_UpdateSimulatorInfo();
		}
		public string GetCurrentSimulatorName()
		{
			return lblSimulatorName.Text;
		}

		protected void Set(ISimulatorInfo SimulatorInfo)
		{
			UnsubscribeEvent_ISimulatorInfo(rSimulatorInfo);
			rSimulatorInfo = SimulatorInfo;
			SubscribeEvent_ISimulatorInfo(rSimulatorInfo);
		}
		protected void Set(ISimulatorControl SimulatorControl)
		{
			UnsubscribeEvent_ISimulatorControl(rSimulatorControl);
			rSimulatorControl = SimulatorControl;
			SubscribeEvent_ISimulatorControl(rSimulatorControl);
		}
		protected void Set(IMoveRequestCalculator MoveRequestCalculator)
		{
			UnsubscribeEvent_IMoveRequestCalculator(rMoveRequestCalculator);
			rMoveRequestCalculator = MoveRequestCalculator;
			SubscribeEvent_IMoveRequestCalculator(rMoveRequestCalculator);
		}
		protected void Set(IHostCommunicator HostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = HostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}

		private void btnSimulatorConnect_Click(object sender, EventArgs e)
		{
			if (rHostCommunicator != null)
			{
				rHostCommunicator.SetConfig("RemoteIpPort", txtHostIpPort.Text);
				if (rHostCommunicator.mIsConnected)
				{
					rHostCommunicator.Disconnect();
				}
				else
				{
					rHostCommunicator.Connect();
				}
			}
		}
		private void btnSimulatorMove_Click(object sender, EventArgs e)
		{
			if (rSimulatorControl != null)
			{
				if (cbMoveTarget.SelectedItem != null)
				{
					string targetString = cbMoveTarget.Text;
					int firstBracketsIndex = targetString.LastIndexOf('(');
					int lastBracketsIndex = targetString.LastIndexOf(')');
					string targetName = targetString.Substring(0, firstBracketsIndex - 1);
					string locationString = targetString.Substring(firstBracketsIndex + 1, lastBracketsIndex - firstBracketsIndex - 1);
					string[] locationSplitString = locationString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
					var moveRequests = rMoveRequestCalculator.Calculate(new Point(rSimulatorInfo.mX, rSimulatorInfo.mY), targetName);
					rSimulatorControl.StartMove(targetName, moveRequests);
				}
				else
				{
					string[] tmpStrings = cbMoveTarget.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
					if (tmpStrings.Length == 2)
					{
						var moveRequests = rMoveRequestCalculator.Calculate(new Point(rSimulatorInfo.mX, rSimulatorInfo.mY), new Point(int.Parse(tmpStrings[0]), int.Parse(tmpStrings[1])));
						rSimulatorControl.StartMove(cbMoveTarget.Text, moveRequests);
					}
					else if (tmpStrings.Length == 3)
					{
						var moveRequests = rMoveRequestCalculator.Calculate(new Point(rSimulatorInfo.mX, rSimulatorInfo.mY), new Point(int.Parse(tmpStrings[0]), int.Parse(tmpStrings[1])), int.Parse(tmpStrings[2]));
						rSimulatorControl.StartMove(cbMoveTarget.Text, moveRequests);
					}
				}
			}
		}
		private void btnSimulatorStop_Click(object sender, EventArgs e)
		{
			if (rSimulatorControl != null)
			{
				rSimulatorControl.StopMove();
			}
		}
		private void btnSimulatorSetLocation_Click(object sender, EventArgs e)
		{
			if (cbGoalList.Items.Count > 0 && cbGoalList.SelectedItem != null)
			{
				// GoalName (X,Y,Toward)
				string selectedItemString = cbGoalList.SelectedItem.ToString();
				int firstBracketsIndex = selectedItemString.LastIndexOf('(');
				int lastBracketsIndex = selectedItemString.LastIndexOf(')');
				string locationString = selectedItemString.Substring(firstBracketsIndex + 1, lastBracketsIndex - firstBracketsIndex - 1);
				string[] locationSplitString = locationString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
				rSimulatorInfo.SetLocation(int.Parse(locationSplitString[0]), int.Parse(locationSplitString[1]), int.Parse(locationSplitString[2]));
			}
		}
		private void dgvSimulatorInfo_SelectionChanged(object sender, EventArgs e)
		{
			if (dgvSimulatorInfo.CurrentCell.ColumnIndex == dgvSimulatorInfo.Columns["ItemKey1"].Index || dgvSimulatorInfo.CurrentCell.ColumnIndex == dgvSimulatorInfo.Columns["ItemKey2"].Index)
			{
				dgvSimulatorInfo.ClearSelection();
			}
			if (dgvSimulatorInfo.CurrentCell.RowIndex == 0 && dgvSimulatorInfo.CurrentCell.ColumnIndex == 1)
			{
				dgvSimulatorInfo.ClearSelection();
			}
			if (dgvSimulatorInfo.CurrentCell.RowIndex == 0 && dgvSimulatorInfo.CurrentCell.ColumnIndex == 3)
			{
				dgvSimulatorInfo.ClearSelection();
			}
			if (dgvSimulatorInfo.CurrentCell.RowIndex == 1 && dgvSimulatorInfo.CurrentCell.ColumnIndex == 3)
			{
				dgvSimulatorInfo.ClearSelection();
			}
		}
		private void dgvSimulatorInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (string.IsNullOrEmpty(GetCurrentSimulatorName())) return;

			string cellValue = dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
			if (e.RowIndex == 1 && e.ColumnIndex == 1) // X
			{
				if (int.TryParse(cellValue, out int newX))
				{
					rSimulatorInfo.SetLocation(newX, rSimulatorInfo.mY, rSimulatorInfo.mToward);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mX;
				}
			}
			else if (e.RowIndex == 2 && e.ColumnIndex == 1) // Y
			{
				if (int.TryParse(cellValue, out int newY))
				{
					rSimulatorInfo.SetLocation(rSimulatorInfo.mX, newY, rSimulatorInfo.mToward);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mY;
				}
			}
			else if (e.RowIndex == 3 && e.ColumnIndex == 1) // Toward
			{
				if (int.TryParse(cellValue, out int newToward))
				{
					rSimulatorInfo.SetLocation(rSimulatorInfo.mX, rSimulatorInfo.mY, newToward);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mToward;
				}
			}
			else if (e.RowIndex == 4 && e.ColumnIndex == 1) // Translate Velocity
			{
				if (int.TryParse(cellValue, out int newTranslateVelocity))
				{
					rSimulatorInfo.SetTranslateVelocity(newTranslateVelocity);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mTranslateVelocity;
				}
			}
			else if (e.RowIndex == 2 && e.ColumnIndex == 3) // Score
			{
				if (double.TryParse(cellValue, out double newScore))
				{
					rSimulatorInfo.SetScore(newScore);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mScore.ToString("F2");
				}
			}
			else if (e.RowIndex == 3 && e.ColumnIndex == 3) // Battery
			{
				if (double.TryParse(cellValue, out double newBattery))
				{
					rSimulatorInfo.SetBattery(newBattery);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mBattery.ToString("F2");
				}
			}
			else if (e.RowIndex == 4 && e.ColumnIndex == 3) // Rotate Velocity
			{
				if (int.TryParse(cellValue, out int newRotateVelocity))
				{
					rSimulatorInfo.SetRotateVelocity(newRotateVelocity);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mRotateVelocity;
				}
			}
		}
		private void SubscribeEvent_SimulatorProcess(SimulatorProcess rSimulatorProcess)
		{
			if (rSimulatorProcess != null)
			{
				Set(rSimulatorProcess.GetReferenceOfISimulatorInfo());
				Set(rSimulatorProcess.GetReferenceOfISimulatorControl());
				Set(rSimulatorProcess.GetReferenceOfIMoveRequestCalculator());
				Set(rSimulatorProcess.GetReferenceOfIHostCommunicator());
			}
		}
		private void UnsubscribeEvent_SimulatorProcess(SimulatorProcess rSimulatorProcess)
		{
			if (rSimulatorProcess != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_ISimulatorInfo(ISimulatorInfo SimulatorInfo)
		{
			if (SimulatorInfo != null)
			{
				SimulatorInfo.StatusUpdated += HandleEvent_SimulatorInfoStatusUpdated;
			}
		}
		private void UnsubscribeEvent_ISimulatorInfo(ISimulatorInfo SimulatorInfo)
		{
			if (SimulatorInfo != null)
			{
				SimulatorInfo.StatusUpdated -= HandleEvent_SimulatorInfoStatusUpdated;
			}
		}
		private void SubscribeEvent_ISimulatorControl(ISimulatorControl SimulatorControl)
		{
			if (SimulatorControl != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_ISimulatorControl(ISimulatorControl SimulatorControl)
		{
			if (SimulatorControl != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMoveRequestCalculator(IMoveRequestCalculator MoveRequestCalculator)
		{
			if (MoveRequestCalculator != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IMoveRequestCalculator(IMoveRequestCalculator MoveRequestCalculator)
		{
			if (MoveRequestCalculator != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.ConnectStateChanged += HandleEvent_HostCommunicatorConnectStateChanged;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.ConnectStateChanged -= HandleEvent_HostCommunicatorConnectStateChanged;
			}
		}
		private void HandleEvent_SimulatorInfoStatusUpdated(object sender, StatusUpdatedEventArgs e)
		{
			if (e.StatusName.Contains("Status"))
			{
				UpdateGui_SetSimulatorStatus(rSimulatorInfo.mStatus.ToString());
			}
			else if (e.StatusName.Contains("X") || e.StatusName.Contains("Y") || e.StatusName.Contains("Toward"))
			{
				UpdateGui_SetSimulatorLocation(rSimulatorInfo.mX, rSimulatorInfo.mY, rSimulatorInfo.mToward);
			}
			else if (e.StatusName.Contains("Target"))
			{
				UpdateGui_SetSimulatorTarget(rSimulatorInfo.mTarget);
			}
			else if (e.StatusName.Contains("Score"))
			{
				UpdateGui_SetSimulatorScore(rSimulatorInfo.mScore);
			}
			else if (e.StatusName.Contains("Battery"))
			{
				UpdateGui_SetSimulatorBattery(rSimulatorInfo.mBattery);
			}
			else if (e.StatusName.Contains("TranslateVelocity"))
			{
				UpdateGui_SetSimulatorTranslateVelocity(rSimulatorInfo.mTranslateVelocity);
			}
			else if (e.StatusName.Contains("RotateVelocity"))
			{
				UpdateGui_SetSimulatorRotationVelocity(rSimulatorInfo.mRotateVelocity);
			}
			else if (e.StatusName.Contains("MapData"))
			{
				UpdateGui_SetSimulatorGoalList(rSimulatorInfo.mMapData);
			}
		}
		private void HandleEvent_HostCommunicatorConnectStateChanged(object sender, ConnectStateChangedEventArgs e)
		{
			UpdateGui_SetSimulatorIsConnect(rHostCommunicator.mIsConnected);
			UpdateGui_SetSimulatorHostIpPort(rHostCommunicator.GetConfig("RemoteIpPort"));
		}
		private void UpdateGui_InitializeDgvSimulatorInfo()
		{
			DataGridView dgv = dgvSimulatorInfo;

			dgv.RowHeadersVisible = false;
			dgv.ColumnHeadersVisible = false;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToResizeRows = false;
			dgv.AllowUserToResizeColumns = false;
			dgv.MultiSelect = false;
			dgv.GridColor = Color.FromArgb(41, 41, 41);

			dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
			dgv.DefaultCellStyle.BackColor = Color.FromArgb(67, 67, 67);
			dgv.DefaultCellStyle.ForeColor = Color.White;
			dgv.RowTemplate.Height = 30;

			dgv.Columns.Add("ItemKey1", "ItemKey1");
			dgv.Columns[0].Width = 450 / 4;
			dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgv.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(54, 54, 54);
			dgv.Columns[0].ReadOnly = true;
			dgv.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
			dgv.Columns.Add("ItemValue1", "ItemValue1");
			dgv.Columns[1].Width = 450 / 4;
			dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dgv.Columns[1].ReadOnly = true;
			dgv.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
			dgv.Columns.Add("ItemKey2", "ItemKey2");
			dgv.Columns[2].Width = 450 / 4;
			dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgv.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(54, 54, 54);
			dgv.Columns[2].ReadOnly = true;
			dgv.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
			dgv.Columns.Add("ItemValue2", "ItemValue2");
			dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dgv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dgv.Columns[3].ReadOnly = true;
			dgv.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			dgv.Rows.Add("Name    ", "", "Status    ", "");
			dgv.Rows.Add("X    ", "0", "Target    ", "");
			dgv.Rows.Add("Y    ", "0", "Score    ", "0.00");
			dgv.Rows.Add("Head    ", "0", "Battery    ", "0.00");
			dgv.Rows.Add("TranslateVelocity    ", "800", "RotateVelocity    ", "90");
			dgv.ClearSelection();

			dgv.Rows[1].Cells[1].ReadOnly = false;
			dgv.Rows[2].Cells[1].ReadOnly = false;
			dgv.Rows[2].Cells[3].ReadOnly = false;
			dgv.Rows[3].Cells[1].ReadOnly = false;
			dgv.Rows[3].Cells[3].ReadOnly = false;
			dgv.Rows[4].Cells[1].ReadOnly = false;
			dgv.Rows[4].Cells[3].ReadOnly = false;
		}
		private void UpdateGui_UpdateSimulatorInfo()
		{
			if (rSimulatorProcess != null && rHostCommunicator != null)
			{
				UpdateGui_SetSimulatorName(rSimulatorInfo.mName);
				UpdateGui_SetSimulatorStatus(rSimulatorInfo.mStatus.ToString());
				UpdateGui_SetSimulatorLocation(rSimulatorInfo.mX, rSimulatorInfo.mY, rSimulatorInfo.mToward);
				UpdateGui_SetSimulatorTarget(rSimulatorInfo.mTarget);
				UpdateGui_SetSimulatorScore(rSimulatorInfo.mScore);
				UpdateGui_SetSimulatorBattery(rSimulatorInfo.mBattery);
				UpdateGui_SetSimulatorTranslateVelocity(rSimulatorInfo.mTranslateVelocity);
				UpdateGui_SetSimulatorRotationVelocity(rSimulatorInfo.mRotateVelocity);
				UpdateGui_SetSimulatorIsConnect(rHostCommunicator.mIsConnected);
				UpdateGui_SetSimulatorHostIpPort(rHostCommunicator.GetConfig("RemoteIpPort"));
				UpdateGui_SetSimulatorGoalList(rSimulatorInfo.mMapData);
			}
			else
			{
				UpdateGui_SetSimulatorName(string.Empty);
				UpdateGui_SetSimulatorStatus(string.Empty);
				UpdateGui_SetSimulatorLocation(default(int), default(int), default(int));
				UpdateGui_SetSimulatorTarget(string.Empty);
				UpdateGui_SetSimulatorScore(default(double));
				UpdateGui_SetSimulatorBattery(default(double));
				UpdateGui_SetSimulatorTranslateVelocity(default(int));
				UpdateGui_SetSimulatorRotationVelocity(default(int));
				UpdateGui_SetSimulatorIsConnect(default(bool));
				UpdateGui_SetSimulatorHostIpPort(string.Empty);
				UpdateGui_SetSimulatorGoalList(null);
			}
		}
		private void UpdateGui_SetSimulatorName(string Value)
		{
			lblSimulatorName.InvokeIfNecessary(() => { lblSimulatorName.Text = Value; });
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[0].Cells[1].Value = Value; });
		}
		private void UpdateGui_SetSimulatorStatus(string Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[0].Cells[3].Value = Value; });
		}
		private void UpdateGui_SetSimulatorLocation(int X, int Y, int Toward)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[1].Cells[1].Value = X; });
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[2].Cells[1].Value = Y; });
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[3].Cells[1].Value = Toward; });
		}
		private void UpdateGui_SetSimulatorTarget(string Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[1].Cells[3].Value = Value; });
		}
		private void UpdateGui_SetSimulatorScore(double Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[2].Cells[3].Value = Value.ToString("F2"); });
		}
		private void UpdateGui_SetSimulatorBattery(double Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[3].Cells[3].Value = Value.ToString("F2"); });
		}
		private void UpdateGui_SetSimulatorTranslateVelocity(int Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[4].Cells[1].Value = Value; });
		}
		private void UpdateGui_SetSimulatorRotationVelocity(int Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[4].Cells[3].Value = Value; });
		}
		private void UpdateGui_SetSimulatorIsConnect(bool Value)
		{
			if (Value == true)
			{
				btnSimulatorConnect.InvokeIfNecessary(() => { btnSimulatorConnect.BackColor = Color.FromArgb(0, 163, 0); });
			}
			else
			{
				btnSimulatorConnect.InvokeIfNecessary(() => { btnSimulatorConnect.BackColor = Color.FromArgb(163, 0, 0); });
			}
		}
		private void UpdateGui_SetSimulatorHostIpPort(string Value)
		{
			txtHostIpPort.InvokeIfNecessary(() => { txtHostIpPort.Text = Value; });
		}
		private void UpdateGui_SetSimulatorGoalList(MapData MapData)
		{
			if (MapData != null)
			{
				cbMoveTarget.InvokeIfNecessary(() =>
				{
					cbMoveTarget.SelectedIndex = -1;
					cbMoveTarget.SelectedItem = null;
					cbMoveTarget.Items.Clear();
				});
				cbGoalList.InvokeIfNecessary(() =>
				{
					cbGoalList.SelectedIndex = -1;
					cbGoalList.SelectedItem = null;
					cbGoalList.Items.Clear();
				});
				if (MapData.mGoals != null && MapData.mGoals.Count > 0)
				{
					string[] goalStrings = MapData.mGoals.OrderBy(o => o.mName).Select(o => $"{o.mName} ({o.mX},{o.mY},{o.mToward})").ToArray();
					cbMoveTarget.InvokeIfNecessary(() => { cbMoveTarget.Items.AddRange(goalStrings); });
					cbGoalList.InvokeIfNecessary(() => { cbGoalList.Items.AddRange(goalStrings); });
				}
			}
		}
	}
}
