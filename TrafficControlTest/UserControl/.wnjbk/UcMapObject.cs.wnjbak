using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryForVM;
using TrafficControlTest.Module.ChargeStation;
using TrafficControlTest.Module.AutomaticDoor;
using TrafficControlTest.Module.LimitVehicleCountZone;
using TrafficControlTest.Module.ParkStation;

namespace TrafficControlTest.UserControl
{
	public partial class UcMapObject : System.Windows.Forms.UserControl
	{
		private enum TagType
		{
			ChargeStation,
			AutomaticDoor,
			LimitVehicleCountZone,
			ParkStation
		}

		public event EventHandler<MapFocusRequestEventArgs> MapFocusRequest;

		private string mDefaultNameOfButton = "button";
		private string mDefaultNameOfUcMapObject = "ucMapObject";
		private int mSerial = 0;

		public UcMapObject()
		{
			InitializeComponent();
		}
		public void Set(IChargeStationInfoManager ChargeStationInfoManager, IAutomaticDoorInfoManager AutomaticDoorInfoManager, ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager, IParkStationInfoManager ParkStationInfoManager)
		{
			// 加入 ParkStation
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.ParkStation, "ParkStation"));
			panel2.Controls.Add(GenerateUcMapObject(GetSerial(), TagType.ParkStation, ParkStationInfoManager));
			UpdateSerial();
			// 加入 LimitVehicleCountZone
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.LimitVehicleCountZone, "LimitVehicleCountZone"));
			panel2.Controls.Add(GenerateUcMapObject(GetSerial(), TagType.LimitVehicleCountZone, LimitVehicleCountZoneInfoManager));
			UpdateSerial();
			// 加入 AutomaticDoor
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.AutomaticDoor, "AutomaticDoor"));
			panel2.Controls.Add(GenerateUcMapObject(GetSerial(), TagType.AutomaticDoor, AutomaticDoorInfoManager));
			UpdateSerial();
			// 加入 ChargeStation
			panel1.Controls.Add(GenerateButton(GetSerial(), TagType.ChargeStation, "ChargeStation"));
			panel2.Controls.Add(GenerateUcMapObject(GetSerial(), TagType.ChargeStation, ChargeStationInfoManager));
			UpdateSerial();
			// 找一個有顯示的頁面，並點一下該按鈕，以讓畫面更新
			foreach (Control control in panel1.Controls)
			{
				if (control.Visible == true)
				{
					HandleEvent_ButtonClick(control, null);
				}
			}
		}

		protected virtual void RaiseEvent_MapFocusRequest(int X, int Y, bool Sync = true)
		{
			if (Sync)
			{
				MapFocusRequest?.Invoke(this, new MapFocusRequestEventArgs(X, Y));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { MapFocusRequest?.Invoke(this, new MapFocusRequestEventArgs(X, Y)); });
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
		private UcMapObjectTemplate<T> GenerateUcMapObject<T>(string Serial, TagType TagType, IItemManager<T> ItemManager) where T : IItem
		{
			UcMapObjectTemplate<T> result = new UcMapObjectTemplate<T>();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcMapObject}{Serial}";
			result.Tag = TagType;
			result.Set(ItemManager);
			result.MapFocusRequest += HandleEvent_UcMapObjectTemplateMapFocusRequest;
			return result;
		}
		private void UpdateGui_ChangeButtonBackColorAndDisplayUcMapObject(string Serial)
		{
			this.InvokeIfNecessary(() =>
			{
				if (panel1.Controls.ContainsKey($"{mDefaultNameOfButton}{Serial}") && panel2.Controls.ContainsKey($"{mDefaultNameOfUcMapObject}{Serial}"))
				{
					// 調整按鈕背景色
					foreach (Control ctrl in panel1.Controls) // 將所有按鈕背景色還原成 panel 的背景色
					{
						ctrl.BackColor = panel1.BackColor;
					}
					panel1.Controls[$"{mDefaultNameOfButton}{Serial}"].BackColor = Color.FromArgb(0, 122, 204); // 選到的按鈕背景色高亮

					// 顯示對應的 UcMapObject
					panel2.Controls[$"{mDefaultNameOfUcMapObject}{Serial}"].BringToFront();
				}
			});
		}
		private void HandleEvent_ButtonClick(object sender, EventArgs e)
		{
			try
			{
				if ((sender as Control).Name.StartsWith(mDefaultNameOfButton))
				{
					string tmpSerial = (sender as Control).Name.Replace(mDefaultNameOfButton, string.Empty);
					UpdateGui_ChangeButtonBackColorAndDisplayUcMapObject(tmpSerial);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void HandleEvent_UcMapObjectTemplateMapFocusRequest(object sender, MapFocusRequestEventArgs e)
		{
			RaiseEvent_MapFocusRequest(e.X, e.Y);
		}
	}
}
