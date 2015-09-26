/* http://keith-wood.name/datetimeEntry.html
   Spanish initialisation for the jQuery date/time entry extension
   Written by diegok (diego@freekeylabs.com) and Vester (xvester@gmail.com). */
(function($) {
	$.datetimeEntry.regional['es'] = {datetimeFormat: 'D/O/Y H:M',
		monthNames: ['Enero','Febrero','Marzo','Abril','Mayo','Junio',
		'Julio','Agosto','Septiembre','Octubre','Noviembre','Diciembre'],
		monthNamesShort: ['Ene','Feb','Mar','Abr','May','Jun',
		'Jul','Ago','Sep','Oct','Nov','Dic'],
		dayNames: ['Domingo','Lunes','Martes','Miércoles','Jueves','Viernes','Sábado'],
		dayNamesShort: ['Dom','Lun','Mar','Mié','Juv','Vie','Sáb'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Ahora', 'Campo anterior', 'Siguiente campo', 'Aumentar', 'Disminuir'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['es']);
})(jQuery);
