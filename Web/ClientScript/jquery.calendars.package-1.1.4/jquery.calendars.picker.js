/* http://keith-wood.name/calendars.html
   Calendars date picker for jQuery v1.1.4.
   Written by Keith Wood (kbwood{at}iinet.com.au) August 2009.
   Dual licensed under the GPL (http://dev.jquery.com/browser/trunk/jquery/GPL-LICENSE.txt) and 
   MIT (http://dev.jquery.com/browser/trunk/jquery/MIT-LICENSE.txt) licenses. 
   Please attribute the author if you use it. */

(function($) { // Hide scope, no $ conflict

/* Calendar picker manager. */
function CalendarsPicker() {
	this._defaults = {
		calendar: $.calendars.instance(), // The calendar to use
		pickerClass: '', // CSS class to add to this instance of the datepicker
		showOnFocus: true, // True for popup on focus, false for not
		showTrigger: null, // Element to be cloned for a trigger, null for none
		showAnim: 'show', // Name of jQuery animation for popup, '' for no animation
		showOptions: {}, // Options for enhanced animations
		showSpeed: 'normal', // Duration of display/closure
		popupContainer: null, // The element to which a popup calendar is added, null for body
		alignment: 'bottom', // Alignment of popup - with nominated corner of input:
			// 'top' or 'bottom' aligns depending on language direction,
			// 'topLeft', 'topRight', 'bottomLeft', 'bottomRight'
		fixedWeeks: false, // True to always show 6 weeks, false to only show as many as are needed
		firstDay: null, // First day of the week, 0 = Sunday, 1 = Monday, ...
			// defaults to calendar local setting if null
		calculateWeek: null, // Calculate week of the year from a date, null for calendar default
		monthsToShow: 1, // How many months to show, cols or [rows, cols]
		monthsOffset: 0, // How many months to offset the primary month by
		monthsToStep: 1, // How many months to move when prev/next clicked
		monthsToJump: 12, // How many months to move when large prev/next clicked
		useMouseWheel: true, // True to use mousewheel if available, false to never use it
		changeMonth: true, // True to change month/year via drop-down, false for navigation only
		yearRange: 'c-10:c+10', // Range of years to show in drop-down: 'any' for direct text entry
			// or 'start:end', where start/end are '+-nn' for relative to today
			// or 'c+-nn' for relative to the currently selected date
			// or 'nnnn' for an absolute year
		showOtherMonths: false, // True to show dates from other months, false to not show them
		selectOtherMonths: false, // True to allow selection of dates from other months too
		defaultDate: null, // Date to show if no other selected
		selectDefaultDate: false, // True to pre-select the default date if no other is chosen
		minDate: null, // The minimum selectable date
		maxDate: null, // The maximum selectable date
		dateFormat: null, // Format for dates, defaults to calendar setting if null
		autoSize: false, // True to size the input field according to the date format
		rangeSelect: false, // Allows for selecting a date range on one date picker
		rangeSeparator: ' - ', // Text between two dates in a range
		multiSelect: 0, // Maximum number of selectable dates, zero for single select
		multiSeparator: ',', // Text between multiple dates
		onDate: null, // Callback as a date is added to the datepicker
		onShow: null, // Callback just before a datepicker is shown
		onChangeMonthYear: null, // Callback when a new month/year is selected
		onSelect: null, // Callback when a date is selected
		onClose: null, // Callback when a datepicker is closed
		altField: null, // Alternate field to update in synch with the datepicker
		altFormat: null, // Date format for alternate field, defaults to dateFormat
		constrainInput: true, // True to constrain typed input to dateFormat allowed characters
		commandsAsDateFormat: false, // True to apply formatDate to the command texts
		commands: this.commands // Command actions that may be added to a layout by name
	};
	this.regional = {
		'': {
			renderer: this.defaultRenderer, // The rendering templates
			prevText: '&lt;Prev', // Text for the previous month command
			prevStatus: 'Show the previous month', // Status text for the previous month command
			prevJumpText: '&lt;&lt;', // Text for the previous year command
			prevJumpStatus: 'Show the previous year', // Status text for the previous year command
			nextText: 'Next&gt;', // Text for the next month command
			nextStatus: 'Show the next month', // Status text for the next month command
			nextJumpText: '&gt;&gt;', // Text for the next year command
			nextJumpStatus: 'Show the next year', // Status text for the next year command
			currentText: 'Current', // Text for the current month command
			currentStatus: 'Show the current month', // Status text for the current month command
			todayText: 'Today', // Text for the today's month command
			todayStatus: 'Show today\'s month', // Status text for the today's month command
			clearText: 'Clear', // Text for the clear command
			clearStatus: 'Clear all the dates', // Status text for the clear command
			closeText: 'Close', // Text for the close command
			closeStatus: 'Close the datepicker', // Status text for the close command
			yearStatus: 'Change the year', // Status text for year selection
			monthStatus: 'Change the month', // Status text for month selection
			weekText: 'Wk', // Text for week of the year column header
			weekStatus: 'Week of the year', // Status text for week of the year column header
			dayStatus: 'Select DD, M d, yyyy', // Status text for selectable days
			defaultStatus: 'Select a date', // Status text shown by default
			isRTL: false // True if language is right-to-left
		}};
	$.extend(this._defaults, this.regional['']);
	this._disabled = [];
}

$.extend(CalendarsPicker.prototype, {
	dataName: 'calendarsPicker',
	
	/* Class name added to elements to indicate already configured with calendar picker. */
	markerClass: 'hasCalendarsPicker',

	_popupClass: 'calendars-popup', // Marker for popup division
	_triggerClass: 'calendars-trigger', // Marker for trigger element
	_disableClass: 'calendars-disable', // Marker for disabled element
	_coverClass: 'calendars-cover', // Marker for iframe backing element
	_monthYearClass: 'calendars-month-year', // Marker for month/year inputs
	_curMonthClass: 'calendars-month-', // Marker for current month/year
	_anyYearClass: 'calendars-any-year', // Marker for year direct input
	_curDoWClass: 'calendars-dow-', // Marker for day of week
	
	commands: { // Command actions that may be added to a layout by name
		// name: { // The command name, use '{button:name}' or '{link:name}' in layouts
		//		text: '', // The field in the regional settings for the displayed text
		//		status: '', // The field in the regional settings for the status text
		//      // The keystroke to trigger the action
		//		keystroke: {keyCode: nn, ctrlKey: boolean, altKey: boolean, shiftKey: boolean},
		//		enabled: fn, // The function that indicates the command is enabled
		//		date: fn, // The function to get the date associated with this action
		//		action: fn} // The function that implements the action
		prev: {text: 'prevText', status: 'prevStatus', // Previous month
			keystroke: {keyCode: 33}, // Page up
			enabled: function(inst) {
				var minDate = inst.curMinDate();
				return (!minDate || inst.drawDate.newDate().
					add(1 - inst.get('monthsToStep') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay).add(-1, 'd').compareTo(minDate) != -1); },
			date: function(inst) {
				return inst.drawDate.newDate().
					add(-inst.get('monthsToStep') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay); },
			action: function(inst) {
				$.calendars.picker.changeMonth(this, -inst.get('monthsToStep')); }
		},
		prevJump: {text: 'prevJumpText', status: 'prevJumpStatus', // Previous year
			keystroke: {keyCode: 33, ctrlKey: true}, // Ctrl + Page up
			enabled: function(inst) {
				var minDate = inst.curMinDate();
				return (!minDate || inst.drawDate.newDate().
					add(1 - inst.get('monthsToJump') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay).add(-1, 'd').compareTo(minDate) != -1); },
			date: function(inst) {
				return inst.drawDate.newDate().
					add(-inst.get('monthsToJump') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay); },
			action: function(inst) {
				$.calendars.picker.changeMonth(this, -inst.get('monthsToJump')); }
		},
		next: {text: 'nextText', status: 'nextStatus', // Next month
			keystroke: {keyCode: 34}, // Page down
			enabled: function(inst) {
				var maxDate = inst.get('maxDate');
				return (!maxDate || inst.drawDate.newDate().
					add(inst.get('monthsToStep') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay).compareTo(maxDate) != +1); },
			date: function(inst) {
				return inst.drawDate.newDate().
					add(inst.get('monthsToStep') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay); },
			action: function(inst) {
				$.calendars.picker.changeMonth(this, inst.get('monthsToStep')); }
		},
		nextJump: {text: 'nextJumpText', status: 'nextJumpStatus', // Next year
			keystroke: {keyCode: 34, ctrlKey: true}, // Ctrl + Page down
			enabled: function(inst) {
				var maxDate = inst.get('maxDate');
				return (!maxDate || inst.drawDate.newDate().
					add(inst.get('monthsToJump') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay).compareTo(maxDate) != +1);	},
			date: function(inst) {
				return inst.drawDate.newDate().
					add(inst.get('monthsToJump') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay); },
			action: function(inst) {
				$.calendars.picker.changeMonth(this, inst.get('monthsToJump')); }
		},
		current: {text: 'currentText', status: 'currentStatus', // Current month
			keystroke: {keyCode: 36, ctrlKey: true}, // Ctrl + Home
			enabled: function(inst) {
				var minDate = inst.curMinDate();
				var maxDate = inst.get('maxDate');
				var curDate = inst.selectedDates[0] || inst.get('calendar').today();
				return (!minDate || curDate.compareTo(minDate) != -1) &&
					(!maxDate || curDate.compareTo(maxDate) != +1); },
			date: function(inst) {
				return inst.selectedDates[0] || inst.get('calendar').today(); },
			action: function(inst) {
				var curDate = inst.selectedDates[0] || inst.get('calendar').today();
				$.calendars.picker.showMonth(this, curDate.year(), curDate.month()); }
		},
		today: {text: 'todayText', status: 'todayStatus', // Today's month
			keystroke: {keyCode: 36, ctrlKey: true}, // Ctrl + Home
			enabled: function(inst) {
				var minDate = inst.curMinDate();
				var maxDate = inst.get('maxDate');
				return (!minDate || inst.get('calendar').today().compareTo(minDate) != -1) &&
					(!maxDate || inst.get('calendar').today().compareTo(maxDate) != +1); },
			date: function(inst) { return inst.get('calendar').today(); },
			action: function(inst) { $.calendars.picker.showMonth(this); }
		},
		clear: {text: 'clearText', status: 'clearStatus', // Clear the datepicker
			keystroke: {keyCode: 35, ctrlKey: true}, // Ctrl + End
			enabled: function(inst) { return true; },
			date: function(inst) { return null; },
			action: function(inst) { $.calendars.picker.clear(this); }
		},
		close: {text: 'closeText', status: 'closeStatus', // Close the datepicker
			keystroke: {keyCode: 27}, // Escape
			enabled: function(inst) { return true; },
			date: function(inst) { return null; },
			action: function(inst) { $.calendars.picker.hide(this); }
		},
		prevWeek: {text: 'prevWeekText', status: 'prevWeekStatus', // Previous week
			keystroke: {keyCode: 38, ctrlKey: true}, // Ctrl + Up
			enabled: function(inst) {
				var minDate = inst.curMinDate();
				return (!minDate || inst.drawDate.newDate().
					add(-inst.get('calendar').daysInWeek(), 'd').compareTo(minDate) != -1); },
			date: function(inst) { return inst.drawDate.newDate().
				add(-inst.get('calendar').daysInWeek(), 'd'); },
			action: function(inst) { $.calendars.picker.changeDay(
				this, -inst.get('calendar').daysInWeek()); }
		},
		prevDay: {text: 'prevDayText', status: 'prevDayStatus', // Previous day
			keystroke: {keyCode: 37, ctrlKey: true}, // Ctrl + Left
			enabled: function(inst) {
				var minDate = inst.curMinDate();
				return (!minDate || inst.drawDate.newDate().add(-1, 'd').
					compareTo(minDate) != -1); },
			date: function(inst) { return inst.drawDate.newDate().add(-1, 'd'); },
			action: function(inst) { $.calendars.picker.changeDay(this, -1); }
		},
		nextDay: {text: 'nextDayText', status: 'nextDayStatus', // Next day
			keystroke: {keyCode: 39, ctrlKey: true}, // Ctrl + Right
			enabled: function(inst) {
				var maxDate = inst.get('maxDate');
				return (!maxDate || inst.drawDate.newDate().add(1, 'd').
					compareTo(maxDate) != +1); },
			date: function(inst) { return inst.drawDate.newDate().add(1, 'd'); },
			action: function(inst) { $.calendars.picker.changeDay(this, 1); }
		},
		nextWeek: {text: 'nextWeekText', status: 'nextWeekStatus', // Next week
			keystroke: {keyCode: 40, ctrlKey: true}, // Ctrl + Down
			enabled: function(inst) {
				var maxDate = inst.get('maxDate');
				return (!maxDate || inst.drawDate.newDate().
					add(inst.get('calendar').daysInWeek(), 'd').compareTo(maxDate) != +1); },
			date: function(inst) { return inst.drawDate.newDate().
				add(inst.get('calendar').daysInWeek(), 'd'); },
			action: function(inst) { $.calendars.picker.changeDay(
				this, inst.get('calendar').daysInWeek()); }
		}
	},

	/* Default template for generating a calendar picker. */
	defaultRenderer: {
		// Anywhere: '{l10n:name}' to insert localised value for name,
		// '{link:name}' to insert a link trigger for command name,
		// '{button:name}' to insert a button trigger for command name,
		// '{popup:start}...{popup:end}' to mark a section for inclusion in a popup datepicker only,
		// '{inline:start}...{inline:end}' to mark a section for inclusion in an inline datepicker only
		// Overall structure: '{months}' to insert calendar months
		picker: '<div class="calendars">' +
		'<div class="calendars-nav">{link:prev}{link:today}{link:next}</div>{months}' +
		'{popup:start}<div class="calendars-ctrl">{link:clear}{link:close}</div>{popup:end}' +
		'<div class="calendars-clear-fix"></div></div>',
		// One row of months: '{months}' to insert calendar months
		monthRow: '<div class="calendars-month-row">{months}</div>',
		// A single month: '{monthHeader:dateFormat}' to insert the month header -
		// dateFormat is optional and defaults to 'MM yyyy',
		// '{weekHeader}' to insert a week header, '{weeks}' to insert the month's weeks
		month: '<div class="calendars-month"><div class="calendars-month-header">{monthHeader}</div>' +
		'<table><thead>{weekHeader}</thead><tbody>{weeks}</tbody></table></div>',
		// A week header: '{days}' to insert individual day names
		weekHeader: '<tr>{days}</tr>',
		// Individual day header: '{day}' to insert day name
		dayHeader: '<th>{day}</th>',
		// One week of the month: '{days}' to insert the week's days, '{weekOfYear}' to insert week of year
		week: '<tr>{days}</tr>',
		// An individual day: '{day}' to insert day value
		day: '<td>{day}</td>',
		// jQuery selector, relative to picker, for a single month
		monthSelector: '.calendars-month',
		// jQuery selector, relative to picker, for individual days
		daySelector: 'td',
		// Class for right-to-left (RTL) languages
		rtlClass: 'calendars-rtl',
		// Class for multi-month datepickers
		multiClass: 'calendars-multi',
		// Class for selectable dates
		defaultClass: '',
		// Class for currently selected dates
		selectedClass: 'calendars-selected',
		// Class for highlighted dates
		highlightedClass: 'calendars-highlight',
		// Class for today
		todayClass: 'calendars-today',
		// Class for days from other months
		otherMonthClass: 'calendars-other-month',
		// Class for days on weekends
		weekendClass: 'calendars-weekend',
		// Class prefix for commands
		commandClass: 'calendars-cmd',
		// Extra class(es) for commands that are buttons
		commandButtonClass: '',
		// Extra class(es) for commands that are links
		commandLinkClass: '',
		// Class for disabled commands
		disabledClass: 'calendars-disabled'
	},

	/* Override the default settings for all calendar picker instances.
	   @param  settings  (object) the new settings to use as defaults
	   @return  (CalendarPicker) this object */
	setDefaults: function(settings) {
		$.extend(this._defaults, settings || {});
		return this;
	},

	/* Attach the calendar picker functionality to an input field.
	   @param  target    (element) the control to affect
	   @param  settings  (object) the custom options for this instance */
	_attachPicker: function(target, settings) {
		target = $(target);
		if (target.hasClass(this.markerClass)) {
			return;
		}
		target.addClass(this.markerClass);
		var inst = {target: target, selectedDates: [], drawDate: null, pickingRange: false,
			inline: ($.inArray(target[0].nodeName.toLowerCase(), ['div', 'span']) > -1),
			get: function(name) { // Get a setting value, defaulting if necessary
				var value = this.settings[name] !== undefined ?
					this.settings[name] : $.calendars.picker._defaults[name];
				if ($.inArray(name, ['defaultDate', 'minDate', 'maxDate']) > -1) { // Decode date settings
					value = this.get('calendar').determineDate(
						value, null, this.selectedDates[0], this.get('dateFormat'), inst.getConfig());
				}
				else if (name == 'dateFormat') {
					value = value || this.get('calendar').local.dateFormat;
				}
				return value;
			},
			curMinDate: function() {
				return (this.pickingRange ? this.selectedDates[0] : this.get('minDate'));
			},
			getConfig: function() {
				return {dayNamesShort: this.get('dayNamesShort'), dayNames: this.get('dayNames'),
					monthNamesShort: this.get('monthNamesShort'), monthNames: this.get('monthNames'),
					calculateWeek: this.get('calculateWeek'),
					shortYearCutoff: this.get('shortYearCutoff')};
			}
		};
		$.data(target[0], this.dataName, inst);
		var inlineSettings = ($.fn.metadata ? target.metadata() : {});
		inst.settings = $.extend({}, settings || {}, inlineSettings || {});
		if (inst.inline) {
			this._update(target[0]);
			if ($.fn.mousewheel) {
				target.mousewheel(this._doMouseWheel);
			}
		}
		else {
			this._attachments(target, inst);
			target.bind('keydown.' + this.dataName, this._keyDown).
				bind('keypress.' + this.dataName, this._keyPress).
				bind('keyup.' + this.dataName, this._keyUp);
			if (target.attr('disabled')) {
				this.disable(target[0]);
			}
		}
	},

	/* Retrieve the settings for a calendar picker control.
	   @param  target  (element) the control to affect
	   @param  name    (string) the name of the setting (optional)
	   @return  (object) the current instance settings (name == 'all') or
	            (object) the default settings (no name) or
	            (any) the setting value (name supplied) */
	options: function(target, name) {
		var inst = $.data(target, this.dataName);
		return (inst ? (name ? (name == 'all' ?
			inst.settings : inst.settings[name]) : $.calendars.picker._defaults) : {});
	},

	/* Reconfigure the settings for a calendar picker control.
	   @param  target    (element) the control to affect
	   @param  settings  (object) the new options for this instance or
	                     (string) an individual property name
	   @param  value     (any) the individual property value (omit if settings is an object) */
	option: function(target, settings, value) {
		target = $(target);
		if (!target.hasClass(this.markerClass)) {
			return;
		}
		settings = settings || {};
		if (typeof settings == 'string') {
			var name = settings;
			settings = {};
			settings[name] = value;
		}
		var inst = $.data(target[0], this.dataName);
		if (settings.calendar && settings.calendar != inst.get('calendar')) {
			var discardDate = function(name) {
				return (typeof inst.settings[name] == 'object' ? null : inst.settings[name]);
			};
			settings = $.extend({defaultDate: discardDate('defaultDate'),
				minDate: discardDate('minDate'), maxDate: discardDate('maxDate')}, settings);
			inst.selectedDates = [];
			inst.drawDate = null;
		}
		var dates = inst.selectedDates;
		extendRemove(inst.settings, settings);
		this.setDate(target[0], dates, null, false, true);
		inst.pickingRange = false;
		var calendar = inst.get('calendar');
		inst.drawDate = this._checkMinMax(
			(settings.defaultDate ? inst.get('defaultDate') : inst.drawDate) ||
			inst.get('defaultDate') || calendar.today(), inst).newDate();
		if (!inst.inline) {
			this._attachments(target, inst);
		}
		if (inst.inline || inst.div) {
			this._update(target[0]);
		}
	},

	/* Attach events and trigger, if necessary.
	   @param  target  (jQuery) the control to affect
	   @param  inst    (object) the current instance settings */
	_attachments: function(target, inst) {
		target.unbind('focus.' + this.dataName);
		if (inst.get('showOnFocus')) {
			target.bind('focus.' + this.dataName, this.show);
		}
		if (inst.trigger) {
			inst.trigger.remove();
		}
		var trigger = inst.get('showTrigger');
		inst.trigger = (!trigger ? $([]) :
			$(trigger).clone().addClass(this._triggerClass)
				[inst.get('isRTL') ? 'insertBefore' : 'insertAfter'](target).
				click(function() {
					if (!$.calendars.picker.isDisabled(target[0])) {
						$.calendars.picker[$.calendars.picker.curInst == inst ?
							'hide' : 'show'](target[0]);
					}
				}));
		this._autoSize(target, inst);
		var dates = this._extractDates(inst, target.val());
		if (dates) {
			this.setDate(target[0], dates, null, true);
		}
		if (inst.get('selectDefaultDate') && inst.get('defaultDate') &&
				inst.selectedDates.length == 0) {
			var calendar = inst.get('calendar');
			this.setDate(target[0], 
				(inst.get('defaultDate') || calendar.today()).newDate());
		}
	},

	/* Apply the maximum length for the date format.
	   @param  inst  (object) the current instance settings */
	_autoSize: function(target, inst) {
		if (inst.get('autoSize') && !inst.inline) {
			var calendar = inst.get('calendar');
			var date = calendar.newDate(2009, 10, 20); // Ensure double digits
			var dateFormat = inst.get('dateFormat');
			if (dateFormat.match(/[DM]/)) {
				var findMax = function(names) {
					var max = 0;
					var maxI = 0;
					for (var i = 0; i < names.length; i++) {
						if (names[i].length > max) {
							max = names[i].length;
							maxI = i;
						}
					}
					return maxI;
				};
				date.month(findMax(calendar.local[dateFormat.match(/MM/) ? // Longest month
					'monthNames' : 'monthNamesShort']) + 1);
				date.day(findMax(calendar.local[dateFormat.match(/DD/) ? // Longest day
					'dayNames' : 'dayNamesShort']) + 20 - date.dayOfWeek());
			}
			inst.target.attr('size', date.formatDate(dateFormat).length);
		}
	},

	/* Remove the calendar picker functionality from a control.
	   @param  target  (element) the control to affect */
	destroy: function(target) {
		target = $(target);
		if (!target.hasClass(this.markerClass)) {
			return;
		}
		var inst = $.data(target[0], this.dataName);
		if (inst.trigger) {
			inst.trigger.remove();
		}
		target.removeClass(this.markerClass).empty().unbind('.' + this.dataName);
		if (inst.inline && $.fn.mousewheel) {
			target.unmousewheel();
		}
		if (!inst.inline && inst.get('autoSize')) {
			target.removeAttr('size');
		}
		$.removeData(target[0], this.dataName);
	},

	/* Apply multiple event functions.
	   Usage, for example: onShow: multipleEvents(fn1, fn2, ...)
	   @param  fns  (function...) the functions to apply */
	multipleEvents: function(fns) {
		var funcs = arguments;
		return function(args) {
			for (var i = 0; i < funcs.length; i++) {
				funcs[i].apply(this, arguments);
			}
		};
	},

	/* Enable the datepicker and any associated trigger.
	   @param  target  (element) the control to use */
	enable: function(target) {
		var $target = $(target);
		if (!$target.hasClass(this.markerClass)) {
			return;
		}
		var inst = $.data(target, this.dataName);
		if (inst.inline)
			$target.children('.' + this._disableClass).remove().end().
				find('button,select').attr('disabled', '').end().
				find('a').attr('href', 'javascript:void(0)');
		else {
			target.disabled = false;
			inst.trigger.filter('button.' + this._triggerClass).
				attr('disabled', '').end().
				filter('img.' + this._triggerClass).
				css({opacity: '1.0', cursor: ''});
		}
		this._disabled = $.map(this._disabled,
			function(value) { return (value == target ? null : value); }); // Delete entry
	},

	/* Disable the datepicker and any associated trigger.
	   @param  target  (element) the control to use */
	disable: function(target) {
		var $target = $(target);
		if (!$target.hasClass(this.markerClass))
			return;
		var inst = $.data(target, this.dataName);
		if (inst.inline) {
			var inline = $target.children(':last');
			var offset = inline.offset();
			var relOffset = {left: 0, top: 0};
			inline.parents().each(function() {
				if ($(this).css('position') == 'relative') {
					relOffset = $(this).offset();
					return false;
				}
			});
			var zIndex = $target.css('zIndex');
			zIndex = (zIndex == 'auto' ? 0 : parseInt(zIndex, 10)) + 1;
			$target.prepend('<div class="' + this._disableClass + '" style="' +
				'width: ' + inline.outerWidth() + 'px; height: ' + inline.outerHeight() +
				'px; left: ' + (offset.left - relOffset.left) + 'px; top: ' +
				(offset.top - relOffset.top) + 'px; z-index: ' + zIndex + '"></div>').
				find('button,select').attr('disabled', 'disabled').end().
				find('a').removeAttr('href');
		}
		else {
			target.disabled = true;
			inst.trigger.filter('button.' + this._triggerClass).
				attr('disabled', 'disabled').end().
				filter('img.' + this._triggerClass).
				css({opacity: '0.5', cursor: 'default'});
		}
		this._disabled = $.map(this._disabled,
			function(value) { return (value == target ? null : value); }); // Delete entry
		this._disabled.push(target);
	},

	/* Is the first field in a jQuery collection disabled as a datepicker?
	   @param  target  (element) the control to examine
	   @return  (boolean) true if disabled, false if enabled */
	isDisabled: function(target) {
		return (target && $.inArray(target, this._disabled) > -1);
	},

	/* Show a popup datepicker.
	   @param  target  (event) a focus event or
	                   (element) the control to use */
	show: function(target) {
		target = target.target || target;
		var inst = $.data(target, $.calendars.picker.dataName);
		if ($.calendars.picker.curInst == inst) {
			return;
		}
		if ($.calendars.picker.curInst) {
			$.calendars.picker.hide($.calendars.picker.curInst, true);
		}
		if (inst) {
			// Retrieve existing date(s)
			inst.lastVal = null;
			inst.selectedDates = $.calendars.picker._extractDates(inst, $(target).val());
			inst.pickingRange = false;
			inst.drawDate = $.calendars.picker._checkMinMax((inst.selectedDates[0] ||
				inst.get('defaultDate') || inst.get('calendar').today()).newDate(), inst);
			$.calendars.picker.curInst = inst;
			// Generate content
			$.calendars.picker._update(target, true);
			// Adjust position before showing
			var offset = $.calendars.picker._checkOffset(inst);
			inst.div.css({left: offset.left, top: offset.top});
			// And display
			var showAnim = inst.get('showAnim');
			var showSpeed = inst.get('showSpeed');
			showSpeed = (showSpeed == 'normal' && $.ui && $.ui.version >= '1.8' ?
				'_default' : showSpeed);
			var postProcess = function() {
				var cover = inst.div.find('.' + $.calendars.picker._coverClass); // IE6- only
				if (cover.length) {
					var borders = $.calendars.picker._getBorders(inst.div);
					cover.css({left: -borders[0], top: -borders[1],
						width: inst.div.outerWidth() + borders[0],
						height: inst.div.outerHeight() + borders[1]});
				}
			};
			if ($.effects && $.effects[showAnim]) {
				var data = inst.div.data(); // Update old effects data
				for (var key in data) {
					if (key.match(/^ec\.storage\./)) {
						data[key] = inst._mainDiv.css(key.replace(/ec\.storage\./, ''));
					}
				}
				inst.div.data(data).show(showAnim, inst.get('showOptions'), showSpeed, postProcess);
			}
			else {
				inst.div[showAnim || 'show']((showAnim ? showSpeed : ''), postProcess);
			}
			if (!showAnim) {
				postProcess();
			}
		}
	},

	/* Extract possible dates from a string.
	   @param  inst  (object) the current instance settings
	   @param  text  (string) the text to extract from
	   @return  (CDate[]) the extracted dates */
	_extractDates: function(inst, datesText) {
		if (datesText == inst.lastVal) {
			return;
		}
		inst.lastVal = datesText;
		var calendar = inst.get('calendar');
		var dateFormat = inst.get('dateFormat');
		var multiSelect = inst.get('multiSelect');
		var rangeSelect = inst.get('rangeSelect');
		datesText = datesText.split(multiSelect ? inst.get('multiSeparator') :
			(rangeSelect ? inst.get('rangeSeparator') : '\x00'));
		var dates = [];
		for (var i = 0; i < datesText.length; i++) {
			try {
				var date = calendar.parseDate(dateFormat, datesText[i]);
				if (date) {
					var found = false;
					for (var j = 0; j < dates.length; j++) {
						if (dates[j].compareTo(date) == 0) {
							found = true;
							break;
						}
					}
					if (!found) {
						dates.push(date);
					}
				}
			}
			catch (e) {
				// Ignore
			}
		}
		dates.splice(multiSelect || (rangeSelect ? 2 : 1), dates.length);
		if (rangeSelect && dates.length == 1) {
			dates[1] = dates[0];
		}
		return dates;
	},

	/* Update the datepicker display.
	   @param  target  (event) a focus event or
	                   (element) the control to use
	   @param  hidden  (boolean) true to initially hide the datepicker */
	_update: function(target, hidden) {
		target = $(target.target || target);
		var inst = $.data(target[0], $.calendars.picker.dataName);
		if (inst) {
			if (inst.inline || $.calendars.picker.curInst == inst) {
				var onChange = inst.get('onChangeMonthYear');
				if (onChange && (!inst.prevDate || inst.prevDate.year() != inst.drawDate.year() ||
						inst.prevDate.month() != inst.drawDate.month())) {
					onChange.apply(target[0], [inst.drawDate.year(), inst.drawDate.month()]);
				}
			}
			if (inst.inline) {
				target.html(this._generateContent(target[0], inst));
			}
			else if ($.calendars.picker.curInst == inst) {
				if (!inst.div) {
					inst.div = $('<div></div>').addClass(this._popupClass).
						css({display: (hidden ? 'none' : 'static'), position: 'absolute',
							left: target.offset().left,
							top: target.offset().top + target.outerHeight()}).
						appendTo($(inst.get('popupContainer') || 'body'));
					if ($.fn.mousewheel) {
						inst.div.mousewheel(this._doMouseWheel);
					}
				}
				inst.div.html(this._generateContent(target[0], inst));
				target.focus();
			}
		}
	},

	/* Update the input field and any alternate field with the current dates.
	   @param  target  (element) the control to use
	   @param  keyUp   (boolean, internal) true if coming from keyUp processing */
	_updateInput: function(target, keyUp) {
		var inst = $.data(target, this.dataName);
		if (inst) {
			var value = '';
			var altValue = '';
			var sep = (inst.get('multiSelect') ? inst.get('multiSeparator') :
				inst.get('rangeSeparator'));
			var calendar = inst.get('calendar');
			var dateFormat = inst.get('dateFormat') || calendar.local.dateFormat;
			var altFormat = inst.get('altFormat') || dateFormat;
			for (var i = 0; i < inst.selectedDates.length; i++) {
				value += (keyUp ? '' : (i > 0 ? sep : '') +
					calendar.formatDate(dateFormat, inst.selectedDates[i]));
				altValue += (i > 0 ? sep : '') +
					calendar.formatDate(altFormat, inst.selectedDates[i]);
			}
			if (!inst.inline && !keyUp) {
				$(target).val(value);
			}
			$(inst.get('altField')).val(altValue);
			var onSelect = inst.get('onSelect');
			if (onSelect && !keyUp && !inst.inSelect) {
				inst.inSelect = true; // Prevent endless loops
				onSelect.apply(target, [inst.selectedDates]);
				inst.inSelect = false;
			}
		}
	},

	/* Retrieve the size of left and top borders for an element.
	   @param  elem  (jQuery) the element of interest
	   @return  (number[2]) the left and top borders */
	_getBorders: function(elem) {
		var convert = function(value) {
			var extra = ($.browser.msie ? 1 : 0);
			return {thin: 1 + extra, medium: 3 + extra, thick: 5 + extra}[value] || value;
		};
		return [parseFloat(convert(elem.css('border-left-width'))),
			parseFloat(convert(elem.css('border-top-width')))];
	},

	/* Check positioning to remain on the screen.
	   @param  inst  (object) the current instance settings
	   @return  (object) the updated offset for the datepicker */
	_checkOffset: function(inst) {
		var base = (inst.target.is(':hidden') && inst.trigger ? inst.trigger : inst.target);
		var offset = base.offset();
		var isFixed = false;
		$(inst.target).parents().each(function() {
			isFixed |= $(this).css('position') == 'fixed';
			return !isFixed;
		});
		if (isFixed && $.browser.opera) { // Correction for Opera when fixed and scrolled
			offset.left -= document.documentElement.scrollLeft;
			offset.top -= document.documentElement.scrollTop;
		}
		var browserWidth = (!$.browser.mozilla || document.doctype ?
			document.documentElement.clientWidth : 0) || document.body.clientWidth;
		var browserHeight = (!$.browser.mozilla || document.doctype ?
			document.documentElement.clientHeight : 0) || document.body.clientHeight;
		if (browserWidth == 0) {
			return offset;
		}
		var alignment = inst.get('alignment');
		var isRTL = inst.get('isRTL');
		var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft;
		var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
		var above = offset.top - inst.div.outerHeight() -
			(isFixed && $.browser.opera ? document.documentElement.scrollTop : 0);
		var below = offset.top + base.outerHeight();
		var alignL = offset.left;
		var alignR = offset.left + base.outerWidth() - inst.div.outerWidth() -
			(isFixed && $.browser.opera ? document.documentElement.scrollLeft : 0);
		var tooWide = (offset.left + inst.div.outerWidth() - scrollX) > browserWidth;
		var tooHigh = (offset.top + inst.target.outerHeight() + inst.div.outerHeight() -
			scrollY) > browserHeight;
		if (alignment == 'topLeft') {
			offset = {left: alignL, top: above};
		}
		else if (alignment == 'topRight') {
			offset = {left: alignR, top: above};
		}
		else if (alignment == 'bottomLeft') {
			offset = {left: alignL, top: below};
		}
		else if (alignment == 'bottomRight') {
			offset = {left: alignR, top: below};
		}
		else if (alignment == 'top') {
			offset = {left: (isRTL || tooWide ? alignR : alignL), top: above};
		}
		else { // bottom
			offset = {left: (isRTL || tooWide ? alignR : alignL),
				top: (tooHigh ? above : below)};
		}
		offset.left = Math.max((isFixed ? 0 : scrollX), offset.left - (isFixed ? scrollX : 0));
		offset.top = Math.max((isFixed ? 0 : scrollY), offset.top - (isFixed ? scrollY : 0));
		return offset;
	},

	/* Close date picker if clicked elsewhere.
	   @param  event  (MouseEvent) the mouse click to check */
	_checkExternalClick: function(event) {
		if (!$.calendars.picker.curInst) {
			return;
		}
		var target = $(event.target);
		if (!target.parents().andSelf().hasClass($.calendars.picker._popupClass) &&
				!target.hasClass($.calendars.picker.markerClass) &&
				!target.parents().andSelf().hasClass($.calendars.picker._triggerClass)) {
			$.calendars.picker.hide($.calendars.picker.curInst);
		}
	},

	/* Hide a popup datepicker.
	   @param  target     (element) the control to use or
	                      (object) the current instance settings
	   @param  immediate  (boolean) true to close immediately without animation */
	hide: function(target, immediate) {
		var inst = $.data(target, this.dataName) || target;
		if (inst && inst == $.calendars.picker.curInst) {
			var showAnim = (immediate ? '' : inst.get('showAnim'));
			var showSpeed = inst.get('showSpeed');
			showSpeed = (showSpeed == 'normal' && $.ui && $.ui.version >= '1.8' ?
				'_default' : showSpeed);
			var postProcess = function() {
				inst.div.remove();
				inst.div = null;
				$.calendars.picker.curInst = null;
				var onClose = inst.get('onClose');
				if (onClose) {
					onClose.apply(target, [inst.selectedDates]);
				}
			};
			inst.div.stop();
			if ($.effects && $.effects[showAnim]) {
				inst.div.hide(showAnim, inst.get('showOptions'), showSpeed, postProcess);
			}
			else {
				var hideAnim = (showAnim == 'slideDown' ? 'slideUp' :
					(showAnim == 'fadeIn' ? 'fadeOut' : 'hide'));
				inst.div[hideAnim]((showAnim ? showSpeed : ''), postProcess);
			}
			if (!showAnim) {
				postProcess();
			}
		}
	},

	/* Handle keystrokes in the datepicker.
	   @param  event  (KeyEvent) the keystroke
	   @return  (boolean) true if not handled, false if handled */
	_keyDown: function(event) {
		var target = event.target;
		var inst = $.data(target, $.calendars.picker.dataName);
		var handled = false;
		if (inst.div) {
			if (event.keyCode == 9) { // Tab - close
				$.calendars.picker.hide(target);
			}
			else if (event.keyCode == 13) { // Enter - select
				$.calendars.picker.selectDate(target,
					$('a.' + inst.get('renderer').highlightedClass, inst.div)[0]);
				handled = true;
			}
			else { // Command keystrokes
				var commands = inst.get('commands');
				for (var name in commands) {
					var command = commands[name];
					if (command.keystroke.keyCode == event.keyCode &&
							!!command.keystroke.ctrlKey == !!(event.ctrlKey || event.metaKey) &&
							!!command.keystroke.altKey == event.altKey &&
							!!command.keystroke.shiftKey == event.shiftKey) {
						$.calendars.picker.performAction(target, name);
						handled = true;
						break;
					}
				}
			}
		}
		else { // Show on 'current' keystroke
			var command = inst.get('commands').current;
			if (command.keystroke.keyCode == event.keyCode &&
					!!command.keystroke.ctrlKey == !!(event.ctrlKey || event.metaKey) &&
					!!command.keystroke.altKey == event.altKey &&
					!!command.keystroke.shiftKey == event.shiftKey) {
				$.calendars.picker.show(target);
				handled = true;
			}
		}
		inst.ctrlKey = ((event.keyCode < 48 && event.keyCode != 32) ||
			event.ctrlKey || event.metaKey);
		if (handled) {
			event.preventDefault();
			event.stopPropagation();
		}
		return !handled;
	},

	/* Filter keystrokes in the datepicker.
	   @param  event  (KeyEvent) the keystroke
	   @return  (boolean) true if allowed, false if not allowed */
	_keyPress: function(event) {
		var target = event.target;
		var inst = $.data(target, $.calendars.picker.dataName);
		if (inst && inst.get('constrainInput')) {
			var ch = String.fromCharCode(event.keyCode || event.charCode);
			var allowedChars = $.calendars.picker._allowedChars(inst);
			return (event.metaKey || inst.ctrlKey || ch < ' ' ||
				!allowedChars || allowedChars.indexOf(ch) > -1);
		}
		return true;
	},

	/* Determine the set of characters allowed by the date format.
	   @param  inst  (object) the current instance settings
	   @return  (string) the set of allowed characters, or null if anything allowed */
	_allowedChars: function(inst) {
		var dateFormat = inst.get('dateFormat');
		var allowedChars = (inst.get('multiSelect') ? inst.get('multiSeparator') :
			(inst.get('rangeSelect') ? inst.get('rangeSeparator') : ''));
		var literal = false;
		var hasNum = false;
		for (var i = 0; i < dateFormat.length; i++) {
			var ch = dateFormat.charAt(i);
			if (literal) {
				if (ch == "'" && dateFormat.charAt(i + 1) != "'") {
					literal = false;
				}
				else {
					allowedChars += ch;
				}
			}
			else {
				switch (ch) {
					case 'd': case 'm': case 'o': case 'w':
						allowedChars += (hasNum ? '' : '0123456789'); hasNum = true; break;
					case 'y': case '@': case '!':
						allowedChars += (hasNum ? '' : '0123456789') + '-'; hasNum = true; break;
					case 'J':
						allowedChars += (hasNum ? '' : '0123456789') + '-.'; hasNum = true; break;
					case 'D': case 'M': case 'Y':
						return null; // Accept anything
					case "'":
						if (dateFormat.charAt(i + 1) == "'") {
							allowedChars += "'";
						}
						else {
							literal = true;
						}
						break;
					default:
						allowedChars += ch;
				}
			}
		}
		return allowedChars;
	},

	/* Synchronise datepicker with the field.
	   @param  event  (KeyEvent) the keystroke
	   @return  (boolean) true if allowed, false if not allowed */
	_keyUp: function(event) {
		var target = event.target;
		var inst = $.data(target, $.calendars.picker.dataName);
		if (inst && !inst.ctrlKey && inst.lastVal != inst.target.val()) {
			try {
				var dates = $.calendars.picker._extractDates(inst, inst.target.val());
				if (dates.length > 0) {
					$.calendars.picker.setDate(target, dates, null, true);
				}
			}
			catch (event) {
				// Ignore
			}
		}
		return true;
	},

	/* Increment/decrement month/year on mouse wheel activity.
	   @param  event  (event) the mouse wheel event
	   @param  delta  (number) the amount of change */
	_doMouseWheel: function(event, delta) {
		var target = ($.calendars.picker.curInst && $.calendars.picker.curInst.target[0]) ||
			$(event.target).closest('.' + $.calendars.picker.markerClass)[0];
		if ($.calendars.picker.isDisabled(target)) {
			return;
		}
		var inst = $.data(target, $.calendars.picker.dataName);
		if (inst.get('useMouseWheel')) {
			delta = ($.browser.opera ? -delta : delta);
			delta = (delta < 0 ? -1 : +1);
			$.calendars.picker.changeMonth(target,
				-inst.get(event.ctrlKey ? 'monthsToJump' : 'monthsToStep') * delta);
		}
		event.preventDefault();
	},

	/* Clear an input and close a popup datepicker.
	   @param  target  (element) the control to use */
	clear: function(target) {
		var inst = $.data(target, this.dataName);
		if (inst) {
			inst.selectedDates = [];
			this.hide(target);
			if (inst.get('selectDefaultDate') && inst.get('defaultDate')) {
				var calendar = inst.get('calendar');
				this.setDate(target, (inst.get('defaultDate') || calendar.today()).newDate());
			}
			else {
				this._updateInput(target);
			}
		}
	},

	/* Retrieve the selected date(s) for a calendar picker.
	   @param  target  (element) the control to examine
	   @return  (CDate[]) the selected date(s) */
	getDate: function(target) {
		var inst = $.data(target, this.dataName);
		return (inst ? inst.selectedDates : []);
	},

	/* Set the selected date(s) for a calendar picker.
	   @param  target   (element) the control to examine
	   @param  dates    (CDate or number or string or [] of these) the selected date(s)
	   @param  endDate  (CDate or number or string) the ending date for a range (optional)
	   @param  keyUp    (boolean, internal) true if coming from keyUp processing
	   @param  setOpt   (boolean, internal) true if coming from option processing */
	setDate: function(target, dates, endDate, keyUp, setOpt) {
		var inst = $.data(target, this.dataName);
		if (inst) {
			if (!$.isArray(dates)) {
				dates = [dates];
				if (endDate) {
					dates.push(endDate);
				}
			}
			var calendar = inst.get('calendar');
			var dateFormat = inst.get('dateFormat');
			var minDate = inst.get('minDate');
			var maxDate = inst.get('maxDate');
			var curDate = inst.selectedDates[0];
			inst.selectedDates = [];
			for (var i = 0; i < dates.length; i++) {
				var date = calendar.determineDate(
					dates[i], null, curDate, dateFormat, inst.getConfig());
				if (date) {
					if ((!minDate || date.compareTo(minDate) != -1) &&
							(!maxDate || date.compareTo(maxDate) != +1)) {
						var found = false;
						for (var j = 0; j < inst.selectedDates.length; j++) {
							if (inst.selectedDates[j].compareTo(date) == 0) {
								found = true;
								break;
							}
						}
						if (!found) {
							inst.selectedDates.push(date);
						}
					}
				}
			}
			var rangeSelect = inst.get('rangeSelect');
			inst.selectedDates.splice(inst.get('multiSelect') ||
				(rangeSelect ? 2 : 1), inst.selectedDates.length);
			if (rangeSelect) {
				switch (inst.selectedDates.length) {
					case 1: inst.selectedDates[1] = inst.selectedDates[0]; break;
					case 2: inst.selectedDates[1] =
						(inst.selectedDates[0].compareTo(inst.selectedDates[1]) == +1 ?
						inst.selectedDates[0] : inst.selectedDates[1]); break;
				}
				inst.pickingRange = false;
			}
			inst.prevDate = (inst.drawDate ? inst.drawDate.newDate() : null);
			inst.drawDate = this._checkMinMax((inst.selectedDates[0] ||
				inst.get('defaultDate') || calendar.today()).newDate(), inst);
			if (!setOpt) {
				this._update(target);
				this._updateInput(target, keyUp);
			}
		}
	},

	/* Determine whether a date is selectable for this datepicker.
	   @param  target  (element) the control to check
	   @param  date    (Date or string or number) the date to check
	   @return  (boolean) true if selectable, false if not */
	isSelectable: function(target, date) {
		var inst = $.data(target, this.dataName);
		if (!inst) {
			return false;
		}
		date = inst.get('calendar').determineDate(date,
			inst.selectedDates[0] || inst.get('calendar').today(), null,
			inst.get('dateFormat'), inst.getConfig());
		return this._isSelectable(target, date, inst.get('onDate'),
			inst.get('minDate'), inst.get('maxDate'));
	},

	/* Internally determine whether a date is selectable for this datepicker.
	   @param  target   (element) the control to check
	   @param  date     (Date) the date to check
	   @param  onDate   (function or boolean) any onDate callback or callback.selectable
	   @param  mindate  (Date) the minimum allowed date
	   @param  maxdate  (Date) the maximum allowed date
	   @return  (boolean) true if selectable, false if not */
	_isSelectable: function(target, date, onDate, minDate, maxDate) {
		var dateInfo = (typeof onDate == 'boolean' ? {selectable: onDate} :
			(!onDate ? {} : onDate.apply(target, [date, true])));
		return (dateInfo.selectable != false) &&
			(!minDate || date.toJD() >= minDate.toJD()) &&
			(!maxDate || date.toJD() <= maxDate.toJD());
	},

	/* Perform a named action for a calendar picker.
	   @param  target  (element) the control to affect
	   @param  action  (string) the name of the action */
	performAction: function(target, action) {
		var inst = $.data(target, this.dataName);
		if (inst && !this.isDisabled(target)) {
			var commands = inst.get('commands');
			if (commands[action] && commands[action].enabled.apply(target, [inst])) {
				commands[action].action.apply(target, [inst]);
			}
		}
	},

	/* Set the currently shown month, defaulting to today's.
	   @param  target  (element) the control to affect
	   @param  year    (number) the year to show (optional)
	   @param  month   (number) the month to show (optional)
	   @param  day     (number) the day to show (optional) */
	showMonth: function(target, year, month, day) {
		var inst = $.data(target, this.dataName);
		if (inst && (day != null ||
				(inst.drawDate.year() != year || inst.drawDate.month() != month))) {
			inst.prevDate = inst.drawDate.newDate();
			var calendar = inst.get('calendar');
			var show = this._checkMinMax((year != null ?
				calendar.newDate(year, month, 1) : calendar.today()), inst);
			inst.drawDate.date(show.year(), show.month(), 
				(day != null ? day : Math.min(inst.drawDate.day(),
				calendar.daysInMonth(show.year(), show.month()))));
			this._update(target);
		}
	},

	/* Adjust the currently shown month.
	   @param  target  (element) the control to affect
	   @param  offset  (number) the number of months to change by */
	changeMonth: function(target, offset) {
		var inst = $.data(target, this.dataName);
		if (inst) {
			var date = inst.drawDate.newDate().add(offset, 'm');
			this.showMonth(target, date.year(), date.month());
		}
	},

	/* Adjust the currently shown day.
	   @param  target  (element) the control to affect
	   @param  offset  (number) the number of days to change by */
	changeDay: function(target, offset) {
		var inst = $.data(target, this.dataName);
		if (inst) {
			var date = inst.drawDate.newDate().add(offset, 'd');
			this.showMonth(target, date.year(), date.month(), date.day());
		}
	},

	/* Restrict a date to the minimum/maximum specified.
	   @param  date  (CDate) the date to check
	   @param  inst  (object) the current instance settings */
	_checkMinMax: function(date, inst) {
		var minDate = inst.get('minDate');
		var maxDate = inst.get('maxDate');
		date = (minDate && date.compareTo(minDate) == -1 ? minDate.newDate() : date);
		date = (maxDate && date.compareTo(maxDate) == +1 ? maxDate.newDate() : date);
		return date;
	},

	/* Retrieve the date associated with an entry in the datepicker.
	   @param  target  (element) the control to examine
	   @param  elem    (element) the selected datepicker element
	   @return  (CDate) the corresponding date, or null */
	retrieveDate: function(target, elem) {
		var inst = $.data(target, this.dataName);
		return (!inst ? null : inst.get('calendar').fromJD(
			parseFloat(elem.className.replace(/^.*jd(\d+\.5).*$/, '$1'))));
	},

	/* Select a date for this datepicker.
	   @param  target  (element) the control to examine
	   @param  elem    (element) the selected datepicker element */
	selectDate: function(target, elem) {
		var inst = $.data(target, this.dataName);
		if (inst && !this.isDisabled(target)) {
			var date = this.retrieveDate(target, elem);
			var multiSelect = inst.get('multiSelect');
			var rangeSelect = inst.get('rangeSelect');
			if (multiSelect) {
				var found = false;
				for (var i = 0; i < inst.selectedDates.length; i++) {
					if (date.compareTo(inst.selectedDates[i]) == 0) {
						inst.selectedDates.splice(i, 1);
						found = true;
						break;
					}
				}
				if (!found && inst.selectedDates.length < multiSelect) {
					inst.selectedDates.push(date);
				}
			}
			else if (rangeSelect) {
				if (inst.pickingRange) {
					inst.selectedDates[1] = date;
				}
				else {
					inst.selectedDates = [date, date];
				}
				inst.pickingRange = !inst.pickingRange;
			}
			else {
				inst.selectedDates = [date];
			}
			this._updateInput(target);
			if (inst.inline || inst.pickingRange || inst.selectedDates.length <
					(multiSelect || (rangeSelect ? 2 : 1))) {
				this._update(target);
			}
			else {
				this.hide(target);
			}
		}
	},

	/* Generate the datepicker content for this control.
	   @param  target  (element) the control to affect
	   @param  inst    (object) the current instance settings
	   @return  (jQuery) the datepicker content */
	_generateContent: function(target, inst) {
		var calendar = inst.get('calendar');
		var renderer = inst.get('renderer');
		var monthsToShow = inst.get('monthsToShow');
		monthsToShow = ($.isArray(monthsToShow) ? monthsToShow : [1, monthsToShow]);
		inst.drawDate = this._checkMinMax(
			inst.drawDate || inst.get('defaultDate') || calendar.today(), inst);
		var drawDate = inst.drawDate.newDate().add(-inst.get('monthsOffset'), 'm');
		// Generate months
		var monthRows = '';
		for (var row = 0; row < monthsToShow[0]; row++) {
			var months = '';
			for (var col = 0; col < monthsToShow[1]; col++) {
				months += this._generateMonth(target, inst, drawDate.year(),
					drawDate.month(), calendar, renderer, (row == 0 && col == 0));
				drawDate.add(1, 'm');
			}
			monthRows += this._prepare(renderer.monthRow, inst).replace(/\{months\}/, months);
		}
		var picker = this._prepare(renderer.picker, inst).replace(/\{months\}/, monthRows).
			replace(/\{weekHeader\}/g, this._generateDayHeaders(inst, calendar, renderer)) +
			($.browser.msie && parseInt($.browser.version, 10) < 7 && !inst.inline ?
			'<iframe src="javascript:void(0);" class="' + this._coverClass + '"></iframe>' : '');
		// Add commands
		var commands = inst.get('commands');
		var asDateFormat = inst.get('commandsAsDateFormat');
		var addCommand = function(type, open, close, name, classes) {
			if (picker.indexOf('{' + type + ':' + name + '}') == -1) {
				return;
			}
			var command = commands[name];
			var date = (asDateFormat ? command.date.apply(target, [inst]) : null);
			picker = picker.replace(new RegExp('\\{' + type + ':' + name + '\\}', 'g'),
				'<' + open +
				(command.status ? ' title="' + inst.get(command.status) + '"' : '') +
				' class="' + renderer.commandClass + ' ' +
				renderer.commandClass + '-' + name + ' ' + classes +
				(command.enabled(inst) ? '' : ' ' + renderer.disabledClass) + '">' +
				(date ? date.formatDate(inst.get(command.text)) : inst.get(command.text)) +
				'</' + close + '>');
		};
		for (var name in commands) {
			addCommand('button', 'button type="button"', 'button', name,
				renderer.commandButtonClass);
			addCommand('link', 'a href="javascript:void(0)"', 'a', name,
				renderer.commandLinkClass);
		}
		picker = $(picker);
		if (monthsToShow[1] > 1) {
			var count = 0;
			$(renderer.monthSelector, picker).each(function() {
				var nth = ++count % monthsToShow[1];
				$(this).addClass(nth == 1 ? 'first' : (nth == 0 ? 'last' : ''));
			});
		}
		// Add calendar behaviour
		var self = this;
		picker.find(renderer.daySelector + ' a').hover(
				function() { $(this).addClass(renderer.highlightedClass); },
				function() {
					(inst.inline ? $(this).parents('.' + self.markerClass) : inst.div).
						find(renderer.daySelector + ' a').
						removeClass(renderer.highlightedClass);
				}).
			click(function() {
				self.selectDate(target, this);
			}).end().
			find('select.' + this._monthYearClass + ':not(.' + this._anyYearClass + ')').change(function() {
				var monthYear = $(this).val().split('/');
				self.showMonth(target, parseInt(monthYear[1], 10), parseInt(monthYear[0], 10));
			}).end().
			find('select.' + this._anyYearClass).click(function() {
				$(this).next('input').css({left: this.offsetLeft, top: this.offsetTop,
					width: this.offsetWidth, height: this.offsetHeight}).show().focus();
			}).end().
			find('input.' + self._monthYearClass).change(function() {
				try {
					var year = parseInt($(this).val(), 10);
					year = (isNaN(year) ? inst.drawDate.year() : year);
					self.showMonth(target, year, inst.drawDate.month(), inst.drawDate.day());
				}
				catch (e) {
					alert(e);
				}
			}).keydown(function(event) {
				if (event.keyCode == 27) { // Escape
					$(event.target).hide();
					inst.target.focus();
				}
			});
		// Add command behaviour
		picker.find('.' + renderer.commandClass).click(function() {
				if (!$(this).hasClass(renderer.disabledClass)) {
					var action = this.className.replace(
						new RegExp('^.*' + renderer.commandClass + '-([^ ]+).*$'), '$1');
					$.calendars.picker.performAction(target, action);
				}
			});
		// Add classes
		if (inst.get('isRTL')) {
			picker.addClass(renderer.rtlClass);
		}
		if (monthsToShow[0] * monthsToShow[1] > 1) {
			picker.addClass(renderer.multiClass);
		}
		var pickerClass = inst.get('pickerClass');
		if (pickerClass) {
			picker.addClass(pickerClass);
		}
		// Resize
		$('body').append(picker);
		var width = 0;
		picker.find(renderer.monthSelector).each(function() {
			width += $(this).outerWidth();
		});
		picker.width(width / monthsToShow[0]);
		// Pre-show customisation
		var onShow = inst.get('onShow');
		if (onShow) {
			onShow.apply(target, [picker, calendar, inst]);
		}
		return picker;
	},

	/* Generate the content for a single month.
	   @param  target    (element) the control to affect
	   @param  inst      (object) the current instance settings
	   @param  year      (number) the year to generate
	   @param  month     (number) the month to generate
	   @param  calendar  (*Calendar) the current calendar
	   @param  renderer  (object) the rendering templates
	   @param  first     (boolean) true if first of multiple months
	   @return  (string) the month content */
	_generateMonth: function(target, inst, year, month, calendar, renderer, first) {
		var daysInMonth = calendar.daysInMonth(year, month);
		var monthsToShow = inst.get('monthsToShow');
		monthsToShow = ($.isArray(monthsToShow) ? monthsToShow : [1, monthsToShow]);
		var fixedWeeks = inst.get('fixedWeeks') || (monthsToShow[0] * monthsToShow[1] > 1);
		var firstDay = inst.get('firstDay');
		firstDay = (firstDay == null ? calendar.local.firstDay : firstDay);
		var leadDays = (calendar.dayOfWeek(year, month, calendar.minDay) -
			firstDay + calendar.daysInWeek()) % calendar.daysInWeek();
		var numWeeks = (fixedWeeks ? 6 : Math.ceil((leadDays + daysInMonth) / calendar.daysInWeek()));
		var showOtherMonths = inst.get('showOtherMonths');
		var selectOtherMonths = inst.get('selectOtherMonths') && showOtherMonths;
		var dayStatus = inst.get('dayStatus');
		var minDate = (inst.pickingRange ? inst.selectedDates[0] : inst.get('minDate'));
		var maxDate = inst.get('maxDate');
		var rangeSelect = inst.get('rangeSelect');
		var onDate = inst.get('onDate');
		var showWeeks = renderer.week.indexOf('{weekOfYear}') > -1;
		var calculateWeek = inst.get('calculateWeek');
		var today = calendar.today();
		var drawDate = calendar.newDate(year, month, calendar.minDay);
		drawDate.add(-leadDays - (fixedWeeks &&
			(drawDate.dayOfWeek() == firstDay || drawDate.daysInMonth() < calendar.daysInWeek())?
			calendar.daysInWeek() : 0), 'd');
		var jd = drawDate.toJD();
		// Generate weeks
		var weeks = '';
		for (var week = 0; week < numWeeks; week++) {
			var weekOfYear = (!showWeeks ? '' : '<span class="jd' + jd + '">' +
				(calculateWeek ? calculateWeek(drawDate) : drawDate.weekOfYear()) + '</span>');
			var days = '';
			for (var day = 0; day < calendar.daysInWeek(); day++) {
				var selected = false;
				if (rangeSelect && inst.selectedDates.length > 0) {
					selected = (drawDate.compareTo(inst.selectedDates[0]) != -1 &&
						drawDate.compareTo(inst.selectedDates[1]) != +1)
				}
				else {
					for (var i = 0; i < inst.selectedDates.length; i++) {
						if (inst.selectedDates[i].compareTo(drawDate) == 0) {
							selected = true;
							break;
						}
					}
				}
				var dateInfo = (!onDate ? {} :
					onDate.apply(target, [drawDate, drawDate.month() == month]));
				var selectable = (selectOtherMonths || drawDate.month() == month) &&
					this._isSelectable(target, drawDate, dateInfo.selectable, minDate, maxDate);
				days += this._prepare(renderer.day, inst).replace(/\{day\}/g,
					(selectable ? '<a href="javascript:void(0)"' : '<span') +
					' class="jd' + jd + ' ' + (dateInfo.dateClass || '') +
					(selected && (selectOtherMonths || drawDate.month() == month) ?
					' ' + renderer.selectedClass : '') +
					(selectable ? ' ' + renderer.defaultClass : '') +
					(drawDate.weekDay() ? '' : ' ' + renderer.weekendClass) +
					(drawDate.month() == month ? '' : ' ' + renderer.otherMonthClass) +
					(drawDate.compareTo(today) == 0 && drawDate.month() == month ?
					' ' + renderer.todayClass : '') +
					(drawDate.compareTo(inst.drawDate) == 0 && drawDate.month() == month ?
					' ' + renderer.highlightedClass : '') + '"' +
					(dateInfo.title || (dayStatus && selectable) ? ' title="' +
					(dateInfo.title || drawDate.formatDate(dayStatus)) + '"' : '') + '>' +
					(showOtherMonths || drawDate.month() == month ?
					dateInfo.content || drawDate.day() : '&nbsp;') +
					(selectable ? '</a>' : '</span>'));
				drawDate.add(1, 'd');
				jd++;
			}
			weeks += this._prepare(renderer.week, inst).replace(/\{days\}/g, days).
				replace(/\{weekOfYear\}/g, weekOfYear);
		}
		var monthHeader = this._prepare(renderer.month, inst).match(/\{monthHeader(:[^\}]+)?\}/);
		monthHeader = (monthHeader[0].length <= 13 ? 'MM yyyy' :
			monthHeader[0].substring(13, monthHeader[0].length - 1));
		monthHeader = (first ? this._generateMonthSelection(
			inst, year, month, minDate, maxDate, monthHeader, calendar, renderer) :
			calendar.formatDate(monthHeader, calendar.newDate(year, month, calendar.minDay)));
		var weekHeader = this._prepare(renderer.weekHeader, inst).
			replace(/\{days\}/g, this._generateDayHeaders(inst, calendar, renderer));
		return this._prepare(renderer.month, inst).replace(/\{monthHeader(:[^\}]+)?\}/g, monthHeader).
			replace(/\{weekHeader\}/g, weekHeader).replace(/\{weeks\}/g, weeks);
	},

	/* Generate the HTML for the day headers.
	   @param  inst      (object) the current instance settings
	   @param  calendar  (*Calendar) the current calendar
	   @param  renderer  (object) the rendering templates
	   @return  (string) a week's worth of day headers */
	_generateDayHeaders: function(inst, calendar, renderer) {
		var firstDay = inst.get('firstDay');
		firstDay = (firstDay == null ? calendar.local.firstDay : firstDay);
		var header = '';
		for (var day = 0; day < calendar.daysInWeek(); day++) {
			var dow = (day + firstDay) % calendar.daysInWeek();
			header += this._prepare(renderer.dayHeader, inst).replace(/\{day\}/g,
				'<span class="' + this._curDoWClass + dow + '" title="' +
				calendar.local.dayNames[dow] + '">' +
				calendar.local.dayNamesMin[dow] + '</span>');
		}
		return header;
	},

	/* Generate selection controls for month.
	   @param  inst         (object) the current instance settings
	   @param  year         (number) the year to generate
	   @param  month        (number) the month to generate
	   @param  minDate      (CDate) the minimum date allowed
	   @param  maxDate      (CDate) the maximum date allowed
	   @param  monthHeader  (string) the month/year format
	   @param  calendar     (*Calendar) the current calendar
	   @return  (string) the month selection content */
	_generateMonthSelection: function(inst, year, month, minDate, maxDate, monthHeader, calendar) {
		if (!inst.get('changeMonth')) {
			return calendar.formatDate(monthHeader, calendar.newDate(year, month, 1));
		}
		// Months
		var monthNames = calendar.local[
			'monthNames' + (monthHeader.match(/mm/i) ? '' : 'Short')];
		var html = monthHeader.replace(/m+/i, '\\x2E').replace(/y+/i, '\\x2F');
		var selector = '<select class="' + this._monthYearClass +
			'" title="' + inst.get('monthStatus') + '">';
		var maxMonth = calendar.monthsInYear(year) + calendar.minMonth;
		for (var m = calendar.minMonth; m < maxMonth; m++) {
			if ((!minDate || calendar.newDate(year, m,
					calendar.daysInMonth(year, m) - 1 + calendar.minDay).
					compareTo(minDate) != -1) &&
					(!maxDate || calendar.newDate(year, m, calendar.minDay).
					compareTo(maxDate) != +1)) {
				selector += '<option value="' + m + '/' + year + '"' +
					(month == m ? ' selected="selected"' : '') + '>' +
					monthNames[m - calendar.minMonth] + '</option>';
			}
		}
		selector += '</select>';
		html = html.replace(/\\x2E/, selector);
		// Years
		var yearRange = inst.get('yearRange');
		if (yearRange == 'any') {
			selector = '<select class="' + this._monthYearClass + ' ' + this._anyYearClass +
				'" title="' + inst.get('yearStatus') + '">' +
				'<option>' + year + '</option></select>' +
				'<input class="' + this._monthYearClass + ' ' + this._curMonthClass +
				month + '" value="' + year + '">';
		}
		else {
			yearRange = yearRange.split(':');
			var todayYear = calendar.today().year();
			var start = (yearRange[0].match('c[+-].*') ? year + parseInt(yearRange[0].substring(1), 10) :
				((yearRange[0].match('[+-].*') ? todayYear : 0) + parseInt(yearRange[0], 10)));
			var end = (yearRange[1].match('c[+-].*') ? year + parseInt(yearRange[1].substring(1), 10) :
				((yearRange[1].match('[+-].*') ? todayYear : 0) + parseInt(yearRange[1], 10)));
			selector = '<select class="' + this._monthYearClass +
				'" title="' + inst.get('yearStatus') + '">';
			start = calendar.newDate(start + 1, calendar.firstMonth, calendar.minDay).add(-1, 'd');
			end = calendar.newDate(end, calendar.firstMonth, calendar.minDay);
			var addYear = function(y) {
				if (y != 0 || calendar.hasYearZero) {
					selector += '<option value="' +
						Math.min(month, calendar.monthsInYear(y) - 1 + calendar.minMonth) +
						'/' + y + '"' + (year == y ? ' selected="selected"' : '') + '>' +
						y + '</option>';
				}
			};
			if (start.toJD() < end.toJD()) {
				start = (minDate && minDate.compareTo(start) == +1 ? minDate : start).year();
				end = (maxDate && maxDate.compareTo(end) == -1 ? maxDate : end).year();
				for (var y = start; y <= end; y++) {
					addYear(y);
				}
			}
			else {
				start = (maxDate && maxDate.compareTo(start) == -1 ? maxDate : start).year();
				end = (minDate && minDate.compareTo(end) == +1 ? minDate : end).year();
				for (var y = start; y >= end; y--) {
					addYear(y);
				}
			}
			selector += '</select>';
		}
		html = html.replace(/\\x2F/, selector);
		return html;
	},

	/* Prepare a render template for use.
	   Exclude popup/inline sections that are not applicable.
	   Localise text of the form: {l10n:name}.
	   @param  text  (string) the text to localise
	   @param  inst  (object) the current instance settings
	   @return  (string) the localised text */
	_prepare: function(text, inst) {
		var replaceSection = function(type, retain) {
			while (true) {
				var start = text.indexOf('{' + type + ':start}');
				if (start == -1) {
					return;
				}
				var end = text.substring(start).indexOf('{' + type + ':end}');
				if (end > -1) {
					text = text.substring(0, start) +
						(retain ? text.substr(start + type.length + 8, end - type.length - 8) : '') +
						text.substring(start + end + type.length + 6);
				}
			}
		};
		replaceSection('inline', inst.inline);
		replaceSection('popup', !inst.inline);
		var pattern = /\{l10n:([^\}]+)\}/;
		var matches = null;
		while (matches = pattern.exec(text)) {
			text = text.replace(matches[0], inst.get(matches[1]));
		}
		return text;
	}
});

/* jQuery extend now ignores nulls!
   @param  target  (object) the object to extend
   @param  props   (object) the new settings
   @return  (object) the updated object */
function extendRemove(target, props) {
	$.extend(target, props);
	for (var name in props)
		if (props[name] == null || props[name] == undefined)
			target[name] = props[name];
	return target;
};

/* Attach the calendar picker functionality to a jQuery selection.
   @param  command  (string) the command to run (optional, default 'attach')
   @param  options  (object) the new settings to use for these instances (optional)
   @return  (jQuery) for chaining further calls */
$.fn.calendarsPicker = function(options) {
	var otherArgs = Array.prototype.slice.call(arguments, 1);
	if ($.inArray(options, ['getDate', 'isDisabled', 'isSelectable', 'options', 'retrieveDate']) > -1) {
		return $.calendars.picker[options].apply($.calendars.picker, [this[0]].concat(otherArgs));
	}
	return this.each(function() {
		if (typeof options == 'string') {
			$.calendars.picker[options].apply($.calendars.picker, [this].concat(otherArgs));
		}
		else {
			$.calendars.picker._attachPicker(this, options || {});
		}
	});
};

/* Initialise the calendar picker functionality. */
$.calendars.picker = new CalendarsPicker(); // singleton instance

$(function() {
	$(document).mousedown($.calendars.picker._checkExternalClick).
		resize(function() { $.calendars.picker.hide($.calendars.picker.curInst); });
});

})(jQuery);
