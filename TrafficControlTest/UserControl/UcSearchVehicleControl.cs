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
	public partial class UcSearchVehicleControl : UcSearch
	{
		public override string mKeyword { get; } = "VehicleControl";

		public UcSearchVehicleControl()
		{
			InitializeComponent();
		}

		protected override string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit, DateTime Date)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(Keyword))
			{
				result = $"SELECT * FROM (SELECT * FROM HistoryVehicleControlInfo WHERE {ConvertDateToSqlCommand("ReceiveTimestamp", Date)}) ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
			}
			else
			{
				result = $"SELECT * FROM (SELECT * FROM HistoryVehicleControlInfo WHERE {ConvertDateToSqlCommand("ReceiveTimestamp", Date)}) WHERE (ID LIKE '%{Keyword}%' OR VehicleID LIKE '%{Keyword}%' OR Command LIKE '%{Keyword}%' OR Parameters LIKE '%{Keyword}%' OR CauseID LIKE '%{Keyword}%' OR SendState LIKE '%{Keyword}%' OR ExecuteState LIKE '%{Keyword}%' OR FailedReason LIKE '%{Keyword}%') ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
			}
			return result;
		}
		protected override string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit, DateTime DateStart, DateTime DateEnd)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(Keyword))
			{
				result = $"SELECT * FROM (SELECT * FROM HistoryVehicleControlInfo WHERE {ConvertTimePeriodToSqlCommand("ReceiveTimestamp", DateStart, DateEnd)}) ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
			}
			else
			{
				result = $"SELECT * FROM (SELECT * FROM HistoryVehicleControlInfo WHERE {ConvertTimePeriodToSqlCommand("ReceiveTimestamp", DateStart, DateEnd)}) WHERE (ID LIKE '%{Keyword}%' OR VehicleID LIKE '%{Keyword}%' OR Command LIKE '%{Keyword}%' OR Parameters LIKE '%{Keyword}%' OR CauseID LIKE '%{Keyword}%' OR SendState LIKE '%{Keyword}%' OR ExecuteState LIKE '%{Keyword}%' OR FailedReason LIKE '%{Keyword}%') ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
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
				dgv.Columns[1].Width = 210;
				dgv.Columns.Add("VehicleId", "VehicleID");
				dgv.Columns[2].Width = 180;
				dgv.Columns.Add("Command", "Command");
				dgv.Columns[3].Width = 140;
				dgv.Columns.Add("Parameters", "Parameters");
				dgv.Columns[4].Width = 170;
				dgv.Columns.Add("CauseId", "CauseID");
				dgv.Columns[5].Width = 210;
				dgv.Columns.Add("CauseDetail", "CauseDetail");
				dgv.Columns[6].Width = 100;
				dgv.Columns.Add("SendState", "SendState");
				dgv.Columns[7].Width = 130;
				dgv.Columns.Add("ExecuteState", "ExecuteState");
				dgv.Columns[8].Width = 130;
				dgv.Columns.Add("FailedReason", "FailedReason");
				dgv.Columns[9].Width = 180;
				dgv.Columns.Add("ReceiveTimestamp", "ReceiveTimestamp");
				dgv.Columns[10].Width = 190;
				dgv.Columns.Add("ExecutionStartTimestamp", "ExecutionStartTimestamp");
				dgv.Columns[11].Width = 190;
				dgv.Columns.Add("ExecutionStopTimestamp", "ExecutionStopTimestamp");
				dgv.Columns[12].Width = 190;
				dgv.Columns.Add("LastUpdateTimestamp", "LastUpdateTimestamp");
				dgv.Columns[13].Width = 190;

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
