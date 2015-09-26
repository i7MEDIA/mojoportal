/* http://keith-wood.name/calendars.html
   Calendars Validation extension for jQuery 1.1.4.
   Requires Jörn Zaefferer's Validation plugin (http://plugins.jquery.com/project/validate).
   Written by Keith Wood (kbwood{at}iinet.com.au).
   Dual licensed under the GPL (http://dev.jquery.com/browser/trunk/jquery/GPL-LICENSE.txt) and 
   MIT (http://dev.jquery.com/browser/trunk/jquery/MIT-LICENSE.txt) licenses. 
   Please attribute the author if you use it. */

(function($) { // Hide the namespace

/* Add validation methods if validation plugin available. */
if ($.fn.validate) {

	$.calendars.picker.selectDateOrig = $.calendars.picker.selectDate;
	
	$.extend($.calendars.picker.regional[''], {
		validateDate: 'Please enter a valid date',
		validateDateMin: 'Please enter a date on or after {0}',
		validateDateMax: 'Please enter a date on or before {0}',
		validateDateMinMax: 'Please enter a date between {0} and {1}',
		validateDateCompare: 'Please enter a date {0} {1}',
		validateDateToday: 'today',
		validateDateOther: 'the other date',
		validateDateEQ: 'equal to',
		validateDateNE: 'not equal to',
		validateDateLT: 'before',
		validateDateGT: 'after',
		validateDateLE: 'not after',
		validateDateGE: 'not before'
	});
	
	$.extend($.calendars.picker._defaults, $.calendars.picker.regional['']);

	$.extend($.calendars.picker, {

		/* Trigger a validation after updating the input field with the selected date.
		   @param  target  (element) the control to examine
		   @param  elem    (element) the selected datepicker element */
		selectDate: function(target, elem) {
			this.selectDateOrig(target, elem);
			var inst = $.data(target, $.calendars.picker.dataName);
			if (!inst.inline && $.fn.validate) {
				var validation = $(target).parents('form').validate();
				if (validation) {
					validation.element('#' + target.id);
				}
			}
		},

		/* Correct error placement for validation errors - after any trigger.
		   @param  error    (jQuery) the error message
		   @param  element  (jQuery) the field in error */
		errorPlacement: function(error, element) {
			var inst = $.data(element[0], $.calendars.picker.dataName);
			if (inst) {
				error[inst.get('isRTL') ? 'insertBefore' : 'insertAfter'](
					inst.trigger.length > 0 ? inst.trigger : element);
			}
			else {
				error.insertAfter(element);
			}
		},

		/* Format a validation error message involving dates.
		   @param  source  (string) the error message
		   @param  params  (Date[]) the dates
		   @return  (string) the formatted message */
		errorFormat: function(source, params) {
			var format = ($.calendars.picker.curInst ?
				$.calendars.picker.curInst.get('dateFormat') :
				$.calendars.picker._defaults.dateFormat);
			$.each(params, function(index, value) {
				source = source.replace(new RegExp('\\{' + index + '\\}', 'g'),
					value.formatDate(format) || 'nothing');
			});
			return source;
		}
	});

	var lastElement = null;

	/* Validate date field. */
	$.validator.addMethod('cpDate', function(value, element, params) {
			lastElement = element;
			return this.optional(element) || validateEach(value, element);
		},
		function(params) {
			var inst = $.data(lastElement, $.calendars.picker.dataName);
			var minDate = inst.get('minDate');
			var maxDate = inst.get('maxDate');
			var messages = $.calendars.picker._defaults;
			return (minDate && maxDate ?
				$.calendars.picker.errorFormat(messages.validateDateMinMax, [minDate, maxDate]) :
				(minDate ? $.calendars.picker.errorFormat(messages.validateDateMin, [minDate]) :
				(maxDate ? $.calendars.picker.errorFormat(messages.validateDateMax, [maxDate]) :
				messages.validateDate)));
		});

	/* Apply a validation test to each date provided.
	   @param  value    (string) the current field value
	   @param  element  (element) the field control
	   @return  (boolean) true if OK, false if failed validation */
	function validateEach(value, element) {
		var inst = $.data(element, $.calendars.picker.dataName);
		var rangeSelect = inst.get('rangeSelect');
		var multiSelect = inst.get('multiSelect');
		var dates = (multiSelect ? value.split(inst.get('multiSeparator')) :
			(rangeSelect ? value.split(inst.get('rangeSeparator')) : [value]));
		var ok = (multiSelect && dates.length <= multiSelect) ||
			(!multiSelect && rangeSelect && dates.length == 2) ||
			(!multiSelect && !rangeSelect && dates.length == 1);
		if (ok) {
			try {
				var dateFormat = inst.get('dateFormat');
				var minDate = inst.get('minDate');
				var maxDate = inst.get('maxDate');
				var cp = $(element);
				$.each(dates, function(i, v) {
					dates[i] = inst.get('calendar').parseDate(dateFormat, v);
					ok = ok && (!dates[i] || (cp.calendarsPicker('isSelectable', dates[i]) &&
						(!minDate || dates[i].compareTo(minDate) != -1) &&
						(!maxDate || dates[i].compareTo(maxDate) != +1)));
				});
			}
			catch (e) {
				ok = false;
			}
		}
		if (ok && rangeSelect) {
			ok = (dates[0].compareTo(dates[1]) != +1);
		}
		return ok;
	}

	/* And allow as a class rule. */
	$.validator.addClassRules('cpDate', {cpDate: true});

	var comparisons = {equal: 'eq', same: 'eq', notEqual: 'ne', notSame: 'ne',
		lessThan: 'lt', before: 'lt', greaterThan: 'gt', after: 'gt',
		notLessThan: 'ge', notBefore: 'ge', notGreaterThan: 'le', notAfter: 'le'};

	/* Cross-validate date fields.
	   params should be an array with [0] comparison type eq/ne/lt/gt/le/ge or synonyms,
	   [1] 'today' or date string or CDate or other field selector/element/jQuery OR
	   an object with one attribute with name eq/ne/lt/gt/le/ge or synonyms
	   and value 'today' or date string or CDate or other field selector/element/jQuery OR
	   a string with eq/ne/lt/gt/le/ge or synonyms followed by 'today' or date string or jQuery selector */
	$.validator.addMethod('cpCompareDate', function(value, element, params) {
			if (this.optional(element)) {
				return true;
			}
			params = normaliseParams(params);
			var thisDate = $(element).calendarsPicker('getDate');
			var thatDate = extractOtherDate(element, params[1]);
			if (thisDate.length == 0 || thatDate.length == 0) {
				return true;
			}
			lastElement = element;
			var finalResult = true;
			for (var i = 0; i < thisDate.length; i++) {
				var result = thisDate[i].compareTo(thatDate[0]);
				switch (comparisons[params[0]] || params[0]) {
					case 'eq': finalResult = (result == 0); break;
					case 'ne': finalResult = (result != 0); break;
					case 'lt': finalResult = (result < 0); break;
					case 'gt': finalResult = (result > 0); break;
					case 'le': finalResult = (result <= 0); break;
					case 'ge': finalResult = (result >= 0); break;
					default:   finalResult = true;
				}
				if (!finalResult) {
					break;
				}
			}
			return finalResult;
		},
		function(params) {
			var messages = $.calendars.picker._defaults;
			params = normaliseParams(params);
			var thatDate = extractOtherDate(lastElement, params[1], true);
			thatDate = (params[1] == 'today' ? messages.validateDateToday : 
				(thatDate.length ? thatDate[0].formatDate() : messages.validateDateOther));
			return messages.validateDateCompare.replace(/\{0\}/,
				messages['validateDate' + (comparisons[params[0]] || params[0]).toUpperCase()]).
				replace(/\{1\}/, thatDate);
		});

	/* Normalise the comparison parameters to an array.
	   @param  params  (array or object or string) the original parameters
	   @return  (array) the normalised parameters */
	function normaliseParams(params) {
		if (typeof params == 'string') {
			params = params.split(' ');
		}
		else if (!$.isArray(params)) {
			var opts = [];
			for (var name in params) {
				opts[0] = name;
				opts[1] = params[name];
			}
			params = opts;
		}
		return params;
	}

	/* Determine the comparison date.
	   @param  element  (element) the current datepicker element
	   @param  source   (string or CDate or jQuery or element) the source of the other date
	   @param  noOther  (boolean) true to not get the date from another field
	   @return  (CDate[1]) the date for comparison */
	function extractOtherDate(element, source, noOther) {
		if (source.newDate && source.extraInfo) { // Already a CDate
			return [source];
		}
		var inst = $.data(element, $.calendars.picker.dataName);
		var calendar = inst.get('calendar');
		var thatDate = null;
		try {
			if (typeof source == 'string' && source != 'today') {
				thatDate = calendar.parseDate(inst.get('dateFormat'), source);
			}
		}
		catch (e) {
			// Ignore
		}
		thatDate = (thatDate ? [thatDate] : (source == 'today' ?
			[calendar.today()] : (noOther ? [] : $(source).calendarsPicker('getDate'))));
		return thatDate;
	}
}

})(jQuery);
