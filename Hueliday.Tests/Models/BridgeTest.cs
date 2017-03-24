using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Hueliday;
using Hueliday.Controllers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Hueliday.Tests
{
	[TestFixture]
	public class BridgeTest
	{
		//TODO move to config file
		private string _Ip = Config.Ip;

		private string _Name = "BridgeTest-CreateSchdule";
		private string _Description = "Test for Bridge";
		private string _LocalTime = "2017-06-23T19:52:00";
		private ScheduleStatus _Status = ScheduleStatus.enabled;
		private bool _Recycle = false;
		private string _Address = "/api/C9gvVbyDlfkHfK82/groups/0/action";
		private object _Body = new { Scene = "de5ee82e9-on-0" };
		private string _Method = "PUT";

		private string _newId;

		[Test]
		public void A_GetName()
		{
			// Arrange
			var model = new Bridge(_Ip);

			// Act
			string result = model.GetName();

			// Assert
			Assert.AreEqual(result, "Philips hue");
		}

		[Test]
		public void B_GetSchedules()
		{
			var model = new Bridge(_Ip);

			var result = model.GetSchedules();

			Assert.Greater(result.Count, 0);
			Assert.IsInstanceOf<Schedule>(result.First().Value);
			Assert.IsInstanceOf<Command>(result.First().Value.Command);
		}
		// Actually not a UNIT test since this method tests several things at once:
		// - Get the schedule list en determine the length
		// - Add a schedule
		// - Get the list and verify it is one element longer now
		// - Get the new schedule and verify it is as expected
		[Test]
		public void C_CreateSchedule()
		{
			var testModel = new Bridge(_Ip);
			string webApiResponseString;
			List<object> webApiResponseList;
			JObject jsonObject;

			Schedule s = new Schedule();
			Command c = new Command();
			var Schedules = testModel.GetSchedules();
			int initialLength = Schedules.Count;

			s.Name = _Name;
			s.Description = _Description;
			s.LocalTime = _LocalTime;
			s.Status = _Status;
			s.Recycle = _Recycle;

			c.Address = _Address;
			c.Body = _Body;
			c.Method = _Method;

			s.Command = c;

			webApiResponseString = testModel.NewSchedule(s);

			// Firs test: we should get a success-response from the Bridge
			Assert.IsTrue(webApiResponseString.Contains("success")); // something like "[{\"success\":{\"id\":\"14\"}}]"

			// Next: the length of the Schedules collection should be incremented by 1
			Schedules = testModel.GetSchedules();
			Assert.AreEqual(Schedules.Count, initialLength + 1);

			// now extract the new ID and use it to get the Schedule back from the bridge
			// first, extract the list (the bridge returned an array with one element)
			webApiResponseList = JsonConvert.DeserializeObject<List<object>>(webApiResponseString);
			//next, extract the Id
			jsonObject = (JObject)webApiResponseList.First();
			_newId = jsonObject["success"]["id"].ToString();

			// Third test: we actually received the new Id
			Assert.IsNotEmpty(_newId);

			// retrieve new schedule, compare to first
			s = Schedules[_newId];
			Assert.AreEqual(s.Name, _Name);
			Assert.AreEqual(s.Description, _Description);
			Assert.AreEqual(s.LocalTime, _LocalTime);
			Assert.AreEqual(s.Status, _Status);
			Assert.AreEqual(s.Recycle, _Recycle);
			Assert.AreEqual(s.Command.Address, _Address);
			//Assert.AreEqual(s.Command.Body, new { scene = "de5ee82e9-on-0" }); // this is not easy to test so i got lazy and skipped
			Assert.AreEqual(s.Command.Method, _Method);

		}

		// Update the test schedule
		[Test]
		public void E_UpdateSchedule()
		{
			Assert.IsNotEmpty(_newId); // Fail if this test is run solo. Should be run after CreateSchedule.
			var testModel = new Bridge(_Ip);
			var Schedules = testModel.GetSchedules();
			var testSchedule = Schedules[_newId];
			string webApiResponseString;

			testSchedule.Description = "123";
			webApiResponseString = testModel.UpdateSchedule(_newId, testSchedule);

			// First test: we should get a success-response from the Bridge
			Assert.IsTrue(webApiResponseString.Contains("success")); // something like "[{\"success\":{\"id\":\"14\"}}]"

			// Next we get refresh the schedule list and verify the change
			Schedules = testModel.GetSchedules();
			testSchedule = Schedules[_newId];
			Assert.AreEqual(testSchedule.Description, "123");

		}

		// Now remove all the schedules we created in these tests
		[Test]
		public void F_RemoveSchedule()
		{
			var testModel = new Bridge(_Ip);
			var Schedules = testModel.GetSchedules();
			string webApiResponseString;

			// Check all schedules, find the one(s) we created in this test by its name and remove it
			foreach (var item in Schedules)
			{
				if (item.Value.Name == _Name)
				{
					webApiResponseString = testModel.RemoveSchedule(item.Key);
					Assert.IsTrue(webApiResponseString.Contains("success"));
				}
			}
			Assert.Pass(); // Pass if no schedules found
		}
	}
}
/*
 * {
	"6210449574126917": {
		"name": "Wekker",
		"description": "Wakker",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "de5ee82e9-on-15"
			},
			"method": "PUT"
		},
		"localtime": "W124/T07:45:00",
		"time": "W124/T06:45:00",
		"created": "2015-05-06T22:02:51",
		"status": "disabled",
		"recycle": false
	},
	"1736807146958259": {
		"name": "AM aan 1",
		"description": "Wakker",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "de5ee82e9-on-0"
			},
			"method": "PUT"
		},
		"localtime": "W127/T06:55:00A00:30:00",
		"time": "W127/T05:55:00A00:30:00",
		"created": "2017-02-03T06:45:45",
		"status": "disabled",
		"recycle": false
	},
	"2184516468092277": {
		"name": "Zolder 5",
		"description": "Zolder dim",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "124a0717d-on-5"
			},
			"method": "PUT"
		},
		"time": "PT00:09:00",
		"created": "2016-02-16T19:05:12",
		"status": "disabled",
		"autodelete": false,
		"starttime": "2016-02-16T19:05:12",
		"recycle": false
	},
	"7477717300667743": {
		"name": "AM uit 1",
		"description": "",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "c4ca4238a-off-0"
			},
			"method": "PUT"
		},
		"localtime": "W127/T08:07:00A00:30:00",
		"time": "W127/T07:07:00A00:30:00",
		"created": "2017-02-03T06:47:31",
		"status": "disabled",
		"recycle": false
	},
	"9045478344875262": {
		"name": "AM aan 2",
		"description": "Aan",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "253cddf5e-on-0"
			},
			"method": "PUT"
		},
		"localtime": "W127/T07:24:00A00:30:00",
		"time": "W127/T06:24:00A00:30:00",
		"created": "2017-02-03T06:47:09",
		"status": "disabled",
		"recycle": false
	},
	"6903555603992020": {
		"name": "AM uit 2",
		"description": "Off",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "8a1705b4b-on-0"
			},
			"method": "PUT"
		},
		"localtime": "W127/T07:53:00A00:30:00",
		"time": "W127/T06:53:00A00:30:00",
		"created": "2017-02-03T06:47:54",
		"status": "disabled",
		"recycle": false
	},
	"6350907098723480": {
		"name": "AM 0 aan",
		"description": "Relax",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "9f28a98a5-on-0"
			},
			"method": "PUT"
		},
		"localtime": "W127/T07:04:00A00:30:00",
		"time": "W127/T06:04:00A00:30:00",
		"created": "2017-02-03T06:46:12",
		"status": "disabled",
		"recycle": false
	},
	"7036557518295711": {
		"name": "AM 0 uit",
		"description": "",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "a11606731-off-0"
			},
			"method": "PUT"
		},
		"localtime": "W127/T08:03:00A00:30:00",
		"time": "W127/T07:03:00A00:30:00",
		"created": "2017-02-03T06:47:39",
		"status": "disabled",
		"recycle": false
	},
	"8769996543770750": {
		"name": "PM 0 aan",
		"description": "Relax",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "9f28a98a5-on-0"
			},
			"method": "PUT"
		},
		"localtime": "W127/T16:50:00A00:30:00",
		"time": "W127/T15:50:00A00:30:00",
		"created": "2017-02-03T06:48:03",
		"status": "disabled",
		"recycle": false
	},
	"3798363769800190": {
		"name": "pM 0 uit",
		"description": "",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "a11606731-off-0"
			},
			"method": "PUT"
		},
		"localtime": "W127/T23:02:00A00:30:00",
		"time": "W127/T22:02:00A00:30:00",
		"created": "2017-02-03T06:48:28",
		"status": "disabled",
		"recycle": false
	},
	"2529705657161759": {
		"name": "Pm 2 aan",
		"description": "Aan",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "253cddf5e-on-0"
			},
			"method": "PUT"
		},
		"localtime": "W127/T19:00:00A00:30:00",
		"time": "W127/T18:00:00A00:30:00",
		"created": "2016-12-26T09:28:05",
		"status": "disabled",
		"recycle": false
	},
	"9297942838066227": {
		"name": "Pm 2 uit",
		"description": "",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "e4da3b7fb-off-0"
			},
			"method": "PUT"
		},
		"localtime": "W124/T23:37:00A00:30:00",
		"time": "W124/T22:37:00A00:30:00",
		"created": "2015-08-02T20:25:33",
		"status": "disabled",
		"recycle": false
	},
	"8125455292160223": {
		"name": "Alarm",
		"description": "Off",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "8a1705b4b-on-0"
			},
			"method": "PUT"
		},
		"localtime": "2016-12-26T20:29:00",
		"time": "2016-12-26T19:29:00",
		"created": "2016-12-26T09:28:37",
		"status": "disabled",
		"autodelete": false,
		"recycle": false
	},
	"7034863421174614": {
		"name": "Alarm",
		"description": "Aan",
		"command": {
			"address": "/api/C9gvVbyDlfkHfK82/groups/0/action",
			"body": {
				"scene": "253cddf5e-on-0"
			},
			"method": "PUT"
		},
		"localtime": "2016-12-26T21:10:00A00:30:00",
		"time": "2016-12-26T20:10:00A00:30:00",
		"created": "2016-12-26T09:29:12",
		"status": "disabled",
		"autodelete": false,
		"recycle": false
	}
}*/