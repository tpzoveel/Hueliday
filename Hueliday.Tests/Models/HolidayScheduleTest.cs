using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Hueliday;
using Hueliday.Controllers;

namespace Hueliday.Tests
{
	[TestFixture]
	public class HolidayScheduleTest
	{
		[Test]
		public void SetName()
		{
			var testName = "123-321";
			var testSchedule = new HolidaySchedule();
			var result = testSchedule.SetName(testName);
			Assert.AreEqual(Config.NamePrefix + testName, result);
		}
	}
}
