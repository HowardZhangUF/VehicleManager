﻿using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;

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
		private int mDgvMissionRightClickRowIndex { get; set; } = -1;
		private int mDgvMissionRightClickColIndex { get; set; } = -1;

		public UcMission()
		{
			InitializeComponent();
			UpdateGui_InitializeDgvMission();
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			UnsubscribeEvent_IMissionStateManager(rMissionStateManager);
			rMissionStateManager = MissionStateManager;
			SubscribeEvent_IMissionStateManager(rMissionStateManager);
		}
		public void EnableManualControl(bool Enable)
		{
			if (Enable)
			{
				dgvMission.ContextMenuStrip = cmenuDgvMission;
			}
			else
			{
				dgvMission.ContextMenuStrip = null;
			}
		}
		public void AddRow(string Id, string[] Datas)
		{
			lock (mLockOfDgvMission)
			{
				int rowIndex = GetRowIndex(Id);
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
							int currPriority = int.Parse(dgvMission.Rows[i].Cells["Priority"].Value.ToString());
							int newPriority = int.Parse(Datas[dgvMission.Columns["Priority"].Index].ToString());
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
		public void RemoveRow(string Id)
		{
			lock (mLockOfDgvMission)
			{
				int rowIndex = GetRowIndex(Id);
				if (rowIndex >= 0 && rowIndex < dgvMission.Columns.Count)
				{
					UpdateGui_RemoveRow(rowIndex);
					UpdateGui_RefreshDgvMissionRowsBackColor();
				}
			}
		}
		public void UpdateRow(string Id, string StatusName, string NewValue)
		{
			lock (mLockOfDgvMission)
			{
				int rowIndex = GetRowIndex(Id);
				if (rowIndex >= 0 && rowIndex < dgvMission.Columns.Count)
				{
					if (StatusName == "Priority")
					{
						if (dgvMission.Rows[rowIndex].Cells["Priority"].Value.ToString() != NewValue)
						{
							dgvMission.Rows[rowIndex].Cells["Priority"].Value = NewValue;
							string[] tmp = ConvertToStringArray(dgvMission.Rows[rowIndex].Cells);
							RemoveRow(Id);
							AddRow(Id, tmp);
							UpdateGui_RefreshDgvMissionRowsBackColor();
						}
					}
					else if (StatusName == "SourceIpPort")
					{
						if (dgvMission.Rows[rowIndex].Cells["SourceIpPort"].Value.ToString() != NewValue)
						{
							dgvMission.Rows[rowIndex].Cells["SourceIpPort"].Value = NewValue;
						}
					}
					else if (StatusName == "ExecutorId")
					{
						if (dgvMission.Rows[rowIndex].Cells["Executor"].Value.ToString() != NewValue)
						{
							dgvMission.Rows[rowIndex].Cells["Executor"].Value = NewValue;
						}
					}
					else if (StatusName.StartsWith("ExecuteState"))
					{
						if (dgvMission.Rows[rowIndex].Cells["ExecuteState"].Value.ToString() != NewValue)
						{
							dgvMission.Rows[rowIndex].Cells["ExecuteState"].Value = NewValue;
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
		private void HandleEvent_MissionStateManagerItemAdded(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			AddRow(Args.ItemName, Args.Item.ToStringArray());
		}
		private void HandleEvent_MissionStateManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			RemoveRow(Args.ItemName);
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			string newValue = null;
			if (Args.StatusName == "Priority")
			{
				newValue = Args.Item.mMission.mPriority.ToString();
			}
			else if (Args.StatusName == "SourceIpPort")
			{
				newValue = Args.Item.mSourceIpPort;
			}
			else if (Args.StatusName == "ExecutorId")
			{
				newValue = Args.Item.mExecutorId;
			}
			else if (Args.StatusName.StartsWith("ExecuteState"))
			{
				newValue = Args.Item.mExecuteState.ToString();
			}

			if (newValue != null)
			{
				UpdateRow(Args.ItemName, Args.StatusName, newValue);
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

				dgv.Columns.Add("ID", "ID");
				dgv.Columns[0].Width = 210;
				dgv.Columns.Add("HostMissionID", "HostMissionID");
				dgv.Columns[1].Width = 210;
				dgv.Columns.Add("Priority", "Priority");
				dgv.Columns[2].Width = 70;
				dgv.Columns.Add("Type", "Type");
				dgv.Columns[3].Width = 100;
				dgv.Columns.Add("Vehicle", "Vehicle");
				dgv.Columns[4].Width = 130;
				dgv.Columns.Add("Parameter", "Parameter");
				dgv.Columns[5].Width = 160;
				dgv.Columns.Add("SourceIpPort", "SourceIPPort");
				dgv.Columns[6].Width = 180;
				dgv.Columns.Add("ExecuteState", "ExecuteState");
				dgv.Columns[7].Width = 130;
				dgv.Columns.Add("Executor", "Executor");
				dgv.Columns[8].Width = 130;
				dgv.Columns.Add("ReceivedTime", "ReceivedTime");
				dgv.Columns[9].Width = 190;
				dgv.Columns.Add("FillColumn", "");
				dgv.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
				if (dgvMission.Rows[RowIndex].Cells["ExecuteState"].Value.ToString() == "Executing")
				{
					if (dgvMission.Rows[RowIndex].DefaultCellStyle.BackColor != TableRowExecutingBackColor) dgvMission.Rows[RowIndex].DefaultCellStyle.BackColor = TableRowExecutingBackColor;
				}
				else
				{
					if (dgvMission.Rows[RowIndex].DefaultCellStyle.BackColor != TableRowUnexecuteBackColor) dgvMission.Rows[RowIndex].DefaultCellStyle.BackColor = TableRowUnexecuteBackColor;
				}
			});
		}
		private int GetRowIndex(string Id)
		{
			return GetRowIndex(dgvMission.Columns["ID"].Index, Id);
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
		private void dgvMission_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					var hit = dgvMission.HitTest(e.X, e.Y);
					mDgvMissionRightClickRowIndex = hit.RowIndex;
					mDgvMissionRightClickColIndex = hit.ColumnIndex;
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void cmenuItemRemoveMission_Click(object sender, EventArgs e)
		{
			try
			{
				if (mDgvMissionRightClickRowIndex >= 0 && mDgvMissionRightClickRowIndex < dgvMission.RowCount)
				{
					string tmpId = dgvMission.Rows[mDgvMissionRightClickRowIndex].Cells["ID"].Value.ToString();
					string tmpType = dgvMission.Rows[mDgvMissionRightClickRowIndex].Cells["Type"].Value.ToString();
					string tmpParameter = dgvMission.Rows[mDgvMissionRightClickRowIndex].Cells["Parameter"].Value.ToString();
					string tmpMessage = $"Sure to Remove Mission:\n{tmpId} / {tmpType}{(string.IsNullOrEmpty(tmpParameter) ? string.Empty : " / " + tmpParameter)}";
					if (CustomMessageBox.ConfirmBox(tmpMessage) == DialogResult.OK)
					{
						if (rMissionStateManager.GetItem(tmpId).mMission.mMissionType == MissionType.Abort)
						{
							rMissionStateManager.UpdateFailedReason(tmpId, FailedReason.CancelByGUI);
							rMissionStateManager.UpdateExecuteState(tmpId, ExecuteState.ExecuteFailed);
						}
						else
						{
							IMission tmpMission = Library.Library.GenerateIMission(MissionType.Abort, string.Empty, MissionAnalyzer.mPriorityMin, string.Empty, new string[] { tmpId });
							IMissionState tmpMissionState = Library.Library.GenerateIMissionState(tmpMission);
							rMissionStateManager.Add(tmpMissionState.mName, tmpMissionState);
						}
					}
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
	}
}
