/* http://keith-wood.name/datetimeEntry.html
   Slovak initialisation for the jQuery date/time entry extension
   Written by Vojtech Rinik (vojto@hmm.sk)  */
(function($) {
	$.datetimeEntry.regional['sk'] = {datetimeFormat: 'D.O.Y H:Ma',
		monthNames: ['Január','Február','Marec','Apríl','Máj','Jún',
		'Júl','August','September','Október','November','December'],
		monthNamesShort: ['Jan','Feb','Mar','Apr','Máj','Jún',
		'Júl','Aug','Sep','Okt','Nov','Dec'],
		dayNames: ['Nedel\'a','Pondelok','Utorok','Streda','Štvrtok','Piatok','Sobota'],
		dayNamesShort: ['Ned','Pon','Uto','Str','Štv','Pia','Sob'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Teraz', 'Predchádzajúce pole', 'Nasledujúce pole', 'Zvýšiť', 'Znížiť'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['sk']);
})(jQuery);
