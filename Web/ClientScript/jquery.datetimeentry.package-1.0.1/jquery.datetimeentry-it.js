/* http://keith-wood.name/datetimeEntry.html
   Italian initialisation for the jQuery date/time entry extension
   Written by Apaella (apaella@gmail.com). */
(function($) {
	$.datetimeEntry.regional['it'] = {datetimeFormat: 'D/O/Y H:M',
		monthNames: ['Gennaio','Febbraio','Marzo','Aprile','Maggio','Giugno',
		'Luglio','Agosto','Settembre','Ottobre','Novembre','Dicembre'],
		monthNamesShort: ['Gen','Feb','Mar','Apr','Mag','Giu',
		'Lug','Ago','Set','Ott','Nov','Dic'],
		dayNames: ['Domenica','Lunedì','Martedì','Mercoledì','Giovedì','Venerdì','Sabato'],
		dayNamesShort: ['Dom','Lun','Mar','Mer','Gio','Ven','Sab'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Adesso', 'Precedente', 'Successivo', 'Aumenta', 'Diminuisci'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['it']);
})(jQuery);
