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

namespace TrafficControlTest.UserControl
{
	public partial class UcLogSearch : System.Windows.Forms.UserControl
	{
		public delegate void EventHandlerSearchSuccessed(object Sender, DateTime OccurTime, string Keyword, int Limit);

		public event EventHandler ClickOnCloseButton;
		public event EventHandlerSearchSuccessed SearchSuccessed;

		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableGridLineColor { get; set; } = Color.FromArgb(86, 86, 86);
		public Color TableHeaderBackColor { get; set; } = Color.FromArgb(8, 122, 233);
		public Color TableHeaderForeColor { get; set; } = Color.White;
		public Color TableOddRowBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableEvenRowBackColor { get; set; } = Color.FromArgb(42, 42, 42);
		public Color TableExceptionRowBackColor { get; set; } = Color.FromArgb(178, 34, 34);
		public Color TableRowForeColor { get; set; } = Color.White;

		private DatabaseAdapter rDatabaseAdapter = null;

		public UcLogSearch()
		{
			InitializeComponent();
			UpdateGui_CbLimit_Initialize();
			UpdateGui_DgvLogSearchResult_Initialize();
		}
		public void Set(DatabaseAdapter DatabaseAdapter)
		{
			rDatabaseAdapter = DatabaseAdapter;
		}
		public void FocusOnSearchTextBox()
		{
			txtSearch.Focus();
		}
		public void DoDefaultSearch()
		{
			SearchAndDisplayResult(string.Empty, int.Parse(cbLimit.Items[0].ToString()));
		}

		private void SearchAndDisplayResult()
		{
			SearchAndDisplayResult(txtSearch.Text, int.Parse(cbLimit.SelectedItem.ToString()));
		}
		private void SearchAndDisplayResult(string Keyword, int Limit)
		{
			if (rDatabaseAdapter == null) return;

			string command = string.Empty;
			if (string.IsNullOrEmpty(Keyword) || Keyword == "Recent")
			{
				command = $"SELECT * FROM GeneralLog ORDER BY Timestamp DESC LIMIT {Limit.ToString()}";
			}
			else
			{
				command = $"SELECT * FROM GeneralLog WHERE (SubCategory LIKE '%{Keyword}%' OR Message LIKE '%{Keyword}%') ORDER BY Timestamp DESC LIMIT {Limit.ToString()}";
			}

			DataTable searchResult = rDatabaseAdapter.ExecuteQueryCommand(command)?.Tables[0];
			if (searchResult != null && searchResult.Rows != null && searchResult.Rows.Count > 0)
			{
				UpdateGui_DgvLogSearchResult_ClearRows();
				UpdateGui_DgvLogSearchResult_AddRows(searchResult.Rows);
				SearchSuccessed?.Invoke(this, DateTime.Now, string.IsNullOrEmpty(Keyword) ? "Recent" : Keyword, Limit);
			}
			else
			{
				MessageBox.Show("No Matches!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
		private void UpdateGui_CbLimit_Initialize()
		{
			cbLimit.Items.Add("100");
			cbLimit.Items.Add("300");
			cbLimit.Items.Add("500");
			cbLimit.Items.Add("1000");
			cbLimit.SelectedIndex = 0;
		}
		private void UpdateGui_DgvLogSearchResult_Initialize()
		{
			DataGridView dgv = dgvLogSearchResult;

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

			dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
			dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
			dgv.DefaultCellStyle.BackColor = TableEvenRowBackColor;
			dgv.DefaultCellStyle.ForeColor = TableRowForeColor;
			dgv.RowTemplate.Height = 25;

			dgv.Columns.Add("No", "No");
			dgv.Columns[0].Width = 50;
			dgv.Columns.Add("Date", "Date");
			dgv.Columns[1].Width = 175;
			dgv.Columns.Add("Category", "Category");
			dgv.Columns[2].Width = 200;
			dgv.Columns.Add("SubCategory", "SubCategory");
			dgv.Columns[3].Width = 200;
			dgv.Columns.Add("Message", "Message");
			dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
				column.ReadOnly = true;
			}
		}
		private void UpdateGui_DgvLogSearchResult_AddRow(params string[] RowData)
		{
			dgvLogSearchResult.Rows.Add(RowData);
			UpdateGui_DgvLogSearchResult_RefreshRowBackColor(dgvLogSearchResult.Rows.Count - 1);
		}
		private void UpdateGui_DgvLogSearchResult_AddRows(DataRowCollection DataRows)
		{
			for (int i = 0; i < DataRows.Count; ++i)
			{
				List<string> datas = new List<string>();
				datas.Add((i + 1).ToString());
				for (int j = 0; j < DataRows[i].ItemArray.Length; ++j)
				{
					if (DataRows[i].ItemArray[j] is DateTime)
					{
						datas.Add(Convert.ToDateTime(DataRows[i].ItemArray[j]).ToString(Library.Library.TIME_FORMAT));
					}
					else
					{
						datas.Add(DataRows[i].ItemArray[j].ToString());
					}
				}
				UpdateGui_DgvLogSearchResult_AddRow(datas.ToArray());
			}
		}
		private void UpdateGui_DgvLogSearchResult_InsertRow(int RowIndex, params string[] RowData)
		{
			if (RowIndex <= dgvLogSearchResult.Rows.Count)
			{
				dgvLogSearchResult.Rows.Insert(RowIndex, RowData);
			}
		}
		private void UpdateGui_DgvLogSearchResult_RemoveRow(int RowIndex)
		{
			if (RowIndex < dgvLogSearchResult.Rows.Count)
			{
				dgvLogSearchResult.Rows.RemoveAt(RowIndex);
			}
		}
		private void UpdateGui_DgvLogSearchResult_ClearRows()
		{
			if (dgvLogSearchResult.Rows.Count > 0)
			{
				dgvLogSearchResult.Rows.Clear();
			}
		}
		private void UpdateGui_DgvLogSearchResult_RefreshRowsBackColor()
		{
			if (dgvLogSearchResult != null && dgvLogSearchResult.Rows != null && dgvLogSearchResult.Rows.Count > 0)
			{
				for (int i = 0; i < dgvLogSearchResult.Rows.Count; ++i)
				{
					UpdateGui_DgvLogSearchResult_RefreshRowBackColor(i);
				}
			}
		}
		private void UpdateGui_DgvLogSearchResult_RefreshRowBackColor(int RowIndex)
		{
			if (RowIndex % 2 == 0)
			{
				if (dgvLogSearchResult.Rows[RowIndex].DefaultCellStyle.BackColor != TableEvenRowBackColor) dgvLogSearchResult.Rows[RowIndex].DefaultCellStyle.BackColor = TableEvenRowBackColor;
			}
			else
			{
				if (dgvLogSearchResult.Rows[RowIndex].DefaultCellStyle.BackColor != TableOddRowBackColor) dgvLogSearchResult.Rows[RowIndex].DefaultCellStyle.BackColor = TableOddRowBackColor;
			}
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			ClickOnCloseButton?.Invoke(this, e);
		}
		private void txtSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SearchAndDisplayResult();
			}
		}
		private void btnSearch_Click(object sender, EventArgs e)
		{
			SearchAndDisplayResult();
		}
		private void cbLimit_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbLimit.SelectedItem != null)
			{
				SearchAndDisplayResult();
			}
		}
	}
}
