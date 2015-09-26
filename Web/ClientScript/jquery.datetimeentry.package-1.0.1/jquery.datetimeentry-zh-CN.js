/* http://keith-wood.name/datetimeEntry.html
   Simplified Chinese initialisation for the jQuery date/time entry extension.
   By Cloudream(cloudream@gmail.com) */
(function($) {
	$.datetimeEntry.regional['zh-CN'] = {datetimeFormat: 'Y-O-D H:Ma',
		monthNames: ['一月','二月','三月','四月','五月','六月',
		'七月','八月','九月','十月','十一月','十二月'],
		monthNamesShort: ['一','二','三','四','五','六',
		'七','八','九','十','十一','十二'],
		dayNames: ['星期日','星期一','星期二','星期三','星期四','星期五','星期六'],
		dayNamesShort: ['周日','周一','周二','周三','周四','周五','周六'],
		ampmNames: ['AM', 'PM'],
		spinnerTexts: ['当前', '左移', '右移', '加一', '减一'],
		isRTL: false};
	$.datetimeEntry.setDefaults($.datetimeEntry.regional['zh-CN']);
})(jQuery);
