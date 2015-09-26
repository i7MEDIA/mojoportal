/* http://keith-wood.name/datetimeEntry.html
   French initialisation for the jQuery date/time entry extension
   Written by Keith Wood (kbwood{at}iinet.com.au) September 2010. */
(function($) {
	$.datetimeEntry.regional['fr'] = {datetimeFormat: 'D/O/Y H:M',
		monthNames: ['Janvier','Février','Mars','Avril','Mai','Juin',
		'Juillet','Août','Septembre','Octobre','Novembre','Décembre'],
		monthNamesShort: ['Jan','Fév','Mar','Avr','Mai','Jun',
		'Jul','Aoû','Sep','Oct','Nov','Déc'],
		dayNames: ['Dimanche','Lundi','Mardi','Mercredi','Jeudi','Vendredi','Samedi'],
		dayNamesShort: ['Dim','Lun','Mar','Mer','Jeu','Ven','Sam'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Maintenant', 'Précédent', 'Suivant', 'Augmenter', 'Diminuer'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['fr']);
})(jQuery);
