using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Configure;
using TrafficControlTest.Module.Map;

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
			UpdateGui_DgvMapManagementSetting_Initialize();
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
				Configurator.ConfigFileLoaded += HandleEvent_ConfiguratorConfigLoaded;
				Configurator.ConfigurationUpdated += HandleEvent_ConfiguratorConfigurationUpdated;
			}
		}
		private void UnsubscribeEvent_IConfigurator(IConfigurator Configurator)
		{
			if (Configurator != null)
			{
				Configurator.ConfigFileLoaded -= HandleEvent_ConfiguratorConfigLoaded;
				Configurator.ConfigurationUpdated -= HandleEvent_ConfiguratorConfigurationUpdated;
			}
		}
		private void HandleEvent_ConfiguratorConfigLoaded(object Sender, ConfigFileLoadedEventArgs Args)
		{
			List<string[]> rowDataCollection = rConfigurator.GetConfigDataGridViewRowDataCollection();

			UpdateGui_DgvSettings_RowsClear();
			for (int i = 0; i < rowDataCollection.Count; ++i)
			{
				if (rowDataCollection[i][1] == "MapManagementSetting") continue; // MapManagement 使用不同的介面來進行設定
				UpdateGui_DgvSettings_AddRow(rowDataCollection[i]);
			}
			UpdateGui_DgvSettings_TlpSettings_UpdateHeight();
			UpdateGui_DgvSettings_UpdateRowsBackColor();
			UpdateGui_DgvSettings_ClearSelection();

			for (int i = 0; i < rowDataCollection.Count; ++i)
			{
				if (rowDataCollection[i][1] == "MapManagementSetting")
				{
					UpdateGui_DgvMapManagementSetting_RowsClear();
					UpdateGui_DgvMapManagementSetting_UpdateData(rowDataCollection[i][2]);
				}
			}
			UpdateGui_DgvMapManagementSetting_TlpMapManagementSetting_UpdateHeight();
			UpdateGui_DgvMapManagementSetting_UpdateRowsBackColor();
			UpdateGui_DgvMapManagementSetting_ClearSelection();
		}
		private void HandleEvent_ConfiguratorConfigurationUpdated(object sender, ConfigurationUpdatedEventArgs e)
		{
			if ((e.Configuration.mCategory == "MapFileManager" || e.Configuration.mCategory == "MapManagerUpdater") && e.Configuration.mName == "MapManagementSetting")
			{
				UpdateGui_DgvMapManagementSetting_RowsClear();
				UpdateGui_DgvMapManagementSetting_UpdateData(e.Configuration.mValue);
				UpdateGui_DgvMapManagementSetting_TlpMapManagementSetting_UpdateHeight();
				UpdateGui_DgvMapManagementSetting_UpdateRowsBackColor();
				UpdateGui_DgvMapManagementSetting_ClearSelection();
			}
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
		private void UpdateGui_DgvSettings_TlpSettings_UpdateHeight()
		{
			dgvSettings.InvokeIfNecessary(() =>
			{
				int height = 0;
				height += dgvSettings.ColumnHeadersHeight;
				foreach (DataGridViewRow dgvr in dgvSettings.Rows)
				{
					height += dgvr.Height;
				}
				dgvSettings.Height = height;
				tlpSettings.Height = 60 + height + dgvSettings.RowTemplate.Height;
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
		private void UpdateGui_DgvMapManagementSetting_Initialize()
		{
			dgvMapManagementSetting.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvMapManagementSetting;

				dgv.SelectionChanged += ((sender, e) => { if (dgv.CurrentCell.ColumnIndex != dgv.Columns["RegionName"].Index && dgv.CurrentCell.ColumnIndex != dgv.Columns["RegionMember"].Index) dgv.ClearSelection(); });

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

				dgv.Columns.Add("RegionID", "RegionID");
				dgv.Columns[0].Width = 100;
				dgv.Columns.Add("RegionName", "RegionName");
				dgv.Columns[1].Width = 200;
				dgv.Columns.Add("RegionMember", "RegionMember");
				dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dgv.Columns.Add("CurrentMapName", "CurrentMapName");
				dgv.Columns[3].Width = 240;
				dgv.Columns.Add("CurrentMapRange", "CurrentMapRange");
				dgv.Columns[4].Width = 360;
				
				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}

				dgv.Columns["RegionName"].ReadOnly = false;
				dgv.Columns["RegionMember"].ReadOnly = false;
			});
		}
		private void UpdateGui_DgvMapManagementSetting_UpdateData(string JsonString)
		{
			dgvMapManagementSetting.InvokeIfNecessary(() =>
			{
				MapManagementSetting tmp = MapManagementSetting.FromJsonString(JsonString);
				List<string[]> rowDatas = ConvertMapRegionSettingsToDataGridViewRows(tmp.mRegionSettings.Values.ToList());
				for (int i = 0; i < rowDatas.Count; ++i)
				{
					dgvMapManagementSetting.Rows.Add(rowDatas[i]);
					if (rowDatas[i][0] == "0") dgvMapManagementSetting.Rows[i].ReadOnly = true; // 第 0 個區域固定存在，沒有被分區的自走車，皆會歸屬於第 0 個區域，所以此區域不開放編輯
				}
			});
		}
		private void UpdateGui_DgvMapManagementSetting_AddEmptyRow()
		{
			dgvMapManagementSetting.InvokeIfNecessary(() =>
			{
				int num = dgvMapManagementSetting.RowCount;
				dgvMapManagementSetting.Rows.Add(num.ToString(), $"Region{num.ToString().PadLeft(3, '0')}", string.Empty, string.Empty, string.Empty);
			});
		}
		private void UpdateGui_DgvMapManagementSetting_RowsClear()
		{
			dgvMapManagementSetting.InvokeIfNecessary(() =>
			{
				dgvMapManagementSetting.Rows.Clear();
			});
		}
		private void UpdateGui_DgvMapManagementSetting_TlpMapManagementSetting_UpdateHeight()
		{
			dgvMapManagementSetting.InvokeIfNecessary(() =>
			{
				int height = 0;
				height += dgvMapManagementSetting.ColumnHeadersHeight;
				foreach (DataGridViewRow dgvr in dgvMapManagementSetting.Rows)
				{
					height += dgvr.Height;
				}
				dgvMapManagementSetting.Height = height;
				tlpMapManagementSetting.Height = 60 + height + dgvMapManagementSetting.RowTemplate.Height;
			});
		}
		private void UpdateGui_DgvMapManagementSetting_UpdateRowsBackColor()
		{
			dgvMapManagementSetting.InvokeIfNecessary(() =>
			{
				if (dgvMapManagementSetting != null && dgvMapManagementSetting.Rows != null && dgvMapManagementSetting.Rows.Count > 0)
				{
					for (int i = 0; i < dgvMapManagementSetting.Rows.Count; ++i)
					{
						UpdateGui_DgvMapManagementSetting_UpdateRowBackColor(i);
					}
				}
			});
		}
		private void UpdateGui_DgvMapManagementSetting_UpdateRowBackColor(int RowIndex)
		{
			dgvMapManagementSetting.InvokeIfNecessary(() =>
			{
				if (RowIndex % 2 == 0)
				{
					if (dgvMapManagementSetting.Rows[RowIndex].DefaultCellStyle.BackColor != TableEvenRowBackColor) dgvMapManagementSetting.Rows[RowIndex].DefaultCellStyle.BackColor = TableEvenRowBackColor;
				}
				else
				{
					if (dgvMapManagementSetting.Rows[RowIndex].DefaultCellStyle.BackColor != TableOddRowBackColor) dgvMapManagementSetting.Rows[RowIndex].DefaultCellStyle.BackColor = TableOddRowBackColor;
				}
			});
		}
		private void UpdateGui_DgvMapManagementSetting_ClearSelection()
		{
			dgvMapManagementSetting.InvokeIfNecessary(() =>
			{
				dgvMapManagementSetting.ClearSelection();
			});
		}
		private void dgvSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex == dgvSettings.Columns["Value"].Index)
				{
					if (!rConfigurator.SetValue(dgvSettings.Rows[e.RowIndex].Cells["Category"].Value.ToString() + "/" + dgvSettings.Rows[e.RowIndex].Cells["Name"].Value.ToString(), dgvSettings.Rows[e.RowIndex].Cells["Value"].Value.ToString()))
					{
						dgvSettings.Rows[e.RowIndex].Cells["Value"].Value = rConfigurator.GetValue(dgvSettings.Rows[e.RowIndex].Cells["Category"].Value.ToString() + "/" + dgvSettings.Rows[e.RowIndex].Cells["Name"].Value.ToString()).ToString();
					}
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
		private void dgvMapManagementSetting_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				List<MapRegionSetting> settings = ConvertDataGridViewRowsToMapRegionSettings(dgvMapManagementSetting).OrderBy(o => o.mRegionId).ToList();
				if (settings != null && settings.Count > 0)
				{
					settings = settings.OrderBy(o => o.mRegionId).ToList();
					// 先讀取舊的值
					MapManagementSetting tmpMapManagementSetting = MapManagementSetting.FromJsonString(rConfigurator.GetValue("MapFileManager/MapManagementSetting"));

					// 再將值更新
					tmpMapManagementSetting.mRegionSettings.Clear();
					for (int i = 0; i < settings.Count; ++i)
					{
						tmpMapManagementSetting.mRegionSettings.Add(settings[i].mRegionId, settings[i]);
					}
					string newValue = tmpMapManagementSetting.ToJsonString();

					// 再儲存至 Config 類別裡
					rConfigurator.SetValue("MapFileManager/MapManagementSetting", newValue);
					rConfigurator.SetValue("MapManagerUpdater/MapManagementSetting", newValue);
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
		private void cmenuItemAddRegion_Click(object sender, EventArgs e)
		{
			try
			{
				UpdateGui_DgvMapManagementSetting_AddEmptyRow();
				UpdateGui_DgvMapManagementSetting_TlpMapManagementSetting_UpdateHeight();
				UpdateGui_DgvMapManagementSetting_UpdateRowsBackColor();
				UpdateGui_DgvMapManagementSetting_ClearSelection();
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
		private static List<MapRegionSetting> ConvertDataGridViewRowsToMapRegionSettings(DataGridView Dgv)
		{
			List<MapRegionSetting> result = new List<MapRegionSetting>();
			if (Dgv.Rows.Count > 0 && Dgv.Columns.Count == 5)
			{
				for (int i = 0; i < Dgv.Rows.Count; ++i)
				{
					MapRegionSetting tmp = new MapRegionSetting();
					tmp.Set(int.Parse(Dgv.Rows[i].Cells[0].Value.ToString()), Dgv.Rows[i].Cells[1].Value.ToString(), (Dgv.Rows[i].Cells[2].Value == null ? string.Empty : Dgv.Rows[i].Cells[2].Value.ToString()), Dgv.Rows[i].Cells[3].Value.ToString(), Dgv.Rows[i].Cells[4].Value.ToString());
					result.Add(tmp);
				}
			}
			return result;
		}
		private static List<string[]> ConvertMapRegionSettingsToDataGridViewRows(List<MapRegionSetting> MapRegionSettings)
		{
			if (MapRegionSettings != null && MapRegionSettings.Count > 0)
			{
				return MapRegionSettings.Select(o => new string[] { o.mRegionId.ToString(), o.mRegionName, o.mRegionMember, o.mCurrentMapName, o.mCurrentMapRange }).ToList();
			}
			else
			{
				return new List<string[]>();
			}
		}
	}
}
