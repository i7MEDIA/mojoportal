/* http://keith-wood.name/calendars.html
   Korean localisation for calendars datepicker for jQuery.
   Written by DaeKwon Kang (ncrash.dk@gmail.com). */
(function($) {
	$.calendars.picker.regional['ko'] = {
		renderer: $.extend({}, $.calendars.picker.defaultRenderer,
			{month: $.calendars.picker.defaultRenderer.month.
				replace(/monthHeader/, 'monthHeader:MM yyyy년')}),
		prevText: '이전달', prevStatus: '',
		prevJumpText: '&#x3c;&#x3c;', prevJumpStatus: '',
		nextText: '다음달', nextStatus: '',
		nextJumpText: '&#x3e;&#x3e;', nextJumpStatus: '',
		currentText: '오늘', currentStatus: '',
		todayText: '오늘', todayStatus: '',
		clearText: '지우기', clearStatus: '',
		closeText: '닫기', closeStatus: '',
		yearStatus: '', monthStatus: '',
		weekText: 'Wk', weekStatus: '',
		dayStatus: 'DD, M d', defaultStatus: '',
		isRTL: false
	};
	$.calendars.picker.setDefaults($.calendars.picker.regional['ko']);
})(jQuery);
