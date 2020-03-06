using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.MissionManager.Implement;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Library.Tests
{
	[TestClass()]
	public class LibraryTests
	{
		[TestMethod()]
		public void RegionFactory()
		{
			IPoint2D p1 = Library.GenerateIPoint2D(33, 77);
			if (p1 == null || p1.mX != 33 || p1.mY != 77) Assert.Fail();

			ITowardPoint2D towardP1 = Library.GenerateITowardPoint2D(55, 66, 30.1);
			if (towardP1 == null || towardP1.mX != 55 || towardP1.mY != 66 || towardP1.mToward != 30.1) Assert.Fail();

			ITowardPoint2D towardP2 = Library.GenerateITowardPoint2D(Library.GenerateIPoint2D(11, 22), 49.5);
			if (towardP2 == null || towardP2.mX != 11 || towardP2.mY != 22 || towardP2.mToward != 49.5) Assert.Fail();

			IVector2D vec1 = Library.GenerateIVector2D(13.5, 43.7);
			if (vec1 == null || vec1.mXComponent != 13.5 || vec1.mYComponent != 43.7) Assert.Fail();

			IRectangle2D rect1 = Library.GenerateIRectangle2D(Library.GenerateIPoint2D(77, 88), Library.GenerateIPoint2D(11, 22));
			if (rect1 == null || rect1.mMaxPoint.mX != 77 || rect1.mMaxPoint.mY != 88 || rect1.mMinPoint.mX != 11 || rect1.mMinPoint.mY != 22) Assert.Fail();

			ITimePeriod timePeriod = Library.GenerateITimePeriod(new DateTime(2001, 5, 4, 23, 56, 07), new DateTime(2002, 4, 7, 11, 50, 59));
			if (timePeriod == null || timePeriod.mStart.ToString("yyyy/MM/dd HH:mm:ss") != "2001/05/04 23:56:07" || timePeriod.mEnd.ToString("yyyy/MM/dd HH:mm:ss") != "2002/04/07 11:50:59") Assert.Fail();

			string tmp1 = "Command=Goto Target=Goal1 CommandID=Cmd003 VehicleID=Vehicle918";
			string tmp2 = "Command=Goto Target=Goal1 CommandID=Cmd003 VehicleID=Vehicle918 Command=Goto";
			string tmp3 = "Command=Goto Target=Goal1 CommandID=Cmd003 VehicleID=Vehicle918 asfxwer";
			if (!Library.ConvertToDictionary(tmp1, out Dictionary<string, string> dic1)) Assert.Fail();
			if (dic1.Count != 4) Assert.Fail();
			if (!dic1.Keys.Contains("Command") || dic1["Command"] != "Goto") Assert.Fail();
			if (!dic1.Keys.Contains("Target") || dic1["Target"] != "Goal1") Assert.Fail();
			if (!dic1.Keys.Contains("CommandID") || dic1["CommandID"] != "Cmd003") Assert.Fail();
			if (!dic1.Keys.Contains("VehicleID") || dic1["VehicleID"] != "Vehicle918") Assert.Fail();
			if (Library.ConvertToDictionary(tmp2, out Dictionary<string, string> dic2)) Assert.Fail();
			if (Library.ConvertToDictionary(tmp3, out Dictionary<string, string> dic3)) Assert.Fail();
		}

		[TestMethod()]
		public void RegionIPoint2D()
		{
			IPoint2D point0 = Library.GenerateIPoint2D(0, 0);
			IPoint2D point1 = Library.GenerateIPoint2D(10, 10);
			IPoint2D point2 = Library.GenerateIPoint2D(-20, 20);
			IPoint2D point3 = Library.GenerateIPoint2D(-30, -30);
			IPoint2D point4 = Library.GenerateIPoint2D(40, -40);
			IPoint2D point5 = Library.GenerateIPoint2D(100, 0);
			IPoint2D point6 = Library.GenerateIPoint2D(-100, 0);
			IPoint2D point7 = Library.GenerateIPoint2D(100, 100);
			IPoint2D point8 = Library.GenerateIPoint2D(-100, -100);
			IPoint2D point9 = Library.GenerateIPoint2D(0, 100);
			IPoint2D point10 = Library.GenerateIPoint2D(0, -100);

			if (Library.GetAngle(point0, point2).ToString("F2") != "135.00") Assert.Fail();
			if (Library.GetAngle(point2, point0).ToString("F2") != "-45.00") Assert.Fail();
			if (Library.GetAngle(point0, point4).ToString("F2") != "-45.00") Assert.Fail();
			if (Library.GetAngle(point4, point0).ToString("F2") != "135.00") Assert.Fail();

			if (Library.GetDistance(point1, point2).ToString("F2") != "31.62") Assert.Fail();

			if (Library.GetDistance(new List<IPoint2D>() { point1, point2, point3}).ToString("F2") != "82.61") Assert.Fail();

			if (Library.GetDistanceSquare(point3, point4) != 5000) Assert.Fail();

			IEnumerable<IPoint2D> intersectionPoints1 = Library.GetIntersectionPoint(Library.GenerateIRectangle2D(point1, point3), point0, point5);
			if (intersectionPoints1.Count() != 1 || intersectionPoints1.Where((o) => o.ToString() == Library.GenerateIPoint2D(10, 0).ToString()).Count() != 1) Assert.Fail();
			IEnumerable<IPoint2D> intersectionPoints2 = Library.GetIntersectionPoint(Library.GenerateIRectangle2D(point1, point3), point5, point6);
			if (intersectionPoints2.Count() != 2 || !intersectionPoints2.Any((o) => o.ToString() == Library.GenerateIPoint2D(10, 0).ToString()) || !intersectionPoints2.Any((o) => o.ToString() == Library.GenerateIPoint2D(-30, 0).ToString())) Assert.Fail();
			IEnumerable<IPoint2D> intersectionPoints3 = Library.GetIntersectionPoint(Library.GenerateIRectangle2D(point1, point0), point3, point4);
			if (intersectionPoints3.Count() != 0) Assert.Fail();
			IEnumerable<IPoint2D> intersectionPoints4 = Library.GetIntersectionPoint(Library.GenerateIRectangle2D(point1, point3), point7, point8);
			if (intersectionPoints4.Count() != 2 || !intersectionPoints4.Any((o) => o.ToString() == Library.GenerateIPoint2D(10, 10).ToString()) || !intersectionPoints4.Any((o) => o.ToString() == Library.GenerateIPoint2D(-30, -30).ToString())) Assert.Fail();

			IPoint2D intersectionPoint1 = Library.GetIntersectionPoint(point1, point3, point2, point4);
			if (intersectionPoint1.mX != 0 || intersectionPoint1.mY != 0) Assert.Fail();
			IPoint2D intersectionPoint2 = Library.GetIntersectionPoint(point1, point4, point0, point5);
			if (intersectionPoint2.mX != 16 || intersectionPoint2.mY != 0) Assert.Fail();

			IEnumerable<IPoint2D> linePoints1 = Library.ConvertLineToPoints(point0, point5, 20);
			if (linePoints1.Count() != 4) Assert.Fail();
			if (linePoints1.ElementAt(0).ToString() != Library.GenerateIPoint2D(20, 0).ToString()) Assert.Fail();
			if (linePoints1.ElementAt(1).ToString() != Library.GenerateIPoint2D(40, 0).ToString()) Assert.Fail();
			if (linePoints1.ElementAt(2).ToString() != Library.GenerateIPoint2D(60, 0).ToString()) Assert.Fail();
			if (linePoints1.ElementAt(3).ToString() != Library.GenerateIPoint2D(80, 0).ToString()) Assert.Fail();
			IEnumerable<IPoint2D> linePoints2 = Library.ConvertLineToPoints(point5, point0, 20);
			if (linePoints2.Count() != 4) Assert.Fail();
			if (linePoints2.ElementAt(0).ToString() != Library.GenerateIPoint2D(80, 0).ToString()) Assert.Fail();
			if (linePoints2.ElementAt(1).ToString() != Library.GenerateIPoint2D(60, 0).ToString()) Assert.Fail();
			if (linePoints2.ElementAt(2).ToString() != Library.GenerateIPoint2D(40, 0).ToString()) Assert.Fail();
			if (linePoints2.ElementAt(3).ToString() != Library.GenerateIPoint2D(20, 0).ToString()) Assert.Fail();
			IEnumerable<IPoint2D> linePoints3 = Library.ConvertLineToPoints(point0, point10, 20);
			if (linePoints3.Count() != 4) Assert.Fail();
			if (linePoints3.ElementAt(0).ToString() != Library.GenerateIPoint2D(0, -20).ToString()) Assert.Fail();
			if (linePoints3.ElementAt(1).ToString() != Library.GenerateIPoint2D(0, -40).ToString()) Assert.Fail();
			if (linePoints3.ElementAt(2).ToString() != Library.GenerateIPoint2D(0, -60).ToString()) Assert.Fail();
			if (linePoints3.ElementAt(3).ToString() != Library.GenerateIPoint2D(0, -80).ToString()) Assert.Fail();
			IEnumerable<IPoint2D> linePoints4 = Library.ConvertLineToPoints(point10, point0, 20);
			if (linePoints4.Count() != 4) Assert.Fail();
			if (linePoints4.ElementAt(0).ToString() != Library.GenerateIPoint2D(0, -80).ToString()) Assert.Fail();
			if (linePoints4.ElementAt(1).ToString() != Library.GenerateIPoint2D(0, -60).ToString()) Assert.Fail();
			if (linePoints4.ElementAt(2).ToString() != Library.GenerateIPoint2D(0, -40).ToString()) Assert.Fail();
			if (linePoints4.ElementAt(3).ToString() != Library.GenerateIPoint2D(0, -20).ToString()) Assert.Fail();
			IEnumerable<IPoint2D> linePoints5 = Library.ConvertLineToPoints(point3, point1, 10);
			if (linePoints5.Count() != 5) Assert.Fail();
			if (linePoints5.ElementAt(0).ToString() != Library.GenerateIPoint2D(-23, -23).ToString()) Assert.Fail();
			if (linePoints5.ElementAt(1).ToString() != Library.GenerateIPoint2D(-16, -16).ToString()) Assert.Fail();
			if (linePoints5.ElementAt(2).ToString() != Library.GenerateIPoint2D(-9, -9).ToString()) Assert.Fail();
			if (linePoints5.ElementAt(3).ToString() != Library.GenerateIPoint2D(-2, -2).ToString()) Assert.Fail();
			if (linePoints5.ElementAt(4).ToString() != Library.GenerateIPoint2D(5, 5).ToString()) Assert.Fail();
			IEnumerable<IPoint2D> linePoints6 = Library.ConvertLineToPoints(point1, point3, 10);
			if (linePoints6.Count() != 5) Assert.Fail();
			if (linePoints6.ElementAt(0).ToString() != Library.GenerateIPoint2D(3, 3).ToString()) Assert.Fail();
			if (linePoints6.ElementAt(1).ToString() != Library.GenerateIPoint2D(-4, -4).ToString()) Assert.Fail();
			if (linePoints6.ElementAt(2).ToString() != Library.GenerateIPoint2D(-11, -11).ToString()) Assert.Fail();
			if (linePoints6.ElementAt(3).ToString() != Library.GenerateIPoint2D(-18, -18).ToString()) Assert.Fail();
			if (linePoints6.ElementAt(4).ToString() != Library.GenerateIPoint2D(-25, -25).ToString()) Assert.Fail();
		}

		[TestMethod()]
		public void RegionIVector2D()
		{
			if (Library.GetVector(10, 10, 30, 30).ToString() != Library.GenerateIVector2D(20.0f, 20.0f).ToString()) Assert.Fail();

			if (Library.GetNormalizeVector(Library.GenerateIVector2D(55.0f, 55.0f)).ToString() != Library.GenerateIVector2D(0.71f, 0.71f).ToString()) Assert.Fail();

			if (Library.GetDotProduct(Library.GenerateIVector2D(100, 0), Library.GenerateIVector2D(100, 100)).ToString("F2") != "10000.00") Assert.Fail();

			if (Library.GetAngleOfTwoVector(Library.GenerateIVector2D(100, 0), Library.GenerateIVector2D(100, 100)).ToString("F2") != "45.00") Assert.Fail();
		}

		[TestMethod()]
		public void RegionIRectangle2D()
		{
			IPoint2D point0 = Library.GenerateIPoint2D(0, 0);
			IPoint2D point1 = Library.GenerateIPoint2D(0, 10);
			IPoint2D point2 = Library.GenerateIPoint2D(0, -10);
			IPoint2D point3 = Library.GenerateIPoint2D(30, 40);
			IPoint2D point4 = Library.GenerateIPoint2D(-30, -40);
			IPoint2D point5 = Library.GenerateIPoint2D(10, 20);
			IPoint2D point6 = Library.GenerateIPoint2D(0, 40);
			IPoint2D point7 = Library.GenerateIPoint2D(-10, 0);
			IPoint2D point8 = Library.GenerateIPoint2D(90, 10);

			if (Library.IsRectangleOverlap(Library.GenerateIRectangle2D(point3, point2), Library.GenerateIRectangle2D(point1, point4)) == true) Assert.Fail();
			if (Library.IsRectangleOverlap(Library.GenerateIRectangle2D(point3, point1), Library.GenerateIRectangle2D(point2, point4)) == true) Assert.Fail();
			if (Library.IsRectangleOverlap(Library.GenerateIRectangle2D(point3, point4), Library.GenerateIRectangle2D(point0, point1)) == false) Assert.Fail();
			if (Library.IsRectangleOverlap(Library.GenerateIRectangle2D(point3, point0), Library.GenerateIRectangle2D(point5, point4)) == false) Assert.Fail();

			if (Library.IsPointInside(point0, Library.GenerateIRectangle2D(point3, point4)) == false) Assert.Fail();
			if (Library.IsPointInside(point6, Library.GenerateIRectangle2D(point3, point4)) == false) Assert.Fail();
			if (Library.IsPointInside(point3, Library.GenerateIRectangle2D(point5, point4)) == true) Assert.Fail();

			if (Library.GetCoverRectangle(Library.GenerateIRectangle2D(point3, point0), Library.GenerateIRectangle2D(point5, point4)).ToString() != Library.GenerateIRectangle2D(point3, point4).ToString()) Assert.Fail();
			if (Library.GetCoverRectangle(Library.GenerateIRectangle2D(point6, point4), Library.GenerateIRectangle2D(point5, point7)).ToString() != Library.GenerateIRectangle2D(Library.GenerateIPoint2D(10, 40), point4).ToString()) Assert.Fail();
			if (Library.GetCoverRectangle(Library.GenerateIRectangle2D(point3, point5), Library.GenerateIRectangle2D(point0, point4)).ToString() != Library.GenerateIRectangle2D(point3, point4).ToString()) Assert.Fail();

			if (Library.GetIntersectionRectangle(Library.GenerateIRectangle2D(point3, point0), Library.GenerateIRectangle2D(point5, point4)).ToString() != Library.GenerateIRectangle2D(point5, point0).ToString()) Assert.Fail();
			if (Library.GetIntersectionRectangle(Library.GenerateIRectangle2D(point6, point4), Library.GenerateIRectangle2D(point5, point7)).ToString() != Library.GenerateIRectangle2D(Library.GenerateIPoint2D(0, 20), point7).ToString()) Assert.Fail();
			if (Library.GetIntersectionRectangle(Library.GenerateIRectangle2D(point3, point5), Library.GenerateIRectangle2D(point0, point4)) != null) Assert.Fail();

			if (Library.GetRectangle(10, 20, 40).ToString() != Library.GenerateIRectangle2D(Library.GenerateIPoint2D(50, 60), Library.GenerateIPoint2D(-30, -20)).ToString()) Assert.Fail();

			if (Library.GetAmplifyRectangle(Library.GenerateIRectangle2D(point3, point7), 10, 5).ToString() != Library.GenerateIRectangle2D(Library.GenerateIPoint2D(40, 45), Library.GenerateIPoint2D(-20, -5)).ToString()) Assert.Fail();

			if (Library.GetCoverRectangle(new List<IPoint2D> { point0, point1, point2, point3, point4, point5, point6, point7 }).ToString() != Library.GenerateIRectangle2D(point3, point4).ToString()) Assert.Fail();

			List<IRectangle2D> rectangles1 = new List<IRectangle2D>();
			rectangles1.Add(Library.GenerateIRectangle2D(point3, point2));
			rectangles1.Add(Library.GenerateIRectangle2D(point5, point0));
			rectangles1.Add(Library.GenerateIRectangle2D(point5, point4));
			List<IRectangle2D> mergedRectangles1 = Library.MergeRectangle(rectangles1).ToList();
			if (mergedRectangles1.Count != 1 || !mergedRectangles1.Any((o) => o.ToString() == Library.GenerateIRectangle2D(point3, point4).ToString())) Assert.Fail();
			rectangles1.Add(Library.GenerateIRectangle2D(point8, point0));
			List<IRectangle2D> mergedRectangles2 = Library.MergeRectangle(rectangles1).ToList();
			if (mergedRectangles2.Count != 1 || !mergedRectangles2.Any((o) => o.ToString() == Library.GenerateIRectangle2D(Library.GenerateIPoint2D(90, 40), point4).ToString())) Assert.Fail();
		}

		[TestMethod()]
		public void ClassGotoMissionAnalyzer()
		{
			IMissionAnalyzer missionAnalyzer = Library.GetMissionAnalyzer(MissionType.Goto);
			string tmp1 = "Mission=Goto Target=Goal1";
			string tmp2 = "Mission=Goto Target=Goal1 MissionID=Miss001";
			string tmp3 = "Mission=Goto Target=Goal1 MissionID=Miss001 VehicleID=Vehicle918";
			string tmp4 = "Mission=Goto Target=Goal1 MissionID=Miss001 VehicleID=Vehicle918 Priority=18";
			string tmp5 = "Target=Goal1 MissionID=Miss001 VehicleID=Vehicle918 Priority=18"; // 缺少 Mission
			string tmp6 = "Mission=Goto MissionID=Miss001 VehicleID=Vehicle918 Priority=18"; // 缺少 Target
			string tmp7 = "Mission=Goto Target=Goal1 MissionID=Miss001 VehicleID=Vehicle918 Priority=999"; // 錯誤的 Priority
			string tmp8 = "Mission=Goto Target=Goal1 MissionID=Miss001 VehicleID=Vehicle918 Priority=ABC"; // 錯誤的 Priority
			string tmp9 = "Mission=Goto Target=Goal1 MissionID=Miss001 VehicleID=Vehicle918 Priority=18 abcde"; // 錯誤的訊息格式
			string tmp10 = "Mission=Goto Target=Goal1 Target=Goal2 MissionID=Miss001 VehicleID=Vehicle918 Priority=18"; // 重複的項目

			if (missionAnalyzer.TryParse(tmp1, out IMission mission1, out string detail1) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission1.mMissionType != MissionType.Goto) Assert.Fail();
			if (mission1.mParameters.Length != 1 || mission1.mParameters[0] != "Goal1") Assert.Fail();
			if (mission1.mMissionId != string.Empty) Assert.Fail();
			if (mission1.mVehicleId != string.Empty) Assert.Fail();
			if (mission1.mPriority != MissionAnalyzer.mPriorityDefault) Assert.Fail();
			if (!string.IsNullOrEmpty(detail1)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp2, out IMission mission2, out string detail2) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission2.mMissionType != MissionType.Goto) Assert.Fail();
			if (mission2.mParameters.Length != 1 || mission2.mParameters[0] != "Goal1") Assert.Fail();
			if (mission2.mMissionId != "Miss001") Assert.Fail();
			if (mission2.mVehicleId != string.Empty) Assert.Fail();
			if (mission2.mPriority != MissionAnalyzer.mPriorityDefault) Assert.Fail();
			if (!string.IsNullOrEmpty(detail2)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp3, out IMission mission3, out string detail3) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission3.mMissionType != MissionType.Goto) Assert.Fail();
			if (mission3.mParameters.Length != 1 || mission3.mParameters[0] != "Goal1") Assert.Fail();
			if (mission3.mMissionId != "Miss001") Assert.Fail();
			if (mission3.mVehicleId != "Vehicle918") Assert.Fail();
			if (mission3.mPriority != MissionAnalyzer.mPriorityDefault) Assert.Fail();
			if (!string.IsNullOrEmpty(detail3)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp4, out IMission mission4, out string detail4) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission4.mMissionType != MissionType.Goto) Assert.Fail();
			if (mission4.mParameters.Length != 1 || mission4.mParameters[0] != "Goal1") Assert.Fail();
			if (mission4.mMissionId != "Miss001") Assert.Fail();
			if (mission4.mVehicleId != "Vehicle918") Assert.Fail();
			if (mission4.mPriority != 18) Assert.Fail();
			if (!string.IsNullOrEmpty(detail4)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp5, out IMission mission5, out string detail5) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission5 != null) Assert.Fail();
			if (detail5 != "UnknownMissionType") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp6, out IMission mission6, out string detail6) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission6 != null) Assert.Fail();
			if (detail6 != "LackOf\"Target\"Parameters") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp7, out IMission mission7, out string detail7) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission7 != null) Assert.Fail();
			if (detail7 != "Parameter\"Priority\"ValueError") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp8, out IMission mission8, out string detail8) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission8 != null) Assert.Fail();
			if (detail8 != "Parameter\"Priority\"ValueError") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp9, out IMission mission9, out string detail9) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission9 != null) Assert.Fail();
			if (detail9 != "DataSyntaxError") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp10, out IMission mission10, out string detail10) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission10 != null) Assert.Fail();
			if (detail10 != "DataSyntaxError") Assert.Fail();
		}

		[TestMethod()]
		public void ClassGotoPointMissionAnalyzer()
		{
			IMissionAnalyzer missionAnalyzer = Library.GetMissionAnalyzer(MissionType.GotoPoint);
			string tmp1 = "Mission=GotoPoint X=123 Y=456";
			string tmp2 = "Mission=GotoPoint X=123 Y=456 Head=270";
			string tmp3 = "Mission=GotoPoint X=123 Y=456 Head=270 MissionID=Miss009";
			string tmp4 = "Mission=GotoPoint X=123 Y=456 Head=270 MissionID=Miss009 VehicleID=Vehicle915";
			string tmp5 = "Mission=GotoPoint X=123 Y=456 Head=270 MissionID=Miss009 VehicleID=Vehicle915 Priority=3";
			string tmp6 = "X=123 Y=456 Head=270 MissionID=Miss009 VehicleID=Vehicle915 Priority=3"; // 缺少 Mission
			string tmp7 = "Mission=GotoPoint Y=456 Head=270 MissionID=Miss009 VehicleID=Vehicle915 Priority=3"; // 缺少 X
			string tmp8 = "Mission=GotoPoint X=123 Head=270 MissionID=Miss009 VehicleID=Vehicle915 Priority=3"; // 缺少 Y
			string tmp9 = "Mission=GotoPoint Head=270 MissionID=Miss009 VehicleID=Vehicle915 Priority=3"; // 缺少 X, Y
			string tmp10 = "Mission=GotoPoint X=AAA Y=BBB Head=CCC MissionID=Miss009 VehicleID=Vehicle915 Priority=DDD"; // 錯誤的 X, Y, Head, Priority
			string tmp11 = "Mission=GotoPoint X=123 Y=456 Head=270 MissionID=Miss009 VehicleID=Vehicle915 Priority=3 abcde"; // 錯誤的訊息格式
			string tmp12 = "Mission=GotoPoint X=123 X=789 Y=456 Head=270 MissionID=Miss009 VehicleID=Vehicle915 Priority=3"; // 重複的項目

			if (missionAnalyzer.TryParse(tmp1, out IMission mission1, out string detail1) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission1.mMissionType != MissionType.GotoPoint) Assert.Fail();
			if (mission1.mParameters.Length != 2 || mission1.mParameters[0] != "123" || mission1.mParameters[1] != "456") Assert.Fail();
			if (mission1.mMissionId != string.Empty) Assert.Fail();
			if (mission1.mVehicleId != string.Empty) Assert.Fail();
			if (mission1.mPriority != MissionAnalyzer.mPriorityDefault) Assert.Fail();
			if (!string.IsNullOrEmpty(detail1)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp2, out IMission mission2, out string detail2) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission2.mMissionType != MissionType.GotoPoint) Assert.Fail();
			if (mission2.mParameters.Length != 3 || mission2.mParameters[0] != "123" || mission2.mParameters[1] != "456" || mission2.mParameters[2] != "270") Assert.Fail();
			if (mission2.mMissionId != string.Empty) Assert.Fail();
			if (mission2.mVehicleId != string.Empty) Assert.Fail();
			if (mission2.mPriority != MissionAnalyzer.mPriorityDefault) Assert.Fail();
			if (!string.IsNullOrEmpty(detail2)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp3, out IMission mission3, out string detail3) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission3.mMissionType != MissionType.GotoPoint) Assert.Fail();
			if (mission3.mParameters.Length != 3 || mission3.mParameters[0] != "123" || mission3.mParameters[1] != "456" || mission3.mParameters[2] != "270") Assert.Fail();
			if (mission3.mMissionId != "Miss009") Assert.Fail();
			if (mission3.mVehicleId != string.Empty) Assert.Fail();
			if (mission3.mPriority != MissionAnalyzer.mPriorityDefault) Assert.Fail();
			if (!string.IsNullOrEmpty(detail3)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp4, out IMission mission4, out string detail4) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission4.mMissionType != MissionType.GotoPoint) Assert.Fail();
			if (mission4.mParameters.Length != 3 || mission4.mParameters[0] != "123" || mission4.mParameters[1] != "456" || mission4.mParameters[2] != "270") Assert.Fail();
			if (mission4.mMissionId != "Miss009") Assert.Fail();
			if (mission4.mVehicleId != "Vehicle915") Assert.Fail();
			if (mission4.mPriority != MissionAnalyzer.mPriorityDefault) Assert.Fail();
			if (!string.IsNullOrEmpty(detail4)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp5, out IMission mission5, out string detail5) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission5.mMissionType != MissionType.GotoPoint) Assert.Fail();
			if (mission5.mParameters.Length != 3 || mission5.mParameters[0] != "123" || mission5.mParameters[1] != "456" || mission5.mParameters[2] != "270") Assert.Fail();
			if (mission5.mMissionId != "Miss009") Assert.Fail();
			if (mission5.mVehicleId != "Vehicle915") Assert.Fail();
			if (mission5.mPriority != 3) Assert.Fail();
			if (!string.IsNullOrEmpty(detail5)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp6, out IMission mission6, out string detail6) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission6 != null) Assert.Fail();
			if (detail6 != "UnknownMissionType") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp7, out IMission mission7, out string detail7) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission7 != null) Assert.Fail();
			if (detail7 != "LackOf\"X\"Parameters") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp8, out IMission mission8, out string detail8) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission8 != null) Assert.Fail();
			if (detail8 != "LackOf\"Y\"Parameters") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp9, out IMission mission9, out string detail9) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission9 != null) Assert.Fail();
			if (detail9 != "LackOf\"X,Y\"Parameters") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp10, out IMission mission10, out string detail10) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission10 != null) Assert.Fail();
			if (detail10 != "Parameter\"X,Y,Head,Priority\"ValueError") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp11, out IMission mission11, out string detail11) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission11 != null) Assert.Fail();
			if (detail11 != "DataSyntaxError") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp12, out IMission mission12, out string detail12) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission12 != null) Assert.Fail();
			if (detail12 != "DataSyntaxError") Assert.Fail();
		}

		[TestMethod()]
		public void ClassDockMissionAnalyzer()
		{
			IMissionAnalyzer missionAnalyzer = Library.GetMissionAnalyzer(MissionType.Dock);
			string tmp1 = "Mission=Dock VehicleID=Vehicle863";
			string tmp2 = "Mission=Dock VehicleID=Vehicle863 MissionID=Miss123";
			string tmp3 = "Mission=Dock VehicleID=Vehicle863 MissionID=Miss123 Priority=3";
			string tmp4 = "VehicleID=Vehicle863 MissionID=Miss123 Priority=3"; // 缺少 Mission
			string tmp5 = "Mission=Dock MissionID=Miss123 Priority=3"; // 缺少 VehicleID
			string tmp6 = "Mission=Dock VehicleID=Vehicle863 MissionID=Miss123 Priority=DDD"; // 錯誤的 Priority
			string tmp7 = "Mission=Dock VehicleID=Vehicle863 MissionID=Miss123 Priority=3 abcde"; // 錯誤的訊息格式
			string tmp8 = "Mission=Dock VehicleID=Vehicle863 VehicleID=Vehicle773 MissionID=Miss123 Priority=3"; // 重複的項目

			if (missionAnalyzer.TryParse(tmp1, out IMission mission1, out string detail1) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission1.mMissionType != MissionType.Dock) Assert.Fail();
			if (mission1.mParameters != null) Assert.Fail();
			if (mission1.mMissionId != string.Empty) Assert.Fail();
			if (mission1.mVehicleId != "Vehicle863") Assert.Fail();
			if (mission1.mPriority != MissionAnalyzer.mPriorityDefault) Assert.Fail();
			if (!string.IsNullOrEmpty(detail1)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp2, out IMission mission2, out string detail2) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission2.mMissionType != MissionType.Dock) Assert.Fail();
			if (mission2.mParameters != null) Assert.Fail();
			if (mission2.mMissionId != "Miss123") Assert.Fail();
			if (mission2.mVehicleId != "Vehicle863") Assert.Fail();
			if (mission2.mPriority != MissionAnalyzer.mPriorityDefault) Assert.Fail();
			if (!string.IsNullOrEmpty(detail2)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp3, out IMission mission3, out string detail3) != MissionAnalyzeResult.Successed) Assert.Fail();
			if (mission3.mMissionType != MissionType.Dock) Assert.Fail();
			if (mission3.mParameters != null) Assert.Fail();
			if (mission3.mMissionId != "Miss123") Assert.Fail();
			if (mission3.mVehicleId != "Vehicle863") Assert.Fail();
			if (mission3.mPriority != 3) Assert.Fail();
			if (!string.IsNullOrEmpty(detail3)) Assert.Fail();

			if (missionAnalyzer.TryParse(tmp4, out IMission mission4, out string detail4) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission4 != null) Assert.Fail();
			if (detail4 != "UnknownMissionType") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp5, out IMission mission5, out string detail5) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission5 != null) Assert.Fail();
			if (detail5 != "LackOf\"VehicleID\"Parameters") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp6, out IMission mission6, out string detail6) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission6 != null) Assert.Fail();
			if (detail6 != "Parameter\"Priority\"ValueError") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp7, out IMission mission7, out string detail7) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission7 != null) Assert.Fail();
			if (detail7 != "DataSyntaxError") Assert.Fail();

			if (missionAnalyzer.TryParse(tmp8, out IMission mission8, out string detail8) != MissionAnalyzeResult.Failed) Assert.Fail();
			if (mission8 != null) Assert.Fail();
			if (detail8 != "DataSyntaxError") Assert.Fail();
		}
	}
}