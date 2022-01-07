using Geometry;
using GLCore;
using GLStyle;
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
using TrafficControlTest.Library;

namespace FootprintViewer
{
	public partial class Form1 : Form
	{
		private DateTime mStartTimestamp { get; set; } = DateTime.MinValue;
		private DateTime mEndTimestamp { get; set; } = DateTime.MinValue;
		private DateTime mCurrentTimestamp { get; set; } = DateTime.MinValue;
		private Dictionary<string, List<HistoryVehicleInfo>> mHistoryVehicleInfos { get; set; } = new Dictionary<string, List<HistoryVehicleInfo>>();
		private DatabaseAdapter mDatabaseAdapter = new SqliteDatabaseAdapter(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false);

		private Dictionary<string, int> mIconIdsOfVehicle = new Dictionary<string, int>();
		private Dictionary<string, int> mIconIdsOfVehiclePath = new Dictionary<string, int>();
		private Dictionary<string, int> mIconIdsOfVehicleLaser = new Dictionary<string, int>();

		public Form1()
		{
			InitializeComponent();

			gluiCtrl1.SetControlMode(true);
			gluiCtrl1.SetEditMode(true);
			gluiCtrl1.ShowObjectText = false;
			StyleManager.LoadStyle("Style.ini");
			GLCMD.CMD.Initial();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				InitializeTimeComboBox(cbStart);
				InitializeTimeComboBox(cbEnd);

				//ReadData("D:\\做完就刪\\20220102 2F Wifi 訊號測試\\CASTEC_Log_VM_20220105\\Database\\Event.db", DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-6));
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void btnSelectMapFile_Click(object sender, EventArgs e)
		{
			try
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title = "Choose Map File";
					ofd.Filter = "All Files (*.*)|*.*";
					ofd.InitialDirectory = Application.StartupPath;
					ofd.Multiselect = false;
					if (ofd.ShowDialog() == DialogResult.OK)
					{
						txtMapFile.Text = ofd.FileName;
					}
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void btnSelectLogFile_Click(object sender, EventArgs e)
		{
			try
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title = "Choose Log File";
					ofd.Filter = "All Files (*.*)|*.*";
					ofd.InitialDirectory = Application.StartupPath;
					ofd.Multiselect = false;
					if (ofd.ShowDialog() == DialogResult.OK)
					{
						txtLogFile.Text = ofd.FileName;
					}
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void btnLoadSetting_Click(object sender, EventArgs e)
		{
			try
			{
				string mapFile = txtMapFile.Text;
				string logFile = txtLogFile.Text;
				DateTime startTimestamp = dtpStart.Value.Date;
				DateTime endTimestamp = dtpEnd.Value.Date;
				startTimestamp = startTimestamp.AddHours(int.Parse(cbStart.SelectedItem.ToString().Substring(0, 2)));
				endTimestamp = endTimestamp.AddHours(int.Parse(cbEnd.SelectedItem.ToString().Substring(0, 2)));

				string tmp = string.Empty;
				tmp += $"[Load Setting]";
				tmp += $" ";
				tmp += $"MapFile:{mapFile}";
				tmp += $" ";
				tmp += $"LogFile:{logFile}";
				tmp += $" ";
				tmp += $"Start:{startTimestamp.ToString("yyyy/MM/dd HH:mm:ss")}";
				tmp += $" ";
				tmp += $"End:{endTimestamp.ToString("yyyy/MM/dd HH:mm:ss")}";
				RecordLogMessage(rtxtLog, tmp);

				InitializeFootprintData(mapFile, logFile, startTimestamp, endTimestamp);
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void tbTimestamp_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (mHistoryVehicleInfos.Count == 0) return;

				mCurrentTimestamp = mStartTimestamp.AddSeconds(mEndTimestamp.Subtract(mStartTimestamp).TotalSeconds / (tbTimestamp.Maximum - tbTimestamp.Minimum) * tbTimestamp.Value);
				lblCurrentTimestamp.Text = mCurrentTimestamp.ToString("yyyy/MM/dd HH:mm:ss");
				RefreshMap();
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}

		private void InitializeTimeComboBox(ComboBox ComboBox)
		{
			ComboBox.Items.Clear();
			ComboBox.Items.Add("00:00");
			ComboBox.Items.Add("01:00");
			ComboBox.Items.Add("02:00");
			ComboBox.Items.Add("03:00");
			ComboBox.Items.Add("04:00");
			ComboBox.Items.Add("05:00");
			ComboBox.Items.Add("06:00");
			ComboBox.Items.Add("07:00");
			ComboBox.Items.Add("08:00");
			ComboBox.Items.Add("09:00");
			ComboBox.Items.Add("10:00");
			ComboBox.Items.Add("11:00");
			ComboBox.Items.Add("12:00");
			ComboBox.Items.Add("13:00");
			ComboBox.Items.Add("14:00");
			ComboBox.Items.Add("15:00");
			ComboBox.Items.Add("16:00");
			ComboBox.Items.Add("17:00");
			ComboBox.Items.Add("18:00");
			ComboBox.Items.Add("19:00");
			ComboBox.Items.Add("20:00");
			ComboBox.Items.Add("21:00");
			ComboBox.Items.Add("22:00");
			ComboBox.Items.Add("23:00");
			ComboBox.SelectedIndex = 0;
		}
		private void InitializeFootprintData(string MapFilePath, string LogFilePath, DateTime StartTimestamp, DateTime EndTimestamp)
		{
			// 初始化設定
			mStartTimestamp = StartTimestamp;
			mEndTimestamp = EndTimestamp;
			mCurrentTimestamp = StartTimestamp;
			tbTimestamp.Value = 0;
			foreach (var pair in mHistoryVehicleInfos)
			{
				EraseVehicleIcon(pair.Key);
			}
			mHistoryVehicleInfos.Clear();

			LoadMap(MapFilePath);
			ReadData(LogFilePath, StartTimestamp, EndTimestamp);
			
			foreach (var pair in mHistoryVehicleInfos)
			{
				RegisterVehicleIconId(pair.Key);
			}

			RefreshMap();
		}
		private void LoadMap(string mapFilePath)
		{
			// 地圖介面讀取地圖
			gluiCtrl1.LoadMap(mapFilePath);
			gluiCtrl1.AdjustZoom();
			gluiCtrl1.Focus(GLCMD.CMD.MapCenter.X, GLCMD.CMD.MapCenter.Y);
		}
		private void ReadData(string LogFilePath, DateTime StartTimestamp, DateTime EndTimestamp)
		{
			if (!File.Exists(LogFilePath)) return;

			// Database 設定與連線
			mDatabaseAdapter.SetDatabaseParameters(LogFilePath, string.Empty, string.Empty, string.Empty, string.Empty);
			if (mDatabaseAdapter.Connect() == true)
			{
				// 取得自走車表格名字
				string sqlCmd1 = "SELECT name FROM sqlite_master WHERE type='table' AND name LIKE 'HistoryVehicleInfoOf%' ORDER BY UPPER(name)";
				string[] vehicleTableNames = GetVehicleTableNames(mDatabaseAdapter.ExecuteQueryCommand(sqlCmd1));
				string startTimestamp = $"{StartTimestamp.ToString("yyyy-MM-dd HH:mm:ss")}";
				string endTimestamp = $"{EndTimestamp.ToString("yyyy-MM-dd HH:mm:ss")}";

				// 取得每一台自走車的歷史資訊
				for (int i = 0; i < vehicleTableNames.Length; ++i)
				{
					string sqlCmd2 = $"SELECT RecordTimestamp,ID,State,X,Y,Toward,Target,BatteryValue,LocationScore,Path FROM {vehicleTableNames[i]} WHERE RecordTimestamp BETWEEN '{startTimestamp}' AND '{endTimestamp}'";
					string[] historyVehicleInfoStrings = GetHistoryVehicleInfoStrings(mDatabaseAdapter.ExecuteQueryCommand(sqlCmd2));
					List<HistoryVehicleInfo> historyVehicleInfos = historyVehicleInfoStrings.Select(o => GetHistoryVehicleInfo(o)).ToList();
					string vehicleName = GetVehicleName(vehicleTableNames[i]);
					mHistoryVehicleInfos.Add(vehicleName, historyVehicleInfos);
				}
			}
		}
		private void RefreshMap()
		{
			// 根據 CurrentTimestamp 更新地圖上的自走車位置
			lblCurrentTimestamp.Text = mCurrentTimestamp.ToString("yyyy/MM/dd HH:mm:ss");

			foreach (var a in mHistoryVehicleInfos)
			{
				for (int i = 0; i < a.Value.Count; ++i)
				{
					if (a.Value[i].Timestamp > mCurrentTimestamp)
					{
						PrintVehicleIcon(a.Key, a.Value[i]);
						break;
					}
				}
			}
		}
		public void RegisterVehicleIconId(string VehicleName)
		{
			// 註冊圖像 ID
			if (!string.IsNullOrEmpty(VehicleName) && !mIconIdsOfVehicle.ContainsKey(VehicleName))
			{
				int VehicleIconId = GLCMD.CMD.SerialNumber.Next();
				int VehiclePathIconId = GLCMD.CMD.AddMultiStripLine("Path", null);
				int VehiclePathPointsIconId = GLCMD.CMD.AddMultiPair("Laser", null);

				mIconIdsOfVehicle.Add(VehicleName, VehicleIconId);
				mIconIdsOfVehiclePath.Add(VehicleName, VehiclePathIconId);
				mIconIdsOfVehicleLaser.Add(VehicleName, VehiclePathPointsIconId);
			}
		}
		public void PrintVehicleIcon(string VehicleName, HistoryVehicleInfo HistoryVehicleInfo)
		{
			// 把圖像加入至地圖
			if (!string.IsNullOrEmpty(VehicleName) && mIconIdsOfVehicle.ContainsKey(VehicleName))
			{
				if (HistoryVehicleInfo != null)
				{
					GLCMD.CMD.AddAGV(mIconIdsOfVehicle[VehicleName], VehicleName, HistoryVehicleInfo.X, HistoryVehicleInfo.Y, HistoryVehicleInfo.Toward);
				}
				if (HistoryVehicleInfo != null && !string.IsNullOrEmpty(HistoryVehicleInfo.Path))
				{
					GLCMD.CMD.SaftyEditMultiGeometry<IPair>(mIconIdsOfVehiclePath[VehicleName], true, (line) =>
					{
						line.Clear();
						line.AddRangeIfNotNull(HistoryVehicleInfo.Path2);
					});
				}
				if (HistoryVehicleInfo != null && !string.IsNullOrEmpty(HistoryVehicleInfo.Laser))
				{
					// do nothing
				}
			}
		}
		public void EraseVehicleIcon(string VehicleName)
		{
			// 把圖像從地圖中移除
			if (!string.IsNullOrEmpty(VehicleName) && mIconIdsOfVehicle.ContainsKey(VehicleName))
			{
				GLCMD.CMD.DeleteAGV(mIconIdsOfVehicle[VehicleName]);
				GLCMD.CMD.DeleteMulti(mIconIdsOfVehiclePath[VehicleName]);
				GLCMD.CMD.DeleteMulti(mIconIdsOfVehicleLaser[VehicleName]);

				mIconIdsOfVehicle.Remove(VehicleName);
				mIconIdsOfVehiclePath.Remove(VehicleName);
				mIconIdsOfVehicleLaser.Remove(VehicleName);
			}
		}

		private static void RecordLogMessage(RichTextBox RichTextBox, string Text)
		{
			RichTextBox.Text += $"[{DateTime.Now.ToString("HH:mm:ss.fff")}] ";
			RichTextBox.Text += Text + "\r\n";
			RichTextBox.ScrollToCaret();
		}
		private static void ClearLogMessage(RichTextBox RichTextBox)
		{
			RichTextBox.Clear();
		}
		private static string[] GetVehicleTableNames(DataSet DataSet)
		{
			List<string> result = new List<string>();
			if (DataSet != null && DataSet.Tables != null && DataSet.Tables.Count > 0)
			{
				DataTable dataTable = DataSet.Tables[0];
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; ++i)
					{
						result.Add(dataTable.Rows[i].ItemArray.First().ToString());
					}
				}
			}
			return result.ToArray();
		}
		private static string GetVehicleName(string VehicleTableName)
		{
			return VehicleTableName.Replace("HistoryVehicleInfoOf", string.Empty).Replace("Dash", "-");
		}
		private static string[] GetHistoryVehicleInfoStrings(DataSet DataSet)
		{
			List<string> result = new List<string>();
			if (DataSet != null && DataSet.Tables != null && DataSet.Tables.Count > 0)
			{
				DataTable dataTable = DataSet.Tables[0];
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; ++i)
					{
						result.Add(string.Join("#", dataTable.Rows[i].ItemArray.Select(o => o.ToString())));
					}
				}
			}
			return result.ToArray();
		}
		private static HistoryVehicleInfo GetHistoryVehicleInfo(string HistoryVehicleInfoString)
		{
			string[] data = HistoryVehicleInfoString.Split(new string[] { "#" }, StringSplitOptions.None);
			return new HistoryVehicleInfo(DateTime.Parse(data[0]), data[1], data[2], int.Parse(data[3]), int.Parse(data[4]), (int)double.Parse(data[5]), data[6], (int)double.Parse(data[7]), (int)double.Parse(data[8]), data[9], string.Empty);
		}
	}

	public class HistoryVehicleInfo
	{
		public DateTime Timestamp { get; private set; } = DateTime.MinValue;
		public string Name { get; private set; } = string.Empty;
		public string State { get; private set; } = string.Empty;
		public int X { get; private set; } = 0;
		public int Y { get; private set; } = 0;
		public int Toward { get; private set; } = 0;
		public string Target { get; private set; } = string.Empty;
		public int Battery { get; private set; } = 0;
		public int Score { get; private set; } = 0;
		public string Path { get; private set; } = string.Empty;
		public List<IPair> Path2
		{
			get
			{
				Console.WriteLine(Path);
				List<IPair> result = new List<IPair>();
				if (!string.IsNullOrEmpty(Path))
				{
					string[] tmp = Path.Split(new string[] { "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < tmp.Length; i += 2)
					{
						result.Add(new Pair(int.Parse(tmp[i]), int.Parse(tmp[i + 1])));
					}
				}
				return result;
			}
		}
		public string Laser { get; private set; } = string.Empty;

		public HistoryVehicleInfo(DateTime Timestamp, string Name, string State, int X, int Y, int Toward, string Target, int Battery, int Score, string Path, string Laser)
		{
			Set(Timestamp, Name, State, X, Y, Toward, Target, Battery, Score, Path, Laser);
		}
		public void Set(DateTime Timestamp, string Name, string State, int X, int Y, int Toward, string Target, int Battery, int Score, string Path, string Laser)
		{
			this.Timestamp = Timestamp;
			this.Name = Name;
			this.State = State;
			this.X = X;
			this.Y = Y;
			this.Toward = Toward;
			this.Target = Target;
			this.Battery = Battery;
			this.Score = Score;
			this.Path = Path;
			this.Laser = Laser;
		}
		public override string ToString()
		{
			return $"{Name}/{State}/{Target}";
		}
	}
}
