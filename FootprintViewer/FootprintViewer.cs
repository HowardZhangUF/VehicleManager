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
			readSettings(SETTINGS_FILE);

			// 註冊 Footprint 圖像識別碼
			footprintIconID = GLCMD.CMD.AddMultiPair("Footprint", null);

			// 讀取地圖
			if (txtMapPath.Text != "" && txtMapPath.Text.EndsWith(".map"))
			{
				gluiCtrl1.LoadMap(txtMapPath.Text);
			}

			// 讀取 Footprint 資料
			if (txtFootprintDirectory.Text != "")
			{
				loadFootprintDirectory(txtFootprintDirectory.Text);
			}
		}

		private void FootprintViewer_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		#region 商業邏輯

		private const string SETTINGS_FILE = "FootprintViewer Settings.ini";

		/// <summary>
		/// Footprint 資料夾關鍵字
		/// </summary>
		private const string FOOTPRINT_DIRECTORY_KEYWORD = "VMLog";

		/// <summary>
		/// Footprint 檔案關鍵字
		/// </summary>
		private const string FOOTPRINT_FILE_KEYWORD = "Footprint.txt";

		/// <summary>
		/// Footprint 資料
		/// </summary>
		private Footprints footprints = new Footprints();

		/// <summary>
		/// Footprint 圖像識別碼
		/// </summary>
		private int footprintIconID;

		/// <summary>
		/// 地圖檔路徑
		/// </summary>
		private string mapFilePath = "";

		/// <summary>
		/// Footprint 資料夾路徑
		/// </summary>
		private string footprintDirPath = "";

		/// <summary>
		/// Footprint 資料夾開始日期
		/// </summary>
		private DateTime footprintDirDateStart;

		/// <summary>
		/// Footprint 資料夾結束日期
		/// </summary>
		private DateTime footprintDirDateEnd;

		/// <summary>
		/// 使用者要讀取的 Footprint 開始日期
		/// </summary>
		private DateTime footprintDateStart;

		/// <summary>
		/// 使用者要讀取的 Footprint 結束日期
		/// </summary>
		private DateTime footprintDateEnd;

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
		/// 讀取 Footprint 資料夾的時間區間(年)，並更新介面的 ComboBox
		/// </summary>
		private bool loadFootprintDirectory(string path)
		{
			bool result = false;
			if (Directory.Exists(path))
			{
				DirectoryInfo baseDirInfo = new DirectoryInfo(path);
				if (baseDirInfo.Name.Contains(FOOTPRINT_DIRECTORY_KEYWORD))
				{
					footprintDirPath = path;
					DateTime nonsense;
					IEnumerable<DirectoryInfo> dirInfos = baseDirInfo.GetDirectories().Where(info => info.Name.Length == 6 && DateTime.TryParseExact(info.Name, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out nonsense));
					if (dirInfos.Count() > 0)
					{
						footprintDirDateStart = DateTime.ParseExact(dirInfos.First().Name, "yyMMdd", CultureInfo.InvariantCulture);
						footprintDirDateEnd = DateTime.ParseExact(dirInfos.Last().Name, "yyMMdd", CultureInfo.InvariantCulture);
						initializeDateComboBoxes(footprintDirDateStart, footprintDirDateEnd);
						result = true;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 讀取 Footprint 資料
		/// </summary>
		private void loadFootprintData(DateTime dateStart, DateTime dateEnd)
		{
			// 計算要處理的天數
			int days = dateEnd.DayOfYear - dateStart.DayOfYear;
			days += 1; // 若起始天於結束天相等，至少還是要讀取一天的資料

			// 計算要處理的檔案的路徑
			List<string> filePaths = new List<string>();
			for (int i = 0; i < days; ++i)
			{
				string tmp = footprintDirPath + "\\" + dateStart.AddDays(i).ToString("yyMMdd") + "\\" + FOOTPRINT_FILE_KEYWORD;
				filePaths.Add(tmp);
			}

			// 從檔案讀取資料
			footprints.clear();
			foreach (string filePath in filePaths)
			{
				if (File.Exists(filePath))
				{
					string[] lines = File.ReadAllLines(filePath);
					foreach (string line in lines)
					{
						footprints.addRange(Footprint.Analyze(line));
					}
				}
			}
		}

		/// <summary>
		/// 將 Footprint 繪製至 gluiCtrl1 上
		/// </summary>
		private void writeFootprintToMap()
		{
			string[] robotList = footprints.getRobotList();
			foreach (string robotID in robotList)
			{
				List<Footprint> tmpFps = footprints.getFootprintsOf(robotID);
				List<IPair> points = new List<IPair>();
				foreach (Footprint fp in tmpFps)
				{
					points.Add(new Pair(fp.position.Position.X, fp.position.Position.Y));
				}
				GLCMD.CMD.SaftyEditMultiGeometry<IPair>(footprintIconID, true, o => o.AddRangeIfNotNull(points));
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
			string inspectionResultDirectory = "";

			mapPath = IniFiles.INI.Read(file, "FootprintViewer", "MapPath", mapPath);
			footprintDirectory = IniFiles.INI.Read(file, "FootprintViewer", "FootprintDirectory", footprintDirectory);
			inspectionResultDirectory = IniFiles.INI.Read(file, "FootprintViewer", "InspectionResultDirectory", inspectionResultDirectory);

			if (mapPath != null) txtMapPath.InvokeIfNecessary(() => { txtMapPath.Text = mapPath; });
			if (footprintDirectory != null) txtFootprintDirectory.InvokeIfNecessary(() => { txtFootprintDirectory.Text = footprintDirectory; });
			if (inspectionResultDirectory != null) txtInspectionResultDirectory.InvokeIfNecessary(() => { txtInspectionResultDirectory.Text = inspectionResultDirectory; });
		}

		/// <summary>
		/// 儲存介面設定
		/// </summary>
		private void writeSettings(string file)
		{
			string mapPath = "";
			string footprintDirectory = "";
			string inspectionResultDirectory = "";

			txtMapPath.InvokeIfNecessary(() => { mapPath = txtMapPath.Text; });
			txtFootprintDirectory.InvokeIfNecessary(() => { footprintDirectory = txtFootprintDirectory.Text; });
			txtInspectionResultDirectory.InvokeIfNecessary(() => { inspectionResultDirectory = txtInspectionResultDirectory.Text; });

			if (mapPath != "") IniFiles.INI.Write(file, "FootprintViewer", "MapPath", mapPath);
			if (footprintDirectory != "") IniFiles.INI.Write(file, "FootprintViewer", "FootprintDirectory", footprintDirectory);
			if (inspectionResultDirectory != "") IniFiles.INI.Write(file, "FootprintViewer", "InspectionResultDirectory", inspectionResultDirectory);
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
					footprintDateStart = dateTime2;
					footprintDateEnd = dateTime1;
				}
				else
				{
					footprintDateStart = dateTime1;
					footprintDateEnd = dateTime2;
				}
				// 讀取 Footprint 資料
				loadFootprintData(footprintDateStart, footprintDateEnd);
				writeFootprintToMap();
			}
		}

		/// <summary>
		/// 儲存介面設定
		/// </summary>
		private void btnSaveSettings_Click(object sender, EventArgs e)
		{
			writeSettings(SETTINGS_FILE);
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

			setDateComboBoxSelectItem("1", footprintDirDateStart.Year.ToString(), footprintDirDateStart.Month.ToString().PadLeft(2, '0'), footprintDirDateStart.Day.ToString().PadLeft(2, '0'), "00", "00", "00");
			setDateComboBoxSelectItem("2", footprintDirDateEnd.Year.ToString(), footprintDirDateEnd.Month.ToString().PadLeft(2, '0'), footprintDirDateEnd.Day.ToString().PadLeft(2, '0'), "23", "59", "59");
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
				mapFilePath = txtMapPath.Text = e.MapPath;
			});

			// 重新註冊 Footprint 圖像識別碼
			footprintIconID = GLCMD.CMD.AddMultiPair("Footprint", null);
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
		public DateTime time;

		/// <summary>
		/// Footprint 機器人 ID
		/// </summary>
		public string robotID;

		/// <summary>
		/// Footprint 位置
		/// </summary>
		public TowardPair position;

		public Footprint(DateTime time, string robotID, double x, double y, double toward)
		{
			this.time = time;
			this.robotID = robotID;
			position = new TowardPair(x, y, toward);
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
		public DateTime lastUpdateTime = DateTime.Now;

		/// <summary>
		/// 所有機器人的 Footprint 儲存區
		/// </summary>
		private Dictionary<string, List<Footprint>> footprints = new Dictionary<string, List<Footprint>>();

		/// <summary>
		/// 執行緒鎖
		/// </summary>
		private readonly object o = new object();

		/// <summary>
		/// 加入 Footprint
		/// </summary>
		public void add(Footprint fp)
		{
			lock (o)
			{
				if (footprints.Keys.Contains(fp.robotID))
				{
					footprints[fp.robotID].Add(fp);
				}
				else
				{
					footprints.Add(fp.robotID, new List<Footprint> { fp });
				}
			}
			lastUpdateTime = DateTime.Now;
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
			return footprints.Keys.ToArray();
		}

		/// <summary>
		///  取得特定機器人的 Footprint 清單
		/// </summary>
		public List<Footprint> getFootprintsOf(string robotID)
		{
			lock (o)
			{
				if (footprints.Keys.Contains(robotID))
				{
					return footprints[robotID];
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
			lock (o)
			{
				footprints.Clear();
			}
			lastUpdateTime = DateTime.Now;
		}
	}
}
