using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.CycleMission;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.UserControl
{
	public partial class UcCycleMission : System.Windows.Forms.UserControl
	{
		// cbVehicleNameList 裡面的項目的格式為：
		//		VehicleName + Split + Suffix
		// 例：
		//		Vehicle001 - Started
		//		Vehicle002 - Stopped

		public string LastVehicleName { get; private set; } = string.Empty;
		public string CurrentVehicleName
		{
			get
			{
				LastVehicleName = _CurrentVehicleName;
				cbVehicleStateList.InvokeIfNecessary(() =>
				{
					_CurrentVehicleName = cbVehicleStateList.SelectedItem == null ? string.Empty : cbVehicleStateList.SelectedItem.ToString().Split(mSplit1, StringSplitOptions.RemoveEmptyEntries)[0];
				});
				return _CurrentVehicleName;
			}
		}
		private string _CurrentVehicleName = string.Empty;
		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableGridLineColor { get; set; } = Color.FromArgb(86, 86, 86);
		public Color TableHeaderBackColor { get; set; } = Color.FromArgb(0, 122, 204);
		public Color TableHeaderForeColor { get; set; } = Color.White;
		public Color TableRowUnexecuteBackColor { get; set; } = Color.FromArgb(31, 31, 31);
		public Color TableRowExecutingBackColor { get; set; } = Color.DarkOrange;
		public Color TableRowForeColor { get; set; } = Color.White;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private ICycleMissionGenerator rCycleMissionGenerator = null;
		private readonly string[] mSplit1 = new string[] { " - " };

		public UcCycleMission()
		{
			InitializeComponent();

			txtMissionListString.SetHintText("(Target1)(Target2)...");
			UpdateGui_DgvMissionList_Initialize();

			txtMissionListString.Enabled = false;
			btnAnalyzeMissionListString.Enabled = false;
			dgvMissionList.Enabled = false;
			btnStartCycle.Enabled = false;
			btnStopCycle.Enabled = false;
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(ICycleMissionGenerator CycleMissionGenerator)
		{
			UnsubscribeEvent_ICycleMissionGenerator(rCycleMissionGenerator);
			rCycleMissionGenerator = CycleMissionGenerator;
			SubscribeEvent_ICycleMissionGenerator(rCycleMissionGenerator);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, ICycleMissionGenerator CycleMissionGenerator)
		{
			Set(VehicleInfoManager);
			Set(CycleMissionGenerator);
		}

		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
			}
		}
		private void SubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.CycleMissionAssigned += HandleEvent_CycleMissionGeneratorCycleMissionAssigned;
				CycleMissionGenerator.CycleMissionRemoved += HandleEvent_CycleMissionGeneratorCycleMissionRemoved;
				CycleMissionGenerator.CycleMissionIndexUpdated += HandleEvent_CycleMissionGeneratorCycleMissionIndexUpdated;
			}
		}
		private void UnsubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.CycleMissionAssigned -= HandleEvent_CycleMissionGeneratorCycleMissionAssigned;
				CycleMissionGenerator.CycleMissionRemoved -= HandleEvent_CycleMissionGeneratorCycleMissionRemoved;
				CycleMissionGenerator.CycleMissionIndexUpdated -= HandleEvent_CycleMissionGeneratorCycleMissionIndexUpdated;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_CbVehicleStateList_UpdateItems();
			cbVehicleStateList_SelectedIndexChanged(null, null);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			if (rCycleMissionGenerator.GetAssigned(Name))
			{
				rCycleMissionGenerator.RemoveCycleMission(Name);
			}
			UpdateGui_CbVehicleStateList_UpdateItems();
			cbVehicleStateList_SelectedIndexChanged(null, null);
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionAssigned(DateTime OccurTime, string VehicleId)
		{
			UpdateGui_CbVehicleStateList_UpdateSpecificItem(VehicleId, "Started");
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionRemoved(DateTime OccurTime, string VehicleId)
		{
			UpdateGui_CbVehicleStateList_UpdateSpecificItem(VehicleId, "Stopped");
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionIndexUpdated(DateTime OccurTime, string VehicleId, int Index)
		{
			if (CurrentVehicleName == VehicleId)
			{
				UpdateGui_DgvMissionList_UpdateSelectedRowIndex(Index);
			}
		}
		private void UpdateGui_CbVehicleStateList_UpdateItems()
		{
			UpdateGui_CbVehicleStateList_UpdateItems(rVehicleInfoManager.GetItemNames().OrderBy(o => o).ToArray(), rVehicleInfoManager.GetItems().OrderBy(o => o.mName).Select(o => rCycleMissionGenerator.GetAssigned(o.mName) ? "Started" : "Stopped").ToArray());
		}
		private void UpdateGui_CbVehicleStateList_UpdateItems(string[] VehicleNameList, string[] SuffixList)
		{
			this.InvokeIfNecessary(() =>
			{
				if (VehicleNameList == null || VehicleNameList.Count() == 0)
				{
					cbVehicleStateList.Items.Clear();
				}
				else
				{
					string lastSelectedVehicleName = CurrentVehicleName;
					cbVehicleStateList.SelectedIndex = -1;
					cbVehicleStateList.Items.Clear();
					for (int i = 0; i < VehicleNameList.Length; ++i)
					{
						cbVehicleStateList.Items.Add(VehicleNameList[i] + mSplit1[0] + SuffixList[i]);
					}
					if (!string.IsNullOrEmpty(lastSelectedVehicleName))
					{
						for (int i = 0; i < cbVehicleStateList.Items.Count; ++i)
						{
							if (cbVehicleStateList.Items[i].ToString().StartsWith(lastSelectedVehicleName))
							{
								cbVehicleStateList.SelectedIndex = i;
								break;
							}
						}
					}
				}
			});
		}
		private void UpdateGui_CbVehicleStateList_UpdateSpecificItem(string VehicleId, string NewSuffix)
		{
			this.InvokeIfNecessary(() =>
			{
				for (int i = 0; i < cbVehicleStateList.Items.Count; ++i)
				{
					if (cbVehicleStateList.Items[i].ToString().StartsWith(VehicleId))
					{
						cbVehicleStateList.Items[i] = VehicleId + mSplit1[0] + NewSuffix;
					}
				}
			});
		}
		private void UpdateGui_DgvMissionList_Initialize()
		{
			dgvMissionList.InvokeIfNecessary(() =>
			{
				DataGridView dgv = dgvMissionList;

				dgv.RowHeadersVisible = false;
				dgv.AllowUserToAddRows = false;
				dgv.AllowUserToDeleteRows = false;
				dgv.AllowUserToResizeRows = false;
				dgv.AllowUserToResizeColumns = false;
				dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dgv.MultiSelect = false;
				dgv.BackgroundColor = TableBackColor;
				dgv.GridColor = TableGridLineColor;
				dgv.BorderStyle = BorderStyle.None;

				dgv.EnableHeadersVisualStyles = false;
				dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold);
				dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				dgv.ColumnHeadersDefaultCellStyle.BackColor = TableHeaderBackColor;
				dgv.ColumnHeadersDefaultCellStyle.ForeColor = TableHeaderForeColor;
				dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
				dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
				dgv.ColumnHeadersHeight = 30;

				dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
				dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
				dgv.DefaultCellStyle.BackColor = TableRowUnexecuteBackColor;
				dgv.DefaultCellStyle.ForeColor = TableRowForeColor;
				dgv.DefaultCellStyle.SelectionBackColor = TableRowExecutingBackColor;
				dgv.RowTemplate.Height = 30;

				dgv.Columns.Add("No", "No");
				dgv.Columns[0].Width = 50;
				dgv.Columns.Add("Target", "Target");
				dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}
			});
		}
		private void UpdateGui_DgvMissionList_AddRange(string MissionListString)
		{
			string[] targets = MissionListString.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
			if (targets.Length > 1)
			{
				UpdateGui_DgvMissionList_AddRange(targets);
			}
		}
		private void UpdateGui_DgvMissionList_AddRange(string[] MissionList)
		{
			dgvMissionList.InvokeIfNecessary(() =>
			{
				dgvMissionList.Rows.Clear();
				for (int i = 0; i < MissionList.Length; ++i)
				{
					dgvMissionList.Rows.Add((i + 1).ToString(), MissionList[i]);
				}
				dgvMissionList.ClearSelection();
			});
		}
		private void UpdateGui_DgvMissinoList_Clear()
		{
			dgvMissionList.InvokeIfNecessary(() =>
			{
				dgvMissionList.Rows.Clear();
			});
		}
		private void UpdateGui_DgvMissionList_UpdateSelectedRowIndex(int RowIndex)
		{
			dgvMissionList.InvokeIfNecessary(() =>
			{
				if (RowIndex >= 0 && RowIndex < dgvMissionList.Rows.Count)
				{
					dgvMissionList.CurrentCell = dgvMissionList.Rows[RowIndex].Cells[1];
				}
			});
		}
		private void UpdateGui_DgvMissionList_InsertSampleData()
		{
			UpdateGui_DgvMissionList_AddRange(new string[] { "Goal7", "Goal6", "Goal5", "Goal4", "Goal3", "Goal2", "Goal1" });
			UpdateGui_DgvMissionList_UpdateSelectedRowIndex(3);
		}
		private void cbVehicleStateList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (LastVehicleName == CurrentVehicleName) return;
			
			string vehicleId = CurrentVehicleName;
			if (!string.IsNullOrEmpty(vehicleId))
			{
				if (rCycleMissionGenerator.GetAssigned(vehicleId))
				{
					this.InvokeIfNecessary(() =>
					{
						txtMissionListString.Enabled = false;
						txtMissionListString.Text = "(" + string.Join(")(", rCycleMissionGenerator.GetMissionList(vehicleId)) + ")";
						btnAnalyzeMissionListString.Enabled = false;
						UpdateGui_DgvMissionList_AddRange(rCycleMissionGenerator.GetMissionList(vehicleId));
						UpdateGui_DgvMissionList_UpdateSelectedRowIndex(rCycleMissionGenerator.GetCurrentMissionIndex(vehicleId));
						dgvMissionList.Enabled = false;
						btnStartCycle.Enabled = false;
						btnStopCycle.Enabled = true;
					});
				}
				else
				{
					this.InvokeIfNecessary(() =>
					{
						txtMissionListString.Enabled = true;
						txtMissionListString.Text = string.Empty;
						btnAnalyzeMissionListString.Enabled = true;
						dgvMissionList.Rows.Clear();
						dgvMissionList.Enabled = true;
						btnStartCycle.Enabled = false;
						btnStopCycle.Enabled = false;
					});
				}
			}
			else
			{
				this.InvokeIfNecessary(() =>
				{
					txtMissionListString.Enabled = false;
					txtMissionListString.Text = string.Empty;
					btnAnalyzeMissionListString.Enabled = false;
					dgvMissionList.Rows.Clear();
					dgvMissionList.Enabled = false;
					btnStartCycle.Enabled = false;
					btnStopCycle.Enabled = false;
				});
			}
		}
		private void btnAnalyzeMissionListString_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(txtMissionListString.Text) && txtMissionListString.Text != txtMissionListString.mHintText)
			{
				UpdateGui_DgvMissionList_AddRange(txtMissionListString.Text);
				btnStartCycle.Enabled = true;
				btnStopCycle.Enabled = false;
			}
			else
			{
				UpdateGui_DgvMissinoList_Clear();
				btnStartCycle.Enabled = false;
				btnStopCycle.Enabled = false;
			}
		}
		private void btnStartCycle_Click(object sender, EventArgs e)
		{
			rCycleMissionGenerator.AssignCycleMission(CurrentVehicleName, txtMissionListString.Text.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries), dgvMissionList.CurrentRow.Index == -1 ? 0 : dgvMissionList.CurrentRow.Index);
			txtMissionListString.Enabled = false;
			btnAnalyzeMissionListString.Enabled = false;
			dgvMissionList.Enabled = false;
			btnStartCycle.Enabled = false;
			btnStopCycle.Enabled = true;
		}
		private void btnStopCycle_Click(object sender, EventArgs e)
		{
			rCycleMissionGenerator.RemoveCycleMission(CurrentVehicleName);
			txtMissionListString.Enabled = true;
			btnAnalyzeMissionListString.Enabled = true;
			dgvMissionList.Enabled = true;
			btnStartCycle.Enabled = true;
			btnStopCycle.Enabled = false;
		}
	}
}
