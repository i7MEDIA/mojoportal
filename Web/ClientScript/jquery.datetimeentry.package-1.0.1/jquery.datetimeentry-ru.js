/* http://keith-wood.name/datetimeEntry.html
   Russian (UTF-8) initialisation for the jQuery date/time entry extension.
   Written by Andrew Stromnov (stromnov@gmail.com). */
(function($) {
	$.datetimeEntry.regional['ru'] = {datetimeFormat: 'D.O.Y H:M',
		monthNames: ['Январь','Февраль','Март','Апрель','Май','Июнь',
		'Июль','Август','Сентябрь','Октябрь','Ноябрь','Декабрь'],
		monthNamesShort: ['Янв','Фев','Мар','Апр','Май','Июн',
		'Июл','Авг','Сен','Окт','Ноя','Дек'],
		dayNames: ['воскресенье','понедельник','вторник','среда','четверг','пятница','суббота'],
		dayNamesShort: ['вск','пнд','втр','срд','чтв','птн','сбт'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Сейчас', 'Предыдущее поле', 'Следующее поле', 'Больше', 'Меньше'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['ru']);
})(jQuery);
