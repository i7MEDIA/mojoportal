/* http://keith-wood.name/datetimeEntry.html
   Dutch initialisation written for the jQuery date/time entry extension.
   Glenn plas (glenn.plas@telenet.be) and Mathias Bynens (http://mathiasbynens.be/). */
(function($) {
	$.datetimeEntry.regional['nl'] = {datevFormat: 'D/O/Y H:M',
		monthNames: ['januari', 'februari', 'maart', 'april', 'mei', 'juni',
		'juli', 'augustus', 'september', 'oktober', 'november', 'december'],
		monthNamesShort: ['jan', 'feb', 'maa', 'apr', 'mei', 'jun',
		'jul', 'aug', 'sep', 'okt', 'nov', 'dec'],
		dayNames: ['zondag', 'maandag', 'dinsdag', 'woensdag', 'donderdag', 'vrijdag', 'zaterdag'],
		dayNamesShort: ['zon', 'maa', 'din', 'woe', 'don', 'vri', 'zat'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Nu', 'Vorig veld', 'Volgend veld','Verhoog', 'Verlaag'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['nl']);
})(jQuery);
