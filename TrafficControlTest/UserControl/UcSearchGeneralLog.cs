using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public class UcSearchGeneralLog : UcSearch
	{
		public override string mKeyword { get; } = "General";

		public UcSearchGeneralLog() : base() { }

		protected override string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(Keyword) || Keyword == "Recent")
			{
				result = $"SELECT Timestamp, Category, SubCategory, Message FROM GeneralLog ORDER BY No DESC LIMIT {Limit.ToString()}";
			}
			else
			{
				result = $"SELECT Timestamp, Category, SubCategory, Message FROM GeneralLog WHERE (Category LIKE '%{Keyword}%' OR SubCategory LIKE '%{Keyword}%' OR Message LIKE '%{Keyword}%') ORDER BY No DESC LIMIT {Limit.ToString()}";
			}
			return result;
		}
		protected override void UpdateGui_DgvSearchResult_Initialize()
		{
			DataGridView dgv = GetDgv();

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
	}
}
