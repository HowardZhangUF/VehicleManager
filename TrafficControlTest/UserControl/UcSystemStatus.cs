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
using TrafficControlTest.Module.CommunicationHost;

namespace TrafficControlTest.UserControl
{
	public partial class UcSystemStatus : System.Windows.Forms.UserControl
	{
		private IImportantEventRecorder rImportantEventRecorder = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private ICollisionEventDetector rCollisionEventDetector = null;
		private IVehicleControlHandler rVehicleControlHandler = null;
		private IHostCommunicator rHostCommunicator = null;
		private IMissionDispatcher rMissionDispatcher = null;
		private ICycleMissionGenerator rCycleMissionGenerator = null;
		private LogExporter rLogExporter = null;

		public UcSystemStatus()
		{
			InitializeComponent();
		}
		public void Set(IImportantEventRecorder ImportantEventRecorder)
		{
			UnsubscribeEvent_IImportantEventRecorder(rImportantEventRecorder);
			rImportantEventRecorder = ImportantEventRecorder;
			SubscribeEvent_IImportantEventRecorder(rImportantEventRecorder);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(ICollisionEventDetector CollisionEventDetector)
		{
			UnsubscribeEvent_ICollisionEventDetector(rCollisionEventDetector);
			rCollisionEventDetector = CollisionEventDetector;
			SubscribeEvent_ICollisionEventDetector(rCollisionEventDetector);
		}
		public void Set(IVehicleControlHandler VehicleControlHandler)
		{
			UnsubscribeEvent_IVehicleControlHandler(rVehicleControlHandler);
			rVehicleControlHandler = VehicleControlHandler;
			SubscribeEvent_IVehicleControlHandler(rVehicleControlHandler);
		}
		public void Set(IHostCommunicator HostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = HostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}
		public void Set(IMissionDispatcher MissionDispatcher)
		{
			UnsubscribeEvent_IMissionDispatcher(rMissionDispatcher);
			rMissionDispatcher = MissionDispatcher;
			SubscribeEvent_IMissionDispatcher(rMissionDispatcher);
		}
		public void Set(ICycleMissionGenerator CycleMissionGenerator)
		{
			UnsubscribeEvent_ICycleMissionGenerator(rCycleMissionGenerator);
			rCycleMissionGenerator = CycleMissionGenerator;
			SubscribeEvent_ICycleMissionGenerator(rCycleMissionGenerator);
		}
		public void Set(LogExporter LogExporter)
		{
			UnsubscribeEvent_LogExporter(rLogExporter);
			rLogExporter = LogExporter;
			SubscribeEvent_LogExporter(rLogExporter);
		}
		public void Set(IImportantEventRecorder ImportantEventRecorder, IVehicleCommunicator VehicleCommunicator, ICollisionEventDetector CollisionEventDetector, IVehicleControlHandler VehicleControlHandler, IHostCommunicator HostCommunicator, IMissionDispatcher MissionDispatcher, ICycleMissionGenerator CycleMissionGenerator, LogExporter LogExporter)
		{
			Set(ImportantEventRecorder);
			Set(VehicleCommunicator);
			Set(CollisionEventDetector);
			Set(VehicleControlHandler);
			Set(HostCommunicator);
			Set(MissionDispatcher);
			Set(CycleMissionGenerator);
			Set(LogExporter);
		}
		public new void BringToFront()
		{
			UpdateGui_UpdatePanelEnable(false);
			base.BringToFront();
		}

		private void SubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStarted += HandleEvent_ImportantEventRecorderSystemStarted;
				ImportantEventRecorder.SystemStopped += HandleEvent_ImportantEventRecorderSystemStopped;
			}
		}
		private void UnsubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStarted -= HandleEvent_ImportantEventRecorderSystemStarted;
				ImportantEventRecorder.SystemStopped -= HandleEvent_ImportantEventRecorderSystemStopped;
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.LocalListenStateChanged += HandleEvent_VehicleCommunicatorLocalListenStateChanged;
				VehicleCommunicator.SystemStarted += HandleEvent_VehicleCommunicatorSystemStarted;
				VehicleCommunicator.SystemStopped += HandleEvent_VehicleCommunicatorSystemStopped;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.LocalListenStateChanged -= HandleEvent_VehicleCommunicatorLocalListenStateChanged;
				VehicleCommunicator.SystemStarted -= HandleEvent_VehicleCommunicatorSystemStarted;
				VehicleCommunicator.SystemStopped -= HandleEvent_VehicleCommunicatorSystemStopped;
			}
		}
		private void SubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStarted += HandleEvent_CollisionEventDetectorSystemStarted;
				CollisionEventDetector.SystemStopped += HandleEvent_CollisionEventDetectorSystemStopped;
			}
		}
		private void UnsubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStarted -= HandleEvent_CollisionEventDetectorSystemStarted;
				CollisionEventDetector.SystemStopped -= HandleEvent_CollisionEventDetectorSystemStopped;
			}
		}
		private void SubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStarted += HandleEvent_VehicleControlHandlerSystemStarted;
				VehicleControlHandler.SystemStopped += HandleEvent_VehicleControlHandlerSystemStopped;
			}
		}
		private void UnsubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStarted -= HandleEvent_VehicleControlHandlerSystemStarted;
				VehicleControlHandler.SystemStopped -= HandleEvent_VehicleControlHandlerSystemStopped;
			}
		}
		private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.LocalListenStateChanged += HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.SystemStarted += HandleEvent_HostCommunicatorSystemStarted;
				HostCommunicator.SystemStopped += HandleEvent_HostCommunicatorSystemStopped;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.LocalListenStateChanged -= HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.SystemStarted -= HandleEvent_HostCommunicatorSystemStarted;
				HostCommunicator.SystemStopped -= HandleEvent_HostCommunicatorSystemStopped;
			}
		}
		private void SubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStarted += HandleEvent_MissionDispatcherSystemStarted;
				MissionDispatcher.SystemStopped += HandleEvent_MissionDispatcherSystemStopped;
			}
		}
		private void UnsubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStarted -= HandleEvent_MissionDispatcherSystemStarted;
				MissionDispatcher.SystemStopped -= HandleEvent_MissionDispatcherSystemStopped;
			}
		}
		private void SubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStarted += HandleEvent_CycleMissionGeneratorSystemStarted;
				CycleMissionGenerator.SystemStopped += HandleEvent_CycleMissionGeneratorSystemStopped;
			}
		}
		private void UnsubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStarted -= HandleEvent_CycleMissionGeneratorSystemStarted;
				CycleMissionGenerator.SystemStopped -= HandleEvent_CycleMissionGeneratorSystemStopped;
			}
		}
		private void SubscribeEvent_LogExporter(LogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ExportStarted += HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted += HandleEvent_LogExporterExportCompleted;
			}
		}
		private void UnsubscribeEvent_LogExporter(LogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ExportStarted -= HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted -= HandleEvent_LogExporterExportCompleted;
			}
		}
		private void HandleEvent_ImportantEventRecorderSystemStarted(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnImportantEventRecorder, SwitchState.On);
		}
		private void HandleEvent_ImportantEventRecorderSystemStopped(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnImportantEventRecorder, SwitchState.Off);
		}
		private void HandleEvent_VehicleCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port)
		{
			if (NewState == ListenState.Listening)
			{
				UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleCommunicatorServer, SwitchState.On);
			}
			else
			{
				UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleCommunicatorServer, SwitchState.Off);
			}
		}
		private void HandleEvent_VehicleCommunicatorSystemStarted(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleCommunicator, SwitchState.On);
		}
		private void HandleEvent_VehicleCommunicatorSystemStopped(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleCommunicator, SwitchState.Off);
		}
		private void HandleEvent_CollisionEventDetectorSystemStarted(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnCollisionEventDetector, SwitchState.On);
		}
		private void HandleEvent_CollisionEventDetectorSystemStopped(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnCollisionEventDetector, SwitchState.Off);
		}
		private void HandleEvent_VehicleControlHandlerSystemStarted(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleControlHandler, SwitchState.On);
		}
		private void HandleEvent_VehicleControlHandlerSystemStopped(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleControlHandler, SwitchState.Off);
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port)
		{
			if (NewState == ListenState.Listening)
			{
				UpdateGui_UpdateSwitchButtonSwitchState(sbtnHostCommunicatorServer, SwitchState.On);
			}
			else
			{
				UpdateGui_UpdateSwitchButtonSwitchState(sbtnHostCommunicatorServer, SwitchState.Off);
			}
		}
		private void HandleEvent_HostCommunicatorSystemStarted(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnHostCommunicator, SwitchState.On);
		}
		private void HandleEvent_HostCommunicatorSystemStopped(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnHostCommunicator, SwitchState.Off);
		}
		private void HandleEvent_MissionDispatcherSystemStarted(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnMissionDispatcher, SwitchState.On);
		}
		private void HandleEvent_MissionDispatcherSystemStopped(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnMissionDispatcher, SwitchState.Off);
		}
		private void HandleEvent_CycleMissionGeneratorSystemStarted(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnCycleMissionGenerator, SwitchState.On);
		}
		private void HandleEvent_CycleMissionGeneratorSystemStopped(DateTime OccurTime)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnCycleMissionGenerator, SwitchState.Off);
		}
		private void HandleEvent_LogExporterExportStarted(DateTime OccurTime, string DirectoryPath, List<string> Items)
		{
			btnExportLog.InvokeIfNecessary(() =>
			{
				btnExportLog.Enabled = false;
			});
		}
		private void HandleEvent_LogExporterExportCompleted(DateTime OccurTime, string DirectoryPath, List<string> Items)
		{
			btnExportLog.InvokeIfNecessary(() =>
			{
				btnExportLog.Enabled = true;
			});
		}
		private void UpdateGui_UpdatePanelEnable(bool Enable)
		{
			btnLockPanel.InvokeIfNecessary(() =>
			{
				if (Enable)
				{
					btnLockPanel.Text = "Unlocked";
					btnLockPanel.BackColor = Color.FromArgb(52, 170, 70);
					btnLockPanel.ForeColor = Color.Black;
					tableLayoutPanel1.Enabled = true;
					//tableLayoutPanel1.BackColor = Color.FromArgb(53, 53, 53);
					//tableLayoutPanel2.BackColor = Color.FromArgb(53, 53, 53);
				}
				else
				{
					btnLockPanel.Text = "Locked";
					btnLockPanel.BackColor = BackColor;
					btnLockPanel.ForeColor = ForeColor;
					tableLayoutPanel1.Enabled = false;
					btnLockPanel.Enabled = true;
					//tableLayoutPanel1.BackColor = Color.FromArgb(202, 202, 202);
					//tableLayoutPanel2.BackColor = Color.FromArgb(202, 202, 202);
				}
			});
		}
		private void UpdateGui_UpdateSwitchButtonSwitchState(SwitchButton SwitchButton, SwitchState SwitchState)
		{
			SwitchButton.InvokeIfNecessary(() =>
			{
				SwitchButton.SwitchState = SwitchState;
			});
		}
		private void btnLockPanel_Click(object sender, EventArgs e)
		{
			if (btnLockPanel.Text == "Locked")
			{
				UpdateGui_UpdatePanelEnable(true);
			}
			else
			{
				UpdateGui_UpdatePanelEnable(false);
			}
		}
		private void btnExportLog_Click(object sender, EventArgs e)
		{
			if (!rLogExporter.mIsExporting)
			{
				rLogExporter.StartExport();
			}
		}
		private void sbtnVehicleCommunicatorServer_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnVehicleCommunicatorServer.SwitchState == SwitchState.On)
			{
				rVehicleCommunicator.StopListen();
			}
			else
			{
				rVehicleCommunicator.StartListen();
			}
		}
		private void sbtnHostCommunicatorServer_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnHostCommunicatorServer.SwitchState == SwitchState.On)
			{
				rHostCommunicator.StopListen();
			}
			else
			{
				rHostCommunicator.StartListen();
			}
		}
		private void sbtnImportantEventRecorder_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnImportantEventRecorder.SwitchState == SwitchState.On)
			{
				rImportantEventRecorder.Stop();
			}
			else
			{
				rImportantEventRecorder.Start();
			}
		}
		private void sbtnVehicleCommunicator_DoubleClick(object sender, EventArgs e)
		{
			// 停用，此與 VehicleCommunicator 連動
		}
		private void sbtnCollisionEventDetector_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnCollisionEventDetector.SwitchState == SwitchState.On)
			{
				rCollisionEventDetector.Stop();
			}
			else
			{
				rCollisionEventDetector.Start();
			}
		}
		private void sbtnVehicleControlHandler_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnVehicleControlHandler.SwitchState == SwitchState.On)
			{
				rVehicleControlHandler.Stop();
			}
			else
			{
				rVehicleControlHandler.Start();
			}
		}
		private void sbtnHostCommunicator_DoubleClick(object sender, EventArgs e)
		{
			// 停用，此與 HostCommunicatorServer 連動
		}
		private void sbtnMissionDispatcher_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnMissionDispatcher.SwitchState == SwitchState.On)
			{
				rMissionDispatcher.Stop();
			}
			else
			{
				rMissionDispatcher.Start();
			}
		}
		private void sbtnCycleMissionGenerator_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnCycleMissionGenerator.SwitchState == SwitchState.On)
			{
				rCycleMissionGenerator.Stop();
			}
			else
			{
				rCycleMissionGenerator.Start();
			}
		}
	}
}
