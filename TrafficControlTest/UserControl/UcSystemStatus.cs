using System;
using System.Collections.Generic;
using System.Drawing;
using TrafficControlTest.Module.CycleMission;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Log;
using TrafficControlTest.Module.CollisionEvent;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.CommunicationVehicle;
using System.Windows.Forms;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.NewCommunication;
using LibraryForVM;

namespace TrafficControlTest.UserControl
{
	public partial class UcSystemStatus : System.Windows.Forms.UserControl
	{
		public Color TableBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableRowBackColor { get; set; } = Color.FromArgb(53, 53, 53);
		public Color TableRowForeColor { get; set; } = Color.White;

		private List<ISystemWithLoopTask> rISystemWithLoopTasks = new List<ISystemWithLoopTask>();
		private ILogExporter rLogExporter = null;
		private Dictionary<string, Label> mSystemLabels = new Dictionary<string, Label>();
		private Dictionary<string, Button> mSystemButtons = new Dictionary<string, Button>();

		public UcSystemStatus()
		{
			InitializeComponent();
		}
		public void Set(List<ISystemWithLoopTask> ISystemWithLoopTasks)
		{
			foreach (ISystemWithLoopTask system in rISystemWithLoopTasks)
			{
				UnsubscribeEvent_ISystemWithLoopTask(system);
			}
			rISystemWithLoopTasks.Clear();
			UpdateGui_TlpSystem_ClearRows();
			if (ISystemWithLoopTasks != null && ISystemWithLoopTasks.Count > 0)
			{
				rISystemWithLoopTasks.AddRange(ISystemWithLoopTasks);
				foreach (ISystemWithLoopTask system in rISystemWithLoopTasks)
				{
					SubscribeEvent_ISystemWithLoopTask(system);
				}
				UpdateGui_TlpSystem_AddRows();
			}
		}
		public void Set(ILogExporter LogExporter)
		{
			UnsubscribeEvent_ILogExporter(rLogExporter);
			rLogExporter = LogExporter;
			SubscribeEvent_ILogExporter(rLogExporter);
		}
		public void Set(List<ISystemWithLoopTask> ISystemWithLoopTasks, ILogExporter LogExporter)
		{
			Set(ISystemWithLoopTasks);
			Set(LogExporter);
		}
		public new void BringToFront()
		{
			UpdateGui_UpdateSystemControlEnable(false);
			base.BringToFront();
		}

		private void SubscribeEvent_ISystemWithLoopTask(ISystemWithLoopTask ISystemWithLoopTask)
		{
			if (ISystemWithLoopTask != null)
			{
				ISystemWithLoopTask.SystemStatusChanged += HandleEvent_ISystemWithLoopTaskSystemStatusChanged;
			}
		}
		private void UnsubscribeEvent_ISystemWithLoopTask(ISystemWithLoopTask ISystemWithLoopTask)
		{
			if (ISystemWithLoopTask != null)
			{
				ISystemWithLoopTask.SystemStatusChanged -= HandleEvent_ISystemWithLoopTaskSystemStatusChanged;
			}
		}
		private void SubscribeEvent_ILogExporter(ILogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ExportStarted += HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted += HandleEvent_LogExporterExportCompleted;
			}
		}
		private void UnsubscribeEvent_ILogExporter(ILogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ExportStarted -= HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted -= HandleEvent_LogExporterExportCompleted;
			}
		}
		private void HandleEvent_ISystemWithLoopTaskSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			UpdateGui_UpdateSystemButtonState(Sender.GetType().Name, Args.SystemNewStatus);
		}
		private void HandleEvent_LogExporterExportStarted(object Sender, LogExportedEventArgs Args)
		{
			btnExportLog.InvokeIfNecessary(() =>
			{
				btnExportLog.Enabled = false;
			});
		}
		private void HandleEvent_LogExporterExportCompleted(object Sender, LogExportedEventArgs Args)
		{
			btnExportLog.InvokeIfNecessary(() =>
			{
				btnExportLog.Enabled = true;
			});
		}
		private void UpdateGui_TlpSystem_AddRows()
		{
			if (rISystemWithLoopTasks == null || rISystemWithLoopTasks.Count == 0) return;

			tlpSystem.InvokeIfNecessary(() =>
			{
				for (int i = 0; i < rISystemWithLoopTasks.Count; ++i)
				{
					tlpSystem.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
				}
				tlpSystem.Height = 55 + rISystemWithLoopTasks.Count * 40;

				for (int j = 0; j < rISystemWithLoopTasks.Count; ++j)
				{
					Label lblSystem = new Label();
					lblSystem.Font = new Font(lblSystem.Font.FontFamily, 12, FontStyle.Regular);
					lblSystem.ForeColor = Color.White;
					lblSystem.Text = rISystemWithLoopTasks[j].GetType().Name;
					lblSystem.AutoSize = false;
					lblSystem.Dock = DockStyle.Fill;
					lblSystem.TextAlign = ContentAlignment.MiddleLeft;
					lblSystem.Name = $"lbl{rISystemWithLoopTasks[j].GetType().Name}";
					mSystemLabels.Add(lblSystem.Name, lblSystem);
					Button btnSystem = new Button();
					btnSystem.Font = new Font(btnSystem.Font.FontFamily, 12, FontStyle.Regular);
					btnSystem.FlatStyle = FlatStyle.Flat;
					btnSystem.BackColor = BackColor;
					btnSystem.Text = "Off";
					btnSystem.Dock = DockStyle.Left;
					btnSystem.Width = 100;
					btnSystem.Name = $"btn{rISystemWithLoopTasks[j].GetType().Name}";
					btnSystem.Click += btnSystem_Click;
					mSystemButtons.Add(btnSystem.Name, btnSystem);
					tlpSystem.Controls.Add(lblSystem, 0, 2 + j);
					tlpSystem.Controls.Add(btnSystem, 1, 2 + j);
				}
			});
		}
		private void UpdateGui_TlpSystem_ClearRows()
		{
			tlpSystem.InvokeIfNecessary(() =>
			{
				while (tlpSystem.RowStyles.Count > 2)
				{
					tlpSystem.RowStyles.RemoveAt(tlpSystem.RowCount - 1);
				}
			});
		}
		private void UpdateGui_UpdateSystemControlEnable(bool Enable)
		{
			btnLockPanel.InvokeIfNecessary(() =>
			{
				if (Enable)
				{
					btnLockPanel.Text = "Unlocked";
					btnLockPanel.BackColor = Color.FromArgb(52, 170, 70);
					btnLockPanel.ForeColor = Color.Black;
					tlpSystem.Enabled = true;
				}
				else
				{
					btnLockPanel.Text = "Locked";
					btnLockPanel.BackColor = BackColor;
					btnLockPanel.ForeColor = ForeColor;
					tlpSystem.Enabled = false;
				}
			});
		}
		private void UpdateGui_UpdateSystemButtonState(string SystemName, bool SystemStatus)
		{
			int rowIndex = -1;
			for (int i = 0; i < rISystemWithLoopTasks.Count; ++i)
			{
				if (SystemName == rISystemWithLoopTasks[i].GetType().Name)
				{
					rowIndex = i;
					break;
				}
			}
			if (rowIndex == -1) return;

			mSystemButtons["btn" + SystemName].InvokeIfNecessary(() =>
			{
				if (SystemStatus)
				{
					mSystemButtons["btn" + SystemName].BackColor = Color.FromArgb(52, 170, 70);
					mSystemButtons["btn" + SystemName].Text = "On";
				}
				else
				{
					mSystemButtons["btn" + SystemName].BackColor = BackColor;
					mSystemButtons["btn" + SystemName].Text = "Off";
				}
			});
		}
		private void btnLockPanel_Click(object sender, EventArgs e)
		{
			try
			{
				if (btnLockPanel.Text == "Locked")
				{
					UpdateGui_UpdateSystemControlEnable(true);
				}
				else
				{
					UpdateGui_UpdateSystemControlEnable(false);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnReportSystemInfo_Click(object sender, EventArgs e)
		{
			try
			{
				if (rISystemWithLoopTasks != null && rISystemWithLoopTasks.Count > 0)
				{
					for (int i = 0; i < rISystemWithLoopTasks.Count; ++i)
					{
						rISystemWithLoopTasks[i].ReportSystemInfo();
					}
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnExportLog_Click(object sender, EventArgs e)
		{
			try
			{
				if (!rLogExporter.mIsExporting)
				{
					rLogExporter.StartExport();
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnOpenSystemFolder_Click(object sender, EventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start(Application.StartupPath);
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnSystem_Click(object sender, EventArgs e)
		{
			try
			{
				Button btn = sender as Button;
				int rowIndex = -1;
				for (int i = 0; i < rISystemWithLoopTasks.Count; ++i)
				{
					if (rISystemWithLoopTasks[i].GetType().Name == btn.Name.Replace("btn", string.Empty))
					{
						rowIndex = i;
					}
				}
				if (rowIndex == -1) return;

				if (btn.Text == "On")
				{
					rISystemWithLoopTasks[rowIndex].Stop();
				}
				else if (btn.Text == "Off")
				{
					rISystemWithLoopTasks[rowIndex].Start();
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
	}
}
