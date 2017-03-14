using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hueliday
{
	public class Schedule
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Command Command { get; set; }
		public string LocalTime { get; set; }
		[JsonIgnore]
		public string Time { get; set; } //deprecated
		[JsonIgnore]
		public string Created { get; set; }
		public string Status { get; set; }
		[JsonIgnore]
		public bool Recycle { get; set; }

        public (DateTime time, TimeSpan random, List<DayOfWeek> days) GetLocalTime()
        {
            string timeString, randomString; //"W127/T06:55:00A00:30:00" 
            int daysMask;
            var days = new List<DayOfWeek>();

            var restString = LocalTime;
            string timePart = null;
            string randomPart = null;
            var separatorIndex = LocalTime.IndexOf('/');
            if (separatorIndex > -1) // slash separates weekdays from time
            {
                var strings = LocalTime.Split('/');
                var daysString = strings[0].TrimStart(new char['W']);
                restString = strings[1];
                if (int.TryParse(daysString, out daysMask))
                {
                    if ((daysMask & 64) == 64) days.Add(DayOfWeek.Monday);
                    if ((daysMask & 32) == 32) days.Add(DayOfWeek.Tuesday);
                    if ((daysMask & 16) == 16) days.Add(DayOfWeek.Wednesday);
                    if ((daysMask & 8) == 8) days.Add(DayOfWeek.Thursday);
                    if ((daysMask & 4) == 4) days.Add(DayOfWeek.Friday);
                    if ((daysMask & 2) == 2) days.Add(DayOfWeek.Saturday);
                    if ((daysMask & 1) == 1) days.Add(DayOfWeek.Sunday);
                }
            }

            // now parse the second part which is the time of day, appended with optional randomize time
            restString = restString.TrimStart(new char['T']);
            separatorIndex = restString.IndexOf('A');
            if (separatorIndex > -1)
            {
                var strings = restString.Split('A');
                timePart = strings[0];
                randomPart = strings[1];
            }
            else
                timePart = restString;

            return (DateTime.Parse(timePart), TimeSpan.Parse(randomPart), days);
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