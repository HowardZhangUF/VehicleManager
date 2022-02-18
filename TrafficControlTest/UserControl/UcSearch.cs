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
using Library;

namespace TrafficControlTest.UserControl
{
	public abstract partial class UcSearch : System.Windows.Forms.UserControl
	{
		public delegate void EventHandlerSearchSuccessed(object Sender, DateTime OccurTime, string SearchCondition);

		public event EventHandlerSearchSuccessed SearchSuccessed;

		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableGridLineColor { get; set; } = Color.FromArgb(86, 86, 86);
		public Color TableHeaderBackColor { get; set; } = Color.FromArgb(8, 122, 233);
		public Color TableHeaderForeColor { get; set; } = Color.White;
		public Color TableOddRowBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableEvenRowBackColor { get; set; } = Color.FromArgb(42, 42, 42);
		public Color TableExceptionRowBackColor { get; set; } = Color.FromArgb(178, 34, 34);
		public Color TableRowForeColor { get; set; } = Color.White;
		public abstract string mKeyword { get; }

		private DatabaseAdapter rDatabaseAdapter = null;
		private int mDgvSearchResultRightClickRowIndex { get; set; } = -1;
		private int mDgvSearchResultRightClickColIndex { get; set; } = -1;

		public UcSearch()
		{
			InitializeComponent();
			UpdateGui_CbSearchCondition_Initialize();
			UpdateGui_CbLimit_Initialize();
			UpdateGui_CbHourFilterStart_Initialize();
			UpdateGui_CbHourFilterEnd_Initialize();
			UpdateGui_DgvSearchResult_Initialize();
		}
		public void Set(DatabaseAdapter DatabaseAdapter)
		{
			rDatabaseAdapter = DatabaseAdapter;
		}
		public void UpdateGui_FocusOnSearchTextBox()
		{
			this.InvokeIfNecessary(() =>
			{
				txtSearch.Focus();
			});
		}
		public DataGridView GetDgv()
		{
			return dgvSearchResult;
		}

		protected abstract string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit, DateTime Date);
		protected abstract string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit, DateTime DateStart, DateTime DateEnd);
		protected abstract void UpdateGui_DgvSearchResult_Initialize();
		protected virtual string ConvertDateToSqlCommand(string ColName, DateTime Date)
		{
			return $"({ColName} >= '{Date.ToString("yyyy-MM-dd")} 00:00:00.000' AND {ColName} < '{Date.AddDays(1).ToString("yyyy-MM-dd")} 00:00:00.000')";
		}
		protected virtual string ConvertTimePeriodToSqlCommand(string ColName, DateTime DateStart, DateTime DateEnd)
		{
			return $"({ColName} >= '{DateStart.ToString("yyyy-MM-dd HH:mm:ss.fff")}' AND {ColName} < '{DateEnd.ToString("yyyy-MM-dd HH:mm:ss.fff")}')";
		}
		protected virtual void RaiseEvent_SearchSuccessed(object Sender, DateTime OccurTime, string SearchCondition, bool Sync = true)
		{
			if (Sync)
			{
				SearchSuccessed?.Invoke(this, DateTime.Now, SearchCondition);
			}
			else
			{
				Task.Run(() => { SearchSuccessed?.Invoke(this, DateTime.Now, SearchCondition); });
			}
		}

		private void SearchAndDisplayResult()
		{
			switch (cbSearchCondition.SelectedItem.ToString())
			{
				case "Date":
					string searchCondition1 = string.Empty;
					searchCondition1 += $"Keyword: {txtSearch.Text}\n";
					searchCondition1 += $"Limit: {cbLimit.SelectedItem.ToString()}\n";
					searchCondition1 += $"Date: {dtpDateFilterStart.Value.Date.ToString("yyyy-MM-dd")}";
					SearchAndDisplayResult(ConvertSearchOptionsToSqlCommand(txtSearch.Text, int.Parse(cbLimit.SelectedItem.ToString()), dtpDateFilterStart.Value.Date), searchCondition1);
					break;
				case "TimePeriod":
					int hourStart = int.Parse(cbHourFilterStart.SelectedItem.ToString().Substring(0, 2));
					int hourEnd = int.Parse(cbHourFilterEnd.SelectedItem.ToString().Substring(0, 2));
					DateTime DateStart = dtpDateFilterStart.Value.Date.AddHours(hourStart);
					DateTime DateEnd = dtpDateFilterEnd.Value.Date.AddHours(hourEnd);
					string searchCondition2 = string.Empty;
					searchCondition2 += $"Keyword: {txtSearch.Text}\n";
					searchCondition2 += $"Limit: {cbLimit.SelectedItem.ToString()}\n";
					searchCondition2 += $"StartDate: {DateStart.ToString("yyyy-MM-dd HH:mm")}\n";
					searchCondition2 += $"EndDate: {DateEnd.ToString("yyyy-MM-dd HH:mm")}";
					SearchAndDisplayResult(ConvertSearchOptionsToSqlCommand(txtSearch.Text, int.Parse(cbLimit.SelectedItem.ToString()), DateStart, DateEnd), searchCondition2);
					break;
				default:
					break;
			}
		}
		private void SearchAndDisplayResult(string Command, string SearchCondition)
		{
			if (rDatabaseAdapter == null) return;

			DataTable searchResult = rDatabaseAdapter.ExecuteQueryCommand(Command)?.Tables[0];
			if (searchResult != null && searchResult.Rows != null && searchResult.Rows.Count > 0)
			{
				UpdateGui_DgvSearchResult_ClearRows();
				UpdateGui_DgvSearchResult_AddRows(searchResult.Rows);
				RaiseEvent_SearchSuccessed(this, DateTime.Now, SearchCondition);
			}
			else
			{
				MessageBox.Show("No Matches!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
		private void UpdateGui_CbSearchCondition_Initialize()
		{
			this.InvokeIfNecessary(() =>
			{
				cbSearchCondition.Items.Clear();
				cbSearchCondition.Items.Add("Date");
				cbSearchCondition.Items.Add("TimePeriod");
				cbSearchCondition.SelectedIndex = 0;
			});
		}
		private void UpdateGui_CbLimit_Initialize()
		{
			this.InvokeIfNecessary(() =>
			{
				cbLimit.Items.Clear();
				cbLimit.Items.Add("100");
				cbLimit.Items.Add("300");
				cbLimit.Items.Add("500");
				cbLimit.Items.Add("1000");
				cbLimit.SelectedIndex = 0;
			});
		}
		private void UpdateGui_CbHourFilterStart_Initialize()
		{
			this.InvokeIfNecessary(() =>
			{
				cbHourFilterStart.Items.Clear();
				for (int i = 0; i < 24; ++i)
				{
					cbHourFilterStart.Items.Add($"{i.ToString().PadLeft(2, '0')}:00");
				}
				cbHourFilterStart.SelectedIndex = 0;
			});
		}
		private void UpdateGui_CbHourFilterEnd_Initialize()
		{
			this.InvokeIfNecessary(() =>
			{
				cbHourFilterEnd.Items.Clear();
				for (int i = 0; i < 24; ++i)
				{
					cbHourFilterEnd.Items.Add($"{i.ToString().PadLeft(2, '0')}:00");
				}
				cbHourFilterEnd.SelectedIndex = 0;
			});
		}
		private void UpdateGui_DgvSearchResult_AddRow(params string[] RowData)
		{
			this.InvokeIfNecessary(() =>
			{
				dgvSearchResult.Rows.Add(RowData);
				UpdateGui_DgvSearchResult_RefreshRowBackColor(dgvSearchResult.Rows.Count - 1);
			});
		}
		private void UpdateGui_DgvSearchResult_AddRows(DataRowCollection DataRows)
		{
			this.InvokeIfNecessary(() =>
			{
				for (int i = 0; i < DataRows.Count; ++i)
				{
					List<string> datas = new List<string>();
					datas.Add((i + 1).ToString());
					for (int j = 0; j < DataRows[i].ItemArray.Length; ++j)
					{
						if (DataRows[i].ItemArray[j] is DateTime)
						{
							DateTime tmp = Convert.ToDateTime(DataRows[i].ItemArray[j]);
							if (tmp == DateTime.MinValue)
							{
								datas.Add(string.Empty);
							}
							else
							{
								datas.Add(tmp.ToString(Library.Library.TIME_FORMAT));
							}
						}
						else
						{
							datas.Add(DataRows[i].ItemArray[j].ToString());
						}
					}
					UpdateGui_DgvSearchResult_AddRow(datas.ToArray());
				}
			});
		}
		private void UpdateGui_DgvSearchResult_InsertRow(int RowIndex, params string[] RowData)
		{
			this.InvokeIfNecessary(() =>
			{
				if (RowIndex <= dgvSearchResult.Rows.Count)
				{
					dgvSearchResult.Rows.Insert(RowIndex, RowData);
				}
			});
		}
		private void UpdateGui_DgvSearchResult_RemoveRow(int RowIndex)
		{
			this.InvokeIfNecessary(() =>
			{
				if (RowIndex < dgvSearchResult.Rows.Count)
				{
					dgvSearchResult.Rows.RemoveAt(RowIndex);
				}
			});
		}
		private void UpdateGui_DgvSearchResult_ClearRows()
		{
			this.InvokeIfNecessary(() =>
			{
				if (dgvSearchResult.Rows.Count > 0)
				{
					dgvSearchResult.Rows.Clear();
				}
			});
		}
		private void UpdateGui_DgvSearchResult_RefreshRowsBackColor()
		{
			this.InvokeIfNecessary(() =>
			{
				if (dgvSearchResult != null && dgvSearchResult.Rows != null && dgvSearchResult.Rows.Count > 0)
				{
					for (int i = 0; i < dgvSearchResult.Rows.Count; ++i)
					{
						UpdateGui_DgvSearchResult_RefreshRowBackColor(i);
					}
				}
			});
		}
		private void UpdateGui_DgvSearchResult_RefreshRowBackColor(int RowIndex)
		{
			this.InvokeIfNecessary(() =>
			{
				if (RowIndex % 2 == 0)
				{
					if (dgvSearchResult.Rows[RowIndex].DefaultCellStyle.BackColor != TableEvenRowBackColor) dgvSearchResult.Rows[RowIndex].DefaultCellStyle.BackColor = TableEvenRowBackColor;
				}
				else
				{
					if (dgvSearchResult.Rows[RowIndex].DefaultCellStyle.BackColor != TableOddRowBackColor) dgvSearchResult.Rows[RowIndex].DefaultCellStyle.BackColor = TableOddRowBackColor;
				}
			});
		}
		private void txtSearch_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Enter)
				{
					SearchAndDisplayResult();
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnSearch_Click(object sender, EventArgs e)
		{
			try
			{
				SearchAndDisplayResult();
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void cbSearchCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				switch (cbSearchCondition.SelectedItem.ToString())
				{
					case "Date":
						cbHourFilterStart.Visible = false;
						label1.Visible = false;
						dtpDateFilterEnd.Visible = false;
						cbHourFilterEnd.Visible = false;
						break;
					case "TimePeriod":
						cbHourFilterStart.Visible = true;
						label1.Visible = true;
						dtpDateFilterEnd.Visible = true;
						cbHourFilterEnd.Visible = true;
						break;
					default:
						break;
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void dgvSearchResult_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					var hit = dgvSearchResult.HitTest(e.X, e.Y);
					mDgvSearchResultRightClickRowIndex = hit.RowIndex;
					mDgvSearchResultRightClickColIndex = hit.ColumnIndex;
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void cmenuItemCopyCellValue_Click(object sender, EventArgs e)
		{
			try
			{
				if (mDgvSearchResultRightClickRowIndex >= 0 && mDgvSearchResultRightClickRowIndex < dgvSearchResult.RowCount && mDgvSearchResultRightClickColIndex >= 0 && mDgvSearchResultRightClickColIndex < dgvSearchResult.ColumnCount && !string.IsNullOrEmpty(dgvSearchResult.Rows[mDgvSearchResultRightClickRowIndex].Cells[mDgvSearchResultRightClickColIndex].Value.ToString()))
				{
					Clipboard.SetText(dgvSearchResult.Rows[mDgvSearchResultRightClickRowIndex].Cells[mDgvSearchResultRightClickColIndex].Value.ToString());
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
	}
}
