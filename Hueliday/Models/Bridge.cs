
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hueliday
{
	public class Bridge
	{
		public string IpAddress { get; set; }

		public Bridge(string ip)
		{
			IpAddress = ip;
		}

		// Return the name of the Hue Bridge
		// String.Empty on failure
		public string GetName()
		{
			string Uri = MakeUri("/config"),
			key = "name";
			JObject configAttributes = GetObjectsFromWebApi(Uri);
			if (configAttributes == null)
				return String.Empty;
			return (string)configAttributes[key];
		}

		// Get the schedules from the Hue Bridge, keyed by their ID
		// or null on failure
		public Dictionary<string, Schedule> GetSchedules()
		{
			string jsonString;
			string Uri = MakeUri("/schedules");
			jsonString = WebApi.Get(Uri);
			if (String.IsNullOrEmpty(jsonString))
				return null;

			return JsonConvert.DeserializeObject<Dictionary<string, Schedule>>(jsonString);
		}

		// Add a new Schedule to Hue Bridge
		// Return the message from the bridge, or null if something is wrong
		public string NewSchedule(Schedule newSchedule)
		{
			string jsonString, Uri = MakeUri("/schedules");
			var settings = new JsonSerializerSettings();
			settings.ContractResolver = new LowercaseContractResolver();
			jsonString = JsonConvert.SerializeObject(newSchedule, Formatting.Indented, settings);

			if (String.IsNullOrEmpty(jsonString))
				return string.Empty;

			return WebApi.Post(Uri, jsonString);
		}

		// Update the schedule on the Hue Bridge, identified by id
		// Return the message from the bridge, or null if something is wrong
		public string UpdateSchedule(string id, Schedule updatedSchedule)
		{
			string result = "", jsonString, Uri = MakeUri($"/schedules/{id}");
			var settings = new JsonSerializerSettings();
			settings.ContractResolver = new LowercaseContractResolver();
			jsonString = JsonConvert.SerializeObject(updatedSchedule, Formatting.Indented, settings);

			result = WebApi.Put(Uri, jsonString);

			return result;
		}

		// Remove the schedule from the Hue Bridge, identified by id
		// Return the message from the bridge
		public string RemoveSchedule(string id)
		{
			string Uri = MakeUri($"/schedules/{id}");
			return WebApi.Delete(Uri, "");
		}

		// Get some info from the Hue Bridge and return as parsed JObject
		private JObject GetObjectsFromWebApi(string uri) => JObject.Parse(WebApi.Get(uri));

		private string MakeUri(string resource) => "http://" + IpAddress + Config.BaseUrl + resource;
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