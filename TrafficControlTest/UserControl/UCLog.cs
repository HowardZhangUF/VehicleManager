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
	/// Button 用來標示當前使用的 SearchControl 與搜尋關鍵字， UcSearch 用來顯示搜尋結果。
	/// </summary>
	public partial class UcLog : System.Windows.Forms.UserControl
	{
		private enum SearchControlType
		{
			GeneralLog,
			MissionState,
			HostCommunication
		}

		private DatabaseAdapter rDatabaseAdapterOfLogRecord = null;
		private DatabaseAdapter rDatabaseAdapterOfEventRecord = null;
		private Dictionary<SearchControlType, Queue<SearchControl>> mUnusedSearchControlCollection = new Dictionary<SearchControlType, Queue<SearchControl>>()
		{
			{ SearchControlType.GeneralLog, new Queue<SearchControl>() },
			{ SearchControlType.MissionState, new Queue<SearchControl>() },
			{ SearchControlType.HostCommunication, new Queue<SearchControl>() }
		};
		private Dictionary<SearchControlType, Queue<SearchControl>> mUsedSearchControlCollection = new Dictionary<SearchControlType, Queue<SearchControl>>()
		{
			{ SearchControlType.GeneralLog, new Queue<SearchControl>() },
			{ SearchControlType.MissionState, new Queue<SearchControl>() },
			{ SearchControlType.HostCommunication, new Queue<SearchControl>() }
		};
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

			if (rDatabaseAdapterOfLogRecord != null)
			{
				foreach (SearchControl searchControl in mUnusedSearchControlCollection[SearchControlType.GeneralLog])
				{
					searchControl.mUcSearch.Set(rDatabaseAdapterOfLogRecord);
				}
				foreach (SearchControl searchControl in mUsedSearchControlCollection[SearchControlType.GeneralLog])
				{
					searchControl.mUcSearch.Set(rDatabaseAdapterOfLogRecord);
				}
			}

			if (rDatabaseAdapterOfEventRecord != null)
			{
				foreach (SearchControl searchControl in mUnusedSearchControlCollection[SearchControlType.MissionState])
				{
					searchControl.mUcSearch.Set(rDatabaseAdapterOfEventRecord);
				}
				foreach (SearchControl searchControl in mUsedSearchControlCollection[SearchControlType.MissionState])
				{
					searchControl.mUcSearch.Set(rDatabaseAdapterOfEventRecord);
				}
				foreach (SearchControl searchControl in mUnusedSearchControlCollection[SearchControlType.HostCommunication])
				{
					searchControl.mUcSearch.Set(rDatabaseAdapterOfEventRecord);
				}
				foreach (SearchControl searchControl in mUsedSearchControlCollection[SearchControlType.HostCommunication])
				{
					searchControl.mUcSearch.Set(rDatabaseAdapterOfEventRecord);
				}
			}
		}
		public void Set(int SearchControlOfGeneralLogCount, int SearchControlOfMissionStateCount, int SearchControlOfHostCommunicationCount)
		{
			// 從介面上移除所有的 SearchControl
			RemoveAllSearchControlFromInterface();

			// 加入 GeneralLog 的 SearchControl 至介面上
			for (int i = 0; i < SearchControlOfGeneralLogCount; ++i)
			{
				AddSearchControlToUserInterface(SearchControlType.GeneralLog);
			}

			// 加入 MissionState 的 SearchControl 至介面上
			for (int i = 0; i < SearchControlOfMissionStateCount; ++i)
			{
				AddSearchControlToUserInterface(SearchControlType.MissionState);
			}

			// 加入 HostCommunication 的 SearchControl 至介面上
			for (int i = 0; i < SearchControlOfHostCommunicationCount; ++i)
			{
				AddSearchControlToUserInterface(SearchControlType.HostCommunication);
			}

			// 讓所有 SearchControl 做一次預設搜尋
			MakeAllSearchControlDoDefaultSearch();

			// 點一下最左邊的 Button
			if (panel1.Controls.Count > 0) HandleEvent_ButtonClick(panel1.Controls[panel1.Controls.Count - 1], null);
		}

		private string GetSerial()
		{
			return mSerial.ToString();
		}
		private void UpdateSerial()
		{
			mSerial += 1;
		}
		private void AddSearchControlToCollection(SearchControlType Type)
		{
			// 根據 Type 產生新的 SearchControl 並設定好 DatabaseAdapter 然後加入至 Unused Collection 中
			switch (Type)
			{
				case SearchControlType.GeneralLog:
					SearchControl tmpSearchControl1 = new SearchControl(GenerateButton(GetSerial()), GenerateUcSearchGeneralLog(GetSerial()), GetSerial());
					UpdateSerial();
					if (rDatabaseAdapterOfLogRecord != null) tmpSearchControl1.mUcSearch.Set(rDatabaseAdapterOfLogRecord);
					mUnusedSearchControlCollection[Type].Enqueue(tmpSearchControl1);
					break;
				case SearchControlType.MissionState:
					SearchControl tmpSearchControl2 = new SearchControl(GenerateButton(GetSerial()), GenerateUcSearchMissionState(GetSerial()), GetSerial());
					UpdateSerial();
					if (rDatabaseAdapterOfEventRecord != null) tmpSearchControl2.mUcSearch.Set(rDatabaseAdapterOfEventRecord);
					mUnusedSearchControlCollection[Type].Enqueue(tmpSearchControl2);
					break;
				case SearchControlType.HostCommunication:
					SearchControl tmpSearchControl3 = new SearchControl(GenerateButton(GetSerial()), GenerateUcSearchHostCommunication(GetSerial()), GetSerial());
					UpdateSerial();
					if (rDatabaseAdapterOfEventRecord != null) tmpSearchControl3.mUcSearch.Set(rDatabaseAdapterOfEventRecord);
					mUnusedSearchControlCollection[Type].Enqueue(tmpSearchControl3);
					break;
			}
		}
		private void AddSearchControlToUserInterface(SearchControlType Type)
		{
			// 從 Unused Collection 中拿出 SearchControl 顯示於介面上並加入 Used Collection
			if (Type == SearchControlType.GeneralLog || Type == SearchControlType.MissionState || Type == SearchControlType.HostCommunication)
			{
				if (mUnusedSearchControlCollection[Type].Count == 0) AddSearchControlToCollection(Type);
				SearchControl tmpSearchControl = mUnusedSearchControlCollection[Type].Dequeue();
				panel1.Controls.Add(tmpSearchControl.mButton);
				panel2.Controls.Add(tmpSearchControl.mUcSearch);
				mUsedSearchControlCollection[Type].Enqueue(tmpSearchControl);
			}
		}
		private void RemoveAllSearchControlFromInterface()
		{
			// 從介面上移除所有的 SearchControl 並將 SearchControl 資料移動至 Unused Collection
			panel1.Controls.Clear();
			panel2.Controls.Clear();
			TransferData(mUsedSearchControlCollection[SearchControlType.GeneralLog], mUnusedSearchControlCollection[SearchControlType.GeneralLog]);
			TransferData(mUsedSearchControlCollection[SearchControlType.MissionState], mUnusedSearchControlCollection[SearchControlType.MissionState]);
			TransferData(mUsedSearchControlCollection[SearchControlType.HostCommunication], mUnusedSearchControlCollection[SearchControlType.HostCommunication]);
		}
		private void MakeAllSearchControlDoDefaultSearch()
		{
			foreach (SearchControl searchControl in mUsedSearchControlCollection[SearchControlType.GeneralLog])
			{
				searchControl.mUcSearch.DoDefaultSearch();
			}
			foreach (SearchControl searchControl in mUsedSearchControlCollection[SearchControlType.MissionState])
			{
				searchControl.mUcSearch.DoDefaultSearch();
			}
			foreach (SearchControl searchControl in mUsedSearchControlCollection[SearchControlType.HostCommunication])
			{
				searchControl.mUcSearch.DoDefaultSearch();
			}
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
		private UcSearch GenerateUcSearchHostCommunication(string Serial)
		{
			UcSearch result = new UcSearchHostCommunication();
			result.Dock = DockStyle.Fill;
			result.Name = $"{mDefaultNameOfUcSearch}{Serial}";
			result.SearchSuccessed += HandleEvent_UcSearchSearchSuccessed;
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
		private void UpdateGui_SearchControl_UpdateButtonText(string Serial, string NewText)
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
				UpdateGui_SearchControl_ChangeButtonBackColorAndDisplayUcSearch(tmpSerial);
			}
		}
		private void HandleEvent_UcSearchSearchSuccessed(object Sender, DateTime OccurTime, string Keyword, int Limit)
		{
			string tmpSerial = (Sender as Control).Name.Replace(mDefaultNameOfUcSearch, string.Empty);
			UpdateGui_SearchControl_UpdateButtonText(tmpSerial, (Sender as UcSearch).mKeyword+ " - " + Keyword);
		}
		private static void TransferData<T>(Queue<T> SrcList, Queue<T> DstList)
		{
			while (SrcList.Count > 0)
			{
				DstList.Enqueue(SrcList.Dequeue());
			}
		}
	}

    public class SearchControl
    {
        public Button mButton { get; } = null;
        public UcSearch mUcSearch { get; } = null;
		public string mSerial { get; } = string.Empty;

        public SearchControl(Button Button, UcSearch UcSearch, string Serial)
        {
            mButton = Button;
            mUcSearch = UcSearch;
			mSerial = Serial;
        }
    }
}
