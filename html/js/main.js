// the HueBridge object holding all Hue functions
var HueBridge = (function () {
	// return all schedules as an array of schedule-objects
	var getSchedules = (cfg, callback) => {
		$.getJSON(cfg.url+cfg.schedules, function(allSchedules, textStatus) {
			var schedules = [];
			$.each(allSchedules, function(index, val) {
				schedules.push(val);
			});
			if (typeof callback == 'function')
				callback(schedules);
		});
	};
	
	// render the html for a single Schedule object
	var renderHtml = (s) => {
		return ("<span class=\"schedule\">Name: " + s.name + "</span></br> \
			<span class=\"schedule\">Description: " + s.description + "</span></br> \
			<span class=\"schedule\">Command: " + JSON.stringify(s.command) + "</span></br> \
			<span class=\"schedule\">Localtime: " + s.localtime + "</span></br> \
			");
	};

	var config = {
		url			: "http://192.168.2.12/api/newdeveloper/",
		prefix		: "HH_",
		schedules 	: "schedules"
	}
	return {
		getSchedules	: getSchedules,
		renderHtml		: renderHtml,
		config			:  config
	}
})();

$(document).ready(function() {
	$(".col-md-1").css('background-color', '#eee');

	// main function
	var main = function() {

		// get schedules from Hue, renderHTML each schedule if it's a Holiday schedule
		HueBridge.getSchedules(HueBridge.config, (sch) => {
			var htmlLines = [];
			$.each(sch, function (index, val) {
				// only holiday schedules
				if (val.name.startsWith(HueBridge.config.prefix))
					htmlLines.push("<li>" + HueBridge.renderHtml(val) + "</li>");
			});
			$("ul").append(htmlLines.join(""));
		});
	}();
});
