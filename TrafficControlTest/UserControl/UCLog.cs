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
	/// 可動態新增、移除 Search Page ，至多有 n 個 Search Page 。
	/// Search Page 由一個上方 Panel 的 Button 與下方 Panel 的 UcLogSearch 兩個元件組成。
	/// Button 的 Name 會為 DefaultName + Serial ，UcLogSearch 的 Name 會為 DefaultName + Serial ，兩者的 Serial 會相等。
	/// Button 用來標示當前使用的 Search Page 與搜尋關鍵字， UcLogSearch 用來顯示搜尋結果。
	/// </summary>
	public partial class UcLog : System.Windows.Forms.UserControl
	{
		private List<Button> mButtons = new List<Button>();
		private List<UcLogSearch> mUcLogSearches = new List<UcLogSearch>();
		private string mDefaultNameOfButton = "button";
		private string mDefaultNameOfUcLogSearch = "ucLogSearch";
		private int mMaxCountOfSearchPage = 3;

		public UcLog()
		{
			InitializeComponent();
			InitializeSearchPageControls();
		}
		public void Set(DatabaseAdapter DatabaseAdapter)
		{
			if (DatabaseAdapter != null)
			{
				for (int i = 0; i < mMaxCountOfSearchPage; ++i)
				{
					mUcLogSearches[i].Set(DatabaseAdapter);
					mUcLogSearches[i].DoDefaultSearch();
					UpdateGui_ButtonAndUcLogSearch_ChangeButtonBackColorAndDisplayUcLogSearch(i.ToString());
				}
			}
		}

		private void InitializeSearchPageControls()
		{
			for (int i = 0; i < mMaxCountOfSearchPage; ++i)
			{
				mButtons.Add(GenerateButton(i.ToString()));
				mUcLogSearches.Add(GenerateUcLogSearch(i.ToString()));
				panel1.Controls.Add(mButtons[i]);
				panel2.Controls.Add(mUcLogSearches[i]);
			}
		}
		private Button GenerateButton(string Serial)
		{
			Button result = new Button();
			result.Name = $"{mDefaultNameOfButton}{Serial}";
			result.FlatStyle = FlatStyle.Flat;
			result.FlatAppearance.BorderSize = 0;
			result.Text = string.Empty;
			result.Dock = DockStyle.Left;
			result.Font = new Font("新細明體", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 136);
			result.MinimumSize = new Size(200, 50);
			result.AutoSize = true;
			result.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			result.Click += HandleEvent_ButtonClick;
			return result;
		}
		private UcLogSearch GenerateUcLogSearch(string Serial)
		{
			UcLogSearch result = new UcLogSearch();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcLogSearch}{Serial}";
			result.SearchSuccessed += HandleEvent_UcLogSearchSearchSuccessed;
			return result;
		}
		private void UpdateGui_ButtonAndUcLogSearch_ChangeButtonBackColorAndDisplayUcLogSearch(string Serial)
		{
			this.InvokeIfNecessary(() =>
			{
				if (panel1.Controls.ContainsKey($"{mDefaultNameOfButton}{Serial}") && panel2.Controls.ContainsKey($"{mDefaultNameOfUcLogSearch}{Serial}"))
				{
					// 調整按鈕背景色
					foreach (Control ctrl in panel1.Controls)
					{
						ctrl.BackColor = panel1.BackColor;
					}
					panel1.Controls[$"{mDefaultNameOfButton}{Serial}"].BackColor = Color.FromArgb(0, 122, 204);

					// 顯示對應的 UcLogSearch
					panel2.Controls[$"{mDefaultNameOfUcLogSearch}{Serial}"].BringToFront();
					(panel2.Controls[$"{mDefaultNameOfUcLogSearch}{Serial}"] as UcLogSearch).FocusOnSearchTextBox();
				}
			});
		}
		private void UpdateGui_ButtonAndUcLogSearch_UpdateButtonText(string Serial, string NewText)
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
				UpdateGui_ButtonAndUcLogSearch_ChangeButtonBackColorAndDisplayUcLogSearch(tmpSerial);
			}
		}
		private void HandleEvent_UcLogSearchSearchSuccessed(object Sender, DateTime OccurTime, string Keyword, int Limit)
		{
			string tmpSerial = (Sender as Control).Name.Replace(mDefaultNameOfUcLogSearch, string.Empty);
			UpdateGui_ButtonAndUcLogSearch_UpdateButtonText(tmpSerial, Keyword);
		}
	}
}
