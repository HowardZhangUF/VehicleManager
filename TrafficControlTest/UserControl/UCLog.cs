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
	/// Search Page 由一個上方 Panel 的 Button 與下方 Panel 的 UcSearchGeneralLog 兩個元件組成。
	/// Button 的 Name 會為 DefaultName + Serial ，UcSearchGeneralLog 的 Name 會為 DefaultName + Serial ，兩者的 Serial 會相等。
	/// Button 用來標示當前使用的 Search Page 與搜尋關鍵字， UcSearchGeneralLog 用來顯示搜尋結果。
	/// </summary>
	public partial class UcLog : System.Windows.Forms.UserControl
	{
		private List<Button> mButtons = new List<Button>();
		private List<UcSearch> mUcSearches = new List<UcSearch>();
		private string mDefaultNameOfButton = "button";
		private string mDefaultNameOfUcSearch = "UcSearch";
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
					mUcSearches[i].Set(DatabaseAdapter);
					mUcSearches[i].DoDefaultSearch();
					UpdateGui_ButtonAndUcSearch_ChangeButtonBackColorAndDisplayUcSearch(i.ToString());
				}
			}
		}

		private void InitializeSearchPageControls()
		{
			for (int i = 0; i < mMaxCountOfSearchPage; ++i)
			{
				mButtons.Add(GenerateButton(i.ToString()));
				mUcSearches.Add(GenerateUcSearch(i.ToString()));
				panel1.Controls.Add(mButtons[i]);
				panel2.Controls.Add(mUcSearches[i]);
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
		private UcSearch GenerateUcSearch(string Serial)
		{
			UcSearch result = new UcSearchGeneralLog();
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
			UpdateGui_ButtonAndUcSearch_UpdateButtonText(tmpSerial, Keyword);
		}
	}
}
