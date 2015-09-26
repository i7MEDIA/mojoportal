/* http://keith-wood.name/datetimeEntry.html
   Hungarian initialisation for the jQuery date/time entry extension
   Written by Karaszi Istvan (raszi@spam.raszi.hu)  */
(function($) {
	$.datetimeEntry.regional['hu'] = {datetimeFormat: 'Y-O-D H:M',
		monthNames: ['Január', 'Február', 'Március', 'Április', 'Május', 'Június',
		'Július', 'Augusztus', 'Szeptember', 'Október', 'November', 'December'],
		monthNamesShort: ['Jan', 'Feb', 'Már', 'Ápr', 'Máj', 'Jún',
		'Júl', 'Aug', 'Szep', 'Okt', 'Nov', 'Dec'],
		dayNames: ['Vasámap', 'Hétfö', 'Kedd', 'Szerda', 'Csütörtök', 'Péntek', 'Szombat'],
		dayNamesShort: ['Vas', 'Hét', 'Ked', 'Sze', 'Csü', 'Pén', 'Szo'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Most', 'Előző mező', 'Következő mező', 'Növel', 'Csökkent'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['hu']);
})(jQuery);
