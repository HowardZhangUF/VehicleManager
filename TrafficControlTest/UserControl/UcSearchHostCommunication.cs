using Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Library;

namespace TrafficControlTest.UserControl
{
	class UcSearchHostCommunication : UcSearch
	{
		public override string mKeyword { get; } = "HostCommunication";

		public UcSearchHostCommunication() : base() { }

		protected override string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit, DateTime Date)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(Keyword))
			{
				result = $"SELECT ReceiveTimestamp, Event, IPPort, Data FROM (SELECT * FROM HistoryHostCommunicationInfo WHERE {ConvertDateToSqlCommand("ReceiveTimestamp", Date)}) ORDER BY No DESC LIMIT {Limit.ToString()}";
			}
			else
			{
				result = $"SELECT ReceiveTimestamp, Event, IPPort, Data FROM (SELECT * FROM HistoryHostCommunicationInfo WHERE {ConvertDateToSqlCommand("ReceiveTimestamp", Date)}) WHERE (Event LIKE '%{Keyword}%' OR IPPort LIKE '%{Keyword}%' OR Data LIKE '%{Keyword}%') ORDER BY No DESC LIMIT {Limit.ToString()}";
			}
			return result;
		}
		protected override string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit, DateTime DateStart, DateTime DateEnd)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(Keyword))
			{
				result = $"SELECT ReceiveTimestamp, Event, IPPort, Data FROM (SELECT * FROM HistoryHostCommunicationInfo WHERE {ConvertTimePeriodToSqlCommand("ReceiveTimestamp", DateStart, DateEnd)}) ORDER BY No DESC LIMIT {Limit.ToString()}";
			}
			else
			{
				result = $"SELECT ReceiveTimestamp, Event, IPPort, Data FROM (SELECT * FROM HistoryHostCommunicationInfo WHERE {ConvertTimePeriodToSqlCommand("ReceiveTimestamp", DateStart, DateEnd)}) WHERE (Event LIKE '%{Keyword}%' OR IPPort LIKE '%{Keyword}%' OR Data LIKE '%{Keyword}%') ORDER BY No DESC LIMIT {Limit.ToString()}";
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
				dgv.Columns.Add("Date", "Date");
				dgv.Columns[1].Width = 175;
				dgv.Columns.Add("Event", "Event");
				dgv.Columns[2].Width = 200;
				dgv.Columns.Add("IPPort", "IPPort");
				dgv.Columns[3].Width = 180;
				dgv.Columns.Add("Data", "Data");
				dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
