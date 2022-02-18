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
	public partial class UcSearchAutomaticDoorControl : UcSearch
	{
		public override string mKeyword { get; } = "AutomaticDoorControl";

		public UcSearchAutomaticDoorControl()
		{
			InitializeComponent();
		}

		protected override string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit, DateTime Date)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(Keyword))
			{
				result = $"SELECT * FROM (SELECT * FROM HistoryAutomaticDoorControlInfo WHERE {ConvertDateToSqlCommand("ReceiveTimestamp", Date)}) ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
			}
			else
			{
				result = $"SELECT * FROM (SELECT * FROM HistoryAutomaticDoorControlInfo WHERE {ConvertDateToSqlCommand("ReceiveTimestamp", Date)}) WHERE (ID LIKE '%{Keyword}%' OR AutomaticDoorName LIKE '%{Keyword}%' OR Command LIKE '%{Keyword}%' OR Cause LIKE '%{Keyword}%' OR SendState LIKE '%{Keyword}%') ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
			}
			return result;
		}
		protected override string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit, DateTime DateStart, DateTime DateEnd)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(Keyword))
			{
				result = $"SELECT * FROM (SELECT * FROM HistoryAutomaticDoorControlInfo WHERE {ConvertTimePeriodToSqlCommand("ReceiveTimestamp", DateStart, DateEnd)}) ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
			}
			else
			{
				result = $"SELECT * FROM (SELECT * FROM HistoryAutomaticDoorControlInfo WHERE {ConvertTimePeriodToSqlCommand("ReceiveTimestamp", DateStart, DateEnd)}) WHERE (ID LIKE '%{Keyword}%' OR AutomaticDoorName LIKE '%{Keyword}%' OR Command LIKE '%{Keyword}%' OR Cause LIKE '%{Keyword}%' OR SendState LIKE '%{Keyword}%') ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
			}
			return result;
		}
		protected override void UpdateGui_DgvSearchResult_Initialize()
		{
			this.InvokeIfNecessary(() =>
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
				dgv.Columns.Add("Name", "Name");
				dgv.Columns[1].Width = 300;
				dgv.Columns.Add("AutomaticDoorName", "AutomaticDoorName");
				dgv.Columns[2].Width = 200;
				dgv.Columns.Add("Command", "Command");
				dgv.Columns[3].Width = 120;
				dgv.Columns.Add("Cause", "Cause");
				dgv.Columns[4].Width = 300;
				dgv.Columns.Add("SendState", "SendState");
				dgv.Columns[5].Width = 120;
				dgv.Columns.Add("ReceiveTimestamp", "ReceiveTimestamp");
				dgv.Columns[6].Width = 190;
				dgv.Columns.Add("LastUpdateTimestamp", "LastUpdateTimestamp");
				dgv.Columns[7].Width = 190;

				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}
			});
		}
	}
}
