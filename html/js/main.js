// the HueBridge object holding all Hue functions
var HueBridge = (function () {
	// return all schedules as an array of schedule-objects
	var getSchedules = (cfg, callback) => {
		$.getJSON(cfg.url+cfg.schedules, function(allSchedules, textStatus) {
				callback(allSchedules);
		});
	};
	
	// render the html for a single Schedule object
	var renderHtml = (s) => {
		// var spans = [];
		// var span = document.createElement("span");
		// span.appendChild(document.createTextNode("Name: " + s.name));
		// spans.push(span);
		return ("<span class=\"schedule\">Name: " + s.name + "</span></br> \
			<span class=\"schedule\">Description: " + s.description + "</span></br> \
			<span class=\"schedule\">Command: " + JSON.stringify(s.command) + "</span></br> \
			<span class=\"schedule\" name=\"localtime\">Localtime: " + s.localtime + "</span></br> \
			");
	};

	var config = {
		url			: "http://192.168.2.12/api/newdeveloper/",
		prefix		: "", //HH_",
		schedules 	: "schedules"
	}
	return {
		getSchedules	: getSchedules,
		renderHtml		: renderHtml,
		config			: config
	}
})();

$(document).ready(function() {
	$(".col-md-1").css('background-color', '#eee');

	$("button[name='ok']").click(() => {
		alert("!!"+document.getElementById("schedules").getElementsByTagName("li")[0].getElementsByTagName("span")["localtime"].textContent)
	});

	// main function
	var main = function() {

		// get schedules from Hue, renderHTML each schedule if it's a Holiday schedule
		HueBridge.getSchedules(HueBridge.config, (sch) => {
			var htmlLines = [];
			$.each(sch, function (id, val) {
				// only holiday schedules, based on prefix in the name
				if (val.name.startsWith(HueBridge.config.prefix))
					$("ul").append("<li id=\"" + id + "\">" + HueBridge.renderHtml(val) + "</li>");
			});
		});
	}();
});
