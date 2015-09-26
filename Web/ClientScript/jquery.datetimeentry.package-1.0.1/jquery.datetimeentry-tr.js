/* http://keith-wood.name/datetimeEntry.html
   Turkish initialisation for the jQuery date/time entry extension
   Written by Vural Dinçer and Izzet Emre Erkan (kara@karalamalar.net) */
(function($) {
	$.datetimeEntry.regional['tr'] = {datetimeFormat: 'D.O.Y H:M',
		monthNames: ['Ocak','Şubat','Mart','Nisan','Mayıs','Haziran',
		'Temmuz','Ağustos','Eylül','Ekim','Kasım','Aralık'],
		monthNamesShort: ['Oca','Şub','Mar','Nis','May','Haz',
		'Tem','Ağu','Eyl','Eki','Kas','Ara'],
		dayNames: ['Pazar','Pazartesi','Salı','Çarşamba','Perşembe','Cuma','Cumartesi'],
		dayNamesShort: ['Pz','Pt','Sa','Ça','Pe','Cu','Ct'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['şu an', 'önceki alan', 'sonraki alan', 'arttır', 'azalt'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['tr']);
})(jQuery);
