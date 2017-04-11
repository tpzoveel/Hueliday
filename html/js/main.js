$(document).ready(function() {
	$(".col-md-1").css('background-color', '#eee');


	// render the html for a single Schedule object
	var renderHtml = (s) => {
		return ("<span class=\"schedule\">Name: " + s.name + "</span></br> \
			<span class=\"schedule\">Description: " + s.description + "</span></br> \
			<span class=\"schedule\">Command: " + JSON.stringify(s.command) + "</span></br> \
			<span class=\"schedule\">Localtime: " + s.localtime + "</span></br> \
			");
	}

	// return all schedules as an array of schedule-objects
	var getSchedules = function(schedules, cfg)
	{
		$.getJSON(cfg.url+cfg.schedules, function(allSchedules, textStatus) {
			$.each(allSchedules, function(index, val) {
				schedules.push(val);
			});
		});
	}

	// main function
	var main = function() {
		var schedules = [];
		var htmlLines = [];
		var config = {
			url			: "http://192.168.2.12/api/newdeveloper/",
			prefix		: "HH_",
			schedules 	: "schedules"
		};

		getSchedules(schedules, config);

		for (var schedule in schedules)
		{
			htmlLines.push("<li>" + renderHtml(schedule) + "</li>");
		}
		$("ul").append(htmlLines.join(""));

	}();

});
