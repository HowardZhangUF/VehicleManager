using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Process;
using TrafficControlTest.Library;

namespace TrafficControlTest.UserControl
{
	public partial class UcSimpleLog : System.Windows.Forms.UserControl
	{
		public bool OrderAscending { get; set; } = false;
		public int Maximum { get; set; } = 200;
		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableGridLineColor { get; set; } = Color.FromArgb(86, 86, 86);
		public Color TableHeaderBackColor { get; set; } = Color.FromArgb(8, 122, 233);
		public Color TableHeaderForeColor { get; set; } = Color.White;
		public Color TableOddRowBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableEvenRowBackColor { get; set; } = Color.FromArgb(42, 42, 42);
		public Color TableExceptionRowBackColor { get; set; } = Color.FromArgb(178, 34, 34);
		public Color TableRowForeColor { get; set; } = Color.White;

		private VehicleManagerProcess rVehicleManagerProcess = null;
		private object mLockOfDgvSimpleLog = new object();

		public UcSimpleLog()
		{
			InitializeComponent();
			UpdateGui_InitializeDgvSimpleLog();
		}
		public void Set(VehicleManagerProcess VehicleManagerProcess)
		{
			UnsubscribeEvent_VehicleManagerProcess(rVehicleManagerProcess);
			rVehicleManagerProcess = VehicleManagerProcess;
			SubscribeEvent_VehicleManagerProcess(rVehicleManagerProcess);
		}
		public void AddLog(string Date, string Category, string Info)
		{
			lock (mLockOfDgvSimpleLog)
			{
				if (OrderAscending)
				{
					UpdateGui_InsertRow(dgvSimpleLog.RowCount, Date, Category, Info);
					UpdateGui_RefreshDgvSimpleLogRowBackColor(dgvSimpleLog.RowCount - 1);
				}
				else
				{
					UpdateGui_InsertRow(0, Date, Category, Info);
					UpdateGui_RefreshDgvSimpleLogRowBackColor(0);
				}
				UpdateGui_AdjustRowCount(Maximum);
			}
		}
		public void ClearLog()
		{
			lock (mLockOfDgvSimpleLog)
			{
				UpdateGui_ClearRow();
			}
		}

		private void SubscribeEvent_VehicleManagerProcess(VehicleManagerProcess VehicleManagerProcess)
		{
			if (VehicleManagerProcess != null)
			{
				VehicleManagerProcess.SignificantEvent += HandleEvent_VehicleManagerProcessSignificantEvent;
			}
		}
		private void UnsubscribeEvent_VehicleManagerProcess(VehicleManagerProcess VehicleManagerProcess)
		{
			if (VehicleManagerProcess != null)
			{
				VehicleManagerProcess.SignificantEvent -= HandleEvent_VehicleManagerProcessSignificantEvent;
			}
		}
		private void HandleEvent_VehicleManagerProcessSignificantEvent(object Sender, SignificantEventEventArgs Args)
		{
			AddLog(Args.OccurTime, Args.Category, Args.Info);
		}
		private void UpdateGui_InitializeDgvSimpleLog()
		{
			dgvSimpleLog.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvSimpleLog;

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
				dgv.ColumnHeadersHeight = 35;

				dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
				dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
				dgv.DefaultCellStyle.BackColor = TableEvenRowBackColor;
				dgv.DefaultCellStyle.ForeColor = TableRowForeColor;
				dgv.RowTemplate.Height = 25;

				dgv.Columns.Add("Date", "Date");
				dgv.Columns[0].Width = 175;
				dgv.Columns.Add("Category", "Category");
				dgv.Columns[1].Width = 200;
				dgv.Columns.Add("Info", "Info");
				dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}
			});
		}
		private void UpdateGui_InsertRow(int RowIndex, params string[] RowData)
		{
			dgvSimpleLog.InvokeIfNecessary(() =>
			{
				if (RowIndex <= dgvSimpleLog.Rows.Count)
				{
					dgvSimpleLog.Rows.Insert(RowIndex, RowData);
				}
			});
		}
		private void UpdateGui_RemoveRow(int RowIndex)
		{
			dgvSimpleLog.InvokeIfNecessary(() =>
			{
				if (RowIndex < dgvSimpleLog.Rows.Count)
				{
					dgvSimpleLog.Rows.RemoveAt(RowIndex);
				}
			});
		}
		private void UpdateGui_ClearRow()
		{
			dgvSimpleLog.InvokeIfNecessary(() =>
			{
				if (dgvSimpleLog.Rows.Count > 0)
				{
					dgvSimpleLog.Rows.Clear();
				}
			});
		}
		private void UpdateGui_RefreshDgvSimpleLogRowBackColor(int RowIndex)
		{
			dgvSimpleLog.InvokeIfNecessary(() =>
			{
				// 僅有第一個 Row 與第二個 Row 會使用 RowCount 去計算其 Background Color 應為何值，
				// 但從第三個 Row 開始，其 Background Color 要與前兩個或後兩個 Row 的 Background Color 一樣，
				// 使用遞增排序時，新增的 Row 會在尾端，所以該 Row 的 Background Color 要與前兩個的 Row 的 Background Color 一樣
				// 使用遞減排序時，新增的 Row 會在開頭，所以該 Row 的 Background Color 要與後兩個的 Row 的 Background Color 一樣
				if (dgvSimpleLog.RowCount > 2)
				{
					if (OrderAscending)
					{
						if (dgvSimpleLog.Rows[RowIndex].DefaultCellStyle.BackColor != dgvSimpleLog.Rows[RowIndex - 2].DefaultCellStyle.BackColor) dgvSimpleLog.Rows[RowIndex].DefaultCellStyle.BackColor = dgvSimpleLog.Rows[RowIndex - 2].DefaultCellStyle.BackColor;
					}
					else
					{
						if (dgvSimpleLog.Rows[RowIndex].DefaultCellStyle.BackColor != dgvSimpleLog.Rows[RowIndex + 2].DefaultCellStyle.BackColor) dgvSimpleLog.Rows[RowIndex].DefaultCellStyle.BackColor = dgvSimpleLog.Rows[RowIndex + 2].DefaultCellStyle.BackColor;
					}
				}
				else
				{
					if (dgvSimpleLog.RowCount % 2 == 0)
					{
						if (dgvSimpleLog.Rows[RowIndex].DefaultCellStyle.BackColor != TableEvenRowBackColor) dgvSimpleLog.Rows[RowIndex].DefaultCellStyle.BackColor = TableEvenRowBackColor;
					}
					else
					{
						if (dgvSimpleLog.Rows[RowIndex].DefaultCellStyle.BackColor != TableOddRowBackColor) dgvSimpleLog.Rows[RowIndex].DefaultCellStyle.BackColor = TableOddRowBackColor;
					}
				}
			});
		}
		private void UpdateGui_AdjustRowCount(int Maximum)
		{
			dgvSimpleLog.InvokeIfNecessary(() =>
			{
				if (dgvSimpleLog.Rows.Count > Maximum)
				{
					if (OrderAscending)
					{
						dgvSimpleLog.Rows.RemoveAt(0);
					}
					else
					{
						dgvSimpleLog.Rows.RemoveAt(dgvSimpleLog.RowCount - 1);
					}
				}
			});
		}
	}
}
