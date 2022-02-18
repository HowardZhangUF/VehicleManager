using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Process;
using TrafficControlTest.Module.Log;
using LibraryForVM;

namespace TrafficControlTest.UserControl
{
	public partial class UcConsoleLog : System.Windows.Forms.UserControl
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

		private IDebugMessageHandler rDebugMessageHandler = null;
		private object mLockOfDgvConsoleLog = new object();

		public UcConsoleLog()
		{
			InitializeComponent();
			UpdateGui_InitializeDgvConsoleLog();
		}
		public void Set(IDebugMessageHandler DebugMessageHandler)
		{
			UnsubscribeEvent_DebugMessageHandler(rDebugMessageHandler);
			rDebugMessageHandler = DebugMessageHandler;
			SubscribeEvent_DebugMessageHandler(rDebugMessageHandler);
		}
		public void AddLog(string Date, string Category, string SubCategory, string Message)
		{
			if (OrderAscending)
			{
				UpdateGui_InsertRow(dgvConsoleLog.RowCount, Date, Category, SubCategory, Message);
				UpdateGui_RefreshDgvConsoleLogRowBackColor(dgvConsoleLog.RowCount - 1);
			}
			else
			{
				UpdateGui_InsertRow(0, Date, Category, SubCategory, Message);
				UpdateGui_RefreshDgvConsoleLogRowBackColor(0);
			}
			UpdateGui_AdjustRowCount(Maximum);
		}
		public void ClearLog()
		{
			lock (mLockOfDgvConsoleLog)
			{
				UpdateGui_ClearRow();
			}
		}

		private void SubscribeEvent_DebugMessageHandler(IDebugMessageHandler DebugMessageHandler)
		{
			if (DebugMessageHandler != null)
			{
				DebugMessageHandler.DebugMessage += HandleEvent_DebugMessageHandlerDebugMessage;
			}
		}
		private void UnsubscribeEvent_DebugMessageHandler(IDebugMessageHandler DebugMessageHandler)
		{
			if (DebugMessageHandler != null)
			{
				DebugMessageHandler.DebugMessage -= HandleEvent_DebugMessageHandlerDebugMessage;
			}
		}
		private void HandleEvent_DebugMessageHandlerDebugMessage(object Sender, DebugMessageEventArgs Args)
		{
			AddLog(Args.OccurTime, Args.Category, Args.SubCategory, Args.Message);
		}
		private void UpdateGui_InitializeDgvConsoleLog()
		{
			dgvConsoleLog.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvConsoleLog;

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
				dgv.Columns.Add("SubCategory", "SubCategory");
				dgv.Columns[2].Width = 200;
				dgv.Columns.Add("Message", "Message");
				dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
			dgvConsoleLog.InvokeIfNecessary(() =>
			{
				if (RowIndex <= dgvConsoleLog.Rows.Count)
				{
					dgvConsoleLog.Rows.Insert(RowIndex, RowData);
				}
			});
		}
		private void UpdateGui_RemoveRow(int RowIndex)
		{
			dgvConsoleLog.InvokeIfNecessary(() =>
			{
				if (RowIndex < dgvConsoleLog.Rows.Count)
				{
					dgvConsoleLog.Rows.RemoveAt(RowIndex);
				}
			});
		}
		private void UpdateGui_ClearRow()
		{
			dgvConsoleLog.InvokeIfNecessary(() =>
			{
				if (dgvConsoleLog.Rows.Count > 0)
				{
					dgvConsoleLog.Rows.Clear();
				}
			});
		}
		private void UpdateGui_RefreshDgvConsoleLogRowBackColor(int RowIndex)
		{
			dgvConsoleLog.InvokeIfNecessary(() =>
			{
				// 僅有第一個 Row 與第二個 Row 會使用 RowCount 去計算其 Background Color 應為何值，
				// 但從第三個 Row 開始，其 Background Color 要與前兩個或後兩個 Row 的 Background Color 一樣，
				// 使用遞增排序時，新增的 Row 會在尾端，所以該 Row 的 Background Color 要與前兩個的 Row 的 Background Color 一樣
				// 使用遞減排序時，新增的 Row 會在開頭，所以該 Row 的 Background Color 要與後兩個的 Row 的 Background Color 一樣
				if (dgvConsoleLog.RowCount > 2)
				{
					if (OrderAscending)
					{
						if (dgvConsoleLog.Rows[RowIndex].DefaultCellStyle.BackColor != dgvConsoleLog.Rows[RowIndex - 2].DefaultCellStyle.BackColor) dgvConsoleLog.Rows[RowIndex].DefaultCellStyle.BackColor = dgvConsoleLog.Rows[RowIndex - 2].DefaultCellStyle.BackColor;
					}
					else
					{
						if (dgvConsoleLog.Rows[RowIndex].DefaultCellStyle.BackColor != dgvConsoleLog.Rows[RowIndex + 2].DefaultCellStyle.BackColor) dgvConsoleLog.Rows[RowIndex].DefaultCellStyle.BackColor = dgvConsoleLog.Rows[RowIndex + 2].DefaultCellStyle.BackColor;
					}
				}
				else
				{
					if (dgvConsoleLog.RowCount % 2 == 0)
					{
						if (dgvConsoleLog.Rows[RowIndex].DefaultCellStyle.BackColor != TableEvenRowBackColor) dgvConsoleLog.Rows[RowIndex].DefaultCellStyle.BackColor = TableEvenRowBackColor;
					}
					else
					{
						if (dgvConsoleLog.Rows[RowIndex].DefaultCellStyle.BackColor != TableOddRowBackColor) dgvConsoleLog.Rows[RowIndex].DefaultCellStyle.BackColor = TableOddRowBackColor;
					}
				}
			});
		}
		private void UpdateGui_AdjustRowCount(int Maximum)
		{
			dgvConsoleLog.InvokeIfNecessary(() =>
			{
				if (dgvConsoleLog.Rows.Count > Maximum)
				{
					if (OrderAscending)
					{
						dgvConsoleLog.Rows.RemoveAt(0);
					}
					else
					{
						dgvConsoleLog.Rows.RemoveAt(dgvConsoleLog.RowCount - 1);
					}
				}
			});
		}
		private void cmenuItemClear_Click(object sender, EventArgs e)
		{
			lock (mLockOfDgvConsoleLog)
			{
				UpdateGui_ClearRow();
			}
		}
	}
}
