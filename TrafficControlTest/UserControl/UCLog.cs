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
	/// Search Page 由一個上方 Panel 的 Button 與下方 Panel 的 UcSearch 兩個元件組成。
	/// Button 的 Name 會為 DefaultName + Serial ，UcSearch 的 Name 會為 DefaultName + Serial ，兩者的 Serial 會相等。
	/// Button 用來標示當前使用的 Search Page 與搜尋關鍵字， UcSearch 用來顯示搜尋結果。
	/// </summary>
	public partial class UcLog : System.Windows.Forms.UserControl
	{
		private List<Button> mButtons = new List<Button>();
		private List<UcSearch> mUcSearches = new List<UcSearch>();
		private string mDefaultNameOfButton = "button";
		private string mDefaultNameOfUcSearch = "ucSearch";
		private int mSerial = 0;

		public UcLog()
		{
			InitializeComponent();
			InitializeSearchPageControls();
		}
		public void Set(DatabaseAdapter DatabaseAdapterOfLogRecord, DatabaseAdapter DatabaseAdapterOfEventRecord)
		{
			if (DatabaseAdapterOfLogRecord != null && mUcSearches.Count >= 3)
			{
				mUcSearches[0].Set(DatabaseAdapterOfLogRecord);
				mUcSearches[1].Set(DatabaseAdapterOfLogRecord);
				mUcSearches[2].Set(DatabaseAdapterOfLogRecord);
			}
			if (DatabaseAdapterOfEventRecord != null && mUcSearches.Count >= 4)
			{
				mUcSearches[3].Set(DatabaseAdapterOfEventRecord);
			}

			for (int i = 0; i < mUcSearches.Count; ++i)
			{
				mUcSearches[i].DoDefaultSearch();
				UpdateGui_ButtonAndUcSearch_ChangeButtonBackColorAndDisplayUcSearch(i.ToString());
			}
		}

		private int GetSerial()
		{
			return mSerial;
		}
		private void UpdateSerial()
		{
			mSerial += 1;
		}
		private void InitializeSearchPageControls()
		{
			AddSearchPageOfGeneralLog();
			AddSearchPageOfGeneralLog();
			AddSearchPageOfGeneralLog();
			AddSearchPageOfMissionState();
		}
		private void AddSearchPageOfGeneralLog()
		{
			mButtons.Add(GenerateButton(GetSerial().ToString()));
			mUcSearches.Add(GenerateUcSearchGeneralLog(GetSerial().ToString()));
			panel1.Controls.Add(mButtons[GetSerial()]);
			panel2.Controls.Add(mUcSearches[GetSerial()]);
			UpdateSerial();
		}
		private void AddSearchPageOfMissionState()
		{
			mButtons.Add(GenerateButton(GetSerial().ToString()));
			mUcSearches.Add(GenerateUcSearchMissionState(GetSerial().ToString()));
			panel1.Controls.Add(mButtons[GetSerial()]);
			panel2.Controls.Add(mUcSearches[GetSerial()]);
			UpdateSerial();
		}
		private Button GenerateButton(string Serial)
		{
			Button result = new Button();
			result.Name = $"{mDefaultNameOfButton}{Serial}";
			result.FlatStyle = FlatStyle.Flat;
			result.FlatAppearance.BorderSize = 0;
			result.Text = "None";
			result.Dock = DockStyle.Left;
			result.Font = new Font("新細明體", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 136);
			result.MinimumSize = new Size(200, 50);
			result.AutoSize = true;
			result.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			result.Click += HandleEvent_ButtonClick;
			return result;
		}
		private UcSearch GenerateUcSearchGeneralLog(string Serial)
		{
			UcSearch result = new UcSearchGeneralLog();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcSearch}{Serial}";
			result.SearchSuccessed += HandleEvent_UcSearchSearchSuccessed;
			return result;
		}
		private UcSearch GenerateUcSearchMissionState(string Serial)
		{
			UcSearch result = new UcSearchMissionState();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcSearch}{Serial}";
			result.SearchSuccessed += HandleEvent_UcSearchSearchSuccessed;
			return result;
		}
		private void UpdateGui_ButtonAndUcSearch_ChangeButtonBackColorAndDisplayUcSearch(string Serial)
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
		private void UpdateGui_ButtonAndUcSearch_UpdateButtonText(string Serial, string NewText)
		{
			this.InvokeIfNecessary(() =>
			{
				(panel1.Controls[$"{mDefaultNameOfButton}{Serial}"] as Button).Text = NewText;
			});
		}
		private void HandleEvent_ButtonClick(object sender, EventArgs e)
		{
			if ((sender as Control).Name.StartsWith(mDefaultNameOfButton))
			{
				string tmpSerial = (sender as Control).Name.Replace(mDefaultNameOfButton, string.Empty);
				UpdateGui_ButtonAndUcSearch_ChangeButtonBackColorAndDisplayUcSearch(tmpSerial);
			}
		}
		private void HandleEvent_UcSearchSearchSuccessed(object Sender, DateTime OccurTime, string Keyword, int Limit)
		{
			string tmpSerial = (Sender as Control).Name.Replace(mDefaultNameOfUcSearch, string.Empty);
			UpdateGui_ButtonAndUcSearch_UpdateButtonText(tmpSerial, (Sender as UcSearch).mKeyword+ " - " + Keyword);
		}
	}
}
