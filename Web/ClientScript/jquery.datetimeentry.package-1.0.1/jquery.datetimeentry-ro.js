/* http://keith-wood.name/datetimeEntry.html
   Romanian initialisation for the jQuery date/time entry extension
   Written by Edmond L. (ll_edmond@walla.com)  */
(function($) {
	$.datetimeEntry.regional['ro'] = {datetimeFormat: 'O/D/Y H:M',
		monthNames: ['Ianuarie','Februarie','Martie','Aprilie','Mai','Iunie',
		'Iulie','August','Septembrie','Octobrie','Noiembrie','Decembrie'],
		monthNamesShort: ['Ian', 'Feb', 'Mar', 'Apr', 'Mai', 'Iun',
		'Iul', 'Aug', 'Sep', 'Oct', 'Noi', 'Dec'],
		dayNames: ['Duminică', 'Luni', 'Marti', 'Miercuri', 'Joi', 'Vineri', 'Sâmbătă'],
		dayNamesShort: ['Dum', 'Lun', 'Mar', 'Mie', 'Joi', 'Vin', 'Sâm'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Acum', 'Campul Anterior', 'Campul Urmator', 'Mareste', 'Micsoreaza'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['ro']);
})(jQuery);
