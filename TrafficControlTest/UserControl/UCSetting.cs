using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Configure;

namespace TrafficControlTest.UserControl
{
	public partial class UcSetting : System.Windows.Forms.UserControl
	{
		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableGridLineColor { get; set; } = Color.FromArgb(86, 86, 86);
		public Color TableHeaderBackColor { get; set; } = Color.FromArgb(0, 122, 204);
		public Color TableHeaderForeColor { get; set; } = Color.White;
		public Color TableOddRowBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableEvenRowBackColor { get; set; } = Color.FromArgb(42, 42, 42);
		public Color TableRowForeColor { get; set; } = Color.White;

		private IConfigurator rConfigurator = null;

		public UcSetting()
		{
			InitializeComponent();
			UpdateGui_DgvSettings_Initialize();
		}
		public void Set(IConfigurator Configurator)
		{
			UnsubscribeEvent_IConfigurator(rConfigurator);
			rConfigurator = Configurator;
			SubscribeEvent_IConfigurator(rConfigurator);
		}

		private void SubscribeEvent_IConfigurator(IConfigurator Configurator)
		{
			if (Configurator != null)
			{
				Configurator.ConfigLoaded += HandleEvent_ConfiguratorConfigLoaded;
			}
		}
		private void UnsubscribeEvent_IConfigurator(IConfigurator Configurator)
		{
			if (Configurator != null)
			{
				Configurator.ConfigLoaded -= HandleEvent_ConfiguratorConfigLoaded;
			}
		}
		private void HandleEvent_ConfiguratorConfigLoaded(DateTime OccurTime)
		{
			UpdateGui_DgvSettings_RowsClear();
			List<string[]> rowDataCollection = rConfigurator.GetConfigDataGridViewRowDataCollection();
			for (int i = 0; i < rowDataCollection.Count; ++i)
			{
				UpdateGui_DgvSettings_AddRow(rowDataCollection[i]);
			}
			UpdateGui_DgvSettings_UpdateRowsBackColor();
			UpdateGui_DgvSettings_ClearSelection();
		}
		private void UpdateGui_DgvSettings_Initialize()
		{
			dgvSettings.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvSettings;

				dgv.SelectionChanged += ((sender, e) => { if (dgv.CurrentCell.ColumnIndex != dgv.Columns["Value"].Index) dgv.ClearSelection(); });

				dgv.RowHeadersVisible = false;
				dgv.AllowUserToAddRows = false;
				dgv.AllowUserToResizeRows = false;
				dgv.AllowUserToResizeColumns = false;
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
				dgv.DefaultCellStyle.BackColor = TableOddRowBackColor;
				dgv.DefaultCellStyle.ForeColor = TableRowForeColor;
				dgv.RowTemplate.Height = 40;
				
				dgv.Columns.Add("Category", "Category");
				dgv.Columns[0].Width = 200;
				dgv.Columns.Add("Name", "Name");
				dgv.Columns[1].Width = 220;
				dgv.Columns.Add("Value", "Value");
				dgv.Columns[2].Width = 70;
				dgv.Columns.Add("Description", "Description");
				dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dgv.Columns.Add("Min", "Min");
				dgv.Columns[4].Width = 70;
				dgv.Columns.Add("Max", "Max");
				dgv.Columns[5].Width = 70;
				dgv.Columns.Add("Default", "Default");
				dgv.Columns[6].Width = 70;

				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}

				dgv.Columns["Value"].ReadOnly = false;
			});
		}
		private void UpdateGui_DgvSettings_AddRow(string[] RowData)
		{
			UpdateGui_DgvSettings_InsertRow(dgvSettings.RowCount, RowData);
		}
		private void UpdateGui_DgvSettings_InsertRow(int RowIndex, string[] RowData)
		{
			dgvSettings.InvokeIfNecessary(() =>
			{
				if (RowIndex <= dgvSettings.Rows.Count)
				{
					dgvSettings.Rows.Insert(RowIndex, RowData);
				}
			});
		}
		private void UpdateGui_DgvSettings_RowsClear()
		{
			dgvSettings.InvokeIfNecessary(() =>
			{
				dgvSettings.Rows.Clear();
			});
		}
		private void UpdateGui_DgvSettings_UpdateRowsBackColor()
		{
			dgvSettings.InvokeIfNecessary(() =>
			{
				if (dgvSettings != null && dgvSettings.Rows != null && dgvSettings.Rows.Count > 0)
				{
					for (int i = 0; i < dgvSettings.Rows.Count; ++i)
					{
						UpdateGui_DgvSettings_UpdateRowBackColor(i);
					}
				}
			});
		}
		private void UpdateGui_DgvSettings_UpdateRowBackColor(int RowIndex)
		{
			dgvSettings.InvokeIfNecessary(() =>
			{
				if (RowIndex % 2 == 0)
				{
					if (dgvSettings.Rows[RowIndex].DefaultCellStyle.BackColor != TableEvenRowBackColor) dgvSettings.Rows[RowIndex].DefaultCellStyle.BackColor = TableEvenRowBackColor;
				}
				else
				{
					if (dgvSettings.Rows[RowIndex].DefaultCellStyle.BackColor != TableOddRowBackColor) dgvSettings.Rows[RowIndex].DefaultCellStyle.BackColor = TableOddRowBackColor;
				}
			});
		}
		private void UpdateGui_DgvSettings_ClearSelection()
		{
			dgvSettings.InvokeIfNecessary(() =>
			{
				dgvSettings.ClearSelection();
			});
		}
		private void dgvSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == dgvSettings.Columns["Value"].Index)
			{
				if (!rConfigurator.SetValue(dgvSettings.Rows[e.RowIndex].Cells["Category"].Value.ToString() + "/" + dgvSettings.Rows[e.RowIndex].Cells["Name"].Value.ToString(), dgvSettings.Rows[e.RowIndex].Cells["Value"].Value.ToString()))
				{
					dgvSettings.Rows[e.RowIndex].Cells["Value"].Value = rConfigurator.GetValue(dgvSettings.Rows[e.RowIndex].Cells["Category"].Value.ToString() + "/" + dgvSettings.Rows[e.RowIndex].Cells["Name"].Value.ToString()).ToString();
				}
			}
		}
	}
}
