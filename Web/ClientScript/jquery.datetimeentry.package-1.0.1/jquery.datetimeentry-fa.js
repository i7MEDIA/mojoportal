/* http://keith-wood.name/datetimeEntry.html
   Persian (Farsi) initialisation for the jQuery date/time entry extension
   Written by benyblack and Javad Mowlanezhad (jmowla@gmail.com). */
(function($) {
	$.datetimeEntry.regional['fa'] = {datetimeFormat: 'Y/O/D H:M',
		monthNames: ['فروردين','ارديبهشت','خرداد','تير','مرداد','شهريور',
		'مهر','آبان','آذر','دي','بهمن','اسفند'],
		monthNamesShort: ['1','2','3','4','5','6',
		'7','8','9','10','11','12'],
		dayNames: ['يکشنبه','دوشنبه','سه‌شنبه','چهارشنبه','پنجشنبه','جمعه','شنبه'],
		dayNamesShort: ['ي','د','س','چ','پ','ج', 'ش'],
		ampmNames: ['ق.ظ', 'ب.ظ'],
		spinnerTexts: ['اکنون', 'قبلی', 'بعدی', 'افزایش', 'کاهش'],
		isRTL: true};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['fa']);
})(jQuery);
