using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TrafficControlTest.Module.Dashboard;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.InterveneCommand;

namespace TrafficControlTest.UserControl
{
	public enum EndDate
	{
		Today,
		Yesterday
	}

	public partial class UcDashboard : System.Windows.Forms.UserControl
	{
		public DateTime mLastUpdateTimestamp { get; private set; } = default(DateTime);
		public DateTime mEndDate { get; private set; } = default(DateTime);

		private DatabaseAdapter rDatabaseAdapter = null;

		public UcDashboard()
		{
			InitializeComponent();
		}
		public void Set(DatabaseAdapter DatabaseAdapter)
		{
			rDatabaseAdapter = DatabaseAdapter;
			UpdateDashboard(EndDate.Yesterday);
		}

		private void UpdateDashboard(EndDate EndDate)
		{
			switch (EndDate)
			{
				case EndDate.Today:
					UpdateDashboard(DateTime.Now.Date);
					break;
				case EndDate.Yesterday:
				default:
					UpdateDashboard(DateTime.Now.AddDays(-1).Date);
					break;
			}
		}
		private void UpdateDashboard(DateTime EndDate)
		{
			if (rDatabaseAdapter == null) return;

			mEndDate = EndDate;
			mLastUpdateTimestamp = DateTime.Now;
			UpdateDashboardViaDatabase_DailyMissionCount();
			UpdateDashboardViaDatabase_DailyMissionAverageCost();
			UpdateDashboardViaDatabase_DailyVehicleBatteryState();
			UpdateDashboardViaDatabase_WeeklyMissionCount();
			UpdateDashboardViaDatabase_WeeklyVehicleUsage();
		}
		private void cmenuItemUpdateDashboardToday_Click(object sender, EventArgs e)
		{
			UpdateDashboard(EndDate.Today);
		}
		private void cmenuItemUpdateDashboardYesterday_Click(object sender, EventArgs e)
		{
			UpdateDashboard(EndDate.Yesterday);
		}

		#region DailyMissionCount
		private void UpdateDashboardViaSampleData_DailyMissionCount()
		{
			DailyMissionCount tmp = new DailyMissionCount(mEndDate.AddDays(-1), 50, 14);
			UpdateDashboard_DailyMissionCount(tmp);
		}
		private void UpdateDashboardViaDatabase_DailyMissionCount()
		{
			// Command:
			//		SELECT ExecuteState, Count(*) FROM MissionState WHERE (ExecuteState == 'ExecuteSuccessed' OR ExecuteState == 'ExecuteFailed') AND (ReceiveTimestamp BETWEEN DATE('now', '-1 days') AND DATE('now', '-0 days')) GROUP BY ExecuteState
			// Result:
			//		ExecuteSuccessed 30342
			// Commnet:
			//		ExecuteFailed Count is 0
			//		Note that the BETWEEN operator is inclusive. https://www.sqlitetutorial.net/sqlite-between/
			DailyMissionCount dataFromDatabase = new DailyMissionCount(mEndDate, 0, 0);
			string startTimestamp = $"{dataFromDatabase.mDate.ToString("yyyy-MM-dd")} 00:00:00.000";
			string endTimestamp = $"{dataFromDatabase.mDate.ToString("yyyy-MM-dd")} 23:59:59.999";
			string sqlCmd = $"SELECT ExecuteState, Count(*) FROM MissionState WHERE(ExecuteState == 'ExecuteSuccessed' OR ExecuteState == 'ExecuteFailed') AND (ReceiveTimestamp BETWEEN '{startTimestamp}' AND '{endTimestamp}') GROUP BY ExecuteState";
			DataSet searchData = rDatabaseAdapter.ExecuteQueryCommand(sqlCmd);
			if (searchData == null || searchData.Tables == null || searchData.Tables.Count == 0) return;

			DataTable searchResult = searchData?.Tables[0];
			if (searchResult != null && searchResult.Rows != null && searchResult.Rows.Count > 0)
			{
				for (int i = 0; i < searchResult.Rows.Count; ++i)
				{
					if (searchResult.Rows[i].ItemArray[0].ToString() == ExecuteState.ExecuteSuccessed.ToString())
					{
						dataFromDatabase.SetSuccessedCount(int.Parse(searchResult.Rows[i].ItemArray[1].ToString()));
						continue;
					}
					if (searchResult.Rows[i].ItemArray[0].ToString() == ExecuteState.ExecuteFailed.ToString())
					{
						dataFromDatabase.SetFailedCount(int.Parse(searchResult.Rows[i].ItemArray[1].ToString()));
						continue;
					}
				}
			}
			UpdateDashboard_DailyMissionCount(dataFromDatabase);
		}
		private void UpdateDashboard_DailyMissionCount(DailyMissionCount DailyMissionCount)
		{
			ucDailyMissionCount1.InvokeIfNecessary(() =>
			{
				ucDailyMissionCount1.Set(DailyMissionCount.mDate, DailyMissionCount.mSuccessedCount, DailyMissionCount.mFailedCount);
			});
		}
		#endregion

		#region DailyMissionAverageCost
		private void UpdateDashboardViaSampleData_DailyMissionAverageCost()
		{
			DailyMissionAverageCost tmp = new DailyMissionAverageCost(mEndDate, 50, 209.3f);
			UpdateDashboard_DailyMissionAverageCost(tmp);
		}
		private void UpdateDashboardViaDatabase_DailyMissionAverageCost()
		{
			// Command:
			//		SELECT COUNT(*), CAST(SUM(JULIANDAY(ExecutionStopTimestamp) - JULIANDAY(ExecutionStartTimestamp)) / COUNT(*) * 24 * 60 * 60 * 1000 AS INTEGER) AS ExeAvgCostInMs FROM MissionState WHERE ExecuteState == 'ExecuteSuccessed' AND (ReceiveTimestamp BETWEEN DATE('now', '-1 days') AND DATE('now', '-0 days'))
			// Result:
			//		1 27815
			// Result:
			//		0
			// Commnet:
			//		Average is Empty when COUNT(*) Equals to 0
			DailyMissionAverageCost dataFromDatabase = new DailyMissionAverageCost(mEndDate, 0, 0.0f);
			string startTimestamp = $"{dataFromDatabase.mDate.ToString("yyyy-MM-dd")} 00:00:00.000";
			string endTimestamp = $"{dataFromDatabase.mDate.ToString("yyyy-MM-dd")} 23:59:59.999";
			string sqlCmd = $"SELECT COUNT(*), CAST(SUM(JULIANDAY(ExecutionStopTimestamp) - JULIANDAY(ExecutionStartTimestamp)) / COUNT(*) * 24 * 60 * 60 * 1000 AS INTEGER) AS ExeAvgCostInMs FROM MissionState WHERE ExecuteState == 'ExecuteSuccessed' AND (ReceiveTimestamp BETWEEN '{startTimestamp}' AND '{endTimestamp}')";
			DataSet searchData = rDatabaseAdapter.ExecuteQueryCommand(sqlCmd);
			if (searchData == null || searchData.Tables == null || searchData.Tables.Count == 0) return;

			DataTable searchResult = searchData?.Tables[0];
			if (searchResult != null && searchResult.Rows != null && searchResult.Rows.Count == 1)
			{
				dataFromDatabase.SetSuccessedMissionCount(int.Parse(searchResult.Rows[0].ItemArray[0].ToString()));
				var tmp = searchResult.Rows[0].ItemArray[1].ToString();
				dataFromDatabase.SetAverageCostInSec(double.Parse(!string.IsNullOrEmpty(searchResult.Rows[0].ItemArray[1].ToString()) ? searchResult.Rows[0].ItemArray[1].ToString() : "0") / 1000);
			}
			UpdateDashboard_DailyMissionAverageCost(dataFromDatabase);
		}
		private void UpdateDashboard_DailyMissionAverageCost(DailyMissionAverageCost DailyMissionAverageCost)
		{
			ucDailyMissionAverageCost1.InvokeIfNecessary(() =>
			{
				ucDailyMissionAverageCost1.Set(DailyMissionAverageCost.mDate, DailyMissionAverageCost.mSuccessedMissionCount, DailyMissionAverageCost.mAverageCostInSec);
			});
		}
		#endregion

		//#region DailyVehicleState
		//private void UpdateDashboardViaSampleData_DailyVehicleState()
		//{
		//	DailyVehicleState tmp = new DailyVehicleState("Vehicle001", DateTime.Now.Date, 10, 8.2f, 3.17f, 6.253f, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1);
		//	UpdateDashboard_DailyVehicleState(tmp);
		//}
		//private void UpdateDashboard_DailyVehicleState(DailyVehicleState DailyVehicleState)
		//{
		//	chart2.InvokeIfNecessary(() =>
		//	{
		//		InitializeChartForDailyVehicleState(chart2, DailyVehicleState.mDate, DailyVehicleState.mVehicleId);
		//		AddDataToChartForDailyVehicleState(chart2, DailyVehicleState);
		//	});
		//}

		//private static void InitializeChartForDailyVehicleState(Chart Chart, DateTime Date, string VehicleId)
		//{
		//	Chart.Series.Clear();
		//	Chart.ChartAreas.Clear();
		//	Chart.Legends.Clear();
		//	Chart.Titles.Clear();
		//	Chart.BackColor = Color.FromArgb(31, 31, 31);

		//	Chart.Titles.Add(GenerateMainTitleForDailyVehicleState());
		//	Chart.Titles.Add(GenerateSubTitleForDailyVehicleState(Date, VehicleId));
		//	Chart.Legends.Add(GenerateLegendForDailyVehicleState());
		//	Chart.ChartAreas.Add(GenerateChartAreaForDailyVehicleState());
		//}
		//private static void AddDataToChartForDailyVehicleState(Chart Chart, DailyVehicleState DailyVehicleState)
		//{
		//	Chart.Series.Add(GenerateSeriesForDailyVehicleState(DailyVehicleState.GetStateCollection(), DailyVehicleState.GetStateDurationCollection()));
		//}
		//private static Title GenerateMainTitleForDailyVehicleState()
		//{
		//	return new Title("Vehicle State of Last Day", Docking.Top, new Font("新細明體", 16, FontStyle.Bold), Color.White);
		//}
		//private static Title GenerateSubTitleForDailyVehicleState(DateTime Date, string VehicleId)
		//{
		//	return new Title($"{VehicleId} - {Date.ToString("yyyy / MM / dd")}", Docking.Top, new Font("新細明體", 10), Color.White);
		//}
		//private static Legend GenerateLegendForDailyVehicleState()
		//{
		//	return new Legend() { BackColor = Color.Transparent, ForeColor = Color.White, Font = new Font("新細明體", 10) };
		//}
		//private static ChartArea GenerateChartAreaForDailyVehicleState()
		//{
		//	ChartArea result = new ChartArea();
		//	result.BackColor = Color.Transparent;
		//	result.Area3DStyle.Enable3D = true;
		//	result.Area3DStyle.Rotation = 45;
		//	result.Area3DStyle.Inclination = 30;
		//	return result;
		//}
		//private static Series GenerateSeriesForDailyVehicleState(string[] StateNames, double[] StateValues)
		//{
		//	Series result = new Series();
		//	result.ChartType = SeriesChartType.Pie;
		//	result.LegendText = "#VALX";
		//	result.Label = "#PERCENT{P1} (#VALY hr)";
		//	result.Font = new Font("新細明體", 10);
		//	result.IsValueShownAsLabel = false;
		//	result.LabelForeColor = Color.White;
		//	result["PieLabelStyle"] = "Outside";
		//	result.ToolTip = "#VALX";
		//	result.Points.DataBindXY(StateNames, StateValues);
		//	result.Points[0].Color = Color.Gray;
		//	result.Points[1].Color = Color.Green;
		//	result.Points[2].Color = Color.LawnGreen;
		//	result.Points[3].Color = Color.Yellow;
		//	result.Points[4].Color = Color.Red;
		//	for (int i = 0; i < result.Points.Count; ++i)
		//	{
		//		result.Points[i]["Exploded"] = "true";
		//	}
		//	return result;
		//}
		//#endregion

		#region DailyVehicleBatteryState
		private void UpdateDashboardViaSampleData_DailyVehicleBatteryState()
		{
			List<DateTime> tmp1 = new List<DateTime>()
			{
				mEndDate.AddHours(0.1),
				mEndDate.AddHours(1.0),
				mEndDate.AddHours(2.0),
				mEndDate.AddHours(3.0),
				mEndDate.AddHours(4.0),
				mEndDate.AddHours(5.0),
				mEndDate.AddHours(6.0),
				mEndDate.AddHours(7.0),
				mEndDate.AddHours(8.0),
				mEndDate.AddHours(9.0),
				mEndDate.AddHours(10.0),
				mEndDate.AddHours(11.0),
				mEndDate.AddHours(12.0),
				mEndDate.AddHours(13.0),
				mEndDate.AddHours(14.0),
				mEndDate.AddHours(15.0),
				mEndDate.AddHours(16.0),
				mEndDate.AddHours(17.0),
				mEndDate.AddHours(18.0),
				mEndDate.AddHours(19.0),
				mEndDate.AddHours(20.0),
				mEndDate.AddHours(21.0),
				mEndDate.AddHours(22.0),
				mEndDate.AddHours(23.0),
				mEndDate.AddHours(23.9)
			};
			List<double> tmp2 = new List<double>()
			{
				0.0f,
				0.0f,
				70.1f,
				60.1f,
				50.1f,
				40.1f,
				30.1f,
				20.1f,
				10.1f,
				30.1f,
				50.1f,
				70.1f,
				90.1f,
				99.1f,
				99.1f,
				99.1f,
				99.1f,
				90.1f,
				70.1f,
				60.1f,
				50.1f,
				40.1f,
				30.1f,
				20.1f,
				30f
			};
			List<double> tmp3 = new List<double>()
			{
				100.0f,
				100.0f,
				100.0f,
				100.0f,
				100.0f,
				100.0f,
				100.0f,
				100.0f,
				100.0f,
				90.1f,
				80.1f,
				70.1f,
				60.1f,
				50.1f,
				40.1f,
				30.1f,
				30.1f,
				30.1f,
				40.1f,
				50.1f,
				60.1f,
				70.1f,
				80.1f,
				90.1f,
				100.0f
			};
			DailyVehicleBatteryState tmp4 = new DailyVehicleBatteryState("Vehicle001", mEndDate, tmp1, tmp2);
			DailyVehicleBatteryState tmp5 = new DailyVehicleBatteryState("Vehicle002", mEndDate, tmp1, tmp3);
			UpdateDashboard_DailyVehicleBatteryState(new DailyVehicleBatteryState[] { tmp4, tmp5 });
		}
		private void UpdateDashboardViaDatabase_DailyVehicleBatteryState()
		{
			// Command:
			//		SELECT name FROM sqlite_master WHERE type='table' AND name LIKE 'HistoryVehicleInfoOf%' ORDER BY UPPER(name)
			// Result:
			//		HistoryVehicleInfoOfiTSDash100
			//		HistoryVehicleInfoOfNewiTSDash300
			//		HistoryVehicleInfoOfVehicle031
			//		HistoryVehicleInfoOfVehicle767
			// Command:
			//		SELECT RecordTimestamp, BatteryValue FROM HistoryVehicleInfoOfiTSDash100 WHERE RecordTimestamp BETWEEN DATE('now', '-1 days') AND DATE('now', '-0 days')
			// Result:
			//		2020-04-14 10:20:45.356 89.14
			//		2020-04-14 10:20:48.356 90.04
			//		2020-04-14 10:20:51.356 92.78
			//		2020-04-14 10:20:54.357 89.25
			//		2020-04-14 10:20:57.357 92.72
			//		2020-04-14 10:21:00.358 95.11
			//		2020-04-14 10:21:03.358 99.19
			//		2020-04-14 10:21:06.358 98.85
			//		2020-04-14 10:21:09.359 96.18
			//		2020-04-14 10:21:12.359 96.6
			//		2020-04-14 10:21:15.359 94.49
			//		2020-04-14 10:21:18.360 90.23
			//		2020-04-14 10:21:21.360 96.83
			//		2020-04-14 10:21:24.360 98.04
			Dictionary<string, DailyVehicleBatteryState> dataFromDatabase = new Dictionary<string, DailyVehicleBatteryState>();
			string sqlCmd = "SELECT name FROM sqlite_master WHERE type='table' AND name LIKE 'HistoryVehicleInfoOf%' ORDER BY UPPER(name)";
			DataSet searchData = rDatabaseAdapter.ExecuteQueryCommand(sqlCmd);
			if (searchData == null || searchData.Tables == null || searchData.Tables.Count == 0) return;
			DataTable searchResult = searchData?.Tables[0];
			if (searchResult != null && searchResult.Rows != null && searchResult.Rows.Count > 0)
			{
				List<string> vehicleIds = new List<string>();
				for (int i = 0; i < searchResult.Rows.Count; ++i)
				{
					string tableName = searchResult.Rows[i].ItemArray[0].ToString();
					string vehicleId = tableName.Replace("HistoryVehicleInfoOf", string.Empty).Replace("Dash", "-");
					string startTimestamp = $"{mEndDate.ToString("yyyy-MM-dd")} 00:00:00.000";
					string endTimestamp = $"{mEndDate.ToString("yyyy-MM-dd")} 23:59:59.999";
					string sqlCmdTmp = $"SELECT RecordTimestamp, BatteryValue FROM {tableName} WHERE RecordTimestamp BETWEEN '{startTimestamp}' AND '{endTimestamp}'";
					DataSet searchDataTmp = rDatabaseAdapter.ExecuteQueryCommand(sqlCmdTmp);
					if (searchDataTmp == null || searchDataTmp.Tables == null || searchDataTmp.Tables.Count == 0) return;

					DataTable searchResultTmp = searchDataTmp?.Tables[0];
					if (searchResultTmp != null && searchResultTmp.Rows != null && searchResultTmp.Rows.Count > 0)
					{
						// 有搜尋到數據才建立容器
						dataFromDatabase.Add(vehicleId, new DailyVehicleBatteryState(vehicleId, mEndDate));
						for (int j = 0; j < searchResultTmp.Rows.Count; ++j)
						{
							var timestamp = searchResultTmp.Rows[j].ItemArray[0];
							var batteryValue = searchResultTmp.Rows[j].ItemArray[1];
							dataFromDatabase[vehicleId].AddBatteryState((DateTime)timestamp, (double)batteryValue);
						}
					}

					if (dataFromDatabase.ContainsKey(vehicleId))
					{
						// 優化電量資料，如果時間紀錄有斷層(車子斷線)，則於斷層處補上電量為 0 的資料
						for (int j = 0; j <= dataFromDatabase[vehicleId].mCollection.Count; ++j)
						{
							// 對第一筆資料做處理
							if (j == 0)
							{
								// 如果第一筆資料與當天 00:00:00.000 差距大於 3 分鐘以上時，則於之間加入電量為 0 的資料
								if (dataFromDatabase[vehicleId].mCollection.First().mTimestamp.Subtract(dataFromDatabase[vehicleId].mDate).TotalMilliseconds > 3.0f)
								{
									VehicleBatteryState tmp1 = new VehicleBatteryState(dataFromDatabase[vehicleId].mDate, 0.0f);
									VehicleBatteryState tmp2 = new VehicleBatteryState(dataFromDatabase[vehicleId].mCollection.First().mTimestamp.AddMinutes(-1), 0.0f);
									dataFromDatabase[vehicleId].mCollection.Insert(0, tmp2);
									dataFromDatabase[vehicleId].mCollection.Insert(0, tmp1);
									j += 2;
								}
							}
							else if (j == dataFromDatabase[vehicleId].mCollection.Count)
							{
								// 如果最後一筆資料與當天 23:59:59.999 差距大於 3 分鐘以上時，則於之間加入電量為 0 的資料
								if (dataFromDatabase[vehicleId].mDate.AddDays(1).AddMilliseconds(-1).Subtract(dataFromDatabase[vehicleId].mCollection.Last().mTimestamp).TotalMinutes > 3.0f)
								{
									VehicleBatteryState tmp1 = new VehicleBatteryState(dataFromDatabase[vehicleId].mCollection.Last().mTimestamp.AddMinutes(1), 0.0f);
									VehicleBatteryState tmp2 = new VehicleBatteryState(dataFromDatabase[vehicleId].mDate.AddDays(1).AddMilliseconds(-1), 0.0f);
									dataFromDatabase[vehicleId].mCollection.Add(tmp1);
									dataFromDatabase[vehicleId].mCollection.Add(tmp2);
									j += 2;
								}
							}
							else
							{
								// 如果與上一筆資料差距大於 3 分鐘以上時，則於之間加入電量為 0 的資料
								if (dataFromDatabase[vehicleId].mCollection[j].mTimestamp.Subtract(dataFromDatabase[vehicleId].mCollection[j - 1].mTimestamp).TotalMinutes > 3.0f)
								{
									VehicleBatteryState tmp1 = new VehicleBatteryState(dataFromDatabase[vehicleId].mCollection[j - 1].mTimestamp.AddMinutes(1), 0.0f);
									VehicleBatteryState tmp2 = new VehicleBatteryState(dataFromDatabase[vehicleId].mCollection[j].mTimestamp.AddMinutes(-1), 0.0f);
									dataFromDatabase[vehicleId].mCollection.Insert(j, tmp2);
									dataFromDatabase[vehicleId].mCollection.Insert(j, tmp1);
									j += 2;
								}
							}
						}
					}
				}
			}
			UpdateDashboard_DailyVehicleBatteryState(dataFromDatabase.Values.ToArray());
		}
		private void UpdateDashboard_DailyVehicleBatteryState(DailyVehicleBatteryState[] DailyVehicleBatteryStateCollection)
		{
			InitializeChartForDisplayingDailyBatteryState(chart2, mEndDate);
			AddDataToChartForDisplayingDailyBatteryState(chart2, DailyVehicleBatteryStateCollection);
		}

		private static void InitializeChartForDisplayingDailyBatteryState(Chart Chart, DateTime Date, bool IsDisplayLegend = true)
		{
			Chart.Series.Clear();
			Chart.ChartAreas.Clear();
			Chart.Legends.Clear();
			Chart.Titles.Clear();
			Chart.BackColor = Color.FromArgb(31, 31, 31);

			Chart.Titles.Add(GenerateMainTitleForDailyVehicleBatteryState());
			Chart.Titles.Add(GenerateSubTitleForDailyVehicleBatteryState(Date));
			if (IsDisplayLegend) Chart.Legends.Add(GenerateLegendForDailyVehicleBatteryState());
			Chart.ChartAreas.Add(GenerateChartAreaForDailyVehicleBatteryState());
		}
		private static void AddDataToChartForDisplayingDailyBatteryState(Chart Chart, params DailyVehicleBatteryState[] DailyVehicleBatteryStateCollection)
		{
			if (DailyVehicleBatteryStateCollection == null || DailyVehicleBatteryStateCollection.Length == 0) return;

			for (int i = 0; i < DailyVehicleBatteryStateCollection.Length; ++i)
			{
				AddDataToChartForDisplayingDailyBatteryState(Chart, DailyVehicleBatteryStateCollection[i]);
			}
		}
		private static void AddDataToChartForDisplayingDailyBatteryState(Chart Chart, DailyVehicleBatteryState DailyVehicleBatteryState)
		{
			Chart.Series.Add(GenerateSeriesForDailyVehicleBatteryState(DailyVehicleBatteryState.mVehicleId, DailyVehicleBatteryState.mCollection.Select(o => o.mTimestamp.Subtract(DailyVehicleBatteryState.mDate).TotalHours).ToArray(), DailyVehicleBatteryState.mCollection.Select(o => o.mBatteryValue).ToArray()));
		}
		private static Title GenerateMainTitleForDailyVehicleBatteryState()
		{
			return new Title("Vehicle Battery State of One Day", Docking.Top, new Font("新細明體", 16, FontStyle.Bold), Color.White);
		}
		private static Title GenerateSubTitleForDailyVehicleBatteryState(DateTime Date)
		{
			return new Title($"{Date.ToString("yyyy / MM / dd")}", Docking.Top, new Font("新細明體", 10), Color.White);
		}
		private static Legend GenerateLegendForDailyVehicleBatteryState()
		{
			return new Legend() { BackColor = Color.Transparent, ForeColor = Color.White, Font = new Font("新細明體", 10), LegendItemOrder = LegendItemOrder.ReversedSeriesOrder };
		}
		private static ChartArea GenerateChartAreaForDailyVehicleBatteryState()
		{
			ChartArea result = new ChartArea();
			result.BackColor = Color.Transparent;

			result.AxisX.Maximum = 24;
			result.AxisX.Minimum = 0;
			result.AxisX.LineColor = Color.FromArgb(160, 160, 160);
			result.AxisX.TitleFont = new Font("新細明體", 10);
			result.AxisX.TitleForeColor = Color.White;
			//result.AxisX.Title = "Time";
			result.AxisX.MajorGrid.Interval = 3;
			result.AxisX.MajorGrid.LineColor = Color.FromArgb(60, 60, 60);
			result.AxisX.MajorGrid.Enabled = false;
			result.AxisX.MajorTickMark.Interval = result.AxisX.MajorGrid.Interval;
			result.AxisX.MajorTickMark.LineColor = result.AxisX.MajorGrid.LineColor;
			result.AxisX.IsLabelAutoFit = false;
			result.AxisX.LabelStyle.ForeColor = Color.White;
			result.AxisX.LabelStyle.Font = new Font("新細明體", 10);
			//result.AxisX.LabelStyle.Angle = -90;
			for (int i = (int)result.AxisX.Minimum; i <= (int)result.AxisX.Maximum; i += (int)result.AxisX.MajorGrid.Interval)
			{
				result.AxisX.CustomLabels.Add(i - 2, i + 2, i.ToString().PadLeft(2, '0')/* + ":00"*/, 0, LabelMarkStyle.None);
			}

			result.AxisY.Maximum = 100;
			result.AxisY.Minimum = 0;
			result.AxisY.LineColor = Color.FromArgb(160, 160, 160);
			result.AxisY.TitleFont = new Font("新細明體", 10);
			result.AxisY.TitleForeColor = Color.White;
			result.AxisY.Title = "Battery Value (%)";
			result.AxisY.MajorGrid.Interval = 20;
			result.AxisY.MajorGrid.LineColor = Color.FromArgb(60, 60, 60);
			result.AxisY.MajorTickMark.Interval = result.AxisY.MajorGrid.Interval;
			result.AxisY.MajorTickMark.LineColor = result.AxisY.MajorGrid.LineColor;
			result.AxisY.IsLabelAutoFit = false;
			result.AxisY.LabelStyle.ForeColor = Color.White;
			result.AxisY.LabelStyle.Font = new Font("新細明體", 10);
			for (int i = (int)result.AxisY.Minimum; i <= (int)result.AxisY.Maximum; i += (int)result.AxisY.MajorGrid.Interval)
			{
				result.AxisY.CustomLabels.Add(i - 0.1, i + 0.1, i.ToString(), 0, LabelMarkStyle.None);
			}
			return result;
		}
		private static Series GenerateSeriesForDailyVehicleBatteryState(string VehicleId, double[] Timestamps, double[] BatteryValues)
		{
			Series result = new Series(VehicleId) { Font = new Font("新細明體", 10), ChartType = SeriesChartType.Line };
			for (int i = 0; i < Timestamps.Length; ++i)
			{
				result.Points.AddXY(Timestamps[i], BatteryValues[i]);
			}
			return result;
		}
		#endregion

		#region WeeklyMissionCout
		private void UpdateDashboardViaSampleData_WeeklyMissionCount()
		{
			DailyVehicleMissionCount tmp01 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-6), 30, 2);
			DailyVehicleMissionCount tmp02 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-5), 40, 3);
			DailyVehicleMissionCount tmp03 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-4), 50, 4);
			DailyVehicleMissionCount tmp04 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-3), 60, 5);
			DailyVehicleMissionCount tmp05 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-2), 70, 6);
			DailyVehicleMissionCount tmp06 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-1), 60, 7);
			DailyVehicleMissionCount tmp07 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(0), 50, 8);
			MultiDayVehicleMissionCount tmp08 = new MultiDayVehicleMissionCount("Vehicle001", tmp01, tmp02, tmp03, tmp04, tmp05, tmp06, tmp07);

			DailyVehicleMissionCount tmp11 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-6), 20, 12);
			DailyVehicleMissionCount tmp12 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-5), 30, 3);
			DailyVehicleMissionCount tmp13 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-4), 40, 12);
			DailyVehicleMissionCount tmp14 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-3), 50, 3);
			DailyVehicleMissionCount tmp15 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-2), 60, 7);
			DailyVehicleMissionCount tmp16 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-1), 70, 9);
			DailyVehicleMissionCount tmp17 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(0), 80, 1);
			MultiDayVehicleMissionCount tmp18 = new MultiDayVehicleMissionCount("Vehicle002", tmp11, tmp12, tmp13, tmp14, tmp15, tmp16, tmp17);

			UpdateDashboard_WeeklyMissionCount(tmp08, tmp18);
		}
		private void UpdateDashboardViaDatabase_WeeklyMissionCount()
		{
			// Command:
			//		SELECT ExecutorID, DATE(ReceiveTimestamp), ExecuteState, COUNT(*) AS 'COUNT' FROM MissionState WHERE (ReceiveTimestamp BETWEEN DATE('now', '-7 days') AND DATE('now', '-0 days')) GROUP BY ExecutorID, DATE(ReceiveTimestamp), ExecuteState ORDER BY UPPER(ExecutorID)
			// Result:
			//		iTS-100    2020-04-14 ExecuteSuccessed 1
			//		Vehicle031 2020-04-08 ExecuteFailed    4
			//		Vehicle031 2020-04-08 ExectueSuccessed 2460
			//		Vehicle031 2020-04-09 ExecuteFailed    13
			//		Vehicle031 2020-04-09 ExecuteSuccessed 2463
			//		Vehicle031 2020-04-10 ExecuteFailed    25
			//		Vehicle031 2020-04-10 ExecuteSuccessed 2445
			//		Vehicle031 2020-04-11 ExecuteFailed    10
			//		Vehicle031 2020-04-11 ExecuteSuccessed 2438
			//		Vehicle031 2020-04-12 ExecuteFailed    41
			//		Vehicle031 2020-04-12 ExecuteSuccessed 2409
			//		Vehicle031 2020-04-13 ExecuteFailed    9
			//		Vehicle031 2020-04-13 ExecuteSuccessed 1608
			//		Vehicle767 2020-04-08 ExecuteFailed    5
			//		Vehicle767 2020-04-08 ExecuteSuccessed 2459
			//		Vehicle767 2020-04-09 ExecuteFailed    13
			//		Vehicle767 2020-04-09 ExecuteSuccessed 2462
			//		Vehicle767 2020-04-09 Executing        1
			//		Vehicle767 2020-04-10 ExecuteFailed    24
			//		Vehicle767 2020-04-10 ExecuteSuccessed 2446
			//		Vehicle767 2020-04-11 ExecuteFailed    10
			//		Vehicle767 2020-04-11 ExecuteSuccessed 2438
			//		Vehicle767 2020-04-12 ExecuteFailed    40
			//		Vehicle767 2020-04-12 ExecuteSuccessed 2410
			//		Vehicle767 2020-04-13 ExecuteFailed    10
			//		Vehicle767 2020-04-13 ExecuteSuccessed 1607
			Dictionary<string, MultiDayVehicleMissionCount> dataFromDatabase = new Dictionary<string, MultiDayVehicleMissionCount>();
			string startTimestamp = $"{mEndDate.AddDays(-6).ToString("yyyy-MM-dd")} 00:00:00.000";
			string endTimestamp = $"{mEndDate.ToString("yyyy-MM-dd")} 23:59:59.999";
			string sqlCmd = $"SELECT ExecutorID, DATE(ReceiveTimestamp), ExecuteState, COUNT(*) AS 'COUNT' FROM MissionState WHERE (ReceiveTimestamp BETWEEN '{startTimestamp}' AND '{endTimestamp}') GROUP BY ExecutorID, DATE(ReceiveTimestamp), ExecuteState ORDER BY UPPER(ExecutorID)";
			DataSet searchData = rDatabaseAdapter.ExecuteQueryCommand(sqlCmd);
			if (searchData == null || searchData.Tables == null || searchData.Tables.Count == 0) return;
			DataTable searchResult = searchData?.Tables[0];
			if (searchResult != null && searchResult.Rows != null && searchResult.Rows.Count > 0)
			{
				for (int i = 0; i < searchResult.Rows.Count; ++i)
				{
					string vehicleId = searchResult.Rows[i].ItemArray[0].ToString();
					string date = searchResult.Rows[i].ItemArray[1].ToString();
					string type = searchResult.Rows[i].ItemArray[2].ToString();
					int count = int.Parse(searchResult.Rows[i].ItemArray[3].ToString());
					// 如果還未建立該 Vehicle 的容器，則建立之
					if (!dataFromDatabase.ContainsKey(vehicleId))
					{
						dataFromDatabase.Add(vehicleId, new MultiDayVehicleMissionCount(vehicleId, mEndDate.AddDays(-6), mEndDate));
					}

					if (type == ExecuteState.ExecuteSuccessed.ToString())
					{
						dataFromDatabase[vehicleId].mCollection.First(o => o.mDate.ToString("yyyy-MM-dd") == date).SetSuccessedMissionCount(count);
						continue;
					}
					if (type == ExecuteState.ExecuteFailed.ToString())
					{
						dataFromDatabase[vehicleId].mCollection.First(o => o.mDate.ToString("yyyy-MM-dd") == date).SetFailedMissionCount(count);
						continue;
					}
				}
			}
			UpdateDashboard_WeeklyMissionCount(dataFromDatabase.Values.ToArray());
		}
		private void UpdateDashboard_WeeklyMissionCount(params MultiDayVehicleMissionCount[] MultiDayVehicleMissionCountCollection)
		{
			chart3.InvokeIfNecessary(() =>
			{
				InitializeChartForWeeklyMissionCout(chart3, mEndDate.AddDays(-6), mEndDate);
				AddDataToChartForWeeklyMissionCout(chart3, MultiDayVehicleMissionCountCollection);
			});
		}

		private static void InitializeChartForWeeklyMissionCout(Chart Chart, DateTime StartDate, DateTime EndDate)
		{
			Chart.Series.Clear();
			Chart.ChartAreas.Clear();
			Chart.Legends.Clear();
			Chart.Titles.Clear();
			Chart.BackColor = Color.FromArgb(31, 31, 31);

			Chart.Titles.Add(GenerateMainTitleForWeeklyMissionCout());
			Chart.Titles.Add(GenerateSubTitleForWeeklyMissionCout(StartDate, EndDate));
			Chart.Legends.Add(GenerateLegendForWeeklyMissionCout());
			Chart.ChartAreas.Add(GenerateChartAreaForWeeklyMissionCout(EndDate));
		}
		private static void AddDataToChartForWeeklyMissionCout(Chart Chart, params MultiDayVehicleMissionCount[] MultiDayVehicleMissionCountCollection)
		{
			if (MultiDayVehicleMissionCountCollection == null || MultiDayVehicleMissionCountCollection.Length == 0) return;

			List<int> successedMissionCount = new List<int>();
			List<int> failedMissionCount = new List<int>();

			for (int i = 0; i < MultiDayVehicleMissionCountCollection.First().mCollection.Count; ++i)
			{
				successedMissionCount.Add(default(int));
				failedMissionCount.Add(default(int));
			}

			for (int i = 0; i < MultiDayVehicleMissionCountCollection.Length; ++i)
			{
				for (int j = 0; j < MultiDayVehicleMissionCountCollection[i].mCollection.Count; ++j)
				{
					successedMissionCount[j] += MultiDayVehicleMissionCountCollection[i].mCollection[j].mSuccessedMissionCount;
					failedMissionCount[j] += MultiDayVehicleMissionCountCollection[i].mCollection[j].mFailedMissionCount;
				}
			}

			Chart.Series.Add(GenerateSeriesForWeeklyMissionCount("Failed", failedMissionCount, Color.Red));
			Chart.Series.Add(GenerateSeriesForWeeklyMissionCount("Successed", successedMissionCount, Color.Green));
		}
		private static Title GenerateMainTitleForWeeklyMissionCout()
		{
			return new Title("Mission Count of Seven Days", Docking.Top, new Font("新細明體", 16, FontStyle.Bold), Color.White);
		}
		private static Title GenerateSubTitleForWeeklyMissionCout(DateTime StartDate, DateTime EndDate)
		{
			return new Title($"{StartDate.ToString("yyyy / MM / dd")} - {EndDate.ToString("yyyy / MM / dd")}", Docking.Top, new Font("新細明體", 10), Color.White);
		}
		private static Legend GenerateLegendForWeeklyMissionCout()
		{
			return new Legend() { BackColor = Color.Transparent, ForeColor = Color.White, Font = new Font("新細明體", 10) };
		}
		private static ChartArea GenerateChartAreaForWeeklyMissionCout(DateTime EndDate)
		{
			ChartArea result = new ChartArea();
			result.BackColor = Color.Transparent;

			result.AxisX.Maximum = 6.6;
			result.AxisX.Minimum = -0.6;
			result.AxisX.LineColor = Color.FromArgb(160, 160, 160);
			result.AxisX.TitleFont = new Font("新細明體", 10);
			result.AxisX.TitleForeColor = Color.White;
			//result.AxisX.Title = "Date";
			result.AxisX.MajorGrid.LineColor = Color.FromArgb(60, 60, 60);
			result.AxisX.MajorGrid.Enabled = false;
			result.AxisX.MajorTickMark.IntervalOffset = 0.6;
			result.AxisX.MajorTickMark.LineColor = result.AxisX.MajorGrid.LineColor;
			result.AxisX.IsLabelAutoFit = false;
			result.AxisX.LabelStyle.ForeColor = Color.White;
			result.AxisX.LabelStyle.Font = new Font("新細明體", 10);
			//result.AxisX.LabelStyle.Angle = -60;
			for (int i = 0; i < 7; ++i)
			{
				result.AxisX.CustomLabels.Add(i - 0.5, i + 0.5, EndDate.AddDays(i + 1 - 7).ToString("MM / dd"), 0, LabelMarkStyle.None);
			}

			result.AxisY.LineColor = Color.FromArgb(160, 160, 160);
			result.AxisY.TitleFont = new Font("新細明體", 10);
			result.AxisY.TitleForeColor = Color.White;
			//result.AxisY.Title = "Count";
			result.AxisY.MajorGrid.Interval = 20;
			result.AxisY.MajorGrid.LineColor = Color.FromArgb(60, 60, 60);
			result.AxisY.MajorTickMark.Interval = result.AxisY.MajorGrid.Interval;
			result.AxisY.MajorTickMark.LineColor = result.AxisY.MajorGrid.LineColor;
			result.AxisY.IsLabelAutoFit = false;
			result.AxisY.LabelStyle.ForeColor = Color.White;
			result.AxisY.LabelStyle.Font = new Font("新細明體", 10);

			return result;
		}
		private static Series GenerateSeriesForWeeklyMissionCount(string Name, List<int> MissionCountOfLastSevenDays, Color SerieColor)
		{
			Series result = new Series(Name) { Color = SerieColor, Font = new Font("新細明體", 10), ChartType = SeriesChartType.StackedColumn };
			result.ToolTip = "#VALY";
			for (int i = 0; i < MissionCountOfLastSevenDays.Count; ++i)
			{
				result.Points.AddXY(i, MissionCountOfLastSevenDays[i]);
			}
			return result;
		}
		#endregion

		#region WeeklyVehicleUsage
		private void UpdateDashboardViaSampleData_WeeklyVehicleUsage()
		{
			DailyVehicleMissionCount tmp01 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-6), 30, 2);
			DailyVehicleMissionCount tmp02 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-5), 40, 3);
			DailyVehicleMissionCount tmp03 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-4), 50, 4);
			DailyVehicleMissionCount tmp04 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-3), 60, 5);
			DailyVehicleMissionCount tmp05 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-2), 70, 6);
			DailyVehicleMissionCount tmp06 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(-1), 60, 7);
			DailyVehicleMissionCount tmp07 = new DailyVehicleMissionCount("Vehicle001", mEndDate.AddDays(0), 50, 8);
			MultiDayVehicleMissionCount tmp08 = new MultiDayVehicleMissionCount("Vehicle001", tmp01, tmp02, tmp03, tmp04, tmp05, tmp06, tmp07);

			DailyVehicleMissionCount tmp11 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-6), 20, 12);
			DailyVehicleMissionCount tmp12 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-5), 30, 3);
			DailyVehicleMissionCount tmp13 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-4), 40, 12);
			DailyVehicleMissionCount tmp14 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-3), 50, 3);
			DailyVehicleMissionCount tmp15 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-2), 60, 7);
			DailyVehicleMissionCount tmp16 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(-1), 70, 9);
			DailyVehicleMissionCount tmp17 = new DailyVehicleMissionCount("Vehicle002", mEndDate.AddDays(0), 80, 1);
			MultiDayVehicleMissionCount tmp18 = new MultiDayVehicleMissionCount("Vehicle002", tmp11, tmp12, tmp13, tmp14, tmp15, tmp16, tmp17);

			InitializeChartForWeeklyVehicleUsage(chart4, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-1));
			AddDataToChartForWeeklyVehicleUsage(chart4, tmp08, tmp18);
		}
		private void UpdateDashboardViaDatabase_WeeklyVehicleUsage()
		{
			// Command:
			//		SELECT ExecutorID, DATE(ReceiveTimestamp), ExecuteState, COUNT(*) AS 'COUNT' FROM MissionState WHERE (ReceiveTimestamp BETWEEN DATE('now', '-7 days') AND DATE('now', '-0 days')) GROUP BY ExecutorID, DATE(ReceiveTimestamp), ExecuteState ORDER BY UPPER(ExecutorID)
			// Result:
			//		iTS-100    2020-04-14 ExecuteSuccessed 1
			//		Vehicle031 2020-04-08 ExecuteFailed    4
			//		Vehicle031 2020-04-08 ExectueSuccessed 2460
			//		Vehicle031 2020-04-09 ExecuteFailed    13
			//		Vehicle031 2020-04-09 ExecuteSuccessed 2463
			//		Vehicle031 2020-04-10 ExecuteFailed    25
			//		Vehicle031 2020-04-10 ExecuteSuccessed 2445
			//		Vehicle031 2020-04-11 ExecuteFailed    10
			//		Vehicle031 2020-04-11 ExecuteSuccessed 2438
			//		Vehicle031 2020-04-12 ExecuteFailed    41
			//		Vehicle031 2020-04-12 ExecuteSuccessed 2409
			//		Vehicle031 2020-04-13 ExecuteFailed    9
			//		Vehicle031 2020-04-13 ExecuteSuccessed 1608
			//		Vehicle767 2020-04-08 ExecuteFailed    5
			//		Vehicle767 2020-04-08 ExecuteSuccessed 2459
			//		Vehicle767 2020-04-09 ExecuteFailed    13
			//		Vehicle767 2020-04-09 ExecuteSuccessed 2462
			//		Vehicle767 2020-04-09 Executing        1
			//		Vehicle767 2020-04-10 ExecuteFailed    24
			//		Vehicle767 2020-04-10 ExecuteSuccessed 2446
			//		Vehicle767 2020-04-11 ExecuteFailed    10
			//		Vehicle767 2020-04-11 ExecuteSuccessed 2438
			//		Vehicle767 2020-04-12 ExecuteFailed    40
			//		Vehicle767 2020-04-12 ExecuteSuccessed 2410
			//		Vehicle767 2020-04-13 ExecuteFailed    10
			//		Vehicle767 2020-04-13 ExecuteSuccessed 1607
			Dictionary<string, MultiDayVehicleMissionCount> dataFromDatabase = new Dictionary<string, MultiDayVehicleMissionCount>();
			string startTimestamp = $"{mEndDate.AddDays(-6).ToString("yyyy-MM-dd")} 00:00:00.000";
			string endTimestamp = $"{mEndDate.ToString("yyyy-MM-dd")} 23:59:59.999";
			string sqlCmd = $"SELECT ExecutorID, DATE(ReceiveTimestamp), ExecuteState, COUNT(*) AS 'COUNT' FROM MissionState WHERE (ReceiveTimestamp BETWEEN '{startTimestamp}' AND '{endTimestamp}') GROUP BY ExecutorID, DATE(ReceiveTimestamp), ExecuteState ORDER BY UPPER(ExecutorID)";
			DataSet searchData = rDatabaseAdapter.ExecuteQueryCommand(sqlCmd);
			if (searchData == null || searchData.Tables == null || searchData.Tables.Count == 0) return;
			DataTable searchResult = searchData?.Tables[0];
			if (searchResult != null && searchResult.Rows != null && searchResult.Rows.Count > 0)
			{
				for (int i = 0; i < searchResult.Rows.Count; ++i)
				{
					string vehicleId = searchResult.Rows[i].ItemArray[0].ToString();
					string date = searchResult.Rows[i].ItemArray[1].ToString();
					string type = searchResult.Rows[i].ItemArray[2].ToString();
					int count = int.Parse(searchResult.Rows[i].ItemArray[3].ToString());
					// 如果還未建立該 Vehicle 的容器，則建立之
					if (!dataFromDatabase.ContainsKey(vehicleId))
					{
						dataFromDatabase.Add(vehicleId, new MultiDayVehicleMissionCount(vehicleId, mEndDate.AddDays(-6), mEndDate));
					}

					if (type == ExecuteState.ExecuteSuccessed.ToString())
					{
						dataFromDatabase[vehicleId].mCollection.First(o => o.mDate.ToString("yyyy-MM-dd") == date).SetSuccessedMissionCount(count);
						continue;
					}
					if (type == ExecuteState.ExecuteFailed.ToString())
					{
						dataFromDatabase[vehicleId].mCollection.First(o => o.mDate.ToString("yyyy-MM-dd") == date).SetFailedMissionCount(count);
						continue;
					}
				}
			}
			UpdateDashboard_WeeklyVehicleUsage(dataFromDatabase.Values.ToArray());
		}
		private void UpdateDashboard_WeeklyVehicleUsage(params MultiDayVehicleMissionCount[] MultiDayVehicleMissionCountCollection)
		{
			chart4.InvokeIfNecessary(() =>
			{
				InitializeChartForWeeklyVehicleUsage(chart4, mEndDate.AddDays(-6), mEndDate);
				AddDataToChartForWeeklyVehicleUsage(chart4, MultiDayVehicleMissionCountCollection);
			});
		}

		private static void InitializeChartForWeeklyVehicleUsage(Chart Chart, DateTime StartDate, DateTime EndDate)
		{
			Chart.Series.Clear();
			Chart.ChartAreas.Clear();
			Chart.Legends.Clear();
			Chart.Titles.Clear();
			Chart.BackColor = Color.FromArgb(31, 31, 31);

			Chart.Titles.Add(GenerateMainTitleForWeeklyVehicleUsage());
			Chart.Titles.Add(GenerateSubTitleForWeeklyVehicleUsage(StartDate, EndDate));
			Chart.Legends.Add(GenerateLegendForWeeklyVehicleUsage());
			Chart.ChartAreas.Add(GenerateChartAreaForWeeklyVehicleUsage(StartDate, EndDate));
		}
		private static void AddDataToChartForWeeklyVehicleUsage(Chart Chart, params MultiDayVehicleMissionCount[] MultiDayVehicleMissionCountCollection)
		{
			if (MultiDayVehicleMissionCountCollection == null || MultiDayVehicleMissionCountCollection.Length == 0) return;

			for (int i = 0; i < MultiDayVehicleMissionCountCollection.Length; ++i)
			{
				Chart.Series.Add(GenerateSeriesForWeeklyVehicleUsage(MultiDayVehicleMissionCountCollection[i].mVehicleId, MultiDayVehicleMissionCountCollection[i].mCollection.Select(o => o.mTotalMissionCount).ToList()));
			}
		}
		private static Title GenerateMainTitleForWeeklyVehicleUsage()
		{
			return new Title("Vehicle Usage of Seven Days", Docking.Top, new Font("新細明體", 16, FontStyle.Bold), Color.White);
		}
		private static Title GenerateSubTitleForWeeklyVehicleUsage(DateTime StartDate, DateTime EndDate)
		{
			return new Title($"{StartDate.ToString("yyyy / MM / dd")} - {EndDate.ToString("yyyy / MM / dd")}", Docking.Top, new Font("新細明體", 10), Color.White);
		}
		private static Legend GenerateLegendForWeeklyVehicleUsage()
		{
			return new Legend() { BackColor = Color.Transparent, ForeColor = Color.White, Font = new Font("新細明體", 10), LegendItemOrder = LegendItemOrder.ReversedSeriesOrder };
		}
		private static ChartArea GenerateChartAreaForWeeklyVehicleUsage(DateTime StartDate, DateTime EndDate)
		{
			ChartArea result = new ChartArea();
			result.BackColor = Color.Transparent;

			result.AxisX.Maximum = 6.6;
			result.AxisX.Minimum = -0.6;
			result.AxisX.LineColor = Color.FromArgb(160, 160, 160);
			result.AxisX.TitleFont = new Font("新細明體", 10);
			result.AxisX.TitleForeColor = Color.White;
			//result.AxisX.Title = "Date";
			result.AxisX.MajorGrid.LineColor = Color.FromArgb(60, 60, 60);
			result.AxisX.MajorGrid.Enabled = false;
			result.AxisX.MajorTickMark.IntervalOffset = result.AxisX.Maximum % 1;
			result.AxisX.MajorTickMark.LineColor = result.AxisX.MajorGrid.LineColor;
			result.AxisX.IsLabelAutoFit = false;
			result.AxisX.LabelStyle.ForeColor = Color.White;
			result.AxisX.LabelStyle.Font = new Font("新細明體", 10);
			for (int i = 0; i < 7; ++i)
			{
				result.AxisX.CustomLabels.Add(i - 0.5, i + 0.5, EndDate.AddDays(i + 1 - 7).ToString("MM / dd"), 0, LabelMarkStyle.None);
			}

			result.AxisY.LineColor = Color.FromArgb(160, 160, 160);
			result.AxisY.TitleFont = new Font("新細明體", 10);
			result.AxisY.TitleForeColor = Color.White;
			result.AxisY.Title = "Mission Count";
			result.AxisY.MajorGrid.Interval = 20;
			result.AxisY.MajorGrid.LineColor = Color.FromArgb(60, 60, 60);
			result.AxisY.MajorTickMark.Interval = result.AxisY.MajorGrid.Interval;
			result.AxisY.MajorTickMark.LineColor = result.AxisY.MajorGrid.LineColor;
			result.AxisY.IsLabelAutoFit = false;
			result.AxisY.LabelStyle.ForeColor = Color.White;
			result.AxisY.LabelStyle.Font = new Font("新細明體", 10);
			return result;
		}
		private static Series GenerateSeriesForWeeklyVehicleUsage(string VehicleId, List<int> VehicleMissionCountOfLastSevenDays)
		{
			Series result = new Series(VehicleId) { Font = new Font("新細明體", 10), ChartType = SeriesChartType.StackedColumn };
			result.ToolTip = "#VALY";
			//result["PixelPointWidth"] = "60";
			for (int i = 0; i < VehicleMissionCountOfLastSevenDays.Count; ++i)
			{
				result.Points.AddXY(i, VehicleMissionCountOfLastSevenDays[i]);
			}
			return result;
		}
		#endregion
	}
}
