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

namespace TrafficControlTest.UserControl
{
	/// <summary>
	/// SearchControl 由一個上方 Panel 的 Button 與下方 Panel 的 UcSearch 兩個元件組成。
	/// Button 的 Name 會為 DefaultName + Serial ，UcSearch 的 Name 會為 DefaultName + Serial ，兩者的 Serial 會相等。
	/// Button 用來標示當前使用的 SearchControl ， UcSearch 用來顯示搜尋結果。
	/// </summary>
	public partial class UcLog : System.Windows.Forms.UserControl
	{
		private enum TagType
		{
			GeneralLog,
			AutomaticDoorControl,
			VehicleControl,
			MissionState,
			HostCommunication
		}

		private DatabaseAdapter rDatabaseAdapterOfLogRecord = null;
		private DatabaseAdapter rDatabaseAdapterOfEventRecord = null;
		private string mDefaultNameOfButton = "button";
		private string mDefaultNameOfUcSearch = "ucSearch";
		private int mSerial = 0;

		public UcLog()
		{
			InitializeComponent();
		}
		public void Set(DatabaseAdapter DatabaseAdapterOfLogRecord, DatabaseAdapter DatabaseAdapterOfEventRecord)
		{
			rDatabaseAdapterOfLogRecord = DatabaseAdapterOfLogRecord;
			rDatabaseAdapterOfEventRecord = DatabaseAdapterOfEventRecord;

			// 加入 General Log 3
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.GeneralLog, "GeneralLog3"));
			panel2.Controls.Add(GenerateUcSearchGeneralLog(GetSerial(), TagType.GeneralLog, rDatabaseAdapterOfLogRecord));
			UpdateSerial();
			// 加入 General Log 2
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.GeneralLog, "GeneralLog2"));
			panel2.Controls.Add(GenerateUcSearchGeneralLog(GetSerial(), TagType.GeneralLog, rDatabaseAdapterOfLogRecord));
			UpdateSerial();
			// 加入 General Log 1
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.GeneralLog, "GeneralLog1"));
			panel2.Controls.Add(GenerateUcSearchGeneralLog(GetSerial(), TagType.GeneralLog, rDatabaseAdapterOfLogRecord));
			UpdateSerial();
			// 加入 AutomaticDoorControl
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.AutomaticDoorControl, "AutomaticDoorControl"));
			panel2.Controls.Add(GenerateUcSearchAutomaticDoorControl(GetSerial(), TagType.AutomaticDoorControl, rDatabaseAdapterOfEventRecord));
			UpdateSerial();
			// 加入 VehicleControl
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.VehicleControl, "VehicleControl"));
			panel2.Controls.Add(GenerateUcSearchVehicleControl(GetSerial(), TagType.VehicleControl, rDatabaseAdapterOfEventRecord));
			UpdateSerial();
			// 加入 MissionState
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.MissionState, "MissionState"));
			panel2.Controls.Add(GenerateUcSearchMissionState(GetSerial(), TagType.MissionState, rDatabaseAdapterOfEventRecord));
			UpdateSerial();
			// 加入 HostCommunication
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.HostCommunication, "HostCommunication"));
			panel2.Controls.Add(GenerateUcSearchHostCommunication(GetSerial(), TagType.HostCommunication, rDatabaseAdapterOfEventRecord));
			UpdateSerial();
		}
		public void Set(bool SearchControlOfGeneralLogDisplay, bool SearchControlOfAutomaticDoorControlDisplay, bool SearchControlOfVehicleControlDisplay, bool SearchControlOfMissionStateDisplay, bool SearchControlOfHostCommunicationDisplay)
		{
			foreach (Control control in panel1.Controls)
			{
				switch (control.Tag.ToString())
				{
					case "GeneralLog":
						control.Visible = SearchControlOfGeneralLogDisplay;
						break;
					case "AutomaticDoorControl":
						control.Visible = SearchControlOfAutomaticDoorControlDisplay;
						break;
					case "VehicleControl":
						control.Visible = SearchControlOfVehicleControlDisplay;
						break;
					case "MissionState":
						control.Visible = SearchControlOfMissionStateDisplay;
						break;
					case "HostCommunication":
						control.Visible = SearchControlOfHostCommunicationDisplay;
						break;
					default:
						break;
				}
			}

			// 找一個有顯示的頁面，並點一下該按鈕，以讓畫面更新
			foreach (Control control in panel1.Controls)
			{
				if (control.Visible == true)
				{
					HandleEvent_ButtonClick(control, null);
				}
			}
		}

		private string GetSerial()
		{
			return mSerial.ToString();
		}
		private void UpdateSerial()
		{
			mSerial += 1;
		}
		private Button GenerateButton(string Serial, TagType TagType, string Text)
		{
			Button result = new ButtonWithToolTip();
			result.Name = $"{mDefaultNameOfButton}{Serial}";
			result.FlatStyle = FlatStyle.Flat;
			result.FlatAppearance.BorderSize = 0;
			result.Text = Text;
			result.Tag = TagType;
			result.Dock = DockStyle.Left;
			result.Font = new Font("新細明體", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 136);
			result.MinimumSize = new Size(TextRenderer.MeasureText(result.Text, result.Font).Width + 20, 50);
			result.AutoSize = true;
			result.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			result.Click += HandleEvent_ButtonClick;
			return result;
		}
		private UcSearch GenerateUcSearchGeneralLog(string Serial, TagType TagType, DatabaseAdapter DatabaseAdapter)
		{
			UcSearch result = new UcSearchGeneralLog();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcSearch}{Serial}";
			result.Tag = TagType;
			result.SearchSuccessed += HandleEvent_UcSearchSearchSuccessed;
			result.Set(DatabaseAdapter);
			return result;
		}
		private UcSearch GenerateUcSearchMissionState(string Serial, TagType TagType, DatabaseAdapter DatabaseAdapter)
		{
			UcSearch result = new UcSearchMissionState();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcSearch}{Serial}";
			result.Tag = TagType;
			result.SearchSuccessed += HandleEvent_UcSearchSearchSuccessed;
			result.Set(DatabaseAdapter);
			return result;
		}
		private UcSearch GenerateUcSearchVehicleControl(string Serial, TagType TagType, DatabaseAdapter DatabaseAdapter)
		{
			UcSearch result = new UcSearchVehicleControl();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcSearch}{Serial}";
			result.Tag = TagType;
			result.SearchSuccessed += HandleEvent_UcSearchSearchSuccessed;
			result.Set(DatabaseAdapter);
			return result;
		}
		private UcSearch GenerateUcSearchAutomaticDoorControl(string Serial, TagType TagType, DatabaseAdapter DatabaseAdapter)
		{
			UcSearch result = new UcSearchAutomaticDoorControl();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcSearch}{Serial}";
			result.Tag = TagType;
			result.SearchSuccessed += HandleEvent_UcSearchSearchSuccessed;
			result.Set(DatabaseAdapter);
			return result;
		}
		private UcSearch GenerateUcSearchHostCommunication(string Serial, TagType TagType, DatabaseAdapter DatabaseAdapter)
		{
			UcSearch result = new UcSearchHostCommunication();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcSearch}{Serial}";
			result.Tag = TagType;
			result.SearchSuccessed += HandleEvent_UcSearchSearchSuccessed;
			result.Set(DatabaseAdapter);
			return result;
		}
		private void UpdateGui_SearchControl_ChangeButtonBackColorAndDisplayUcSearch(string Serial)
		{
			this.InvokeIfNecessary(() =>
			{
				if (panel1.Controls.ContainsKey($"{mDefaultNameOfButton}{Serial}") && panel2.Controls.ContainsKey($"{mDefaultNameOfUcSearch}{Serial}"))
				{
					// 調整按鈕背景色
					foreach (Control ctrl in panel1.Controls)
					{
						ctrl.BackColor = panel1.BackColor;
					}
					panel1.Controls[$"{mDefaultNameOfButton}{Serial}"].BackColor = Color.FromArgb(0, 122, 204);

					// 顯示對應的 UcSearch
					panel2.Controls[$"{mDefaultNameOfUcSearch}{Serial}"].BringToFront();
					(panel2.Controls[$"{mDefaultNameOfUcSearch}{Serial}"] as UcSearch).UpdateGui_FocusOnSearchTextBox();
				}
			});
		}
		private void UpdateGui_SearchControl_UpdateButtonWithToolTipToolTipText(string Serial, string NewText)
		{
			this.InvokeIfNecessary(() =>
			{
				(panel1.Controls[$"{mDefaultNameOfButton}{Serial}"] as ButtonWithToolTip).SetToolTipText(NewText);
			});
		}
		private void HandleEvent_ButtonClick(object sender, EventArgs e)
		{
			try
			{
				if ((sender as Control).Name.StartsWith(mDefaultNameOfButton))
				{
					string tmpSerial = (sender as Control).Name.Replace(mDefaultNameOfButton, string.Empty);
					UpdateGui_SearchControl_ChangeButtonBackColorAndDisplayUcSearch(tmpSerial);
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
		private void HandleEvent_UcSearchSearchSuccessed(object Sender, DateTime OccurTime, string SearchCondition)
		{
			string tmpSerial = (Sender as Control).Name.Replace(mDefaultNameOfUcSearch, string.Empty);
			UpdateGui_SearchControl_UpdateButtonWithToolTipToolTipText(tmpSerial, SearchCondition);
		}
	}
}
