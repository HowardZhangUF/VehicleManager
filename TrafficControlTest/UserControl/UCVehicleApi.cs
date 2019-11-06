using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public partial class UCVehicleApi : System.Windows.Forms.UserControl
	{
		public delegate void EventHandlerStringStringStrings(string VehicleName, string Command, params string[] Parameters);

		public event EventHandlerStringStringStrings VehicleGoto;
		public event EventHandlerStringStringStrings VehicleGotoPoint;
		public event EventHandlerStringStringStrings VehicleDock;
		public event EventHandlerStringStringStrings VehicleStop;
		public event EventHandlerStringStringStrings VehicleInsertMovingBuffer;
		public event EventHandlerStringStringStrings VehicleRemoveMovingBuffer;
		public event EventHandlerStringStringStrings VehiclePause;
		public event EventHandlerStringStringStrings VehicleResume;
		public event EventHandlerStringStringStrings VehicleRequestMapList;
		public event EventHandlerStringStringStrings VehicleGetMap;
		public event EventHandlerStringStringStrings VehicleUploadMap;
		public event EventHandlerStringStringStrings VehicleChangeMap;

		public UCVehicleApi()
		{
			InitializeComponent();

			txtCoordinate1.KeyPress += ((sender, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '-')) e.Handled = true; });
			txtCoordinate2.KeyPress += ((sender, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '-')) e.Handled = true; });
		}
		public void UpdateVehicleNameList(string[] VehicleNameList)
		{
			if (VehicleNameList == null || VehicleNameList.Count() == 0)
			{
				cbVehicleNameList.Items.Clear();
			}
			else
			{
				string lastSelectedItemText = cbVehicleNameList.SelectedItem != null ? cbVehicleNameList.SelectedItem.ToString() : string.Empty;
				cbVehicleNameList.SelectedIndex = -1;
				cbVehicleNameList.Items.Clear();
				cbVehicleNameList.Items.AddRange(VehicleNameList.OrderBy((o) => o).ToArray());
				if (!string.IsNullOrEmpty(lastSelectedItemText))
				{
					for (int i = 0; i < cbVehicleNameList.Items.Count; ++i)
					{
						if (lastSelectedItemText == cbVehicleNameList.Items[i].ToString())
						{
							cbVehicleNameList.SelectedIndex = i;
							break;
						}
					}
				}
			}
		}
		public void UpdateGoalNameList(string[] GoalNameList)
		{
			if (GoalNameList == null || GoalNameList.Count() == 0)
			{
				cbGoalNameList.Items.Clear();
			}
			else
			{
				cbGoalNameList.Items.Clear();
				cbGoalNameList.Items.AddRange(GoalNameList.OrderBy((o) => o).ToArray());
			}
		}
		public void UpdateRemoteMapNameList(string[] MapNameList)
		{
			if (MapNameList == null || MapNameList.Count() == 0)
			{
				cbRemoteMapNameList1.Items.Clear();
				cbRemoteMapNameList2.Items.Clear();
			}
			else
			{
				cbRemoteMapNameList1.Items.Clear();
				cbRemoteMapNameList1.Items.AddRange(MapNameList.OrderBy((o) => o).ToArray());
				cbRemoteMapNameList2.Items.Clear();
				cbRemoteMapNameList2.Items.AddRange(MapNameList.OrderBy((o) => o).ToArray());
			}
		}
		public void UpdateLocalMapNameList(string[] MapNameList)
		{
			if (MapNameList == null || MapNameList.Count() == 0)
			{
				cbLocalMapNameList.Items.Clear();
			}
			else
			{
				cbLocalMapNameList.Items.Clear();
				cbLocalMapNameList.Items.AddRange(MapNameList.OrderBy((o) => o).ToArray());
			}
		}

		protected virtual void RaiseEvent_VehicleGoto(string VehicleName, string GoalName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleGoto?.Invoke(VehicleName, "Goto", GoalName);
			}
			else
			{
				Task.Run(() => { VehicleGoto?.Invoke(VehicleName, "Goto", GoalName); });
			}
		}
		protected virtual void RaiseEvent_VehicleGotoPoint(string VehicleName, string X, string Y, bool Sync = true)
		{
			if (Sync)
			{
				VehicleGotoPoint?.Invoke(VehicleName, "GotoPoint", X, Y);
			}
			else
			{
				Task.Run(() => { VehicleGotoPoint?.Invoke(VehicleName, "GotoPoint", X, Y); });
			}
		}
		protected virtual void RaiseEvent_VehicleGotoPoint(string VehicleName, string X, string Y, string Toward, bool Sync = true)
		{
			if (Sync)
			{
				VehicleGotoPoint?.Invoke(VehicleName, "GotoPoint", X, Y, Toward);
			}
			else
			{
				Task.Run(() => { VehicleGotoPoint?.Invoke(VehicleName, "GotoPoint", X, Y, Toward); });
			}
		}
		protected virtual void RaiseEvent_VehicleDock(string VehicleName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleDock?.Invoke(VehicleName, "Dock");
			}
			else
			{
				Task.Run(() => { VehicleDock?.Invoke(VehicleName, "Dock"); });
			}
		}
		protected virtual void RaiseEvent_VehicleStop(string VehicleName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleStop?.Invoke(VehicleName, "Stop");
			}
			else
			{
				Task.Run(() => { VehicleStop?.Invoke(VehicleName, "Stop"); });
			}
		}
		protected virtual void RaiseEvent_VehicleInsertMovingBuffer(string VehicleName, string X, string Y, bool Sync = true)
		{
			if (Sync)
			{
				VehicleInsertMovingBuffer?.Invoke(VehicleName, "InsertMovingBuffer", X, Y);
			}
			else
			{
				Task.Run(() => { VehicleInsertMovingBuffer?.Invoke(VehicleName, "InsertMovingBuffer", X, Y); });
			}
		}
		protected virtual void RaiseEvent_VehicleRemoveMovingBuffer(string VehicleName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleRemoveMovingBuffer?.Invoke(VehicleName, "RemoveMovingBuffer");
			}
			else
			{
				Task.Run(() => { VehicleRemoveMovingBuffer?.Invoke(VehicleName, "RemoveMovingBuffer"); });
			}
		}
		protected virtual void RaiseEvent_VehiclePause(string VehicleName, bool Sync = true)
		{
			if (Sync)
			{
				VehiclePause?.Invoke(VehicleName, "PauseMoving");
			}
			else
			{
				Task.Run(() => { VehiclePause?.Invoke(VehicleName, "PauseMoving"); });
			}
		}
		protected virtual void RaiseEvent_VehicleResume(string VehicleName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleResume?.Invoke(VehicleName, "ResumeMoving");
			}
			else
			{
				Task.Run(() => { VehicleResume?.Invoke(VehicleName, "ResumeMoving"); });
			}
		}
		protected virtual void RaiseEvent_VehicleRequestMapList(string VehicleName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleRequestMapList?.Invoke(VehicleName, "RequestMapList");
			}
			else
			{
				Task.Run(() => { VehicleRequestMapList?.Invoke(VehicleName, "RequestMapList"); });
			}
		}
		protected virtual void RaiseEvent_VehicleGetMap(string VehicleName, string MapName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleGetMap?.Invoke(VehicleName, "GetMap", MapName);
			}
			else
			{
				Task.Run(() => { VehicleGetMap?.Invoke(VehicleName, "GetMap", MapName); });
			}
		}
		protected virtual void RaiseEvent_VehicleUploadMap(string VehicleName, string MapPath, bool Sync = true)
		{
			if (Sync)
			{
				VehicleUploadMap?.Invoke(VehicleName, "UploadMapToAGV", MapPath);
			}
			else
			{
				Task.Run(() => { VehicleUploadMap?.Invoke(VehicleName, "UploadMapToAGV", MapPath); });
			}
		}
		protected virtual void RaiseEvent_VehicleChangeMap(string VehicleName, string MapName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleChangeMap?.Invoke(VehicleName, "ChangeMap", MapName);
			}
			else
			{
				Task.Run(() => { VehicleChangeMap?.Invoke(VehicleName, "ChangeMap", MapName); });
			}
		}
		private void btnVehicleGoto_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && cbGoalNameList.SelectedItem != null)
			{
				RaiseEvent_VehicleGoto(cbVehicleNameList.SelectedItem.ToString(), cbGoalNameList.SelectedItem.ToString());
			}
		}
		private void btnVehicleGotoPoint_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && !string.IsNullOrEmpty(txtCoordinate1.Text))
			{
				string[] datas = txtCoordinate1.Text.Split(',');
				if (datas.Length == 2)
				{
					RaiseEvent_VehicleGotoPoint(cbVehicleNameList.SelectedItem.ToString(), datas[0], datas[1]);
				}
				else if (datas.Length == 3)
				{
					RaiseEvent_VehicleGotoPoint(cbVehicleNameList.SelectedItem.ToString(), datas[0], datas[1], datas[2]);
				}
			}
		}
		private void btnVehicleDock_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				RaiseEvent_VehicleDock(cbVehicleNameList.SelectedItem.ToString());
			}
		}
		private void btnVehicleStop_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				RaiseEvent_VehicleStop(cbVehicleNameList.SelectedItem.ToString());
			}
		}
		private void btnVehicleInsertMovingBuffer_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && !string.IsNullOrEmpty(txtCoordinate2.Text))
			{
				string[] datas = txtCoordinate2.Text.Split(',');
				if (datas.Length == 2)
				{
					RaiseEvent_VehicleInsertMovingBuffer(cbVehicleNameList.SelectedItem.ToString(), datas[0], datas[1]);
				}
			}
		}
		private void btnVehicleRemoveMovingBuffer_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				RaiseEvent_VehicleRemoveMovingBuffer(cbVehicleNameList.SelectedItem.ToString());
			}
		}
		private void btnVehiclePause_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				RaiseEvent_VehiclePause(cbVehicleNameList.SelectedItem.ToString());
			}
		}
		private void btnVehicleResume_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				RaiseEvent_VehicleResume(cbVehicleNameList.SelectedItem.ToString());
			}
		}
		private void btnVehicleRequestMapList_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				RaiseEvent_VehicleRequestMapList(cbVehicleNameList.SelectedItem.ToString());
			}
		}
		private void btnVehicleGetMap_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && cbRemoteMapNameList1.SelectedItem != null)
			{
				RaiseEvent_VehicleGetMap(cbVehicleNameList.SelectedItem.ToString(), cbRemoteMapNameList1.SelectedItem.ToString());
			}
		}
		private void btnVehicleUploadMap_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && cbLocalMapNameList.SelectedItem != null)
			{
				RaiseEvent_VehicleUploadMap(cbVehicleNameList.SelectedItem.ToString(), cbLocalMapNameList.SelectedItem.ToString());
			}
		}
		private void btnVehicleChangeMap_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && cbRemoteMapNameList2.SelectedItem != null)
			{
				RaiseEvent_VehicleChangeMap(cbVehicleNameList.SelectedItem.ToString(), cbRemoteMapNameList2.SelectedItem.ToString());
			}
		}
	}
}
