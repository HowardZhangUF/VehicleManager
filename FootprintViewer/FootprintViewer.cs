using GLStyle;
using GLUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootprintViewer
{
	public partial class FootprintViewer : Form
	{
		public FootprintViewer()
		{
			InitializeComponent();
		}

		private void FootprintViewer_Load(object sender, EventArgs e)
		{
			StyleManager.LoadStyle("Style.ini");

			gluiCtrl1.LoadMapEvent += gluiCtrl1_LoadMapEvent;

			gluiCtrl1.SetEditMode(false);
			gluiCtrl1.SetControlMode(false);

			if (txtFootprintDirectory.Text != "")
			{
				checkFootprintPath(txtFootprintDirectory.Text);
			}
		}

		private void FootprintViewer_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		/// <summary>
		/// 載入地圖
		/// </summary>
		private void btnBrowseMapPath_Click(object sender, EventArgs e)
		{
			gluiCtrl1.LoadMap();
		}

		/// <summary>
		/// 載入 Footprint 資料夾
		/// </summary>
		private void btnBrowseFootprintDirectory_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
			txtFootprintDirectory.Text = folderBrowserDialog.SelectedPath;
			checkFootprintPath(folderBrowserDialog.SelectedPath);
		}

		/// <summary>
		/// 設定時間區間
		/// </summary>
		private void btnSetTimeInterval_Click(object sender, EventArgs e)
		{
			string tmp1 = $"{cbYear1.Text}/{cbMonth1.Text}/{cbDay1.Text} {cbHour1.Text}:{cbMinute1.Text}:{cbSecond1.Text}";
			string tmp2 = $"{cbYear2.Text}/{cbMonth2.Text}/{cbDay2.Text} {cbHour2.Text}:{cbMinute2.Text}:{cbSecond2.Text}";
			DateTime dateTime1, dateTime2;
			if (DateTime.TryParse(tmp1, out dateTime1) && DateTime.TryParse(tmp2, out dateTime2))
			{

			}
		}

		/// <summary>
		/// Footprint 資料夾關鍵字
		/// </summary>
		private string FOOTPRINT_DIRECTORY_KEYWORD = "Footprint";

		/// <summary>
		/// 確認 Footprint 資料夾的時間區間(年)，並更新介面的 ComboBox
		/// </summary>
		private void checkFootprintPath(string path)
		{
			if (Directory.Exists(path))
			{
				DirectoryInfo baseDirInfo = new DirectoryInfo(path);
				if (baseDirInfo.Name.Contains(FOOTPRINT_DIRECTORY_KEYWORD))
				{
					DirectoryInfo[] dirInfos = baseDirInfo.GetDirectories();
					int yearMin = 0, yearMax = 0;
					foreach (DirectoryInfo dirInfo in dirInfos)
					{
						int num = 0;
						// format: yyMMdd
						if (dirInfo.Name.Length == 6 && int.TryParse(dirInfo.Name, out num))
						{
							int year = int.Parse(dirInfo.Name.Substring(0, 2)) + DateTime.Now.Year / 100 * 100;
							if (yearMin == 0 && yearMax == 0)
							{
								yearMin = yearMax = year;
							}
							else
							{
								yearMin = Math.Min(yearMin, year);
								yearMax = Math.Max(yearMax, year);
							}
						}
					}
					initializeTimeComboBox(yearMin, yearMax);
				}
			}
		}

		/// <summary>
		/// 初始化時間區間的 ComboBox
		/// </summary>
		private void initializeTimeComboBox(int yearMin, int yearMax)
		{
			for (int i = yearMin; i <= yearMax; ++i)
			{
				cbYear1.Items.Add(i.ToString());
				cbYear2.Items.Add(i.ToString());
			}

			string[] obj1 = new string[12];
			for (int i = 0; i < obj1.Count(); ++i)
				obj1[i] = (i + 1).ToString().PadLeft(2, '0');
			string[] obj2 = new string[31];
			for (int i = 0; i < obj2.Count(); ++i)
				obj2[i] = (i + 1).ToString().PadLeft(2, '0');
			string[] obj3 = new string[24];
			for (int i = 0; i < obj3.Count(); ++i)
				obj3[i] = i.ToString().PadLeft(2, '0');
			string[] obj4 = new string[60];
			for (int i = 0; i < obj4.Count(); ++i)
				obj4[i] = i.ToString().PadLeft(2, '0');

			cbMonth1.Items.AddRange(obj1);
			cbMonth2.Items.AddRange(obj1);
			cbDay1.Items.AddRange(obj2);
			cbDay2.Items.AddRange(obj2);
			cbHour1.Items.AddRange(obj3);
			cbHour2.Items.AddRange(obj3);
			cbMinute1.Items.AddRange(obj4);
			cbMinute2.Items.AddRange(obj4);
			cbSecond1.Items.AddRange(obj4);
			cbSecond2.Items.AddRange(obj4);

			if (cbYear1.Items.Count > 0) cbYear1.SelectedIndex = 0;
			if (cbYear2.Items.Count > 0) cbYear2.SelectedIndex = 0;
			if (cbMonth1.Items.Count > 0) cbMonth1.SelectedIndex = 0;
			if (cbMonth2.Items.Count > 0) cbMonth2.SelectedIndex = 0;
			if (cbDay1.Items.Count > 0) cbDay1.SelectedIndex = 0;
			if (cbDay2.Items.Count > 0) cbDay2.SelectedIndex = 0;
			if (cbHour1.Items.Count > 0) cbHour1.SelectedIndex = 0;
			if (cbHour2.Items.Count > 0) cbHour2.SelectedIndex = 0;
			if (cbMinute1.Items.Count > 0) cbMinute1.SelectedIndex = 0;
			if (cbMinute2.Items.Count > 0) cbMinute2.SelectedIndex = 0;
			if (cbSecond1.Items.Count > 0) cbSecond1.SelectedIndex = 0;
			if (cbSecond2.Items.Count > 0) cbSecond2.SelectedIndex = 0;

		}

		#region	gluiCtrl1 事件

		/// <summary>
		/// 載入地圖後觸發
		/// </summary>
		private void gluiCtrl1_LoadMapEvent(object sender, LoadMapEventArgs e)
		{
			txtMapPath.Text = e.MapPath;
		}

		#endregion
	}
}
