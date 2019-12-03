using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.UserControl
{
	public partial class UcMission : System.Windows.Forms.UserControl
	{
		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableGridLineColor { get; set; } = Color.FromArgb(86, 86, 86);
		public Color TableHeaderBackColor { get; set; } = Color.FromArgb(0, 122, 204);
		public Color TableHeaderForeColor { get; set; } = Color.White;
		public Color TableRowUnexecuteBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableRowExecutingBackColor { get; set; } = Color.FromArgb(35, 128, 76);
		public Color TableRowForeColor { get; set; } = Color.White;

		private IMissionStateManager rMissionStateManager = null;
		private object mLockOfDgvMission = new object();
		private Dictionary<string, int> mColumnHeaderDictionary = new Dictionary<string, int>()
		{
			{ "MissionID", 0 },
			{ "Priority", 1 },
			{ "SendState", 5 },
			{ "ExecuteState", 5 },
			{ "ExecutorId", 6 },
			{ "SourceIpPort", 7 }
		};

		public UcMission()
		{
			InitializeComponent();
			UpdateGui_InitializeDgvMission();
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			UnsubscribeEvent_IMissionStateManager(MissionStateManager);
			rMissionStateManager = MissionStateManager;
			SubscribeEvent_IMissionStateManager(MissionStateManager);
		}
		public void AddRow(string MissionId, string[] Datas)
		{
			lock (mLockOfDgvMission)
			{
				int rowIndex = GetRowIndex(MissionId);
				if (rowIndex < 0)
				{
					if (dgvMission.Rows.Count == 0)
					{
						rowIndex = 0;
					}
					else
					{
						for (int i = 0; i < dgvMission.Rows.Count; ++i)
						{
							int currPriority = int.Parse(dgvMission.Rows[i].Cells[mColumnHeaderDictionary["Priority"]].Value.ToString());
							int newPriority = int.Parse(Datas[mColumnHeaderDictionary["Priority"]].ToString());
							if (newPriority < currPriority)
							{
								rowIndex = i;
								break;
							}
						}
						// 如果經過迴圈計算仍沒找到合適位置，則插入尾端
						if (rowIndex < 0) rowIndex = dgvMission.Rows.Count;
					}
					UpdateGui_InsertRow(rowIndex, Datas);
					UpdateGui_RefreshDgvMissionRowsBackColor();
				}
			}
		}
		public void RemoveRow(string MissionId)
		{
			lock (mLockOfDgvMission)
			{
				int rowIndex = GetRowIndex(MissionId);
				if (rowIndex >= 0 && rowIndex < dgvMission.Columns.Count)
				{
					UpdateGui_RemoveRow(rowIndex);
					UpdateGui_RefreshDgvMissionRowsBackColor();
				}
			}
		}
		public void UpdateRow(string MissionId, string StateName, string NewValue)
		{
			lock (mLockOfDgvMission)
			{
				int rowIndex = GetRowIndex(MissionId);
				if (rowIndex >= 0 && rowIndex < dgvMission.Columns.Count)
				{
					if (StateName == "Priority")
					{
						if (dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["Priority"]].Value.ToString() != NewValue)
						{
							dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["Priority"]].Value = NewValue;
							string[] tmp = ConvertToStringArray(dgvMission.Rows[rowIndex].Cells);
							RemoveRow(MissionId);
							AddRow(MissionId, tmp);
							UpdateGui_RefreshDgvMissionRowsBackColor();
						}
					}
					else if (StateName == "SourceIpPort")
					{
						if (dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["SourceIpPort"]].Value.ToString() != NewValue)
						{
							dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["SourceIpPort"]].Value = NewValue;
						}
					}
					else if (StateName == "ExecutorId")
					{
						if (dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["ExecutorId"]].Value.ToString() != NewValue)
						{
							dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["ExecutorId"]].Value = NewValue;
						}
					}
					else if (StateName.StartsWith("SendState"))
					{
						if (dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["SendState"]].Value.ToString() != NewValue)
						{
							dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["SendState"]].Value = NewValue;
							UpdateGui_RefreshDgvMissionRowBackColor(rowIndex);
						}
					}
					else if (StateName.StartsWith("ExecuteState"))
					{
						if (dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["ExecuteState"]].Value.ToString() != NewValue)
						{
							dgvMission.Rows[rowIndex].Cells[mColumnHeaderDictionary["ExecuteState"]].Value = NewValue;
							UpdateGui_RefreshDgvMissionRowBackColor(rowIndex);
						}
					}
				}
			}
		}

		private void SubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded += HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemRemoved += HandleEvent_MissionStateManagerItemRemoved;
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded -= HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemRemoved -= HandleEvent_MissionStateManagerItemRemoved;
				MissionStateManager.ItemUpdated -= HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void HandleEvent_MissionStateManagerItemAdded(DateTime OccurTime, string Name, IMissionState Item)
		{
			AddRow(Name, Item.ToStringArray());
		}
		private void HandleEvent_MissionStateManagerItemRemoved(DateTime OccurTime, string Name, IMissionState Item)
		{
			RemoveRow(Name);
		}
		private void HandleEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IMissionState Item)
		{
			string newValue = null;
			if (StateName == "Priority")
			{
				newValue = Item.mMission.mPriority.ToString();
			}
			else if (StateName == "SourceIpPort")
			{
				newValue = Item.mSourceIpPort;
			}
			else if (StateName == "ExecutorId")
			{
				newValue = Item.mExecutorId;
			}
			else if (StateName.StartsWith("SendState"))
			{
				newValue = $"{Item.mSendState.ToString()} / {Item.mExecuteState.ToString()}";
			}
			else if (StateName.StartsWith("ExecuteState"))
			{
				newValue = $"{Item.mSendState.ToString()} / {Item.mExecuteState.ToString()}";
			}

			if (newValue != null)
			{
				UpdateRow(Name, StateName, newValue);
			}
		}
		private void UpdateGui_InsertRow(int RowIndex, string[] RowData)
		{
			dgvMission.InvokeIfNecessary(() =>
			{
				if (RowIndex <= dgvMission.Rows.Count)
				{
					dgvMission.Rows.Insert(RowIndex, RowData);
				}
			});
		}
		private void UpdateGui_RemoveRow(int RowIndex)
		{
			dgvMission.InvokeIfNecessary(() =>
			{
				if (RowIndex < dgvMission.Rows.Count)
				{
					dgvMission.Rows.RemoveAt(RowIndex);
				}
			});
		}
		private void UpdateGui_UpdateRow(int RowIndex, int ColumnIndex, string Data)
		{
			dgvMission.InvokeIfNecessary(() =>
			{
				if (RowIndex < dgvMission.Rows.Count && ColumnIndex < dgvMission.Columns.Count)
				{
					if (dgvMission.Rows[RowIndex].Cells[ColumnIndex].Value.ToString() != Data)
					{
						dgvMission.Rows[RowIndex].Cells[ColumnIndex].Value = Data;
						UpdateGui_RefreshDgvMissionRowBackColor(RowIndex);
					}
				}
			});
		}
		private void UpdateGui_InitializeDgvMission()
		{
			dgvMission.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvMission;

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

				dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
				dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
				dgv.DefaultCellStyle.BackColor = TableRowUnexecuteBackColor;
				dgv.DefaultCellStyle.ForeColor = TableRowForeColor;
				dgv.RowTemplate.Height = 40;

				dgv.Columns.Add("MissionID", "MissionID");
				dgv.Columns[0].Width = 210;
				dgv.Columns.Add("Priority", "Priority");
				dgv.Columns[1].Width = 70;
				dgv.Columns.Add("Type", "Type");
				dgv.Columns[2].Width = 100;
				dgv.Columns.Add("Vehicle", "Vehicle");
				dgv.Columns[3].Width = 130;
				dgv.Columns.Add("Parameter", "Parameter");
				dgv.Columns[4].Width = 160;
				dgv.Columns.Add("State", "State");
				dgv.Columns[5].Width = 240;
				dgv.Columns.Add("Executor", "Executor");
				dgv.Columns[6].Width = 130;
				dgv.Columns.Add("From", "From");
				dgv.Columns[7].Width = 180;
				dgv.Columns.Add("ReceivedTime", "ReceivedTime");
				dgv.Columns[8].Width = 190;
				dgv.Columns.Add("FillColumn", "");
				dgv.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}
			});
		}
		private void UpdateGui_RefreshDgvMissionRowsBackColor()
		{
			dgvMission.InvokeIfNecessary(() =>
			{
				if (dgvMission != null && dgvMission.Rows != null && dgvMission.Rows.Count > 0)
				{
					for (int i = 0; i < dgvMission.Rows.Count; ++i)
					{
						UpdateGui_RefreshDgvMissionRowBackColor(i);
					}
				}
			});
		}
		private void UpdateGui_RefreshDgvMissionRowBackColor(int RowIndex)
		{
			dgvMission.InvokeIfNecessary(() =>
			{
				if (dgvMission.Rows[RowIndex].Cells[5].Value.ToString().EndsWith("Executing"))
				{
					if (dgvMission.Rows[RowIndex].DefaultCellStyle.BackColor != TableRowExecutingBackColor) dgvMission.Rows[RowIndex].DefaultCellStyle.BackColor = TableRowExecutingBackColor;
				}
				else
				{
					if (dgvMission.Rows[RowIndex].DefaultCellStyle.BackColor != TableRowUnexecuteBackColor) dgvMission.Rows[RowIndex].DefaultCellStyle.BackColor = TableRowUnexecuteBackColor;
				}
			});
		}
		private void UpdateGui_InsertSampleDataToDgvMission()
		{
			dgvMission.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvMission;

				dgv.Rows.Add("Mission20191122142552342", "2", "Goto", "Vehicle982", "Goal999", "Unsend / Unexecute", "", "127.0.0.1:65535", "2019/11/22 14:25:52.342");
				dgv.Rows.Add("Mission20191122142552342", "2", "Goto", "", "Goal999", "Unsend / Executing", "", "127.0.0.1:65535", "2019/11/22 14:25:52.342");
				dgv.Rows.Add("Mission20191122142552342", "2", "GotoPoint", "Vehicle982", "(456347,939399,120)", "Unsend / Executing", "", "127.0.0.1:65535", "2019/11/22 14:25:52.342");
				dgv.Rows.Add("Mission20191122142552342", "2", "GotoPoint", "", "(1111,2222)", "Unsend / Unexecute", "", "127.123.123.123:65535", "2019/11/22 14:25:52.342");
				dgv.Rows.Add("Mission20191122142552342", "2", "Dock", "Vehicle982", "Goal999", "SendSuccessed / ExecuteSuccessed", "", "127.0.0.1:65535", "2019/11/22 14:25:52.342");
				dgv.Rows.Add("Mission20191122142552342", "2", "Dock", "Vehicle982", "Goal999", "SendSuccessed / ExecuteSuccessed", "", "127.0.0.1:65535", "2019/11/22 14:25:52.342");
				dgv.Rows.Add("Mission20191122142552342", "2", "Dock", "Vehicle982", "Goal999", "SendSuccessed / Executing", "", "127.0.0.1:65535", "2019/11/22 14:25:52.342");

				UpdateGui_RefreshDgvMissionRowsBackColor();
			});
		}
		private int GetRowIndex(string MissionId)
		{
			return GetRowIndex(mColumnHeaderDictionary["MissionID"], MissionId);
		}
		private int GetRowIndex(int ColumnIndex, string CellValue)
		{
			int result = -1;
			dgvMission.InvokeIfNecessary(() =>
			{
				if (ColumnIndex < dgvMission.Columns.Count)
				{
					for (int i = 0; i < dgvMission.Rows.Count; ++i)
					{
						if (dgvMission.Rows[i].Cells[ColumnIndex].Value.ToString() == CellValue)
						{
							result = i;
							break;
						}
					}
				}
			});
			return result;
		}
		private string[] ConvertToStringArray(DataGridViewCellCollection Cells)
		{
			List<string> result = new List<string>();
			for (int i = 0; i < Cells.Count; ++i)
			{
				result.Add(Cells[i].ToString());
			}
			return result.ToArray();
		}
	}
}
