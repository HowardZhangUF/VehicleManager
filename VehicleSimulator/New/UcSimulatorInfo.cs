using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleSimulator.New
{
    public partial class UcSimulatorInfo : UserControl
    {
        public UcSimulatorInfo()
        {
            InitializeComponent();
			UpdateGui_InitializeDgvSimulatorInfo();
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
			dgv.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			dgv.Rows.Add("Name    ", "Vehicle001");
			dgv.Rows.Add("Status    ", "Idle");
			dgv.Rows.Add("X    ", "59099", "IsTranslating    ", "false");
			dgv.Rows.Add("Y    ", "234618");
			dgv.Rows.Add("Head    ", "160", "IsRotating    ", "false");
			dgv.Rows.Add("TranslateVelocity    ", "800", "RotateVelocity    ", "10");
			dgv.ClearSelection();
		}
        private void dgvSimulatorInfo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSimulatorInfo.CurrentCell.ColumnIndex == dgvSimulatorInfo.Columns["ItemKey1"].Index 
                || dgvSimulatorInfo.CurrentCell.ColumnIndex == dgvSimulatorInfo.Columns["ItemKey2"].Index
				|| (dgvSimulatorInfo.CurrentCell.ColumnIndex == 1 && (dgvSimulatorInfo.CurrentCell.RowIndex == 0 || dgvSimulatorInfo.CurrentCell.RowIndex == 1))
				|| (dgvSimulatorInfo.CurrentCell.ColumnIndex == 3 && (dgvSimulatorInfo.CurrentCell.RowIndex == 0 || dgvSimulatorInfo.CurrentCell.RowIndex == 1 || dgvSimulatorInfo.CurrentCell.RowIndex == 2 || dgvSimulatorInfo.CurrentCell.RowIndex == 3 || dgvSimulatorInfo.CurrentCell.RowIndex == 4)))
            {
                dgvSimulatorInfo.ClearSelection();
            }
        }
    }
}
