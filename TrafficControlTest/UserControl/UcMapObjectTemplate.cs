using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryForVM;
using TrafficControlTest.Module.ChargeStation;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.LimitVehicleCountZone;

namespace TrafficControlTest.UserControl
{
	public partial class UcMapObjectTemplate<T> : System.Windows.Forms.UserControl where T : IItem
	{
		public event EventHandler<MapFocusRequestEventArgs> MapFocusRequest;

		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableGridLineColor { get; set; } = Color.FromArgb(86, 86, 86);
		public Color TableHeaderBackColor { get; set; } = Color.FromArgb(0, 122, 204);
		public Color TableHeaderForeColor { get; set; } = Color.White;
		public Color TableRowBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableRowForeColor { get; set; } = Color.White;
		public string TimestampFormat { get; set; } = "yyyy/MM/dd HH:mm:ss.fff";

		private IItemManager<T> rItemManager = null;
		private Dictionary<string, int> mColumnDictionary = null;
		private List<KeyValuePair<string, int>> mColumnList = null;
		private object mLockOfDgvMapObjectInfo = new object();
		private int mDgvMapObjectInfoLeftClickRowIndex { get; set; } = -1;
		private int mDgvMapObjectInfoLeftClickColIndex { get; set; } = -1;

		public UcMapObjectTemplate()
		{
			InitializeComponent();
		}
		public void Set(IItemManager<T> ItemManager)
		{
			UnsubscribeEvent_IItemManager(rItemManager);
			rItemManager = ItemManager;
			SubscribeEvent_IItemManager(rItemManager);

			mColumnDictionary = GenerateColumnList(rItemManager);
			mColumnList = mColumnDictionary.ToList();

			UpdateGui_DgvMapObjectInfo_Initialize();
		}

		protected virtual void RaiseEvent_MapFocusRequest(int X, int Y, bool Sync = true)
		{
			if (Sync)
			{
				MapFocusRequest?.Invoke(this, new MapFocusRequestEventArgs(X, Y));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { MapFocusRequest?.Invoke(this, new MapFocusRequestEventArgs(X, Y)); });
			}
		}

		private void SubscribeEvent_IItemManager(IItemManager<T> ItemManager)
		{
			if (ItemManager != null)
			{
				ItemManager.ItemAdded += HandleEvent_ItemManagerItemAdded;
				ItemManager.ItemRemoved += HandleEvent_ItemManagerItemRemoved;
				ItemManager.ItemUpdated += HandleEvent_ItemManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IItemManager(IItemManager<T> ItemManager)
		{
			if (ItemManager != null)
			{
				ItemManager.ItemAdded -= HandleEvent_ItemManagerItemAdded;
				ItemManager.ItemRemoved -= HandleEvent_ItemManagerItemRemoved;
				ItemManager.ItemUpdated -= HandleEvent_ItemManagerItemUpdated;
			}
		}
		private void HandleEvent_ItemManagerItemAdded(object Sender, ItemCountChangedEventArgs<T> Args)
		{
			UpdateGui_DgvMapObjectInfo_AddRow(GetDataArray(Args.Item));
		}
		private void HandleEvent_ItemManagerItemRemoved(object Sender, ItemCountChangedEventArgs<T> Args)
		{
			UpdateGui_DgvMapObjectInfo_RemoveRow(Args.Item);
		}
		private void HandleEvent_ItemManagerItemUpdated(object Sender, ItemUpdatedEventArgs<T> Args)
		{
			string[] updatedStatusNames = Args.StatusName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < updatedStatusNames.Length; ++i)
			{
				if (mColumnDictionary.ContainsKey(updatedStatusNames[i]))
				{
					UpdateGui_DgvMapObjectInfo_UpdateItem(Args.Item, updatedStatusNames[i]);
				}
			}
		}
		private void dgvMapObjectInfo_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{
					var hit = dgvMapObjectInfo.HitTest(e.X, e.Y);
					mDgvMapObjectInfoLeftClickRowIndex = hit.RowIndex;
					mDgvMapObjectInfoLeftClickColIndex = hit.ColumnIndex;
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void dgvMapObjectInfo_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{
					var hit = dgvMapObjectInfo.HitTest(e.X, e.Y);
					if (hit.RowIndex > -1 && hit.ColumnIndex > -1)
					{
						string itemName = dgvMapObjectInfo.Rows[hit.RowIndex].Cells["Name"].Value.ToString();
						IItem item = rItemManager.GetItem(itemName);
						GetLocation(item, out int X, out int Y);
						RaiseEvent_MapFocusRequest(X, Y);
					}
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void UpdateGui_DgvMapObjectInfo_Initialize()
		{
			this.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvMapObjectInfo;

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

				dgv.Columns.Clear();
				for (int i = 0; i < mColumnList.Count; ++i)
				{
					dgv.Columns.Add(mColumnList[i].Key, mColumnList[i].Key);
					dgv.Columns[mColumnList[i].Key].Width = mColumnList[i].Value;
				}
				dgv.Columns.Add("FillColumn", "");
				dgv.Columns["FillColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}
			});
		}
		private void UpdateGui_DgvMapObjectInfo_AddRow(params string[] RowData)
		{
			dgvMapObjectInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvMapObjectInfo)
				{
					dgvMapObjectInfo.Rows.Add(RowData);
				}
			});
		}
		private void UpdateGui_DgvMapObjectInfo_RemoveRow(IItem Item)
		{
			dgvMapObjectInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvMapObjectInfo)
				{
					int rowIndex = -1;
					for (int i = 0; i < dgvMapObjectInfo.RowCount; ++i)
					{
						if (string.Compare(Item.mName, dgvMapObjectInfo.Rows[i].Cells["Name"].Value.ToString()) == 0)
						{
							rowIndex = i;
							break;
						}
					}
					if (rowIndex != -1)
					{
						dgvMapObjectInfo.Rows.RemoveAt(rowIndex);
					}
				}
			});
		}
		private void UpdateGui_DgvMapObjectInfo_UpdateItem(IItem Item, string StatusName)
		{
			dgvMapObjectInfo.InvokeIfNecessary(() =>
			{
				lock (mLockOfDgvMapObjectInfo)
				{
					int rowIndex = -1;
					for (int i = 0; i < dgvMapObjectInfo.RowCount; ++i)
					{
						if (string.Compare(Item.mName, dgvMapObjectInfo.Rows[i].Cells["Name"].Value.ToString()) == 0)
						{
							rowIndex = i;
							break;
						}
					}
					if (rowIndex != -1)
					{
						dgvMapObjectInfo.Rows[rowIndex].Cells[StatusName].Value = GetData(Item, StatusName);
						dgvMapObjectInfo.Rows[rowIndex].Cells["LastUpdate"].Value = GetData(Item, "LastUpdate");
					}
				}
			});
		}
		private void UpdateGui_DgvMapObjectInfo_ClearRows()
		{
			this.InvokeIfNecessary(() =>
			{
				if (dgvMapObjectInfo.Rows.Count > 0)
				{
					dgvMapObjectInfo.Rows.Clear();
				}
			});
		}

		private Dictionary<string, int> GenerateColumnList(IItemManager<T> ItemManager)
		{
			if (rItemManager is IChargeStationInfoManager)
			{
				return GenerateColumnList_IChargeStation();
			}
			else if (rItemManager is IAutomaticDoorInfoManager)
			{
				return GenerateColumnList_IAutomaticDoor();
			}
			else if (rItemManager is ILimitVehicleCountZoneInfoManager)
			{
				return GenerateColumnList_ILimitVehicleCountZone();
			}
			else
			{
				throw new Exception("Unknown item!");
			}
		}
		private Dictionary<string, int> GenerateColumnList_IChargeStation()
		{
			return new Dictionary<string, int>()
			{
				{"Name", 200 },
				{"Location", 200 },
				{"LocationRange", 300 },
				{"Enable", 100 }, // changeable
				{"IsBeingUsed", 100 }, // changeable
				{"LastUpdate", 200 }
			};
		}
		private Dictionary<string, int> GenerateColumnList_IAutomaticDoor()
		{
			return new Dictionary<string, int>()
			{
				{"Name", 200 },
				{"Range", 300 },
				{"IpPort", 200 },
				{"IsConnected", 100 }, // changeable
				{"State", 100 }, // changeable
				{"LastUpdate", 200 }
			};
		}
		private Dictionary<string, int> GenerateColumnList_ILimitVehicleCountZone()
		{
			return new Dictionary<string, int>()
			{
				{"Name", 200 },
				{"Range", 300 },
				{"MaxVehicleCount", 150 },
				{"IsUnioned", 100 },
				{"UnionId", 100 },
				{"CurrentVehicleNameList", 400 }, // changeable
				{"LastUpdate", 200 }
			};
		}

		private string[] GetDataArray(IItem Item)
		{
			if (Item is IChargeStationInfo)
			{
				return GetDataArray_IChargeStation(Item);
			}
			else if (Item is IAutomaticDoorInfo)
			{
				return GetDataArray_IAutomaticDoor(Item);
			}
			else if (Item is ILimitVehicleCountZoneInfo)
			{
				return GetDataArray_ILimitVehicleCountZone(Item);
			}
			else
			{
				throw new Exception("Unknown item!");
			}
		}
		private string[] GetDataArray_IChargeStation(IItem Item)
		{
			var tmpItem = Item as IChargeStationInfo;
			return new string[] { tmpItem.mName, tmpItem.mLocation.ToString(), tmpItem.mLocationRange.ToString(), tmpItem.mEnable.ToString(), tmpItem.mIsBeingUsed.ToString(), tmpItem.mLastUpdated.ToString(TimestampFormat) };
		}
		private string[] GetDataArray_IAutomaticDoor(IItem Item)
		{
			var tmpItem = Item as IAutomaticDoorInfo;
			return new string[] { tmpItem.mName, tmpItem.mRange.ToString(), tmpItem.mIpPort, tmpItem.mIsConnected.ToString(), tmpItem.mState.ToString(), tmpItem.mLastUpdated.ToString(TimestampFormat) };
		}
		private string[] GetDataArray_ILimitVehicleCountZone(IItem Item)
		{
			var tmpItem = Item as ILimitVehicleCountZoneInfo;
			return new string[] { tmpItem.mName, tmpItem.mRange.ToString(), tmpItem.mMaxVehicleCount.ToString(), tmpItem.mIsUnioned.ToString(), tmpItem.mUnionId.ToString(), string.Join(",", tmpItem.mCurrentVehicleNameList.Select(o => o.Item1)), tmpItem.mLastUpdated.ToString(TimestampFormat) };
		}

		private string GetData(IItem Item, string StatusName)
		{
			if (Item is IChargeStationInfo)
			{
				return GetData_IChargeStation(Item, StatusName);
			}
			else if (Item is IAutomaticDoorInfo)
			{
				return GetData_IAutomaticDoor(Item, StatusName);
			}
			else if (Item is ILimitVehicleCountZoneInfo)
			{
				return GetData_ILimitVehicleCountZone(Item, StatusName);
			}
			else
			{
				throw new Exception("Unknown item!");
			}
		}
		private string GetData_IChargeStation(IItem Item, string StatusName)
		{
			var tmpItem = Item as IChargeStationInfo;
			switch (StatusName)
			{
				case "Enable":
					return tmpItem.mEnable.ToString();
				case "IsBeingUsed":
					return tmpItem.mIsBeingUsed.ToString();
				case "LastUpdate":
					return tmpItem.mLastUpdated.ToString(TimestampFormat);
				default:
					return null;
			}
		}
		private string GetData_IAutomaticDoor(IItem Item, string StatusName)
		{
			var tmpItem = Item as IAutomaticDoorInfo;
			switch (StatusName)
			{
				case "IsConnected":
					return tmpItem.mIsConnected.ToString();
				case "State":
					return tmpItem.mState.ToString();
				case "LastUpdate":
					return tmpItem.mLastUpdated.ToString(TimestampFormat);
				default:
					return null;
			}
		}
		private string GetData_ILimitVehicleCountZone(IItem Item, string StatusName)
		{
			var tmpItem = Item as ILimitVehicleCountZoneInfo;
			switch (StatusName)
			{
				case "CurrentVehicleNameList":
					return string.Join(",", tmpItem.mCurrentVehicleNameList.Select(o => o.Item1));
				case "LastUpdate":
					return tmpItem.mLastUpdated.ToString(TimestampFormat);
				default:
					return null;
			}
		}

		private void GetLocation(IItem Item, out int X, out int Y)
		{
			if (Item is IChargeStationInfo)
			{
				GetLocation_IChargeStation(Item, out X, out Y);
			}
			else if (Item is IAutomaticDoorInfo)
			{
				GetLocation_IAutomaticDoor(Item, out X, out Y);
			}
			else if (Item is ILimitVehicleCountZoneInfo)
			{
				GetLocation_ILimitVehicleCountZone(Item, out X, out Y);
			}
			else
			{
				throw new Exception("Unknown item!");
			}
		}
		private void GetLocation_IChargeStation(IItem Item, out int X, out int Y)
		{
			var tmpItem = Item as IChargeStationInfo;
			X = tmpItem.mLocation.mX;
			Y = tmpItem.mLocation.mY;
		}
		private void GetLocation_IAutomaticDoor(IItem Item, out int X, out int Y)
		{
			var tmpItem = Item as IAutomaticDoorInfo;
			X = tmpItem.mRange.mCenterX;
			Y = tmpItem.mRange.mCenterY;
		}
		private void GetLocation_ILimitVehicleCountZone(IItem Item, out int X, out int Y)
		{
			var tmpItem = Item as ILimitVehicleCountZoneInfo;
			X = tmpItem.mRange.mCenterX;
			Y = tmpItem.mRange.mCenterY;
		}
	}

	public class MapFocusRequestEventArgs : EventArgs
	{
		public int X { get; private set; }
		public int Y { get; private set; }

		public MapFocusRequestEventArgs(int X, int Y)
		{
			this.X = X;
			this.Y = Y;
		}
	}
}
