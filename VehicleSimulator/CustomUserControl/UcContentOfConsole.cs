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

namespace VehicleSimulator
{
	public partial class UcContentOfConsole : UserControl
	{
		public int mMaxRowCount { get; set; } = 200;

		public UcContentOfConsole()
		{
			InitializeComponent();
			UpdateGui_InitializeDgvConsole();
		}
		public void AddLog(string Timestamp, string Log)
		{
			UpdateGui_AddRow(Timestamp, Log);
			UpdateGui_AdjustRowCount(mMaxRowCount);
		}
		public void ClearLog()
		{
			UpdateGui_ClearRow();
		}

		private void menuItemClearDgvConsole_Click(object sender, EventArgs e)
		{
			UpdateGui_ClearRow();
		}
		private void UpdateGui_InitializeDgvConsole()
		{
			DataGridView dgv = dgvConsole;

			dgv.SelectionChanged += ((sender, e) => dgv.ClearSelection());

			dgv.RowHeadersVisible = false;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToResizeRows = false;
			dgv.AllowUserToResizeColumns = false;
			dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv.MultiSelect = false;
			dgv.GridColor = Color.FromArgb(41, 41, 41);
			dgv.BorderStyle = BorderStyle.None;

			dgv.EnableHeadersVisualStyles = false;
			dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold);
			dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(8, 122, 233);
			dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
			dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dgv.ColumnHeadersHeight = 35;

			dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
			dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
			dgv.DefaultCellStyle.BackColor = Color.FromArgb(67, 67, 67);
			dgv.DefaultCellStyle.ForeColor = Color.White;
			dgv.RowTemplate.Height = 25;

			dgv.Columns.Add("Timestamp", "Timestamp");
			dgv.Columns[0].Width = 175;
			dgv.Columns.Add("Log", "Log");
			dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
				column.ReadOnly = true;
			}
		}
		private void UpdateGui_AddRow(params string[] RowData)
		{
			dgvConsole.InvokeIfNecessary(() =>
			{
				dgvConsole.Rows.Add(RowData);
			});
		}
		private void UpdateGui_RemoveRow(int RowIndex)
		{
			dgvConsole.InvokeIfNecessary(() =>
			{
				if (RowIndex < dgvConsole.Rows.Count)
				{
					dgvConsole.Rows.RemoveAt(RowIndex);
				}
			});
		}
		private void UpdateGui_ClearRow()
		{
			dgvConsole.InvokeIfNecessary(() =>
			{
				if (dgvConsole.Rows.Count > 0)
				{
					dgvConsole.Rows.Clear();
				}
			});
		}
		private void UpdateGui_AdjustRowCount(int Maximum)
		{
			dgvConsole.InvokeIfNecessary(() =>
			{
				if (dgvConsole.Rows.Count > Maximum)
				{
					dgvConsole.Rows.RemoveAt(0);
				}
			});
		}
	}
}
