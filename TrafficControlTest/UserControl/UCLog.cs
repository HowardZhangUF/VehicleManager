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
		private DatabaseAdapter rDatabaseAdapter = null;
		private List<Button> mButtons = new List<Button>();
		private List<UcLogSearch> mUcLogSearches = new List<UcLogSearch>();
		private object mLock = new object();
		private string mDefaultNameOfButton = "button";
		private string mDefaultNameOfUcLogSearch = "ucLogSearch";
		private int mSerial = 0;
		private int mMaxCountOfSearchPage = 5;

		public UcLog()
		{
			InitializeComponent();
		}
		public void Set(DatabaseAdapter DatabaseAdapter)
		{
			rDatabaseAdapter = DatabaseAdapter;
		}

		private int GetSerial()
		{
			return mSerial;
		}
		private void UpdateSerial()
		{
			mSerial = (mSerial == int.MaxValue) ? 0 : mSerial + 1;
		}
		private void UpdateGui_ButtonAndUcLogSearch_Add()
		{
			// 新增 Button
			Button btn = new Button();
			btn.Name = $"{mDefaultNameOfButton}{GetSerial().ToString()}";
			btn.FlatStyle = FlatStyle.Flat;
			btn.FlatAppearance.BorderSize = 0;
			btn.Text = string.Empty;
			btn.Dock = DockStyle.Left;
			btn.Font = new Font("新細明體", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 136);
			btn.MinimumSize = new Size(200, 50);
			btn.AutoSize = true;
			btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			btn.Click += HandleEvent_ButtonClick;

			// 新增 UCLogSearch
			UcLogSearch ucLogSearch = new UcLogSearch();
			ucLogSearch.Dock = DockStyle.Fill;
			ucLogSearch.Name = $"{mDefaultNameOfUcLogSearch}{GetSerial().ToString()}";
			ucLogSearch.ClickOnCloseButton += HandleEvent_UcLogSearchClickOnCloseButton;
			ucLogSearch.SearchSuccessed += HandleEvent_UcLogSearchSearchSuccessed;
			ucLogSearch.Set(rDatabaseAdapter);

			// 加入 Button 與 UCLogSearch 控制項
			lock (mLock)
			{
				mButtons.Add(btn);
				panel1.Controls.Add(btn);
				mUcLogSearches.Add(ucLogSearch);
				panel2.Controls.Add(ucLogSearch);
			}

			// 調整 Button 的 ChildIndex
			for (int i = 0; i < mButtons.Count; ++i)
			{
				panel1.Controls.SetChildIndex(mButtons[i], mButtons.Count - 1 - i);
			}

			// 顯示最新加入的 UCLogSearch
			UpdateGui_ButtonAndUcLogSearch_ChangeButtonBackColorAndDisplayUcLogSearch(GetSerial().ToString());

			// UcLogSearch 做一次預設搜尋
			ucLogSearch.DoDefaultSearch();

			// 更新序列號
			UpdateSerial();
		}
		private void UpdateGui_ButtonAndUcLogSearch_Remove(string Serial)
		{
			if (panel1.Controls.ContainsKey($"{mDefaultNameOfButton}{Serial}") && panel2.Controls.ContainsKey($"{mDefaultNameOfUcLogSearch}{Serial}"))
			{
				Button tmpButton = (panel1.Controls[$"{mDefaultNameOfButton}{Serial}"] as Button);
				UcLogSearch tmpUcLogSearch = (panel2.Controls[$"{mDefaultNameOfUcLogSearch}{Serial}"] as UcLogSearch);
				int tmpIndex = mButtons.IndexOf(tmpButton);

				// 取消訂閱事件
				tmpButton.Click -= HandleEvent_ButtonClick;
				tmpUcLogSearch.ClickOnCloseButton -= HandleEvent_UcLogSearchClickOnCloseButton;
				tmpUcLogSearch.SearchSuccessed -= HandleEvent_UcLogSearchSearchSuccessed;

				// 移除控制項
				lock (mLock)
				{
					mButtons.Remove(tmpButton);
					mUcLogSearches.Remove(tmpUcLogSearch);
					panel1.Controls.Remove(tmpButton);
					panel2.Controls.Remove(tmpUcLogSearch);
				}

				// 顯示其他剩餘的 UCLogSearch
				if (mButtons.Count > 0)
				{
					int nextIndex = (tmpIndex == mButtons.Count) ? tmpIndex - 1 : tmpIndex;
					UpdateGui_ButtonAndUcLogSearch_ChangeButtonBackColorAndDisplayUcLogSearch(mUcLogSearches[nextIndex].Name.Replace(mDefaultNameOfUcLogSearch, string.Empty));
				}
			}
		}
		private void UpdateGui_ButtonAndUcLogSearch_ChangeButtonBackColorAndDisplayUcLogSearch(string Serial)
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
		}
		private void UpdateGui_ButtonAndUcLogSearch_UpdateButtonText(string Serial, string NewText)
		{
			Button tmpButton = (panel1.Controls[$"{mDefaultNameOfButton}{Serial}"] as Button);
			tmpButton.Text = NewText;
		}
		private void HandleEvent_ButtonClick(object sender, EventArgs e)
		{
			if ((sender as Control).Name.StartsWith(mDefaultNameOfButton))
			{
				string tmpSerial = (sender as Control).Name.Replace(mDefaultNameOfButton, string.Empty);
				UpdateGui_ButtonAndUcLogSearch_ChangeButtonBackColorAndDisplayUcLogSearch(tmpSerial);
			}
		}
		private void HandleEvent_UcLogSearchClickOnCloseButton(object sender, EventArgs e)
		{
			UpdateGui_ButtonAndUcLogSearch_Remove((sender as Control).Name.Replace(mDefaultNameOfUcLogSearch, string.Empty));
		}
		private void HandleEvent_UcLogSearchSearchSuccessed(object Sender, DateTime OccurTime, string Keyword, int Limit)
		{
			UpdateGui_ButtonAndUcLogSearch_UpdateButtonText((Sender as Control).Name.Replace(mDefaultNameOfUcLogSearch, string.Empty), Keyword);
		}
		private void btnAddSearchPage_Click(object sender, EventArgs e)
		{
			if (mButtons.Count < 5 && mUcLogSearches.Count < 5)
			{
				UpdateGui_ButtonAndUcLogSearch_Add();
			}
			else
			{
				MessageBox.Show($"Can't Add Search Page Anymore!\r\nMaximum: {mMaxCountOfSearchPage.ToString()}", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
	}
}
