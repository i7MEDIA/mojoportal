/* http://keith-wood.name/datetimeEntry.html
   Polish initialisation for the jQuery date/time entry extension. 
   Polish translation by Jacek Wysocki (jacek.wysocki@gmail.com). */
(function($) {
	$.datetimeEntry.regional['pl'] = {datetimeFormat: 'Y-O-D H:M',
		monthNames: ['Styczeń','Luty','Marzec','Kwiecień','Maj','Czerwiec',
		'Lipiec','Sierpień','Wrzesień','Październik','Listopad','Grudzień'],
		monthNamesShort: ['Sty','Lut','Mar','Kwi','Maj','Cze',
		'Lip','Sie','Wrz','Paź','Lis','Gru'],
		dayNames: ['Niedziela','Poniedzialek','Wtorek','Środa','Czwartek','Piątek','Sobota'],
		dayNamesShort: ['Nie','Pon','Wto','Śro','Czw','Pią','Sob'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Teraz', 'Poprzednie pole', 'Następne pole', 'Zwiększ wartość', 'Zmniejsz wartość'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['pl']);
})(jQuery);
