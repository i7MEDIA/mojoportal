/* http://keith-wood.name/datetimeEntry.html
   Portuguese initialisation for the jQuery date/time entry extension
   Written by Dino Sane (dino@asttra.com.br) and Leonildo Costa Silva (leocsilva@gmail.com). */
(function($) {
	$.datetimeEntry.regional['pt'] = {datetimeFormat: 'D/O/Y H:M',
		monthNames: ['Janeiro','Fevereiro','Março','Abril','Maio','Junho',
		'Julho','Agosto','Setembro','Outubro','Novembro','Dezembro'],
		monthNamesShort: ['Jan','Fev','Mar','Abr','Mai','Jun',
		'Jul','Ago','Set','Out','Nov','Dez'],
		dayNames: ['Domingo','Segunda-feira','Terça-feira',
		'Quarta-feira','Quinta-feira','Sexta-feira','Sábado'],
		dayNamesShort: ['Dom','Seg','Ter','Qua','Qui','Sex','Sáb'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Agora', 'Campo anterior', 'Campo Seguinte', 'Aumentar', 'Diminuir'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['pt']);
})(jQuery);
