using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Interface;

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
	}
}