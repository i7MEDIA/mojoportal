/* http://keith-wood.name/calendars.html
   Brazilian Portuguese localisation for calendars datepicker for jQuery.
   Written by Leonildo Costa Silva (leocsilva@gmail.com). */
(function($) {
	$.calendars.picker.regional['pt-BR'] = {
		renderer: $.calendars.picker.defaultRenderer,
		prevText: '&#x3c;Anterior', prevStatus: '',
		prevJumpText: '&#x3c;&#x3c;', prevJumpStatus: '',
		nextText: 'Pr&oacute;ximo&#x3e;', nextStatus: '',
		nextJumpText: '&#x3e;&#x3e;', nextJumpStatus: '',
		currentText: 'Hoje', currentStatus: '',
		todayText: 'Hoje', todayStatus: '',
		clearText: 'Limpar', clearStatus: '',
		closeText: 'Fechar', closeStatus: '',
		yearStatus: '', monthStatus: '',
		weekText: 'Sm', weekStatus: '',
		dayStatus: 'DD, M d', defaultStatus: '',
		isRTL: false
	};
	$.calendars.picker.setDefaults($.calendars.picker.regional['pt-BR']);
})(jQuery);
