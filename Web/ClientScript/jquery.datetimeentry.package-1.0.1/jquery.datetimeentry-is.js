/* http://keith-wood.name/datetimeEntry.html
   Icelandic initialisation for the jQuery date/time entry extension
   Written by Már Örlygsson (http://mar.anomy.net/) */
(function($) {
	$.datetimeEntry.regional['is'] = {datetimeFormat: 'D.O.Y H:M',
		monthNames: ['janúar','febrúar','mars','apríl','maí','júní',
		'júlí','ágúst','september','október','nóvember','desember'],
		monthNamesShort: ['jan','feb','mar','apr','maí','jún',
		'júl','ágú','sep','okt','nóv','des'],
		dayNames: ['sunnudagur','mánudagur','þriðjudagur','miðvikudagur','fimmtudagur','föstudagur','laugardagur'],
		dayNamesShort: ['sun','mán','þri','mið','fim','fös','lau'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Dagurinn í dag', 'Fyrra svæði', 'Næsta svæði', 'Hækka', 'Lækka'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['is']);
})(jQuery);
