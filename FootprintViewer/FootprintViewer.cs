using Geometry;
using GLCore;
using GLStyle;
using GLUI;
using LittleGhost;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
			// gluiCtrl1 圖像設定
			StyleManager.LoadStyle("Style.ini");

			// gluiCtrl1 事件訂閱
			gluiCtrl1.LoadMapEvent += gluiCtrl1_LoadMapEvent;

			// gluiCtrl1 右鍵選單的編輯模式與控制模式關閉
			gluiCtrl1.SetEditMode(false);
			gluiCtrl1.SetControlMode(false);

			// 讀取介面設定
			readSettings(mSETTINGS_FILE);
			readSettings_TSMC(mSETTINGS_FILE);

			// 註冊 Footprint 圖像識別碼
			mFootprintIconID = GLCMD.CMD.AddMultiPair("Footprint", null);

			// 讀取地圖資料
			if (txtMapPath.Text != "" && txtMapPath.Text.EndsWith(".map"))
			{
				gluiCtrl1.LoadMap(txtMapPath.Text);
			}

			// 讀取 Footprint 資料
			if (txtFootprintDirectory.Text != "")
			{
				loadFootprintDirectory(txtFootprintDirectory.Text);
			}

			// 讀取 Inspection Result 資料
			if (txtInspectionResultDirectory.Text != "")
			{
				loadInspectionResultDirectory(txtInspectionResultDirectory.Text);
			}
		}

		private void FootprintViewer_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		#region 商業邏輯

		private const string mSETTINGS_FILE = "FootprintViewer Settings.ini";

		/// <summary>
		/// Footprint 資料夾關鍵字
		/// </summary>
		private const string mFOOTPRINT_DIRECTORY_KEYWORD = "VMLog";

		/// <summary>
		/// Footprint 檔案關鍵字
		/// </summary>
		private const string mFOOTPRINT_FILE_KEYWORD = "Footprint.txt";

		/// <summary>
		/// Footprint 資料
		/// </summary>
		private Footprints mFootprints = new Footprints();

		/// <summary>
		/// Footprint 圖像識別碼
		/// </summary>
		private int mFootprintIconID;

		/// <summary>
		/// 地圖檔路徑
		/// </summary>
		private string mMapFilePath = "";

		/// <summary>
		/// Footprint 資料夾路徑
		/// </summary>
		private string mFootprintDirPath = "";

		/// <summary>
		/// Footprint 資料夾開始日期
		/// </summary>
		private DateTime mFootprintDirDateStart;

		/// <summary>
		/// Footprint 資料夾結束日期
		/// </summary>
		private DateTime mFootprintDirDateEnd;

		/// <summary>
		/// 使用者要讀取的 Footprint 開始日期
		/// </summary>
		private DateTime mFootprintDateStart;

		/// <summary>
		/// 使用者要讀取的 Footprint 結束日期
		/// </summary>
		private DateTime mFootprintDateEnd;

		/// <summary>
		/// 載入地圖
		/// </summary>
		private bool loadMapFile(string path)
		{
			bool result = false;
			if (File.Exists(path) && path.EndsWith(".map"))
			{
				gluiCtrl1.LoadMap(path);
				result = true;
			}
			return result;
		}

		/// <summary>
		/// 讀取 Footprint 資料夾的時間區間(年)，並更新介面的 ComboBox，重置 RobotID 的清單與地圖
		/// </summary>
		/// 若是讀取 \\VMLog ，並計算底下 yyMMdd 的範圍
		/// 若是讀取 \\VMLog\\yyMMdd ，則使用 yyMMdd 當作範圍
		private bool loadFootprintDirectory(string path)
		{
			bool result = false;
			if (Directory.Exists(path))
			{
				DirectoryInfo baseDirInfo = new DirectoryInfo(path);
				// 若是選取 \\VMLog
				if (baseDirInfo.Name.Contains(mFOOTPRINT_DIRECTORY_KEYWORD))
				{
					mFootprintDirPath = path;
					DateTime nonsense;
					IEnumerable<DirectoryInfo> dirInfos = baseDirInfo.GetDirectories().Where(info => info.Name.Length == 6 && DateTime.TryParseExact(info.Name, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out nonsense));
					if (dirInfos.Count() > 0)
					{
						mFootprintDirDateStart = DateTime.ParseExact(dirInfos.First().Name, "yyMMdd", CultureInfo.InvariantCulture);
						mFootprintDirDateEnd = DateTime.ParseExact(dirInfos.Last().Name, "yyMMdd", CultureInfo.InvariantCulture);
						initializeDateComboBoxes(mFootprintDirDateStart, mFootprintDirDateEnd);
						result = true;
					}
				}
				// 若是選取 \\VMLog\\yyMMdd
				else if (baseDirInfo.Parent.Name.Contains(mFOOTPRINT_DIRECTORY_KEYWORD) && baseDirInfo.Name.Length == 6)
				{
					DateTime time;
					if (DateTime.TryParseExact(baseDirInfo.Name, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
					{
						mFootprintDirPath = path;
						mFootprintDirDateStart = time.Date;
						mFootprintDirDateEnd = time.Date;
						initializeDateComboBoxes(mFootprintDirDateStart, mFootprintDirDateEnd);
						result = true;
					}
				}

				// RobotID 清單清空
				if (lbRobotID.Items.Count > 0) lbRobotID.Items.Clear();

				// 地圖 Footprint 清空
				GLCMD.CMD.SaftyEditMultiGeometry<IPair>(mFootprintIconID, true, o => { if (o.Count() > 0) o.Clear(); });
			}
			return result;
		}

		/// <summary>
		/// 讀取 Footprint 資料，於指定路徑 (VMLog) ，讀取其底下介於指定時間區間的資料夾的 Footprint 資料
		/// </summary>
		private void loadFootprintData(DateTime dateStart, DateTime dateEnd)
		{
			// 計算要處理的天數
			int days = dateEnd.DayOfYear - dateStart.DayOfYear;
			days += 1; // 若起始天於結束天相等，至少還是要讀取一天的資料

			// 計算要處理的檔案的路徑
			List<string> filePaths = new List<string>();

			// 若是選取 \\VMLog
			if (mFootprintDirPath.EndsWith(mFOOTPRINT_DIRECTORY_KEYWORD))
			{
				for (int i = 0; i < days; ++i)
				{
					string tmp = mFootprintDirPath + "\\" + dateStart.AddDays(i).ToString("yyMMdd") + "\\" + mFOOTPRINT_FILE_KEYWORD;
					if (File.Exists(tmp)) filePaths.Add(tmp);
				}
			}
			// 若是選取 \\VMLog\\yyMMdd
			else
			{
				string tmp = mFootprintDirPath + "\\" + mFOOTPRINT_FILE_KEYWORD;
				if (File.Exists(tmp)) filePaths.Add(tmp);
			}

			// 從檔案讀取 Footprint 資料
			mFootprints.clear();
			foreach (string filePath in filePaths)
			{
				if (File.Exists(filePath))
				{
					string[] lines = File.ReadAllLines(filePath);
					foreach (string line in lines)
					{
						List<Footprint> data = Footprint.Analyze(line);
						foreach (Footprint fp in data)
						{
							if (dateStart < fp.mTime && dateEnd > fp.mTime)
								mFootprints.add(fp);
						}
					}
				}
			}
		}

		/// <summary>
		/// 從 Footprints 中擷取指定 Footprint 並繪製至 gluiCtrl1 上
		/// </summary>
		private void writeFootprintToMap(string robotID = "")
		{
			string[] robotList = mFootprints.getRobotList();
			GLCMD.CMD.SaftyEditMultiGeometry<IPair>(mFootprintIconID, true, o => { o.Clear(); });
			foreach (string tmpRobotID in robotList)
			{
				if (robotID == "" || tmpRobotID == robotID)
				{
					List<Footprint> tmpFps = mFootprints.getFootprintsOf(tmpRobotID);
					List<IPair> points = new List<IPair>();
					foreach (Footprint fp in tmpFps)
					{
						points.Add(new Pair(fp.mPosition.Position.X, fp.mPosition.Position.Y));
					}
					GLCMD.CMD.SaftyEditMultiGeometry<IPair>(mFootprintIconID, true, o => { o.AddRangeIfNotNull(points); });
				}
			}
		}

		#endregion

		#region 方法

		/// <summary>
		/// 開啟一資料夾選擇視窗，並回傳資料夾路徑
		/// </summary>
		private string getDirectoryPath(string defaultPath = "")
		{
			string result = "";
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (defaultPath != "" && Directory.Exists(defaultPath))
				fbd.SelectedPath = defaultPath;
			else
				fbd.SelectedPath = Application.StartupPath;
			if (fbd.ShowDialog() == DialogResult.OK)
				result = fbd.SelectedPath;
			return result;
		}

		/// <summary>
		/// 開啟一檔案選擇視窗，並回傳檔案路徑
		/// </summary>
		private string getFilePath(string defaultPath = "")
		{
			string result = "";
			OpenFileDialog ofd = new OpenFileDialog();
			if (defaultPath != "" && Directory.Exists(defaultPath))
				ofd.InitialDirectory = defaultPath;
			else
				ofd.InitialDirectory = Application.StartupPath;
			if (ofd.ShowDialog() == DialogResult.OK)
				result = ofd.FileName;
			return result;
		}

		/// <summary>
		/// 讀取介面設定
		/// </summary>
		private void readSettings(string file)
		{
			if (!File.Exists(file)) return;

			string mapPath = "";
			string footprintDirectory = "";

			mapPath = IniFiles.INI.Read(file, "FootprintViewer", "MapPath", mapPath);
			footprintDirectory = IniFiles.INI.Read(file, "FootprintViewer", "FootprintDirectory", footprintDirectory);

			if (mapPath != null) txtMapPath.InvokeIfNecessary(() => { txtMapPath.Text = mapPath; });
			if (footprintDirectory != null) txtFootprintDirectory.InvokeIfNecessary(() => { txtFootprintDirectory.Text = footprintDirectory; });
		}

		/// <summary>
		/// 儲存介面設定
		/// </summary>
		private void writeSettings(string file)
		{
			string mapPath = "";
			string footprintDirectory = "";

			txtMapPath.InvokeIfNecessary(() => { mapPath = txtMapPath.Text; });
			txtFootprintDirectory.InvokeIfNecessary(() => { footprintDirectory = txtFootprintDirectory.Text; });

			if (mapPath != "") IniFiles.INI.Write(file, "FootprintViewer", "MapPath", mapPath);
			if (footprintDirectory != "") IniFiles.INI.Write(file, "FootprintViewer", "FootprintDirectory", footprintDirectory);
		}

		#endregion

		#region GUI 事件

		/// <summary>
		/// 載入地圖
		/// </summary>
		private void btnBrowseMapPath_Click(object sender, EventArgs e)
		{
			string mapPath = "";
			if (txtMapPath.Text != "" && File.Exists(txtMapPath.Text))
				mapPath = getFilePath(Path.GetDirectoryName(txtMapPath.Text));
			else
				mapPath = getFilePath();
			if (mapPath != "" && mapPath.EndsWith(".map"))
			{
				gluiCtrl1.LoadMap(mapPath);
				txtMapPath.Text = mapPath;
			}
		}

		/// <summary>
		/// 載入 Footprint 資料夾
		/// </summary>
		private void btnBrowseFootprintDirectory_Click(object sender, EventArgs e)
		{
			string footprintDir = "";
			if (txtFootprintDirectory.Text != "" && Directory.Exists(txtFootprintDirectory.Text))
				footprintDir = getDirectoryPath(txtFootprintDirectory.Text);
			else
				footprintDir = getDirectoryPath();
			if (footprintDir != "")
			{
				if (loadFootprintDirectory(footprintDir))
				{
					txtFootprintDirectory.Text = footprintDir;
				}
			}
		}

		/// <summary>
		/// 讀取日期 ComboBox
		/// </summary>
		private void btnSetTimeInterval_Click(object sender, EventArgs e)
		{
			string tmp1 = $"{cbYear1.Text}/{cbMonth1.Text}/{cbDay1.Text} {cbHour1.Text}:{cbMinute1.Text}:{cbSecond1.Text}";
			string tmp2 = $"{cbYear2.Text}/{cbMonth2.Text}/{cbDay2.Text} {cbHour2.Text}:{cbMinute2.Text}:{cbSecond2.Text}";
			DateTime dateTime1, dateTime2;
			// 若日期皆為有效日期
			if (DateTime.TryParse(tmp1, out dateTime1) && DateTime.TryParse(tmp2, out dateTime2))
			{
				if (dateTime1 > dateTime2)
				{
					mFootprintDateStart = dateTime2;
					mFootprintDateEnd = dateTime1;
				}
				else
				{
					mFootprintDateStart = dateTime1;
					mFootprintDateEnd = dateTime2;
				}
				// 讀取 Footprint 資料
				loadFootprintData(mFootprintDateStart, mFootprintDateEnd);

				if (mFootprints.getRobotList().Count() == 0)
				{
					MessageBox.Show("No Data", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else
				{
					lbRobotID.Items.Clear();
					lbRobotID.Items.Add("All");
					lbRobotID.Items.AddRange(mFootprints.getRobotList());
					lbRobotID.SelectedIndex = 0;
				}
			}
		}

		/// <summary>
		/// 儲存介面設定
		/// </summary>
		private void btnSaveSettings_Click(object sender, EventArgs e)
		{
			writeSettings(mSETTINGS_FILE);
			writeSettings_TSMC(mSETTINGS_FILE);
		}

		/// <summary>
		/// 重新讀取介面設定
		/// </summary>
		private void btnReloadSettings_Click(object sender, EventArgs e)
		{
			// 讀取地圖資料
			if (txtMapPath.Text != "" && txtMapPath.Text.EndsWith(".map"))
			{
				gluiCtrl1.LoadMap(txtMapPath.Text);
			}

			// 讀取 Footprint 資料
			if (txtFootprintDirectory.Text != "")
			{
				loadFootprintDirectory(txtFootprintDirectory.Text);
			}

			// 讀取 Inspection Result 資料
			if (txtInspectionResultDirectory.Text != "")
			{
				loadInspectionResultDirectory(txtInspectionResultDirectory.Text);
			}
		}

		/// <summary>
		/// 繪製特定 Footprint
		/// </summary>
		private void lbRobotID_SelectedIndexChanged(object sender, EventArgs e)
		{
			string robotID = lbRobotID.SelectedItem.ToString();
			if (robotID == "All")
			{
				writeFootprintToMap("");
			}
			else if (mFootprints.getRobotList().Contains(robotID))
			{
				writeFootprintToMap(robotID);
			}
		}

		#endregion

		#region 介面操作

		/// <summary>
		/// 重新設定日期 ComboBox 項目
		/// </summary>
		private void resetDateComboBoxItem(string keyword, string[] year, string[] month, string[] day, string[] hour, string[] minute, string[] second)
		{
			if (keyword == "1")
			{
				cbYear1.Items.Clear();
				cbMonth1.Items.Clear();
				cbDay1.Items.Clear();
				cbHour1.Items.Clear();
				cbMinute1.Items.Clear();
				cbSecond1.Items.Clear();
				cbYear1.Items.AddRange(year);
				cbMonth1.Items.AddRange(month);
				cbDay1.Items.AddRange(day);
				cbHour1.Items.AddRange(hour);
				cbMinute1.Items.AddRange(minute);
				cbSecond1.Items.AddRange(second);
			}
			else if (keyword == "2")
			{
				cbYear2.Items.Clear();
				cbMonth2.Items.Clear();
				cbDay2.Items.Clear();
				cbHour2.Items.Clear();
				cbMinute2.Items.Clear();
				cbSecond2.Items.Clear();
				cbYear2.Items.AddRange(year);
				cbMonth2.Items.AddRange(month);
				cbDay2.Items.AddRange(day);
				cbHour2.Items.AddRange(hour);
				cbMinute2.Items.AddRange(minute);
				cbSecond2.Items.AddRange(second);
			}
		}

		/// <summary>
		/// 切換日期 ComboBox 選擇項目
		/// </summary>
		private void setDateComboBoxSelectItem(string keyword, DateTime date)
		{
			string year = date.Year.ToString();
			string month = date.Month.ToString().PadLeft(2, '0');
			string day = date.Day.ToString().PadLeft(2, '0');
			string hour = date.Hour.ToString().PadLeft(2, '0');
			string minute = date.Minute.ToString().PadLeft(2, '0');
			string second = date.Second.ToString().PadLeft(2, '0');

			setDateComboBoxSelectItem(keyword, year, month, day, hour, minute, second);
		}

		/// <summary>
		/// 切換日期 ComboBox 選擇項目
		/// </summary>
		private void setDateComboBoxSelectItem(string keyword, string year, string month, string day, string hour, string minute, string second)
		{
			if (keyword == "1")
			{
				if (cbYear1.Items.Contains(year)) cbYear1.SelectedItem = year;
				if (cbMonth1.Items.Contains(month)) cbMonth1.SelectedItem = month;
				if (cbDay1.Items.Contains(day)) cbDay1.SelectedItem = day;
				if (cbHour1.Items.Contains(hour)) cbHour1.SelectedItem = hour;
				if (cbMinute1.Items.Contains(minute)) cbMinute1.SelectedItem = minute;
				if (cbSecond1.Items.Contains(second)) cbSecond1.SelectedItem = second;
			}
			else if (keyword == "2")
			{
				if (cbYear2.Items.Contains(year)) cbYear2.SelectedItem = year;
				if (cbMonth2.Items.Contains(month)) cbMonth2.SelectedItem = month;
				if (cbDay2.Items.Contains(day)) cbDay2.SelectedItem = day;
				if (cbHour2.Items.Contains(hour)) cbHour2.SelectedItem = hour;
				if (cbMinute2.Items.Contains(minute)) cbMinute2.SelectedItem = minute;
				if (cbSecond2.Items.Contains(second)) cbSecond2.SelectedItem = second;
			}
		}

		/// <summary>
		/// 切換日期 ComboBox 選擇索引
		/// </summary>
		private void setDateComboBoxSelectIndex(string keyword, int index1, int index2, int index3, int index4, int index5, int index6)
		{
			if (keyword == "1")
			{
				if (cbYear1.Items.Count > index1) cbYear1.SelectedIndex = index1;
				if (cbMonth1.Items.Count > index2) cbMonth1.SelectedIndex = index2;
				if (cbDay1.Items.Count > index3) cbDay1.SelectedIndex = index3;
				if (cbHour1.Items.Count > index4) cbHour1.SelectedIndex = index4;
				if (cbMinute1.Items.Count > index5) cbMinute1.SelectedIndex = index5;
				if (cbSecond1.Items.Count > index6) cbSecond1.SelectedIndex = index6;
			}
			else if (keyword == "2")
			{
				if (cbYear2.Items.Count > index1) cbYear2.SelectedIndex = index1;
				if (cbMonth2.Items.Count > index2) cbMonth2.SelectedIndex = index2;
				if (cbDay2.Items.Count > index3) cbDay2.SelectedIndex = index3;
				if (cbHour2.Items.Count > index4) cbHour2.SelectedIndex = index4;
				if (cbMinute2.Items.Count > index5) cbMinute2.SelectedIndex = index5;
				if (cbSecond2.Items.Count > index6) cbSecond2.SelectedIndex = index6;
			}
		}

		/// <summary>
		/// 初始化日期 ComboBox
		/// </summary>
		private void initializeDateComboBoxes(DateTime dateStart, DateTime dateEnd)
		{
			List<string> obj1 = new List<string>();
			for (int i = dateStart.Year; i <= dateEnd.Year; ++i)
			{
				obj1.Add(i.ToString());
			}
			string[] obj2 = new string[12];
			for (int i = 0; i < obj2.Count(); ++i)
				obj2[i] = (i + 1).ToString().PadLeft(2, '0');
			string[] obj3 = new string[31];
			for (int i = 0; i < obj3.Count(); ++i)
				obj3[i] = (i + 1).ToString().PadLeft(2, '0');
			string[] obj4 = new string[24];
			for (int i = 0; i < obj4.Count(); ++i)
				obj4[i] = i.ToString().PadLeft(2, '0');
			string[] obj5 = new string[60];
			for (int i = 0; i < obj5.Count(); ++i)
				obj5[i] = i.ToString().PadLeft(2, '0');

			resetDateComboBoxItem("1", obj1.ToArray(), obj2, obj3, obj4, obj5, obj5);
			resetDateComboBoxItem("2", obj1.ToArray(), obj2, obj3, obj4, obj5, obj5);
			
			setDateComboBoxSelectItem("1", mFootprintDirDateStart);
			setDateComboBoxSelectItem("2", mFootprintDirDateEnd.AddDays(1).AddSeconds(-1));
		}

		#endregion

		#region	gluiCtrl1 事件

		/// <summary>
		/// 載入地圖後觸發
		/// </summary>
		private void gluiCtrl1_LoadMapEvent(object sender, LoadMapEventArgs e)
		{
			txtMapPath.InvokeIfNecessary(() => 
			{
				mMapFilePath = txtMapPath.Text = e.MapPath;
			});

			// 重新註冊 Footprint 圖像識別碼
			mFootprintIconID = GLCMD.CMD.AddMultiPair("Footprint", null);
		}

		#endregion

		#region 擴充功能 - 台積巡檢機專案專用

		// 此功能為可透過讀取 Inspection Result ，來將各個巡檢的時間區間輸入於 cmbInspectionResultIntervals 內，
		// 然後透過選擇 cmbInspectionResultIntervals 的項目來快速設定 Time1 與 Time2 。

		/// <summary>
		/// Inspection Result 資料夾關鍵字
		/// </summary>
		private const string mINSPECTION_RESULT_DIRECTORY_KEYWORD = "CIMLog";

		/// <summary>
		/// Inspection Result 檔案關鍵字
		/// </summary>
		private const string mINSPECTION_RESULT_FILE_KEYWORD = "InspectionResult.log";

		/// <summary>
		/// Inspection Result 資料夾路徑，台積巡檢機專案專用
		/// </summary>
		private string mInspectionResultDirPath = "";

		/// <summary>
		/// 讀取設定，台積巡檢機專案專用
		/// </summary>
		/// <param name="filePath"></param>
		private void readSettings_TSMC(string filePath)
		{
			if (!File.Exists(filePath)) return;

			string inspectionResultDirectory = IniFiles.INI.Read(filePath, "TSMC", "InspectionResultDirectory", "");
			if (inspectionResultDirectory != null) txtInspectionResultDirectory.InvokeIfNecessary(() => { txtInspectionResultDirectory.Text = inspectionResultDirectory; });
		}

		/// <summary>
		/// 儲存設定，台積巡檢機專案專用
		/// </summary>
		private void writeSettings_TSMC(string filePath)
		{
			string inspectionResultDirectory = "";
			txtInspectionResultDirectory.InvokeIfNecessary(() => { inspectionResultDirectory = txtInspectionResultDirectory.Text; });
			if (inspectionResultDirectory != "") IniFiles.INI.Write(filePath, "TSMC", "InspectionResultDirectory", inspectionResultDirectory);
		}

		/// <summary>
		/// 讀取 Inspection Result 資料並更新 ComboBox ，台積巡檢機專案專用
		/// </summary>
		private bool loadInspectionResultDirectory(string path)
		{
			bool result = false;
			if (Directory.Exists(path))
			{
				// 計算要處理的檔案路徑
				List<string> filePaths = new List<string>();
				DirectoryInfo tmpDir = new DirectoryInfo(path);
				// 若是選取 \\CIMLog
				if (tmpDir.Name.Contains(mINSPECTION_RESULT_DIRECTORY_KEYWORD))
				{
					mInspectionResultDirPath = path;
					DateTime nonsense;
					IEnumerable<DirectoryInfo> dirInfos = tmpDir.GetDirectories().Where(info => info.Name.Length == 8 && DateTime.TryParseExact(info.Name, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out nonsense));
					if (dirInfos.Count() > 0)
					{
						foreach (DirectoryInfo dirInfo in dirInfos)
						{
							string tmpFilePath = dirInfo.FullName + "\\" + mINSPECTION_RESULT_FILE_KEYWORD;
							if (File.Exists(tmpFilePath)) filePaths.Add(tmpFilePath);
						}
					}
				}
				// 若是選取 \\CIMLog\\yyyyMMdd
				else if (tmpDir.Parent.Name.Contains(mINSPECTION_RESULT_DIRECTORY_KEYWORD) && tmpDir.Name.Length == 8)
				{
					DateTime nonsense;
					if (DateTime.TryParseExact(tmpDir.Name, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out nonsense))
					{
						string tmpFilePath = tmpDir.FullName + "\\" + mINSPECTION_RESULT_FILE_KEYWORD;
						if (File.Exists(tmpFilePath)) filePaths.Add(tmpFilePath);
					}
				}

				// 分析 Inspection Result
				List<string> inspectionIntervals = new List<string>();
				foreach (var filePath in filePaths)
				{
					inspectionIntervals.AddRange(analyzeInspectionResultFile(filePath));
				}

				// 將分析完的資料顯示於介面
				if (inspectionIntervals.Count > 0)
				{
					cmbInspectionResultIntervals.InvokeIfNecessary(() =>
					{
						cmbInspectionResultIntervals.Items.Clear();
						cmbInspectionResultIntervals.Items.AddRange(inspectionIntervals.ToArray());
					});
					result = true;
				}
			}
			return result;
		}

		/// <summary>
		/// 讀取 InspectionResult.log 裡每一趟 Inspection 的開始、結束時間，再將其組合成 "yyyy/MM/dd HH:mm:ss - yyyy/MM/dd HH:mm:ss - ... - ..." 的字串
		/// </summary>
		private List<string> analyzeInspectionResultFile(string filePath)
		{
			List<string> result = new List<string>();
			if (File.Exists(filePath) && filePath.EndsWith(mINSPECTION_RESULT_FILE_KEYWORD))
			{
				string[] tmpData = File.ReadAllLines(filePath);
				DirectoryInfo tmpDirInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));
				DateTime tmpTime = DateTime.ParseExact(tmpDirInfo.Name, "yyyyMMdd", CultureInfo.InvariantCulture);
				for (int i = 0; i < tmpData.Count(); ++i)
				{
					if (tmpData[i].StartsWith(tmpTime.ToString("yyyy/MM/dd")) && tmpData[i].Contains("[InspectionResult]"))
					{
						DateTime inspectionStartTime = DateTime.ParseExact(tmpData[i + 1].Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1], "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
						DateTime inspectionEndTime = DateTime.ParseExact(tmpData[i + 2].Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1], "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
						result.Add(inspectionStartTime.ToString("yyyy/MM/dd HH:mm:ss") + " - " + inspectionEndTime.ToString("yyyy/MM/dd HH:mm:ss") + " - " + tmpData[i + 3] + " - " + tmpData[i + 4]);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 載入 Inspection Result 資料夾，台積巡檢機專案專用
		/// </summary>
		private void btnBrowseInsepctionResultDirectory_Click(object sender, EventArgs e)
		{
			string inspectionResultDir = "";
			if (txtInspectionResultDirectory.Text != "" && Directory.Exists(txtInspectionResultDirectory.Text))
				inspectionResultDir = getDirectoryPath(txtInspectionResultDirectory.Text);
			else
				inspectionResultDir = getDirectoryPath();
			if (inspectionResultDir != "")
			{
				if (loadInspectionResultDirectory(inspectionResultDir))
				{
					txtInspectionResultDirectory.Text = inspectionResultDir;
				}
			}
		}

		/// <summary>
		/// 根據 ComboBox 選擇的項目來切換日期的 ComboBox ，台積巡檢機專案專用
		/// </summary>
		private void cmbInspectionResultIntervals_SelectedIndexChanged(object sender, EventArgs e)
		{
			string content = cmbInspectionResultIntervals.SelectedItem.ToString();
			string[] tmp = content.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
			if (tmp.Count() >= 2)
			{
				DateTime time1 = DateTime.ParseExact(tmp[0], "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
				DateTime time2 = DateTime.ParseExact(tmp[1], "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
				setDateComboBoxSelectItem("1", time1);
				setDateComboBoxSelectItem("2", time2);
			}
		}

		#endregion
	}

	/// <summary>
	/// Footprint 資料
	/// </summary>
	public class Footprint
	{
		/// <summary>
		/// Footprint 時間點
		/// </summary>
		public DateTime mTime;

		/// <summary>
		/// Footprint 機器人 ID
		/// </summary>
		public string mRobotID;

		/// <summary>
		/// Footprint 位置
		/// </summary>
		public TowardPair mPosition;

		public Footprint(DateTime time, string robotID, double x, double y, double toward)
		{
			this.mTime = time;
			this.mRobotID = robotID;
			mPosition = new TowardPair(x, y, toward);
		}

		public static List<Footprint> Analyze(string src)
		{
			//Format: [2018/09/02 00:00:54.960] [iTS-TSMC,-30016.42,-8586.81,332.36][iTS-300B,-26198.43,-8654.82,124.67]
			List<Footprint> fps = new List<Footprint>();
			string[] datas = src.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
			if (datas.Count() > 1)
			{
				DateTime tmpTime = DateTime.ParseExact(datas[0], "yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
				for (int i = 2; i < datas.Count(); ++i)
				{
					string[] tmp = datas[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
					if (tmp.Count() == 4)
					{
						fps.Add(new Footprint(tmpTime, tmp[0], double.Parse(tmp[1]), double.Parse(tmp[2]), double.Parse(tmp[3])));
					}
				}
			}
			return fps;
		}
	}

	public class Footprints
	{
		/// <summary>
		/// 最後更新時間
		/// </summary>
		public DateTime mLastUpdateTime = DateTime.Now;

		/// <summary>
		/// 所有機器人的 Footprint 儲存區
		/// </summary>
		private Dictionary<string, List<Footprint>> mFootprints = new Dictionary<string, List<Footprint>>();

		/// <summary>
		/// 執行緒鎖
		/// </summary>
		private readonly object mO = new object();

		/// <summary>
		/// 加入 Footprint
		/// </summary>
		public void add(Footprint fp)
		{
			lock (mO)
			{
				if (mFootprints.Keys.Contains(fp.mRobotID))
				{
					mFootprints[fp.mRobotID].Add(fp);
				}
				else
				{
					mFootprints.Add(fp.mRobotID, new List<Footprint> { fp });
				}
			}
			mLastUpdateTime = DateTime.Now;
		}

		/// <summary>
		/// 加入 Footprint 集合
		/// </summary>
		public void addRange(List<Footprint> fps)
		{
			foreach (Footprint fp in fps)
			{
				add(fp);
			}
		}

		/// <summary>
		/// 目前儲存的 Footprint 的機器人清單
		/// </summary>
		public string[] getRobotList()
		{
			return mFootprints.Keys.ToArray();
		}

		/// <summary>
		///  取得特定機器人的 Footprint 清單
		/// </summary>
		public List<Footprint> getFootprintsOf(string robotID)
		{
			lock (mO)
			{
				if (mFootprints.Keys.Contains(robotID))
				{
					return mFootprints[robotID];
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// 清除所有資料
		/// </summary>
		public void clear()
		{
			lock (mO)
			{
				mFootprints.Clear();
			}
			mLastUpdateTime = DateTime.Now;
		}
	}
}
