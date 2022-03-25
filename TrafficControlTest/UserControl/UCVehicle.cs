using LibraryForVM;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.UserControl
{
	public partial class UcVehicle : System.Windows.Forms.UserControl
	{
		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableGridLineColor { get; set; } = Color.FromArgb(86, 86, 86);
		public Color TableHeaderBackColor { get; set; } = Color.FromArgb(0, 122, 204);
		public Color TableHeaderForeColor { get; set; } = Color.White;
		public Color TableRowBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableRowForeColor { get; set; } = Color.White;
		public Color TableRowExecutingBackColor { get; set; } = Color.FromArgb(35, 128, 76);

		private IVehicleInfoManager rVehicleInfoManager = null;
        private IVehicleControlManager rVehicleControlManager = null;
		private object mLockOfDgvVehicleInfo = new object();
		private object mLockOfDgvVehicleControl = new object();
        private int mDgvVehicleInfoRightClickRowIndex { get; set; } = -1;
        private int mDgvVehicleControlRightClickRowIndex { get; set; } = -1;

        public UcVehicle()
		{
			InitializeComponent();
			UpdateGui_DgvVehicleInfo_Initialize();
			UpdateGui_DgvVehicleControl_Initialize();
        }
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
		}
        public void Set(IVehicleControlManager VehicleControlManager)
        {
            UnsubscribeEvent_VehicleControlManager(rVehicleControlManager);
            rVehicleControlManager = VehicleControlManager;
            SubscribeEvent_VehicleControlManager(rVehicleControlManager);
        }
        public void Set(IVehicleInfoManager VehicleInfoManager, IVehicleControlManager VehicleControlManager)
        {
            Set(VehicleInfoManager);
            Set(VehicleControlManager);
        }
		public void SetVehicleControlVisible(bool Visible)
		{
			if (tableLayoutPanel1.RowCount == 0) return;

			if (Visible)
			{
				tableLayoutPanel1.RowStyles[1].Height = 50;
				tableLayoutPanel1.RowStyles[0].Height = 50;
			}
			else
			{
				tableLayoutPanel1.RowStyles[1].Height = 0;
				tableLayoutPanel1.RowStyles[0].Height = 100;
			}
		}

		private void SubscribeEvent_VehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_VehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
        }
        private void SubscribeEvent_VehicleControlManager(IVehicleControlManager VehicleControlManager)
        {
            if (VehicleControlManager != null)
            {
                VehicleControlManager.ItemAdded += HandleEvent_VehicleControlManagerItemAdded;
                VehicleControlManager.ItemRemoved += HandleEvent_VehicleControlManagerItemRemoved;
                VehicleControlManager.ItemUpdated += HandleEvent_VehicleControlManagerItemUpdated;
            }
        }
        private void UnsubscribeEvent_VehicleControlManager(IVehicleControlManager VehicleControlManager)
        {
            if (VehicleControlManager != null)
            {
                VehicleControlManager.ItemAdded -= HandleEvent_VehicleControlManagerItemAdded;
                VehicleControlManager.ItemRemoved -= HandleEvent_VehicleControlManagerItemRemoved;
                VehicleControlManager.ItemUpdated -= HandleEvent_VehicleControlManagerItemUpdated;
            }
        }
        private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_DgvVehicleInfo_AddItem(Args.Item);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_DgvVehicleInfo_RemoveItem(Args.Item);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_DgvVehicleInfo_UpdateItem(Args.Item, Args.StatusName);
        }
        private void HandleEvent_VehicleControlManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleControl> Args)
        {
            UpdateGui_DgvVehicleControl_AddItem(Args.Item);
        }
        private void HandleEvent_VehicleControlManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleControl> Args)
        {
            UpdateGui_DgvVehicleControl_RemoveItem(Args.Item);
        }
        private void HandleEvent_VehicleControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleControl> Args)
        {
            UpdateGui_DgvVehicleControl_UpdateItem(Args.Item, Args.StatusName);
        }
        private void UpdateGui_DgvVehicleInfo_Initialize()
		{
			DataGridView dgv = dgvVehicleInfo;

			dgv.SelectionChanged += ((sender, e) => dgv.ClearSelection());

			dgv.RowHeadersVisible = false;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToResizeRows = false;
			dgv.AllowUserToResizeColumns = false;
			dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv.MultiSelect = false;
			dgv.BackgroundColor = TableBackColor;
			dgv.GridColor = TableGridLineColor;
			dgv.BorderStyle = BorderStyle.None;

			dgv.EnableHeadersVisualStyles = false;
			dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold);
			dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dgv.ColumnHeadersDefaultCellStyle.BackColor = TableHeaderBackColor;
			dgv.ColumnHeadersDefaultCellStyle.ForeColor = TableHeaderForeColor;
			dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dgv.ColumnHeadersHeight = 60;

			dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
			dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
			dgv.DefaultCellStyle.BackColor = TableRowBackColor;
			dgv.DefaultCellStyle.ForeColor = TableRowForeColor;
			dgv.RowTemplate.Height = 40;

			dgv.Columns.Add("Name", "Name");
			dgv.Columns[0].Width = 180;
			dgv.Columns.Add("State", "State");
			dgv.Columns[1].Width = 140;
			dgv.Columns.Add("Location", "Location");
			dgv.Columns[2].Width = 170;
			dgv.Columns.Add("Target", "Target");
			dgv.Columns[3].Width = 170;
			dgv.Columns.Add("Battery", "Battery");
			dgv.Columns[4].Width = 80;
			dgv.Columns.Add("LocationScore", "LocationScore");
			dgv.Columns[5].Width = 120;
            dgv.Columns.Add("MissionID", "MissionID");
            dgv.Columns[6].Width = 210;
			dgv.Columns.Add("InterveneCommand", "InterveneCommand");
			dgv.Columns[7].Width = 210;
			dgv.Columns.Add("InterveneCause", "InterveneCause");
			dgv.Columns[8].Width = 200;
			dgv.Columns.Add("ErrorMessage", "ErrorMessage");
            dgv.Columns[9].Width = 140;
            dgv.Columns.Add("MapName", "MapName");
			dgv.Columns[10].Width = 240;
			dgv.Columns.Add("IpPort", "IPPort");
			dgv.Columns[11].Width = 180;
			dgv.Columns.Add("LastUpdate", "LastUpdate");
			dgv.Columns[12].Width = 190;
			dgv.Columns.Add("FillColumn", "");
			dgv.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
				column.ReadOnly = true;
			}
		}
		private void UpdateGui_DgvVehicleInfo_AddItem(IVehicleInfo Item)
		{
			dgvVehicleInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvVehicleInfo)
				{
					if (dgvVehicleInfo.RowCount == 0)
					{
						dgvVehicleInfo.Rows.Add(new string[] { Item.mName, Item.mCurrentState, $"({Item.mLocationCoordinate.mX},{Item.mLocationCoordinate.mY},{(int)Item.mLocationToward})", Item.mCurrentTarget, Item.mBatteryValue.ToString("F2") + " %", Item.mLocationScore.ToString("F2") + " %", Item.mCurrentMissionId, Item.mCurrentInterveneCommand, Item.mCurrentInterveneCause, Item.mErrorMessage, Item.mCurrentMapName, Item.mIpPort, Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT) });
					}
					else
					{
						int newItemRowIndex = -1;
						for (int i = 0; i < dgvVehicleInfo.RowCount; ++i)
						{
							if (string.Compare(Item.mName, dgvVehicleInfo.Rows[i].Cells["Name"].Value.ToString()) == -1)
							{
								newItemRowIndex = i;
								break;
							}
						}

						if (newItemRowIndex != -1)
						{
							dgvVehicleInfo.Rows.Insert(newItemRowIndex, new string[] { Item.mName, Item.mCurrentState, $"({Item.mLocationCoordinate.mX},{Item.mLocationCoordinate.mY},{(int)Item.mLocationToward})", Item.mCurrentTarget, Item.mBatteryValue.ToString("F2") + " %", Item.mLocationScore.ToString("F2") + " %", Item.mCurrentMissionId, Item.mCurrentInterveneCommand, Item.mCurrentInterveneCause, Item.mErrorMessage, Item.mCurrentMapName, Item.mIpPort, Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT) });
						}
						else
						{
							dgvVehicleInfo.Rows.Add(new string[] { Item.mName, Item.mCurrentState, $"({Item.mLocationCoordinate.mX},{Item.mLocationCoordinate.mY},{(int)Item.mLocationToward})", Item.mCurrentTarget, Item.mBatteryValue.ToString("F2") + " %", Item.mLocationScore.ToString("F2") + " %", Item.mCurrentMissionId, Item.mCurrentInterveneCommand, Item.mCurrentInterveneCause, Item.mErrorMessage, Item.mCurrentMapName, Item.mIpPort, Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT) });
						}
					}
				}
			});
		}
		private void UpdateGui_DgvVehicleInfo_RemoveItem(IVehicleInfo Item)
		{
			dgvVehicleInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvVehicleInfo)
				{
					int rowIndex = -1;
					for (int i = 0; i < dgvVehicleInfo.RowCount; ++i)
					{
						if (string.Compare(Item.mName, dgvVehicleInfo.Rows[i].Cells["Name"].Value.ToString()) == 0)
						{
							rowIndex = i;
							break;
						}
					}
					if (rowIndex != -1)
					{
						dgvVehicleInfo.Rows.RemoveAt(rowIndex);
					}
				}
			});
		}
		private void UpdateGui_DgvVehicleInfo_UpdateItem(IVehicleInfo Item, string StateName)
		{
			dgvVehicleInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvVehicleInfo)
				{
					int rowIndex = -1;
					for (int i = 0; i < dgvVehicleInfo.RowCount; ++i)
					{
						if (string.Compare(Item.mName, dgvVehicleInfo.Rows[i].Cells["Name"].Value.ToString()) == 0)
						{
							rowIndex = i;
							break;
						}
					}
					if (rowIndex != -1)
					{
						if (StateName.Contains("CurrentState"))
						{
							dgvVehicleInfo.Rows[rowIndex].Cells["State"].Value = Item.mCurrentState;
						}
						if (StateName.Contains("LocationCoordinate") || StateName.Contains("LocationToward"))
						{
							dgvVehicleInfo.Rows[rowIndex].Cells["Location"].Value = $"({Item.mLocationCoordinate.mX},{Item.mLocationCoordinate.mY},{(int)Item.mLocationToward})";
						}
						if (StateName.Contains("CurrentTarget"))
						{
							dgvVehicleInfo.Rows[rowIndex].Cells["Target"].Value = Item.mCurrentTarget;
						}
						if (StateName.Contains("BatteryValue"))
						{
							dgvVehicleInfo.Rows[rowIndex].Cells["Battery"].Value = Item.mBatteryValue.ToString("F2") + " %";
						}
						if (StateName.Contains("LocationScore"))
						{
							dgvVehicleInfo.Rows[rowIndex].Cells["LocationScore"].Value = Item.mLocationScore.ToString("F2") + " %";
						}
                        if (StateName.Contains("CurrentMissionId"))
                        {
                            dgvVehicleInfo.Rows[rowIndex].Cells["MissionID"].Value = Item.mCurrentMissionId;
						}
						if (StateName.Contains("CurrentInterveneCommand"))
						{
							dgvVehicleInfo.Rows[rowIndex].Cells["InterveneCommand"].Value = Item.mCurrentInterveneCommand;
						}
						if (StateName.Contains("CurrentInterveneCause"))
						{
							dgvVehicleInfo.Rows[rowIndex].Cells["InterveneCause"].Value = Item.mCurrentInterveneCause;
						}
						if (StateName.Contains("ErrorMessage"))
                        {
                            dgvVehicleInfo.Rows[rowIndex].Cells["ErrorMessage"].Value = Item.mErrorMessage;
                        }
                        if (StateName.Contains("CurrentMapName"))
						{
							dgvVehicleInfo.Rows[rowIndex].Cells["MapName"].Value = Item.mCurrentMapName;
						}
						if (StateName.Contains("IpPort"))
						{
							dgvVehicleInfo.Rows[rowIndex].Cells["IpPort"].Value = Item.mIpPort;
						}
						dgvVehicleInfo.Rows[rowIndex].Cells["LastUpdate"].Value = Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT);
					}
				}
			});
        }
        private void UpdateGui_DgvVehicleControl_Initialize()
        {
            DataGridView dgv = dgvVehicleControl;

            dgv.SelectionChanged += ((sender, e) => dgv.ClearSelection());

            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.BackgroundColor = TableBackColor;
            dgv.GridColor = TableGridLineColor;
            dgv.BorderStyle = BorderStyle.None;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = TableHeaderBackColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TableHeaderForeColor;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 60;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
            dgv.DefaultCellStyle.BackColor = TableRowBackColor;
            dgv.DefaultCellStyle.ForeColor = TableRowForeColor;
            dgv.RowTemplate.Height = 40;

            dgv.Columns.Add("Name", "Name");
            dgv.Columns[0].Width = 210;
            dgv.Columns.Add("VehicleId", "VehicleID");
            dgv.Columns[1].Width = 180;
			dgv.Columns.Add("Command", "Command");
			dgv.Columns[2].Width = 140;
			dgv.Columns.Add("Parameter", "Parameter");
            dgv.Columns[3].Width = 170;
            dgv.Columns.Add("CauseId", "CauseID");
            dgv.Columns[4].Width = 210;
            dgv.Columns.Add("SendState", "SendState");
            dgv.Columns[5].Width = 130;
            dgv.Columns.Add("ExecuteState", "ExecuteState");
            dgv.Columns[6].Width = 130;
            dgv.Columns.Add("FailedReason", "FailedReason");
            dgv.Columns[7].Width = 180;
            dgv.Columns.Add("LastUpdate", "LastUpdate");
            dgv.Columns[8].Width = 190;
            dgv.Columns.Add("FillColumn", "");
            dgv.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }
        private void UpdateGui_DgvVehicleControl_AddItem(IVehicleControl Item)
        {
            dgvVehicleControl.InvokeIfNecessary(() =>
            {
                lock (mLockOfDgvVehicleControl)
                {
                    dgvVehicleControl.Rows.Add(new string[] { Item.mName, Item.mVehicleId, Item.mCommand.ToString(), Item.mParametersString, Item.mCauseId, Item.mSendState.ToString(), Item.mExecuteState.ToString(), Item.mFailedReason.ToString(), Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT) });
					if (dgvVehicleControl.Rows[dgvVehicleControl.RowCount - 1].Cells["ExecuteState"].Value.ToString() == ExecuteState.Executing.ToString()) dgvVehicleControl.Rows[dgvVehicleControl.RowCount - 1].DefaultCellStyle.BackColor = TableRowExecutingBackColor;
				}
			});
        }
        private void UpdateGui_DgvVehicleControl_RemoveItem(IVehicleControl Item)
        {
            dgvVehicleControl.InvokeIfNecessary(() =>
            {
                lock (mLockOfDgvVehicleControl)
                {
					int rowIndex = -1;
					for (int i = 0; i < dgvVehicleControl.RowCount; ++i)
					{
						if (string.Compare(Item.mName, dgvVehicleControl.Rows[i].Cells["Name"].Value.ToString()) == 0)
						{
							rowIndex = i;
							break;
						}
					}
					if (rowIndex != -1)
					{
						dgvVehicleControl.Rows.RemoveAt(rowIndex);
					}
				}
            });
        }
        private void UpdateGui_DgvVehicleControl_UpdateItem(IVehicleControl Item, string StateName)
        {
            dgvVehicleControl.InvokeIfNecessary(() =>
            {
                lock (mLockOfDgvVehicleControl)
                {
                    int rowIndex = -1;
                    for (int i = 0; i < dgvVehicleControl.RowCount; ++i)
                    {
                        if (string.Compare(Item.mName, dgvVehicleControl.Rows[i].Cells["Name"].Value.ToString()) == 0)
                        {
                            rowIndex = i;
                            break;
                        }
                    }
                    if (rowIndex != -1)
                    {
                        if (StateName.Contains("SendState"))
                        {
                            dgvVehicleControl.Rows[rowIndex].Cells["SendState"].Value = Item.mSendState;
                        }
                        if (StateName.Contains("ExecuteState"))
                        {
                            dgvVehicleControl.Rows[rowIndex].Cells["ExecuteState"].Value = Item.mExecuteState;
							if (dgvVehicleControl.Rows[rowIndex].Cells["ExecuteState"].Value.ToString() == ExecuteState.Executing.ToString()) dgvVehicleControl.Rows[rowIndex].DefaultCellStyle.BackColor = TableRowExecutingBackColor;
                        }
                        if (StateName.Contains("FailedReason"))
                        {
                            dgvVehicleControl.Rows[rowIndex].Cells["FailedReason"].Value = Item.mFailedReason.ToString();
                        }
                        dgvVehicleControl.Rows[rowIndex].Cells["LastUpdate"].Value = Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT);
                    }
                }
            });
        }
    }
}
