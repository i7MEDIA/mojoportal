/* http://keith-wood.name/datetimeEntry.html
   Vietnamese initialisation for the jQuery date/time entry extension
   Written by Le Thanh Huy (lthanhhuy@cit.ctu.edu.vn)  */
(function($) {
	$.datetimeEntry.regional['vi'] = {datetimeFormat: 'D/O/Y H:Ma',
		monthNames: ['Tháng Một', 'Tháng Hai', 'Tháng Ba', 'Tháng Tư', 'Tháng Năm', 'Tháng Sáu',
		'Tháng Bảy', 'Tháng Tám', 'Tháng Chín', 'Tháng Mười', 'Tháng Mười Một', 'Tháng Mười Hai'],
		monthNamesShort: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6',
		'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
		dayNames: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
		dayNamesShort: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['Hôm nay', 'Mục trước', 'Mục sau', 'Tăng', 'Giảm'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['vi']);
})(jQuery);

