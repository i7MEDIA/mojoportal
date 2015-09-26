/* http://keith-wood.name/calendars.html
   Korean localisation for Gregorian/Julian calendars for jQuery.
   Written by DaeKwon Kang (ncrash.dk@gmail.com). */
(function($) {
	$.calendars.calendars.gregorian.prototype.regional['ko'] = {
		name: 'Gregorian',
		epochs: ['BCE', 'CE'],
		monthNames: ['1월(JAN)','2월(FEB)','3월(MAR)','4월(APR)','5월(MAY)','6월(JUN)',
		'7월(JUL)','8월(AUG)','9월(SEP)','10월(OCT)','11월(NOV)','12월(DEC)'],
		monthNamesShort: ['1월(JAN)','2월(FEB)','3월(MAR)','4월(APR)','5월(MAY)','6월(JUN)',
		'7월(JUL)','8월(AUG)','9월(SEP)','10월(OCT)','11월(NOV)','12월(DEC)'],
		dayNames: ['일','월','화','수','목','금','토'],
		dayNamesShort: ['일','월','화','수','목','금','토'],
		dayNamesMin: ['일','월','화','수','목','금','토'],
		dateFormat: 'yyyy-mm-dd',
		firstDay: 0,
		isRTL: false
	};
	if ($.calendars.calendars.julian) {
		$.calendars.calendars.julian.prototype.regional['ko'] =
			$.calendars.calendars.gregorian.prototype.regional['ko'];
	}
})(jQuery);
