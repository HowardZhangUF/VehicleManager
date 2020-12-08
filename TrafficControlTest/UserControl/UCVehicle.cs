using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
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

		private IVehicleInfoManager rVehicleInfoManager = null;
		private object mLockOfDgvVehicleInfo = new object();
		private int mDgvVehicleInfoRightClickRowIndex { get; set; } = -1;

		public UcVehicle()
		{
			InitializeComponent();
			UpdateGui_DgvVehicleInfo_Initialize();
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
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
			dgv.Columns.Add("MapName", "MapName");
			dgv.Columns[6].Width = 240;
			dgv.Columns.Add("IpPort", "IPPort");
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
		private void UpdateGui_DgvVehicleInfo_AddItem(IVehicleInfo Item)
		{
			dgvVehicleInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvVehicleInfo)
				{
					if (dgvVehicleInfo.RowCount == 0)
					{
						dgvVehicleInfo.Rows.Add(new string[] { Item.mName, Item.mCurrentState, $"({Item.mLocationCoordinate.mX},{Item.mLocationCoordinate.mY},{(int)Item.mLocationToward})", Item.mCurrentTarget, Item.mBatteryValue.ToString("F2") + " %", Item.mLocationScore.ToString("F2") + " %", Item.mCurrentMapName, Item.mIpPort, Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT) });
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
							dgvVehicleInfo.Rows.Insert(newItemRowIndex, new string[] { Item.mName, Item.mCurrentState, $"({Item.mLocationCoordinate.mX},{Item.mLocationCoordinate.mY},{(int)Item.mLocationToward})", Item.mCurrentTarget, Item.mBatteryValue.ToString("F2") + " %", Item.mLocationScore.ToString("F2") + " %", Item.mCurrentMapName, Item.mIpPort, Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT) });
						}
						else
						{
							dgvVehicleInfo.Rows.Add(new string[] { Item.mName, Item.mCurrentState, $"({Item.mLocationCoordinate.mX},{Item.mLocationCoordinate.mY},{(int)Item.mLocationToward})", Item.mCurrentTarget, Item.mBatteryValue.ToString("F2") + " %", Item.mLocationScore.ToString("F2") + " %", Item.mCurrentMapName, Item.mIpPort, Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT) });
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
							dgvVehicleInfo.Rows[rowIndex].Cells["LocationScore"].Value = Item.mBatteryValue.ToString("F2") + " %";
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
	}
}
