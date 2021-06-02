using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.General;
using TrafficControlTest.Library;

namespace TrafficControlTest.UserControl
{
	public partial class UcAutomaticDoor : System.Windows.Forms.UserControl
	{
		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableGridLineColor { get; set; } = Color.FromArgb(86, 86, 86);
		public Color TableHeaderBackColor { get; set; } = Color.FromArgb(0, 122, 204);
		public Color TableHeaderForeColor { get; set; } = Color.White;
		public Color TableRowBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableRowUnsendBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableRowSendingBackColor { get; set; } = Color.FromArgb(128, 128, 0);
		public Color TableRowSentSuccessedBackColor { get; set; } = Color.FromArgb(0, 128, 0);
		public Color TableRowSentFailedBackColor { get; set; } = Color.FromArgb(128, 0, 0);
		public Color TableRowForeColor { get; set; } = Color.White;
		public Color TableRowConnectedForeColor { get; set; } = Color.White;
		public Color TableRowDisconnectedForeColor { get; set; } = Color.FromArgb(89, 89, 89);
		public string TimestampFormat { get; set; } = "yyyy/MM/dd HH:mm:ss.fff";
		public int MaxRowCountOfHistoryAutomaticDoorControl { get; set; } = 10;

		private IAutomaticDoorInfoManager rAutomaticDoorInfoManager = null;
		private IAutomaticDoorControlManager rAutomaticDoorControlManager = null;
		private object mLockOfDgvAutomaticDoorInfo = new object();
		private object mLockOfDgvAutomaticDoorControl = new object();
		private int mDgvAutomaticDoorInfoRightClickRowIndex { get; set; } = -1;
		private int mDgvAutomaticDoorInfoRightClickColIndex { get; set; } = -1;

		public UcAutomaticDoor()
		{
			InitializeComponent();
			UpdateGui_DgvAutomaticDoorInfo_Initialize();
			UpdateGui_DgvAutomaticDoorControl_Initialize();
		}
		public void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			UnsubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
			rAutomaticDoorInfoManager = AutomaticDoorInfoManager;
			SubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
		}
		public void Set(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			UnsubscribeEvent_IAutomaticDoorControlManager(rAutomaticDoorControlManager);
			rAutomaticDoorControlManager = AutomaticDoorControlManager;
			SubscribeEvent_IAutomaticDoorControlManager(rAutomaticDoorControlManager);
		}
		public void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager, IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			Set(AutomaticDoorInfoManager);
			Set(AutomaticDoorControlManager);
		}
		public void EnableManualControl(bool Enable)
		{
			if (Enable)
			{
				dgvAutomaticDoorInfo.ContextMenuStrip = cmenuDgvAutomaticDoorInfo;
			}
			else
			{
				dgvAutomaticDoorInfo.ContextMenuStrip = null;
			}
		}
		public void SetAutomaticDoorControlVisible(bool Visible)
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

		private void SubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				AutomaticDoorInfoManager.ItemAdded += HandleEvent_AutomaticDoorInfoManagerItemAdded;
				AutomaticDoorInfoManager.ItemRemoved += HandleEvent_AutomaticDoorInfoManagerItemRemoved;
				AutomaticDoorInfoManager.ItemUpdated += HandleEvent_AutomaticDoorInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				AutomaticDoorInfoManager.ItemAdded -= HandleEvent_AutomaticDoorInfoManagerItemAdded;
				AutomaticDoorInfoManager.ItemRemoved -= HandleEvent_AutomaticDoorInfoManagerItemRemoved;
				AutomaticDoorInfoManager.ItemUpdated -= HandleEvent_AutomaticDoorInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				AutomaticDoorControlManager.ItemAdded += HandleEvent_AutomaticDoorControlManagerItemAdded;
				AutomaticDoorControlManager.ItemRemoved += HandleEvent_AutomaticDoorControlManagerItemRemoved;
				AutomaticDoorControlManager.ItemUpdated += HandleEvent_AutomaticDoorControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorControlManager(IAutomaticDoorControlManager AutomaticDoorControlManager)
		{
			if (AutomaticDoorControlManager != null)
			{
				AutomaticDoorControlManager.ItemAdded -= HandleEvent_AutomaticDoorControlManagerItemAdded;
				AutomaticDoorControlManager.ItemRemoved -= HandleEvent_AutomaticDoorControlManagerItemRemoved;
				AutomaticDoorControlManager.ItemUpdated -= HandleEvent_AutomaticDoorControlManagerItemUpdated;
			}
		}
		private void HandleEvent_AutomaticDoorInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IAutomaticDoorInfo> Args)
		{
			UpdateGui_DgvAutomaticDoorInfo_AddItem(Args.Item);
		}
		private void HandleEvent_AutomaticDoorInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IAutomaticDoorInfo> Args)
		{
			UpdateGui_DgvAutomaticDoorInfo_RemoveItem(Args.Item);
		}
		private void HandleEvent_AutomaticDoorInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IAutomaticDoorInfo> Args)
		{
			UpdateGui_DgvAutomaticDoorInfo_UpdateItem(Args.Item, Args.StatusName);
		}
		private void HandleEvent_AutomaticDoorControlManagerItemAdded(object Sender, ItemCountChangedEventArgs<IAutomaticDoorControl> Args)
		{
			UpdateGui_DgvAutomaticDoorControl_AddItem(Args.Item);
			if (dgvAutomaticDoorControl.RowCount > MaxRowCountOfHistoryAutomaticDoorControl) UpdateGui_DgvAutomaticDoorControl_RemoveLastItem();
		}
		private void HandleEvent_AutomaticDoorControlManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IAutomaticDoorControl> Args)
		{
			//UpdateGui_DgvAutomaticDoorControl_RemoveItem(Args.Item);
		}
		private void HandleEvent_AutomaticDoorControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IAutomaticDoorControl> Args)
		{
			UpdateGui_DgvAutomaticDoorControl_UpdateItem(Args.Item, Args.StatusName);
		}
		private void UpdateGui_DgvAutomaticDoorInfo_Initialize()
		{
			dgvAutomaticDoorInfo.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvAutomaticDoorInfo;

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
				dgv.Columns[0].Width = 200;
				dgv.Columns.Add("IPPort", "IPPort");
				dgv.Columns[1].Width = 180;
				dgv.Columns.Add("IsConnected", "IsConnected");
				dgv.Columns[2].Width = 120;
				dgv.Columns.Add("State", "State");
				dgv.Columns[3].Width = 120;
				dgv.Columns.Add("LastUpdate", "LastUpdate");
				dgv.Columns[4].Width = 190;
				dgv.Columns.Add("FillColumn", "");
				dgv.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}
			});
		}
		private void UpdateGui_DgvAutomaticDoorInfo_AddItem(IAutomaticDoorInfo Item)
		{
			dgvAutomaticDoorInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvAutomaticDoorInfo)
				{
					if (dgvAutomaticDoorInfo.RowCount == 0)
					{
						dgvAutomaticDoorInfo.Rows.Add(new string[] { Item.mName, Item.mIpPort, Item.mIsConnected.ToString(), Item.mState.ToString(), Item.mLastUpdated.ToString(TimestampFormat) });
						dgvAutomaticDoorInfo.Rows[0].DefaultCellStyle.ForeColor = Item.mIsConnected ? TableRowConnectedForeColor : TableRowDisconnectedForeColor;
					}
					else
					{
						int newItemRowIndex = -1;
						for (int i = 0; i < dgvAutomaticDoorInfo.RowCount; ++i)
						{
							if (string.Compare(Item.mName, dgvAutomaticDoorInfo.Rows[i].Cells["Name"].Value.ToString()) == -1)
							{
								newItemRowIndex = i;
								break;
							}
						}

						if (newItemRowIndex != -1)
						{
							dgvAutomaticDoorInfo.Rows.Insert(newItemRowIndex, new string[] { Item.mName, Item.mIpPort, Item.mIsConnected.ToString(), Item.mState.ToString(), Item.mLastUpdated.ToString(TimestampFormat) });
							dgvAutomaticDoorInfo.Rows[newItemRowIndex].DefaultCellStyle.ForeColor = GetAutomaticDoorInfoRowForeColor(Item.mIsConnected);
						}
						else
						{
							dgvAutomaticDoorInfo.Rows.Add(new string[] { Item.mName, Item.mIpPort, Item.mIsConnected.ToString(), Item.mState.ToString(), Item.mLastUpdated.ToString(TimestampFormat) });
							dgvAutomaticDoorInfo.Rows[dgvAutomaticDoorInfo.RowCount - 1].DefaultCellStyle.ForeColor = GetAutomaticDoorInfoRowForeColor(Item.mIsConnected);
						}
					}
				}
			});
		}
		private void UpdateGui_DgvAutomaticDoorInfo_RemoveItem(IAutomaticDoorInfo Item)
		{
			dgvAutomaticDoorInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvAutomaticDoorInfo)
				{
					int rowIndex = -1;
					for (int i = 0; i < dgvAutomaticDoorInfo.RowCount; ++i)
					{
						if (string.Compare(Item.mName, dgvAutomaticDoorInfo.Rows[i].Cells["Name"].Value.ToString()) == 0)
						{
							rowIndex = i;
							break;
						}
					}
					if (rowIndex != -1)
					{
						dgvAutomaticDoorInfo.Rows.RemoveAt(rowIndex);
					}
				}
			});
		}
		private void UpdateGui_DgvAutomaticDoorInfo_UpdateItem(IAutomaticDoorInfo Item, string StateName)
		{
			dgvAutomaticDoorInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvAutomaticDoorInfo)
				{
					int rowIndex = -1;
					for (int i = 0; i < dgvAutomaticDoorInfo.RowCount; ++i)
					{
						if (string.Compare(Item.mName, dgvAutomaticDoorInfo.Rows[i].Cells["Name"].Value.ToString()) == 0)
						{
							rowIndex = i;
							break;
						}
					}
					if (rowIndex != -1)
					{
						switch (StateName)
						{
							case "IsConnected":
								dgvAutomaticDoorInfo.Rows[rowIndex].Cells[StateName].Value = Item.mIsConnected.ToString();
								dgvAutomaticDoorInfo.Rows[rowIndex].Cells["LastUpdate"].Value = Item.mLastUpdated.ToString(TimestampFormat);
								dgvAutomaticDoorInfo.Rows[rowIndex].DefaultCellStyle.ForeColor = GetAutomaticDoorInfoRowForeColor(Item.mIsConnected);
								break;
							case "State":
								dgvAutomaticDoorInfo.Rows[rowIndex].Cells[StateName].Value = Item.mState.ToString();
								dgvAutomaticDoorInfo.Rows[rowIndex].Cells["LastUpdate"].Value = Item.mLastUpdated.ToString(TimestampFormat);
								break;
						}
					}
				}
			});
		}
		private void UpdateGui_DgvAutomaticDoorControl_Initialize()
		{
			dgvAutomaticDoorControl.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvAutomaticDoorControl;

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
				dgv.Columns[0].Width = 300;
				dgv.Columns.Add("AutomaticDoorName", "AutomaticDoorName");
				dgv.Columns[1].Width = 200;
				dgv.Columns.Add("Command", "Command");
				dgv.Columns[2].Width = 120;
				dgv.Columns.Add("Cause", "Cause");
				dgv.Columns[3].Width = 300;
				dgv.Columns.Add("SendState", "SendState");
				dgv.Columns[4].Width = 120;
				dgv.Columns.Add("LastUpdate", "LastUpdate");
				dgv.Columns[5].Width = 190;
				dgv.Columns.Add("FillColumn", "");
				dgv.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}
			});
		}
		private void UpdateGui_DgvAutomaticDoorControl_AddItem(IAutomaticDoorControl Item)
		{
			dgvAutomaticDoorControl.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvAutomaticDoorControl)
				{
					dgvAutomaticDoorControl.Rows.Insert(0, new string[] { Item.mName, Item.mAutomaticDoorName, Item.mCommand.ToString(), Item.mCause, Item.mSendState.ToString(), Item.mLastUpdated.ToString(TimestampFormat) });
					dgvAutomaticDoorControl.Rows[0].DefaultCellStyle.BackColor = GetAutomaticDoorControlRowBackColor(Item.mSendState);
				}
			});
		}
		private void UpdateGui_DgvAutomaticDoorControl_RemoveItem(IAutomaticDoorControl Item)
		{
			dgvAutomaticDoorControl.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvAutomaticDoorControl)
				{
					int rowIndex = -1;
					for (int i = 0; i < dgvAutomaticDoorControl.RowCount; ++i)
					{
						if (string.Compare(Item.mName, dgvAutomaticDoorControl.Rows[i].Cells["Name"].Value.ToString()) == 0)
						{
							rowIndex = i;
							break;
						}
					}
					if (rowIndex != -1)
					{
						dgvAutomaticDoorControl.Rows.RemoveAt(rowIndex);
					}
				}
			});
		}
		private void UpdateGui_DgvAutomaticDoorControl_UpdateItem(IAutomaticDoorControl Item, string StateName)
		{
			dgvAutomaticDoorControl.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvAutomaticDoorControl)
				{
					int rowIndex = -1;
					for (int i = 0; i < dgvAutomaticDoorControl.RowCount; ++i)
					{
						if (string.Compare(Item.mName, dgvAutomaticDoorControl.Rows[i].Cells["Name"].Value.ToString()) == 0)
						{
							rowIndex = i;
							break;
						}
					}
					if (rowIndex != -1)
					{
						switch (StateName)
						{
							case "SendState":
								dgvAutomaticDoorControl.Rows[rowIndex].Cells[StateName].Value = Item.mSendState.ToString();
								dgvAutomaticDoorControl.Rows[rowIndex].Cells["LastUpdate"].Value = Item.mLastUpdated.ToString(TimestampFormat);
								dgvAutomaticDoorControl.Rows[rowIndex].DefaultCellStyle.BackColor = GetAutomaticDoorControlRowBackColor(Item.mSendState);
								break;
						}
					}
				}
			});
		}
		private void UpdateGui_DgvAutomaticDoorControl_RemoveLastItem()
		{
			dgvAutomaticDoorControl.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvAutomaticDoorControl)
				{
					if (dgvAutomaticDoorControl.RowCount > 0)
					{
						dgvAutomaticDoorControl.Rows.RemoveAt(dgvAutomaticDoorControl.RowCount - 1);
					}
				}
			});
		}
		private void UpdateGui_DgvAutomaticDoorControl_RemoveAll()
		{
			dgvAutomaticDoorControl.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvAutomaticDoorControl)
				{
					if (dgvAutomaticDoorControl.RowCount > 0)
					{
						dgvAutomaticDoorControl.Rows.Clear();
					}
				}
			});
		}
		private Color GetAutomaticDoorInfoRowForeColor(bool IsConnected)
		{
			switch (IsConnected)
			{
				case true:
					return TableRowConnectedForeColor;
				case false:
				default:
					return TableRowDisconnectedForeColor;
			}
		}
		private Color GetAutomaticDoorControlRowBackColor(AutomaticDoorControlCommandSendState SendState)
		{
			switch (SendState)
			{
				case AutomaticDoorControlCommandSendState.Sending:
					return TableRowSendingBackColor;
				case AutomaticDoorControlCommandSendState.SentSuccessed:
					return TableRowSentSuccessedBackColor;
				case AutomaticDoorControlCommandSendState.SentFailed:
					return TableRowSentFailedBackColor;
				case AutomaticDoorControlCommandSendState.Unsend:
				default:
					return TableRowUnsendBackColor;
			}
		}
		private void dgvAutomaticDoorInfo_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					var hit = dgvAutomaticDoorInfo.HitTest(e.X, e.Y);
					mDgvAutomaticDoorInfoRightClickRowIndex = hit.RowIndex;
					mDgvAutomaticDoorInfoRightClickColIndex = hit.ColumnIndex;
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
		private void cmenuItemOpenAutomaticDoor_Click(object sender, EventArgs e)
		{
			try
			{
				if (mDgvAutomaticDoorInfoRightClickRowIndex >= 0 && mDgvAutomaticDoorInfoRightClickRowIndex < dgvAutomaticDoorInfo.RowCount)
				{
					string automaticDoorName = dgvAutomaticDoorInfo.Rows[mDgvAutomaticDoorInfoRightClickRowIndex].Cells["Name"].Value.ToString();
					IAutomaticDoorControl control = Library.Library.GenerateIAutomaticDoorControl(automaticDoorName, AutomaticDoorControlCommand.Open, "Manual");
					rAutomaticDoorControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
		private void cmenuItemCloseAutomaticDoor_Click(object sender, EventArgs e)
		{
			try
			{
				if (mDgvAutomaticDoorInfoRightClickRowIndex >= 0 && mDgvAutomaticDoorInfoRightClickRowIndex < dgvAutomaticDoorInfo.RowCount)
				{
					string automaticDoorName = dgvAutomaticDoorInfo.Rows[mDgvAutomaticDoorInfoRightClickRowIndex].Cells["Name"].Value.ToString();
					IAutomaticDoorControl control = Library.Library.GenerateIAutomaticDoorControl(automaticDoorName, AutomaticDoorControlCommand.Close, "Manual");
					rAutomaticDoorControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
		private void cmenuItemClear_Click(object sender, EventArgs e)
		{
			try
			{
				UpdateGui_DgvAutomaticDoorControl_RemoveAll();
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
	}
}
