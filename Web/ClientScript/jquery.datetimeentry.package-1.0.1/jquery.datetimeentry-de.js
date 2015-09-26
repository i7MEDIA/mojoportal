/* http://keith-wood.name/datetimeEntry.html
   German initialisation for the jQuery date/time entry extension
   Written by Eyk Schulz (eyk.schulz@gmx.net) and Milian Wolff (mail@milianw.de) */
(function($) {
	$.datetimeEntry.regional['de'] = {datetimeFormat: 'D.O.Y H:M',
		monthNames: ['Januar','Februar','März','April','Mai','Juni',
		'Juli','August','September','Oktober','November','Dezember'],
		monthNamesShort: ['Jan','Feb','Mär','Apr','Mai','Jun',
		'Jul','Aug','Sep','Okt','Nov','Dez'],
		dayNames: ['Sonntag','Montag','Dienstag','Mittwoch','Donnerstag','Freitag','Samstag'],
		dayNamesShort: ['So','Mo','Di','Mi','Do','Fr','Sa'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Jetzt', 'vorheriges Feld', 'nächstes Feld', 'hoch', 'runter'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['de']);
})(jQuery);
