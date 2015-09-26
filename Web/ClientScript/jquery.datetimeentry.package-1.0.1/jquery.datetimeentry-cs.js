/* http://keith-wood.name/datetimeEntry.html
   Czech initialisation for the jQuery date/time entry extension
   Written by Stanislav Kurinec (stenly.kurinec@gmail.com) and Tomas Muller (tomas@tomas-muller.net) */
(function($) {
	$.datetimeEntry.regional['cs'] = {datetimeFormat: 'D.O.Y H:M',
		monthNames: ['leden','únor','březen','duben','květen','červen',
		'červenec','srpen','září','říjen','listopad','prosinec'],
		monthNamesShort: ['led','úno','bře','dub','kvě','čer',
		'čvc','srp','zář','říj','lis','pro'],
		dayNames: ['neděle', 'pondělí', 'úterý', 'středa', 'čtvrtek', 'pátek', 'sobota'],
		dayNamesShort: ['ne', 'po', 'út', 'st', 'čt', 'pá', 'so'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Nyní', 'Předchozí pole', 'Následující pole', 'Zvýšit', 'Snížit'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['cs']);
})(jQuery);
