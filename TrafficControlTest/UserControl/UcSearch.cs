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
	public abstract partial class UcSearch : System.Windows.Forms.UserControl
	{
		public delegate void EventHandlerSearchSuccessed(object Sender, DateTime OccurTime, string Keyword, int Limit);

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

		public UcSearch()
		{
			InitializeComponent();
			UpdateGui_CbLimit_Initialize();
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
		public void DoDefaultSearch()
		{
			SearchAndDisplayResult(string.Empty, int.Parse(cbLimit.Items[0].ToString()));
		}
		public DataGridView GetDgv()
		{
			return dgvSearchResult;
		}

		protected abstract string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit);
		protected abstract void UpdateGui_DgvSearchResult_Initialize();
		protected virtual void RaiseEvent_SearchSuccessed(object Sender, DateTime OccurTime, string Keyword, int Limit, bool Sync = true)
		{
			if (Sync)
			{
				SearchSuccessed?.Invoke(this, DateTime.Now, string.IsNullOrEmpty(Keyword) ? "Recent" : Keyword, Limit);
			}
			else
			{
				Task.Run(() => { SearchSuccessed?.Invoke(this, DateTime.Now, string.IsNullOrEmpty(Keyword) ? "Recent" : Keyword, Limit); });
			}
		}

		private void SearchAndDisplayResult()
		{
			SearchAndDisplayResult(txtSearch.Text, int.Parse(cbLimit.SelectedItem.ToString()));
		}
		private void SearchAndDisplayResult(string Keyword, int Limit)
		{
			if (rDatabaseAdapter == null) return;

			string command = ConvertSearchOptionsToSqlCommand(Keyword, Limit);

			DataTable searchResult = rDatabaseAdapter.ExecuteQueryCommand(command)?.Tables[0];
			if (searchResult != null && searchResult.Rows != null && searchResult.Rows.Count > 0)
			{
				UpdateGui_DgvSearchResult_ClearRows();
				UpdateGui_DgvSearchResult_AddRows(searchResult.Rows);
				RaiseEvent_SearchSuccessed(this, DateTime.Now, string.IsNullOrEmpty(Keyword) ? "Recent" : Keyword, Limit);
			}
			else
			{
				// 如果做 Default Search 時沒有找到資料，則代表資料庫剛建立所以沒有資料，此時不跳出提醒視窗
				if (!string.IsNullOrEmpty(Keyword))
				{
					MessageBox.Show("No Matches!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}
		private void UpdateGui_CbLimit_Initialize()
		{
			this.InvokeIfNecessary(() =>
			{
				cbLimit.Items.Add("100");
				cbLimit.Items.Add("300");
				cbLimit.Items.Add("500");
				cbLimit.Items.Add("1000");
				cbLimit.SelectedIndex = 0;
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
							datas.Add(Convert.ToDateTime(DataRows[i].ItemArray[j]).ToString(Library.Library.TIME_FORMAT));
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
