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
    public partial class UcContentOfSetting : UserControl
    {
        public UcContentOfSetting()
        {
            InitializeComponent();
            UpdateGui_InitializeDgvMapFileList();
        }

        private void UpdateGui_InitializeDgvMapFileList()
        {
            dgvMapFileList.InvokeIfNecessary(() =>
            {
                DataGridView dgv = dgvMapFileList;

                dgv.RowHeadersVisible = false;
                dgv.ColumnHeadersVisible = false;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToResizeRows = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.BackgroundColor = Color.FromArgb(28, 28, 28);
                dgv.GridColor = Color.FromArgb(28, 28, 28);
                dgv.BorderStyle = BorderStyle.None;

                dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dgv.DefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28);
                dgv.DefaultCellStyle.ForeColor = Color.FromArgb(255, 255, 255);
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 255);
                dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 0, 0);

                dgv.Columns.Add("MapFileName", "MapFileName");
                dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }
            });

            dgvMapFileList.Rows.Add("File1.map");
            dgvMapFileList.Rows.Add("File31.map");
            dgvMapFileList.Rows.Add("File234.map");
        }
	}
}
