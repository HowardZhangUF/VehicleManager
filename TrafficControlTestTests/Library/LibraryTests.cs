using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrafficControlTest.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;

namespace TrafficControlTest.Library.Tests
{
	[TestClass()]
	public class LibraryTests
	{
		[TestMethod()]
		public void GetDistanceTest()
		{
			IPoint2D point1 = Library.GenerateIPoint2D(10, 10);
			IPoint2D point2 = Library.GenerateIPoint2D(30, 30);
			int distanceSquare = Library.GetDistanceSquare(point1, point2);
			double distance = Math.Sqrt(distanceSquare);

			if (distance.ToString("F2") != "28.28") Assert.Fail();
			if (distanceSquare != 800) Assert.Fail();
		}
	}
}