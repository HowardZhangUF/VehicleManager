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

namespace TrafficControlTest.UserControl
{
	public partial class UcDashboard : System.Windows.Forms.UserControl
	{
		public UcDashboard()
		{
			InitializeComponent();

			TestForDailyMissionCount();
			TestForDailyMissionAverageCost();
			//TestForDailyVehicleState();
			TestOfChartOfDailyBatteryState();
			TestOfChartOfWeeklyMissionAmount();
			TestOfChartOfWeeklyVehicleUsage();
		}

		#region DailyMissionCount
		public void TestForDailyMissionCount()
		{
			DailyMissionCount tmp = new DailyMissionCount(DateTime.Now.AddDays(-1), 50, 14);
			ucDailyMissionCount1.Set(tmp.mDate, tmp.mSuccessedCount, tmp.mFailedCount);
		}
		#endregion

		#region DailyMissionAverageCost
		public void TestForDailyMissionAverageCost()
		{
			DailyMissionAverageCost tmp = new DailyMissionAverageCost(DateTime.Now.AddDays(-1), 50, 209.3f);
			ucDailyMissionAverageCost1.Set(tmp.mDate, tmp.mSuccessedMissionCount, tmp.mAverageCostInSec);
		}
		#endregion

		#region DailyVehicleState
		public void TestForDailyVehicleState()
		{
			DailyVehicleState tmp = new DailyVehicleState("Vehicle001", DateTime.Now.Date, 10, 8.2f, 3.17f, 6.253f, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1);
			//InitializeChartForDailyVehicleState(chart1, tmp.mDate, tmp.mVehicleId);
			//AddDataToChartForDailyVehicleState(chart1, tmp);
		}

		private static void InitializeChartForDailyVehicleState(Chart Chart, DateTime Date, string VehicleId)
		{
			Chart.Series.Clear();
			Chart.ChartAreas.Clear();
			Chart.Legends.Clear();
			Chart.Titles.Clear();
			Chart.BackColor = Color.FromArgb(31, 31, 31);

			Chart.Titles.Add(GenerateMainTitleForDailyVehicleState());
			Chart.Titles.Add(GenerateSubTitleForDailyVehicleState(Date, VehicleId));
			Chart.Legends.Add(GenerateLegendForDailyVehicleState());
			Chart.ChartAreas.Add(GenerateChartAreaForDailyVehicleState());
		}
		private static void AddDataToChartForDailyVehicleState(Chart Chart, DailyVehicleState DailyVehicleState)
		{
			Chart.Series.Add(GenerateSeriesForDailyVehicleState(DailyVehicleState.GetStateCollection(), DailyVehicleState.GetStateDurationCollection()));
		}
		private static Title GenerateMainTitleForDailyVehicleState()
		{
			return new Title("Vehicle State of Last Day", Docking.Top, new Font("新細明體", 16, FontStyle.Bold), Color.White);
		}
		private static Title GenerateSubTitleForDailyVehicleState(DateTime Date, string VehicleId)
		{
			return new Title($"{VehicleId} - {Date.ToString("yyyy / MM / dd")}", Docking.Top, new Font("新細明體", 10), Color.White);
		}
		private static Legend GenerateLegendForDailyVehicleState()
		{
			return new Legend() { BackColor = Color.Transparent, ForeColor = Color.White, Font = new Font("新細明體", 10) };
		}
		private static ChartArea GenerateChartAreaForDailyVehicleState()
		{
			ChartArea result = new ChartArea();
			result.BackColor = Color.Transparent;
			result.Area3DStyle.Enable3D = true;
			result.Area3DStyle.Rotation = 45;
			result.Area3DStyle.Inclination = 30;
			return result;
		}
		private static Series GenerateSeriesForDailyVehicleState(string[] StateNames, double[] StateValues)
		{
			Series result = new Series();
			result.ChartType = SeriesChartType.Pie;
			result.LegendText = "#VALX";
			result.Label = "#PERCENT{P1} (#VALY hr)";
			result.Font = new Font("新細明體", 10);
			result.IsValueShownAsLabel = false;
			result.LabelForeColor = Color.White;
			result["PieLabelStyle"] = "Outside";
			result.ToolTip = "#VALX";
			result.Points.DataBindXY(StateNames, StateValues);
			result.Points[0].Color = Color.Gray;
			result.Points[1].Color = Color.Green;
			result.Points[2].Color = Color.LawnGreen;
			result.Points[3].Color = Color.Yellow;
			result.Points[4].Color = Color.Red;
			for (int i = 0; i < result.Points.Count; ++i)
			{
				result.Points[i]["Exploded"] = "true";
			}
			return result;
		}
		#endregion

		#region DailyVehicleBatteryState
		public void TestOfChartOfDailyBatteryState()
		{
			List<DateTime> tmp1 = new List<DateTime>()
			{
				DateTime.Now.Date.AddHours(0.1),
				DateTime.Now.Date.AddHours(1.0),
				DateTime.Now.Date.AddHours(2.0),
				DateTime.Now.Date.AddHours(3.0),
				DateTime.Now.Date.AddHours(4.0),
				DateTime.Now.Date.AddHours(5.0),
				DateTime.Now.Date.AddHours(6.0),
				DateTime.Now.Date.AddHours(7.0),
				DateTime.Now.Date.AddHours(8.0),
				DateTime.Now.Date.AddHours(9.0),
				DateTime.Now.Date.AddHours(10.0),
				DateTime.Now.Date.AddHours(11.0),
				DateTime.Now.Date.AddHours(12.0),
				DateTime.Now.Date.AddHours(13.0),
				DateTime.Now.Date.AddHours(14.0),
				DateTime.Now.Date.AddHours(15.0),
				DateTime.Now.Date.AddHours(16.0),
				DateTime.Now.Date.AddHours(17.0),
				DateTime.Now.Date.AddHours(18.0),
				DateTime.Now.Date.AddHours(19.0),
				DateTime.Now.Date.AddHours(20.0),
				DateTime.Now.Date.AddHours(21.0),
				DateTime.Now.Date.AddHours(22.0),
				DateTime.Now.Date.AddHours(23.0),
				DateTime.Now.Date.AddHours(23.9)
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
			DailyVehicleBatteryState tmp4 = new DailyVehicleBatteryState("Vehicle001", DateTime.Now, tmp1, tmp2);
			DailyVehicleBatteryState tmp5 = new DailyVehicleBatteryState("Vehicle002", DateTime.Now, tmp1, tmp3);

			InitializeChartForDisplayingDailyBatteryState(chart2, tmp4.mDate, tmp4.mVehicleId);
			AddDataToChartForDisplayingDailyBatteryState(chart2, tmp4);
			AddDataToChartForDisplayingDailyBatteryState(chart2, tmp5);
		}

		private static void InitializeChartForDisplayingDailyBatteryState(Chart Chart, DateTime Date, string VehicleName, bool IsDisplayLegend = true)
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
		private static void AddDataToChartForDisplayingDailyBatteryState(Chart Chart, DailyVehicleBatteryState DailyVehicleBatteryState)
		{
			Chart.Series.Add(GenerateSeriesForDailyVehicleBatteryState(DailyVehicleBatteryState.mVehicleId, DailyVehicleBatteryState.mCollection.Select(o => o.mTimestamp.Subtract(DailyVehicleBatteryState.mDate).TotalHours).ToArray(), DailyVehicleBatteryState.mCollection.Select(o => o.mBatteryValue).ToArray()));
		}
		private static Title GenerateMainTitleForDailyVehicleBatteryState()
		{
			return new Title("Vehicle Battery State of Last Day", Docking.Top, new Font("新細明體", 16, FontStyle.Bold), Color.White);
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
		public void TestOfChartOfWeeklyMissionAmount()
		{
			DailyVehicleMissionCount tmp01 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-7), 30, 2);
			DailyVehicleMissionCount tmp02 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-6), 40, 3);
			DailyVehicleMissionCount tmp03 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-5), 50, 4);
			DailyVehicleMissionCount tmp04 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-4), 60, 5);
			DailyVehicleMissionCount tmp05 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-3), 70, 6);
			DailyVehicleMissionCount tmp06 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-2), 60, 7);
			DailyVehicleMissionCount tmp07 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-1), 50, 8);
			MultiDayVehicleMissionCount tmp08 = new MultiDayVehicleMissionCount("Vehicle001", tmp01, tmp02, tmp03, tmp04, tmp05, tmp06, tmp07);

			DailyVehicleMissionCount tmp11 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-7), 20, 12);
			DailyVehicleMissionCount tmp12 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-6), 30, 3);
			DailyVehicleMissionCount tmp13 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-5), 40, 12);
			DailyVehicleMissionCount tmp14 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-4), 50, 3);
			DailyVehicleMissionCount tmp15 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-3), 60, 7);
			DailyVehicleMissionCount tmp16 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-2), 70, 9);
			DailyVehicleMissionCount tmp17 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-1), 80, 1);
			MultiDayVehicleMissionCount tmp18 = new MultiDayVehicleMissionCount("Vehicle002", tmp11, tmp12, tmp13, tmp14, tmp15, tmp16, tmp17);

			InitializeChartForWeeklyMissionCout(chart3, tmp08.mStartDate, tmp08.mEndDate);
			AddDataToChartForWeeklyMissionCout(chart3, tmp08, tmp18);
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
			return new Title("Mission Count of Last Week", Docking.Top, new Font("新細明體", 16, FontStyle.Bold), Color.White);
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
		public void TestOfChartOfWeeklyVehicleUsage()
		{
			DailyVehicleMissionCount tmp01 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-7), 30, 2);
			DailyVehicleMissionCount tmp02 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-6), 40, 3);
			DailyVehicleMissionCount tmp03 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-5), 50, 4);
			DailyVehicleMissionCount tmp04 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-4), 60, 5);
			DailyVehicleMissionCount tmp05 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-3), 70, 6);
			DailyVehicleMissionCount tmp06 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-2), 60, 7);
			DailyVehicleMissionCount tmp07 = new DailyVehicleMissionCount("Vehicle001", DateTime.Now.AddDays(-1), 50, 8);
			MultiDayVehicleMissionCount tmp08 = new MultiDayVehicleMissionCount("Vehicle001", tmp01, tmp02, tmp03, tmp04, tmp05, tmp06, tmp07);

			DailyVehicleMissionCount tmp11 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-7), 20, 12);
			DailyVehicleMissionCount tmp12 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-6), 30, 3);
			DailyVehicleMissionCount tmp13 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-5), 40, 12);
			DailyVehicleMissionCount tmp14 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-4), 50, 3);
			DailyVehicleMissionCount tmp15 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-3), 60, 7);
			DailyVehicleMissionCount tmp16 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-2), 70, 9);
			DailyVehicleMissionCount tmp17 = new DailyVehicleMissionCount("Vehicle002", DateTime.Now.AddDays(-1), 80, 1);
			MultiDayVehicleMissionCount tmp18 = new MultiDayVehicleMissionCount("Vehicle002", tmp11, tmp12, tmp13, tmp14, tmp15, tmp16, tmp17);

			InitializeChartForWeeklyVehicleUsage(chart4, tmp08.mStartDate, tmp08.mEndDate);
			AddDataToChartForWeeklyVehicleUsage(chart4, tmp08, tmp18);
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
			for (int i = 0; i < MultiDayVehicleMissionCountCollection.Length; ++i)
			{
				Chart.Series.Add(GenerateSeriesForWeeklyVehicleUsage(MultiDayVehicleMissionCountCollection[i].mVehicleId, MultiDayVehicleMissionCountCollection[i].mCollection.Select(o => o.mTotalMissionCount).ToList()));
			}
		}
		private static Title GenerateMainTitleForWeeklyVehicleUsage()
		{
			return new Title("Vehicle Usage of Last Week", Docking.Top, new Font("新細明體", 16, FontStyle.Bold), Color.White);
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
